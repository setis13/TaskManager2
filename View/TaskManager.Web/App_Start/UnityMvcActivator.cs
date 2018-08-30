using System.Linq;
using System.Web.Mvc;
using TaskManager.Web;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebActivator), "Shutdown")]

namespace TaskManager.Web {
    /// <summary>
    ///     Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator {
        /// <summary>
        ///     Integrates Unity when the application starts.</summary>
        public static void Start() {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));

            //DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>
        ///     Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown() {
            UnityConfig.Container.Dispose();
        }
    }
}