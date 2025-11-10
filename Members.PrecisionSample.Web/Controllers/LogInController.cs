using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using System.Web.Security;
using Members.PrecisionSample.Common.Security;
using System.Net.Http;
using System.Configuration;
using Members.PrecisionSample.Components.Business_Layer;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;


namespace Members.PrecisionSample.Web.Controllers
{
    public class LogInController : BaseController
    {
        // GET: LogIn
        public ActionResult Testing()
        {
            if (Identity.Current != null)
            {
                ViewBag.EmailAddress = Identity.Current.UserData.EmailAddress;
            }
            return View();
        }

        #region User Login
        public ActionResult Login(User oUser)
        {
            string result = "false";
            if (result == "true")
            {
                // redirect to your application home page or other page
                return RedirectToAction("OtherAction", "Controller");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The user name or password is incorrect");
                //  return View("/Views/LogIn.cshtml", oUser);
                return View("/Views/LogIn.cshtml", oUser);
            }
        }
        #endregion

        #region Forgot Password Page 
        public ActionResult ForgotPsw()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Render/ForgotPsw.cshtml");
        }
        #endregion

        #region Privacy Page
        public ActionResult p()
        {
            return View("/Views/Footer/Privacy.cshtml");
        }
        #endregion

        #region Terms Page
        public ActionResult t()
        {
            return View("/Views/Footer/Terms.cshtml");
        }
        #endregion

        #region Contact Us
        public ActionResult cu()
        {
            return View("/Views/Footer/ContactUs.cshtml");
        }
        #endregion

        #region About Page
        public ActionResult abt()
        {
            return View("/Views/Footer/About.cshtml");
        }
        #endregion

        #region FAQ Page
        public ActionResult faq()
        {
            return View("/Views/Footer/FAQ.cshtml");
        }
        #endregion

        #region Do Not Sell My Info Page
        public ActionResult dns()
        {
            return View("/Views/Footer/DoNotSellInfo.cshtml");
        }
        #endregion

        #region Widget Do Not Sell My Info Page
        public ActionResult wdns(string lc)
        {
            if (!string.IsNullOrEmpty(lc))
            {
                ViewBag.recaptchalangCode = lc;
            }
            else
            {
                ViewBag.recaptchalangCode = "en";
            }
            return View("/Views/Partner/DoNotSellInfo.cshtml");
        }
        #endregion

        #region Change Password 
        [HttpPost]
        public string ChangePassword( string NewPassword,string email)
        {
            string result = string.Empty;
            //if (Request.UrlReferrer != null)
            //{
            //    ReferrerUrl = Request.UrlReferrer.ToString();
            //}
            UserManager oManager = new UserManager();
            return result = oManager.ChangePassword(NewPassword,email);
        }
        #endregion

        #region SendResetLink 
        [HttpPost]
        public int SendresetLink( int orgid, int rid, string extmid)
        {
            int result = 0;
           
            UserManager oManager = new UserManager();
            return result = oManager.SendresetLink(orgid, rid, extmid);
        }
        #endregion
        #region Cancel Account
        public ActionResult ca()
        {
            return View("/Views/Footer/CancelAct.cshtml");
        }
        #endregion

