using System.Web;
using System.Web.Optimization;

namespace TaskManager.Web {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/toastr.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/api/api.js",
                "~/Scripts/app/app.js")
                .IncludeDirectory("~/Scripts/app/controllers/", "*.js", true));

            bundles.Add(new ScriptBundle("~/styles/common").Include(
                "~/Content/toastr.css",
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/jquery.dataTables.css"
            ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
