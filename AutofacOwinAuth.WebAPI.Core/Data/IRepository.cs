using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.WebAPI.Core.Domain;

namespace AutofacOwinAuth.WebAPI.Core.Data
{
    /// <summary>
    /// From NopCommerce
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

    }
}
