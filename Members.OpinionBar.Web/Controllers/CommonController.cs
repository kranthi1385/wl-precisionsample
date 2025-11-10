using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using Members.OpinionBar.Web.Models;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Data_Layer;
using System.Web.Security;
using Members.OpinionBar.Web.Filters;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace Members.OpinionBar.Web.Controllers
{
    public class CommonController : BaseController
    {

        string requestUrl = string.Empty;
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

        public void LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            //return RedirectToAction("LogIn", "Home");
        }
        // GET: Common
        public ActionResult Language()
        {
            //CommonManager oMananger = new CommonManager();
            //List<Continent> lstContinent = new List<Continent>();
            //lstContinent = oMananger.GetContientList();
            return View("~/Views/Home/Language.cshtml");
        }

        #region Get Country & States List
        [HttpGet]
        public JsonResult GetCountrysAndStates()
        {
            CommonManager oManager = new CommonManager();
            CountryAndStates oData = new CountryAndStates();
            var pagedata = oManager.GetCountrysAndStates();
            return Json(pagedata, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Update Language Code
        /// <summary>                       
        /// Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        [HttpPost]
        public string UpdateLanguageCode(User oUser, string LangCode, string UserGuid)
        {
            requestUrl = GetAbsoluteUrl();
            CommonManager oManager = new CommonManager();

            return oManager.UpdateLanguageCode(oUser, LangCode, requestUrl);
        }
        #endregion


        #region Get All Avaliable Ethnicites

        /// <summary>
        /// Get All Ethnicites
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEthnicityList()
        {
            CommonManager oManager = new CommonManager();
            if (ViewBag.LangCode == null)
                ViewBag.LangCode = "en";
            var pagedata = oManager.GetEthinicity(ViewBag.LangCode);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Avaliable Ethnicites

        /// <summary>
        /// Get All Ethnicites
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

        #region Set user Selected Language
        /// <summary>
        /// Set User Selected language
        /// </summary>
        /// <param name="langcode">Language Code</param>
        /// <returns></returns>
        public ActionResult SetLanguage(string cnname, string langcode)
        {
            new SetLanguage().SetLanguageCookie(cnname, langcode);
            return RedirectToAction("home", "LogIn");
        }
        #endregion

        #region Validate Google captcha
        /// <summary>
        /// Validate google captcha
        /// </summary>
        /// <param name="googleResponse">Google captcha response</param>
        /// <returns></returns>
        //[ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult ValidateCaptcha(string googleResponse)
        {
            int isValid = 0;
            //secret that was generated in key value pair
            //const string secret = "6LeYOw8UAAAAAFWJ2pPIS7D7_YQ1AQxC_6RfRSJ1"; //for psr.ob.com
            const string secret = "6LfVcForAAAAAHNXhuHtttyoUMVOxMNTfkPDKYGJ"; // ob.com
            string responseFromServer = "";
            WebRequest request = WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" + googleResponse);
            request.Method = "GET";
            using (WebResponse res = request.GetResponse())
            {
                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    responseFromServer = reader.ReadToEnd();
                }
            }

            if (responseFromServer != "")
            {
                CaptchaResponse oCaptchaRes = new CaptchaResponse();
                oCaptchaRes = JsonConvert.DeserializeObject<CaptchaResponse>(responseFromServer);
                if (oCaptchaRes.success)
                {
                    isValid = 1;
                }
                else
                {
                    isValid = 0;
                }


            }


            return Json(isValid, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}