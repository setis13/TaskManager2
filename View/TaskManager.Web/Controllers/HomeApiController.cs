using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Controllers.Base;

namespace TaskManager.Web.Controllers {
    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    [RoutePrefix("api/Home")]
    public class HomeApiController : BaseApiController {

        public HomeApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }
    }
}