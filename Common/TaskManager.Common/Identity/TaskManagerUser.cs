using System;
using System.ComponentModel.DataAnnotations.Schema;
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

        /// <summary>
        ///     Company ID </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        ///     Invitation company ID </summary>
        /// <remarks>
        /// User can receive an invitation
        /// </remarks>
        public Guid? InvitationCompanyId { get; set; }
        /// <summary>
        ///     See Scripts/app/enums/SortByEnum.ts </summary>
        public byte SortBy { get; set; }

        [NotMapped]
        public object Company { get; set; }

        [NotMapped]
        public static TaskManagerUser SystemAdmin => new TaskManagerUser {
            Id = new Guid("1ABB568A-2ECD-43E6-B814-BE164CF2F6F4"),
            Email = "admin@taskmanager.com",
            UserName = "admin",
        };
    }
}
