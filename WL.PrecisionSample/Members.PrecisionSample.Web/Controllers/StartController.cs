using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Members.PrecisionSample.Web.Controllers
{
    public class StartController : BaseController
    {
        // GET: Start
        public ActionResult Index(string rid, string sid, string txid, string transId, int fid, int rcheckr, string fn, string ln, string em, string dob)
        {
            #region set cookie values
            if (rid != string.Empty)
            {
                ReferrerIds = (rid + "/" + sid + "/" + txid + "/" + transId + "///").Split('/');
            }
            else
            {
                ReferrerIds = ("-1////").Split('/');
            }
            FriendId = fid;
            RefererUrl = (Request.UrlReferrer != null) ? Request.UrlReferrer.AbsoluteUri : string.Empty;
            if (rid == "-1" || string.IsNullOrEmpty(rid))
            {
                int orgId1 = MemberIdentity.Client.ClientId;
                ViewBag.CountryCode = "en";
                User objUser1 = new User();
                return Redirect("~/Home/LogIn");
            }
            else
            {
                //Get the landing page for the Referrer
                CommonManager objCommonManager = new CommonManager();
                string url = objCommonManager.GetLandingpageUrl(Convert.ToInt32(rid));
                if (!string.IsNullOrEmpty(url))
                {
                    url = url.Replace("%%referrer_id%%", rid);
                    url = url.Replace("%%sub_id%%", sid);
                    url = url.Replace("%%external_member_id%%", txid);
                    url = url.Replace("%%app_id%%", ConfigurationManager.AppSettings["AppId"].ToString());
                    url = url.Replace("%%app_name%%", ConfigurationManager.AppSettings["AppName"].ToString());
                    url = url.Replace("%%transaction_id%%", transId);
                    if (rcheckr == 1) //To Switch Off Relevant & Verity check for members.
                    {
                        Response.Redirect(url + "?rcheckr=1");
                    }
                    else
                    {
                        //pass additional paramaters like fn,ln.am,dob to url
                        if (!string.IsNullOrEmpty(fn))
                        {
                            url = url + "?fn=" + fn;
                        }
                        else
                        {
                            url = url + "?fn=";
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
            }
            return RedirectToAction("Index", "Home");
            #endregion
        }
    }
}