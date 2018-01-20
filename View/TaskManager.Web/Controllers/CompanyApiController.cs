﻿using System;
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
                    CompanyDto company = this._service.GetCompanyById(userDto.CompanyId);
                    List<UserDto> users = Mapper.Map<List<UserDto>>(this.UserManager.Users
                        .Where(e => e.CompanyId == userDto.CompanyId));
                    return WebApiResult.Succeed(new { Company = company, Users = users });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/Save </summary>
        [HttpPost, Route("Save")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Save(CompanyDto company) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var userDto = Mapper.Map<UserDto>(base.UserManager.FindById(userId));
                    this._service.Save(company, userDto);
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("Save", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}