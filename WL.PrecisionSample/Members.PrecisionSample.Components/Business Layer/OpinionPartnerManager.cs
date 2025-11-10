using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using System.Net;
using System.IO;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class OpinionPartnerManager
    {
        OpinionPartnerDataServer objDataServer = new OpinionPartnerDataServer();

        #region check user already exist or not

        public MemberEntity objUserDeialscCheck(int Rid, string ExtId)
        {
            return objDataServer.objUserDeialscCheck(Rid, ExtId);
        }
        public MemberEntity save(MemberEntity objUser)
        {
            return objDataServer.save(objUser);
        }
        #endregion

        public MemberEntity SaveMemberDetails(MemberEntity oMemberEntity)
        {
            return objDataServer.SaveMemberDetails(oMemberEntity);
        }

        #region GetMemberDetails List

        //public List<PartnerHistory> GetUserDetailsList(Guid UserGuid, int OrgId)
        //{
        //    return objDataServer.GetUserDetailsList(UserGuid, OrgId);
        //}
        public List<PartnerHistory> GetSurveyList(Guid UserGuid, int OrgId, string ConnectionstringName)
        {
            return objDataServer.GetSurveyList(UserGuid, OrgId, ConnectionstringName);
        }
        public List<PartnerHistory> GetProfileList(Guid UserGuid, int OrgId, decimal partnerRevShare)
        {
            return objDataServer.GetProfileList(UserGuid, OrgId, partnerRevShare);
        }
        public List<PartnerHistory> GetRewardList(Guid UserGuid, int OrgId)
        {
            return objDataServer.GetRewardList(UserGuid, OrgId);
        }
        public List<PartnerHistory> GetSurveyAndRedeemptionHistory(Guid UserGuid, int OrgId)
        {
            return objDataServer.GetSurveyAndRedeemptionHistory(UserGuid, OrgId);
        }
        //public List<Surveys> GetUserSurveyList(Guid UserGuid, int OrgId)
        //{
        //    return objDataServer.GetUserSurveyList(UserGuid, OrgId);
        //}
        //public List<Profile> GetUserProfileList(Guid UserGuid, int OrgId)
        //{
        //    return objDataServer.GetUserProfileList(UserGuid, OrgId);
        //}
        //public List<PartnerHistory> GetUserRewardList(Guid UserGuid, int OrgId)
        //{
        //    return objDataServer.GetUserRewardList(UserGuid, OrgId);
        //}
        public List<PartnerHistory> GetCatalougAndRewardData(Guid UserGuid, Guid CatalougeGuid, int OrgId, string EmailAddress)
        {
            return objDataServer.GetCatalougAndRewardData(UserGuid, CatalougeGuid, OrgId, EmailAddress);
        }
        public Rewards RewardRedeemprtions(int Amount, Guid CatalougeGuid, int UserId)
        {
            return objDataServer.RewardRedeemprtions(Amount, CatalougeGuid, UserId);
        }
        public void RedeemMemberRewards(string Sku, decimal Ut, int UserId, int Points, int OrgId, string FirstName, string EmailAddress)
        {
            objDataServer.RedeemMemberRewards(Sku, Ut, UserId, Points, OrgId, FirstName, EmailAddress);
        }
       
        #endregion


        #region Get Country & States List

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public CountryAndState GetCountrysAndStates(string LanguageName)
        {
            return objDataServer.GetCountrysAndStates(LanguageName);
        }
        #endregion

        #region SaveUserDetails
        public MemberEntity SaveDetails(MemberEntity oMemberEntity)
        {
            return objDataServer.SaveDetails(oMemberEntity);
        }
        #endregion

        #region Get ethinicityList

        public List<Ethnicity> GetEthinicity(string LanguageName)
        {
            return objDataServer.GetEthinicity(LanguageName);
        }
        public MemberEntity objUserDeialscCheckByEmailAddress(int Rid, string EmailAddress)
        {
            return objDataServer.objUserDeialscCheckByEmailAddress(Rid, EmailAddress);
        }
        #endregion
        #region getting coupon names
        public List<Rewards> RedeemCoupons(Guid RedeemptionGuId, int UId)
        {
            return objDataServer.RedeemCoupons(RedeemptionGuId, UId);
        }
        #endregion


    }
}
