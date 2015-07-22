using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.EntityFramework.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<T> AggregatePredicates<T>(this IQueryable<T> source,
            Expression<Func<T, bool>>[] predicates)
        {
            if (!predicates.Any())
                return source;

            return predicates.Aggregate(source, (query, predicate) => query.Where(predicate));
        }
    }
}
