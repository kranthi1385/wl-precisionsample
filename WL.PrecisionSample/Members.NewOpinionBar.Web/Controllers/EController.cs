using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Configuration;
using Members.OpinionBar.Components.Business_Layer;

namespace Members.NewOpinionBar.Web.Controllers
{
    public class EController : BaseController
    {
        #region public Varialbes

        ExternalMembersManager oManager = new ExternalMembersManager();
        UserManager oUserManager = new UserManager();
        string requestUrl = string.Empty;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion
        // GET: E
        public ActionResult Index()
        {
            return View();
        }
        public void s(string rid, string sid, string fid, int? rcheckr, string txid, string trans_id, string fn, string ln, string em, string dob, string lname)
        {
            string ReferrerUrl = string.Empty;

            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            }
            if (!string.IsNullOrEmpty(fid))
            {
                FriendId = Convert.ToInt32(fid);
            }
            RefererUrl = ReferrerUrl;
            if (rid != string.Empty)
            {
                ReferrerIds = (rid + "/" + sid + "/" + txid + "/" + trans_id + "///").Split('/');
            }
            else
            {
                requestUrl = GetAbsoluteUrl();
                ReferrerIds = (MemberIdentity.Client.Referrerid + "////").Split('/');
            }
            if (string.IsNullOrEmpty(rid))
            {
                Response.Redirect("/e/login");
            }
            else
            {
                UserManager oUserManager = new UserManager();
                string url = oUserManager.GetLandingpageUrl(Convert.ToInt32(rid));
                if (!string.IsNullOrEmpty(url))
                {
                    url = url.Replace("%%referrer_id%%", rid.ToString());
                    if (sid != null)
                    {
                        url = url.Replace("%%sub_id%%", sid.ToString());
                    }
                    else
                    {
                        url = url.Replace("%%sub_id%%", string.Empty);
                    }

                    if (txid != null)
                    {
                        url = url.Replace("%%external_member_id%%", txid.ToString());
                    }
                    else
                    {
                        url = url.Replace("%%external_member_id%%", string.Empty);
                    }

                    if (lname != null)
                    {
                        url = url.Replace("%%language%%", lname.ToString());

                    }
                    else
                    {
                        url = url.Replace("%%language%%", "english");
                    }
                    url = url.Replace("%%app_id%%", ConfigurationManager.AppSettings["AppId"].ToString());
                    url = url.Replace("%%app_name%%", ConfigurationManager.AppSettings["AppName"].ToString());
                    url = url.Replace("%%transaction_id%%", trans_id);

                    if (rcheckr == 1) //we have added this to Switch Off Relevant & Verity check for members.
                    {
                        Response.Redirect(url + "?rcheckr=1");
                    }
                    else
                    {
                        //added on 23/6/2015 to pass additional paramaters like fn,ln.am,dob to url
                        if (!string.IsNullOrEmpty(fn))
                        {
                            url = url + "&fn=" + fn;
                        }
                        else
                        {
                            url = url + "&fn=";
                        }
                        if (!string.IsNullOrEmpty(ln))
                        {
                            url = url + "&ln=" + ln;
                        }
                        if (!string.IsNullOrEmpty(em))
                        {
                            url = url + "&em=" + em;
                        }
                        if (!string.IsNullOrEmpty(dob))
                        {
                            url = url + "&dob=" + dob;
                        }
                        Response.Redirect(url);
                    }
                }
                else
                {
                    Response.Redirect("/e/login");
                }
            }
        }

        public ActionResult s1(string rid, string sid, string fid, int? rcheckr, string txid, string trans_id, string fn, string ln, string em, string dob, string lname)
        {
            string ReferrerUrl = string.Empty;

            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            }
            if (!string.IsNullOrEmpty(fid))
            {
                FriendId = Convert.ToInt32(fid);
            }
            RefererUrl = ReferrerUrl;
            if (rid != string.Empty)
            {
                ReferrerIds = (rid + "/" + sid + "/" + txid + "/" + trans_id + "///").Split('/');
            }
            else
            {
                requestUrl = GetAbsoluteUrl();
                ReferrerIds = (MemberIdentity.Client.Referrerid + "////").Split('/');
            }
            if (string.IsNullOrEmpty(rid))
            {
                Response.Redirect("/e/login");
            }
            string ipAddress = string.Empty;
            ipAddress = Request.UserHostName;
            int clickId = oUserManager.InserClicks(ReferrerIds[0], ReferrerIds[1], ReferrerIds[2], IpAddress, RefererUrl, 1, 0, 0);
            ViewBag.IsShowLogIn = 1;
            return View("~/Views/E/s1.cshtml");
        }

        #region Login
        /// <summary>
        /// Redirect to member login page
        /// </summary>
        public void Login()
        {
            string redirectUrl = Request.Url.AbsoluteUri;
            if (redirectUrl.ToLower().Contains("http://www.opinionetwork.com") || redirectUrl.ToLower().Contains("https://www.opinionetwork.com") || redirectUrl.ToLower().Contains("http://opinionetwork.com")
               || redirectUrl.ToLower().Contains("https://opinionetwork.com") || redirectUrl.ToLower().Contains("http://s.opinionetwork.com"))
            {
                Response.Redirect("http://www.opinionetwork.com/hm.html");
            }
            else
            {
                string _rurl = Request.Params[0];
                string actualParam = "";
                string path = string.Empty;
                if (Request.RawUrl.Split('?').Length > 1)
                {
                    actualParam = Request.RawUrl.Split('?')[1];
                }
                path = MemberIdentity.Client.MgLoginPath + "?" + actualParam;
                Response.Redirect(path);
            }
        }
        #endregion
    }
}