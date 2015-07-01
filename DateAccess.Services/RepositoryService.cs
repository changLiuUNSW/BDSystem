using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Infrastructure;

namespace DateAccess.Services
{
    public interface IRepositoryService<T> where T : class
    {
        IList<T> Get(bool disableProxy = false);
        T GetByKey<TKey>(TKey key, bool disableProxy = false);
        IList<T> GetWithInclude(Expression<Func<T, object>>[] paths, bool disableProxy = false);
        T Add(T item);
        bool Any(Expression<Func<T, bool>> predicate);
        int Delete<TKey>(TKey id);
        int Delete<TKey1, TKey2>(TKey1 keyOne, TKey2 keyTwo);
        int Update(T item);
        int Update(IList<T> items);
        void Reload(T entity);
    }

    /// <summary>
    /// provide database access capability to derived service class 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryService<T> where T: class 
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<T> _repository;
        protected delegate IList<T> DelegateGet();
        protected delegate T DelegateGet<in TKey>(TKey key);
        protected delegate IList<T> DelegateInclude(params Expression<Func<T, object>>[] paths);

        protected RepositoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        protected int Save()
        {
            return _unitOfWork.Save();
        }

        protected IList<T> GetWithoutProxy(DelegateGet method)
        {
            UnitOfWork.EnableProxyCreation(false);
            var result = method();
            UnitOfWork.EnableProxyCreation(true);
            return result;
        }

        protected T GetWithoutProxy<TKey>(DelegateGet<TKey> method, TKey key)
        {
            _unitOfWork.EnableProxyCreation(false);
            var result = method(key);
            _unitOfWork.EnableProxyCreation(true);
            return result;
        }

        protected IList<T> GetWithoutProxy(DelegateInclude method, Expression<Func<T, object>>[] paths)
        {
            UnitOfWork.EnableProxyCreation(false);
            var result = method(paths);
            UnitOfWork.EnableProxyCreation(true);
            return result;
        }

        protected IRepository<T> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = _unitOfWork.GetRepository<T>();
                    return _repository;
                }

                return _repository;
            }
        }

        /// <summary>
        /// default get function
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> Get(bool disableProxy = false)
        {
            switch (disableProxy)
            {
                case true:
                    DelegateGet method = Repository.Get;
                    return GetWithoutProxy(method);
                default:
                    return Repository.Get();
            }
        } 

        public virtual IList<TResult> Distinct<TResult>(Expression<Func<T, TResult>> predicate)
        {
            return Repository.Distinct(predicate);
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return Repository.Any(predicate);
        }

        public virtual T GetByKey<TKey>(TKey key, bool disableProxy = false)
        {
            switch (disableProxy)
            {
                case true:
                    DelegateGet<TKey> method = Repository.Get<TKey>;
                    return GetWithoutProxy(method, key);
                default:
                    return Repository.Get(key);
            }
        }

        public virtual IList<T> GetWithInclude(Expression<Func<T, object>>[] paths, bool disableProxy = false)
        {
            switch (disableProxy)
            {
                case true:
                    DelegateInclude method = Repository.Include;
                    return GetWithoutProxy(method, paths);
                default:
                    return Repository.Include(paths);
            }
        }

        /// <summary>
        /// default add function
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual T Add(T item)
        {
            Repository.Add(item);
            Save();
            return item;
        }

        /// <summary>
        /// default update function
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int Update(T item)
        {
            Repository.Update(item);
            return Save();
        }

        public virtual int Update(IList<T> items)
        {
            Repository.Update(items.ToArray());
            return Save();
        }

        /// <summary>
        /// default delete function
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int Delete<TKey>(TKey key)
        {
            Repository.Delete(key);
            return Save();
        }
        
        /// <summary>
        /// delete function for composite key
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="keyOne"></param>
        /// <param name="keyTwo"></param>
        /// <returns></returns>
        public virtual int Delete<TKey1, TKey2>(TKey1 keyOne, TKey2 keyTwo)
        {
            Repository.Delete(keyOne, keyTwo);
            return Save();
        }

        public virtual void Reload(T entity)
        {
            Repository.Reload(entity);
        }
    }
}
