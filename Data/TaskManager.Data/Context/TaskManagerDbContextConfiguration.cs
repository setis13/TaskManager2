﻿using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using TaskManager.Common.Enums;
using TaskManager.Common.Extensions;
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

            // admin user
            var admin = TaskManagerUser.SystemAdmin;

            // generates all system roles
            roles.ForEach(roleName => {
                // creates Role if it does not exist
                if (!roleManager.RoleExists(roleName)) {
                    roleManager.Create(new TaskManagerRole(roleName));
                }
            });

            //// creates admin if doesn't exist
            //if (userManager.FindById(admin.Id) == null) {
            //    // createa User
            //    userManager.Create(admin, "123456");
            //    userManager.SetLockoutEnabled(admin.Id, false);
            //}

            // creates default database
            if (userManager.FindById(admin.Id) == null) {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "TaskManager.Data.script.sql";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream)) {
                    string result = reader.ReadToEnd();
                    context.DbContext.Database.ExecuteSqlCommand(result);
                }
            }

            base.Seed(context);
        }
    }
}
