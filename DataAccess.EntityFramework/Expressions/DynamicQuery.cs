using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.Common.Paging;
using DataAccess.Common.SearchModels;

namespace DataAccess.EntityFramework.Expressions
{
    public enum SearchFieldType
    {
        dateTime,
        text,
        list
    }

    /// <summary>
    /// expression binding
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// combine source expression with target expression with OR( || ) operator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> source, Expression<Func<T, bool>> target)
        {
            return source.Compose(target, Expression.Or);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> source,
            Expression<Func<T, bool>> target)
        {
            return source.Compose(target, Expression.And);
        }

        /// <summary>
        /// compose two expressions/predicates with supplied merge option by replace target parameters with source parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        private static Expression<T> Compose<T>(this Expression<T> source, Expression<T> target,
            Func<Expression, Expression, Expression> merge)
        {
            var map = source.Parameters.Select((src, i) => new { src, target = target.Parameters[i] }).ToDictionary(x => x.target, x => x.src);
            var targetBody = ExpressionHelper.ExpressionParamRebinder.ReplaceParameter(map, target.Body);
            return Expression.Lambda<T>(merge(source.Body, targetBody), source.Parameters);
        }
    }

    /// <summary>
    /// extension class to compile dynamic query
    /// </summary>
    internal static class DynamicQuery
    {
        //private const string DateType = "dateTime";
        //private const string BooleanType = "boolean";

        /// <summary>
        /// return query result that filter each column base on the provided column field/value and logic operator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static IQueryable<T> SearchByFields<T>(this IQueryable<T> source, Search search) where T : class
        {
            if (search.ConditionalOr)
                return source.SearchByFields(search.SearchFields, true);

            return source.SearchByFields(search.SearchFields);
        }

        /// <summary>
        /// return query result that filter each column base on the provided column field and value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="source"></param>
        /// <param name="conditionalOr">bool for and/or field search</param>
        /// <returns></returns>
        public static IQueryable<T> SearchByFields<T>(this IQueryable<T> source, IEnumerable<SearchField> fields, bool conditionalOr = false) where T : class
        {
            var param = Expression.Parameter(typeof(T));
            var expression = BuildSearchExpression(fields.GetEnumerator(), ref param, conditionalOr);
            if (expression == null)
                return source;

            var predicate = Expression.Lambda<Func<T, bool>>(expression, new[] { param });
            return source.Where(predicate);
        }

        /// <summary>
        /// project T into TResult using LINQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<TResult> Project<T, TResult>(this IQueryable<T> source) where T: class
                                                                                   where TResult: class
        {
            return source.Select(BuildProjectExpression<T, TResult>());
        }

        /// <summary>
        /// materialize the query result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="search"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async static Task<SearchResult<T>> CompileSearchReslt<T>(this IQueryable<T> source, Search search)
        {
            //count() has to be done before hand
            //.net does not support multi thread async access on the same context
            var count = await source.CountAsync();
            source = source.OrderByField(search.SortColumn, search.Order != "desc");

            return new SearchResult<T>
            {
                Total = count,
                List = await source.Skip(PagingHelper.NumSkips(search.CurrentPage, search.PageSize))
                    .Take(search.PageSize).ToListAsync()
            };
        }

        /// <summary>
        /// order query by provided field name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderField"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> source, string orderField, bool ascending)
        {
            var param = Expression.Parameter(typeof(T));
            var prop = Expression.Property(param, orderField);
            var exp = Expression.Lambda(prop, param);
            var method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = { source.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);

            return source.Provider.CreateQuery<T>(mce);
        }

        /// <summary>
        /// build dynamic predicate base on the column fields provided
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="param"></param>
        /// <param name="conditionalOr">bool for AND/OR expression</param>
        /// <returns></returns>
        private static Expression BuildSearchExpression(IEnumerator<SearchField> fields, ref ParameterExpression param, bool conditionalOr = false)
        {
            Expression expression = null;

            while (true)
            {
                if (!fields.MoveNext())
                    return expression;

                if (fields.Current == null || string.IsNullOrEmpty(fields.Current.Term))
                    continue;

                var property = Expression.Property(param, fields.Current.Field);
                Expression newExpression = null;

                //special case where the type is date, the matching string has two dates(start, end), we need to split them
                if (fields.Current.Term.IndexOf('-') >= 0 && fields.Current.Type == SearchFieldType.dateTime.ToString())
                {
                    DateTime startDate;
                    DateTime endDate;
                    if ( !DateTime.TryParse(fields.Current.Term.Split('-')[0], out startDate) ||
                         !DateTime.TryParse(fields.Current.Term.Split('-')[1], out endDate))
                        continue;

                    var startDateConstant = Expression.Constant(startDate);
                    var endDateConstant = Expression.Constant(endDate);
                    var startDateExpression = Expression.GreaterThanOrEqual(property, Expression.Convert(startDateConstant, property.Type));
                    var endDateExpression = Expression.LessThanOrEqual(property, Expression.Convert(endDateConstant, property.Type));
                    newExpression = Expression.And(startDateExpression, endDateExpression);
                }
                //all other types fall in here
                else
                {
                    ConstantExpression constant;
                    MethodInfo method;

                    SearchFieldType searchType;
                    if (!Enum.TryParse(fields.Current.Type, out searchType))
                        continue;

                    switch (searchType)
                    {
                        case SearchFieldType.dateTime:
                            constant = Expression.Constant(ToBoolean(fields.Current.Term));
                            newExpression = Expression.Equal(Expression.Convert(constant, property.Type), property);
                            break;

                        case SearchFieldType.list:
                            var values = fields.Current.Term.Split(',');

                            if (!property.Type.IsGenericType || property.Type.GetGenericTypeDefinition() != typeof (IEnumerable<>))
                                continue;

                            method =
                                typeof (Enumerable).GetMethods()
                                    .SingleOrDefault(x => x.Name == "Contains" && x.GetParameters().Length == 2);
                                
                            if (method == null)
                                continue;

                            foreach (var value in values)
                            {
                                constant = Expression.Constant(value);
                                var expr = Expression.Call(method.MakeGenericMethod(constant.Type), property, constant);

                                if (newExpression == null)
                                    newExpression = expr;
                                else
                                    newExpression = Expression.And(newExpression, expr);
                            }
                            break;
                        default:
                            constant = Expression.Constant(fields.Current.Term);
                            method = property.Type.GetMethod("Contains", new[] {property.Type});
                            //method = property.Type.GetMethod("StartsWith", new[] {property.Type});
                            newExpression = Expression.Call(property, method, Expression.Convert(constant, property.Type));
                            break;
                    }
                }

                if (expression == null && newExpression != null)
                    expression = newExpression;
                else if (expression != null && newExpression != null)
                {
                    expression = conditionalOr ? Expression.Or(expression, newExpression) : Expression.And(expression, newExpression);
                }
            }
        }

        /// <summary>
        /// projection is built base on source member name and type to destination member name and type
        /// its does not support deep recursive projection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        private static Expression<Func<T, TResult>> BuildProjectExpression<T, TResult>() where T: class 
                                                                                         where TResult: class
        {
            var srcMembers = typeof(T).GetProperties();
            var destMembers = typeof(TResult).GetProperties();
            var matchingMembers = srcMembers.Select(x => new
            {
                src = x,
                dest = destMembers.FirstOrDefault(y => y.Name == x.Name && y.PropertyType == x.PropertyType)
            }).Where(x => x.dest != null);
            
            const string name = "src";
            var parameterExpression = Expression.Parameter(typeof(T), name);
            var newExpression = Expression.MemberInit(
                Expression.New(typeof (TResult)),
                matchingMembers.Select(x => Expression.Bind(x.dest, Expression.Property(parameterExpression, x.src)))
                );

            return Expression.Lambda<Func<T, TResult>>(newExpression, parameterExpression);
        }

        private static bool ToBoolean(string val)
        {
            if (val == "1")
                return true;

            return false;
        }
    }
}
