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
using Microsoft.AspNet.Identity;

namespace TaskManager.Logic.Services {
    public class CompanyService : HostService<ICompanyService>, ICompanyService {
        public CompanyService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        /// <summary>
        ///     Gets Company and users </summary>
        /// <param name="userId">User ID</param>
        /// <param name="company">Company DTO</param>
        /// <param name="invitationCompany">Invitation Company DTO</param>
        /// <param name="users">List of User DTOS</param>
        /// <param name="invitedUsers">List of invited User DTOS</param>
        public void GetData(Guid userId, out CompanyDto company, out CompanyDto invitationCompany, out List<UserDto> users, out List<UserDto> invitedUsers) {
            var userDto = Mapper.Map<UserDto>(base.ServicesHost.UserManager.FindById(userId));
            // company
            if (userDto.CompanyId != Guid.Empty) {
                company = this.GetCompanyById(userDto.CompanyId);
            } else {
                company = null;
            }
            // invitation company
            if (userDto.InvitationCompanyId != null) {
                invitationCompany = this.GetCompanyById(userDto.InvitationCompanyId.Value);
            } else {
                invitationCompany = null;
            }
            // users
            users = Mapper.Map<List<UserDto>>(base.ServicesHost.UserManager.Users
               .Where(e => e.CompanyId == userDto.CompanyId));
            // invited users, but they were not accepted a invitation
            invitedUsers = new List<UserDto>();
            // I am owner of my company
            if (company != null && company.CreatedById == userDto.Id) {
                invitedUsers = Mapper.Map<List<UserDto>>(base.ServicesHost.UserManager.Users
                    .Where(e => e.InvitationCompanyId == userDto.CompanyId));
            } else {
                invitedUsers = null;
            }
        }

        /// <summary>
        ///     To Leave from a compnay </summary>
        /// <param name="userId">Owner ID</param>
        public void LeaveCompany(Guid userId) {
            var user = base.ServicesHost.UserManager.FindById(userId);
            var userDto = Mapper.Map<UserDto>(user);
            if (user.CompanyId != null) {
                var oldCompanyId = user.CompanyId.Value;
                user.CompanyId = null;
                base.ServicesHost.UserManager.Update(user);
                // removes company if it is empty
                if (base.ServicesHost.UserManager.Users.Any(e => e.CompanyId == oldCompanyId) == false) {
                    this.DeleteCompany(oldCompanyId, userDto);
                } else {
                    throw new Exception("User doesn't have a company");
                }
            }
        }

        /// <summary>
        ///     To Accept Invitation to a company </summary>
        /// <param name="userId">Owner ID</param>
        public void AcceptCompany(Guid userId) {
            var model = base.ServicesHost.UserManager.FindById(userId);
            if (model.InvitationCompanyId != null) {
                model.CompanyId = model.InvitationCompanyId;
                model.InvitationCompanyId = null;
                base.ServicesHost.UserManager.Update(model);
            } else {
                throw new Exception("User doesn't have an invitation");
            }
        }

        /// <summary>
        ///     To Reject Invitation to a company </summary>
        /// <param name="userId">Owner ID</param>
        public void RejectCompany(Guid userId) {
            var model = base.ServicesHost.UserManager.FindById(userId);
            if (model.InvitationCompanyId != null) {
                model.InvitationCompanyId = null;
                base.ServicesHost.UserManager.Update(model);
            } else {
                throw new Exception("User doesn't have an invitation");
            }
        }

        /// <summary>
        ///     Removes user from my company </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Removing user ID</param>
        public void RemoveUser(Guid userId, Guid id) {
            var user = base.ServicesHost.UserManager.FindById(userId);
            if (user.CompanyId != null) {
                var companyDto = this.GetCompanyById(user.CompanyId.Value);
                if (user.Id == companyDto.CreatedById) {
                    var invitedUser = base.ServicesHost.UserManager.FindById(id);
                    if (invitedUser.CompanyId == companyDto.EntityId) {
                        invitedUser.CompanyId = null;
                        base.ServicesHost.UserManager.Update(invitedUser);
                    } else {
                        throw new Exception("Users have different companies");
                    }
                } else {
                    throw new Exception("User is not owner of company");
                }
            } else {
                throw new Exception("User doesn't have a company");
            }
        }

