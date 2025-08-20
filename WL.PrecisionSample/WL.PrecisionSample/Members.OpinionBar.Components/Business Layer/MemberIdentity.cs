using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using System.Web;

namespace Members.OpinionBar.Components.Business_Layer
{
   public class MemberIdentity
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

