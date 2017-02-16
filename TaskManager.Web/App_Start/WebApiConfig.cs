﻿using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using Newtonsoft.Json.Serialization;

namespace TaskManager.Web {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            config.DependencyResolver = resolver;

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
        }
    }
}