using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class FCDaataServices
    {
        #region get surveylink Security
        /// <summary>
        /// Survey Link Security Check
        /// </summary>
        /// <param name="InvitationGuid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public Surveys SurveyLinkSecurityCheck(Guid InvitationGuid, int userid)
        {
            Surveys objSurveys = new Surveys();
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[SurveyLinkSecurity_Check]", cn);
            cm.Parameters.AddWithValue("@guid", InvitationGuid);
            cm.Parameters.AddWithValue("user_id", userid);
           
            using (IDataReader reader = cm.ExecuteReader())
            {

                while (reader.Read())
                {
                    if (reader["count"] != DBNull.Value)
                    {
                        objSurveys.Count = Convert.ToInt32(reader["count"]);
                    }
                    if (reader["isallowed"] != DBNull.Value)
                    {
                        objSurveys.IsAlloed = Convert.ToBoolean(reader["isallowed"]);
                    }
                    if (reader["survey_name"] != DBNull.Value)
                    {
                        objSurveys.SurveyName = Convert.ToString(reader["survey_name"]);
                    }
                    if (reader["member_reward_amount"] != DBNull.Value)
                    {
                        objSurveys.SurveyCompleteRewardAmount = Convert.ToDecimal(reader["member_reward_amount"]);
                    }

                }
                return objSurveys;
            }
        }

        #endregion

        #region Project Click Date Insert
        /// <summary>
        /// Insert Click Date
        /// </summary>
        /// <param name="oUser"></param>
        public Surveys InsertClickDate(Guid surveyGuid, string src, int userid, int _user_traffic_type,
            int org_id, string sub_id, string ipaddress, string mobiledevice, string verityId, int verityScore, string releventid, int releventscore, string fpfscore, int GeoCorrelationFlag, string BrowserInfo, string AgentInfo, int FraudProfileScore, string IsNew, string RID, Guid OldSurveyInvitationId,
            string ChallengeId, string Question1, string Question2, string Question3, string Option1, string Option2, string Option3)
        {
            int _new = 0;
            if (IsNew.ToLower() == "true")
                _new = 1;
            else
            {
                _new = 0;
            }
            //Database db = DatabaseFactory.CreateDatabase();
            //String sqlCommand = "[pms].[ProjectClikDate_Insert]";
            //DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[ProjectClikDate_Insert]", cn);
            #region Adding params for the SQL Proc (IF NOT NULL)
            cm.Parameters.AddWithValue("user_invitation_guid", surveyGuid);
            cm.Parameters.AddWithValue("user_id", userid);
            cm.Parameters.AddWithValue("source", src);
            cm.Parameters.AddWithValue("user_traffic_type_id", _user_traffic_type);
            cm.Parameters.AddWithValue("org_id", org_id);
            if (!string.IsNullOrEmpty(sub_id))
            {
                cm.Parameters.AddWithValue("sub_id", sub_id);
               
            }
            else
            {
                cm.Parameters.AddWithValue("sub_id", DBNull.Value);
            }
            cm.Parameters.AddWithValue("ip_address", ipaddress);
            cm.Parameters.AddWithValue("mobile_device_model", mobiledevice);
            //Added for Verity Implemantation
            cm.Parameters.AddWithValue("verity_score", verityScore);
            cm.Parameters.AddWithValue("verity_id", verityId);
            cm.Parameters.AddWithValue("relevant_id", releventid);
            cm.Parameters.AddWithValue("relevant_score", releventscore);
            cm.Parameters.AddWithValue("fpf_scores", fpfscore);
            //Added on 07/10/2014 for GeoCorrelation Flag.
            cm.Parameters.AddWithValue("verity_geo_flag", GeoCorrelationFlag);
            cm.Parameters.AddWithValue("browser_info", BrowserInfo);
            cm.Parameters.AddWithValue("agent_info", AgentInfo);
            cm.Parameters.AddWithValue("fraud_profile_score", FraudProfileScore);
            //Added on 02/16/2015 for Isnew Flag of relevant
            cm.Parameters.AddWithValue("@is_new", _new);
            cm.Parameters.AddWithValue("@rid", RID);
            if (OldSurveyInvitationId != Guid.Empty)
            {
                cm.Parameters.AddWithValue("old_survey_invtation_id", OldSurveyInvitationId);
            }
            else
            {
                cm.Parameters.AddWithValue("old_survey_invtation_id", DBNull.Value);
             }

            //Added on 4/18/2016 by sandy to store the Verity Enhanced Questions.
            cm.Parameters.AddWithValue("@challenge_id", ChallengeId);
            cm.Parameters.AddWithValue("@question_text1", Question1);
            cm.Parameters.AddWithValue("@option_text1", Option1);
            cm.Parameters.AddWithValue("@question_text2", Question2);
            cm.Parameters.AddWithValue("@option_text2", Option2);
            cm.Parameters.AddWithValue("@question_text3", Question3);
            cm.Parameters.AddWithValue("@option_text3", Option3);
            #endregion
            Surveys oservey = new Surveys();
            using (IDataReader reader = cm.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["user_invitation_guid"] != DBNull.Value)
                    {
                        oservey.UserInvitationId = new Guid(reader["user_invitation_guid"].ToString());
                    }
                    if (reader["active_project_url"] != DBNull.Value)
                    {
                        oservey.SurveyUrl = reader["active_project_url"].ToString();
                    }
                    if (reader["survey_guid"] != DBNull.Value)
                    {
                        oservey.SurveyGuid = new Guid(reader["survey_guid"].ToString());
                    }
                    //Added on 11/21/2012 to know weather Pre-Screener for the user Invitation is enabled or not.
                    if (reader["is_enableprescreener"] != DBNull.Value)
                    {
                        oservey.IsenablePrescreener = Convert.ToBoolean(reader["is_enableprescreener"]);
                    }
                    //Added on 11/12/2013 to get the Survey Traffic ID to check which members are to be allowed.
                    if (reader["survey_user_type_id"] != DBNull.Value)
                    {
                        oservey.SurveyUserTypeId = Convert.ToInt32(reader["survey_user_type_id"]);
                    }

                    //Added on 3/26/2014 to get the Project switch 
                    if (reader["isrequire_verity_validated_members"] != DBNull.Value)
                    {
                        oservey.IsrequireVerityValidatedMembers = Convert.ToBoolean(reader["isrequire_verity_validated_members"]);
                    }
                    if (reader["isrequire_relevantid_validated_members"] != DBNull.Value)
                    {
                        oservey.Isrequirerelevantidvalidatedmembers = Convert.ToBoolean(reader["isrequire_relevantid_validated_members"]);
                    }
                    if (reader["verity_questions_count"] != DBNull.Value)
                    {
                        oservey.VerityQstCount = Convert.ToInt32(reader["verity_questions_count"]);
                    }
                }
                return oservey;
            }

        }

        #endregion

        #region Project get Alternate SurveyUrl
        /// <summary>
        /// Get Survey Quota Alternate Guid
        /// </summary>
        /// <param name="oUser"></param>
        public Surveys GetSurveyQuotaAlternateGuid(Guid UserInviationGuid, int org_id)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[SurveyLinkSecurity_Check]", cn);
            #region Adding params for the SQL Proc (IF NOT NULL)
            cm.Parameters.AddWithValue("user_invitation_guid", UserInviationGuid);
            cm.Parameters.AddWithValue("org_id", org_id);
            #endregion
            Surveys oservey = new Surveys();
            using (IDataReader reader = cm.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["quota_group_guid"] != DBNull.Value)
                    {
                        oservey.QuotagroupGuid = new Guid(reader["quota_group_guid"].ToString());
                    }
                }
                return oservey;
            }
        }

        #endregion

        #region Project get Alternate SurveyUrl
        /// <summary>
        /// Get Alternate SurveyUrl
        /// </summary>
        /// <param name="oUser"></param>
        public Surveys GetAlternateSurveyUrl(Guid QuotagroupId, int userid, int org_id)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            //String sqlCommand = "[pms].[SurveyQuotaUrl_Get]";
            //DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[SurveyQuotaUrl_Get]", cn);

            #region Adding params for the SQL Proc (IF NOT NULL)
            cm.Parameters.AddWithValue("quota_group_guid", QuotagroupId);
            cm.Parameters.AddWithValue("user_id", userid); 
            cm.Parameters.AddWithValue("org_id", org_id);
            #endregion
            Surveys oservey = new Surveys();
            using (IDataReader reader = cm.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["user_invitation_guid"] != DBNull.Value)
                    {
                        oservey.UserInvitationId = new Guid(reader["user_invitation_guid"].ToString());
                    }
                    if (reader["active_project_url"] != DBNull.Value)
                    {
                        oservey.SurveyUrl = Convert.ToString(reader["active_project_url"]);
                    }
                }
                return oservey;
            }

        }

        #endregion
    }
}
