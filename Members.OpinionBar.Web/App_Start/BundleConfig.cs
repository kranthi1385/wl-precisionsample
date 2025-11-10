using System.Web;
using System.Web.Optimization;

namespace Members.OpinionBar.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                  "~/Scripts/angular.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/angularval").Include(
                "~/Scripts/angular-translate.min.js",
                "~/Scripts/angular-translate-loader-partial.js",
                "~/Scripts/angular-cookies.min.js",
                "~/Scripts/translate-pluggable-loader.js",
                "~/Scripts/services/translationService.js",
                "~/Scripts/services/customServices.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css", "~/Content/style.css", "~/Content/custom.css"));
            BundleTable.EnableOptimizations = true;
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/jquery-ui.min.css",
                      "~/Content/jquery-ui.theme.min.css",
                      "~/Content/selectboxit.css"
                      ));
        }
    }
}
