using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using Members.OpinionBar.Components.Data_Layer;

namespace Members.NewOpinionBar.Web.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base


        /// <summary>
        /// Get Cinet Details
        /// </summary>
        public Client ClientDetails
        {
            get
            {
                if (Session["ClientDetails"] != null)
                {
                    return Session["ClientDetails"] as Client;
                }
                return new Client();
            }
            set
            {
                Session["ClientDetails"] = value;
            }
        }

        /// <summary>
        /// Get Referrer Url
        /// </summary>
        public string RefererUrl
        {
            get
            {
                if (Session["ReferrerUrl"] != null)
                {
                    return Convert.ToString(Session["ReferrerUrl"]);
                }
                return string.Empty;
            }
            set
            {
                Session["ReferrerUrl"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FriendId
        {
            get
            {
                if (Session["sessionfriendid"] != null)
                {
                    return Convert.ToInt32(Session["sessionfriendid"]);
                }
                return -1;
            }
            set
            {
                Session["sessionfriendid"] = value;
            }
        }

        /// <summary>
        /// Check Cookie 
        /// </summary>
        public bool CookiePresent
        {
            get
            {
                HttpCookie cookie = Request.Cookies.Get(Members.PrecisionSample.Common.Utils.Names.Cookie.Name);
                if (cookie != null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Cehck Cookie
        /// </summary>
        public HttpCookie Cookie
        {
            get
            {
                if (CookiePresent)
                {
                    HttpCookie cookie = Request.Cookies.Get(Members.PrecisionSample.Common.Utils.Names.Cookie.Name);
                    return cookie;
                }
                return null;
            }
        }

        ///// <summary>
        ///// Get Userr Data By User Name
        ///// </summary>
        ///// <param name="userName">UserGuid</param>
        ///// <returns></returns>
        //public User GetUserDetails(string userName)
        //{
        //    if (Session["User"] == null)
        //    {
        //        UserManager oUserManager = new UserManager();
        //        Session["User"] = oUserManager.GetUserData(userName);
        //    }
        //    else if (userName != null)
        //    {
        //        UserManager oUserManager = new UserManager();
        //        Session["User"] = oUserManager.GetUserData(userName);
        //    }
        //    return Session["User"] as User;
        //}

        /// <summary>
        /// Get Client Details by memberUlr
        /// </summary>
        /// <returns></returns>

        public Client GetClientDetails()
        {
            if (Session["ClientDetails"] == null)
            {
                var uri = new Uri(Request.Url.OriginalString.ToString());
                string Hosturl = uri.Scheme + "://" + uri.Host;
                UserDataServices oUserManager = new UserDataServices();
                Session["ClientDetails"] = oUserManager.GetClientDetailsByRid(Hosturl, null, null);
            }
            return Session["ClientDetails"] as Client;
        }

        public void ClearUserSession()
        {
            // Session["User"] = null;
            Session["ClientDetails"] = null;
        }


        /// <summary>
        /// Get IpAddress
        /// </summary>
        public string IpAddress
        {
            get
            {
                return Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
        }

        /// <summary>
        /// Get ReferrId 
        /// </summary>
        public string[] ReferrerIds
        {
            get
            {
                if (Session["referrerid"] == null)
                {
                    if (CookiePresent)
                    {
                        string refeererId = Cookie.Values[Members.PrecisionSample.Common.Utils.Names.Cookie.ReffererId].ToString();
                        Session["referrerid"] = (refeererId + "/////").ToString().Split('/');
                    }
                    else
                    {
                        if (Request.Url.Host == "surveydownline.com") //if Surveydownline site and no referrer id passed default set to -1
                        {
                            Session["referrerid"] = "-1/////".Split('/');
                        }
                        else // for other than survey downline site no default referrer id.
                        {
                            Session["referrerid"] = "/////".Split('/');
                        }
                    }
                }

                return Session["referrerid"] as string[];
            }
            set
            {
                Session["referrerid"] = value;
            }
        }

        //protected override void OnActionExecuting(HttpActionContext requestContext)
        //{

        //}
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Session.Abandon();

            this.GetClientDetails();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // ... log stuff after execution
        }


        /// <summary>
        /// DoLogin
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        public void DoLogin(Guid UserGuid)
        {
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(UserGuid.ToString(), false);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                // issue date
                DateTime oDateBegin = DateTime.Now;
                // expiry date (Set default to 60 mts)
                DateTime oDateEnd = oDateBegin.AddMinutes(60);
                // create a new ticket for each request
                FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(1, UserGuid.ToString(), oDateBegin, oDateEnd, false, UserGuid.ToString());
                cookie.Value = FormsAuthentication.Encrypt(newticket);
                HttpContext.Response.Cookies.Set(cookie);
                FormsAuthentication.SetAuthCookie(UserGuid.ToString(), false);
                // Create a new cookie just to rember the user name, don' t use the authentication cookie
                string sCookieName = "PS Member Cookie";
                HttpCookie accountNameCookie = new HttpCookie(sCookieName);
                Response.Cookies.Remove(sCookieName);
                Response.Cookies.Add(accountNameCookie);
                DateTime dtExpiry = DateTime.Now.AddYears(1);
                Response.Cookies[sCookieName].Expires = dtExpiry;
            }

        }
        #region Get Absolute Url
        /// <summary>
        /// Get Absolute Url
        /// </summary>
        /// <returns></returns>
        public string GetAbsoluteUrl()
        {
            var uri = new Uri(Request.Url.OriginalString.ToString());
            string Hosturl = uri.Scheme + "://" + uri.Host;
            return Hosturl;
        }
        #endregion
    }
}