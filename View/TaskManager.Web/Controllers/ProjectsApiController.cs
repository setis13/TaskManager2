using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/Projects")]
    public class ProjectsApiController : BaseApiController {

        public ProjectsApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Projects/GetData </summary>
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
                    List<ProjectDto> projects = this.ServicesHost
                        .GetService<IProjectsService>().GetData(userDto);
                    return WebApiResult.Succeed(new { Projects = projects });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Projects/Save </summary>
        [HttpPost, Route("Save")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Save(ProjectDto project) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var userDto = Mapper.Map<UserDto>(base.UserManager.FindById(userId));
                    this.ServicesHost.GetService<IProjectsService>().Save(project, userDto);
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("Save", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Projects/Delete </summary>
        [HttpPost, Route("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Delete(Guid id) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
                    var userDto = Mapper.Map<UserDto>(base.UserManager.FindById(userId));
                    this.ServicesHost.GetService<IProjectsService>().Delete(id, userDto);
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("Delete", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }
    }
}