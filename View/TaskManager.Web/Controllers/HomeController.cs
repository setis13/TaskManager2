using System.Web.Mvc;
using AutoMapper;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Attributes;
using TaskManager.Web.Controllers.Base;

namespace TaskManager.Web.Controllers {
    [TaskManagerAuthorize]
    public class HomeController : BaseController {
        public HomeController(IServicesHost servicesHost, IMapper mapper)
            : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     GET /Home </summary>
        public ActionResult Index() {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}