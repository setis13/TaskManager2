using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Data.Contracts.Identity {
    public class TaskManagerUserClaim : IdentityUserClaim<Guid> {
    }
}
