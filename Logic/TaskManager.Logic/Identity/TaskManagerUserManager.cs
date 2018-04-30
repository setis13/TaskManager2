using System;
using Microsoft.AspNet.Identity;
using TaskManager.Data.Contracts.Identity;

namespace TaskManager.Logic.Identity {
    public class TaskManagerUserManager : UserManager<TaskManagerUser, Guid> {

        public TaskManagerUserManager(IUserStore<TaskManagerUser, Guid> store)
            : base(store) {

            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<TaskManagerUser, Guid>(this) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = false;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;
        }
    }
}