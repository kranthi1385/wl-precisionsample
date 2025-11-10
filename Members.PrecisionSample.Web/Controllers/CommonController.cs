using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Data_Layer;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Security;
using Newtonsoft.Json;
using Members.PrecisionSample.Web.Filters;

namespace Members.PrecisionSample.Web.Controllers
{
    public class CommonController : BaseController
    {
        #region public variables
        string requestUrl = string.Empty;
        #endregion
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }


        #region Get All Avaliable Ethnicites

        /// <summary>
        /// Get All Ethnicites
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEthnicityList(string langid = null)
        {
            CommonManager oManager = new CommonManager();
            if (langid == null)
            {
                langid = "en";
            }
            var pagedata = oManager.GetEthinicity(langid);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Avaliable Ethnicites

        /// <summary>
        /// Get country wise states
        /// </summary>
        /// <param name="Cid">Selected CountryId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStates(int Cid)
        {
            CommonManager oManager = new CommonManager();
            if (ViewBag.LangCode == null)
                ViewBag.LangCode = "en";
            var pagedata = oManager.GetStatesList(Cid, ViewBag.LangCode);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Language Options
        public JsonResult GetLanguageList()
        {
            CommonManager oManager = new CommonManager();
            var pagedata = oManager.GetLanguageList();
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get current domain details
        /// <summary>
        /// Get current domain details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCurrentDomainDetails()
        {
            requestUrl = GetAbsoluteUrl();
            UserDataServices oDataServer = new UserDataServices();
            var pagadata = oDataServer.GetClientDetailsByRid(requestUrl, null, null);
            return Json(MemberIdentity.Client, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Post data

        /// <summary>
        /// </summary>
        /// <param name="RequestURL"></param>
        /// <param name="LoginCredentials"></param>
        /// <returns></returns>

        public string PostRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            //string postData = LoginCredentials;
            //byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            //LoginRequest.ContentType = "application/x-www-form-urlencoded";
            //LoginRequest.ContentLength = data.Length;
            //Stream LoginRequestStream = LoginRequest.GetRequestStream();
            //LoginRequestStream.Write(data, 0, data.Length);
            //LoginRequestStream.Close();

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
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

        #region Validate Google captcha
        /// <summary>
        /// Validate google captcha
        /// </summary>
        /// <param name="googleResponse">Google captcha response</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]        
        public JsonResult ValidateCaptcha(string googleResponse)
        {
            try
            { 
                int isValid = 0;
                const string secret = "6LetblorAAAAAJY6VqbUK32nH3AZuhxlYb0HQq23";                
                using (var client = new WebClient())
                {
                    var googleReply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={googleResponse}");
                    var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(googleReply);
                    if (captchaResult != null && captchaResult.success)
                    {
                        isValid = 1;
                    }
                }
                return Json(isValid, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Server error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public void LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            //return RedirectToAction("LogIn", "Home");
        }

        #region Update Language Code
        /// <summary>
        /// Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        [HttpPost]
        public void UpdateLanguageCode(User oUser, string LangCode, string UserGuid)
        {
            requestUrl = GetAbsoluteUrl();
            CommonManager oManager = new CommonManager();

            oManager.UpdateLanguageCode(oUser, LangCode, requestUrl);
        }
        #endregion

        #region Get Languages

        /// <summary>
        /// Get Languages
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetObLang()
        {
            CommonManager oManager = new CommonManager();
            var pagedata = oManager.GetObLang();
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}