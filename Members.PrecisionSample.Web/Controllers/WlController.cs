using Members.PrecisionSample.Components.Business_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Members.PrecisionSample.Web.Controllers
{
    public class WlController : BaseController
    {
        // GET: Wl
        public ActionResult Index()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View();
        }

        #region Mummyknows surveys step1 page
        public ActionResult Step1(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/MkStep1.cshtml");
            }
        }
        #endregion

        #region Mummyknows surveys step2 page
        public ActionResult Step2(string ug)
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/MkStep2.cshtml");
        }
        #endregion

        #region US Opinion Poll Page
        public ActionResult Uos(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/Uos.cshtml");
            }
        }
        #endregion

        #region Loquedigo surveys Lqe step1 page
        public ActionResult LqeStep1(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/LqeStep1.cshtml");
            }
        }
        #endregion

        #region Loquedigo surveys Lqe step2 page
        public ActionResult LqeStep2(string ug)
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/LqeStep2.cshtml");
        }
        #endregion

        #region We-Tell step1 page
        public ActionResult WtlStep1(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/WtlStep1.cshtml");
            }
        }
        #endregion

        #region We-Tell step2 page
        public ActionResult WtlStep2(string ug)
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/WtlStep2.cshtml");
        }
        #endregion

        #region Surveys 4Moms step1 page
        public ActionResult S4mStep1(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/S4mStep1.cshtml");
            }
        }
        #endregion

        #region Surveys 4Moms step2 page
        public ActionResult S4mStep2(string ug)
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/S4mStep2.cshtml");
        }
        #endregion

        #region Voter Insight page
        public ActionResult ViLogin(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/ViLogin.cshtml");
            }
        }
        #endregion

        #region Magazine Opinions Page
        public ActionResult moLogin(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/moLogin.cshtml");
            }
        }
        #endregion

        #region Panel Of Gamers
        public ActionResult pgslogin(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/pgslogin.cshtml");
            }
        }
        #endregion

        #region Opinion Squad
        public ActionResult oslogin(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/oslogin.cshtml");
            }
        }
        #endregion

        #region MarketPlace Step1
        public ActionResult Mplogin(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/Ms/Surveys");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/MpLogin.cshtml");
            }
        }
        #endregion

        #region MarketPlace Step2
        public ActionResult MpStep2()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/MpStep2.cshtml");
        }
        #endregion

        #region Site Map
        public ActionResult sm()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Wl/SiteMap.cshtml");
        }
        #endregion

        #region Research World Wide
        public ActionResult rwwStep1(string ug)
        {
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
            {
                DoLogin(new Guid(ug));
                return Redirect("~/hm/Home");
            }
            else
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                return View("/Views/Wl/rwwStep1.cshtml");
            }
        }
        #endregion
    }
}