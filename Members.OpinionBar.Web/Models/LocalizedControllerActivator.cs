using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Members.OpinionBar.Web.Models
{
    public class LocalizedControllerActivator : IControllerActivator
    {
        #region set resourec file for each request from cuurent culture cookie
        /// <summary>
        /// set resourec file for each request from cuurent culture cookie
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            //Get the {language} parameter in the RouteData
            HttpCookie cookie = HttpContext.Current.Request.Cookies["culture"];
            if (cookie != null)
            {
                try
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
                }
                catch (Exception e)
                {
                    //throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'.", lang));
                }
            }
            else
            {
                new SetLanguage().SetLanguageCookie("", "");
            }
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
        #endregion
    }
}