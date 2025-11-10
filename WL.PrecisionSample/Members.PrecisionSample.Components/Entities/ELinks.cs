using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
   public class ELinks
    {
        public string ExternalMemberId { get; set; }
        public int IsRiver { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsTop10CompleteCheck { get; set; }
        public int OrganizationTypeId { get; set; }
        public string MemberUrl { get; set; }
        public Guid StatusGuid { get; set; }
        public int UserId { get; set; }
        public int TartgetTypeId { get; set; }
        public int UserInvitationStatusId { get; set; }
        public string ExtRedirectUrl { get; set; }
        public string PostbackURL { get; set; }
        public int PixelTypeId { get; set; }
        public string FedRedirectURl { get; set; }
        public bool IsTop10Enable { get; set; }
        public int ProjectStatusId { get; set; }
        public string postbacktext { get; set; }
        public int orgid { get; set; }
        public int targetid { get; set; }
        public bool IsInternalMenber { get; set; }
        public int QuotaGroupId { get; set; }
        public Guid UserInvitationId { get; set; }
        public int QuotagroupId { get; set; }
        public decimal SurveyCompleteRewardAmount { get; set; }
        public decimal MemberReward;
        public decimal MemberRewardPoints;
        public decimal PartnerRevenueShare;
        public decimal ApiPartnerProjectCost;
        public string SurveyName { get; set; }
        public int ProjectId { get; set; }
        public int SubId { get; set; }
        public string SurveyTakingIp { get; set; }
        public int OrginalProjectId { get; set; }
        public string Source { get; set; }
        private Guid _fedresponseid = Guid.Empty;
        public DateTime RedirectDt { get; set; }
        public int ActivityTypeId { get; set; }
        public int QuotaType { get; set; }
        public Guid Fedresponseid { get; set; }
        public Guid invitationGuid { get; set; }
        public Guid ExternalMemberGUID { get; set; }
        public string E_RM { get; set; }
        public string E_Rl { get; set; }







    }
}
