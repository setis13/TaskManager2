using System;
using System.Linq;
using System.Linq.Expressions;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Repositories.Base {

    /// <summary>
    /// Base repository interface
    /// </summary>
    public interface IEntityRepository { };

    /// <summary>
    ///     Base entity repository interface </summary>
    /// <typeparam name="T">BaseEntity</typeparam>
    public interface IEntityRepository<T> : IEntityRepository where T : BaseEntity {
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
        void Insert(T entity);
        /// <summary>
        ///     Updates existing entity </summary>
        /// <param name="entity">Entity instance</param>
        void Update(T entity);
        /// <summary>
        ///     Deletes existing entity </summary>
        /// <param name="entity">Entity instance</param>
        void Delete(T entity);
    }
}
