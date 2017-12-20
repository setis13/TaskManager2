using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using TaskManager.Common.Enums;
using TaskManager.Common.Extensions;
using TaskManager.Common.Identity;
using TaskManager.Data.Identity;
using Microsoft.AspNet.Identity;

namespace TaskManager.Data.Context {
    public class TaskManagerDbContextConfiguration : DbMigrationsConfiguration<TaskManagerDbContext> {
        /// <summary>
        ///     Creates configutation instance </summary>
        public TaskManagerDbContextConfiguration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskManagerDbContext context) {
            // inits users and roles manager
            var userManager = new UserManager<TaskManagerUser, Guid>(new TaskManagerUserStore(context));
            var roleManager = new RoleManager<TaskManagerRole, Guid>(new TaskManagerRoleStore(context));

            // system roles list
            var roles = typeof(RoleName).GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Select(fi => ((Enum)fi.GetValue(null)).GetDescription()).ToList();

            // generates all system roles
            roles.ForEach(roleName => {
                //Creates Role if it does not exist
                if (!roleManager.RoleExists(roleName)) {
                    roleManager.Create(new TaskManagerRole(roleName));
                }
            });

            base.Seed(context);
        }
    }
}
