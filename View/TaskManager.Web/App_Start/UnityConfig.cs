using System;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using AutoMapper.Unity;
using TaskManager.Data;
using TaskManager.Data.Context;
using TaskManager.Data.Contracts;
using TaskManager.Data.Contracts.Context;
using TaskManager.Logic.Mappings;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using TaskManager.Logic;
using TaskManager.Logic.Contracts;
using TaskManager.Logic.Contracts.Services;
using TaskManager.Logic.Contracts.Services.Base;
using TaskManager.Logic.Services;
using TaskManager.Data.Identity;
using TaskManager.Logic.Identity;

namespace TaskManager.Web {
    /// <summary>
    ///     Specifies the Unity configuration for the main container. </summary>
    public static class UnityConfig {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> _container =
          new Lazy<IUnityContainer>(() => {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        ///     Gets the configured Unity container. </summary>
        public static IUnityContainer GetConfiguredContainer() {
            return _container.Value;
        }
        #endregion

        /// <summary>
        ///     Registers the type mappings with the Unity container. </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container) {
            // register Db context
            container.RegisterType<ITaskManagerDbContext, TaskManagerDbContext>(new HierarchicalLifetimeManager());
            // Register Services
            container.RegisterType<ISessionService, SessionService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompanyService, CompanyService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectsService, ProjectsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAlarmsService, AlarmsService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITaskService, TaskService>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFileService, FileService>(new HierarchicalLifetimeManager());

            // Register AutoMapper
            container.RegisterMapper();
            container.RegisterMappingProfile<BllMappingProfile>();
            var mapper = container.Resolve<IMapper>();

            // register Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => {
                    var context = c.Resolve<ITaskManagerDbContext>();
                    return new UnitOfWork(context, new TaskManagerRoleStore(context), new TaskManagerUserStore(context));
                }));
            // session
            container.RegisterType<IUserSession, UserSession>(new HierarchicalLifetimeManager());

            // register services host
            container.RegisterType<IServicesHost, ServicesHost>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => {
                    var uow = c.Resolve<IUnitOfWork>();
                    var host = new ServicesHost(new TaskManagerRoleManager(uow.RoleStore), new TaskManagerUserManager(uow.UserStore));
                    c.Registrations.ToList().Where(
                        item =>
                            item.RegisteredType.GetInterfaces().Contains(typeof(IService)) &&
                            !item.MappedToType.IsInterface && !item.MappedToType.IsGenericType &&
                            !item.MappedToType.IsAbstract).ToList()
                        .ForEach(item => c.Resolve(item.RegisteredType,
                        new ResolverOverride[] {
                                new ParameterOverride("servicesHost", host),
                                new ParameterOverride("unitOfWork", uow),
                                new ParameterOverride("mapper", mapper)
                        }));
                    return host;
                }));

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}