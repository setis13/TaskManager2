using AutoMapper;
using TaskManager.Data;

namespace TaskManager.Logic.Services {
    /// <summary>
    ///     Base services abstract class </summary>
    public abstract class HostService : IService {

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
        }
    }
}