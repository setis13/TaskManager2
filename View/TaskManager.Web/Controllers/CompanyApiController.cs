using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
using TaskManager.Common.Extensions;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Web.Controllers.Base;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers {
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    [RoutePrefix("api/Company")]
    public class CompanyApiController : BaseApiController {

        private ICompanyService _service => this.ServicesHost.GetService<ICompanyService>();

        public CompanyApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Company/GetData </summary>
        [HttpPost, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData() {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var userDto = Mapper.Map<UserDto>(base.UserManager.FindById(userId));
                    CompanyDto company = null, invitationCompany = null;
                    // company
                    if (userDto.CompanyId != Guid.Empty) {
                        company = this._service.GetCompanyById(userDto.CompanyId);
                    }
                    // invitation company
                    if (userDto.InvitationCompanyId != null) {
                        invitationCompany = this._service.GetCompanyById(userDto.InvitationCompanyId.Value);
                    }
                    // users
                    List<UserDto> users = Mapper.Map<List<UserDto>>(this.UserManager.Users
                        .Where(e => e.CompanyId == userDto.CompanyId));
                    // invited users, but they were not accepted a invitation
                    List<UserDto> invitedUsers = new List<UserDto>();
                    // I am owner of my company
                    if (company != null && company.CreatedById == userDto.Id) {
                        invitedUsers = Mapper.Map<List<UserDto>>(this.UserManager.Users
                        .Where(e => e.InvitationCompanyId == company.EntityId));
                    }
                    return WebApiResult.Succeed(new {
                        Company = company,
                        InvitationCompany = invitationCompany,
                        Users = users,
                        InvitedUsers = invitedUsers
                    });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/LeaveCompany </summary>
        [HttpPost, Route("LeaveCompany")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> LeaveCompany() {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var user = base.UserManager.FindById(userId);
                    var userDto = Mapper.Map<UserDto>(user);
                    if (user.CompanyId != null) {
                        var oldCompanyId = user.CompanyId.Value;
                        user.CompanyId = null;
                        base.UserManager.Update(user);
                        // removes company if it is empty
                        if (base.UserManager.Users.Any(e => e.CompanyId == oldCompanyId) == false) {
                            _service.DeleteCompany(oldCompanyId, userDto);
                        }
                    } else {
                        throw new Exception("User doesn't have a company");
                    }
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("LeaveCompany", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/AcceptCompany </summary>
        [HttpPost, Route("AcceptCompany")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> AcceptCompany() {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var model = base.UserManager.FindById(userId);
                    if (model.InvitationCompanyId != null) {
                        model.CompanyId = model.InvitationCompanyId;
                        model.InvitationCompanyId = null;
                        base.UserManager.Update(model);
                    } else {
                        throw new Exception("User doesn't have an invitation");
                    }
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("AcceptCompany", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/RejectCompany </summary>
        [HttpPost, Route("RejectCompany")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> RejectCompany() {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var model = base.UserManager.FindById(userId);
                    if (model.InvitationCompanyId != null) {
                        model.InvitationCompanyId = null;
                        base.UserManager.Update(model);
                    } else {
                        throw new Exception("User doesn't have an invitation");
                    }
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("RejectCompany", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/RemoveUser </summary>
        [HttpPost, Route("RemoveUser")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> RemoveUser(Guid id) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var user = base.UserManager.FindById(userId);
                    if (user.CompanyId != null) {
                        var companyDto = _service.GetCompanyById(user.CompanyId.Value);
                        if (user.Id == companyDto.CreatedById) {
                            var invitedUser = base.UserManager.FindById(id);
                            if (invitedUser.CompanyId == companyDto.EntityId) {
                                invitedUser.CompanyId = null;
                                base.UserManager.Update(invitedUser);
                                return WebApiResult.Succeed();
                            } else {
                                throw new Exception("Users have different companies");
                            }
                        } else {
                            throw new Exception("User is not owner of company");
                        }
                    } else {
                        throw new Exception("User doesn't have a company");
                    }
                });
            } catch (Exception e) {
                Logger.e("RemoveUser", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/CancelInvitation </summary>
        [HttpPost, Route("CancelInvitation")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> CancelInvitation(Guid id) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var user = base.UserManager.FindById(userId);
                    if (user.CompanyId != null) {
                        var companyDto = _service.GetCompanyById(user.CompanyId.Value);
                        if (user.Id == companyDto.CreatedById) {
                            var invitedUser = base.UserManager.FindById(id);
                            if (invitedUser.InvitationCompanyId == companyDto.EntityId) {
                                invitedUser.InvitationCompanyId = null;
                                base.UserManager.Update(invitedUser);
                                return WebApiResult.Succeed();
                            } else {
                                throw new Exception("Users have different companies");
                            }
                        } else {
                            throw new Exception("User is not owner of company");
                        }
                    } else {
                        throw new Exception("User doesn't have a company");
                    }
                });
            } catch (Exception e) {
                Logger.e("CancelInvitation", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/FindUser </summary>
        [HttpPost, Route("FindUser")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> FindUser(UserDto userDto) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var owner = base.UserManager.FindById(userId);
                    if (owner.CompanyId != null) {
                        var companyDto = _service.GetCompanyById(owner.CompanyId.Value);
                        if (owner.Id == companyDto.CreatedById) {
                            // TODO: check up/down case
                            var user = UserManager.Users.FirstOrDefault(e =>
                                e.Email == userDto.UserName || e.UserName == userDto.UserName);
                            if (user != null) {
                                return WebApiResult.Succeed(new { User = user });
                            } else {
                                throw new Exception("User was not found");
                            }
                        } else {
                            throw new Exception("User is not owner of company");
                        }
                    } else {
                        throw new Exception("User doesn't have a company");
                    }
                });
            } catch (Exception e) {
                Logger.e("FindUser", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/InviteUser </summary>
        [HttpPost, Route("InviteUser")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> InviteUser(Guid id) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var owner = base.UserManager.FindById(userId);
                    if (owner.CompanyId != null) {
                        var companyDto = _service.GetCompanyById(owner.CompanyId.Value);
                        if (owner.Id == companyDto.CreatedById) {
                            var user = base.UserManager.FindById(id);
                            if (user.CompanyId == null) {
                                user.InvitationCompanyId = companyDto.EntityId;
                                this.UserManager.Update(user);
                                return WebApiResult.Succeed();
                            } else {
                                throw new Exception("User already has a company");
                            }
                        } else {
                            throw new Exception("User is not owner of company");
                        }
                    } else {
                        throw new Exception("User doesn't have a company");
                    }
                });
            } catch (Exception e) {
                Logger.e("InviteUser", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/CreateCompany </summary>
        [HttpPost, Route("CreateCompany")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> CreateCompany(CompanyDto companyDto) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    if (this._service.GetCompanyByName(companyDto.Name) != null) {
                        return WebApiResult.Failed($"Company with this name '{companyDto.Name}' already exists");
                    }
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var model = base.UserManager.FindById(userId);
                    var userDto = Mapper.Map<UserDto>(model);
                    this._service.CreateCompany(companyDto, userDto);
                    // Modifies company ID
                    model.CompanyId = companyDto.EntityId;
                    base.UserManager.Update(model);
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("CreateCompany", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}