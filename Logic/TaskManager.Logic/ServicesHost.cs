using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using TaskManager.Common.Identity;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Services.Base;
using TaskManager.Logic.Identity;

namespace TaskManager.Logic {
    /// <summary>
    ///     Services host implementation </summary>
    public class ServicesHost : IServicesHost {
        /// <summary>
        ///     Holds registered services </summary>
        private readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        /// <summary>
        ///     Gets Role Manager </summary>
        public RoleManager<TaskManagerRole, Guid> RoleManager { get; }
        /// <summary>
        ///     Gets User manager </summary>
        public UserManager<TaskManagerUser, Guid> UserManager { get; }

        public ServicesHost(TaskManagerRoleManager roleManager, TaskManagerUserManager userManager) {
            this.RoleManager = roleManager;
            this.UserManager = userManager;
        }

        /// <summary>
        ///     Registers Service with type T </summary>
        /// <typeparam name="T">Service Type</typeparam>
        /// <param name="service">Service instance</param>
        public void Register<T>(T service) where T : IService {
            if (!services.ContainsKey(typeof(T)))
                services.Add(typeof(T), service);
        }

        /// <summary>
        ///     Gets Service by it's type </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        public T GetService<T>() where T : IService {
            if (services.ContainsKey(typeof(T)))
                return (T)services[typeof(T)];
            return default(T);
        }
}
}