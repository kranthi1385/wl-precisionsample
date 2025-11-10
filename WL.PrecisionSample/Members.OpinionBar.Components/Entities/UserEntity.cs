using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class UserEntity
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public string surveynName { get; set; }
        public string SurveyClickDate { get; set; }
        public decimal memberReward { get; set; }
        public int surveyId { get; set; }
        public decimal partnerReward { get; set; }
        public string AccessCode { get; set; }
        public Guid userGuid { get; set; }
        public string ProfileName { get; set; }
        public int ProfileId { get; set; }
        public string surveyCompleteDate { get; set; }
        public DateTime prilmCompleteDate { get; set; }
        public string ProfileClickDate { get; set; }
        public string profileCompleteDate { get; set; }
        public string partnerRewardAmount { get; set; }
        public int profileCompleted { get; set; }
        public Guid pendingProfileGuid { get; set; }
        public string subId { get; set; }
        public int projectId { get; set; }

        public decimal partnerRevenueShare { get; set; }
        public int preliminaryStatusId { get; set; }
        public bool isPixelFired { get; set; }
        public int user2ProjectId { get; set; }
        public int orgId { get; set; }
        public DateTime vindaleSurveyClickDate { get; set; }
        public string txId { get; set; }
        public int actualLoi { get; set; }
        public string subId3 { get; set; }
        public int userId { get; set; }
        public string rid { get; set; }
        public string partnerRedirectUrl { get; set; }
        public bool isShowEndPage { get; set; }
        public string source { get; set; }
        public string status { get; set; }
        public bool isS2SEndpage { get; set; }
        public string postbackUrl { get; set; }
        public string termUrl { get; set; }
        #endregion
    }
}
