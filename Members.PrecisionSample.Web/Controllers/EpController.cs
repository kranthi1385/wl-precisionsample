using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Common.Security;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Members.PrecisionSample.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Ep")]
    public class EpController : BaseController
    {

        // GET: Ep
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Account()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.UserId = Identity.Current.UserData.UserId;
            }
            return View("/Views/Render/Account.cshtml");
        }
        #region Get User Data
        /// <summary>
        /// Get Accounts Details
        /// </summary>
        /// <param name="UserId">userId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserData()
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            objuser = objUserManager.GetUserData(Identity.Current.UserData.UserGuid, null, MemberIdentity.Client.ClientId);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update User
        [Route("saveUser")]
        [HttpPost]
        public string saveUser(User oUser)
        {
            oUser.Dob = oUser.Month + "/" + oUser.Day + "/" + oUser.Year;
            oUser.UpdatedBy = oUser.EmailAddress;
            HttpClient client = new HttpClient();
            var userContent = JsonConvert.SerializeObject(oUser);
            var content = new StringContent(userContent, Encoding.UTF8, "application/json");
            var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/UpdateWL", content).Result;
            return result.ToString();
        }
        #endregion
        #region Delete User
        [Route("DeleteUserData")]
        [HttpPost]
        public void DeleteUserData(string SubId3)
        {
            UserManager objUserManager = new UserManager();
            objUserManager.DeleteUserData(Identity.Current.UserData.UserGuid, MemberIdentity.Client.Referrerid, SubId3);
        }
        #endregion

    }
}