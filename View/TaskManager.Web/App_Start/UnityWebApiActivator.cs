using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TaskManager.Web.UnityWebApiActivator), nameof(TaskManager.Web.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(TaskManager.Web.UnityWebApiActivator), nameof(TaskManager.Web.UnityWebApiActivator.Shutdown))]

namespace TaskManager.Web {
    /// <summary>
    ///     Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET. </summary>
    public static class UnityWebApiActivator {
        /// <summary>
        ///     Integrates Unity when the application starts. </summary>
        public static void Start() {
            // Use UnityHierarchicalDependencyResolver if you want to use
            // a new child container for each IHttpController resolution.
            var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            //var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        ///     Disposes the Unity container when the application is shut down. </summary>
        public static void Shutdown() {
            UnityConfig.GetConfiguredContainer().Dispose();
        }
    }
}