        /// <summary>
        ///     Refects user's invitatuon </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Removing user ID</param>
        public void CancelInvitation(Guid userId, Guid id) {
            var user = base.ServicesHost.UserManager.FindById(userId);
            if (user.CompanyId != null) {
                var companyDto = this.GetCompanyById(user.CompanyId.Value);
                if (user.Id == companyDto.CreatedById) {
                    var invitedUser = base.ServicesHost.UserManager.FindById(id);
                    if (invitedUser.InvitationCompanyId == companyDto.EntityId) {
                        invitedUser.InvitationCompanyId = null;
                        base.ServicesHost.UserManager.Update(invitedUser);
                    } else {
                        throw new Exception("Users have different companies");
                    }
                } else {
                    throw new Exception("User is not owner of company");
                }
            } else {
                throw new Exception("User doesn't have a company");
            }
        }

        /// <summary>
        ///     Finds user by UserName or Email </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="userDto">Searching Data</param>
        /// <returns>User DTO</returns>
        public UserDto FindUser(Guid userId, UserDto userDto) {
            var owner = base.ServicesHost.UserManager.FindById(userId);
            if (owner.CompanyId != null) {
                var companyDto = this.GetCompanyById(owner.CompanyId.Value);
                if (owner.Id == companyDto.CreatedById) {
                    // TODO: check up/down case
                    var user = this.ServicesHost.UserManager.Users.FirstOrDefault(e =>
                        e.Email == userDto.UserName || e.UserName == userDto.UserName);
                    if (user != null) {
                        return Mapper.Map<UserDto>(user);
                    } else {
                        throw new Exception("User was not found");
                    }
                } else {
                    throw new Exception("User is not owner of company");
                }
            } else {
                throw new Exception("User doesn't have a company");
            }
        }

        /// <summary>
        ///     To Invite a user </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="id">Inviting user ID</param>
        public void InviteUser(Guid userId, Guid id) {
            var owner = base.ServicesHost.UserManager.FindById(userId);
            if (owner.CompanyId != null) {
                var companyDto = this.GetCompanyById(owner.CompanyId.Value);
                if (owner.Id == companyDto.CreatedById) {
                    var user = base.ServicesHost.UserManager.FindById(id);
                    if (user.CompanyId == null) {
                        user.InvitationCompanyId = companyDto.EntityId;
                        base.ServicesHost.UserManager.Update(user);
                    } else {
                        throw new Exception("User already has a company");
                    }
                } else {
                    throw new Exception("User is not owner of company");
                }
            } else {
                throw new Exception("User doesn't have a company");
            }
        }

        /// <summary>
        ///  Creates new company
        /// </summary>
        /// <param name="userId">Owner ID</param>
        /// <param name="companyDto">Company DTO</param>
        public void CreateCompany(Guid userId, CompanyDto companyDto) {
            if (this.GetCompanyByName(companyDto.Name) != null) {
                throw new Exception($"Company with this name '{companyDto.Name}' already exists");
            }
            var model = base.ServicesHost.UserManager.FindById(userId);
            var userDto = Mapper.Map<UserDto>(model);
            this.CreateCompany(companyDto, userDto);
            // Modifies company ID
            model.CompanyId = companyDto.EntityId;
            base.ServicesHost.UserManager.Update(model);
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

        /// <summary>
        ///     Deletes company </summary>
        /// <param name="companyId">Company ID</param>
        /// <param name="userDto">User DTO</param>
        public void DeleteCompany(Guid companyId, UserDto userDto) {
            var rep = UnitOfWork.GetRepository<Company>();
            var company = rep.GetById(userDto.CompanyId);
            if (company != null) {
                company.Name += "_DELETED";
                company.IsDeleted = true;
                this.UnitOfWork.GetRepository<Company>().Update(company, userDto.Id);
                UnitOfWork.SaveChanges();
            } else {
                throw new Exception("Company was not found");
            }
        }
    }
}
