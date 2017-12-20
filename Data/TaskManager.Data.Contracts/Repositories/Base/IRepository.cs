using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Repositories.Base {

    /// <summary>
    /// Base repository interface
    /// </summary>
    public interface IRepository { };

    /// <summary>
    ///     Base entity repository interface </summary>
    /// <typeparam name="T">BaseEntity</typeparam>
    public interface IRepository<T> : IRepository where T : BaseEntity {
        /// <summary>
        ///     Gets entity by PK Id </summary>
        /// <param name="id">Id</param>
        /// <returns>Entity instance</returns>
        T GetById(Guid id);
        /// <summary>
        ///     Searchs entities using predicate expression </summary>
        /// <param name="predicate">Predicate expression</param>
        /// <returns>Collection of entities</returns>
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        /// <summary>
        ///     Gets all entities </summary>
        /// <returns>Collection of entities</returns>
        IQueryable<T> GetAll();
        /// <summary>
        ///     Inserts a new entity </summary>
        /// <param name="entity">Entity instance</param>
        /// <param name="userId">User ID</param>
        void Insert(T entity, Guid userId);
        /// <summary>
        ///     Updates existing entity </summary>
        /// <param name="entity">Entity instance</param>
        /// <param name="userId">User ID</param>
        void Update(T entity, Guid userId);

        /// <summary>
        ///     Deletes existing entity by its id </summary>
        /// <param name="id">Entity Id</param>
        void DeleteById(Guid id);
        /// <summary>
        ///     Deletes existing entity by its ids </summary>
        /// <param name="ids">List of Entity Ids</param>
        void DeleteByIds(List<Guid> ids);
    }
}