using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Members.OpinionBar.Web.Controllers
{
    public class MemberController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region
        public ActionResult confred(string ug, int rid)
        {
            int result = 0;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                //DoLogin(new Guid(ug));
                logger.Info("URL: " + Request.Url.AbsoluteUri + " | User GUID: " + ug + " | rid: " + rid + " | IP Address: " + HttpContext.Request.Headers["X-Forwarded-For"].ToString());
                UserManager oManager = new UserManager();
                result = oManager.UpdateIsVerified(ug, rid);
                if (result == 1)
                {
                    ViewBag.Msg = "You have successfully redeemed your reward. Your redemption will be verified and approved in 2 to 3 mins.";
                }
                else
                {
                    ViewBag.ErrorMsg = "Redemption request expired. Please redeem again.";
                }
            }
            return View("~/Views/Home/MemberVerify.cshtml");
        }
        #endregion
    }
}