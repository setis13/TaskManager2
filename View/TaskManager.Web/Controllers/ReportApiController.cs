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
    [RoutePrefix("api/Report")]
    public class ReportApiController : BaseApiController {

        private IReportService _service => this.ServicesHost.GetService<IReportService>();

        public ReportApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Company/GetSingle </summary>
        [HttpPost, Route("GetSingle")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetSingle(DateTime date) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    var projectDtos = _service.GetSingleDayData(date, GetUserDto());
                    return WebApiResult.Succeed(new { ReportProjects = projectDtos });
                });
            } catch (Exception e) {
                Logger.e("GetSingle", e);
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Company/GetPeriod </summary>
        [HttpPost, Route("GetPeriod")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetPeriod(DateTime start, DateTime end) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {

                    return WebApiResult.Succeed(new {
                    });
                });
            } catch (Exception e) {
                Logger.e("GetPeriod", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}