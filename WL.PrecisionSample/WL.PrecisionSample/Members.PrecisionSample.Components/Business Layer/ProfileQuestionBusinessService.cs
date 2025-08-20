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
    public class ProfileQuestionBusinessService
    {
        ProfileQuestionDataLayer objDataServer = new ProfileQuestionDataLayer();

        #region Get Top1 pre prescreener questions
        /// <summary>
        /// Get Top1 pre prescreener questions
        /// </summary>
        /// <param name="uig"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public PiiResponse GetQuestion(Guid Uig, Guid Ug, int ProjectId, int TargetId, int UserId, int cid, string clientIp,string dvtype)
        {
            PiiRequest request = new PiiRequest()
            {
                ClientIP = clientIp,
                IsSaveResponse = false,
                UserId = UserId,
                UserGuid = Ug.ToString(),
                ProjectId = ProjectId,
                TargetId = TargetId,
                DeviceType = dvtype,
                UserInvitationGuid = Uig.ToString(),
                ClientId = cid
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_pii_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<PiiResponse>(result);
            //return objDataServer.GetQuestion(Uig, Ug, ProjectId, TargetId, UserId, cid);
        }
        #endregion

        #region Save Answers And Return Question
        /// <summary>
        /// Save Answers And Return Question
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="qid"></param>
        /// <param name="otext"></param>
        /// <param name="optId"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public PiiResponse SaveResponse(Guid Uig, int qid, string otext, int optId, Guid Ug, int clientid,int userId, string address1, string address2, string city, string zip, string clientIp,string dvtype)
        {
            PiiRequest request = new PiiRequest()
            {
                ClientIP = clientIp,
                IsSaveResponse = true,
                UserGuid = Ug.ToString(),
                ClientId = clientid,
                QuestionId = qid,
                OptionText = otext,
                OptionId = optId,
                City = city,
                ZipCode = zip,
                UserId = userId,
                Address1 = address1,
                Address2 = address2,
                DeviceType = dvtype,
                UserInvitationGuid = Uig.ToString()
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_pii_path"].ToString(), content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<PiiResponse>(result);
            //return objDataServer.SaveResponse(Uig, qid, otext, optId, Ug, clientid, address1, address2, city, zip);
        }
        #endregion

        //#region Get User Data
        ///// <summary>
        /////  Get User Data
        ///// </summary>
        ///// <param name="Ug"></param>
        ///// <returns></returns>
        //public User GetUserData(Guid Ug)
        //{
        //    return objDataServer.GetUserData(Ug);
        //}
        //#endregion

        # region Save Verity Repsonse
        /// <summary>
        /// Save Verity Repsonse
        /// </summary>
        /// <param name="Ug"></param>
        /// <param name="Uig"></param>
        /// <param name="verityScore"></param>
        /// <param name="verityId"></param>
        /// <param name="geoCorrelationFlag"></param>
        public void SaveVerityRepsonse(Guid Ug, Guid Uig, int verityScore, string verityId, int geoCorrelationFlag, int clientId)
        {
            objDataServer.SaveVerityRepsonse(Ug, Uig, verityScore, verityId, geoCorrelationFlag, clientId);
        }
        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid, int ClientId)
        {
            return objDataServer.GetquestionsforTop10(UserGuid, ClientId);
        }
        #endregion

        #region Top10SaveOptions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listXml"></param>
        public string Top10SaveOptions(string listXml, Guid UserGuid, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                       int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                       string RobotFlag, string ErrorMessage, int ClientId)
        {
            return objDataServer.Top10SaveOptions(listXml, UserGuid, ResponseText, Rq1, Rq2, Rq3, Rq4, RealAnswerScore, BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag,
                                       RobotFlag, ErrorMessage, ClientId);
        }
        #endregion

        #region Get Profile questions
        /// <summary>
        /// Get Profile questions
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetProfileQuestions(Guid ProfileId, Guid UserGuid, int Clientid)
        {
            return objDataServer.GetProfileQuestions(ProfileId, UserGuid, Clientid);
        }

        #endregion

        #region Save Profile Options
        /// <summary>
        /// Save Profile Options
        /// </summary>
        /// <param name="listXml">Profile Data Xml</param>
        /// <param name="UserGuid">user Guid</param>
        public void ProfileSave(String listXml, Guid UserGuid, int ClientId)
        {
            objDataServer.ProfileSave(listXml, UserGuid, ClientId);
        }

        #endregion

        #region Get Profile Responses
        /// <summary>
        /// Get Profile Responses For FusionCash and Vindale
        /// </summary>
        /// <param name="UserGuid">UsetGuid</param>
        /// <param name="ClientId">ClientId</param>
        /// <param name="PfId">ProfileId</param>
        /// <returns></returns>
        public string GetProfileResponse(Guid UserGuid, int ClientId, string PfId)
        {
            return objDataServer.GetProfileResponse(UserGuid, ClientId, PfId);
        }
        #endregion

        #region  Get Profile PixelDetails
        /// <summary>
        /// Get Profile Responses For FusionCash and Vindale
        /// </summary>
        /// <param name="UserGuid">UsetGuid</param>
        /// <param name="ClientId">ClientId</param>
        /// <param name="PfId">ProfileId</param>
        /// <returns></returns>
        public UserEntity GetProfilePixelDetails(Guid UserGuid, int ClientId, string ProfileId)
        {
            return objDataServer.GetProfilePixelDetails(UserGuid, ClientId, ProfileId);
        }
        #endregion

        #region gdprCompliancesave
        public string gdprCompliancesave(string userGuid, int Usid, int cid, string uid, string uig, string clientIp,string dvtype)
        {
            PriRequest request = new PriRequest()
            {
                ClientIP = clientIp,
                ClientId = cid,
                UserId = Usid,
                UserGuid = userGuid,
                UserInvitationGuid = uig,
                UserInvitationId = Convert.ToInt64(uid),
                DeviceType = dvtype
            };
            HttpClient client = new HttpClient();
            string json = JsonConvert.SerializeObject(request);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["gateway_click_base_url"].ToString());
            StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(ConfigurationManager.AppSettings["click_priterms_path"].ToString(), content).Result;
            string output = response.Content.ReadAsStringAsync().Result;
            return output;
        }
        #endregion
    }
}
