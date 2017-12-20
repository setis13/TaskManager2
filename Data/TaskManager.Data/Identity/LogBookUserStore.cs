using System;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts.Context;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Data.Identity {
    public class TaskManagerUserStore :
        UserStore<TaskManagerUser, TaskManagerRole, Guid, TaskManagerUserLogin, TaskManagerUserRole, TaskManagerUserClaim> {
        public TaskManagerUserStore(ITaskManagerDbContext context) : base(context.DbContext) {
        }
    }
}
