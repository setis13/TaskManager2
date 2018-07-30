using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TaskManager.Logic;

namespace TaskManager.Web.Controllers {
    [AllowAnonymous]
    public class HomeController : BaseController {
        public HomeController(IServicesHost servicesHost, IMapper mapper)
            : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     GET /Home </summary>
        public ActionResult Index() {
            // gets name of action
            var arr = ((HttpRequestWrapper)this.Request).Path.Split('/');
            if (arr.Length > 0 && arr[1].ToLower() == "home") {
                // if home then redirect
                return RedirectToAction("Index");
            }
            if (Request.IsAuthenticated) {
                // empty template for angular
                return View("Empty");
            } else {
                // home page of site. guest page
                return View();
            }
        }
    }
}