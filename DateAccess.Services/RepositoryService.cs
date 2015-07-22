using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.EntityFramework;
using DataAccess.EntityFramework.Infrastructure;
using Newtonsoft.Json.Linq;

namespace DateAccess.Services
{
    public interface IRepositoryService<T> where T : class
    {
        IList<T> Get();
        T GetByKey<TKey>(TKey key);
        T Add(T item);
        int Delete<TKey>(TKey id);
        int Delete<TKey1, TKey2>(TKey1 keyOne, TKey2 keyTwo);
        int Update(T item);
        IRepository<T> Repository { get; }
        IUnitOfWork UnitOfWork { get; }
    }

    /// <summary>
    /// provide database access capability to derived service class 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryService<T> where T: class 
    {
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<T> _repository;

        protected RepositoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected int Save()
        {
            return _unitOfWork.Save();
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public IRepository<T> Repository
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
        public virtual IList<T> Get()
        {
            return Repository.Get();
        }

        public virtual T GetByKey<TKey>(TKey key)
        {
            return Repository.Get(key);
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

        /// <summary>
        /// update database from json
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="id">id property name</param>
        /// <param name="source">json src</param>
        public void UpdateFromJson<TDestination>(string id, JObject source) where TDestination : class
        {
            TDestination dest;
            var jsonKey = source[id];
            
            if (jsonKey.Type == JTokenType.String)
                dest = UnitOfWork.GetRepository<TDestination>().Get((string)source[id]);
            else
                dest = UnitOfWork.GetRepository<TDestination>().Get((int) source[id]);

            var properties = source.Children<JProperty>();
            var type = typeof (TDestination);
            foreach (var property in properties)
            {
                if (property.Name == id || 
                    property.Value.Type == JTokenType.Array || 
                    property.Value.Type == JTokenType.Object)
                    continue;

                var propertyType = type.GetProperty(property.Name);
                if (propertyType != null)
                    propertyType.SetValue(dest, Convert.ChangeType(property.Value, propertyType.PropertyType));
            }
        }
    }
}
