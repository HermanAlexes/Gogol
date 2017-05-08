using System.Web;
using System.Web.Optimization;

namespace Gogol
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular/angular.js",
                      "~/Scripts/angular-mocks.js",
                      "~/Scripts/angular-animate/angular-animate.js",
                      "~/Scripts/angular-aria/angular-aria.js",
                      "~/Scripts/angular-material/angular-material.js",
                      "~/Scripts/Headroom.js",
                      "~/Scripts/jquery-2.1.1.min.js",
                      "~/Scripts/angular.headroom.js",
                      "~/Scripts/ng-infinite-scroll.min.js",
                      "~/Scripts/odometer.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/controllers").Include(
                 "~/Scripts/app.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/complete-styles.css",
                      "~/Content/angular-material.css",
                      "~/Content/angular-material.layout-attributes.css",
                      "~/Content/angular-material.layouts.css",
                      "~/Content/angular-material.layouts.ie-fixes.css",
                      "~/Content/odometer-0.4.6/themes/odometer-theme-default.css"
                     ));
        }
    }
}