        #region Forgot Password
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpPost]
        public void ForgetPassword(int campid, User objuser, string CustomAttribute)
        {
            UserManager oManager = new UserManager();
            oManager.ForgetPassword(objuser, campid, MemberIdentity.Client.Referrerid, CustomAttribute);
        }
        #endregion

        #region Get User Data
        [HttpGet]
        public JsonResult GetUserDataEmail(string EmailAddress)
        {
            User oUser = new User();
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserDataEmail(EmailAddress, MemberIdentity.Client.ClientId);
            return Json(oUser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Send Email
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="comments"></param>
        /// <param name="fromname"></param>
        [HttpPost]
        public int SendMail(string fromaddress, string fromname, string comments)
        {
            UserManager oManager = new UserManager();
            return oManager.SendEmail(fromaddress, fromname, comments);
        }
        #endregion

        #region linkedIn
        public void linkedinCallback(string code, string state, string error, string error_description)
        {
            if (state == "fooobar")
            {
                if (error == null || error == "")
                {
                    string APIURL = ConfigurationManager.AppSettings["LinkedinAPI"].ToString();
                    var result = PostRequest1(APIURL, "grant_type=authorization_code&code=" + code.ToString() + "&redirect_uri=http://dev.affiliate.sdl.com/Login/linkedinCallback" + "&client_id=78y5i0qq41i9rp&client_secret=xxC3E6vYlNOJSdJg");
                    var jsonContect = JObject.Parse(result);
                    string API = "https://api.linkedin.com/v2/me";
                    string Authorization = "Bearer " + jsonContect["access_token"].ToString();
                    var data = GetRequest(API, Authorization);
                    var memberdata = JObject.Parse(data);
                    var LastName = memberdata["lastName"]["localized"]["en_US"].ToString();
                    var FirstName = memberdata["firstName"]["localized"]["en_US"].ToString();
                    var country = memberdata["lastName"]["preferredLocale"]["country"].ToString();
                    var id = memberdata["id"].ToString();

                }
                else
                {

                }
            }
        }

        public string PostRequest(string postUrl, string LoginCredentials, string accesstoken)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            LoginRequest.Method = "POST";
            LoginRequest.Headers.Add("Authorization", accesstoken);
            LoginRequest.ContentType = "x-www-form-urlencoded";
            LoginRequest.ContentLength = data.Length;
            Stream LoginRequestStream = LoginRequest.GetRequestStream();
            LoginRequestStream.Write(data, 0, data.Length);
            LoginRequestStream.Close();

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        public string PostRequest1(string RequestURL, string LoginCredentials)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/x-www-form-urlencoded";
            LoginRequest.ContentLength = data.Length;
            Stream LoginRequestStream = LoginRequest.GetRequestStream();
            LoginRequestStream.Write(data, 0, data.Length);
            LoginRequestStream.Close();

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        public string GetRequest(string RequestURL, string LoginCredentials)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            LoginRequest.Headers.Add("Authorization", LoginCredentials);
            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        #endregion

        #region Save Do Not Sell My Info
        [HttpPost]
        public int SaveDoNotSellMyInfo(string fstName, string lstName, string email, string presite, int reqid, string reqname)
        {
            string ReferrerUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.ToString();
            }
            UserManager oManager = new UserManager();
            return oManager.SaveDoNotSellMyInfo(fstName, lstName, email, presite, reqid, reqname, MemberIdentity.Client.ClientId, ReferrerUrl);
        }
        #endregion

        #region Privacy Policy File Download
        [HttpGet]
        public void DownloadPrivacyPDF()
        {
            string Path = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["PrivacyPolicyFileDownload"]);
            string strFilePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["PrivacyPolicyFileDownload"]) + "//PrivacyPolicy.pdf";
            FileInfo TragetFile = new FileInfo(strFilePath);
            if (TragetFile.Exists)
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=PrivacyPolicy.pdf");
                const int bufferLength = 10000;
                byte[] buffer = new Byte[bufferLength];
                int length = 0;
                Stream download = null;
                try
                {
                    download = new FileStream(Server.MapPath("~/Upload/PrivacyPolicy/PrivacyPolicy.pdf"),
                                                                   FileMode.Open,
                                                                   FileAccess.Read);
                    do
                    {
                        if (Response.IsClientConnected)
                        {
                            length = download.Read(buffer, 0, bufferLength);
                            Response.OutputStream.Write(buffer, 0, length);
                            buffer = new Byte[bufferLength];
                        }
                        else
                        {
                            length = -1;
                        }
                    }
                    while (length > 0);
                    Response.Flush();
                    Response.End();
                }
                finally
                {
                    if (download != null)
                        download.Close();
                }
            }

        }
        #endregion

        #region CookieStatement
        public ActionResult CookieSt()
        {
            return View("~/Views/Footer/CookieSt.cshtml");
        }
        #endregion

        #region CookieSettings
        public ActionResult CookieSettings()
        {
            return View("~/Views/Footer/CookieSettings.cshtml");
        }
        #endregion
    }
}