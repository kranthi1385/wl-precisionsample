using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;
using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Members.OpinionBar.Components.Business_Layer
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

        #region UserLogin Check

        /// <summary>
        /// UserLogin Check
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <param name="Password">Password</param>
        public User LoginCheck(string EmailAddress, string Password, string host, int ClientId)
        {
            var result = suppressionDeleted(EmailAddress);
            return objDataServer.LoginCheck(EmailAddress, Password, host, ClientId);
        }

        #endregion

        #region Get External Member Details
        /// <summary>
        /// Get External Member Details
        /// </summary>
        /// <param name="leadid"></param>
        /// <returns></returns>
        public LbUser GetExtMemDetails(string leadid)
        {
            return objDataServer.GetExtMemDetails(leadid);
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

        //#region Forgot password
        ///// <summary>
        ///// Forgot password
        ///// </summary>
        ///// <param name="oUser"></param>
        //public void ForgotPwd(User oUser)
        //{
        //    UserDataServices odataservices = new UserDataServices();
        //    odataservices.ForgotPwd(oUser);
        //}
        //#endregion

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

        #region Forgot Password
        public void ForgetPassword(User ouser, int campid, int rId, string CustomAttribute)
        {
            UserDataServices odataservices = new UserDataServices();
            odataservices.ForgetPassword(ouser, campid, rId, CustomAttribute);
        }
        #endregion
        #region Get User Data
        public User GetUserDataEmail(string EmailAddress, int ClientId, string ip, string IpNumber)
        {
            UserDataServices odataservices = new UserDataServices();
            var result = suppressionDeleted(EmailAddress);
            return odataservices.GetUserDataEmail(EmailAddress, ClientId, ip, IpNumber);
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
        public void DeleteUserDataEmail(string UserGuid, int Referrerid, string SubId3, string Reason, int campaign_id, string custom_attr)
        {
            objDataServer.DeleteUserDataEmail(UserGuid, Referrerid, SubId3, Reason, campaign_id, custom_attr);
        }
        #endregion

        #region Delete UserData
        public int DeleteUserData(string UserGuid, int Referrerid, string SubId3)
        {
            return objDataServer.DeleteUserData(UserGuid, Referrerid, SubId3);
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
        public int ReleventUpdateForSDLOrWL(Guid UserGuid, int RelevantScore, int PfScore, string FpfScore, string RelevantId, int ClientId, string VerityId, int VerityScore, int GeoCorrelationFlag, bool VerityDOBFail, string userId, string pid, string sessionId)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            string sessionResponse = string.Empty;
            if (sessionId != "undefined")
            {
                sessionResponse = AuthenticateSessionId(sessionId, userId.ToString(), pid);
            }
            return oUserDataServices.ReleventUpdateForSDLOrWL(UserGuid, RelevantScore, PfScore, FpfScore, RelevantId, ClientId, VerityId, VerityScore, GeoCorrelationFlag, VerityDOBFail, sessionResponse);
        }
        #endregion

        #region Verisoul Authentication
        private string AuthenticateSessionId(string sessionId, string extmid, string pid)
        {
            string sessionResponse = string.Empty;
            string sessionResponse2 = string.Empty;
            try
            {
                string verisoulURL = ConfigurationManager.AppSettings["VerisoulURL"].ToString();
                string verisoulAPIKey = ConfigurationManager.AppSettings["VerisoulAPIKey"].ToString();
                if (!string.IsNullOrEmpty(verisoulURL) && !string.IsNullOrEmpty(verisoulAPIKey))
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, verisoulURL);
                    var body = new
                    {
                        session_id = sessionId,
                        account = new { id = $"{extmid}", group = "Reach_Collective" }
                    };
                    request.Headers.Add("x-api-key", verisoulAPIKey);
                    var content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json");
                    request.Content = content;
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    sessionResponse = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(pid))
                    {
                        var body2 = new
                        {
                            session_id = sessionId,
                            account = new { id = $"{extmid}-{pid}", group = pid.ToString() }
                        };
                        var request2 = new HttpRequestMessage(HttpMethod.Post, verisoulURL);
                        request2.Headers.Add("x-api-key", verisoulAPIKey);
                        var response2 = client.SendAsync(request2).Result;
                        if (response2.IsSuccessStatusCode)
                        {
                            sessionResponse = response2.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            sessionResponse2 = response2.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
            catch
            {
                sessionResponse = string.Empty;
            }
            return sessionResponse;
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
        #region Update Language Code
        /// <summary>
        ///  Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        public int GetLangCode(User oUser, string RequestUrl)
        {
            return objDataServer.GetLangCode(oUser, RequestUrl);
        }
        #endregion

        #region Save Do Not Sell My Info
        public void SaveDoNotSellMyInfo(string fstName, string lstName, string email, string presite, int reqid, int ClientId, string ReferrerUrl)
        {
            objDataServer.SaveDoNotSellMyInfo(fstName, lstName, email, presite, reqid, ClientId);
        }
        #endregion

        #region Change Password 
        public int ChangePassword(string OldPassword, string NewPassword, string CnfNewPassword, string ug)
        {
            int result = 0;
            result = objDataServer.ChangePassword(OldPassword, NewPassword, CnfNewPassword, ug);
            return result;
        }
        #endregion

        #region Get Reward Access Password 
        public int GetRewardAccessPwd(string ug, string password)
        {
            int result = 0;
            result = objDataServer.GetRewardAccessPwd(ug, password);
            return result;
        }
        #endregion

        #region Sub id Check for Local blox
        /// <summary>
        /// Sub id Check for Local blox
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="ClientId"></param>
        /// <param name="subid"></param>
        /// <returns></returns>
        public User SubidCheck(string EmailAddress, int ClientId, string subid)
        {
            return objDataServer.SubidCheck(EmailAddress, ClientId, subid);
        }
        #endregion

        #region Update IsVerified
        public int UpdateIsVerified(string UserGUID, int Rid)
        {
            return objDataServer.UpdateIsVerified(UserGUID, Rid);
        }
        #endregion

        #region pii confirmation
        public string piiconfirm(Guid UserGUID)
        {
            return objDataServer.piiconfirm(UserGUID);
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

        #region PostbackScript
        /// <summary>
        /// PostbackScript
        /// </summary>
        /// <param name="leadid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public pixel PostbackScript(string leadid, int flag)
        {
            return objDataServer.PostbackScript(leadid, flag);
        }
        #endregion

        public int InserClicks(string SubId1, string SubId2, string SubId3, string Ipaddress, string Referrerurl, int PageNo, int CpatchaFlag, int OpttiburonsplitFlag)
        {

            return objDataServer.InserClicks(SubId1, SubId2, SubId3, Ipaddress, Referrerurl, PageNo, CpatchaFlag, OpttiburonsplitFlag);
        }

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

        #region
        public string InsertAffClicks(int rid, string SubID3, string IPAddress, string sid, string fid, string trans_id, string RefererUrl, int isClick)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.InsertAffClicks(rid, SubID3, IPAddress, sid, fid, trans_id, RefererUrl, isClick);
        }
        #endregion

        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop20(Guid UserGUID, string OrgGUID, int ispeerly2)
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

        public string surveygetbyprjid(string prjid, int rid, string IPAddress)
        {
            string url = string.Empty;
            url = objDataServer.surveygetbyprjid(prjid, rid, IPAddress);
            return url;
        }

        #region SparkPost Suppression Delete
        public string suppressionDeleted(string EmailAddress)
        {
            string sparkpostUrl = string.Empty;
            string result = string.Empty;
            sparkpostUrl = ConfigurationManager.AppSettings["sparkposturl"].ToString();
            try
            {
                string api = sparkpostUrl + EmailAddress;
                var response = GetMethod(api);
                if (!string.IsNullOrEmpty(response))
                {
                    var details = Newtonsoft.Json.JsonConvert.DeserializeObject<SuppresionListResults>(response);
                    var nonTransactional = details.results.Where(re => re.non_transactional == true).Select(re => re).FirstOrDefault();
                    if (nonTransactional != null)
                    {
                        DeleteMethod(sparkpostUrl + nonTransactional.recipient, nonTransactional.subaccount_id);
                    }
                    else
                    {
                        result = "User not available in suppression list";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }
        #endregion

        #region GetMethod
        public string GetMethod(string RequestURL)
        {
            String strMainPage = string.Empty;
            StreamReader sr = null;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
                LoginRequest.Method = "GET";
                LoginRequest.Headers.Add("Authorization", "feaca7da84add98d9d18c04e52dc3ed42a3fe27e");
                LoginRequest.ContentType = "application/json; charset=UTF-8";
                HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
                string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
                sr = new StreamReader(LoginResponse.GetResponseStream());
                strMainPage = sr.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //sr.Close();
            }

            return strMainPage;
        }
        #endregion

        #region DeleteMethod
        public string DeleteMethod(string RequestURL, int subacountId)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "DELETE";
            LoginRequest.Headers.Add("Authorization", "feaca7da84add98d9d18c04e52dc3ed42a3fe27e");
            LoginRequest.Headers.Add("X-MSYS-SUBACCOUNT", subacountId.ToString());
            LoginRequest.ContentType = "application/json; charset=UTF-8";
            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }
        #endregion

        #region
        public string GetSurveyURL(int pid, int rid, string IPAddress, int userTrafficTypeId, string mobiledeviceModel, string browserInfo, string AgentInfo, string extid)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.GetSurveyURL(pid, rid, IPAddress, userTrafficTypeId, mobiledeviceModel, browserInfo, AgentInfo, extid);
        }
        #endregion

        #region Check member Exists
        public Boolean CheckMemberExist(string ug, int cid)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.CheckMemberExist(ug, cid);
        }
        #endregion

        #region Get IP Address
        public string GetIPAddress(string IPAddress, string IPNumber)
        {
            UserDataServices oUserDataServices = new UserDataServices();
            return oUserDataServices.GetIPAddress(IPAddress, IPNumber);
        }
        #endregion
    }


}
