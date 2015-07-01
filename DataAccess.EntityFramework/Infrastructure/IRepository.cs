using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework.Infrastructure
{
    public interface IRepository<T>
    {
        ObservableCollection<T> Local();
        IList<T> Get();
        Task<List<T>> GetAsync();
        Task<List<T>> GetTopAsync(int take);
        IList<T> GetTop(int take);
        Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, TResult>> predicate);
        Task<List<TResult>> GetAsync<TResult>(Expression<Func<T, TResult>> predicate, int take);
        Task<List<T>> IncludeAsync(params Expression<Func<T, object>>[] paths);

        Task<List<TResult>> FilterAsync<TResult>(Expression<Func<T, bool>> @where, Expression<Func<T, TResult>> @select,
            params Expression<Func<T, object>>[] includes);

        Task<List<TResult>> FilterAsync<TResult>(Expression<Func<T, bool>> @where, Expression<Func<T, TResult>> @select,
            int take, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate, int top);
        IList<TResult> Get<TResult>(Expression<Func<T, TResult>> predicate);
        IList<TResult> Get<TResult>(Expression<Func<T, TResult>> predicate, int take);
        IList<T> Get(Expression<Func<T, bool>> predicate, int top);
        IList<T> Get(Expression<Func<T, bool>> predicate);
        Task<List<TResult>> DistinctAsync<TResult>(Expression<Func<T, TResult>> predicate);
        IList<TResult> Distinct<TResult>(Expression<Func<T, TResult>> predicate);
        IList<T> Include(params Expression<Func<T, object>>[] paths);

        IList<T> Add(IList<T> entities);

        IList<TResult> Filter<TResult>(Expression<Func<T, bool>> where,
            Expression<Func<T, TResult>> select,
            params Expression<Func<T, object>>[] includes);

        IList<TResult> Filter<TResult>(Expression<Func<T, bool>> where,
            Expression<Func<T, TResult>> select,
            int take,
            params Expression<Func<T, object>>[] includes);

        T Get<TKey>(TKey key);
        T Add(T entity);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        void Update(params T[] entity);
        void Delete<TKey>(params TKey[] key);
        void Delete<TKey1, TKey2>(TKey1 firstKey, TKey2 secondKey);
        void Delete<TKey1, TKey2, TKey3>(TKey1 firstKey, TKey2 secondKey, TKey3 thirdKey);
        void RemoveRange(IList<T> entities);
        void EnableProxyCreation(bool set);
        void EnableLazyLoading(bool set);
        void DeAttach(T entity);
        void Attach(T entity);
        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        int Save();

        void Reload(T entity);
        Task ReloadAsync(T entity);
    }
}