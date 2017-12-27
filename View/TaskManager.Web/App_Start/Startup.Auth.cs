using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TaskManager.Common.Extensions;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts.Context;
using TaskManager.Data.Identity;
using TaskManager.Logic.Identity;

namespace TaskManager.Web {
    public partial class Startup {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup() {
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app) {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ITaskManagerDbContext>());

            app.CreatePerOwinContext<IRoleStore<TaskManagerRole, Guid>>(
                (options, context) => new TaskManagerRoleStore(context.Get<ITaskManagerDbContext>()));
            app.CreatePerOwinContext<IUserStore<TaskManagerUser, Guid>>(
                (options, context) => new TaskManagerUserStore(context.Get<ITaskManagerDbContext>()));

            app.CreatePerOwinContext<TaskManagerUserManager>(TaskManagerUserManager.Create);
            app.CreatePerOwinContext<TaskManagerSignInManager>(TaskManagerSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/login"),
                Provider = new CookieAuthenticationProvider {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<TaskManagerUserManager, TaskManagerUser, Guid>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: TaskManager.Common.Extensions.IdentityExtensions1.GetUserId)

                }
            });
        }
    }
}
