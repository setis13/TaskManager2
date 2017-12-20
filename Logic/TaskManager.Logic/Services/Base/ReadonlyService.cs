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
    public class ReadonlyService<TDto, TEntity> : IReadonlyService<TDto>
        where TDto : BaseDto
        where TEntity : BaseEntity {

        /// <summary>
        ///     The unit of work </summary>
        protected readonly IUnitOfWork UnitOfWork;
        /// <summary>
        ///     The AutoMapper </summary>
        protected readonly IMapper Mapper;
        /// <summary>
        ///     The Service Host </summary>
        protected readonly IServicesHost ServicesHost;

        /// <summary>
        ///     Initializes a new instance of the class. </summary>
        public ReadonlyService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) {
            this.ServicesHost = servicesHost;
            this.UnitOfWork = unitOfWork;
            this.Mapper = mapper;
        }

        /// <summary>
        ///     Gets all DTOs list </summary>
        /// <returns>DTOs list</returns>
        public List<TDto> GetAll() {
            var store = this.UnitOfWork.GetRepository<TEntity>().GetAll();
            return Mapper.Map<List<TDto>>(store);
        }

        /// <summary>
        ///     Gets DTO by ID </summary>
        /// <param name="id">DTO ID</param>
        /// <returns>Dto</returns>
        public TDto GetById(Guid id) {
            var store = this.UnitOfWork.GetRepository<TEntity>().GetById(id);
            return Mapper.Map<TDto>(store);
        }
    }
}