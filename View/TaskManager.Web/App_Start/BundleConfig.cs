using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using TaskManager.Common;

namespace TaskManager.Web {
    public class BundleConfig {
        protected sealed class TaskManagerStylesBundle : Bundle {
            public TaskManagerStylesBundle(string virtualPath) : base(virtualPath) {
                this.Builder = new NullBuilder();
                this.Orderer = new NullOrderer();
                Transforms.Add(new StyleTransformer());
            }
        }

        protected sealed class TaskManagerScriptsBundle : Bundle {
            public TaskManagerScriptsBundle(string virtualPath) : base(virtualPath) {
                this.Builder = new NullBuilder();
                this.Orderer = new AsIsBundleOrderer();
                Transforms.Add(new ScriptTransformer());
            }
        }

        protected class AsIsBundleOrderer : IBundleOrderer {
            private void Bubble<T>(T[] arr, Comparison<T> comparison) {
                for (int write = 0; write < arr.Length; write++) {
                    for (int sort = 0; sort < arr.Length - 1; sort++) {
                        var result = comparison(arr[sort], arr[sort + 1]);
                        if (result > 0) {
                            var temp = arr[sort + 1];
                            arr[sort + 1] = arr[sort];
                            arr[sort] = temp;
                        }
                    }
                }
            }

            /// <summary>
            ///     Sorts files. base classes before children </summary>
            public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files) {
                var arr = files.ToArray();
                Bubble(arr, (a, b) => {
                    var arr1 = a.IncludedVirtualPath.Split('\\');
                    var arr2 = b.IncludedVirtualPath.Split('\\');
                    if (arr1.Contains("base") && arr1.Length - 1 == arr2.Length && arr1[0] == arr2[0]) {
                        return -1;
                    }
                    if (arr2.Contains("base") && arr1.Length == arr2.Length - 1 && arr1[0] == arr2[0]) {
                        return 1;
                    }
                    return 0;
                });
                foreach (var file in arr) {
                    Debug.WriteLine(file.IncludedVirtualPath);
                }
                return arr.ToList();
            }
        }

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new TaskManagerScriptsBundle("~/Scripts/Common")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/moment-duration-format.js",
                    "~/Scripts/linq.js",
                    "~/Scripts/toastr.js",
                    "~/Scripts/semantic.js",
                    "~/Scripts/semantic-calendar.js",
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
                    "~/Scripts/app/models", "*.js", true)
                .IncludeDirectory(
                    "~/Scripts/app/controllers", "*.js", true)
                .IncludeDirectory(
                    "~/Scripts/app/", "*.js", false)
                .IncludeDirectory(
                    "~/Scripts/app/filters", "*.js", false)
            );

            bundles.Add(new TaskManagerStylesBundle("~/Content/Common")
                .Include(
                    "~/Content/toastr.css",
                    "~/Content/semantic.css",
                    "~/Content/semantic-calendar.css",
                    "~/Content/semantic-responsive.css",
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
