using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.Web {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );
            routes.MapRoute(
                name: "Sign-up",
                url: "sign-up",
                defaults: new { controller = "Account", action = "Register" }
            );
            routes.MapRoute(
                name: "Profile",
                url: "profile",
                defaults: new { controller = "Account", action = "Profile1" }
            );
            /* Important! Home at first action index */
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "Company",
                url: "company",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "Projects",
                url: "projects",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "ReportSingle",
                url: "report-single",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "ReportPeriod",
                url: "report-period",
                defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
