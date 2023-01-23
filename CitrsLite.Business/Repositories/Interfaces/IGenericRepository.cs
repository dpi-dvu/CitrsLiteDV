using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CitrsLite.Business.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository,
    /// </summary>
    /// <typeparam name="T">Entity Model</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Get a list of models from database.
        /// </summary>
        /// <param name="predicate">The filter condition</param>
        /// <param name="includedProperties">included propertiers</param>
        /// <returns></returns>
        public IList<T> GetList(Expression<Func<T, bool>> predicate = null, 
            params string[] includedProperties);

        /// <summary>
        /// Return a list of models from the database.
        /// </summary>
        /// <param name="predicate">The filter condition</param>
        /// <param name="includedProperties">Included properties</param>
        /// <returns>List of models.</returns>
        public Task<IList<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
            params string[] includedProperties);

        public T GetFirst(Expression<Func<T, bool>> predicate,
            params string[] inludedProperties);

        /// <summary>
        /// Return the First or Default object from a List from the database.
        /// </summary>
        /// <param name="predicate">The filter condition</param>
        /// <param name="includedProperties">Included properties</param>
        /// <returns>First or Defaut item in a List.</returns>
        public Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate,
            params string[] includedProperties);

        /// <summary>
        /// Creates a new Model in the Database.
        /// </summary>
        /// <param name="t">Model being Created</param>
        public void Create(T t);

        public Task CreateAsync(T t);

        /// <summary>
        /// Updates an existing Model.
        /// </summary>
        /// <param name="t">Pre-existing Model in the Database</param>
        public void Edit(T t);

        /// <summary>
        /// Remove Entry from Database.
        /// </summary>
        /// <param name="t">Entry in the Database you want to Remove</param>
        public void Remove(T t);

    }
}
