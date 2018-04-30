using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TaskManager.Logic.Identity;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Extensions;
using TaskManager.Data.Contracts.Identity;
using TaskManager.Logic.Contracts;

namespace TaskManager.Web {
    public partial class Startup {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup() {
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app) {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<IUnitOfWork>().Context);

            app.CreatePerOwinContext<IRoleStore<TaskManagerRole, Guid>>(
                (options, context) => DependencyResolver.Current.GetService<IUnitOfWork>().RoleStore);
            app.CreatePerOwinContext<IUserStore<TaskManagerUser, Guid>>(
                (options, context) => DependencyResolver.Current.GetService<IUnitOfWork>().UserStore);

            app.CreatePerOwinContext<TaskManagerUserManager>(
                (IdentityFactoryOptions<TaskManagerUserManager> options, IOwinContext context) => {
                    var manager = DependencyResolver.Current.GetService<IServicesHost>().UserManager;
                    var dataProtectionProvider = options.DataProtectionProvider;
                    if (dataProtectionProvider != null) {
                        manager.UserTokenProvider =
                            new DataProtectorTokenProvider<TaskManagerUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity")) {
                                TokenLifespan = TimeSpan.FromDays(30 * 12)
                            };
                    }
                    return (TaskManagerUserManager)manager;
                });
            app.CreatePerOwinContext<TaskManagerSignInManager>(TaskManagerSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                ExpireTimeSpan = TimeSpan.FromDays(30 * 12),
                CookieName = "TaskManager",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/login"),
                Provider = new CookieAuthenticationProvider {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<TaskManagerUserManager, TaskManagerUser, Guid>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: IdentityExtensions1.GetUserId)
                }
            });
        }
    }
}
