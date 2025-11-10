using Members.PrecisionSample.Common.Security;
using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Web.Filters;
using Members.PrecisionSample.Web.Utlis;
using System.Text;
using System.Net;
using System.IO;
using NLog;

namespace Members.PrecisionSample.Web.Controllers
{
    public class RgController : BaseController
    {
        private static NLog.Logger Logger = LogManager.GetCurrentClassLogger();
        // GET: Registration
        public ActionResult Index()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View();
        }

        #region Step2
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public ActionResult Step2()
        {
            return View("/Views/Registration/sdl/Step2.cshtml");
        }
        #endregion

        [Route("saveUser")]
        [HttpPost]

        public string saveUser(User user)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var response = client.PostAsJsonAsync("api/Member/Create", user).Result;
            return response.ToString();
        }

        #region Step1
        /// <summary>
        /// Step1
        /// </summary>
        /// <returns></returns>
        public ActionResult Step1()
        {
            return View("/Views/Registration/sdl/Step1.cshtml");
        }
        #endregion

        #region step1 data insert
        /// <summary>
        /// Friend Information
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Step1UserDataInsert(User user)
        {
            string host = Request.Url.Host;
            //User objUser = new User();
            UserManager objUserManager = new UserManager();
            objUserManager.Step1UserDataInsert(user, host);
            return Json(objUserManager, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region step1 data Get
        ///// <summary>
        ///// Friend Information
        ///// </summary>
        ///// <param name="UserId">UserId</param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonResult Step1UserGet()
        //{
        //    User objUser = new User();
        //    UserManager objUserManager = new UserManager();
        //    objUserManager.Step1UserGet(Convert.ToInt32(Identity.Current.UserData.UserGuid));
        //    return Json(objUserManager, JsonRequestBehavior.AllowGet);
        //}
        //#endregion


        public ActionResult Relevant()
        {
            return View("/views/rg/relevant.cshtml");
        }
        #region Get Top10 Questions
        /// <summary>
        /// Get Top10 Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        /// <param name="usg">UserStatusGuid</param>
        /// <returns></returns>
        public ActionResult Top10(Guid ug, Guid? uig, Guid? usg, int cid)
        {
            User oUser = new User();
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserData(ug.ToString(), null, cid);
            ViewBag.ltlAffiliateTracking = string.Empty;
            //Firing pixcel for the members not in 2step prcocss only.( split_flag = 0 for normal affilaite).
            //But this Changed we arefiring pixeles for 2 step affialites, also after completing the 2Step.
            if (oUser.SplitFlag == 0 && !oUser.IsFraud && oUser.Is_soi_pixel_fired == 0)
            {                                                   //.. | ..//
                                                                //Fire Pixel for only SOI Members , where the affilaite is of type SOI Cost Affialite added on 11/16/2011 by sandy.
                                                                //Fire Pixel for non fraud members
                AffiliateTrackingManager oATM = new AffiliateTrackingManager();
                List<AffiliateTrackingEntities> lstATE = new List<AffiliateTrackingEntities>();
                lstATE = oATM.GetTrackingDetails(oUser.RefferId, oUser.UserId, cid);
                string referrer = ConfigurationManager.AppSettings["AffilaitePixelUSHispanic"].ToString();//added on 03/15/2016 to fire pixels only for us hispanic affiliates

                if (oUser.RefferId != -1 && oUser.RefferId != 368 && oUser.RefferId != 369)
                {
                    for (int i = 0; i < lstATE.Count; i++)
                    {
                        if (lstATE[i].FirePixel > 0 && lstATE[i].PixelTypeId == 1)
                        {
                            //Fire Tp only members from US/UK/CA/AUS only
                            UserManager oUserManager = new UserManager();
                            string countrycode = oUserManager.GetCountryForIp(IpAddress);
                            if (countrycode == "US" || countrycode == "AU" || countrycode == "UK" || countrycode == "CA" || countrycode == "O1")
                            {
                                string pixelurl = Code.PEReplaceMents(lstATE[i].TrackingDetails, oUser, oUser.SubId2);
                                if (referrer.Contains(oUser.RefferId.ToString()))
                                {
                                    if (countrycode == "US" && oUser.EthnicityId == 3)
                                    {
                                        if (lstATE[i].CallbacktypeId == 1) //Pixel
                                        {
                                            if (lstATE[i].TrackingType == 'I')
                                            {
                                                ViewBag.ltlAffiliateTracking = pixelurl;
                                            }
                                            if (lstATE[i].TrackingType == 'J')
                                            {
                                                ViewBag.ltlAffiliateTracking = pixelurl;
                                            }
                                        }
                                        else if (lstATE[i].CallbacktypeId == 2) //Call back
                                        {
                                            PostRequest(pixelurl);
                                        }
                                        oUserManager.UpdateSOIPixelFiredStatusUpdate(oUser.UserId, oUser.RefferId, cid);
                                    }
                                }
                                else
                                {
                                    if (lstATE[i].CallbacktypeId == 1) //Pixel
                                    {

                                        if (lstATE[i].TrackingType == 'I')
                                        {
                                            ViewBag.ltlAffiliateTracking = pixelurl;
                                        }
                                        if (lstATE[i].TrackingType == 'J')
                                        {
                                            ViewBag.ltlAffiliateTracking = pixelurl;
                                        }
                                    }
                                    else if (lstATE[i].CallbacktypeId == 2) //Call back
                                    {
                                        PostRequest(pixelurl);
                                    }
                                    oUserManager.UpdateSOIPixelFiredStatusUpdate(oUser.UserId, oUser.RefferId, cid);
                                }
                            }

                        }
                    }
                }
            }
            return View("/views/rg/top10.cshtml");
        }
        #endregion

        #region optPage3 iframe
        /// <summary>
        /// optPage3 iframe
        /// </summary>
        /// <param name="ug"></param>
        /// <returns></returns>
        public ActionResult OptPage3(string ug)
        {
            User oUser = new User();
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            ViewBag.OrgLogo = MemberIdentity.Client.OrgLogo;
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserData(ug, null, MemberIdentity.Client.ClientId);
            string url = string.Empty;
            string phone1 = string.Empty;
            string phone2 = string.Empty;
            string phone3 = string.Empty;
            string gender = string.Empty;
            string[] dob = Convert.ToString(oUser.DateOfBirth).Split('/');
            string dob1 = oUser.DateOfBirth.ToString("yyyy-MM-dd");
            string dob2 = oUser.DateOfBirth.ToString("MM/dd/yyyy");
            string domain = Request.Url.Host;
            string[] dob3 = dob1.Split('-');
            string yyyy = dob3[0];
            string mm = dob3[1];
            string dd = dob3[2];
            string _address1 = oUser.Address1;
            string[] s = @"<,>,#,%,{,},|,\,^,~,[,],`".Split(',');
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (_address1.Contains(s[i]))
                {
                    _address1 = _address1.Replace(s[i], "");
                }
            }
            if (!string.IsNullOrEmpty(oUser.PhoneNumber))
            {
                if (oUser.PhoneNumber.Length > 9)
                {
                    phone1 = oUser.PhoneNumber.Substring(0, 3);
                    phone2 = oUser.PhoneNumber.Substring(3, 3);
                    phone3 = oUser.PhoneNumber.Substring(6, 4);
                }
                else
                {
                    phone1 = string.Empty;
                    phone2 = string.Empty;
                    phone3 = string.Empty;
                }
            }
            else
            {
                phone1 = string.Empty;
                phone2 = string.Empty;
                phone3 = string.Empty;
            }


            if (oUser.CountryId == 15 || oUser.CountryId == 229 || oUser.CountryId == 38)  //australia
            {
                //url = ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + UserDetails.UserGuid.ToString();
                Response.Redirect("/rg/sm?ug=" + ug.ToString());
            }
            else if (oUser.CountryId == 231)  // USA
            {
                url = "https://ads.ifficient.com/embedded?pubid=1009&srcid=2358&first=" +
                Server.UrlEncode(oUser.FirstName) + "&last=" + Server.UrlEncode(oUser.LastName) + "&email=" + Server.UrlEncode(oUser.EmailAddress) +
                    "&add1=" + Server.UrlEncode(_address1) + "&add2=" + Server.UrlEncode(oUser.Address2) +
                    "&city=" + Server.UrlEncode(oUser.City) + "&state=" + Server.UrlEncode(oUser.StateCode.Replace(" ", "").TrimEnd()) +
                    "&zip=" + Server.UrlEncode(oUser.ZipCode) + "&phone=" + Server.UrlEncode(oUser.PhoneNumber) + "&gender=" + Server.UrlEncode(oUser.Gender) + "&subid1=" + oUser.RefferId.ToString() +
                    "&dob=" + Server.UrlEncode(dob2) + "&rurl=" + Server.UrlEncode(domain) + "/Rg/offers.aspx?ug=" + Server.UrlEncode(oUser.UserGuid.ToString()) + "&isTest=n";
            }
            else
            {
                Response.Redirect("/rg/sm?ug=" + ug.ToString());
            }
            ViewBag.iframe = url;
            return View("/views/rg/optpage3.cshtml");
        }

        #endregion

        #region Showme surveys
        /// <summary>
        ///Showme surveys
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <returns></returns>
        public ActionResult Sm(string ug)
        {
            User oUser = new User();
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserData(ug, null, MemberIdentity.Client.ClientId);
            ViewBag.FirstName = oUser.FirstName;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            ViewBag.OrgLogo = MemberIdentity.Client.OrgLogo;
            DoLogin(new Guid(ug));
            return View("/views/rg/sm.cshtml");
        }
        #endregion

        #region Redirect ShowMeOffers
        /// <summary>
        ///  Redirect ShowMeOffers
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="pg">PerkGuid</param>
        /// <param name="src">Src</param>
        /// <returns></returns>
        public ActionResult Sm1(string ug, string pg, string src)
        {
            User oUser = new User();
            UserManager oUserManager = new UserManager();
            if (!string.IsNullOrEmpty(ug))
            {
                oUser = oUserManager.GetUserData(ug, null, MemberIdentity.Client.ClientId);
            }
            PerksManager oManager = new PerksManager();
            Perks oPerk = new Perks();
            Perks oPerk1 = new Perks();
            string perkCompleteDt = string.Empty;
            oPerk1 = oManager.GetPerkDetails(pg, ug);
            perkCompleteDt = oManager.GetPerkCompletedDate1(pg, oUser.UserId, ug, MemberIdentity.Client.ClientId);
            if (!string.IsNullOrEmpty(perkCompleteDt))
            {
                ViewBag.Message = "You have already Completed this Offer";

            }
            else
            {
                oPerk = oManager.InsertClickDate1(pg, oUser.UserId, src, ug, MemberIdentity.Client.ClientId);
                if (!string.IsNullOrEmpty(oPerk.PerkUrl))
                {
                    string url = Code.PEReplaceMents(oPerk.PerkUrl, oUser, oPerk.User2PerkGuid.ToString());
                    if (oPerk.IsPixelTracked)
                    {
                        HttpCookie cookie = new HttpCookie("user_offer");
                        cookie.Value = oPerk.User2PerkGuid.ToString();
                        TimeSpan ts = new TimeSpan(3, 0, 0, 0);
                        cookie.Expires = System.DateTime.Now + ts;
                        Response.Cookies.Add(cookie);
                    }
                    Response.Redirect(url.Trim());
                }
            }
            return View("/views/rg/sm1.cshtml");
        }
        #endregion

        #region GetMethod
        public string GetrdJson(string RequestURL, string token)
        {
            String rdjson = string.Empty;
            StreamReader sr = null;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(RequestURL);
                Request.Method = "GET";
                Request.Headers.Add("token", token);
                Request.ContentType = "application/json; charset=UTF-8";
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                sr = new StreamReader(Response.GetResponseStream());
                rdjson = sr.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //sr.Close();
            }

            return rdjson;
        }
        #endregion

        #region Save Relevant Information
        /// <summary>
        /// Save  Relevant Information
        /// </summary>
        /// <param name="ug">User Guid</param>
        /// <param name="score">Relevant Score</param>
        /// <param name="rvid">RelevantId</param>
        /// <param name="pscore">Profile Score</param>
        /// <param name="fpfscores">Fraud ProfileScore</param>
        /// <param name="isNew">is</param>
        /// <param name="rcheckeR"></param>
        /// <param name="cid">ClientId</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public void saverelevantinfo(string ug, int score, string rvid, int pfScore, string fpfScores, int isNew, int rchecker, int cid,string pid, string userId, string sessionId = null)
        {
            UserManager oManager = new UserManager();
            User objUser = new User();
            //string RequestURL = string.Empty;
            //RequestURL = System.Configuration.ConfigurationManager.AppSettings["RDefender"].ToString();
            //RequestURL = string.Format(RequestURL, ug, cid, geoLonLat, passParam);
            //string rdjson = GetrdJson(RequestURL, token);
            Logger.Info("Verisoul Session Id|" + sessionId);
            string userVerityInfo = oManager.GetVerityInformation(ug, cid);
            string _queryChallenge = "N";
            string verityId = null;
            int verityScore = -2;
            int geoCorrelationFlag = -1;
            bool verityDOBFail = false;
            string challengeId = string.Empty;
            com.imperium.verity.RespSvc client = new Members.PrecisionSample.Web.com.imperium.verity.RespSvc();
            com.imperium.verity.ValidationResultsPlus9 vr = new Members.PrecisionSample.Web.com.imperium.verity.ValidationResultsPlus9();
            com.imperium.verity.AuthHeader authentication = new Members.PrecisionSample.Web.com.imperium.verity.AuthHeader();
            authentication.Username = ConfigurationManager.AppSettings["VerityUserName"].ToString();
            authentication.Password = ConfigurationManager.AppSettings["VerityPassword"].ToString();
            client.AuthHeaderValue = authentication;
            client.Timeout = 6000;
            string[] verityDetails = userVerityInfo.Split('|');
            //Need to Call Verity 5 API
            _queryChallenge = "N";
            if (!string.IsNullOrEmpty(userVerityInfo)) //Implement of first Survey Click.
            {
                DateTime dob = Convert.ToDateTime(verityDetails[verityDetails.Length - 1]);
                //Call the Verity+ webservice and receive response
                if (!string.IsNullOrEmpty(verityDetails[0]) && !string.IsNullOrEmpty(verityDetails[1]) &&
                   !string.IsNullOrEmpty(verityDetails[2]) && !string.IsNullOrEmpty(verityDetails[4]) && dob.ToString("yyyyMM") != "190001")
                {
                    vr = client.ValidateDataPlus9(ConfigurationManager.AppSettings["VerityClientId"].ToString(),
                      verityDetails[0], verityDetails[1], verityDetails[2], verityDetails[3], verityDetails[4], verityDetails[5].Trim(),
                        dob.ToString("yyyyMM"), "", "U", "N", _queryChallenge, Request.ServerVariables["REMOTE_ADDR"].ToString());

                    //Read all Verity 
                    verityScore = Convert.ToInt32(vr.Score);
                    verityId = vr.VID;
                    challengeId = vr.Challenge.ChallengeID;
                    //added on 10/03/2016 to evaluate based on DOB from Verity
                    if (vr.MatchResults.DOB.ToLower() == "no match" && ConfigurationManager.AppSettings["VerityDOBON"].ToString() == "1")
                    {
                        verityScore = 2;
                        verityDOBFail = true;
                    }

                    //we are calling Verity 6 API Here.

                    try
                    {
                        if (!string.IsNullOrEmpty(vr.GeoCorrelationFlag))
                            geoCorrelationFlag = Convert.ToInt32(vr.GeoCorrelationFlag);
                        else
                            geoCorrelationFlag = -1;
                    }
                    catch
                    {
                        geoCorrelationFlag = -1;
                    }
                    string IP = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    string[] IpAddress = IP.Split('.');
                    if (verityScore >= 3 && geoCorrelationFlag != -1 && geoCorrelationFlag != 1)
                    {
                        if (IpAddress[0] == "10" || IpAddress[0] == "192")
                        {
                            geoCorrelationFlag = 1;
                        }
                    }
                    //oSurvey = oManager.SaveVerityQuestions(uig, ug, cid, verityScore, verityId, challengeId, geoCorrelationFlag, qstText1, qstText2, qstText3, optText1, optText2, optText3, verityDOBFail);
                }


                //So For Non US/No Verity Required Partner we do not return Verity data.
            }
            if (rchecker == 1) //Added to test Pixel Firing BY Excluding the Relevant Check :
            {
                objUser.RelevantProfileScore = 0;
                objUser.RelevantScore = 0;
                oManager.ReleventUpdateForSDLOrWL(new Guid(ug), score, pfScore, fpfScores, rvid, cid, verityId, verityScore, geoCorrelationFlag, verityDOBFail, userId,pid, sessionId);
            }
            else
            {
                objUser.UserGuid = new Guid(ug);
                objUser.RelevantProfileScore = pfScore;
                objUser.RelevantScore = score;
                objUser.RelevantId = rvid;
                objUser.FpfScore = fpfScores;
                oManager.ReleventUpdateForSDLOrWL(new Guid(ug), score, pfScore, fpfScores, rvid, cid, verityId, verityScore, geoCorrelationFlag, verityDOBFail, userId,pid, sessionId);
            }
        }
        #endregion


        #region Get ShowMe Surveys List
        /// <summary>
        /// Get ShowMe Surveys List
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetShowMeSurveysList(string ug)
        {
            PerksManager oPerksManager = new PerksManager();
            List<Surveys> lstPerks = oPerksManager.GetShowMeSurveysList(ug, MemberIdentity.Client.ClientId);
            return Json(lstPerks, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Top 10 Profile Questions
        /// <summary>
        /// Get Top 10 Profile Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult Top10Get(string ug, int cid)
        {
            List<ProfileQuestions> lstQuestion = new List<ProfileQuestions>();
            ProfileQuestionBusinessService objQuestionBl = new ProfileQuestionBusinessService();
            var Pagedata = objQuestionBl.GetquestionsforTop10(new Guid(ug), cid);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Top 10 Profiles Save
        /// <summary>
        /// Top 10 Profiles Save
        /// </summary>
        /// <param name="ug"> User Guid</param>
        /// <param name="rstext">Real answer score text</param>
        /// <param name="Rq1">Rq1</param>
        /// <param name="Rq2">Rq2</param>
        /// <param name="Rq3">Rq3</param>
        /// <param name="Rq4">Rq4</param>
        /// <param name="xml">profile xml data</param>
        [ValidateJsonAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public string Top10Save(string ug, string rstext, string Rq1, string Rq2, string Rq3, string Rq4, string rg, string xml, int cid)
        {
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
                List<ProfileQuestions> objQuestionSave = new List<ProfileQuestions>();
                ProfileQuestionBusinessService objQuestionBl = new ProfileQuestionBusinessService();
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
                        com.imperium.ra.raService client = new com.imperium.ra.raService();
                        com.imperium.ra.AuthHeader authentication = new com.imperium.ra.AuthHeader(); com.imperium.ra.AnalyzeAnswerResponse4 res;
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
                        ra1 = Rq1;
                        ra2 = Rq2;
                        ra3 = Rq3;
                        ra4 = Rq4;
                        res = client.AnalyzeAnswer5(ClientID, SurveyID, QuestionID, rstext, LanguageCode, ra1, ra2, ra3, ra4, ug.ToString(), EnagagedLength, "0", "0", "0", "0", "0", "0", "0", "US", "", "");
                        RealAnswerScore = Convert.ToInt32(res.RAS);
                        BadWordsFlag = res.BadWordsFlag;
                        BadPhraseFlag = res.BadPhraseFlag;
                        GarbageWordsFlag = res.GarbageWordsFlag;
                        NonEngagedFlag = res.NonEngagedFlag;
                        PastedTextFlag = res.PastedTextFlag;
                        RobotFlag = res.RobotFlag;
                        ErrorMessage = res.ErrorMsg;
                        memberUrl = objQuestionBl.Top10SaveOptions(xml, new Guid(ug), rstext, Rq1, Rq2, Rq3, Rq4, RealAnswerScore,
                                                         BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag, RobotFlag, ErrorMessage, cid);
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
                    memberUrl = objQuestionBl.Top10SaveOptions(xml, new Guid(ug), rstext, Rq1, Rq2, Rq3, Rq4, RealAnswerScore,
                                                     BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag, RobotFlag, ErrorMessage, cid);
                }
                //writer.Write(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return memberUrl;
        }

        #endregion

        #region Registration Step Update
        /// <summary>
        /// Registration Step Update
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="rgstep"></param>
        [HttpGet]
        public void RegistrationStepUpdate(Guid ug, string rgstep)
        {
            User oUser = new User();
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserData(ug.ToString(), null, MemberIdentity.Client.ClientId);
            oUser.RegistrationStep = rgstep;
            oUser.UserGuid = ug;
            UserManager omanger = new UserManager();
            omanger.UserRegistrationStepUpdate(oUser, MemberIdentity.Client.ClientId);
        }


        #endregion

        #region Help methods
        /// <summary>
        /// Helper Methods
        /// </summary>
        /// <param name="RequestURL"></param>
        /// <returns></returns>
        public string PostRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }
        #endregion

        public ActionResult olp()
        {
            return View("/Views/Rg/olp.cshtml");
        }
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult ExternalMemberByIdGet(string extMemGuid)
        {
            ExternalMembersManager obj = new ExternalMembersManager();
            if (!string.IsNullOrEmpty(extMemGuid) && extMemGuid != "undefined")
            {
                var pagedata = obj.ExternalMemberByIdGet(new Guid(extMemGuid));
                return Json(pagedata, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult RewardAndUserInsert(User objUser)
        {
            ExternalMembersManager obj = new ExternalMembersManager();
            var pagedata = obj.RewardAndUserInsert(objUser);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }

    }
}