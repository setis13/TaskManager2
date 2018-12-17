using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Common;
using TaskManager.Logic;
using TaskManager.Logic.Services;
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
        ///     POST: /api/Company/GetData </summary>
        [HttpPost, Route("GetData")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> GetData(ReportModel model) {
            try {
#if DEBUG
                await Task.Delay(500);
#endif
                return await Task.Factory.StartNew(() => {
                    // gets all time of day
                    var start = model.Start.Date;
                    var end = (model.End?.Date ?? start).AddDays(1).AddMilliseconds(-1);

                    var projectDtos = _service.GetData(start, end, model.IncludeNew, model.IncludeZero, model.ProjectIds, GetUserDto());

                    var sumActualWork = new TimeSpan(projectDtos
                        .Select(e => e.SumActualWork.Ticks)
                        .DefaultIfEmpty(0)
                        .Sum());

                    return WebApiResult.Succeed(
                        new {
                            ReportProjects = projectDtos,
                            SumActualWork = sumActualWork
                        });
                });
            } catch (Exception e) {
                Logger.e("GetSingle", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}