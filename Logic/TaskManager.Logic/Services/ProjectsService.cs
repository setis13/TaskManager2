using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Data.Contracts.Repositories.Base;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class ProjectsService : HostService<IProjectsService>, IProjectsService {

        private IRepository<Project> _rep;

        public ProjectsService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
            _rep = UnitOfWork.GetRepository<Project>();
        }

        /// <summary>
        ///     Returns projects </summary>
        /// <param name="userDto">user who requests the projects</param>
        /// <returns>List of projects</returns>
        public List<ProjectDto> GetData(UserDto userDto) {
            var counts = UnitOfWork.GetRepository<Task1>()
                .SearchFor(e => e.CompanyId == userDto.CompanyId)
                .GroupBy(e => e.ProjectId)
                .ToDictionary(group => group.Key, group => group.Count());
            var models = _rep.SearchFor(e => e.CompanyId == userDto.CompanyId).ToList();
            var changed = false;
            foreach (Project model in models) {
                if (counts.ContainsKey(model.EntityId) && model.Count != counts[model.EntityId]) {
                    model.Count = counts[model.EntityId];
                    _rep.Update(model, userDto.Id);
                    changed = true;
                }
            }
            if (changed) {
                UnitOfWork.SaveChanges();
            }
            return Mapper.Map<List<ProjectDto>>(models.OrderByDescending(e => e.Count));
        }

        /// <summary>
        ///     Creates or Updates project </summary>
        /// <param name="projectDto">project DTO</param>
        /// <param name="userDto">user who updates the project</param>
        public void Save(ProjectDto projectDto, UserDto userDto) {
            var model = _rep.GetById(projectDto.EntityId);
            if (model == null) {
                projectDto.CompanyId = userDto.CompanyId;
                model = this.Mapper.Map<Project>(projectDto);
                _rep.Insert(model, userDto.Id);
            } else {
                projectDto.CompanyId = userDto.CompanyId;
                this.Mapper.Map(projectDto, model);
                _rep.Update(model, userDto.Id);
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
            var model = _rep.GetById(projectId);
            if (model != null && model.CompanyId == userDto.CompanyId) {
                _rep.MarkAsDelete(model, userDto.Id);
                this.UnitOfWork.SaveChanges();
            }
        }
    }
}
