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
using Members.OpinionBar.Web.Filters;
using System.Numerics;
using NLog;

namespace Members.OpinionBar.Web.Controllers
{
    public class LoginController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Get User Data        
        [HttpGet]
        public JsonResult GetUserDataEmail(string EmailAddress, string token)
        {
            User oUser = new User();
            if (!String.IsNullOrEmpty(token))
            {
                var validate = RecaptchaClient.CloudFlareCaptchaValidate(token);
                if (!validate) { return Json(oUser, JsonRequestBehavior.AllowGet); }
            }
            else
            {
                return Json(oUser, JsonRequestBehavior.AllowGet);
            }
            string[] ipaddress = { };
            string IpCheck = string.Empty;
            BigInteger IpNumber = 0;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                ipaddress = IpCheck.Split(',');
                IpNumber = Dot2LongIP(ipaddress[0]);
            }
            UserManager oManager = new UserManager();
            logger.Info("GetUserDataEmail| " + HttpContext.Request.Headers["X-Forwarded-For"].ToString() + "| Email Address: " + EmailAddress + "| URL: " + Request.Url.AbsoluteUri);
            oUser = oManager.GetUserDataEmail(EmailAddress, MemberIdentity.Client.ClientId, ipaddress[0].ToString(), IpNumber.ToString());
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

        #region IpAddress2IPnumber
        public BigInteger Dot2LongIP(string ipv6)
        {
            System.Net.IPAddress address;
            System.Numerics.BigInteger ipnum;

            if (System.Net.IPAddress.TryParse(ipv6, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();
                if (System.BitConverter.IsLittleEndian)
                {
                    System.Collections.Generic.List<byte> byteList = new System.Collections.Generic.List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }
                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = System.BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += System.BitConverter.ToUInt64(addrBytes, 0);
                }
                else
                {
                    //IPv4
                    ipnum = System.BitConverter.ToUInt32(addrBytes, 0);
                }
                return ipnum;
            }
            else
            {
                return 0;
            }

        }
        #endregion

        #region Payswell AWS log 
        [HttpPost]
        public string LogData()
        {
            string country = string.Empty;
            //Nlog.CreateLogger("OpinionBarPayswellLog");
            UserManager oManager = new UserManager();
            string[] ipaddress = { };
            string IpCheck = string.Empty;
            BigInteger IpNumber = 0;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                ipaddress = IpCheck.Split(',');
                IpNumber = Dot2LongIP(ipaddress[0]);
            }

            country = oManager.GetIPAddress(ipaddress[0].ToString(), IpNumber.ToString());
            //Nlog.Info($"Payswell Log | Country: {country} | IPAddress: {ipaddress[0]} | Date: {DateTime.Now}");
            return country;
        }
        #endregion
    }
}