using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
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
#if DEBUG
                await Task.Delay(500);
#endif
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

        /// <summary>
        ///     POST: /api/Home/SaveTask </summary>
        [HttpPost, Route("SaveTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> SaveTask(Guid id) {
            try {
                await Task.Delay(1000);
                throw new Exception("SaveTask err");
            } catch (Exception e) {
                Logger.e("SaveTask", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DeleteTask </summary>
        [HttpPost, Route("DeleteTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DeleteTask(Guid id) {
            try {
                await Task.Delay(1000);
                throw new Exception("DeleteTask err");
            } catch (Exception e) {
                Logger.e("DeleteTask", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/SaveSubTask </summary>
        [HttpPost, Route("SaveSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> SaveSubTask(Guid id) {
            try {
                await Task.Delay(1000);
                throw new Exception("SaveSubTask err");
            } catch (Exception e) {
                Logger.e("SaveSubTask", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DeleteSubTask </summary>
        [HttpPost, Route("DeleteSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DeleteSubTask(Guid id) {
            try {
                await Task.Delay(1000);
                throw new Exception("DeleteSubTask err");
            } catch (Exception e) {
                Logger.e("DeleteSubTask", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }
    }
}