
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TaskManager.Common;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services;
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

                // gets user by email
                var user = await UserManager.FindByEmailAsync(model.Login);

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(
                    user != null ? user.UserName : model.Login, model.Password, true, shouldLockout: false);
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
                return WebApiResult.Failed(e.Message);
            }
        }

        /// <summary>
        ///     POST: /api/Account/Register </summary>
        [HttpPost, Route("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<WebApiResult> Register(RegisterViewModel model) {
            try {
                // to Trim and to Lower strings
                model.CompanyName = model.CompanyName?.Trim();
                model.Email = model.Email?.Trim().ToLower();
                model.UserName = model.UserName?.Trim();

                if (!ModelState.IsValid) {
                    return WebApiResult.Failed(base.GetErrors());
                }
                if (UserManager.Users.Any(e => e.UserName == model.UserName)) {
                    return WebApiResult.Failed($"User with this name '{model.UserName}' already exists");
                }
                var service = ServicesHost.GetService<ICompanyService>();
                if (model.CompanyName != null && service.GetCompanyByName(model.CompanyName) != null) {
                    return WebApiResult.Failed($"Company with this name '{model.CompanyName}' already exists");
                }

                // Creates user
                var user = new TaskManagerUser {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    CompanyDto companyDto = null;
                    var userDto = Mapper.Map<UserDto>(user);
                    if (model.CompanyName != null) {
                        // Creates company
                        companyDto = new CompanyDto();
                        companyDto.Name = model.CompanyName;
                        service.CreateCompany(companyDto, userDto);
                        // Modifies user
                        user.CompanyId = userDto.CompanyId = companyDto.EntityId;
                        await UserManager.UpdateAsync(user);
                    }
                    // Sign in
                    await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);
                    return WebApiResult.Succeed(new { ReturnUrl = "/Home", User = userDto, Company = companyDto });
                }
                AddErrors(result);

                // If we got this far, something failed, redisplay form
                return WebApiResult.Failed(base.GetErrors());

            } catch (Exception e) {
                Logger.e("Register", e);
                return WebApiResult.Failed(e.Message);
            }
        }
    }
}