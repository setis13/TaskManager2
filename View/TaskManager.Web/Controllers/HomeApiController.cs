using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Web.Controllers.Base;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers {
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    [RoutePrefix("api/Home")]
    public class HomeApiController : BaseApiController {

        public HomeApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Home/GetData </summary>
        [HttpPost, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData() {
            try {
                return await Task.Factory.StartNew(() => {
                    List<UserDto> users;
                    List<ProjectDto> projects;
                    List<Task1Dto> tasks;
                    this.ServicesHost.GetService<ITaskService>().GetData(
                        null,
                        out users,
                        out projects,
                        out tasks);
                    return WebApiResult.Succeed(new { Projects = projects, Users = users, Tasks = tasks });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }
    }
}