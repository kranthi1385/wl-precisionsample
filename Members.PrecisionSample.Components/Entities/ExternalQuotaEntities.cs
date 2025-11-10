using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ExternalQuotaEntities
    {
        public string MemberId { get; set; }
        public int QuotaGroupId { get; set; }
        public Guid ExternalMemberGuid { get; set; }
        public string ExternalSurveyUrl { get; set; }
        public int ProjectStatusId { get; set; }
        public bool IsAllowMultipleEntries { get; set; }
        public Guid SurveyGuid { get; set; }
        public int User_traffic_type_id { get; set; }
        public int Survey_user_type_id { get; set; }
        public string Mobile_device_model { get; set; }
        public string SurveyName { get; set; }
        public decimal RewardAmount { get; set; }
        public int QuotaOrgId { get; set; }
        public string OrgLogo { get; set; }
        public int RelevantIdScore { get; set; }
        public string RelevantId { get; set; }
        public bool Isrequire_relevantid_validated_members { get; set; }
        public string FpfScores { get; set; }
        public string BrowserInfo { get; set; }
        public string AgentInfo { get; set; }
        public int RelevantFraudProfileScore { get; set; }
        public int IsNew { get; set; }
        public bool Is_ip_2_country_check_external { get; set; }
        public int ExtQuotaCountryId { get; set; }
        public string E_rm { get; set; }


        public string E_s { get; set; }


        public string E_rl { get; set; }
        public string ZipCode { get; set; }
        public int CountryCode { get; set; }

        public string ExternalSurveyTermianteURL { get; set; }
        public string Dob { get; set; }
        public string Ethnicity { get; set; }
        public string Gender { get; set; }
     
    }
}
