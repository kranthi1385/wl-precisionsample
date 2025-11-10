using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Business_Layer;
using Members.PrecisionSample.Common.Security;
using Members.OpinionBar.Components.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using Members.NewOpinionBar.Web.Filters;
using System.Web.Script.Serialization;

namespace Members.NewOpinionBar.Web.Controllers
{
    [Authorize]
    public class PrController : BaseController
    {
        #region Profiles action method
        /// <summary>
        /// Load Profiles Page
        /// </summary>
        /// <returns></returns>
        public ActionResult OBProfile()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.Name = Identity.Current.UserData.FirstName;
                ViewBag.EmailAddress = Identity.Current.UserData.EmailAddress;
            }
            return View("/Views/Home/OBProfile.cshtml");
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
    }
}