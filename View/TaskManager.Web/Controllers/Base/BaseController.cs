using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TaskManager.Common;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Identity;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers.Base {

    public class BaseController : Controller {
        /// <summary>
        ///     The session service. </summary>
        protected readonly IServicesHost ServicesHost;
        /// <summary>
        ///     The Mapper </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseController" /> class. </summary>
        /// <param name="servicesHost">The Services host.</param>
        /// <param name="mapper">The AutoMapper interface</param>
        public BaseController(IServicesHost servicesHost, IMapper mapper) {
            this.ServicesHost = servicesHost;
            this.Mapper = mapper;
        }

        /// <summary>
        ///     Gets or sets the current session. </summary>
        public SessionDto CurrentSession { get; set; }

        /// <summary>
        ///     Gets user manager </summary>
        protected TaskManagerUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<TaskManagerUserManager>();
        /// <summary>
        ///     Gets role manager </summary>
        protected TaskManagerRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<TaskManagerRoleManager>();
        /// <summary>
        ///     Gets Sign In manager </summary>
        protected TaskManagerSignInManager SignInManager => HttpContext.GetOwinContext().Get<TaskManagerSignInManager>();
        /// <summary>
        ///     Auth manager </summary>
        protected IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        /// <summary>
        ///     The on authorization. </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void OnAuthorization(AuthorizationContext filterContext) {
            // Retrieve/create root domain authorisation cookie.
            this.CurrentSession = this.ServicesHost.GetService<ISessionService>().Authenticate();
            base.OnAuthorization(filterContext);
        }

        /// <summary>
        ///     On controller exception </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void OnException(ExceptionContext filterContext) {
            Logger.f("Failed to Execute controller Result - Controller: {0}, Request: {1}", filterContext.Exception, ToString(), Request);
            base.OnException(filterContext);
        }

        /// <summary>
        ///     Add error to mpdel state </summary>
        /// <param name="result">Edentity result</param>
        protected void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        ///     Redirect to location </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <param name="action">Action</param>
        /// <param name="controller">Controller</param>
        /// <returns>Action result</returns>
        protected ActionResult RedirectToLocal(string returnUrl, string action = "Index", string controller = "Home") {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction(action, controller);
        }

        /// <summary>
        ///     Returns json result </summary>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding,
            JsonRequestBehavior behavior) {
            return new JsonResult() {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        /// <summary>
        ///     The view. </summary>
        /// <returns>The <see cref="ActionResult" />.</returns>
        protected new ActionResult View() {
            return this.View(new BaseModel(this.CurrentSession));
        }

        /// <summary>
        ///     The view. </summary>
        /// <param name="viewName">The view name.</param>
        /// <returns>The <see cref="ActionResult" />.</returns>
        protected new ActionResult View(string viewName) {
            return this.View(viewName, new BaseModel(this.CurrentSession));
        }

        /// <summary>
        ///     Return last InnerException </summary>
        protected Exception GetException(Exception exception) {
            if (exception.InnerException != null) {
                return GetException(exception.InnerException);
            } else {
                return exception;
            }
        }
    }
}