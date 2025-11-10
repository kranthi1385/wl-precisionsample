using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
   public class Surveys
    {
        #region public variables
        /// <summary>
        /// 
        /// </summary>
        public int Count { get; set; }
        public bool IsAllowed { get; set; }
        public string SurveyName { get; set; }
        public string PerkDescription { get; set; }
        public string SurveyUrl { get; set; }
        public string StrSurveyName { get; set; }
        public Guid PerkGuid { get; set; }
        public string StrStatus { get; set; }
        public string SurveyClickdt { get; set; }
        public string SurveyCompletedt { get; set; }
        public string StrSurveyUrl { get; set; }
        public string StrSurveyDescription { get; set; }
        public Guid User2PerkGuid { get; set; }
        public string StrEmailDt { get; set; }
        public Guid UserInvitationId { get; set; }
        public string Logo { get; set; }
        public int UserInvitationStatusId { get; set; }
        public Guid StatusGuid { get; set; }
        public Guid UserGuid { get; set; }
        public int QuotaType { get; set; }
        public string ExternalRedirectUrl { get; set; }
        public string ExternalMemberId { get; set; }
        public int QuotagroupId { get; set; }
        public int UserId { get; set; }
        public Guid QuotagroupGuid { get; set; }
        public decimal SurveyCompleteRewardAmount { get; set; }
        public Guid ExternalMemberGuid { get; set; }

        public Guid SurveyGuid { get; set; }

        public int ProjectStatusId { get; set; }
        public string MemberUrl { get; set; }
        public string Organizationtype { get; set; }
        public bool IsenablePrescreener { get; set; }

        public Guid Fedresponseid { get; set; }
        public Guid FedProjectCost { get; set; }
        public int QuotaGroupTypeId { get; set; }

        public int IsAlreadyClicked { get; set; }
        public int SurveyLength { get; set; }
        public string SurveyCompletereward { get; set; }
        public int VerityScore { get; set; }
        public string VerityDetails { get; set; }
        public int IsRiver { get; set; }
        public int RewardPoints { get; set; }
        public int OrgId { get; set; }
        public int Targetid { get; set; }
        public string MatchedQuotas { get; set; }
        public bool IsInternalMenber { get; set; }
        public int OrginalProjectId { get; set; }
        public string Source { get; set; }
        public bool IsIp2Country { get; set; }
        public int ProjectId { get; set; }
        public string SubId { get; set; }
        public string SurveyTakingIp { get; set; }
        public bool IsTop10Enable { get; set; }
        public bool IsTop10CompleteCheck { get; set; }
        public int VerityQstCount { get; set; }
        public int IsRewardsInDollar { get; set; }
        public string RewardText { get; set; }
        public int SurveyUserTypeId { get; set; }
        public Boolean IsrequireVerityValidatedMembers { get; set; }
        public Boolean Isrequirerelevantidvalidatedmembers { get; set; }
        public string ExtRedirectUrl { get; set; }
        public string PostbackURL { get; set; }
        public int PixelTypeId { get; set; }
        public string ERM { get; set; }
        public string ERl { get; set; }
        public decimal MemberReward;
        public decimal MemberRewardPoints;
        public decimal PartnerRevenueShare;
        public decimal ApiPartnerProjectCost;
        public string RedirectUrl { get; set; }
        public string SurveyUserTypeIds { get; set; }
        public bool IsStandalone { get; set; }
        public bool IsEmailInvitationEnable { get; set; }
        public bool IsSmsInvitation { get; set; }
        public string CountyCode { get; set; }
        public string OrgLogo { get; set; }
        public int IsMobileNumberExist { get; set; }
        public DateTime RedirectDt { get; set; }
        public int ActivityTypeId { get; set; }
        public DateTime ActivityDt { get; set; }
        private Guid _fedresponseid = Guid.Empty;
        public string E_RM { get; set; }
        public string E_Rl { get; set; }
        public Guid ExternalMemberGUID { get; set; }
        public int TartgetTypeId { get; set; }
        public string TargetSuccessUrl { get; set; }
        public string TargetTerminateUrl { get; set; }
        public string TargetOverQuotaUrl { get; set; }
        public int OrganizationTypeId { get; set; }
        public string FedRedirectURl { get; set; }
        public string Zipcode { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Ethnicity { get; set; }
        public string postbacktext { get; set; }
        public int CountyId { get; set; }
        public int TargetTypeId { get; set; }
        public Int64 InvitationId { get; set; }
        public string ChallengeId { get; set; }
        public string orgInformation { get; set; }
        public Guid ActualInvitationGuid { get; set; }

        public int ReferrerId { get; set; }

        public string SubId3 { get; set; }
        public string Txid { get; set; }

        public string EnableRecaptcha { get; set; }
        public int ExternalPartnerID { get; set; }

        #endregion
    }
}
