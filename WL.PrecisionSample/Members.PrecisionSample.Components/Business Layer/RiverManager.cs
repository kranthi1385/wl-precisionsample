using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class RiverManager
    {

        MemberEntity objUserDeialscCheck = new MemberEntity();
        RiverDataServices oRiverDataServices = new RiverDataServices();

        #region GetUserDetails
        public MemberEntity GetMemberDetails(Guid UserGuid)
        {
            return oRiverDataServices.GetMemberDetails(UserGuid);
        }
        #endregion

        #region Insert UserInvitation
        public River InsertUserInvitaiton(Guid UserGuid, Guid QuotaGroupGuid, string RelevantId, int RelevantScore, string FPFScore, string IpAddress, string BrowserInfo, string Agentinfo, int IsNew, int RelevantFalg, int Ip2ContryFlag, int UserTrafficType, string DeviceModel,string CleanID)
        {
            return oRiverDataServices.InsertUserInvitaiton(UserGuid, QuotaGroupGuid, RelevantId, RelevantScore, FPFScore, IpAddress, BrowserInfo, Agentinfo, IsNew, RelevantFalg, Ip2ContryFlag, UserTrafficType, DeviceModel, CleanID);
        }
        #endregion

        #region GetSurveyUrl
        public string GetSurveyUrl(Guid UserInvitationGuid)
        {
            return oRiverDataServices.GetSurveyUrl(UserInvitationGuid);
        }
        #endregion

        #region Save Member Details
        public string SaveUserDetails(MemberEntity oMemberEntity)
        {
            return oRiverDataServices.SaveUserDetails(oMemberEntity);
        }
        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid)
        {
            return oRiverDataServices.GetquestionsforTop10(UserGuid);
        }
        #endregion

        #region Top10SaveOptions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listXml"></param>
        public void Top10SaveOptions(string listXml)
        {
            oRiverDataServices.Top10SaveOptions(listXml);
        }
        #endregion

        #region Insert Member Details
        public string InsertUserDetails(User oUser)
        {
            return oRiverDataServices.InsertUserDetails(oUser);
        }
        #endregion

        #region Insert Member Details
        public string CheckMemberExistence(User oUser)
        {
            return oRiverDataServices.CheckMemberExistence(oUser);
        }
        #endregion

        #region relevant save
        public void UpdateRelevantandVerityData(User oUser)
        {
            oRiverDataServices.UpdateRelevantandVerityData(oUser);
        }
        #endregion

        #region Get Project Details
        public string GetProjectDetails(Guid UserGuid)
        {
            return oRiverDataServices.GetProjectDetails(UserGuid);
        }
        #endregion

        #region Get Survey URL
        public string GetSurveyURL(Guid UserGuid, Guid QuotaGroupGuid, string RelevantId, int RelevantScore, string FPFScore, string IpAddress, string BrowserInfo, string Agentinfo, int IsNew, int RelevantFalg, int Ip2ContryFlag)
        {
            return oRiverDataServices.GetSurveyURL(UserGuid, QuotaGroupGuid, RelevantId, RelevantScore, FPFScore, IpAddress, BrowserInfo, Agentinfo, IsNew, RelevantFalg, Ip2ContryFlag);
        }
        #endregion

        #region Update UserInvitation Details
        public string UpdateUserInvitationDetails(Guid UserInvitaionGuid, Guid UserStatusGuid)
        {
            return oRiverDataServices.UpdateUserInvitationDetails(UserStatusGuid, UserInvitaionGuid);
        }
        #endregion

        #region ClickInsert

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rid"></param>
        /// <param name="Sid"></param>
        /// <param name="ReferrerUrl"></param>
        /// <param name="IpAddress"></param>
        /// <param name="Txid"></param>
        /// <returns></returns>
        public int ClickInsert(int Rid, string Sid, string ReferrerUrl, string IpAddress, string Txid, string TransactionId)
        {
            return oRiverDataServices.ClickInsert(Rid, Sid, ReferrerUrl, IpAddress, Txid, TransactionId);
        }
        #endregion

        #region Insert Lead

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="SubReferrerCode"></param>
        /// <param name="PhoneNo"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        public string InsertLead(int ReferrerId, string SubReferrerCode, string PhoneNo, string Ipaddress)
        {
            RiverDataServices objService = new RiverDataServices();
            return objService.InsertLead(ReferrerId, SubReferrerCode, PhoneNo, Ipaddress);
        }
        #endregion

        #region Insert top 10page member skip page click log
        public void InsertTop10PageSkipLog(Guid UserGuid)
        {
            RiverDataServices objService = new RiverDataServices();
            objService.InsertTop10PageSkipLog(UserGuid);
        }
        #endregion

        #region inserting the postback transactions
        public void InsertPostbackTransaction(string PixelUrl, Guid UserGuid, Guid UserInvitationGuid, string ApiResponse)
        {
            oRiverDataServices.InsertPosbackTransactions(UserInvitationGuid, UserGuid, ApiResponse, PixelUrl);
        }

        #endregion

        public int GetReferrerDetails(int ReferrerId, string SubReferrer)
        {
            return oRiverDataServices.GetReferrerDetails(ReferrerId, SubReferrer);
        }

        public void SurveyActivityInsert(River oRiver)
        {
            oRiverDataServices.SurveyActivityInsert(oRiver);
        }
    }
}
