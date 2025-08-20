using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Common.Utils;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Numerics;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class UserManager
    {
        UserDataServices objDataServer = new UserDataServices();

        #region Get CountryCode from ipaddress
        /// <summary>
        /// Get CountryCode from ipaddress
        /// </summary>
        /// <param name="IpAddress">IpAdress</param>
        /// <returns></returns>
        public string GetCountryForIp(string IpAddress)
        {
            return objDataServer.GetCountryForIp(IpAddress);
        }
        #endregion

        #region Get UserData

        /// <summary>
        /// Get User Data By USerId
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ClientId">ClientId</param>
        /// <returns></returns>
        public User GetUserData(string userId, int? Rid, int? ClientId)
        {
            return objDataServer.GetUserData(userId, Rid, ClientId);
        }
        #endregion

        #region Delete UserData
        public void DeleteUserData(string UserGuid, int Referrerid, string SubId3)
        {
            objDataServer.DeleteUserData(UserGuid, Referrerid, SubId3);
        }
        #endregion

        #region Get UserData

        /// <summary>
        /// Get User Data By USerId
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ClientId">ClientId</param>
        /// <returns></returns>
        public void UnsubUserDnc(string userId, int ClientId)
        {
            objDataServer.UnsubUserDnc(userId, ClientId);
        }
        #endregion

        #region Get UserData

        /// <summary>
        /// Get User Data By USerId
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="ClientId">ClientId</param>
        /// <returns></returns>
        public void UserEmailDncInsert(string EmailAddress, int ClientId, int Referrerid)
        {
            objDataServer.UserEmailDncInsert(EmailAddress, ClientId, Referrerid);
        }
        #endregion


        //#region Get Userdatabased on user guid / reeferrer ID
        //public User GetUserDataByUserGuidandRid(string UserGuid, int Rid)
        //{
        //    UserDataServices oServer = new UserDataServices();
        //    return oServer.GetUserDataByUserGuidandRid(UserGuid, Rid);
        //}
        //#endregion

        #region Get Organization Deatils by hosturl

        /// <summary>
        /// Get Organization Deatils by hosturl
        /// </summary>
        /// <param name="HostUrl">HostUrl</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Client GetPartnerOrgIdByMemberUrl(string HostUrl, Guid UserGuid)
        {
            return objDataServer.GetPartnerOrgIdByMemberUrl(HostUrl, UserGuid);
        }
        #endregion

        #region UserLogin Check

        /// <summary>
        /// UserLogin Check
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="Password">Password</param>
        public User LoginCheck(string EmailAddress, string Password, string host, int ClientId)
        {
            return objDataServer.LoginCheck(EmailAddress, Password, host, ClientId);
        }

        #endregion

        #region UserLogin Check

        /// <summary>
        /// UserLogin Check
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="Password">Password</param>
        public User WidgetLoginCheck(string EmailAddress, string Password, string host, int ClientId)
        {
            return objDataServer.WidgetLoginCheck(EmailAddress, Password, host, ClientId);
        }

        #endregion

        #region Get survey Url By invitation Guid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSurveyUrlByInvitationGuid(string userInvitationGuid)
        {
            UserDataServices oUserServer = new UserDataServices();
            return oUserServer.GetSurveyUrlByInvitationGuid(userInvitationGuid);
        }
        #endregion

        #region Save UserDetails
        /// <summary>
        /// Save User Details
        /// </summary>
        /// <param name="oUser">User Entity</param>
        /// <returns></returns>
        public User UserInsert(User oUser)
        {
            return objDataServer.UserInsert(oUser);
        }
        #endregion

        #region Referrer Clicks Insert
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubId1">SubId1</param>
        /// <param name="SubId2">SubId2</param>
        /// <param name="SubId3">SubId3</param>
        /// <param name="Ipaddress">IpAddress</param>
        /// <param name="Referrerurl">Referrerurl</param>
        /// <param name="PageNo">PageNo</param>
        /// <param name="CpatchaFlag">CpatchaFlag</param>
        /// <param name="CpatchaFlag">CpatchaFlag</param>
        /// <returns></returns>
        public int InserClicks(string SubId1, string SubId2, string SubId3, string Ipaddress, string Referrerurl, int PageNo, int CpatchaFlag, int OpttiburonsplitFlag)
        {

            return objDataServer.InserClicks(SubId1, SubId2, SubId3, Ipaddress, Referrerurl, PageNo, CpatchaFlag, OpttiburonsplitFlag);
        }

        #endregion

        #region Getting User Information for Home Page
        /// <summary>
        ///  Getting User Information for Home Page
        /// </summary>
        /// <returns></returns>
        public Home GetHomePageDetails(int UserId, int ClientId)
        {
            return objDataServer.GetHomePageDetails(UserId, ClientId);
        }
        #endregion

        #region Update SOI Pixel Fired Status
        /// <summary>
        /// Get Update SOI Pixel Fired Status
        /// </summary>
        /// <param name="UserId">userid</param>
        /// <param name="ReferrerId">referrerid</param>
        public void UpdateSOIPixelFiredStatusUpdate(int UserId, int ReferrerId, int ClientId)
        {
            objDataServer.UpdateSOIPixelFiredStatusUpdate(UserId, ReferrerId, ClientId);
        }
        #endregion

        #region UserRegistrationStep-Update

        /// <summary>
        /// user registration step update
        /// </summary>
        /// <param name="oUser">ouser</param>
        public void UserRegistrationStepUpdate(User oUser, int ClientId)
        {
            objDataServer.UserRegistrationStepUpdate(oUser, ClientId);
        }
        #endregion

        //#region GetUserDetails

        ///// <summary>
        ///// get user details
        ///// </summary>
        ///// <param name="userName">username</param>
        ///// <returns></returns>
        //public User GetUserDetails(string userName)
        //{
        //    return objDataServer.GetUserData(userName);
        //}
        //#endregion GetUserDetails

        //#region GetDvariable

        ///// <summary>
        ///// get Dvariable
        ///// </summary>
        ///// <param name="emailAddress">emailaddress</param>
        ///// <param name="reason">reason</param>
        //public string GetDvariable()
        //{
        //    return objDataServer.GetDvariable();
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        //public string GetTEvariable(int userid)
        //{
        //    return objDataServer.GetTEvariable(userid);
        //}

        //#endregion

        //#region GetSNvariable

        ///// <summary>
        ///// get SNvariable
        ///// </summary>
        ///// <param name="emailAddress">emailaddress</param>
        ///// <param name="reason">reason</param>
        //public string GetSNvariable()
        //{
        //    return objDataServer.GetSNvariable();
        //}
        //#endregion

        #region Reffer Cliks Insert

        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <param name="oUser">ouser</param>
        /// <returns></returns>
        public int InsertClicks(string subId1, string subId2, string subId3, string ipaddress, string referrerurl, int pageno, int captchaflag, int opttiburonsplitflag)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.InserClicks(subId1, subId2, subId3, ipaddress, referrerurl, pageno, captchaflag, opttiburonsplitflag);
        }

        #endregion

        #region Step1 User Insert
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>

        public Guid Step1UserDataInsert(User oUser, string Host)
        {
            UserDataServices oServices = new UserDataServices();
            return oServices.Step1UserInsert(oUser, Host);
        }

        #endregion

        #region Insert OB/CP external Member
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        public Guid RouteruserInsert(User oUser)
        {
            UserDataServices oServices = new UserDataServices();
            return oServices.RouteruserInsert(oUser);
        }
        #endregion 

        #region check user already exist or not

        public MemberEntity UserDeialscCheck(int Rid, string ExtId, string host)
        {
            return objDataServer.UserDeialscCheck(Rid, ExtId, host);
        }
        public MemberEntity UserDeialscCheckByEmailAddress(int Rid, string EmailAddress, string host)
        {
            return objDataServer.UserDeialscCheckByEmailAddress(Rid, EmailAddress, host);
        }
        #endregion

        #region Update Relevant Score For SDL And Members
        /// <summary>
        /// ReleventUpdate
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="PfScore"></param>
        /// <param name="FpfScore"></param>
        /// <param name="RelevantId"></param>
        /// <returns></returns>
        public int ReleventUpdateForSDLOrWL(Guid UserGuid, int RelevantScore, int PfScore, string FpfScore, string RelevantId, int ClientId, string VerityId, int VerityScore, int GeoCorrelationFlag, bool VerityDOBFail,string ipqsJson)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.ReleventUpdateForSDLOrWL(UserGuid, RelevantScore, PfScore, FpfScore, RelevantId, ClientId, VerityId, VerityScore, GeoCorrelationFlag, VerityDOBFail, ipqsJson);
        }
        #endregion
        #region Update Relevant Score 
        /// <summary>
        /// Update Relevant Score
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="FpfScore"></param>
        /// <param name="RelevantId"></param>
        /// <returns></returns>
        public int ReleventUpdate(Guid UserGuid, int RelevantScore, int PfScore, string FpfScore, string RelevantId, int ClientId,string ipqsJson)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.ReleventUpdate(UserGuid, RelevantScore, PfScore, FpfScore, RelevantId, ClientId, ipqsJson);
        }
        #endregion

        #region Get IpAddress Count
        /// <summary>
        /// Get IpAddress Count
        /// </summary>
        /// <param name="IpAddress">IpAddress</param>
        /// <returns></returns>
        public int Step1GetIpAddressCount(string IpAddress)
        {
            return objDataServer.Step1GetIpAddressCount(IpAddress);
        }
        #endregion

        #region GetUserDetials For PeanutLabs
        /// <summary>
        /// GetUserDetials For PeanutLabs
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetUserDetailsForPeanutLabs(Guid UserGuid, int clientid)
        {
            return objDataServer.GetUserDetailsForPeanutLabs(UserGuid, clientid);
        }
        #endregion

        #region GetLandingPage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referrerid"></param>
        /// <returns></returns>
        public string GetLandingpageUrl(int referrerid)
        {
            return objDataServer.GetLandingpageUrl(referrerid);
        }
        #endregion

        #region GetLandingPage for CP/OB Router
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referrerid"></param>
        /// <returns></returns>
        public string GetRouterLandingpageUrl(int referrerid, string txid)
        {
            return objDataServer.GetRouterLandingpageUrl(referrerid, txid);
        }
        #endregion


        #region Forgot Password
        public void ForgetPassword(User ouser, int campid, int rId, string CustomAttribute)
        {
            UserDataServices odataservices = new UserDataServices();
            odataservices.ForgetPassword(ouser, campid, rId, CustomAttribute);
        }
        #endregion
        public string ChangePassword( string NewPassword,string email)
        {
            string result = string.Empty;
            result = objDataServer.ChangePassword( NewPassword, email);
            return result;
        }
        public int SendresetLink(int orgid, int rid, string extmid)
        {
            int result = 0;
            result = objDataServer.SendresetLink(orgid, rid, extmid); ;
            return result;
        }
        #region Get User Data
        public User GetUserDataEmail(string EmailAddress, int ClientId)
        {
            UserDataServices odataservices = new UserDataServices();
            return odataservices.GetUserDataEmail(EmailAddress, ClientId);
        }
        #endregion

        #region Get User Data
        public User emailAddressvaild(string EmailAddress, int ClientId)
        {
            UserDataServices odataservices = new UserDataServices();
            return odataservices.emailAddressvaild(EmailAddress, ClientId);
        }
        #endregion

        #region Send Email
        public int SendEmail(string fromaddress, string fromName, string comments)
        {
            string toAddresses = ConfigurationManager.AppSettings["ZendeskToAddress"].ToString();
            //string ccAddresses = string.Empty;
            string subject = "Contact Us From " + fromaddress;
            //string body = "hi test";
            int result = 1;
            try
            {
                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"].ToString(),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["mailingPort"]),
                    EnableSsl = false
                };
                MailMessage message = new MailMessage();

                message.From = new MailAddress(fromaddress);
                message.Sender = new MailAddress(toAddresses);
                comments = comments.Replace("\n\n", "<br />");
                message.To.Add(toAddresses);

                // subject
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = comments;

                // message body
                message.BodyEncoding = Encoding.UTF8;
                //send message            
                try
                {
                    smtp.Send(message);
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                result = 0;
            }
            return result;
        }
        #endregion

        #region Cancel User Account
        public void CancelUser(Guid UserGuid, int ClientId)
        {
            objDataServer.CancelUser(UserGuid, ClientId);
        }
        #endregion

        #region Get Step2 Details
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public User GetStep2Details(string lid, int ClientId)
        {
            User ouser = new User();
            ouser = objDataServer.GetStep2Details(lid, ClientId);
            return ouser;
        }
        #endregion

        #region Get Client Details By Rid
        /// <summary>
        /// Get Client Details By Rid
        /// </summary>
        /// <param name="RId">rid</param>
        /// <returns></returns>
        public Client GetClientDetailsByRid(string DomainUrl, int? RId = null, int? ClientId = null)
        {
            Client oClient = new Client();
            oClient = objDataServer.GetClientDetailsByRid(DomainUrl, RId, ClientId);
            return oClient;
        }
        #endregion

        #region Email Address Check
        /// <summary>
        /// Email Address Check
        /// </summary>
        /// <param name="EmailAddress">emailaddress</param>
        /// <param name="ClientId">clientid</param>
        /// <returns></returns>
        public User EmailAddressCheck(string EmailAddress, int ClientId)
        {
            return objDataServer.EmailAddressCheck(EmailAddress, ClientId);
        }
        #endregion

        #region Get Verity Information
        /// <summary>
        /// Get User Verity Information
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public string GetVerityInformation(string UserGuid, int CId)
        {
            return objDataServer.GetVerityInformation(UserGuid, CId);
        }
        #endregion

        #region Md5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string md5(string plainText)
        {
            MD5 enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }
            return str.ToString().ToLower();
        }
        #endregion

        #region Save Linkedin Data
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public string SaveLinkedinData(string json)
        {
            return objDataServer.SaveLinkedinData(json);
        }
        #endregion

        #region Get Linkedin Data
        /// <summary>
        /// Get Linkedin Data
        /// </summary>
        /// <param name="id">UserGuid</param>
        /// <returns></returns>
        public User GetLinkedinData(int id)
        {
            User ouser = new User();
            ouser = objDataServer.GetLinkedinData(id);
            return ouser;
        }
        #endregion

        #region GetPollQuestions
        /// <summary>
        /// GetPollQuestions
        /// </summary>
        /// <param name="UsId"></param>
        /// <returns></returns>
        public List<PSquestion> GetPollQuestions(int UsId)
        {
            return objDataServer.GetPollQuestions(UsId);
        }
        #endregion

        #region Save Poll Options
        /// <summary>
        /// Save Poll Options
        /// </summary>
        /// <param name="Xml"></param>
        /// <param name="UsId"></param>
        /// <param name="QuestionId"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public string SavePollOptions(string Xml, int UsId, int QuestionId, int SortOrder)
        {
            return objDataServer.SavePollOptions(Xml, UsId, QuestionId, SortOrder);
        }
        #endregion

        #region Save Do Not Sell My Info
        public int SaveDoNotSellMyInfo(string fstName, string lstName, string email, string presite, int reqid, string reqname, int ClientId, string ReferrerUrl)
        {
            //objDataServer.SaveDoNotSellMyInfo(fstName, lstName, email, presite, reqid, reqname, ClientId);
            string toAddresses = ConfigurationManager.AppSettings["ZendeskToAddress"].ToString();
            //string ccAddresses = string.Empty;
            string subject = "Do Not Sell My Info " + email;
            string comments = "Do Not Sell My Info at " + presite + " " + reqname + ".<br/> Referring URL : " + ReferrerUrl;
            //string body = "hi test";
            int result = 1;
            try
            {
                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"].ToString(),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["mailingPort"]),
                    EnableSsl = false
                };
                MailMessage message = new MailMessage();

                message.From = new MailAddress(email);
                message.Sender = new MailAddress(toAddresses);
                comments = comments.Replace("\n\n", "<br />");
                message.To.Add(toAddresses);

                // subject
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = comments;

                // message body
                message.BodyEncoding = Encoding.UTF8;
                //send message            
                try
                {
                    smtp.Send(message);
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                result = 0;
            }
            return result;
        }
        #endregion

        #region IpAddress2IPnumber
        public BigInteger GetIPNumber(string ipv6)
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

        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop20(Guid UserGUID, string OrgGUID,int ispeerly2)
        {
            return objDataServer.GetquestionsforTop20(UserGUID, OrgGUID, ispeerly2);
        }
        #endregion

        #region Top20SaveOptions
        /// <summary>
        /// Top20SaveOptions
        /// </summary>
        /// <param name="listXml"></param>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public string Top20SaveOptions(string listXml, Guid UserGUID)
        {
            return objDataServer.Top20SaveOptions(listXml, UserGUID);
        }
        #endregion

        #region Router Activity Get 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public int RouteruserActivityGet(int user_id)
        {
            return objDataServer.RouteruserActivityGet(user_id);
        }
        #endregion

        #region Get pixel Script
        /// <summary>
        /// Get pixel Script
        /// </summary>
        /// <param name="leadid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<pixel> GetScript(string leadid, int flag)
        {
            return objDataServer.GetScript(leadid, flag);
        }
        #endregion

        #region
        public string InsertAffClicks(int rid, string SubID3, string IPAddress, string sid, string fid, string trans_id, string RefererUrl)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.InsertAffClicks(rid, SubID3, IPAddress, sid, fid, trans_id, RefererUrl);
        }
        #endregion

        public string surveygetbyprjid(string prjid, int rid, string IPAddress)
        {
            string url = string.Empty;
            url = objDataServer.surveygetbyprjid(prjid, rid, IPAddress);
            return url;
        }

        #region Check member Exists
        public Boolean CheckMemberExist(string ug, int cid)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.CheckMemberExist(ug, cid);
        }
        #endregion
    }
}
