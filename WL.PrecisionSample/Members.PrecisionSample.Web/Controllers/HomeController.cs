using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Net.Http;
using System.Configuration;
using System.Web.Services;
using System.Text;
using System.Net;
using System.IO;
using Members.PrecisionSample.Common.Security;
using Newtonsoft.Json;
using Members.PrecisionSample.Web.Filters;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using NLog;

namespace Members.PrecisionSample.Web.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        UserManager oManager = new UserManager();

        #region LogIn Action method
        /// <summary>
        /// LogIn Action Method
        /// </summary>
        /// <returns></returns>
        // GET: Home
        //public ActionResult LogIn()
        //{ 
        public ActionResult LogIn(string ug)
        {
            Boolean IsExists = false;
            string redirectUrl = Request.Url.AbsoluteUri;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            IsExists = oManager.CheckMemberExist(ug, MemberIdentity.Client.ClientId);
            if (ug != null && (ug.Length == 36 || ug.Length == 32) && IsExists)
            {
                DoLogin(new Guid(ug));
                if (redirectUrl.Contains("ReturnUrl"))
                {
                    string actualParam = "";
                    string path = string.Empty;
                    if (Request.RawUrl.Split('=').Length > 1)
                    {
                        actualParam = Request.RawUrl.Split('=')[1];
                    }

                    path = actualParam;
                    return Redirect(HttpUtility.UrlDecode(path));
                }
                else
                {
                    return Redirect("~/hm/Home");
                }
            }
            else
            {
                int orgId = MemberIdentity.Client.ClientId;
                ViewBag.CountryCode = "en";
                User objUser = new User();
                return View("/Views/Home/LogIn.cshtml", objUser);
            }

        }
        #endregion

        #region  Member LogIn Check
        /// <summary>
        /// Login Member validate method
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult LogIn(string email, string psw)
        {
            string host = Request.Url.Host;
            //    HttpClient client = new HttpClient();
            //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            //    var response = client.PostAsJsonAsync("api/Member/UserLogin/", ologin).Result;
            //    return response.ToString();
            User oUser = new Components.Entities.User();
            UserManager oManager = new UserManager();
            oUser = oManager.LoginCheck(email, psw, host, MemberIdentity.Client.ClientId);
            if (MemberIdentity.Client.ClientId == 383 && oUser.UserGuid != Guid.Empty)
            {
                DoLogin(oUser.UserGuid);
                return Json("/Ms/Surveys", JsonRequestBehavior.AllowGet);
            }
            else if (oUser.UserGuid != Guid.Empty && MemberIdentity.Client.ClientId != 383)
            {
                DoLogin(oUser.UserGuid);
                return Json("/hm/Home", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        #endregion 

        #region Insert User Click
        /// <summary>
        ///  Insert User Click
        /// </summary>
        /// <returns></returns>
        [Route("GetUserData")]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetUserData()
        {
            User oUser = new User();
            int clickId = 0;
            Random onumber = new Random();
            int numer = onumber.Next();
            oUser.HitId = numer;
            if (!CookiePresent)
            {
                //flashplaceholder.Visible = true;
                UserManager oManager = new UserManager();
                if (string.IsNullOrEmpty(ReferrerIds[0].ToString()))
                {
                    Client ouserClient = new Client();
                    ouserClient = GetClientDetails();
                    oUser.RefferId = ouserClient.Referrerid;
                    clickId = oManager.InserClicks(oUser.RefferId.ToString(), ReferrerIds[1], ReferrerIds[2], IpAddress, RefererUrl, 1, 0, 0);
                }
                else
                {
                    //inserting affiliate hits
                    clickId = oManager.InserClicks(ReferrerIds[0], ReferrerIds[1], ReferrerIds[2], IpAddress, RefererUrl, 1, 0, 0);
                }
                try
                {
                    oUser.ClickId = clickId;
                }
                catch
                {

                }
            }
            else
            {
                //showImage.Visible = true;

            }

            return Json(oUser, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region validate email
        /// <summary>
        /// Validate email
        /// </summary>
        /// <param name="e">Email Address</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult vldEmail(string email)
        {
            string msg = string.Empty;
            try
            {
                //string url = "https://www.xverify.com/services/emails/verify/?email=" + email + "&type=xml&apikey=" + ConfigurationManager.AppSettings["Xveirfy"].ToString() + "&domain=" + ConfigurationManager.AppSettings["Xverifydomain"].ToString();
                //var errormessage = PostRequest(url);
                //logger.Debug("XverifyCall|" + Request.Url.AbsoluteUri.ToString() + "| IPAddress :" + IpAddress.ToString() + "| Domain :" + ConfigurationManager.AppSettings["Xverifydomain"].ToString() + "| API :" + url + "| response :" + errormessage);
                logger.Debug("XverifyCall|" + Request.UrlReferrer.AbsoluteUri.ToString() + "| IPAddress :" + IpAddress.ToString() + "| Domain :" + ConfigurationManager.AppSettings["Xverifydomain"].ToString() + "| Email :" + email + "| OrgID :" + MemberIdentity.Client.ClientId.ToString());
                //if (errormessage.Contains(@">valid</status>") || errormessage.Contains(@">unknown</status>"))
                //{
                msg = "accepted";
                //}
                //else
                //{
                //    msg = "rejected";
                //}
            }
            catch
            {
                msg = "rejected";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Email address check
        /// <summary>
        /// EmailAddressCheck
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        public JsonResult EmailAddressCheck(string email)
        {
            User ouser = new User();
            UserManager objUserManager = new UserManager();
            ouser = objUserManager.EmailAddressCheck(email, MemberIdentity.Client.ClientId);
            return Json(ouser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Step1  Registration Form Save
        /// <summary>
        ///  Step1  Registration Form Save
        /// </summary>
        /// <param name="oUser">User</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult Step1Registration(User oUser)
        {
            string userGuid = string.Empty;
            oUser.RefferId = MemberIdentity.Client.Referrerid;
            oUser.SubId2 = ReferrerIds[1];
            try
            {
                if (ConfigurationManager.AppSettings["CPADetectiveCheckOn"] == "1")
                {
                    string clientKey = string.Empty;
                    clientKey = "303bd5c48be99544agxh";
                    string count = PostRequest("https://2pth.com/ipcheck.php?ck=" + clientKey.ToString() + "&ip=" + IpAddress + "&s=" + oUser.HitId + "&p=" + ReferrerIds[0] + "&a=" + ReferrerIds[1]);

                    if (Convert.ToInt32(count) > 65 && Convert.ToInt32(count) < 100)
                    {
                        oUser.CpaCount = Convert.ToInt32(count);
                        oUser.IsFraud = true;
                        oUser.IsDnc = true;
                        oUser.DncReason = "CPA-Fraud";

                    }
                    else
                    {
                        oUser.CpaCount = Convert.ToInt32(count);
                        oUser.IsFraud = false;
                        oUser.IsDnc = false;
                    }
                }
            }
            catch
            {
                oUser.CpaCount = 0;
                oUser.IsFraud = false;
                oUser.IsDnc = false;
            }
            oUser.IpAddress = IpAddress;
            oUser.ReferrerUrl = RefererUrl;
            oUser.ClickId = Convert.ToInt32(oUser.ClickId);
            oUser.HitId = Convert.ToInt32(oUser.HitId);
            string sCookieName1 = Members.PrecisionSample.Common.Utils.Names.Cookie.Name;
            HttpCookie cookie = Request.Cookies.Get(sCookieName1);
            if (cookie == null)
            {
                UserManager oUserManager = new UserManager();
                //UserDataServer oUserDataServer = new UserDataServer();
                int count1 = 0;
                count1 = oUserManager.Step1GetIpAddressCount(IpAddress);
                if (count1 < 3)
                {
                    string host = Request.Url.Host;
                    userGuid = Convert.ToString(oUserManager.Step1UserDataInsert(oUser, host));

                }
            }
            return Json(userGuid, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Save User Details
        /// <summary>
        /// Save User Details
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>
        [Route("saveUser")]
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult saveUser(User oUser)
        {
            if (string.IsNullOrEmpty(ReferrerIds[0].ToString()))
            {
                Client ouserClient = new Client();
                ouserClient = GetClientDetails();
                oUser.RefferId = ouserClient.Referrerid;
            }
            else
            {
                oUser.RefferId = Convert.ToInt32(ReferrerIds[0].ToString());
            }
            oUser.SubId2 = ReferrerIds[1];
            if (oUser.RefferId != 20708)
            {
                oUser.SubId3 = ReferrerIds[2];
            }
            oUser.IpAddress = IpAddress;
            oUser.ReferrerUrl = RefererUrl;
            oUser.FriendId = FriendId;
            //oUser.CountryId = oUser.CountryId;
            //oUser.StateId = oUser.StateId;
            //oUser.Zip = oUser.ZipCode;
            //oUser.EthnicityId = oUser.EthnicityId;
            oUser.DomainUrl = Request.Url.Host;
            oUser.Dob = oUser.Month + "/" + oUser.Day + "/" + oUser.Year;
            HttpClient client = new HttpClient();
            var userContent = JsonConvert.SerializeObject(oUser);
            var content = new StringContent(userContent, Encoding.UTF8, "application/json");
            var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/CreateWL", content).Result;
            if (result.ReasonPhrase.ToLower() == "ok")
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                // To convert an XML node contained in string xml into a JSON string   
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(jsonString.Replace('"', ' ').Trim());
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                RootObject oUserRes = new JavaScriptSerializer().Deserialize<RootObject>(jsonText);
                return Json(oUserRes.result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Unsubscirbe Action method 
        public ActionResult Unsub(string ug)
        {
            ViewBag.UserGuid = ug;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Misc/us.cshtml");

        }
        #endregion

        #region Get User Email
        /// Get Accounts Details
        /// </summary>
        /// <param name="UserId">userId</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetUserEmail(string ug)
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            ClickflowManager oManager = new ClickflowManager();
            int cid = 0;
            if (cid == 0)
            {
                cid = oManager.GetOrgidByUserDPV(ug);
            }
            objuser = objUserManager.GetUserData(ug, null, cid);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region External member unsubscribe from email
        /// <summary>
        /// Unsubscribe user by email address
        /// </summary>
        /// <param name="em">Email Address</param>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public void UnsubUserByEmail(string em)
        {
            UserManager objUserManager = new UserManager();
            objUserManager.UserEmailDncInsert(em, -1, -1);
        }
        #endregion

        #region Get UserId For Unsubscribe
        /// Get Accounts Details
        /// </summary>
        /// <param name="UserId">userId</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public void UnsubUserDnc(string ug, int cid)
        {
            UserManager objUserManager = new UserManager();
            ClickflowManager oManager = new ClickflowManager();
            if (cid == 0)
            {
                cid = oManager.GetOrgidByUserDPV(ug);
            }
            objUserManager.UnsubUserDnc(ug, cid);
        }
        #endregion

        #region User Email Dnc Insert
        /// <summary>
        /// User Email Dnc Insert
        /// </summary>
        /// <param name="EmailAddress">EmailAddress</param>
        public void UserEmailDncInsert(string EmailAddress, int cid, int refId)
        {
            UserManager oManager = new UserManager();
            oManager.UserEmailDncInsert(EmailAddress, cid, refId);
            Recipients objrecipients = new Recipients();
            // objrecipients.recipients = new List<ApiCall>();
            ApiCall objApiCall = new ApiCall();
            objApiCall.recipient = EmailAddress;
            objrecipients.recipients.Add(objApiCall);
            ApiCall objApiCall1 = new ApiCall();
            objApiCall1.recipient = EmailAddress;
            objApiCall1.type = "non_transactional";
            objrecipients.recipients.Add(objApiCall1);
            var jsonstring = JsonConvert.SerializeObject(objrecipients);
            string result = ApiMethodCall(ConfigurationManager.AppSettings["sparkposturl"].ToString(), jsonstring);

        }
        #endregion

        #region Unsubscribe User
        /// <summary>
        /// Unsubscribe User
        /// </summary>
        /// <param name="oUser">User</param>
        /// <returns></returns>
        public JsonResult unsubUser(string EmailAddress)
        {
            string msg = string.Empty;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var response = client.PostAsJsonAsync("api/Member/Unsubscribe?Rid=" + Convert.ToInt32(MemberIdentity.Client.Referrerid) + "&UserName=" + EmailAddress, "").Result;
            if (response.ReasonPhrase == "OK")
            {
                Recipients objrecipients = new Recipients();
                // objrecipients.recipients = new List<ApiCall>();
                ApiCall objApiCall = new ApiCall();
                objApiCall.recipient = EmailAddress;
                objrecipients.recipients.Add(objApiCall);
                ApiCall objApiCall1 = new ApiCall();
                objApiCall1.recipient = EmailAddress;
                objApiCall1.type = "non_transactional";
                objrecipients.recipients.Add(objApiCall1);
                var jsonstring = JsonConvert.SerializeObject(objrecipients);
                string result = ApiMethodCall(ConfigurationManager.AppSettings["sparkposturl"].ToString(), jsonstring);
                msg = "accepted";
            }
            else
            {
                msg = "rejected";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region API Method Call
        public string ApiMethodCall(string apiurl, string jsonstring)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append(apiurl);
            var _request = (HttpWebRequest)WebRequest.Create(_sb.ToString());

            var utf8 = Encoding.UTF8;
            _request.Timeout = 60000;
            var _postdata = utf8.GetBytes(jsonstring);
            var _data = _postdata;
            _request.Method = "PUT";
            _request.ContentType = "application/json";
            _request.ContentLength = _data.Length;
            _request.Headers.Add("Authorization", ConfigurationManager.AppSettings["SparkPostKey"]);
            var _responseString = "";
            string _message = "";
            string _methodName = "Unsubscribe";
            ApiCall objApiCall;
            try
            {
                using (var stream = _request.GetRequestStream())
                {
                    stream.Write(_data, 0, _data.Length);
                }
                var _response = (HttpWebResponse)_request.GetResponse();
                // TransAM.Common.Logging.NLog.ClassLogger.Trace("TranscationId:" + objeFinishAnswer.TransactionalId.ToString() + "|Airs Response Time:" + (airscallendtime - airscallstarttime).ToString());
                _responseString = new StreamReader(_response.GetResponseStream()).ReadToEnd();
                objApiCall = JsonConvert.DeserializeObject<ApiCall>(_responseString);
                //if (objApiCall.StatusCode != 0)//if airs api return any error message 
                //{
                //    int _rewardId = objAnswerDataService.FinalReward(_finishAnswerId, objeFinishAnswer.AnswerStatus, false, true);
                //    _message = objAnswerDataService.AIRSAPICallFailedRecordInsert(_apiURL, _airsJson, _methodName, _rewardId);
                //}
                //if (_objDetails.StatusCode == 0)
                //    objAnswerDataService.FinalReward(_finishAnswerId, objeFinishAnswer.AnswerStatus, true, true);
                _message = "success" + "_" + _responseString;
                logger.Info("ApiMethodCall|" + Request.Url.AbsoluteUri.ToString() + "|" + _message);
            }
            catch (Exception ex)
            {
                logger.Error("ApiMethodCall|" + Request.Url.AbsoluteUri.ToString() + "|" + _message);
            }
            return _message;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
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

        #region Cancel User Account
        /// <summary>
        /// Delete Client
        /// </summary>
        /// <param name="clGuid">Client Guid</param>
        [HttpPost]
        public void CancelUser()
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            objUserManager.CancelUser(MemberIdentity.Client.UserGuid, MemberIdentity.Client.ClientId);
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
        public JsonResult GetStep2Details(string lid)
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            objuser = objUserManager.GetStep2Details(lid, MemberIdentity.Client.ClientId);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save linkedin user data
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        [Route("SaveLinkedinData")]
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public string SaveLinkedinData(string json)
        {
            UserManager objUserManager = new UserManager();
            return objUserManager.SaveLinkedinData(json);
        }
        #endregion

        #region Get Linkedin Data
        /// <summary>
        /// Get Linkedin Data
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [Route("GetLinkedinData")]
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetLinkedinData(int id)
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            objuser = objUserManager.GetLinkedinData(id);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Error()
        {
            return View("/Views/pages/Error.cshtml");
        }
    }
}