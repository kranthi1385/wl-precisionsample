using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using Newtonsoft.Json;
using NLog;
using System.Net.Http;
using System.Configuration;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ExternalMembersManager
    {
        ExternalMemberDataLayer oDataLayer = new ExternalMemberDataLayer();
        public Surveys UpdateExternalMember(string QgId, string mid, string pid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                            string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore, string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl, string IPNumber, bool is_dupe, string external_member_id, int project_id, string external_member_guid)
        {
            return oDataLayer.UpdateExternalMember(QgId, mid, pid, Rid, Source, SubId, IsNew, UserTrafficTypeId, MobiledeviceModel, BrowserInfo, AgentInfo, IpAddress, RelevantId,
                              RelevantScore, FpfScores, FraudProfilefScore, OldSurveyInvitationId, fed_response_id, ecost, e_rm, e_rl, IPNumber, is_dupe, external_member_id, project_id, external_member_guid);
        }

        public Surveys CagSaveClickInformation(string QgId, string mid, int pid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                           string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore, string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl, string IPNumber)
        {
            return oDataLayer.CagSaveClickInformation(QgId, mid, pid, Rid, Source, SubId, IsNew, UserTrafficTypeId, MobiledeviceModel, BrowserInfo, AgentInfo, IpAddress, RelevantId,
                              RelevantScore, FpfScores, FraudProfilefScore, OldSurveyInvitationId, fed_response_id, ecost, e_rm, e_rl, IPNumber);
        }

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
            return oDataLayer.SendSms(UserInvitationGuid, UserGuid, ProjectId, MobileNumber, SurveyName, OrgId);
        }
        #endregion

        public string ExternalMemberByIdGet(Guid extMemGuid)
        {
            return oDataLayer.ExternalMemberByIdGet(extMemGuid);
        }
        public User RewardAndUserInsert(User oUser)
        {
            return oDataLayer.RewardAndUserInsert(oUser);
        }

        public ExtMemberGuidChk ExtMemberInsert(string mid, string pid)
        {
            return oDataLayer.ExtMemberInsert(mid, pid);
        }

        public string ExternalMemberActivityInsert(string external_member_id, int project_id, string pid, string ipAddress, string frid, decimal ecost, string mobiledeviceModel, int userTrafficTypeId, int org_id, string external_member_guid)
        {
            return oDataLayer.ExternalMemberActivityInsert(external_member_id, project_id, pid, ipAddress, frid, ecost, mobiledeviceModel, userTrafficTypeId, org_id, external_member_guid);
        }

        #region CleanIDDataInsert
        public string CleanIDDataInsert(string uig, string ug, int pid, string extmid, int cid, string json, string IpqsJSON, string sessionId)
        {
            string sessionResponse = string.Empty;
            if (sessionId != "undefined")
            {
                sessionResponse = AuthenticateSessionId(sessionId, extmid);
            }
            return oDataLayer.CleanIDDataInsert(uig, ug, pid, extmid, cid, json, IpqsJSON,sessionId,sessionResponse);
        }
        #endregion

        #region Verisoul Authentication
        private string AuthenticateSessionId(string sessionId, string extmid)
        {
            string sessionResponse = string.Empty;
            try
            {
                string verisoulURL = ConfigurationManager.AppSettings["VerisoulURL"].ToString();
                string verisoulAPIKey = ConfigurationManager.AppSettings["VerisoulAPIKey"].ToString();
                if (!string.IsNullOrEmpty(verisoulURL) && !string.IsNullOrEmpty(verisoulAPIKey))
                {
                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, verisoulURL);
                    request.Headers.Add("x-api-key", verisoulAPIKey);
                    var body = new { session_id = sessionId, account = new { id = extmid } };
                    var content = new StringContent(JsonConvert.SerializeObject(body), null, "application/json");
                    request.Content = content;
                    var response = client.SendAsync(request).Result;
                    sessionResponse = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                }
            }
            catch            
            {
                sessionResponse = string.Empty;
            }
            return sessionResponse;
        }
        #endregion
    }
}
