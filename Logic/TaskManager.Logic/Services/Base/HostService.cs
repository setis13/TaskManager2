using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Services.Base {
    /// <summary>
    ///     Base services abstract class </summary>
    public abstract class HostService<T> : IService where T : IService {
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
        ///     .ctor </summary>
        protected HostService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) {
            this.UnitOfWork = unitOfWork;
            this.Mapper = mapper;
            this.ServicesHost = servicesHost;
            servicesHost.Register((T)(this as IService));
        }
    }
}