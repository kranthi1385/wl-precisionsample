using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Prescreener.Filters;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using NLog;


namespace Members.PrecisionSample.Prescreener.Controllers
{
    public class PrsController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string logMessage = string.Empty;
        #region public variables
        PrescreenerManager oManager = new PrescreenerManager();
        #endregion

        public ActionResult Index()
        {
            return View();
        }
        #region Prescreener Action Methods
        public ActionResult Umq()
        {
            return View("/Views/Prescreener/umq.cshtml");
        }
        public ActionResult Pp()
        {
            return View("/Views/Prescreener/pp.cshtml");
        }
        public ActionResult Psl()
        {
            return View("/Views/Prescreener/psl.cshtml");
        }
        public ActionResult PrsQst()
        {
            //NLog.Logger logger = NLog.LogManager.GetLogger("serverlogger");
            //logger.Info("info server log message");

            return View("/Views/Prescreener/prs.cshtml");
        }
        public ActionResult Rep()
        {
            return View("/Views/Prescreener/rep.cshtml");
        }

        public ActionResult Ccpa()
        {
            return View("/Views/Prescreener/ccpa.cshtml");
        }

        public ActionResult Zip()
        {
            return View("/Views/Prescreener/Zip.cshtml");
        }
        public ActionResult Addvld()
        {
            return View("/Views/Prescreener/AddVld.cshtml");
        }
        #endregion

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

