using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace QDT.Core.MSData
{
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// Get object instance by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetByID<T>(object id) where T : class;

        /// <summary>
        /// Add a new object to data context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        T Add<T>(T t) where T : class;

        /// <summary>
        ///  Mark the object is changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Update<T>(T t) where T : class;

        /// <summary>
        /// Mark the object is delete.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Delete<T>(T t) where T : class;
        
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        void Delete<T, TKey>(TKey key) where T :class ,IEntityable<TKey> ;

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Submit changes to database.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
