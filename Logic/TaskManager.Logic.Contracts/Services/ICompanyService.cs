using System;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Company Service interface. </summary>
    public interface ICompanyService : IService {
        /// <summary>
        ///     Gets company by ID</summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>Compnay DTO</returns>
        CompanyDto GetCompanyById(Guid companyId);
        /// <summary>
        ///     Gets company by Name</summary>
        /// <param name="name">Company Name</param>
        /// <returns>Compnay DTO</returns>
        CompanyDto GetCompanyByName(string name);
        /// <summary>
        ///     Gets company by owner ID</summary>
        /// <param name="ownerId">Company Owner ID</param>
        /// <returns>Compnay DTO</returns>
        CompanyDto GetCompanyByOwnerId(Guid ownerId);
        /// <summary>
        ///     Creates new Company for User </summary>
        /// <param name="companyDto">Creating company</param>
        /// <param name="userDto">Company Owner DTO</param>
        void CreateCompany(CompanyDto companyDto, UserDto userDto);
    }
}