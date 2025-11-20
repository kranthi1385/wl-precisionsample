using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Recaptcha
    {
        public Guid UserInvitationGuid { get; set; }

        public Guid Step7Guid { get; set; }
        public bool HasPrescreenerQuestions { get; set; }
        public string SurveyUrl { get; set; }
        public int BalancingTypeId { get; set; }
        public int ProjectId { get; set; }
        public Boolean IsInternalMember { get; set; }
        public int UserId { get; set; }
        public Int64 UserInvitationId { get; set; }
        public int OrgId { get; set; }
        public string Source { get; set; }
        public int QuotaGroupId { get; set; }
        public bool IsClosed { get; set; }
        public int ActivityTypeId { get; set; }
        public string MatchedQuotas { get; set; }
        public string FedResponseId { get; set; }
        public int TargetId { get; set; }
        public int CountryID { get; set; }
        public string DemandAPIQuotaID { get; set; }
    }
    public class Quotas
    {
        public int QuotaGroupId { get; set; }
        public bool IsClosed { get; set; }
    }
    public class TolunaSurvey
    {
        public int SurveyId { get; set; }
        public int WaveID { get; set; }
        public int QuotaID { get; set; }
        public double MemberAmount { get; set; }
        public double PartnerAmount { get; set; }
        public string URL { get; set; }

    }

}