        #region Check UerMobileNumber
        /// <summary>
        /// Check User MobileNumber
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitation Guiud</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [Route("CheckUserMobileNumber")]
        public JsonResult CheckUserMobileNumber(Guid ug, Guid uig, int cid)
        {
            var pagedata = oManager.GetUserMobileNumber(ug, uig, cid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Question Object
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [Route("MobileQuestion")]
        public JsonResult MobileQuestion()
        {
            List<VerityEnhancedQuestions> objenhanedlist = new List<VerityEnhancedQuestions>();
            VerityEnhancedQuestions objEnchanceQuestion = new VerityEnhancedQuestions();
            objenhanedlist.Add(objEnchanceQuestion);
            var pagedata = objenhanedlist;
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Mobile Number
        /// <summary>
        ///  Update mobile number
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        /// <param name="phNumber">UserResponse</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("UpdateMobileNumber")]
        public JsonResult UpdateMobileNumber(Guid ug, int cid, Guid uig, string phNumber)
        {
            //string phonenumber = Request.InputStream[0];
            PrescreenerManager objPsService = new PrescreenerManager();
            var pagedata = oManager.UpdateMobileNumber(ug, cid, uig, phNumber);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get project selected languages
        /// <summary>
        /// Get project selected languages
        /// </summary>
        /// <param name="ug">User Guid</param>
        /// <param name="uig">User Invitation Guid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [Route("GetProjectSelectedLanguages")]
        public JsonResult GetPorjectLanguages(Guid ug, Guid uig, int cid)
        {
            Question oQuestions = new Question();
            oQuestions = oManager.GetProjectSelectedLanguages(ug, cid, uig);
            return Json(oQuestions, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetQuestions(Guid ug, int cid, Guid uig, int lngId)
        {
            var pagedata = oManager.GetPSQuestions(ug, cid, lngId, uig);
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
        [HttpPost]
        public JsonResult SaveUserPrescreenerOptions(Guid ug, Guid uig, int qId, int sr, int lngId, int cid, int? gdprRadio, string xml)
        {
            int gdpr = 0;
            if (string.IsNullOrEmpty(gdprRadio.ToString()))
            {
                gdpr = 0;
            }
            else
            {
                gdpr = Convert.ToInt32(gdprRadio);
            }
            var pagedata = oManager.SaveUserPrescreenerOptions(xml, ug, cid, uig, qId, sr, lngId, gdpr);
            // var pagedata = objPsDataService.UpdateMobileNumber(ug, uig, phonenumber);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SaveUserPrescreenerptions
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult TakeAnotherSurvey(Guid ug, Guid uig, int cid)
        {
            string surveyURL = string.Empty;
            string ipAddress = string.Empty;
            EndLinksManager oManager = new EndLinksManager();
            Surveys oSurveys = new Surveys();
            //var pagedata = oManager.TakeAnotherSurvey(ug, uig, "p", 1, cid);
            //to get top 1 survey based on device type TFS ID -- 6560,6613
            string u = string.Empty;
            u = Request.ServerVariables["HTTP_USER_AGENT"];
            ipAddress = Request.UserHostAddress;
            // var pagedata = objPsDataService.UpdateMobileNumber(ug, uig, phonenumber);
            //surveyURL = oManager.GetRouterSurveyURL(ug, uig, "p", cid, u, ipAddress);
            oSurveys.SurveyUrl = surveyURL;
            return Json(oSurveys, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Project Reward Details
        /// <summary>
        /// Get Project Reward Details
        /// </summary>
        /// <param name="uig">uig</param>
        /// <param name="ug">ug</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetProjectRewardDetails(Guid uig, Guid ug, int cid)
        {
            var pagadata = oManager.GetProjectRewardDetails(ug, cid, uig);
            return Json(pagadata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get profile prescreener
        /// <summary>
        /// get profile prescreener
        /// </summary>
        /// <param name="ug">User Guid</param>
        /// <param name="lngId">LanguageId</param>
        /// <param name="uig">UserInvationGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetProfilePrescreener(Guid ug, int cid, Guid uig)
        {
            var pagedata = oManager.GetProfilePrescreener(ug, cid, uig);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Profile Prescreener
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
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveProfilePrescreener(string ug, Guid uig, string rstext, string Rq1, string Rq2, string Rq3, string Rq4, string rg, string xml, int cid)
        {
            List<ProfileQuestions> objQuestionSave = new List<ProfileQuestions>();
            string _message = string.Empty;
            string url = string.Empty;
            int RealAnswerScore = 0;
            string BadWordsFlag = string.Empty;
            string BadPhraseFlag = string.Empty;
            string GarbageWordsFlag = string.Empty;
            string NonEngagedFlag = string.Empty;
            string PastedTextFlag = string.Empty;
            string RobotFlag = string.Empty;
            string ErrorMessage = string.Empty;
            string memberUrl = string.Empty;
            string DetectedLangCode = string.Empty;
            string RelatedConfidenceScore = string.Empty;
            string RepeatedWordsPct = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(rg))
                {
                    User oUser = new User();
                    UserManager oManager = new UserManager();
                    oUser = oManager.GetUserData(ug, null, cid);
                    oUser.RegistrationStep = rg;
                    oUser.UserGuid = new Guid(ug);
                    UserManager omanger = new UserManager();
                    omanger.UserRegistrationStepUpdate(oUser, cid);
                }
                PrescreenerManager objQuestionBl = new PrescreenerManager();
                if (!string.IsNullOrEmpty(rstext))
                {
                    try
                    {
                        string ClientID = string.Empty;
                        string SurveyID = string.Empty;
                        string QuestionID = string.Empty;
                        string ra1 = string.Empty;
                        string ra2 = string.Empty;
                        string ra3 = string.Empty;
                        string ra4 = string.Empty;
                        string LanguageCode = string.Empty;
                        string EnagagedLength = string.Empty;
                        com.imperium.ra1.raService client = new com.imperium.ra1.raService();
                        com.imperium.ra1.AuthHeader authentication = new com.imperium.ra1.AuthHeader(); com.imperium.ra1.AnalyzeAnswerResponse6 res;
                        // Set timeout to 6 seconds
                        client.Timeout = 6 * 1000;
                        //Fill Authentication Header
                        authentication.Username = ConfigurationManager.AppSettings["RealAnswerUserName"].ToString();
                        authentication.Password = ConfigurationManager.AppSettings["RealAnswerPassword"].ToString();
                        client.AuthHeaderValue = authentication;
                        ClientID = ConfigurationManager.AppSettings["ClientId"].ToString();
                        SurveyID = ConfigurationManager.AppSettings["SurveyId"].ToString(); ;
                        QuestionID = ConfigurationManager.AppSettings["RealAnswerQuestionId"].ToString();
                        LanguageCode = ConfigurationManager.AppSettings["LanguageCode"].ToString();
                        EnagagedLength = ConfigurationManager.AppSettings["EngagedLength"].ToString();
                        ra1 = getRaValue("responseField", Rq1);
                        ra2 = getRaValue("responseField", Rq2);
                        ra3 = getRaValue("responseField", Rq3);
                        ra4 = getRaValue("responseField", Rq4);
                        res = client.AnalyzeAnswer6(ClientID, SurveyID, QuestionID, rstext, LanguageCode, ra1, ra2, ra3, ra4, ug.ToString(), EnagagedLength, "0", "0", "0", "0", "0", "0", "0", "US", "", "", "1", "");
                        RealAnswerScore = Convert.ToInt32(res.RAS);
                        BadWordsFlag = res.BadWordsFlag;
                        BadPhraseFlag = res.BadPhraseFlag;
                        GarbageWordsFlag = res.GarbageWordsFlag;
                        NonEngagedFlag = res.NonEngagedFlag;
                        PastedTextFlag = res.PastedTextFlag;
                        RobotFlag = res.RobotFlag;
                        ErrorMessage = res.ErrorMsg;
                        DetectedLangCode = res.DetectedLanguageCode;
                        RelatedConfidenceScore = res.RelatedConfidenceScore;
                        RepeatedWordsPct = res.RepeatedWordsPct;
                        objQuestionSave = objQuestionBl.SaveProfilePrescreener(xml, new Guid(ug), uig, rstext, ra1, ra2, ra3, ra4, RealAnswerScore,
                                                         BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag, RobotFlag, ErrorMessage, cid, DetectedLangCode, RelatedConfidenceScore, RepeatedWordsPct);
                    }
                    catch (Exception ex)
                    {
                        string exception = string.Empty;

                        if (string.IsNullOrEmpty(ex.Message))
                        {
                            exception = ex.InnerException.ToString();
                        }
                        else
                        {
                            exception = ex.Message;
                        }
                    }
                }
                else
                {
                    objQuestionSave = objQuestionBl.SaveProfilePrescreener(xml, new Guid(ug), uig, rstext, Rq1, Rq2, Rq3, Rq4, RealAnswerScore,
                                                     BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag, RobotFlag, ErrorMessage, cid, DetectedLangCode, RelatedConfidenceScore, RepeatedWordsPct);
                }
                //writer.Write(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(objQuestionSave, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get invitation guid
        /// <summary>
        /// get invitation guid
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetInvitationGuid(Guid uig, int cid)
        {
            var pagedata = oManager.GetInvitationGuid(uig, cid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion


        private string getRaValue(string questionId, string raValueFromForm)
        {
            string value = null;
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(raValueFromForm);
            return dictionary.TryGetValue(questionId, out value) ? value : null;
        }

        #region Get Cookie Data
        /// <summary>
        /// get invitation guid
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult getCookieData(Guid ug, Guid uig, int cid)
        {
            var pagedata = oManager.GetCookieData(ug, uig, cid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Cookie Data
        /// <summary>
        /// get invitation guid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void SaveUserCookie(Guid ug, int cid, bool resid, bool tapid)
        {
            oManager.SaveUserCookie(ug, cid, resid, tapid);
        }
        #endregion

        #region Max mind geo ip fraud
        /// <summary>
        /// saveZipcode
        /// </summary>
        /// <param name="ug">User Guid</param>
        /// <param name="lngId">LanguageId</param>
        /// <param name="uig">UserInvationGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveZipcode(Guid ug, Guid uig, string zip, int cid)
        {
            logMessage = "Zip code: " + zip + "|user guid: " + ug + "|uig: " + uig + " | Quality Sentinel Fail-Postal Code Country Mismatch |";
            logger.Trace(logMessage);
            var pagedata = oManager.saveZipcode(ug, uig, zip, cid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Country & States List
        [HttpGet]
        public JsonResult GetCountrysAndStates(int orgId = 0)
        {
            CommonManager oManager = new CommonManager();
            CountryAndState oData = new CountryAndState();
            var pagedata = oManager.GetCountrysAndStates(orgId);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}