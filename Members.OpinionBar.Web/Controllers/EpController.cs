using Members.OpinionBar.Components.Business_Layer;
using Members.PrecisionSample.Common.Security;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using System.Web.Http;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace Members.OpinionBar.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public class EpController : BaseController
    {
        string requestUrl = string.Empty;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Account()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.UserId = Identity.Current.UserData.UserId;
            }
            return View("/Views/Home/Account.cshtml");
        }

        #region Get User Data
        /// <summary>
        /// Get Accounts Details
        /// </summary>
        /// <param name="UserId">userId</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpGet]
        public JsonResult GetUserData()
        {
            User objuser = new User();
            UserManager objUserManager = new UserManager();
            objuser = objUserManager.GetUserData(Identity.Current.UserData.UserGuid, null, MemberIdentity.Client.ClientId);
            return Json(objuser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update User
        [System.Web.Mvc.Route("saveUser")]
        [System.Web.Mvc.HttpPost]
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
        [System.Web.Mvc.Route("DeleteUserDataEmail")]
        [System.Web.Mvc.HttpPost]
        public void DeleteUserDataEmail(string SubId3, string Reason,int campaign_id,string custom_attr)
        {
            UserManager objUserManager = new UserManager();
            objUserManager.DeleteUserDataEmail(Identity.Current.UserData.UserGuid, MemberIdentity.Client.Referrerid, SubId3, Reason,campaign_id,custom_attr);
        }
        #endregion

        #region Delete User
       
        public ActionResult DeleteUserData(string SubId3)
        {
            int result = 0;
            UserManager objUserManager = new UserManager();
           result =  objUserManager.DeleteUserData(Identity.Current.UserData.UserGuid, MemberIdentity.Client.Referrerid, SubId3);
            if (result == 1)
            {
                //return View("~/Views/Home/Profile.cshtml");
                return View("/Views/Account/OPLogin.cshtml");
            }
            if (result == 0)
            {
                //return View("~/Views/Home/Profile.cshtml");
                return View("/Views/Home/EmailExpire.cshtml");
            }
            return View("/Views/Home/Account.cshtml");
        }
        #endregion

        #region Update Language Code
        /// <summary>
        /// Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        [System.Web.Mvc.HttpPost]
        public int GetLangCode(User user)
        {
            requestUrl = GetAbsoluteUrl();
            UserManager objUserManager = new UserManager();
            return objUserManager.GetLangCode(user, requestUrl);
        }
        #endregion


        public void downloadUser()
        {
           
            string Path = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["UserDownload"]);
            UserDataServices objUserManager = new UserDataServices();
            objUserManager.downloadUser(Path, Identity.Current.UserData.UserId, MemberIdentity.Client.ClientId);
            string strFilePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["UserDownload"]);
            FileInfo TragetFile = new FileInfo(strFilePath + "UserDownload" + ".xml");
            if (TragetFile.Exists)
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=User Details.xml");
                const int bufferLength = 10000;
                byte[] buffer = new Byte[bufferLength];
                int length = 0;
                Stream download = null;
                try
                {
                    download = new FileStream(Server.MapPath("~/Download/UserDownload.xml"),
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

        #region Update User
        public ActionResult piiconfirm(Guid ugg)
        {
            string result = string.Empty;
            string msg = string.Empty;
            UserManager objUserManager = new UserManager();
            result =  objUserManager.piiconfirm(ugg);
            if(!string.IsNullOrEmpty(result))
            {
                //return View("~/Views/Home/Profile.cshtml");
                //return View("/Views/Home/Account.cshtml");
                return Redirect("/Ms/Surveys?ug=" + result);
            }
            if (string.IsNullOrEmpty(result))
            {
                //return View("~/Views/Home/Profile.cshtml");
                return View("/Views/Home/EmailExpire.cshtml");
            }
            return View("/Views/Home/Account.cshtml");
        }
        #endregion
    }
}