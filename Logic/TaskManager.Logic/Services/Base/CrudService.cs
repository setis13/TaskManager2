using System;
using System.Collections.Generic;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities.Base;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos.Base;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Services.Base {
    /// <summary>
    ///     Base service for CRUD queries </summary>
    /// <typeparam name="TDto">DTO</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    public class CrudService<TDto, TEntity> : ReadonlyService<TDto, TEntity>, ICrudService<TDto>
        where TDto : BaseDto
        where TEntity : BaseEntity {

        /// <summary>
        ///     Initializes a new instance of the class. </summary>
        public CrudService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Save entity to database </summary>
        /// <param name="dto">The DTO</param>
        public void Save(TDto dto) {
            var store = this.UnitOfWork.GetRepository<TEntity>().GetById(dto.EntityId);
            if (store == null) {
                store = Mapper.Map<TEntity>(dto);
                this.UnitOfWork.GetRepository<TEntity>().Insert(store);
            } else {
                Mapper.Map(dto, store);
                this.UnitOfWork.GetRepository<TEntity>().Update(store);
            }
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Delete entity from database </summary>
        /// <param name="dto">The DTO</param>
        public void Delete(TDto dto) {
            this.UnitOfWork.GetRepository<TEntity>().DeleteById(dto.EntityId);
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Delete entiy from database by Id </summary>
        /// <param name="id">The DTO Id</param>
        public void DeleteById(Guid id) {
            this.UnitOfWork.GetRepository<TEntity>().DeleteById(id);
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Delete entities from database by ids </summary>
        /// <param name="ids">Entity ids</param>
        public void DeleteByIds(List<Guid> ids) {
            this.UnitOfWork.GetRepository<TEntity>().DeleteByIds(ids);
            this.UnitOfWork.SaveChanges();
        }
    }
}