using System;
using TaskManager.Data.Contracts.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskManager.Data.Contracts.Identity;

namespace TaskManager.Data.Identity {
    public class TaskManagerUserStore :
        UserStore<TaskManagerUser, TaskManagerRole, Guid, TaskManagerUserLogin, TaskManagerUserRole, TaskManagerUserClaim> {
        public TaskManagerUserStore(ITaskManagerDbContext context) : base(context.DbContext) {
        }
    }
}
