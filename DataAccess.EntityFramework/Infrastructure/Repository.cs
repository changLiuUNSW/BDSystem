using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Infrastructure
{
    /// <summary>
    /// base repository object for DB connection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _dataContext;

        public Repository(IDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            _dataContext = dbContext;
        }

        protected IDbContext DataContext
        {
            get { return _dataContext; }
        }

        protected IDbSet<T> DbSet
        {
            get
            {
                if (DataContext == null)
                    throw new NullReferenceException("No database context found");

                return DataContext.DbSet<T>();
            }
        }

        public virtual void DeAttach(T entity)
        {
            DataContext.DbEntry(entity).State = EntityState.Detached;       
        }

        public virtual void Attach(T entity)
        {
            DbSet.Attach(entity);
        }

        /// <summary>
        /// Insert a single entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbSet.Add(entity);

            return entity;
        }

        public virtual IList<T> Add(IList<T> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException("entities");

            return entities.Select(entity => DbSet.Add(entity)).ToList();
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
                DataContext.DbEntry(entity).State = EntityState.Modified;
            }
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.AnyAsync(predicate);
        }

        public virtual Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefaultAsync(predicate);
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
        /// <returns></returns>
        public virtual void RemoveRange(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }
        }


        /// <summary>
        /// Get total count by predicate
        /// </summary>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).CountAsync();
        }

        public virtual Task<int> CountAsync()
        {
            return DbSet.CountAsync();
        }

        public virtual int Count()
        {
            return DbSet.Count();
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
        public virtual IList<T> Get()
        {
            return DbSet.ToList();
        }

        public virtual Task<List<T>> GetAsync()
        {
            return DbSet.ToListAsync();
        }

        public virtual ObservableCollection<T> Local()
        {
            return DbSet.Local;
        }

        /// <summary>
        /// get evertything from the table
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetTop(int take)
        {
            return DbSet.Take(take).ToList();
        }

        public virtual Task<List<T>> GetTopAsync(int take)
        {
            return DbSet.Take(take).ToListAsync();
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
        ///  select entity base on where condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IList<T> Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }


        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        ///  select entity base on where condition and take top
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public virtual IList<T> Get(Expression<Func<T, bool>> predicate,int top)
        {
            return DbSet.Where(predicate).Take(top).ToList();
        }

        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, int top)
        {
            return DbSet.Where(predicate).Take(top).ToListAsync();
        }



        /// <summary>
        /// save method for individual repository
        /// use only when the repository is used outside of the unit of work
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            return _dataContext.Save();
        }

        public virtual Task<int> SaveAsync()
        {
            return _dataContext.SaveAsync();
        }

        /// <summary>
        /// enable / disable underlying data context proxy creation option
        /// </summary>
        /// <param name="set"></param>
        public virtual void EnableProxyCreation(bool set)
        {
            _dataContext.EnableProxyCreation(set);
        }

        /// <summary>
        /// enable /disable underlying data context lazy load option
        /// </summary>
        /// <param name="set"></param>
        public virtual void EnableLazyLoading(bool set)
        {
            _dataContext.EnableLazyLoading(set);
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
            DataContext.DbEntry(entity).Reload();
        }

        public virtual Task ReloadAsync(T entity)
        {
            return DataContext.DbEntry(entity).ReloadAsync();
        }
    }
}