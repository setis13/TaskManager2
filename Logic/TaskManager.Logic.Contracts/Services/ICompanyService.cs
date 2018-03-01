using System;
using System.Collections.Generic;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Company Service interface. </summary>
    public interface ICompanyService : IService {

        /// <summary>
        ///     Gets Company and users </summary>
        /// <param name="userId">User ID</param>
        /// <param name="company">Company DTO</param>
        /// <param name="invitationCompany">Invitation Company DTO</param>
        /// <param name="users">List of User DTOS</param>
        /// <param name="invitedUsers">List of invited User DTOS</param>
        void GetData(Guid userId, out CompanyDto company, out CompanyDto invitationCompany, out List<UserDto> users, out List<UserDto> invitedUsers);

        /// <summary>
        ///     To Leave from a compnay </summary>
        /// <param name="userId">Owner ID</param>
        void LeaveCompany(Guid userId);

        /// <summary>
        ///     To Accept Invitation to a company </summary>
        /// <param name="userId">Owner ID</param>
        void AcceptCompany(Guid userId);

        /// <summary>
        ///     To Reject Invitation to a company </summary>
        /// <param name="userId">Owner ID</param>
        void RejectCompany(Guid userId);

        /// <summary>
        ///     Removes user from my company </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Removing user ID</param>
        void RemoveUser(Guid userId, Guid id);

        /// <summary>
        ///     Refects user's invitatuon </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Removing user ID</param>
        void CancelInvitation(Guid userId, Guid id);

        /// <summary>
        ///     Finds user by UserName or Email </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="userDto">Searching Data</param>
        /// <returns>User DTO</returns>
        UserDto FindUser(Guid userId, UserDto userDto);

        /// <summary>
        ///     To Invite a user </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Inviting user ID</param>
        void InviteUser(Guid userId, Guid id);

        /// <summary>
        ///  Creates new company
        /// </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="companyDto">Company DTO</param>
        void CreateCompany(Guid userId, CompanyDto companyDto);

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
        /// <param name="ownerId">Owner ID</param>
        /// <returns>Compnay DTO</returns>
        CompanyDto GetCompanyByOwnerId(Guid ownerId);

        /// <summary>
        ///     Creates new Company for User </summary>
        /// <param name="companyDto">Creating company</param>
        /// <param name="userDto">Company Owner DTO</param>
        void CreateCompany(CompanyDto companyDto, UserDto userDto);

        /// <summary>
        ///     Deletes company </summary>
        /// <param name="companyId">Company ID</param>
        /// <param name="userDto">User DTO</param>
        void DeleteCompany(Guid companyId, UserDto userDto);
    }
}
