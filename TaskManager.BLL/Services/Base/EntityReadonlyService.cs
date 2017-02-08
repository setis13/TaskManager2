using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Dtos.Base;
using TaskManager.BLL.Contracts.Services.Base;
using TaskManager.DAL.Contracts;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.BLL.Services.Base {
    /// <summary>
    ///     Base service for Read queries </summary>
    /// <typeparam name="TService">Interface Type of service</typeparam>
    /// <typeparam name="TDto">DTO</typeparam>
    /// <typeparam name="TEntity">Model</typeparam>
    public class EntityReadonlyService<TService, TDto, TEntity> : HostService<TService>, IEntityReadonlyService<TDto>
        where TService : IService
        where TEntity: BaseEntity
        where TDto : BaseDto {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityReadonlyService" /> class. </summary>
        public EntityReadonlyService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     The model get by id. </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="TDto" />.</returns>
        public virtual TDto GetById(object id) {
            var store = this.UnitOfWork.GetEntityRepository<TEntity>().GetById(id);
            return this.Mapper.Map<TDto>(store);
        }

        /// <summary>
        ///     Get all models list </summary>
        /// <returns>Sessions</returns>
        public virtual List<TDto> GetAll() {
            var store = this.UnitOfWork.GetEntityRepository<TEntity>().GetAll();
            return this.Mapper.Map<List<TDto>>(store);
        }

        /// <summary>
        ///     Searches entities with predicate </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>List of entities</returns>
        public virtual List<TDto> SearchFor<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity {
            var store = this.UnitOfWork.GetEntityRepository<TEntity>().SearchFor(predicate).ToList();
            return this.Mapper.Map<List<TDto>>(store);
        }
    }
}