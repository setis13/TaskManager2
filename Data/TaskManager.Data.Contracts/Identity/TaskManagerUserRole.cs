using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Data.Contracts.Identity {
    public class TaskManagerUserRole : IdentityUserRole<Guid> {
    }
}
