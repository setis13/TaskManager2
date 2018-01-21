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
        ///     Creates new Company for User </summary>
        /// <param name="companyDto">Creating company</param>
        /// <param name="userDto">Company Owner DTO</param>
        public void CreateCompany(CompanyDto companyDto, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Company>();
            var oldCompany = rep.GetById(userDto.CompanyId);
            if (oldCompany == null) {
                var model = this.Mapper.Map<Company>(companyDto);
                this.UnitOfWork.GetRepository<Company>().Insert(model, userDto.Id);
                UnitOfWork.SaveChanges();
                // Modify TaskManagerUser.CompanyId in caller method
            } else {
                throw new Exception("Company already exists");
            }
        }
    }
}
