using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using System.Net;
using System.IO;
using NLog;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ClickflowManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region public Varialbes
        ClickflowDataServer oDataServer = new ClickflowDataServer();
        string GrantType = ConfigurationManager.AppSettings["grant_type"].ToString();
        string UserType = ConfigurationManager.AppSettings["user_type"].ToString();
        #endregion

        #region Save User Click Inforamtion
        /// <summary>
        ///Save User Click Inforamtion
        /// </summary>
        /// <param name="QgId">QuotaGroupId</param>
        /// <param name="UgId">UserGuid</param>
        /// <param name="PrjId">ProjectId</param>
        /// <param name="Rid">ReferrerId</param>
        /// <param name="Soruce">Source</param>
        /// <param name="SubId">SubId</param>
        /// <param name="IsNew">IsNew</param>
        /// <param name="UserTrafficTypeId">UserTrafficOd</param>
        /// <param name="MobiledeviceModel">DeviceModel</param>
        /// <param name="BrowserInfo">BrowserInfo</param>
        /// <param name="AgentInfo">AgentInfo</param>
        /// <param name="IpAddress">IpAddress</param>
        /// <param name="RelevantId">RelevantId</param>
        /// <param name="RelevantScore">RelevantScore</param>
        /// <param name="FpfScores">FpfScore</param>
        /// <param name="FraudProfilefScore">FraudPfScore</param>
        /// <param name="OldSurveyInvitationId">OldSurveyInvitationd</param>
        /// <returns></returns>
        public Surveys SaveClickInformation(string QgId, string UgId, int PrjId, int cid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                            string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore, string OldSurveyInvitationId, string Vid, int Vscore, string fedresid, string geodata, string IPriskScore, string IPNumber)
        {
            return oDataServer.SaveClickInformation(QgId, UgId, PrjId, cid, Rid, Source, SubId, IsNew, UserTrafficTypeId, MobiledeviceModel, BrowserInfo, AgentInfo, IpAddress, RelevantId,
                              RelevantScore, FpfScores, FraudProfilefScore, OldSurveyInvitationId, Vid, Vscore, fedresid, geodata, IPriskScore, IPNumber);
        }
        #endregion

        #region SendSms
        /// <summary>
        /// SendSms
        /// </summary>
        /// <param name="UserInvitaitonGuid">UserInvitaitonGuid</param>
        /// <param name="UserGuid">UserguId</param>
        /// <param name="ProjectId">UserguId</param>
        /// <param name="MobileNumber">MobileNumber</param>
        /// <param name="SurveyName">SurveyName</param>
        /// <param name="OrgId">OrgId</param>
        public int SendSms(string UserInvitationGuid, string UserGuid, string ProjectId, string MobileNumber, string SurveyName, string OrgId)
        {
            return oDataServer.SendSms(UserInvitationGuid, UserGuid, ProjectId, MobileNumber, SurveyName, OrgId);
        }
        #endregion

        #region checkradius
        /// <summary>
        /// Check Radius
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="geodata"></param>
        /// <returns></returns>

        public int checkradius(string ug, string geodata)
        {
            return oDataServer.checkradius(ug, geodata);
        }
        #endregion

        #region GetUserViertyDetails
        /// <summary>
        /// Get User VerityDetails
        /// </summary>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public Surveys GetUserVerityDetails(Guid UserGuid, Guid UserInvitationGuid, int cid, int pid, int tid, int usid)
        {
            return oDataServer.GetUserVerityDetails(UserGuid, cid, UserInvitationGuid, pid, tid, usid);
        }
        #endregion

        #region Save Veriy
        /// <summary>
        /// Save Verity
        /// </summary>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="VerityScore">VerityScore</param>
        /// <param name="VerityId">VerityId</param>
        /// <param name="ChallangeId">ChallangeId</param>
        /// <param name="GeoCorrelationFlag">GeoCorrekationFlag</param>
        /// <param name="QstText1">QuestionText1</param>
        /// <param name="QstText2">QuestionText2</param>
        /// <param name="QstText3">QuestionText3</param>
        /// <param name="OptText1">OptionText1</param>
        /// <param name="OptText2">OptionText2</param>
        /// <param name="OptText3">OptionText3</param>
        /// <param name="VerityDOBFail">VerityDOBFail</param>
        /// <returns></returns>
        public Surveys SaveVerityQuestions(Guid UserInvitationGuid, Guid UserGuid, int cid, int VerityScore, string VerityId, string ChallangeId, int GeoCorrelationFlag, string QstText1, string QstText2,
            string QstText3, string OptText1, string OptText2, string OptText3, bool VerityDOBFail)
        {
            return oDataServer.SaveVerityQuestions(UserInvitationGuid, UserGuid, cid, VerityScore, VerityId, ChallangeId, GeoCorrelationFlag, QstText1, QstText2,
              QstText3, OptText1, OptText2, OptText3, VerityDOBFail);
        }
        #endregion

        #region Get Verity ChallengeQuestions
        /// <summary>
        /// Get VerityQuestions
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public List<VerityChallengeResponse> GetVerityQuestions(Guid InvitationGuid, Guid userGuid, int cid, int pid, int tid, int usid, string dvtype)
        {
            VerityChallengeRequest request = new VerityChallengeRequest()
            {
                IsSaveResponse = false,
                UserId = usid,
                UserGuid = userGuid.ToString(),
                ProjectId = pid,
                TargetId = tid,
                ClientId = cid,
                DeviceType = dvtype,
                UserInvitationGuid = InvitationGuid.ToString()
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_verity_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<VerityChallengeResponse>>(result);
        }
        #endregion

        #region Save Challange Question Response
        /// <summary>
        /// Save Verity Challenge Questions
        /// </summary>
        /// <param name="ChallangeScore">ChallangeScore</param>
        /// <param name="ChallangeId">ChallengeId</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitaitonGuid</param>
        /// <param name="Option1">Option1</param>
        /// <param name="Option2">Option2</param>
        /// <param name="Option3">Option3</param>
        /// <param name="ErrorMsg">Error</param>
        /// <returns></returns>
        public VerityChallengeResponse SaveChallangeQuestionResponse(Guid UserGuid, int cid, Guid UserInvitationGuid, List<VerityEnhancedQuestions> oQuestions, string dvtype)
        {
            VerityChallengeRequest request = new VerityChallengeRequest()
            {
                IsSaveResponse = true,
                UserGuid = UserGuid.ToString(),
                ClientId = cid,
                SelectedAnswer1 = oQuestions[0].SelectedAnsId,
                SelectedAnswer2 = oQuestions[1].SelectedAnsId,
                SelectedAnswer3 = oQuestions[2].SelectedAnsId,
                Option1 = oQuestions[0].AnswerText,
                Option2 = oQuestions[1].AnswerText,
                Option3 = oQuestions[2].AnswerText,
                UserInvitationGuid = UserInvitationGuid.ToString(),
                DeviceType = dvtype
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_verity_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<VerityChallengeResponse>(result);
        }
        #endregion

        #region skip verity questions
        /// <summary>
        /// Skip Verity Enchance Questions
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitatinGuid</param>
        public void SkipVerityQuestions(Guid UserGuid, Guid UserInvitationGuid)
        {
            // oDataServer.SkipVerityQuestions(UserGuid, UserInvitationGuid);
        }
        #endregion

        #region Get OrgId by userDPV
        /// <summary>
        /// Get Connectionsttring by userguids
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public int GetOrgidByUserDPV(string UserGuid)
        {
            return oDataServer.GetOrgidByUserDPV(UserGuid);
        }
        #endregion

        #region InsertIpRiskScroe
        public void insertIpRiskScore(string Ipaddress, string IPriskScore)
        {
            oDataServer.insertIpRiskScore(Ipaddress, IPriskScore);
        }
        #endregion

        #region Get Ipaddress
        public string GetIpRiskScore(string Ipaddress)
        {
            return oDataServer.GetIpRiskScore(Ipaddress);

        }
        #endregion

        #region Insert Survey Click
        //public Surveys InsertClick(string QgId, string UgId, int PrjId, int ClientId, string Rid, string Source, string SubId, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
        //                  string AgentInfo, string IpAddress, string OldSurveyInvitationId, string fedresid, string IPNumber, string ReaserchDefender, bool isEligible)
        //{
        //    return oDataServer.InsertClick(QgId, UgId, PrjId, ClientId, Rid, Source, SubId, UserTrafficTypeId, MobiledeviceModel, BrowserInfo, AgentInfo, IpAddress, OldSurveyInvitationId, fedresid, IPNumber, ReaserchDefender, isEligible);
        //}
        #endregion

        public string InsertRelevantData(Guid UserinvitationGuid, string RelevantId, int RelevantScore, string FPFscore, int FraudProfileScore,
           Boolean IsNew, string Browserinfo, string AgentInfo, int UserID, int ClientId, string geodata)
        {
            return oDataServer.InsertRelevantData(UserinvitationGuid, RelevantId, RelevantScore, FPFscore, FraudProfileScore,
            IsNew, Browserinfo, AgentInfo, UserID, ClientId, geodata);
        }

        public RelevantCheck Check(Int64 UserInvitationId, int cid)
        {
            return oDataServer.CheckRelevantScore(UserInvitationId, cid);
        }
        public string RdjsonInsert(int userid, string uid, int cid, string uig, string ug, string clientIp, string dvtype,string sessionId, string dfiqJson)
        {
            //return oDataServer.RdjsonInsert(userid, uid, cid, uig, CleanIDJson, IpqsJSON);
            ValidRequest request = new ValidRequest()
            {
                ClientIP = clientIp,
                UserId = userid,
                UserInvitationGuid = uig,
                UserInvitationId = uid,
                UserGuid = ug,
                ClientId = cid,
                //IpqsJSON = IpqsJSON,
                DeviceType = dvtype,
                SessionId = sessionId,
                //DfiqJson = dfiqJson
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_valid_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        public string BrowserInfo(System.Web.HttpBrowserCapabilitiesBase browser)
        {
            string browserInfo = string.Empty;
            if (browser != null)
            {
                browserInfo = "Type = " + browser.Type + "|"
                    + "Name = " + browser.Browser + "|"
                    + "Version = " + browser.Version + "|"
                    + "Major Version = " + browser.MajorVersion + "|"
                    + "Minor Version = " + browser.MinorVersion + "|"
                    + "Platform = " + browser.Platform;
            }
            return browserInfo;
        }
        public string DeviceTypeCheck(string UserAgent)
        {
            var mobiledeviceModel = string.Empty;
            if (UserAgent.Contains("Android"))
            {
                mobiledeviceModel = "Android";
            }
            else if (UserAgent.Contains("webOS"))
            {
                mobiledeviceModel = "webOS";
            }
            else if (UserAgent.Contains("iPhone"))
            {
                mobiledeviceModel = "iPhone";
            }
            else if (UserAgent.Contains("iPad"))
            {
                mobiledeviceModel = "iPad";
            }
            else if (UserAgent.Contains("iPod"))
            {
                mobiledeviceModel = "iPod";
            }
            else if (UserAgent.Contains("BlackBerry"))
            {
                mobiledeviceModel = "BlackBerry";
            }
            else if (UserAgent.Contains("Windows Phone"))
            {
                mobiledeviceModel = "Windows Phone";
            }
            return mobiledeviceModel;
        }

        public string InsertClick(string QgId, string UgId, int PrjId, int ClientId, string Rid, string Source, string SubId, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                  string AgentInfo, string IpAddress, string OldSurveyInvitationId, string fedresid, string IPNumber)
        {
            ClickRequest request = new ClickRequest()
            {
                ClientId = ClientId,
                UserGuid = UgId,
                ProjectId = PrjId,
                TargetGuid = QgId,
                OldSurveyInvitationId = OldSurveyInvitationId,
                Source = Source,
                RId = Rid,
                SubId = SubId,
                UserTrafficTypeId = UserTrafficTypeId,
                MobiledeviceModel = MobiledeviceModel,
                BrowserInfo = BrowserInfo,
                AgentInfo = AgentInfo,
                IpAddress = IpAddress,
                FedResId = fedresid,
                IPNumber = IPNumber
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public HttpWebResponse Get(string url, string key)
        {
            HttpWebResponse res = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Headers.Clear();
                //request.Headers.Remove("Host");
                if (!url.Contains("get_token"))
                {
                    request.Headers.Add("token", key);
                }
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                logger.Trace("Research Defender api call Get Method" + ex.ToString());
                return null;
            }
            return res;
        }

        #region Get Client Details By Rid
        /// <summary>
        /// Get Client Details By Rid
        /// </summary>
        /// <param name="RId">rid</param>
        /// <returns></returns>
        public Client GetClientDetailsByRid(string DomainUrl, int? RId = null, int? ClientId = null)
        {
            Client oClient = new Client();
            PriRequest request = new PriRequest()
            {
                ClientId = ClientId ?? 0,
                HostUrl = DomainUrl,
                NeedOrgLogo = true,
            };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_priterms_path"].ToString(), content).Result;
            string orgLogo = response.Content.ReadAsStringAsync().Result;
            oClient.OrgLogo = string.IsNullOrEmpty(orgLogo) ? null : orgLogo;
            return oClient;
        }
        #endregion

        #region GetAuthToken
        public string GetAuthToken()
        {
            APInfo info = GetApiInfo();
            string authURL = info.ProdAPIURL + "token";
            string authToken = GetAuthToken(authURL, GrantType, info.UserName, info.Password, UserType);
            dynamic respData = JObject.Parse(authToken);
            return respData.access_token;
        }
        #endregion

        #region GetOutsidePanelProjectId
        public string GetOutSideProjectId(string ProjectId)
        {
            return oDataServer.GetOutSideProjectId(ProjectId);
        }
        #endregion

        #region GetApiInfo
        public APInfo GetApiInfo()
        {
            return oDataServer.GetApInfo();
        }
        #endregion

        #region getIpsosEligibility
        public IpsosEligibility getIpsosEligibility(string ipaddress, string token, string surveyId)
        {
            string ipsosUrl = string.Empty;
            string RequestURL = string.Empty;
            IpsosEligibility IpsosEligibility = null;
            try
            {
                Stream dataStream;
                ipsosUrl = System.Configuration.ConfigurationManager.AppSettings["ipsos"].ToString();
                RequestURL = string.Format(ipsosUrl, surveyId, ipaddress);
                Dictionary<string, string> headers = new Dictionary<string, string>() { { "Authorization", "Bearer " + token } };
                WebResponse response = IpsosGet(RequestURL, headers);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //Console.WriteLine(reader.ReadToEnd());
                IpsosEligibility = JsonConvert.DeserializeObject<IpsosEligibility>(reader.ReadToEnd());
                return IpsosEligibility;
            }
            catch (Exception ex)
            {
                logger.Trace("Get Ipsos Eligibility Method" + ex.ToString());
                return IpsosEligibility;
            }
        }
        #endregion

        #region Get
        public static HttpWebResponse IpsosGet(string url, Dictionary<string, string> headers)
        {
            HttpWebResponse res = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                logger.Error($"Calling getIpsosEligibility {url} failed with {ex.ToString()} ");
            }

            return res;

        }
        #endregion

        #region GetAuthToken
        public string GetAuthToken(string url, string grant_type, string username, string password, string user_type)
        {
            string authToken = string.Empty;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            var postData = "grant_type=" + Uri.EscapeDataString(grant_type);
            postData += "&username=" + Uri.EscapeDataString(username);
            postData += "&password=" + Uri.EscapeDataString(password);
            var data = Encoding.UTF8.GetBytes(postData);
            Dictionary<string, string> headers = new Dictionary<string, string>() {
                    { "ContentType", "application/x-www-form-urlencoded" },
                    { "UserType",user_type}
            };
            Stream dataStream;
            WebResponse response = Post(url, headers, data);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        #endregion

        #region GetAuthTokenPost
        public HttpWebResponse Post(string url, Dictionary<string, string> headers, byte[] payload = null)
        {
            HttpWebResponse response = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                if (payload != null)
                {
                    request.ContentLength = payload.Length;
                }
                else
                {
                    request.ContentLength = 0;
                    payload = new byte[0];
                }
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(payload, 0, payload.Length);
                response = (HttpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {
                logger.Error($"Calling Post API {url} failed with {ex.ToString()} ");
            }
            return response;
        }
        #endregion

        #region ipsos eligibility check by IP
        public bool IsEligibleForIPSOS(string IP, string surveyId)
        {
            string token = string.Empty;
            token = GetAuthToken();
            if (string.IsNullOrEmpty(token))
            {
                logger.Trace($"getIpsosEligibility | Token : {token}");
                return true;
            }
            var eligibilty = getIpsosEligibility(IP, token, surveyId);
            logger.Trace($"IPAddress: {IP} | Token: {token} | SurveyId: {surveyId} | IsEligibleCheck: {eligibilty.isEligible}");
            if (eligibilty == null)
            {
                return true;
            }
            else if (eligibilty.isEligible == false) //Making IP Dupe 
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
