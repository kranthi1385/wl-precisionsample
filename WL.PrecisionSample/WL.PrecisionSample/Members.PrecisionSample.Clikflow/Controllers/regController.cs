using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Clikflow.Filters;
using System.Configuration;

namespace Members.PrecisionSample.Clikflow.Controllers
{
    public class regController : Controller
    {
        ProfileQuestionBusinessService oManager = new ProfileQuestionBusinessService();
        ClickflowManager objCManager = new ClickflowManager();
        // GET: reg
        public ActionResult pii()
        {
            return View("/Views/pages/pprs.cshtml");
        }

        public ActionResult priterms()
        {
            return View("/Views/pages/priterms.cshtml");
        }

        #region GetQuestion
        /// <summary>
        /// GetQuestion
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="Ug"></param>
        /// <param name="Pl"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetQuestion(Guid uig, Guid ug, int pid, int tid, int usid, int cid, string dvtype)
        {
            PiiResponse request = new PiiResponse();
            string clientIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
            request = oManager.GetQuestion(uig, ug, pid, tid, usid, cid, clientIp,dvtype);
            return Json(request, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region Save Answers And Return Question
        /// <summary>
        /// Save Answers And Return Question
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="qid"></param>
        /// <param name="otext"></param>
        /// <param name="optId"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveResponse(Guid Uig, int qid, string otext, int optId, Guid Ug, int cid,int usid,string address1, string address2, string city, string zip,string dvtype)
        {
            PiiResponse response = new PiiResponse();
            string clientIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
            response = oManager.SaveResponse(Uig, qid, otext, optId, Ug, cid, usid, address1, address2, city, zip, clientIp,dvtype);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get User VerityScore
        /// <summary>
        /// Get User VerityScore 
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="uig"></param>
        [ValidateJsonAntiForgeryToken]
        public void SaveVerity(Guid ug, Guid uig, int cid)
        {
            Surveys oSurvey = new Surveys();
            int verityScore = -2;
            string verityId = null;
            int geoCorrelationFlag = -1;
            DateTime DOB = DateTime.MinValue;
            string country = "";
            User oUser = new Components.Entities.User();
            ClickflowManager oClickManager = new ClickflowManager();
            UserManager objManager = new UserManager();
            if (ug == uig)
            {
                oSurvey = objCManager.GetUserVerityDetails(ug, uig, cid, 0, 0, 0);
                com.imperium.verity.RespSvc client1 = new Members.PrecisionSample.Clikflow.com.imperium.verity.RespSvc();
                com.imperium.verity.ValidationResultsPlus9 vr1 = new Members.PrecisionSample.Clikflow.com.imperium.verity.ValidationResultsPlus9();
                com.imperium.verity.AuthHeader authentication1 = new Members.PrecisionSample.Clikflow.com.imperium.verity.AuthHeader();
                authentication1.Username = ConfigurationManager.AppSettings["VerityUserName"].ToString();
                authentication1.Password = ConfigurationManager.AppSettings["VerityPassword"].ToString();
                client1.AuthHeaderValue = authentication1;
                string[] verityDetails = oSurvey.VerityDetails.Split('|');
                DateTime dob = Convert.ToDateTime(verityDetails[verityDetails.Length - 1]);
                if (!string.IsNullOrEmpty(oSurvey.VerityDetails)) //Implement of first Survey Click.
                {
                    //Call the Verity+ webservice and receive response
                    if (!string.IsNullOrEmpty(verityDetails[0]) && !string.IsNullOrEmpty(verityDetails[1]) &&
                       !string.IsNullOrEmpty(verityDetails[2]) && !string.IsNullOrEmpty(verityDetails[4]) && dob.ToString("yyyyMM") != "190001")
                    {
                        vr1 = client1.ValidateDataPlus9(ConfigurationManager.AppSettings["VerityClientId"].ToString(),
                          verityDetails[0], verityDetails[1], verityDetails[2], verityDetails[3], verityDetails[4], verityDetails[5].Trim(),
                            dob.ToString("yyyyMM"), "", "U", "N", "N", Request.ServerVariables["REMOTE_ADDR"].ToString());

                        //Read all Verity 
                        verityScore = Convert.ToInt32(vr1.Score);
                        verityId = vr1.VID;
                        try
                        {
                            if (!string.IsNullOrEmpty(vr1.GeoCorrelationFlag))
                                geoCorrelationFlag = Convert.ToInt32(vr1.GeoCorrelationFlag);
                            else
                                geoCorrelationFlag = -1;
                        }
                        catch
                        {
                            geoCorrelationFlag = -1;
                        }
                        oManager.SaveVerityRepsonse(ug, uig, verityScore, verityId, geoCorrelationFlag, cid);
                    }
                }
            }
            else
            {
                oUser = objManager.GetUserData(ug.ToString(), null, cid);
                //Implement Verity Only For USA Members only.
                if (oUser.CountryId == 231)
                {
                    if (!string.IsNullOrEmpty(oUser.FirstName) && !string.IsNullOrEmpty(oUser.LastName) &&
                      !string.IsNullOrEmpty(oUser.Address1) && !string.IsNullOrEmpty(oUser.ZipCode) && !string.IsNullOrEmpty(oUser.DateOfBirth.ToString()))
                    {
                        DOB = Convert.ToDateTime(oUser.DateOfBirth);
                        country = "US";
                        com.imperium.verity.RespSvc client = new Members.PrecisionSample.Clikflow.com.imperium.verity.RespSvc();
                        com.imperium.verity.ValidationResultsPlus9 vr = new Members.PrecisionSample.Clikflow.com.imperium.verity.ValidationResultsPlus9();

                        com.imperium.verity.AuthHeader authentication = new Members.PrecisionSample.Clikflow.com.imperium.verity.AuthHeader();
                        authentication.Username = ConfigurationManager.AppSettings["VerityUserName"].ToString();
                        authentication.Password = ConfigurationManager.AppSettings["VerityPassword"].ToString();
                        client.AuthHeaderValue = authentication;
                        //Call the Verity+ webservice and receive response
                        vr = client.ValidateDataPlus9(ConfigurationManager.AppSettings["VerityClientId"].ToString(),
                            oUser.FirstName, oUser.LastName, oUser.Address1, oUser.Address2, oUser.ZipCode, country.Trim(),
                            DOB.ToString("yyyyMM"), "", "U", "N", "N", Request.ServerVariables["REMOTE_ADDR"].ToString());
                        verityScore = Convert.ToInt32(vr.Score);
                        verityId = vr.VID;
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

                        oManager.SaveVerityRepsonse(ug, uig, verityScore, verityId, geoCorrelationFlag, cid);
                    }
                }
            }
        }
        #endregion

        #region Save Answers And Return Question
        /// <summary>
        /// Save Answers And Return Question
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="qid"></param>
        /// <param name="otext"></param>
        /// <param name="optId"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public string gdprCompliancesave(string ug, int usid, int cid, string uid, string uig, string dvtype)
        {
            string clientIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
            return oManager.gdprCompliancesave(ug, usid, cid, uid, uig, clientIp,dvtype);
        }
        #endregion
    }
}
