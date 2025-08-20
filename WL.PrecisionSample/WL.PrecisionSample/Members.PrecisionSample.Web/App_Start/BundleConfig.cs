using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Members.PrecisionSample.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
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
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css", "~/Content/style.css", "~/Content/custom.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}