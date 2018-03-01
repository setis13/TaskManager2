using Microsoft.AspNet.Identity;
using System;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts {
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
        ///     Registers Service with type T </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="service">Service instance</param>
        void Register<T>(T service) where T : IService;

        /// <summary>
        ///     Gets Service by it's type </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        T GetService<T>() where T : IService;
    }
}