using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
    public class Surveys
    {
        public int OrgId;
        public int PriorityId;
        public int ProjectId;
        public string TargetGuid;
        public string SurveyName;
        public int SurveyLength;
        public decimal PartnerRevenueShare;
        public decimal MemberReward;
        public decimal MemberRewardPoints;
        public string SurveyUserTypeIds;
        public string SurveyUrl;
        public int Ir;
        public string Message = string.Empty;
        public string RewardText = string.Empty;
    }
}
