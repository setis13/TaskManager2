using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
using TaskManager.Common.Extensions;
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
                    CompanyDto company, invitationCompany;
                    List<UserDto> users, invitedUsers;
                    _service.GetData(userId, out company, out invitationCompany, out users, out invitedUsers);
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
                    _service.LeaveCompany(userId);
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
                    _service.AcceptCompany(userId);
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
                    _service.RejectCompany(userId);
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
                    _service.RemoveUser(userId, id);
                    return WebApiResult.Succeed();
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
                    _service.CancelInvitation(userId, id);
                    return WebApiResult.Succeed();
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
                    userDto = _service.FindUser(userId, userDto);
                    return WebApiResult.Succeed(new { User = userDto });
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
                    _service.InviteUser(userId, id);
                    return WebApiResult.Succeed();
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
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    _service.CreateCompany(userId, companyDto);
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("CreateCompany", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}