using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Members.PrecisionSample.Web.Controllers
{
    public class Step2Controller : Controller
    {
        // GET: Step2
        public ActionResult Index()
        {
            return View();
        }

        #region Step2
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public ActionResult step2()
        {
            return View("/Views/Render/step2.cshtml");
        }
        #endregion
    }
}