using System;
using System.Linq;
using System.Linq.Expressions;

namespace QDT.Core.MSData
{
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count();

        /// <summary>
        /// Save all changes.
        /// </summary>
        /// <returns></returns>
        int Submit();
    }

    public interface IRepository<T> : IRepository
        where T : class
    {
        /// <summary>
        /// Get all objects 
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// Get objects by filter expression.
        /// </summary>
        /// <param name="predicate">The filter expressions</param>
        /// <returns>Returns the IQueryable objects for this filter.</returns>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get objects by filtering and paging specifications.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="sortingSelector">The default sorting field selector</param>
        /// <param name="filter">The filter expressions</param>
        /// <param name="total"></param>
        /// <param name="sortby"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IQueryable<T> Filter<TKey>(Expression<Func<T, TKey>> sortingSelector, Expression<Func<T, bool>> filter, out int total, SortingOrders sortby = SortingOrders.Asc, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        bool Contains(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get the total by predicate expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        T Create(T t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param> 
        void Delete(T t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        int Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        T Update(T t);


    }
}
