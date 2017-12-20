using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Common.Identity {
    public class TaskManagerUser : IdentityUser<Guid, TaskManagerUserLogin, TaskManagerUserRole, TaskManagerUserClaim> {

        #region [ .ctor ]

        /// <summary>
        ///     Constructor which creates a new Guid for the Id </summary>
        public TaskManagerUser() {
            Id = Guid.NewGuid();
        }

        /// <summary>
        ///     Constructor that takes a userName </summary>
        public TaskManagerUser(string email) : this() {
            Email = email;
        }

        #endregion [ .ctor ]  

        #region [ Public ]

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<TaskManagerUser, Guid> manager,
            string authenticationType) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Adds custom user claims here
            return userIdentity;
        }

        #endregion [ Public ]
    }
}
