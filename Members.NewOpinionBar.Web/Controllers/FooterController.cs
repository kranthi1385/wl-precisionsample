using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Members.NewOpinionBar.Web.Controllers
{
    public class FooterController : Controller
    {
        // GET: Footer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MoreAbout()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/MoreAbout.cshtml");
        }
        public ActionResult TC()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/TC.cshtml");
        }
        public ActionResult Privacy()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/Privacy.cshtml");
        }
        public ActionResult ns()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/newsite.cshtml");
        }
        public ActionResult CookieSt()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/CookieSt.cshtml");
        }
        public ActionResult CookieSettings()
        {
            ViewBag.IsShowLogIn = 0;
            return View("~/Views/Footer/CookieSettings.cshtml");
        }
    }
}