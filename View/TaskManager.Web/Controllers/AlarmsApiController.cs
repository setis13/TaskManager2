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
    [RoutePrefix("api/Alarms")]
    public class AlarmsApiController : BaseApiController {

        private IAlarmsService _service => this.ServicesHost.GetService<IAlarmsService>();

        public AlarmsApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Alarms/GetData </summary>
        [HttpPost, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData() {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    List<AlarmDto> alarms = _service.GetData(GetUserDto());
                    return WebApiResult.Succeed(new { Alarms = alarms });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Alarms/GetNearAlarms </summary>
        [HttpPost, Route("GetNearAlarms")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetNearAlarms(DateTime date) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    List<AlarmDto> alarms = _service.GetNearAlarms(date, GetUserDto());
                    return WebApiResult.Succeed(new { Alarms = alarms });
                });
            } catch (Exception e) {
                Logger.e("GetData", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Alarms/Save </summary>
        [HttpPost, Route("Save")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Save(AlarmDto alarm) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.Save(alarm, GetUserDto());
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("Save", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Alarms/Delete </summary>
        [HttpPost, Route("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Delete(Guid id) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    this._service.Delete(id, GetUserDto());
                    return WebApiResult.Succeed();
                });
            } catch (Exception e) {
                Logger.e("Delete", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}