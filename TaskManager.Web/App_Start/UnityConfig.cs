using System;
using System.Linq;
using AutoMapper;
using AutoMapper.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using TaskManager.BLL;
using TaskManager.BLL.Contracts;
using TaskManager.BLL.Contracts.Services;
using TaskManager.BLL.Contracts.Services.Base;
using TaskManager.BLL.Mappings;
using TaskManager.BLL.Services;
using TaskManager.DAL;
using TaskManager.DAL.Context;
using TaskManager.DAL.Contracts;
using TaskManager.DAL.Contracts.Context;

namespace TaskManager.Web {
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer() {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container) {
            container.RegisterType<ITaskManagerDbContext, TaskManagerDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISubprojectService, SubprojectService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITaskService, TaskService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommentService, CommentService>(new HierarchicalLifetimeManager());

            container.RegisterMappingProfile<BllMappingProfile>();
            container.RegisterMapper();

            var mapper = container.Resolve<IMapper>();

            // register Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => {
                    var uow = new UnitOfWork(c.Resolve<ITaskManagerDbContext>());
                    return uow;
                }));

            // register services host
            container.RegisterType<IServicesHost, ServicesHost>(new HierarchicalLifetimeManager(),
                new InjectionFactory(c => {
                    var host = new ServicesHost();
                    var uow = c.Resolve<IUnitOfWork>();
                    c.Registrations.Where(
                            item =>
                                item.RegisteredType.GetInterfaces().Contains(typeof(IService)) &&
                                !item.MappedToType.IsInterface && !item.MappedToType.IsGenericType &&
                                !item.MappedToType.IsAbstract).ToList()
                            .ForEach(
                                item =>
                                    c.Resolve(item.RegisteredType,
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
