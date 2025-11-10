using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
   public class FCBusinessServices
    {
        FCDaataServices objDataservice = new FCDaataServices();

        #region SurveyLinkSecurityCheck
        /// <summary>
        /// get Survey Link Security Check
        /// </summary>
        /// <param name="InvitationGuid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Surveys SurveyLinkSecurityCheck(Guid InvitationGuid, int userid)
        {
            return objDataservice.SurveyLinkSecurityCheck(InvitationGuid, userid);
        }
        #endregion

        #region InsertClickDate
        /// <summary>
        /// get Insert Click Date
        /// </summary>
        /// <param name="surveyGuid"></param>
        /// <param name="src"></param>
        /// <param name="userid"></param>
        /// <param name="user_traffic_type"></param>
        /// <param name="org_id"></param>
        /// <param name="sub_id"></param>
        /// <param name="ipaddress"></param>
        /// <param name="mobiledevice"></param>
        /// <param name="verityId"></param>
        /// <param name="verityScore"></param>
        /// <returns></returns>
        public Surveys InsertClickDate(Guid surveyGuid, string src, int userid, int user_traffic_type, int org_id, string sub_id, string ipaddress, string mobiledevice, string verityId, int verityScore, string releventid, int releventscore, string fpfscore, int GeoCorrelationFlag, string BrowserInfo, string AgentInfo, int FraudProfileScore, string IsNew, string RID, Guid OldSurveyInvitationId,
            string ChallengeId, string Question1, string Question2, string Question3, string Option1, string Option2, string Option3)
        {
            return objDataservice.InsertClickDate(surveyGuid, src, userid, user_traffic_type, org_id, sub_id, ipaddress, mobiledevice, verityId, verityScore, releventid, releventscore, fpfscore, GeoCorrelationFlag, BrowserInfo, AgentInfo, FraudProfileScore, IsNew, RID, OldSurveyInvitationId,
               ChallengeId, Question1, Question2, Question3, Option1, Option2, Option3);

        }
        #endregion

        #region GetSurveyQuotaAlternateGuid
        /// <summary>
        /// Get Survey Quota Alternate Guid
        /// </summary>
        /// <param name="UserInviationGuid"></param>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public Surveys GetSurveyQuotaAlternateGuid(Guid UserInviationGuid, int org_id)
        {
            return objDataservice.GetSurveyQuotaAlternateGuid(UserInviationGuid, org_id);
        }
        #endregion

        #region GetAlternateSurveyUrl
        /// <summary>
        /// Get Alternate Survey Url
        /// </summary>
        /// <param name="QuotagroupId"></param>
        /// <param name="userid"></param>
        /// <param name="org_id"></param>
        /// <returns></returns>
        public Surveys GetAlternateSurveyUrl(Guid QuotagroupId, int userid, int org_id)
        {
            return objDataservice.GetAlternateSurveyUrl(QuotagroupId, userid, org_id);
        }
        #endregion
    }
}
