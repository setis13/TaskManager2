using System;
using System.Collections.Generic;
using System.Data.Entity;
using AutoMapper;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Dtos.Base;
using TaskManager.BLL.Contracts.Services.Base;
using TaskManager.DAL.Contracts;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.BLL.Services.Base {
    /// <summary>
    ///     Base service for CRUD queries </summary>
    /// <typeparam name="TService">Interface Type of service</typeparam>
    /// <typeparam name="TDto">DTO</typeparam>
    /// <typeparam name="TEntity">Model</typeparam>
    public class EntityCrudService<TService, TDto, TEntity> : EntityReadonlyService<TService, TDto, TEntity>, ICrudService<TDto>
        where TDto : BaseDto
        where TEntity : BaseEntity 
        where TService : IService {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityCrudService" /> class. </summary>
        public EntityCrudService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Save model to database </summary>
        /// <param name="dto">The model</param>
        public virtual void Save(TDto dto) {
            var model = this.UnitOfWork.GetEntityRepository<TEntity>().GetById(dto.EntityId);

            if (model == null) {
                model = this.Mapper.Map<TEntity>(dto);
                this.UnitOfWork.GetEntityRepository<TEntity>().Insert(model);
                if (model.EntityId == Guid.Empty) {
                    dto.EntityId = model.EntityId = Guid.NewGuid();
                }
                model.CreatedDate = model.LastModifiedDate = DateTime.UtcNow;
            } else {
                var newModel = this.Mapper.Map<TEntity>(dto);
                newModel.CreatedDate = model.CreatedDate;
                newModel.LastModifiedDate = DateTime.UtcNow;
                this.UnitOfWork.Context.DbContext.Entry(model).State = EntityState.Detached;
                this.UnitOfWork.GetEntityRepository<TEntity>().Update(newModel);
            }

            this.UnitOfWork.SaveChanges();
        }
        
        /// <summary>
        ///     Delete model </summary>
        /// <param name="model">Model</param>
        public void MarkAsDeleted(TDto model) {
            this.UnitOfWork.GetEntityRepository<TEntity>().MarkAsDeletedId(model.EntityId);
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Delete mnodel by Id </summary>
        /// <param name="modelId">Model id</param>
        public void MarkAsDeletedId(Guid modelId) {
            this.UnitOfWork.GetEntityRepository<TEntity>().MarkAsDeletedId(modelId);
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Delete mnodel by Ids </summary>
        /// <param name="ids">Model ids</param>
        public void MarkAsDeletedIds(List<Guid> ids) {
            this.UnitOfWork.GetEntityRepository<TEntity>().MarkAsDeletedIds(ids);
            this.UnitOfWork.SaveChanges();
        }
    }
}