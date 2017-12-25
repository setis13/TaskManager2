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
        ///     GET: /api/Home/GetData </summary>
        [HttpGet, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData() {
            try {
                return await Task.Factory.StartNew(() => {
                    List<Task1Dto> tasks;
                    List<SubTaskDto> subTasks;
                    List<ProjectDto> projects;
                    List<CommentDto> comments;
                    this.ServicesHost.GetService<ITaskService>().GetData(
                        null,
                        out projects,
                        out tasks,
                        out subTasks,
                        out comments);
                    return WebApiResult.Succeed(new { projects, tasks, subTasks, comments });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }
    }
}