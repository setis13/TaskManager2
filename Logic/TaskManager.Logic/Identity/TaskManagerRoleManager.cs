﻿using System;
using Microsoft.AspNet.Identity;
using TaskManager.Data.Identity;

namespace TaskManager.Logic.Identity {
    public class TaskManagerRoleManager : RoleManager<TaskManagerRole, Guid> {
        public TaskManagerRoleManager(IRoleStore<TaskManagerRole, Guid> roleStore) : base(roleStore) {
        }
    }
}