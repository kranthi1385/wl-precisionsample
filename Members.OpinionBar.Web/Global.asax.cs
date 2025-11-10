using Members.OpinionBar.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using Members.PrecisionSample.Common.Security;

namespace Members.OpinionBar.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (this.Context.Request.IsAuthenticated)
            {
                // retrieve user's identity from httpcontext user
                FormsIdentity identity = (FormsIdentity)this.Context.User.Identity;
                //  extract the userid from the ticket (it is populated by the log in module)
                Guid userId = new Guid(identity.Ticket.Name);
                //  if the ticket is not expired, recreate it
                this.Context.User = new GenericPrincipal(new Identity(identity.Ticket.Name, userId, Request.ServerVariables["HTTP_HOST"].ToString().ToLower(), PerSessionUserDataProvider.Instance), null);
            }
        }
        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {

        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("X-Frame-Options");
            Response.AddHeader("X-Frame-Options", "AllowAll");

        }

    }
}
