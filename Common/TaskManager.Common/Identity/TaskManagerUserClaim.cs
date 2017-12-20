using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskManager.Common.Identity {
    public class TaskManagerUserClaim : IdentityUserClaim<Guid> {
    }
}
