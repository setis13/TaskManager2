using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TaskManager.Common.Identity;

namespace TaskManager.Common.Extensions {

    public static class IdentityExtensions1 {

        /// <summary>
        ///     Gets UserID in Guid format </summary>
        public static Guid GetUserId(this IIdentity identity) {
            Guid result;
            return Guid.TryParse(identity.GetUserId<string>(), out result) ? result : Guid.Empty;
        }

        /// <summary>
        ///     Helper method to generate identity </summary>
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this TaskManagerUser user, UserManager<TaskManagerUser, Guid> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            return userIdentity;
        }
    }
}
