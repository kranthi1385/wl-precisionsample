using System;
using System.Web;
using System.Web.Optimization;

namespace Members.PrecisionSample.Clikflow.App_Start
{
    public class BundleConfig : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            //If we want to user angular.js file in entire application, we can define in this section.
            //bundles.Add(new ScriptBundle("~/bundles/angular").Include(
            //            "~/Scripts/angular.js"));
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
        "~/Scripts/angular.min.js",
        "~/Scripts/jquery-1.12.4.js",
        "~/Scripts/angular-sanitize.min.js",
        "~/Scripts/angular-translate.min.js",
        "~/Scripts/translate-pluggable-loader.js",
        "~/Scripts/angular-translate-loader-partial.js"

 ));

            BundleTable.EnableOptimizations = true;
        }

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
        }

        #endregion
    }
}
