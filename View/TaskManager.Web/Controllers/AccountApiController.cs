
using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TaskManager.Common;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Web.Controllers.Base;
using TaskManager.Web.Models;

namespace TaskManager.Web.Controllers {

    [Authorize]
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    [RoutePrefix("api/Account")]
    public class AccountApiController : BaseApiController {

        public AccountApiController(IServicesHost servicesHost, IMapper mapper) : base(servicesHost, mapper) {
        }

        /// <summary>
        ///     POST: /api/Account/Login </summary>
        [AllowAnonymous]
        [HttpPost, Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Login([FromBody] LoginViewModel model) {
            try {
                if (!ModelState.IsValid) {
                    return WebApiResult.Failed(base.GetErrors());
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
                switch (result) {
                    case SignInStatus.Success:
                        return WebApiResult.Succeed(new { ReturnUrl = "/Home" });
                    case SignInStatus.LockedOut:
                        throw new NotImplementedException();
                    //return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        throw new NotImplementedException();
                    //return RedirectToAction("SendCode", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return WebApiResult.Failed(base.GetErrors());
                }
            } catch (Exception e) {
                Logger.e("Login", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }

        /// <summary>
        ///     POST: /api/Account/Register </summary>
        [HttpPost, Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Register(RegisterViewModel model) {
            try {
                if (!ModelState.IsValid) {
                    return WebApiResult.Failed(base.GetErrors());
                }

                var user = new TaskManagerUser {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);
                    return WebApiResult.Succeed(new { ReturnUrl = "/Home", UserId = user.Id });
                }
                AddErrors(result);

                // If we got this far, something failed, redisplay form
                return WebApiResult.Failed(base.GetErrors());

            } catch (Exception e) {
                Logger.e("Register", e);
                return WebApiResult.Failed(e.GetBaseException().Message);
            }
        }
    }
}