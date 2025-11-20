using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
using System.Net.Http;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class EndLinksManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        EndLinksDataServer oServer = new EndLinksDataServer();

        public SentryEndURL SentryValidationCheck(Guid uig, string sentry_status, int cid)
        {
            return oServer.SentryValidationCheck(uig, sentry_status, cid);
        }
        public Surveys GetSurveyInvitationStatus(Guid statusGuid, Guid invitationGuid, Decimal cost, int federatedProjectId, string car, string sdv, string ApipartnerStatus)
        {

            return oServer.GetSurveyInvitationStatus(statusGuid, invitationGuid, cost, federatedProjectId, car, sdv, ApipartnerStatus);
        }
        public bool SurveyActivityInsert(Surveys oSurveys, Guid invitationGuid, string ApipartnerStatus, string ApiclientStatus)
        {

            return oServer.SurveyActivityInsert(oSurveys, invitationGuid, ApipartnerStatus, ApiclientStatus);
        }

        public string GetOrgInfo(int clientid)
        {
            return oServer.GetOrgInfo(clientid);
        }
        public void PartnerTransInsert(Surveys oSurveys, Guid invitationGuid, decimal cost)
        {

            oServer.PartnerTransInsert(oSurveys, invitationGuid, cost);
        }
        #region Take Another Survey
        /// <summary>
        ///  Take Another Survey
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="uig"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public Surveys TakeAnotherSurvey(Guid ug, Guid uig, string Source, int top1, int clientid)
        {

            return oServer.TakeAnotherSurvey(ug, uig, Source, top1, clientid);
        }
        #endregion

        #region Get Router Survey URL
        /// <summary>
        ///  Take Another Survey
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="uig"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public Surveys GetRouterSurveyURL(Guid ug, Guid uig, string Source, int clientid, string u, string ipAddress)
        {
            Surveys objSurveys = new Surveys();
            string surveyURL = string.Empty;
            int userTrafficTypeId = 2;
            List<Surveys> lstSurveys = new List<Surveys>();
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var content = new StringContent("", Encoding.UTF8, "application/json");
            string Url = ConfigurationManager.AppSettings["gsapiurl"].ToString();
            client.BaseAddress = new Uri(Url);
            //HTTP GET
            var responseTask = client.GetStringAsync("SurveysGet?userGuid=" + ug + "&clientId=" + clientid + "&ipAddress=" + ipAddress);
            responseTask.Wait();
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
            //if (jsonString.ToLower() != ("no survey was found for your profile"))
            {
                lstSurveys = null;
            }
            else
            {

                lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
                if (!string.IsNullOrEmpty(lstSurveys[0].SurveyUserTypeIds))
                {
                    lstSurveys = lstSurveys.Where(survey => survey.SurveyUserTypeIds.TrimEnd(';').Split(';').Where(surveytypeid => surveytypeid != "").Select(surveytypeid => Convert.ToInt32(surveytypeid)).Contains(userTrafficTypeId)).ToList();
                }
                else
                {
                    lstSurveys = lstSurveys.Where(survey => survey.SurveyTrafficType.TrimEnd(';').Split(';').Where(surveytypeid => surveytypeid != "").Select(surveytypeid => Convert.ToInt32(surveytypeid)).Contains(userTrafficTypeId)).ToList();
                }
                if (lstSurveys.Count > 0)
                {
                    if (!string.IsNullOrEmpty(lstSurveys[0].SurveyUrl))
                    {
                        objSurveys = lstSurveys[0];
                        //var uri = new Uri(lstSurveys[0].SurveyUrl);
                        //var replaceParam = HttpUtility.ParseQueryString(uri.Query);
                        //replaceParam.Set("s", Source);
                        //var uriBuilder = new UriBuilder(uri);
                        //uriBuilder.Query = replaceParam.ToString();
                        //var newUri = uriBuilder.Uri;
                        //surveyURL = newUri.ToString();

                    }
                }
            }
            return objSurveys;
        }
        #endregion

        public InterstitialResponse InterstialBeforeCaptcha(Guid ug, Guid uig, string sr, int cid, string cc, string fc, string pid, string sentry_status, string conid)
        {
            InterstitialRequest request = new InterstitialRequest()
            {
               UserGuid = ug,
               UserInvitationGuid = uig,
               ShowRecaptcha = sr,
               ClientId = cid,
               CountryId = string.IsNullOrEmpty(conid) ? 0 : Convert.ToInt32(conid),
               FirstClick = fc,
               ProjectId = string.IsNullOrEmpty(pid) ? 0 : Convert.ToInt32(pid),
               SentryStatus = sentry_status,
               RequestUriQuery = HttpContext.Current.Request.Url.Query,
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["endlink_interstitial_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<InterstitialResponse>(result);
        }

        public string InterstialAfterCaptcha(Guid ug, Guid uig, string sr, int cid)
        {
            InterstitialRequest request = new InterstitialRequest()
            {
                UserGuid = ug,
                UserInvitationGuid = uig,
                ShowRecaptcha = sr,
                ClientId = cid,
                IsSaveRecaptcha = true
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["endlink_interstitial_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            var interstitialResponse = JsonConvert.DeserializeObject<InterstitialResponse>(result);
            return interstitialResponse.RedirectURL;
        }

        public Recaptcha GetRecaptchaEntryInfo(Guid uig, Guid ug, int clientid)
        {
            return oServer.GetRecaptchaEntryInfo(uig, ug, clientid);
        }
        #region Quota Completes/accesses
        public List<Quotas> GetQuotaCompletesaccesses(int prjId, int balaneTypeId, Guid ug, int clientid)
        {
            return oServer.GetQuotaCompletesaccesses(prjId, balaneTypeId, ug, clientid);

        }
        #endregion

        #region Quota status Update
        public string QuotaStatusUpdate(int prjId, string json, int ClientID, Guid uig)
        {
            return oServer.QuotaStatusUpdate(prjId, json, ClientID, uig);
        }
        #endregion
        #region Selected Quotas get

        public Recaptcha GetSelectedQuotas(Guid uig, Guid ug, string json, bool isInternalMem, int prjId, int userId, Int64 userInvitationId, int clientid)
        {
            return oServer.GetSelectedQuotas(uig, ug, json, isInternalMem, prjId, userId, userInvitationId, clientid);
        }
        #endregion
        #region Insert Mached Quotas
        public void InsertMatchedQuotas(Guid ug, int prjId, int userId, bool isInternalMem, Int64 userInvitationId, int activityTypeId, string matchedQuotas, Guid uig, int clientid, int targetId, string fedResponseId)
        {
            oServer.InsertMatchedQuotas(ug, prjId, userId, isInternalMem, userInvitationId, activityTypeId, matchedQuotas, uig, clientid, targetId, fedResponseId);
        }
        #endregion
        #region SurveyUrl Get
        public SurveyUrl GetSurveyUrl(Guid ug, Guid uig, string source, int clientid)
        {
            return oServer.GetSurveyUrl(ug, uig, source, clientid);
        }
        #endregion

        #region Project Refresh
        public void ProjectRefresh(Guid projectGuid, string projectManagerEmailAddress, string Isr, double cpi)
        {
            oServer.ProjectRefresh(projectGuid, projectManagerEmailAddress, Isr, cpi);
        }
        #endregion

        #region Get Cookie
        public string GetCookie(Guid ug, int cid)
        {
            return oServer.GetCookie(ug, cid);
        }
        #endregion

        #region Save Cookie
        public void saveCookie(Guid ug, string CookieIds, int cid)
        {
            oServer.saveCookie(ug, CookieIds, cid);
        }
        #endregion

        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop20(Guid leadguid)
        {
            return oServer.GetquestionsforTop20(leadguid);
        }
        #endregion

        #region Top20SaveOptions
        /// <summary>
        /// Top20SaveOptions
        /// </summary>
        /// <param name="listXml"></param>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public string Top20SaveOptions(string listXml, Guid leadguid)
        {
            return oServer.Top20SaveOptions(listXml, leadguid);
        }
        #endregion

        #region Insert Exception Data
        public void InsertExceptionData(Surveys oSurveys, string response, string key, string url, string json)
        {
            oServer.InsertExceptionData(oSurveys, response, key, url, json);
        }
        #endregion

        public string GetProfileAsURLParams(string ug, int client_id)
        {
            string param = string.Empty;
            param = oServer.GetUserProfileQueryParameters(ug, client_id);
            return param;

        }

        #region Get Client ID   
        public Client GetClientDetails(int pid)
        {
            return oServer.GetClientDetails(pid);
        }
        #endregion

        #region Is Toluna Member Exist
        public TolunaUser IsTolunaMemberExist(Guid UserGuid, int cid)
        {
            return oServer.IsTolunaMemberExist(UserGuid, cid);
        }
        #endregion

        #region Toluna Update User
        public void TolunaUpdateUser(Guid ug, int clientid)
        {
            oServer.TolunaUpdateUser(ug, clientid);
        }
        #endregion

        public M2MToken GetM2MToken()
        {
            M2MToken m2mToken = new M2MToken();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient httpClient = new HttpClient();
                var payload = new M2MTokenRequest()
                {
                    grant_type = "client_credentials",
                    client_id = ConfigurationManager.AppSettings["M2MClientID"],
                    client_secret = ConfigurationManager.AppSettings["M2MClientSecretKey"],
                    audience = ConfigurationManager.AppSettings["M2MClientAudience"]
                };
                string _tenantDomain = ConfigurationManager.AppSettings["BeraTenantDomain"];
                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync($"{_tenantDomain}/oauth/token", content).Result;
                m2mToken = JsonConvert.DeserializeObject<M2MToken>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
            }
            return m2mToken;
        }

        public ReserveCatBrands RespondentReserve(int ProjectID, JObject jObj, string _accessToken, string _survey_API_Url)
        {
            ReserveCatBrands brands = new ReserveCatBrands();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                string json = JsonConvert.SerializeObject(jObj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{_survey_API_Url}/respondent/reserve", content).Result;
                logger.Trace($"Bera Error Response:  {response.ToString()}");
                brands = JsonConvert.DeserializeObject<ReserveCatBrands>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
            }
            return brands;
        }
    }
}
