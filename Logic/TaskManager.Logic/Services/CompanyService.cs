using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Entities;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Services.Base;

namespace TaskManager.Logic.Services {
    public class CompanyService : HostService<ICompanyService>, ICompanyService {
        public CompanyService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Gets company by ID</summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>Compnay DTO</returns>
        public CompanyDto GetCompanyById(Guid companyId) {
            var model = UnitOfWork.GetRepository<Company>().GetById(companyId);
            return Mapper.Map<CompanyDto>(model);
        }

        /// <summary>
        ///     Gets company by Name</summary>
        /// <param name="name">Company Name</param>
        /// <returns>Compnay DTO</returns>
        public CompanyDto GetCompanyByName(string name) {
            var models = UnitOfWork.GetRepository<Company>().SearchFor(e => e.Name == name);
            if (models.Any()) {
                return Mapper.Map<CompanyDto>(models.First());
            } else {
                return null;
            }
        }

        /// <summary>
        ///     Gets company by owner ID</summary>
        /// <param name="ownerId">Owner ID</param>
        /// <returns>Compnay DTO</returns>
        public CompanyDto GetCompanyByOwnerId(Guid ownerId) {
            var models = UnitOfWork.GetRepository<Company>().SearchFor(e => e.CreatedById == ownerId);
            if (models.Any()) {
                return Mapper.Map<CompanyDto>(models.First());
            } else {
                return null;
            }
        }

        /// <summary>
        ///     Creates or Updates company </summary>
        /// <param name="companyDto">company DTO</param>
        /// <param name="userDto">user who updates the company</param>
        public void Save(CompanyDto companyDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Company>();
            var model = rep.GetById(companyDto.EntityId);
            if (model == null) {
                model = this.Mapper.Map<Company>(companyDto);
                this.UnitOfWork.GetRepository<Company>().Insert(model, userDto.Id);
            } else {
                this.Mapper.Map(companyDto, model);
                this.UnitOfWork.GetRepository<Company>().Update(model, userDto.Id);
            }
            this.UnitOfWork.SaveChanges();
        }
    }
}
