using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Members.OpinionBar.Web.Utlis
{
    public class Attribute
    {

        public static class personalizationElements
        {
            //Member Related
            public static string AccessCode = "%%sub_id3%%";
            public static string UserGUID = "%%user_guid%%";
            public static string Md5 = "%%md5%%";

            //Survey
            public static string SurveyName = "%%survey_name%%";
            public static string MemberReward = "%%member_reward%%";
            public static string SurveyStatus = "%%status%%";
            public static string PremiminaryCompletedDt = "%%survey_completed_date%%";
            public static string SurveyId = "%%survey_id%%";
            public static string PartnerGrossRevenue = "%%partner_gross_revenue%%";
            public static string SurveyType = "%%survey_type%%";
            //public static string Md5Hash = "%%md5hash%%";

            //Profilers
            public static string ProfileName = "%%profile_name%%";
            public static string ProfileId = "%%profile_id%%";
            public static string ProfileCompletedDt = "%%profile_complete_date%%";
            public static string ProfileClickDt = "%%profile_click_date%%";
            public static string SurveyClickDt = "%%survey_click_date%%";
            public static string SurveyCompletedDt = "%%survey_completed_dt%%";


            public static string PartnerRewardAmount = "%%partner_reward_amount%%";
        }

    }
}