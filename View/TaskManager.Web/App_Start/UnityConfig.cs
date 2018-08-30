using System;
using AutoMapper;
using AutoMapper.Unity;
using TaskManager.Data;
using TaskManager.Data.Context;
using TaskManager.Logic.Mappings;
using TaskManager.Logic;
using TaskManager.Logic.Services;
using TaskManager.Data.Identity;
using Unity.Lifetime;
using Unity;
using Unity.Injection;
using CommonServiceLocator;
using Unity.ServiceLocation;

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
        public static IUnityContainer Container => _container.Value;
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
            container.RegisterType<ITaskManagerDbContext, TaskManagerDbContext>(new PerResolveLifetimeManager());
            // Register Services
            container.RegisterType<ISessionService, SessionService>(new PerResolveLifetimeManager());
            container.RegisterType<ICompanyService, CompanyService>(new PerResolveLifetimeManager());
            container.RegisterType<IProjectsService, ProjectsService>(new PerResolveLifetimeManager());
            container.RegisterType<IAlarmsService, AlarmsService>(new PerResolveLifetimeManager());
            container.RegisterType<ITaskService, TaskService>(new PerResolveLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager());
            container.RegisterType<IFileService, FileService>(new PerResolveLifetimeManager());

            // Register AutoMapper
            container.RegisterType<IMapper>(new ContainerControlledLifetimeManager(),
                        new InjectionFactory(c => {
                            var config = new MapperConfiguration(cfg => {
                                cfg.AddProfile<BllMappingProfile>();
                            });
                            return config.CreateMapper();
                        }));
            var mapper = container.Resolve<IMapper>();

            // register Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager()/*,
                new InjectionFactory(c => {
                    var context = c.Resolve<ITaskManagerDbContext>();
                    return new UnitOfWork(context, new TaskManagerRoleStore(context), new TaskManagerUserStore(context));
                })*/);
            // session
            container.RegisterType<IUserSession, UserSession>(new PerResolveLifetimeManager());

            // register services host
            container.RegisterType<IServicesHost>(new PerResolveLifetimeManager(), new InjectionFactory(c => new ServicesHost(c.Resolve<IUnitOfWork>(), mapper)));

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}