using System.Web;
using System.Web.Optimization;

namespace TaskManager.Web {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/scripts/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
                "~/Scripts/toastr.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/styles/css").Include(
                "~/Content/toastr.css",
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/scripts/project").Include(
                "~/Scripts/app/controllers/project-controller.js",
                "~/Scripts/app/models/project-model.js"));

            bundles.Add(new ScriptBundle("~/scripts/dataTables").Include(
                "~/Scripts/jquery.dataTables.js"));

            bundles.Add(new StyleBundle("~/styles/dataTables").Include(
                "~/Content/jquery.dataTables.css"));

            bundles.Add(new ScriptBundle("~/scripts/api").Include(
                "~/Scripts/api/api.js"));

            bundles.Add(new ScriptBundle("~/scripts/app").Include(
                "~/Scripts/app/app.js"));

            bundles.Add(new ScriptBundle("~/scripts/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-resource.js"));
        }
    }
}
