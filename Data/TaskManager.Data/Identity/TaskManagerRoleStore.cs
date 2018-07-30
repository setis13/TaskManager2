using System;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskManager.Data.Context;

namespace TaskManager.Data.Identity {
    public class TaskManagerRoleStore : RoleStore<TaskManagerRole, Guid, TaskManagerUserRole> {
        public TaskManagerRoleStore(ITaskManagerDbContext context) : base(context.DbContext) {
        }
    }
}
