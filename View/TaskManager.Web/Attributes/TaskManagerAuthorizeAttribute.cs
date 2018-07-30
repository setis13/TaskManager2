using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.Web.Attributes {
    public class TaskManagerAuthorizeAttribute : AuthorizeAttribute {

        public string Controller { get; set; }

        public string Action { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext) {

            if (!filterContext.HttpContext.Request.IsAuthenticated) {
                // TODO: check roles
                // get context
                var context = filterContext.HttpContext.Request.RequestContext;

                // get route data
                var routeValues = context.RouteData.Values;

                // change route values
                routeValues["controller"] = "Account";
                routeValues["action"] = "Login";

                // get path based on changed route values collection
                var virtualPathData = RouteTable.Routes.Select(route => route.GetVirtualPath(context, routeValues)).FirstOrDefault(path => path != null);

                // setup redirection result
                filterContext.Result = new RedirectResult($"~/{virtualPathData?.VirtualPath ?? String.Empty}");
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}
