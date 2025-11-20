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
    public class ExternalQuotaDataServer
    {
        #region Insert - External Quota Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oExternalQuotaEntities"></param>
        /// <returns></returns>

        public ExternalQuotaEntities InsertExternalQuotaMember(ExternalQuotaEntities oExternalQuotaEntities)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[ExternalMember_Insert]", cn);
            cm.Parameters.AddWithValue("user_id", oExternalQuotaEntities.MemberId);
            cm.Parameters.AddWithValue("quota_group_id", oExternalQuotaEntities.QuotaGroupId);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
            {
                cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
            }
            else
            {
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
            }

            //Added on 1/24/2014 for Mobile device detection:
            cm.Parameters.AddWithValue("@user_device_type_id", oExternalQuotaEntities.User_traffic_type_id);
            cm.Parameters.AddWithValue("@mobile_device_model", oExternalQuotaEntities.Mobile_device_model);
           //Added on 4/8/2014 to save the relevant Id & relevant Score :
            cm.Parameters.AddWithValue("@relevant_score", oExternalQuotaEntities.RelevantIdScore);
            cm.Parameters.AddWithValue("@relevant_id", oExternalQuotaEntities.RelevantId);
            cm.Parameters.AddWithValue("@fpf_scores", oExternalQuotaEntities.FpfScores);
            cm.Parameters.AddWithValue("browser_info", oExternalQuotaEntities.BrowserInfo);
            cm.Parameters.AddWithValue("@agent_info", oExternalQuotaEntities.AgentInfo);
            cm.Parameters.AddWithValue("@fraud_profile_score", oExternalQuotaEntities.RelevantFraudProfileScore);
            //Added on 02/16/2015 for isnew param relevant
            cm.Parameters.AddWithValue("@is_new", oExternalQuotaEntities.IsNew);
            cm.Parameters.AddWithValue("@e_rm", oExternalQuotaEntities.E_rm);
            cm.Parameters.AddWithValue("@e_rl", oExternalQuotaEntities.E_rl);
            cm.Parameters.AddWithValue("@zipcode", oExternalQuotaEntities.ZipCode);
            cm.Parameters.AddWithValue("@gender", oExternalQuotaEntities.Gender);
            cm.Parameters.AddWithValue("@dob", oExternalQuotaEntities.Dob);
            cm.Parameters.AddWithValue("@ethnicity", oExternalQuotaEntities.Ethnicity);
            cm.Parameters.AddWithValue("@country_id", oExternalQuotaEntities.CountryCode);
            using (IDataReader reader = cm.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["ext_url"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.ExternalSurveyUrl = reader["ext_url"].ToString();
                    }
                    if (reader["member_guid"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.ExternalMemberGuid = new Guid(reader["member_guid"].ToString());
                    }
                    if (reader["terminate_url"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.ExternalSurveyTermianteURL = reader["terminate_url"].ToString();
                    }
                    if (reader["project_status_id"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.ProjectStatusId = Convert.ToInt32(reader["project_status_id"]);
                    }
                    if (reader["is_allow_multiple_entries"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.IsAllowMultipleEntries = Convert.ToBoolean(reader["is_allow_multiple_entries"]);
                    }
                    if (reader["survey_guid"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.SurveyGuid = new Guid(reader["survey_guid"].ToString());
                    }
                    if (reader["survey_user_type_id"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.Survey_user_type_id = Convert.ToInt32(reader["survey_user_type_id"]);
                    }
                    if (reader["survey_name"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.SurveyName = Convert.ToString(reader["survey_name"]);
                    }
                    if (reader["reward_amount"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.RewardAmount = Convert.ToDecimal(reader["reward_amount"]);
                    }
                    if (reader["quota_org_id"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.QuotaOrgId = Convert.ToInt32(reader["quota_org_id"]);
                    }
                    if (reader["org_logo"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.OrgLogo = Convert.ToString(reader["org_logo"]);
                    }
                    if (reader["isrequire_relevantid_validated_members"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.Isrequire_relevantid_validated_members = Convert.ToBoolean(reader["isrequire_relevantid_validated_members"]);
                    }
                    if (reader["ip_2_country_check_external"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.Is_ip_2_country_check_external = Convert.ToBoolean(reader["ip_2_country_check_external"]);
                    }
                    if (reader["ext_quota_country_id"] != DBNull.Value)
                    {
                        oExternalQuotaEntities.ExtQuotaCountryId = Convert.ToInt32(reader["ext_quota_country_id"]);
                    }
                }
                return oExternalQuotaEntities;
            }
        }

        #endregion

        #region Get External Member By Id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extmemid"></param>
        /// <returns></returns>

        public ExternalQuotaEntities ExternalMemberByIdGet(Guid extmemid)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[pms].[ExternalMemberById_Get]", cn);
            cm.Parameters.AddWithValue("external_member_guid", extmemid);
            ExternalQuotaEntities oExtEntities = new ExternalQuotaEntities();
            using (IDataReader reader = cm.ExecuteReader())
            {

                while (reader.Read())
                {
                    if (reader["user_id"] != DBNull.Value)
                    {
                        oExtEntities.MemberId = reader["user_id"].ToString();
                    }
                }
                return oExtEntities;
            }
        }

        #endregion
    }
}
