using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using TaskManager.Data.Contracts.Extensions;
using TaskManager.Data.Contracts.Identity;
using TaskManager.Logic.Identity;

namespace TaskManager.Web {
    // Configure the application sign-in manager which is used in this application.
    public class TaskManagerSignInManager : SignInManager<TaskManagerUser, Guid> {
        public TaskManagerSignInManager(UserManager<TaskManagerUser, Guid> userManager,
            IAuthenticationManager authenticationManager) : base(userManager, authenticationManager) {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(TaskManagerUser user) {
            return user.GenerateUserIdentityAsync(UserManager);
        }

        public static TaskManagerSignInManager Create(IdentityFactoryOptions<TaskManagerSignInManager> options, IOwinContext context) {
            return new TaskManagerSignInManager(context.GetUserManager<TaskManagerUserManager>(), context.Authentication);
        }
    }
}