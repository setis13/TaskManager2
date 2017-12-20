using System;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts.Context;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Data.Identity {
    public class TaskManagerRoleStore : RoleStore<TaskManagerRole, Guid, TaskManagerUserRole> {
        public TaskManagerRoleStore(ITaskManagerDbContext context) : base(context.DbContext) {
        }
    }
}
