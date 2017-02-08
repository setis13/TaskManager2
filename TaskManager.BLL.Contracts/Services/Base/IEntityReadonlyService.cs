using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TaskManager.BLL.Contracts.Dtos.Base;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.BLL.Contracts.Services.Base {
    /// <summary>
    ///     Readonly service contract </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityReadonlyService<T> : IService
        where T : BaseDto {
        /// <summary>
        ///     Get all models list </summary>
        /// <returns>Models list</returns>
        List<T> GetAll();

        /// <summary>
        ///     Get model by ID </summary>
        /// <param name="id">Model ID</param>
        /// <returns>Model</returns>
        T GetById(object id);

        /// <summary>
        /// Searches entities with predicate </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns>Models list</returns>
        List<T> SearchFor<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
    }
}