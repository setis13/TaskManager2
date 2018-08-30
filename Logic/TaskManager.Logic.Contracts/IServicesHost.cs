using Microsoft.AspNet.Identity;
using System;
using TaskManager.Data.Identity;
using TaskManager.Logic.Services;

namespace TaskManager.Logic {
    /// <summary>
    ///     Service Host contract </summary>
    public interface IServicesHost {
        /// <summary>
        ///     Gets Role Manager </summary>
        RoleManager<TaskManagerRole, Guid> RoleManager { get; }
        /// <summary>
        ///     Gets User manager </summary>
        UserManager<TaskManagerUser, Guid> UserManager { get; }

        /// <summary>
        ///     Gets Service by it's type </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        T GetService<T>() where T : IService;
    }
}