using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;

namespace Members.PrecisionSample.Components.Business_Layer
{

    public class PrescreenerManager
    {

        PrescreenerDataServices objDataServices = new PrescreenerDataServices();

        #region Get User MobileNumber
        /// <summary>
        /// Check User Mobile Number
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        /// <returns></returns>
        public Surveys GetUserMobileNumber(Guid UserGuid, Guid UserInvitationGuid, int ClientId)
        {
            return objDataServices.GetUserMobileNumber(UserGuid, UserInvitationGuid, ClientId);
        }
        #endregion

        #region Save user MobileNumber
        /// <summary>
        /// Check User Mobile Number
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        ///<param name="MobileNumber">Mobile Number</param>
        /// <returns></returns>
        public Surveys UpdateMobileNumber(Guid UserGuid, int ClientId, Guid UserInvitationGuid, string MobileNumber)
        {
            return objDataServices.UpdateUserMobileNumber(UserGuid, ClientId, UserInvitationGuid, MobileNumber);
        }
        #endregion

        #region Get project selected languages
        /// <summary>
        /// Get project selected languages
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        /// <returns></returns>
        public Question GetProjectSelectedLanguages(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            //return objDataServices.GetProjectSelectedLanguages(UserGuid, ClientId, UserInvitationGuid);
            PrescreenerLanguageRequest request = new PrescreenerLanguageRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                UserInvitationGuid = UserInvitationGuid.ToString()
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["psl_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Question>(result);
        }
        #endregion

        #region Save Prescreener Selected language
        /// <summary>
        /// Get project selected languages
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        /// <returns></returns>
        public string SavePrescreenerLanguage(Guid UserGuid, int ClientId, Guid UserInvitationGuid, int langid)
        {
            PrescreenerLanguageRequest request = new PrescreenerLanguageRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                UserInvitationGuid = UserInvitationGuid.ToString(),
                IsSavePrescreenerLanguage = true,
                LanguageId = langid
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["psl_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }
        #endregion

        #region Get Prescreener Question List By ProjectId
        /// <summary>
        /// Get User Prescree Questions
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public List<PSquestion> GetPSQuestions(Guid UserGuid, int ClientId, int LanguageId, Guid UserInvitationGuid)
        {
            //return objDataServices.GetPSQuestions(UserGuid, ClientId, LanguageId, UserInvitationGuid);
            PrescreenerRequest request = new PrescreenerRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                UserInvitationGuid = UserInvitationGuid.ToString(),
                LanguageId = LanguageId
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["prescreener_pending_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<PSquestion>>(result);
        }
        #endregion

        #region SaveUserPrescreenerptions
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <param name="QuestionId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <returns></returns>
        public List<PSquestion> SaveUserPrescreenerOptions(string Xml, Guid UserGuid, int ClientId, Guid UserInvitationGuid, int QuestionId, int SortOrder, int LanguageId, int GDPRValue)
        {
            //return objDataServices.SaveUserPrescreenerOptions(Xml, UserGuid, ClientId, UserInvitationGuid, QuestionId, SortOrder, LanguageId, GDPRValue);
            PrescreenerRequest request = new PrescreenerRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                IsSavePrescreenerResponse = true,
                GDPR = GDPRValue,
                XML = Xml,
                CurrentQuestionSortOrder = SortOrder,
                CurrentQuestionId = QuestionId,
                CurrentExecution = "save",
                UserInvitationGuid = UserInvitationGuid.ToString(),
                LanguageId = LanguageId
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["prescreener_pending_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<PSquestion>>(result);
        }
        #endregion

        #region Get Project Reward Details
        /// <summary>
        /// Get Project Reward Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public Surveys GetProjectRewardDetails(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            return objDataServices.GetProjectRewardDetails(UserGuid, ClientId, UserInvitationGuid);
        }
        #endregion

        #region Get profile prescreener
        /// <summary>
        /// Get User Prescree Questions
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetProfilePrescreener(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            PrescreenerProfileRequest request = new PrescreenerProfileRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                IsSaveResponse = false,
                UserInvitationGuid = UserInvitationGuid.ToString()
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["profile_prescreener_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<ProfileQuestions>>(result);
        }
        #endregion

        #region Save Profile Prescreener
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <param name="QuestionId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <returns></returns>
        public List<ProfileQuestions> SaveProfilePrescreener(string listXml, Guid UserGuid, Guid uig, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                       int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                       string RobotFlag, string ErrorMessage, int ClientId, string DetectedLangCode, string RelatedConfidenceScore, string RepeatedWordsPct)
        {
            PrescreenerProfileRequest request = new PrescreenerProfileRequest()
            {
                UserGuid = UserGuid.ToString(),
                ClientId = ClientId,
                IsSaveResponse = true,
                UserInvitationGuid = uig.ToString(),
                XML = listXml
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["profile_prescreener_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<ProfileQuestions>>(result);
        }
        #endregion

        #region get invitation guid
        /// <summary>
        ///  get invitation guid
        /// </summary>
        /// <param name="uig"></param>
        public ProfileQuestions GetInvitationGuid(Guid uig, int ClientId)
        {
            return objDataServices.GetInvitationGuid(uig, ClientId);
        }
        #endregion

        #region Get Cookie Data
        /// <summary>
        ///  get invitation guid
        /// </summary>
        /// <param name="uig"></param>
        public CcpaResponse GetCookieData(Guid ug, Guid uig, int ClientId)
        {
            CcpaRequest request = new CcpaRequest()
            {
                UserGuid = ug.ToString(),
                ClientId = ClientId,
                IsSaveCookie = false,
                UserInvitationGuid = uig.ToString()
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["ccpa_prescreener_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<CcpaResponse>(result);
        }
        #endregion

        #region SaveUserCookie
        /// <summary>
        ///  get invitation guid
        /// </summary>
        /// <param name="uig"></param>
        public void SaveUserCookie(Guid ug, int cid, bool ResonateCookie, bool TapadCookie)
        {
            CcpaRequest request = new CcpaRequest()
            {
                UserGuid = ug.ToString(),
                ClientId = cid,
                IsSaveCookie = true,
                IsResonateCookieDropped = ResonateCookie,
                IsTapadCookieDropped = TapadCookie
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            client.PostAsync(ConfigurationManager.AppSettings["ccpa_prescreener_path"].ToString(), content).Result.Dispose();
            //objDataServices.SaveUserCookie(ug, cid, ResonateCookie, TapadCookie);
        }
        #endregion

        #region Mix mind geo ip fraud
        /// <summary>
        /// Mix mind geo ip fraud
        /// </summary>
        /// <returns></returns>
        public Surveys saveZipcode(Guid ug, Guid uig, string zip, int cid)
        {
            return objDataServices.saveZipcode(ug, uig, zip, cid);
        }
        #endregion
    }
}
