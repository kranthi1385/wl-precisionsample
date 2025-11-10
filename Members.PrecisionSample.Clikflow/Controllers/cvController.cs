using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Configuration;
using Members.PrecisionSample.Clikflow.Filters;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using MaxMind.MinFraud;
using MaxMind.MinFraud.Request;
using NLog;

namespace Members.PrecisionSample.Clikflow.Controllers
{
    public class cvController : Controller
    {
        #region public variables
        ClickflowManager oManager = new ClickflowManager();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string logMessage = string.Empty;
        #endregion
        // GET: cv
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult v5()
        {
            //Render the View.
            return View("/Views/pages/cv.cshtml");
        }

        public ActionResult v6()
        {
            //Render the View.
            return View("/Views/pages/cvq.cshtml");
        }

        #region Check Verity Questiond
        /// <summary>
        /// Check verity and save verity questiions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        //[Route("VerityCheck")]
        public JsonResult VerityCheck(Guid ug, Guid uig, int cid, int pid, int tid, int usid)
        {
            //Render the View.
            DateTime datOfBirth = DateTime.MinValue;
            string verityId = null;
            int verityScore = -2;
            int geoCorrelationFlag = -1;
            bool verityDOBFail = false;
            string challengeId = string.Empty;
            Surveys oSurvey = new Surveys();
            User oFCUser = new User();
            string qstText1 = string.Empty;
            string qstText2 = string.Empty;
            string qstText3 = string.Empty;
            string optText1 = string.Empty;
            string optText2 = string.Empty;
            string optText3 = string.Empty;
            string _queryChallenge = "N";
            oSurvey = oManager.GetUserVerityDetails(ug, uig, cid, pid, tid, usid);
            try
            {
                if (oSurvey.VerityScore >= 0 && oSurvey.VerityScore <= 4)
                {
                    //We need not to any Verity 5 
                }
                if (oSurvey.VerityScore == 5 && !string.IsNullOrEmpty(oSurvey.ChallengeId))
                {
                    //We need not to do any Verity 6.
                }

                else if (oSurvey.VerityScore == -2 || (oSurvey.VerityScore == 5 && string.IsNullOrEmpty(oSurvey.ChallengeId)))
                {
                    com.imperium.verity.RespSvc client = new Members.PrecisionSample.Clikflow.com.imperium.verity.RespSvc();
                    com.imperium.verity.ValidationResultsPlus9 vr = new Members.PrecisionSample.Clikflow.com.imperium.verity.ValidationResultsPlus9();
                    com.imperium.verity.AuthHeader authentication = new Members.PrecisionSample.Clikflow.com.imperium.verity.AuthHeader();
                    authentication.Username = ConfigurationManager.AppSettings["VerityUserName"].ToString();
                    authentication.Password = ConfigurationManager.AppSettings["VerityPassword"].ToString();
                    client.AuthHeaderValue = authentication;
                    client.Timeout = 6000;
                    string[] verityDetails = oSurvey.VerityDetails.Split('|');
                    DateTime dob = Convert.ToDateTime(verityDetails[verityDetails.Length - 1]);

                    //Need to Call Verity 5 API
                    if (oSurvey.VerityScore == -2)
                    {
                        _queryChallenge = "N";
                    }

                    //Need to Call Verity 6 API
                    if (oSurvey.VerityScore == 5 && string.IsNullOrEmpty(oSurvey.ChallengeId))
                    {
                        _queryChallenge = "Y";
                    }

                    if (!string.IsNullOrEmpty(oSurvey.VerityDetails)) //Implement of first Survey Click.
                    {
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
                            if ((oSurvey.VerityScore == 5 || verityScore == 5) && string.IsNullOrEmpty(oSurvey.ChallengeId))
                            {
                                challengeId = vr.Challenge.ChallengeID;
                                qstText1 = vr.Challenge.QuestionText1;
                                qstText2 = vr.Challenge.QuestionText2;
                                qstText3 = vr.Challenge.QuestionText3;
                                optText1 = vr.Challenge.AnswerChoices1;
                                optText2 = vr.Challenge.AnswerChoices2;
                                optText3 = vr.Challenge.AnswerChoices3;
                            }
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

                            oSurvey = oManager.SaveVerityQuestions(uig, ug, cid, verityScore, verityId, challengeId, geoCorrelationFlag, qstText1, qstText2, qstText3, optText1, optText2, optText3, verityDOBFail);
                        }
                    }

                    //So For Non US/No Verity Required Partner we do not return Verity data.
                }

            }
            catch
            {

            }

            return Json(oSurvey, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get VerityQuestions
        /// <summary>
        /// Get Verity Enchanced Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        [Route("GetVerityQUestions")]
        public JsonResult GetVerityQuestions(Guid uig, Guid ug, int cid, int pid, int tid, int usid, string dvtype)
        {
            List<VerityChallengeResponse> oQuestions = oManager.GetVerityQuestions(uig, ug, cid, pid, tid, usid, dvtype);
            return Json(oQuestions, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Save VerityChallenge Questions
        /// <summary>
        /// Save Verrity Challange Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        /// <param name="oQuestions">QuestionsList</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("SaveVerityQuestions")]
        public JsonResult SaveVerityQuestions(Guid ug, Guid uig, int cid, string dvtype, List<VerityEnhancedQuestions> oQuestions)
        {
            VerityChallengeResponse oResponse = oManager.SaveChallangeQuestionResponse(ug, cid, uig, oQuestions, dvtype);
            return Json(oResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region skip verity questions
        /// <summary>
        /// Skip Verity Enchance Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitatinGuid</param>
        [ValidateJsonAntiForgeryToken]
        public void SkipVerityQuestions(Guid ug, Guid uig)
        {
            oManager.SkipVerityQuestions(ug, uig);
        }
        #endregion

        #region Minfraud API
        public async Task<dynamic> MinFraudAsync(String Ipaddress, string UserAgent)
        {
            var transaction = new Transaction(
                device: new Device(
                    ipAddress: System.Net.IPAddress.Parse(Ipaddress),
                    userAgent: UserAgent,
                    acceptLanguage: "en-US,en;q=0.8"
                )
            );
            int AccountID = Convert.ToInt32(ConfigurationManager.AppSettings["MaxmindAccountID"]);
            string Key = ConfigurationManager.AppSettings["Maxmindkey"].ToString();
            using (var client = new WebServiceClient(AccountID, Key))
            {
                // Use `InsightsAsync` if querying Insights
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var response = client.ScoreAsync(transaction);
                response.Wait();
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new IPConverter());
                var json = JsonConvert.SerializeObject(response, Formatting.Indented, jsonSettings);// json string
                dynamic output = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return output;
            }
        }
        #endregion

    }
}