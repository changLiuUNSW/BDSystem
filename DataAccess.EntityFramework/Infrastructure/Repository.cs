using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Common.SearchModels;
using DataAccess.EntityFramework.Expressions;
using DataAccess.EntityFramework.Extensions;

namespace DataAccess.EntityFramework.Infrastructure
{
    /// <summary>
    /// base repository object for DB connection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dataContext;

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            _dataContext = dbContext;
        }

        protected DbContext DataContext
        {
            get { return _dataContext; }
        }

        protected DbSet<T> DbSet
        {
            get
            {
                if (DataContext == null)
                    throw new NullReferenceException("No database context found");

                return DataContext.Set<T>();
            }
        }

        public virtual void DeAttach(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Detached;       
        }

        public virtual void Attach(T entity)
        {
            DbSet.Attach(entity);
        }

        /// <summary>
        /// Insert a single entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual void Add(params T[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                DbSet.Add(entity);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual void Update(params T[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                DbSet.Attach(entity);
                DataContext.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Remove entities
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual void Delete<TKey>(params TKey[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            foreach (var key in keys)
            {
                var delObj = DbSet.Find(key);

                if (delObj != null)
                    DbSet.Remove(delObj);
            }
        }


        public virtual void Delete<TKey1, TKey2>(TKey1 firstKey, TKey2 secondKey)
        {
            var delObj = DbSet.Find(firstKey, secondKey);

            if (delObj != null)
                DbSet.Remove(delObj);
        }

        public virtual void Delete<TKey1, TKey2, TKey3>(TKey1 firstKey, TKey2 secondKey, TKey3 thirdKey)
        {
            var delObj = DbSet.Find(firstKey, secondKey, thirdKey);

            if (delObj != null)
                DbSet.Remove(delObj);
        }

        /// <summary>
        /// Batch Remove entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void RemoveRange(params T[] entities)
        {
            DbSet.RemoveRange(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public virtual int Count(params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = query.AggregatePredicates(predicates);

            return query.Count();
        }

        public virtual Task<int> CountAsync(params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = query.AggregatePredicates(predicates);

            return query.CountAsync();
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths)
        {
            var query = DbSet.AsQueryable();

            if (paths.Length > 0)
            {
                query = paths.Aggregate(DbSet.AsQueryable(), (queryable, path) => queryable.Include(path));
            }

            return query.SingleOrDefault(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths)
        {
            var query = DbSet.AsQueryable();

            if (paths.Length > 0)
            {
                query = paths.Aggregate(DbSet.AsQueryable(), (queryable, path) => queryable.Include(path));
            }
            return query.FirstOrDefault(predicate);
        }

        public virtual Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefaultAsync(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// select entity using primary key
        /// this function uses dbset.find() thus it first return entity from data context then the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<TKey>(TKey key)
        {
            return DbSet.Find(key);
        }

        /// <summary>
        /// get evertything from the table
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> Get(params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = query.AggregatePredicates(predicates);

            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public virtual Task<List<T>> GetAsync(params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = DbSet.AggregatePredicates(predicates);

            return query.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetTop(int? take, params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = DbSet.AggregatePredicates(predicates);
            if(take!=null)return query.Take(take.Value).ToList();
            return query.ToList();
        }

        public virtual Task<List<T>> GetTopAsync(int? take, params Expression<Func<T, bool>>[] predicates)
        {
            var query = DbSet.AsQueryable();

            if (predicates.Length > 0)
                query = DbSet.AggregatePredicates(predicates);
            if(take!=null)return query.Take(take.Value).ToListAsync();
            return query.ToListAsync();
        }

        public virtual IList<TResult> Distinct<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return DbSet.Select(predicate).Distinct().ToList();
        }

        public virtual Task<List<TResult>> DistinctAsync<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return DbSet.Select(predicate).Distinct().ToListAsync();
        } 

        /// <summary>
        /// custom select on entity property
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IList<TResult> Get<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return DbSet.Select(predicate).ToList();
        }

        public virtual Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return DbSet.Select(predicate).ToListAsync();
        }

        public virtual Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, TResult>> predicate, int take)
        {
            return DbSet.Select(predicate).Take(take).ToListAsync();
        }

        public virtual IList<TResult> Get<TResult>(Expression<Func<T, TResult>> predicate, int take)
        {
            return DbSet.Select(predicate).Take(take).ToList();
        }

        /// <summary>
        /// save method for individual repository
        /// use only when the repository is used outside of the unit of work
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            return _dataContext.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// enable / disable underlying data context proxy creation option
        /// </summary>
        /// <param name="set"></param>
        public virtual void EnableProxyCreation(bool set)
        {
            _dataContext.Configuration.ProxyCreationEnabled = set;
        }

        /// <summary>
        /// enable /disable underlying data context lazy load option
        /// </summary>
        /// <param name="set"></param>
        public virtual void EnableLazyLoading(bool set)
        {
            _dataContext.Configuration.LazyLoadingEnabled = set;
        }

        /// <summary>
        /// select entities including its sub tables 
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public virtual IList<T> Include(params Expression<Func<T, object>>[] paths)
        {
            if (paths == null)
                return DbSet.ToList();

            return paths.Aggregate(DbSet.AsQueryable(), (query, path) => query.Include(path)).ToList();
        }


        public virtual Task<List<T>> IncludeAsync(params Expression<Func<T, object>>[] paths)
        {
            if (paths == null)
                return DbSet.ToListAsync();

            return paths.Aggregate(DbSet.AsQueryable(), (query, path) => query.Include(path)).ToListAsync();
        }

        /// <summary>
        /// return query after combine where / select and include clause
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="select"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IList<TResult> Filter<TResult>(Expression<Func<T, bool>> where,
                                                Expression<Func<T, TResult>> select,
                                                params Expression<Func<T, object>>[] includes)
        {
            var dbset = DbSet.AsQueryable();
            if (includes != null) dbset = includes.Aggregate(dbset, (query, path) => query.Include(path));
            return dbset.Where(where).Select(select).ToList();
        }


        public virtual Task<List<TResult>> FilterAsync<TResult>(Expression<Func<T, bool>> @where, Expression<Func<T, TResult>> @select, params Expression<Func<T, object>>[] includes)
        {
            var dbset = includes.Aggregate(DbSet.AsQueryable(), (query, path) => query.Include(path));
            return dbset.Where(where).Select(select).ToListAsync();
        }

        public virtual IList<TResult> Filter<TResult>(Expression<Func<T, bool>> where,
                                        Expression<Func<T, TResult>> select,
                                        int take,
                                        params Expression<Func<T, object>>[] includes)
        {
            var dbset = includes.Aggregate(DbSet.AsQueryable(), (query, path) => query.Include(path));
            return dbset.Where(where).Select(select).Take(take).ToList();
        }

        public virtual Task<List<TResult>> FilterAsync<TResult>(Expression<Func<T, bool>> @where, Expression<Func<T, TResult>> @select, int take, params Expression<Func<T, object>>[] includes)
        {
            var dbset = includes.Aggregate(DbSet.AsQueryable(), (query, path) => query.Include(path));
            return dbset.Where(where).Select(select).Take(take).ToListAsync();
        }

        public virtual void Reload(T entity)
        {
            DataContext.Entry(entity).Reload();
        }

        public virtual Task ReloadAsync(T entity)
        {
            return DataContext.Entry(entity).ReloadAsync();
        }

        /// <summary>
        /// search with where predicates
        /// </summary>
        /// <param name="search"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public virtual Task<SearchResult<T>> SearchAsync(Search search, params Expression<Func<T, bool>>[] predicates)
        {
            return DbSet.AggregatePredicates(predicates)
                .SearchByFields(search)
                .CompileSearchReslt(search);
        }

        /// <summary>
        /// search and return projected type
        /// </summary>
        /// <typeparam name="TResult">typeof TResult</typeparam>
        /// <param name="search"></param>
        /// <param name="select"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public virtual Task<SearchResult<TResult>> SearchAsync<TResult>(Search search,
            Expression<Func<T, TResult>> select,
            params Expression<Func<T, bool>>[] predicates) where TResult: class
        {
            return
                DbSet.AggregatePredicates(predicates)
                    .Select(select)
                    .SearchByFields(search)
                    .CompileSearchReslt(search);
        }

        /// <summary>
        /// This method offers an alternative way of projecting the query into destination. 
        /// It compiles a new object expression from the given type
        /// only use for simple 1-1 mapping, deep recursive query is not suppported
        /// destination property name must match source property name        /// 
        /// </summary>
        /// <typeparam name="TResult">projection type</typeparam>
        /// <param name="search"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public virtual Task<SearchResult<TResult>> SearchAsync<TResult>(Search search, params Expression<Func<T, bool>>[] predicates)
            where TResult : class
        {
            return DbSet.AggregatePredicates(predicates)
                .Project<T, TResult>()
                .SearchByFields(search)
                .CompileSearchReslt(search);
        }
    }
}