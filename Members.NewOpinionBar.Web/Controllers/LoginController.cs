using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Entities;
using System.Web.Security;
using System.Net.Http;
using System.Configuration;
using Members.OpinionBar.Components.Business_Layer;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Members.NewOpinionBar.Web.Controllers
{
    public class LoginController : BaseController
    {

        #region Get User Data
        [HttpGet]
        public JsonResult GetUserDataEmail(string EmailAddress)
        {
            User oUser = new User();
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserDataEmail(EmailAddress, MemberIdentity.Client.ClientId,"","");
            return Json(oUser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Contact Us
        public ActionResult cu()
        {
            return View("~/Views/Home/Contact.cshtml");
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

        #region Help Desk
        public ActionResult HelpDesk()
        {
            return View("~/Views/Home/HelpDesk.cshtml");
        }
        #endregion

        #region Do Not Sell My Info Page
        public ActionResult dns(string lc)
        {
            if (!string.IsNullOrEmpty(lc))
            {
                ViewBag.recaptchalangCode = lc;
            }
            else
            {
                ViewBag.recaptchalangCode = "en";
            }
            return View("/Views/Footer/DoNotSellInfo.cshtml");
        }
        #endregion

        #region Save Do Not Sell My Info
        [HttpPost]
        public void SaveDoNotSellMyInfo(string fstName, string lstName, string email, string presite, int reqid)
        {
            string ReferrerUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.ToString();
            }
            UserManager oManager = new UserManager();
            oManager.SaveDoNotSellMyInfo(fstName, lstName, email, presite, reqid, MemberIdentity.Client.ClientId, ReferrerUrl);
        }
        #endregion

        #region Change Password 
        [HttpPost]
        public int ChangePassword(string OldPassword, string NewPassword, string CnfNewPassword, string ug)
        {
            int result = 0;
            //if (Request.UrlReferrer != null)
            //{
            //    ReferrerUrl = Request.UrlReferrer.ToString();
            //}
            UserManager oManager = new UserManager();
            return result = oManager.ChangePassword(OldPassword, NewPassword, CnfNewPassword, ug);
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
    }
}