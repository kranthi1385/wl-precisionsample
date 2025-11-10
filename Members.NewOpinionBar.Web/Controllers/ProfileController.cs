using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using System.Web.Security;
using Members.PrecisionSample.Common.Security;
using Members.NewOpinionBar.Web.Filters;
using System.Net.Http;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Members.NewOpinionBar.Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        #region Home action method
        /// <summary>
        /// Load Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult main()
        {
            if (Identity.Current != null)
            {
                // ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.Name = Identity.Current.UserData.FirstName;
                ViewBag.EmailAddress = Identity.Current.UserData.EmailAddress;
            }
            return View("~/Views/Home/Profile.cshtml");
        }
        #endregion

        public ActionResult Surveys()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.UserId = Identity.Current.UserData.UserId;
            }
            return View("/Views/Render/Surveys.cshtml");
        }
        #region Get Surveys Details
        /// <summary>
        /// Get Surveys Details
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetSurveysList()
        {
            List<Surveys> lstSurveys = new List<Surveys>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetSurveys?UserGuid=" + Identity.Current.UserData.UserGuid + "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString.Contains("No Survey"))
            //if (jsonString.ToLower() != ("no survey was found for your profile"))
            {
                lstSurveys = null;
            }
            else
            {
                lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
            }

            return Json(lstSurveys, JsonRequestBehavior.AllowGet);
            //lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
            //return Json(lstSurveys, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Rewards Historys
        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardsHistory()
        {
            Rewards oRewards = new Rewards();
            //RewardHistory objRewardHistory = new RewardHistory();
            //RewardManager objrewardmanager = new RewardManager();
            //User objuser = new User();
            //oRewards = objrewardmanager.GetRewardsHistory(Convert.ToInt32(Identity.Current.UserData.UserId));
            //List<RewardHistory> lstRewardHistory = new List<RewardHistory>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetRewardsHistory?UserGuid=" + Identity.Current.UserData.UserGuid + "&UserId=" + Identity.Current.UserData.UserId +
                "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (!jsonString.ToLower().Contains("no reward history was found"))
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }
            else
            {
                oRewards = null;
            }
            return Json(oRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}