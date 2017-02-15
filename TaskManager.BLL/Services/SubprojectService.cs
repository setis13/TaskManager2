using AutoMapper;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Dtos;
using TaskManager.BLL.Contracts.Services;
using TaskManager.BLL.Services.Base;
using TaskManager.DAL.Contracts;
using TaskManager.DAL.Contracts.Entities;

namespace TaskManager.BLL.Services {
    public class SubprojectService : EntityCrudService<ISubprojectService, SubprojectDto, Subproject>, ISubprojectService {
        public SubprojectService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper) : base(servicesHost, unitOfWork, mapper) {
        }
    }
}
