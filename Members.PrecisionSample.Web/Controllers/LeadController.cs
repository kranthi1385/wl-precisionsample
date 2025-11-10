using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Members.PrecisionSample.Web.Controllers
{
    public class LeadController : Controller
    {
        LeadManager omanager = new LeadManager();
        // GET: Lead
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult start()
        {
            return View("/Views/lead/start.cshtml");
        }

        public ActionResult q()
        {
            return View("/Views/lead/q.cshtml");
        }

        public ActionResult end()
        {
            return View("/Views/lead/end.cshtml");
        }

        #region Get Orgnization Details
        /// <summary>
        /// Get ClientDetails
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [Route("GetOrgDetails")]
        public JsonResult GetOrgDetails()
        {
            Client oClient = new Client();
            oClient.ClientId = MemberIdentity.Client.ClientId;
            oClient.OrgLogo = MemberIdentity.Client.OrgLogo;
            if (MemberIdentity.Client.ClientId == 70)
            {
                oClient.OrgName = "OpinioNetwork";
            }
            else
            {
                oClient.OrgName = MemberIdentity.Client.OrgName;
            }
            oClient.Referrerid = MemberIdentity.Client.Referrerid;
            oClient.MemberUrl = MemberIdentity.Client.MemberUrl;
            oClient.Emailaddress = MemberIdentity.Client.Emailaddress;
            oClient.MgLoginPath = MemberIdentity.Client.MgLoginPath;
            oClient.Password = MemberIdentity.Client.Password;
            oClient.CopyrightYear = MemberIdentity.Client.CopyrightYear;
            oClient.Address = MemberIdentity.Client.Address;
            oClient.AboutusText = MemberIdentity.Client.AboutusText;
            oClient.StyleSheettheme = MemberIdentity.Client.StyleSheettheme;
            oClient.HomePageURL = MemberIdentity.Client.HomePageURL;
            oClient.IsPopUp = MemberIdentity.Client.IsPopUp;
            oClient.IsProfilePixel = MemberIdentity.Client.IsProfilePixel;
            oClient.IsSurveyPixel = MemberIdentity.Client.IsSurveyPixel;
            oClient.ProfileClickPixelUrl = MemberIdentity.Client.ProfileClickPixelUrl;
            oClient.SurveyClickPixelUrl = MemberIdentity.Client.SurveyClickPixelUrl;
            oClient.ProfileCompletePixelUrl = MemberIdentity.Client.ProfileCompletePixelUrl;
            oClient.SurveyCompletePixelUrl = MemberIdentity.Client.SurveyCompletePixelUrl;
            oClient.OrgTypeId = MemberIdentity.Client.OrgTypeId;
            oClient.IsStep1Enable = MemberIdentity.Client.IsStep1Enable; // Added on 9/26/2014 to Diable Step1 For some API /Social Partners.
            return Json(oClient, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Get Prescreener Question List By ProjectId
        /// <summary>
        /// Get User Prescree Questions
        /// </summary>
        /// <param name="ug">User Guid</param>
        /// <param name="lngId">LanguageId</param>
        /// <param name="uig">UserInvationGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetQuestions(Guid lid)
        {
            var pagedata = omanager.GetPSQuestions(lid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SaveUserPrescreenerptions
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvationGuid</param>
        /// <param name="sr">SortOrder</param>
        /// <param name="xml">LanguageId</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [Route("SaveUserPrescreenerOptions")]
        [HttpPost]
        public JsonResult SaveUserPrescreenerOptions(Guid lid, int qId, int sr, string xml)
        {
            var pagedata = omanager.SaveUserPrescreenerOptions(xml, lid, qId, sr);
            // var pagedata = objPsDataService.UpdateMobileNumber(ug, uig, phonenumber);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Step2 Details
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        [Route("GetStep2Details")]
        [HttpGet]
        public JsonResult GetStep2Details(Guid lid)
        {
            User objuser = new User();
            objuser = omanager.GetStep2Details(lid);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}