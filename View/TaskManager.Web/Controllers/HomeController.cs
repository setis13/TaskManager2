using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Attributes;
using TaskManager.Web.Controllers.Base;

namespace TaskManager.Web.Controllers {
    [AllowAnonymous]
    public class HomeController : BaseController {
        public HomeController(IServicesHost servicesHost, IMapper mapper)
            : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     GET /Home </summary>
        public ActionResult Index() {
            var arr = ((HttpRequestWrapper)this.Request).Path.Split('/');
            if (arr.Length > 0 && arr[1].ToLower() == "home") {
                return RedirectToAction("Index");
            }
            if (Request.IsAuthenticated) {
                return View("Empty");
            } else {
                return View();
            }
        }
    }
}