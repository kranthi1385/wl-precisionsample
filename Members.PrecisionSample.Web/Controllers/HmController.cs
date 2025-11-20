using Members.PrecisionSample.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Web.Filters;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using NLog;

namespace Members.PrecisionSample.Web.Controllers
{
    [Authorize]
    public class HmController : BaseController
    {
        // GET: Hm
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            return View();
        }

        #region Home action method
        /// <summary>
        /// Load Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.Name = Identity.Current.UserData.FirstName;
                ViewBag.EmailAddress = Identity.Current.UserData.EmailAddress;
            }
            return View("/Views/Render/Home.cshtml");
        }
        #endregion

        #region Get UserDetails
        /// <summary>
        /// Get UserDetails
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetUserDetails()
        {
            UserManager oManger = new UserManager();
            User objUser = new User();
            objUser = oManger.GetUserData(Identity.Current.UserData.UserGuid, null, MemberIdentity.Client.ClientId);
            return Json(objUser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get MemberHistroy
        /// <summary>
        /// Get UserDetails
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetMemberHistory()
        {
            UserManager oManger = new UserManager();
            User objUser = new User();
            objUser = oManger.GetUserData(Identity.Current.UserData.EmailAddress, null, MemberIdentity.Client.ClientId);
            return Json(objUser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Referral vlaues
        /// <summary>
        /// Get User Referral Details
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetUserReferrerDetails(int UsId)
        {
            UserManager oManager = new UserManager();
            Home oHome = new Home();
            oHome = oManager.GetHomePageDetails(UsId, MemberIdentity.Client.ClientId);
            return Json(oHome, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get All Avlialbe Surveys
        /// <summary>
        /// Get all avaliable surveys by userid
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetAllAvaliableSurveys()
        {
            string u = string.Empty;
            int userTrafficTypeId = 2;
            string browserInfo = string.Empty;
            string mobiledeviceModel = string.Empty;
            string fpfScores = string.Empty;
            string[] ipAddress = { };
            string IpCheck = string.Empty;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            ipAddress = IpCheck.Split(',');
            logger.Trace($"SDL Home Page - IP Address: {ipAddress[0]}");
            //SurveyManager oSurveyManager = new SurveyManager();
            List<Surveys> lstSurveys = new List<Surveys>();
            //lstSurvey = oSurveyManager.GetAllAvaliableSurveys(Convert.ToInt32(UsId));

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            //var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            //HttpResponseMessage response = client.PostAsync("api/Member/GetSurveys?UserGuid=" + Identity.Current.UserData.UserGuid + "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var content = new StringContent("", Encoding.UTF8, "application/json");
            string Url = ConfigurationManager.AppSettings["gsapiurl"].ToString();
            client.BaseAddress = new Uri(Url);
            //HTTP GET
            var responseTask = client.GetStringAsync("SurveysGet?userGuid=" + Identity.Current.UserData.UserGuid + "&clientId=" + MemberIdentity.Client.ClientId + "&ipAddress=" + ipAddress[0]);
            responseTask.Wait();
            u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (u != null)
            {
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    //If Mobile Device Matched:
                    userTrafficTypeId = 2;
                }
                else
                {
                    //If Non Mobile Device Matched.
                    userTrafficTypeId = 3;
                }
            }
            var jsonString = responseTask.Result;
            if (jsonString.Contains("No Survey"))
            {
                lstSurveys = null;
            }
            else
            {
                lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
                lstSurveys = lstSurveys.Where(survey => survey.SurveyUserTypeIds.TrimEnd(';').Split(';').Where(surveytypeid => surveytypeid != "").Select(surveytypeid => Convert.ToInt32(surveytypeid)).Contains(userTrafficTypeId)).ToList();
            }

            return Json(lstSurveys, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Get All Profiles
        /// <summary>
        /// Get All Profiles
        /// <param name="UserId"></param>
        /// <param name="MemberLanguage">Member Selected Language</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetAllProfiles()
        {
            //ProfileManager oManager = new ProfileManager();
            List<Profile> lstProfiles = new List<Profile>();
            //lstProfiles = oManager.GetProfiles(MemLang, ug);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetProfile?UserGuid=" + Identity.Current.UserData.UserGuid + "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            lstProfiles = new JavaScriptSerializer().Deserialize<List<Profile>>(jsonString);
            foreach (Profile oProfile in lstProfiles)
            {
                if (oProfile.ProfileStatus.ToLower() == "not completed")
                {
                    oProfile.IsChecked = false;
                }
                else
                {
                    oProfile.IsChecked = true;
                }
            }
            return Json(lstProfiles, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Poll Questions
        /// <summary>
        /// Get Poll Questions
        /// </summary>
        /// <param name="UsId"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetPollQuestions(int UsId)
        {
            UserManager oManager = new UserManager();
            var pagedata = oManager.GetPollQuestions(UsId);
            return Json(pagedata, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Save Poll Options
        /// <summary>
        /// Save Poll Options
        /// </summary>
        /// <param name="UsId"></param>
        /// <param name="qId"></param>
        /// <param name="sr"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [Route("SavePollOptions")]
        [HttpPost]
        public JsonResult SavePollOptions(int UsId, int qId, int sr, string xml)
        {
            UserManager oManager = new UserManager();
            var pagedata = oManager.SavePollOptions(xml, UsId, qId, sr);
            // var pagedata = objPsDataService.UpdateMobileNumber(ug, uig, phonenumber);
            return Json(pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}