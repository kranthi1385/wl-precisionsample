using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Members.PrecisionSample.Components.Entities;


namespace Members.PrecisionSample.Components.Business_Layer
{
    public static class MemberIdentity
    {
        /// <summary>
        /// Get Current Clinet Details
        /// </summary>
        public static Client Client
        {
            get
            {
                if (HttpContext.Current.Session["ClientDetails"] != null)
                {
                    return HttpContext.Current.Session["ClientDetails"] as Client;
                }
                return new Client();
            }
        }
    }
}
