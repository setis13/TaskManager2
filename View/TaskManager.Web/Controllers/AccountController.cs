using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Controllers.Base;

namespace TaskManager.Web.Controllers {
    [AllowAnonymous]
    public class AccountController : BaseController {

        public AccountController(IServicesHost servicesHost, IMapper mapper)
            : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     GET: /Account/Login </summary>
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        ///     GET: /Account/Register </summary>
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        /// <summary>
        ///     GET: /Account/Profile </summary>
        [Authorize, Route("Account/Profile")]
        public ActionResult Profile1() {
            return View();
        }

        /// <summary>
        ///     GET: /Account/LogOff </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}