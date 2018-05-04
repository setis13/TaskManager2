using System;
using System.Collections.Generic;
using System.Linq;
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

        private ITaskService _service => this.ServicesHost.GetService<ITaskService>();

        public HomeApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Home/GetData </summary>
        [HttpPost, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData(DateTime? historyFilter = null) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {

                    List<UserDto> users;
                    List<ProjectDto> projects;
                    List<Task1Dto> tasks;
                    List<DateTime> historyFilters;
                    List<Guid> lastResponsibleIds;
                    bool lastFavorite;

                    var user = GetUserDto();
                    users = Mapper.Map<List<UserDto>>(
                        this.UserManager.Users.Where(e => e.CompanyId == user.CompanyId));

                    this._service.GetData(user, historyFilter, out projects, out tasks, out historyFilters, out lastResponsibleIds, out lastFavorite);
                    return WebApiResult.Succeed(new {
                        Projects = projects,
                        Users = users,
                        Tasks = tasks,
                        HistoryFilters = historyFilters,
                        SortBy = user.SortBy,
                        FavoriteFilter = user.FavoriteFilter,
                        LastResponsibleIds = lastResponsibleIds,
                        LastFavorite = lastFavorite
                    });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/SaveTask </summary>
        [HttpPost, Route("SaveTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> SaveTask(Task1Dto task) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.SaveTask(task, GetUserDto());
                    return WebApiResult.Succeed(new { EntityId = task.EntityId });
                });
            } catch (Exception e) {
                Logger.e("SaveTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DeleteTask </summary>
        [HttpPost, Route("DeleteTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DeleteTask(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.DeleteTask(id, GetUserDto());
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("DeleteTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/SaveSubTask </summary>
        [HttpPost, Route("SaveSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> SaveSubTask(SubTaskDto subtask) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.SaveSubTask(subtask, GetUserDto());
                    return WebApiResult.Succeed(new { EntityId = subtask.EntityId });
                });
            } catch (Exception e) {
                Logger.e("SaveSubTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DeleteSubTask </summary>
        [HttpPost, Route("DeleteSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DeleteSubTask(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.DeleteSubTask(id, GetUserDto());
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("DeleteSubTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/UpSubTask </summary>
        [HttpPost, Route("UpSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> UpSubTask(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    var subTasks = this._service.UpSubTask(id, GetUserDto());
                    return WebApiResult.Succeed(new { SubTasks = subTasks });
                });
            } catch (Exception e) {
                Logger.e("UpSubTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DownSubTask </summary>
        [HttpPost, Route("DownSubTask")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DownSubTask(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    var subTasks = this._service.DownSubTask(id, GetUserDto());
                    return WebApiResult.Succeed(new { SubTasks = subTasks });
                });
            } catch (Exception e) {
                Logger.e("DownSubTask", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/SaveComment </summary>
        [HttpPost, Route("SaveComment")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> SaveComment(CommentDto comment) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.SaveComment(comment, GetUserDto());
                    return WebApiResult.Succeed(new { EntityId = comment.EntityId });
                });
            } catch (Exception e) {
                Logger.e("SaveComment", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/DeleteComment </summary>
        [HttpPost, Route("DeleteComment")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> DeleteComment(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.DeleteComment(id, GetUserDto());
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("DeleteComment", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/InvertTaskFavorite </summary>
        [HttpPost, Route("InvertTaskFavorite")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> InvertTaskFavorite(Guid id) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    var result = this._service.InvertTaskFavorite(id, GetUserDto());
                    return WebApiResult.Succeed(new { Favorite = result });
                });
            } catch (Exception e) {
                Logger.e("InvertTaskFavorite", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Home/InvertSubTaskFavorite </summary>
        [HttpPost, Route("InvertSubTaskFavorite")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> InvertSubTaskFavorite(Guid taskId, Guid subtaskId) {
            try {
#if DEBUG
                await Task.Delay(300);
#endif
                return await Task.Factory.StartNew(() => {
                    var result = this._service.InvertSubTaskFavorite(taskId, subtaskId, GetUserDto());
                    return WebApiResult.Succeed(new { Favorite = result });
                });
            } catch (Exception e) {
                Logger.e("InvertSubTaskFavorite", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}