using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Common.Security;
using System.Web.Mvc;

namespace Members.PrecisionSample.Web.Controllers
{
    [Authorize]
    public class ShController : BaseController
    {
        // GET: Sh
        public ActionResult SurveyHistory()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            ViewBag.UserId = Identity.Current.UserData.UserId;
            ViewBag.UserGuid = Identity.Current.UserData.UserGuid;
            return View("/Views/Render/SurveyHistory.cshtml");
        }
    }
}