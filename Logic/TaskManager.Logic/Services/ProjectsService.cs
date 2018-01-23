using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class ProjectsService : HostService<IProjectsService>, IProjectsService {
        public ProjectsService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Returns projects </summary>
        /// <param name="userDto">user who requests the projects</param>
        /// <returns>List of projects</returns>
        public List<ProjectDto> GetData(UserDto userDto) {
            var models = UnitOfWork.GetRepository<Project>().SearchFor(e => e.CompanyId == userDto.CompanyId);
            return Mapper.Map<List<ProjectDto>>(models);
        }

        /// <summary>
        ///     Creates or Updates project </summary>
        /// <param name="projectDto">project DTO</param>
        /// <param name="userDto">user who updates the project</param>
        public void Save(ProjectDto projectDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Project>();
            var model = rep.GetById(projectDto.EntityId);
            if (model == null) {
                projectDto.CompanyId = userDto.CompanyId;
                model = this.Mapper.Map<Project>(projectDto);
                this.UnitOfWork.GetRepository<Project>().Insert(model, userDto.Id);
            } else {
                projectDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(projectDto, model);
                this.UnitOfWork.GetRepository<Project>().Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Deletes project by id </summary>
        /// <param name="projectId">project id</param>
        /// <param name="userDto">user who deletes the project</param>
        public void Delete(Guid projectId, UserDto userDto) {
            if (userDto.CompanyId == Guid.Empty) {
                throw new Exception("Please create a company");
            }
            var rep = UnitOfWork.GetRepository<Project>();
            var model = rep.GetById(projectId);
            if (model != null && model.CompanyId == userDto.CompanyId) {
                this.UnitOfWork.GetRepository<Project>().MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
