using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TaskManager.Data.Contracts.Extensions;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Identity;
using TaskManager.Web.Extensions;

namespace TaskManager.Web.Controllers.Base {
    public class BaseApiController : ApiController {
        /// <summary>
        ///     The session service. </summary>
        protected readonly IServicesHost ServicesHost;
        /// <summary>
        ///     The Mapper </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseApiController" /> class. </summary>
        /// <param name="servicesHost">The Services host.</param>
        /// <param name="mapper">Automapper Instance</param>
        public BaseApiController(IServicesHost servicesHost, IMapper mapper) {
            this.ServicesHost = servicesHost;
            this.Mapper = mapper;
        }

        /// <summary>
        ///     Gets user manager </summary>
        protected TaskManagerUserManager UserManager => Request.GetOwinContext().GetUserManager<TaskManagerUserManager>();
        /// <summary>
        ///     Gets role manager </summary>
        protected TaskManagerRoleManager RoleManager => Request.GetOwinContext().GetUserManager<TaskManagerRoleManager>();
        /// <summary>
        ///     Gets Sign In manager </summary>
        protected TaskManagerSignInManager SignInManager => Request.GetOwinContext().Get<TaskManagerSignInManager>();
        /// <summary>
        ///     Auth manager </summary>
        protected IAuthenticationManager AuthenticationManager => Request.GetOwinContext().Authentication;

        /// <summary>
        ///     Add error to mpdel state </summary>
        /// <param name="result">Edentity result</param>
        protected void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        protected string GetErrors() {
            return String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)).GetHtml();
        }

        /// <summary>
        ///     Gets current user DTO </summary>
        /// <remarks>
        /// The method is used to get/set data. So here I implemented to check by a company
        /// </remarks>
        protected UserDto GetUserDto() {
            Guid userId = IdentityExtensions1.GetUserId(this.User.Identity);
            var userDto = Mapper.Map<UserDto>(this.UserManager.FindById(userId));
            if (userDto.CompanyId == Guid.Empty) {
                throw new Exception("Please create a company");
            }
            return userDto;
        }
    }
}