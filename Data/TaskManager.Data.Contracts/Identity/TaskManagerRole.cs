using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Data.Identity {
    public class TaskManagerRole : IdentityRole<Guid, TaskManagerUserRole> {
        /// <summary>
        ///     Constructor </summary>
        public TaskManagerRole() {
            Id = Guid.NewGuid();
        }
        /// <summary>
        ///     Constructor </summary>
        public TaskManagerRole(string roleName) : this() {
            Name = roleName;
        }
    }
}
