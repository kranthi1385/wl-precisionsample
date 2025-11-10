using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members.PrecisionSample.Components.Entities
{
    public class ApiUserEntity
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public int surveyClicksCount { get; set; }
        public string surveyUrl { get; set; }
        public int userId { get; set; }
        public Guid user2ProejctGuid { get; set; }
        public string ip2CountryoftheMember { get; set; }
        public string ip2CountryoftheSurvey { get; set; }
        public string surveynName { get; set; }
        public DateTime SurveyClickDate { get; set; }
        public decimal memberReward { get; set; }
        public int surveyId { get; set; }
        public decimal partnerReward { get; set; }
        public string accessCode { get; set; }
        public int profileCompleted { get; set; }
        public Guid pendingProfileGuid { get; set; }
        public string zipCode { get; set; }
        public string extMemberid { get; set; }
        public string gender { get; set; }
        public int countryId { get; set; }
        public int stateId { get; set; }
        public int ethnicityId { get; set; }
        public string countryName { get; set; }
        public string stateName { get; set; }
        public string ethnicityName { get; set; }
        public Guid userGuid { get; set; }
        public int referrerId { get; set; }
        public string dob { get; set; }
        public bool isenableprescreener { get; set; }


        public int projectId { get; set; }
        public string subId { get; set; }
        public decimal memberRewardAmount { get; set; }
        public decimal partnerRevenueShare { get; set; }
        public DateTime surveyCompletedDate { get; set; }
        public int orgId { get; set; }
        public int preliminaryStatusId { get; set; }
        public bool ispixelfired { get; set; }
        public int user2ProjectId { get; set; }
        #endregion
    }
}
