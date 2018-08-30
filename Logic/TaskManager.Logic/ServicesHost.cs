using AutoMapper;
using CommonServiceLocator;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using TaskManager.Data;
using TaskManager.Data.Identity;
using TaskManager.Logic.Identity;
using TaskManager.Logic.Services;
using Unity;
using Unity.Resolution;

namespace TaskManager.Logic {
    /// <summary>
    ///     Services host implementation </summary>
    public class ServicesHost : IServicesHost {
        /// <summary>
        ///     Holds registered services </summary>
        private readonly Dictionary<Type, IService> services = new Dictionary<Type, IService>();

        /// <summary>
        ///     Gets Role Manager </summary>
        public RoleManager<TaskManagerRole, Guid> RoleManager => new TaskManagerRoleManager(UnitOfWork.RoleStore);
        /// <summary>
        ///     Gets User manager </summary>
        public UserManager<TaskManagerUser, Guid> UserManager => new TaskManagerUserManager(UnitOfWork.UserStore);
        /// <summary>
        ///     Gets Unit Of Work </summary>
        public IUnitOfWork UnitOfWork { get; }
        /// <summary>
        ///     Gets Mapper </summary>
        public IMapper Mapper { get; }

        public ServicesHost(IUnitOfWork unitOfWork, IMapper mapper) {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        /// <summary>
        ///     Gets Service by it's type </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Service instance</returns>
        public T GetService<T>() where T : IService {
            if (services.ContainsKey(typeof(T)) == false)
                services[typeof(T)] = ServiceLocator.Current.GetInstance<IUnityContainer>().Resolve<T>(
                    new ResolverOverride[] {
                        new ParameterOverride("host", this),
                        new ParameterOverride("unitOfWork", UnitOfWork),
                        new ParameterOverride("mapper", Mapper)
                    });
            return (T)services[typeof(T)];
        }
    }
}