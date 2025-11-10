using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Members.OpinionBar.Components.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web.Script.Serialization;
using NLog;
using Members.NewOpinionBar.Web.Filters;
using Members.OpinionBar.Components.Business_Layer;
using System.Net;
using System.IO;
using System.Net.Http.Formatting;
using System.Web;

namespace Members.NewOpinionBar.Web.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : BaseController
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region Login
        /// <summary>
        /// Render LogIn Page
        /// </summary>
        /// <returns></returns>
        //public ActionResult LogIn()
        //{
        //    return View("~/Views/Account/OPLogin.cshtml");
        //}
        #endregion

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
            string redirectUrl = Request.Url.AbsoluteUri;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            if (ug != null && (ug.Length == 36 || ug.Length == 32))
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
                    return Redirect("~/Ms/Surveys");
                }
            }
            else
            {
                int orgId = MemberIdentity.Client.ClientId;
                ViewBag.CountryCode = "en";
                User oUser = new OpinionBar.Components.Entities.User();
                ViewBag.IsShowLogIn = 1;
                return View("~/Views/Account/OPLogin.cshtml", oUser);
            }

        }
        #endregion

        #region Forgot Password Page 
        public ActionResult ForgotPsw()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Home/ForgotPwd.cshtml");
        }
        #endregion

        public ActionResult Registration(string leadid)
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            int orgId = MemberIdentity.Client.ClientId;
            ViewBag.CountryCode = "en";
            User oUser = new OpinionBar.Components.Entities.User();
            ViewBag.IsShowLogIn = 1;
            return View("~/Views/Account/PixelLogin.cshtml", oUser);
        }
        public ActionResult signup()
        {
            ViewBag.SelectedMenu = "OPSignup";
            return View("~/Views/Home/Signup.cshtml");
        }

        public ActionResult About()
        {
            ViewBag.SelectedMenu = "about";
            return View("~/Views/Home/about.cshtml");
        }
        #region  Member LogIn Check
        /// <summary>
        /// Login Member validate method
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult LogIn(User oUser)
        {
            string host = Request.Url.Host;
            //    HttpClient client = new HttpClient();
            //    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            //    var response = client.PostAsJsonAsync("api/Member/UserLogin/", ologin).Result;
            //    return response.ToString();
            //User oUser = new Components.Entities.User();
            UserManager oManager = new UserManager();
            oUser = oManager.LoginCheck(oUser.EmailAddress, oUser.Password, host, MemberIdentity.Client.ClientId);
            //if (MemberIdentity.Client.ClientId == 383 && oUser.UserGuid != Guid.Empty)
            //{
            //    DoLogin(oUser.UserGuid);
            //    return Json("/Ms/Surveys" + oUser.pwdExpired, JsonRequestBehavior.AllowGet);
            //}
            //else 
            if (oUser.UserGuid != Guid.Empty)
            {
                DoLogin(oUser.UserGuid);
                return Json("/Ms/Surveys?ug=" + oUser.UserGuid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        #endregion       

        #region Get External Member Details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ug"></param>
        /// <returns></returns>
        public JsonResult join(string leadid)
        {
            LbUser oUser = new LbUser();
            UserManager oManager = new UserManager();
            oUser = oManager.GetExtMemDetails(leadid);
            return Json(oUser, JsonRequestBehavior.AllowGet);

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
            string[] ipaddress;
            if (oUser.CreatedBy == "Affiliate")
            {
                //    oUser.RefferId = oUser.RouterReferrerId;
                //    oUser.SubId2 = oUser.RouterSubId2;
            }
            else
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
            }
            if (oUser.SubId3 == "" || oUser.SubId3 == null)
            {
                if (oUser.RefferId != 20708)
                {
                    oUser.SubId3 = ReferrerIds[2];
                }
            }
            //For router traffic we need to pass router referrer id,subid2 on panel regestration.
            if (oUser.CreatedBy == "router")
            {
                oUser.RefferId = oUser.RouterReferrerId;
                oUser.SubId2 = oUser.RouterSubId2;
            }
            //oUser.IpAddress = IpAddress;
            logger.Info("CreateWL|" + HttpContext.Request.Headers["X-Forwarded-For"].ToString());
            ipaddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',');
            oUser.IpAddress = ipaddress[0];
            oUser.ReferrerUrl = RefererUrl;
            oUser.FriendId = FriendId;
            //oUser.CountryId = oUser.CountryId;
            //oUser.StateId = oUser.StateId;
            //oUser.Zip = oUser.ZipCode;
            //oUser.EthnicityId = oUser.EthnicityId;
            oUser.DomainUrl = Request.Url.Host;
            oUser.Dob = oUser.Month + "/" + oUser.Day + "/" + oUser.Year;
            string jsonText = string.Empty;
            HttpClient client = new HttpClient();
            var userContent = JsonConvert.SerializeObject(oUser);
            var content = new StringContent(userContent, Encoding.UTF8, "application/json");
            var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/CreateWL", content).Result;
            if (result.ReasonPhrase.ToLower() == "ok")
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                // To convert an XML node contained in string xml into a JSON string         
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(jsonString.Replace('"', ' ').Trim());
                    jsonText = JsonConvert.SerializeXmlNode(doc);
                }
                catch (Exception ex)
                {
                    logger.Error("CreateWL|" + result + "|" + jsonString + "|" + userContent + "|" + jsonText + "|" + ex);
                }
                RootObject oUserRes = new JavaScriptSerializer().Deserialize<RootObject>(jsonText);
                return Json(oUserRes.result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
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
                string url = "https://www.xverify.com/services/emails/verify/?email=" + email + "&type=xml&apikey=" + ConfigurationManager.AppSettings["Xveirfy"].ToString() + "&domain=" + ConfigurationManager.AppSettings["Xverifydomain"].ToString();
                var errormessage = PostRequest(url);
                logger.Debug("XverifyCall|" + Request.Url.AbsoluteUri.ToString() + "| IPAddress :" + IpAddress.ToString() + "| Domain :" + ConfigurationManager.AppSettings["Xverifydomain"].ToString() + "| API :" + url + "| response :" + errormessage);
                //var errormessage = PostRequest("https://www.xverify.com/services/emails/verify/?email=" + email + "&type=xml&apikey=" + ConfigurationManager.AppSettings["Xveirfy"].ToString() + "&domain=surveydownline.com");
                if (errormessage.Contains(@">valid</status>") || errormessage.Contains(@">unknown</status>"))
                {
                    msg = "accepted";
                }
                else
                {
                    msg = "rejected";
                }
            }
            catch
            {
                msg = "rejected";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

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
        public ActionResult Contact(string lc)
        {
            ViewBag.SelectedMenu = "contact";
            if (!string.IsNullOrEmpty(lc))
            {
                ViewBag.recaptchalangCode = lc;
            }
            else
            {
                ViewBag.recaptchalangCode = "en";
            }
            return View("~/Views/Home/contact.cshtml");
        }

        #region Unsubscirbe Action method 
        public ActionResult Unsub(string ug)
        {
            ViewBag.UserGuid = ug;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Misc/us.cshtml");

        }
        #endregion

        #region change password 
        public ActionResult changepassword(string ug, int? lid)
        {
            ViewBag.UserGuid = ug;
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Home/PwdExpire.cshtml");

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

        #region Sub id Check for Local blox
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subid"></param>
        /// <returns></returns>
        public JsonResult SubidCheck(string email, string subid)
        {
            User ouser = new User();
            UserManager objUserManager = new UserManager();
            ouser = objUserManager.SubidCheck(email, MemberIdentity.Client.ClientId, subid);
            return Json(ouser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public string pixelFiring(string leadid, Guid ug, string pc, string txid)
        {
            if (leadid != "" && txid != "")
            {
                if (pc == "t491")
                {
                    return ($"https://www.opinionbar.com/Ms/Surveys?ug={ug}&pc={pc}&txid={txid}&leadid={leadid}");
                }
                else
                {
                    return ($"https://www.opinionbar.com/Ms/Surveys?ug={ug}");
                }
            }
            return ($"https://www.opinionbar.com/Ms/Surveys?ug={ug}");
        }
    }
}
