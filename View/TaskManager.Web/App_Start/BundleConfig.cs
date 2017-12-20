using System.Web.Optimization;

namespace TaskManager.Web {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new ScriptBundle("~/Scripts/Common")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/linq.js",
                    "~/Scripts/toastr.js",
                    "~/Scripts/semantic.js",
                    "~/Content/components/*.js",
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-route.js",
                    "~/Scripts/angular-sanitize.js")
                .IncludeDirectory(
                    "~/Scripts/app/api", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/directives", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/enums", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/models", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/controllers", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/", "*.js", false)
            );

            bundles.Add(new StyleBundle("~/Content/Common")
                .Include(
                    "~/Content/toastr.css",
                    "~/Content/semantic.css",
                    "~/Content/components/*.css",
                    "~/Content/Site.css"
                    ));

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
