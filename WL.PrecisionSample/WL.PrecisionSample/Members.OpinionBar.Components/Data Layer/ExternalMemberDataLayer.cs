using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using System.Configuration;

namespace Members.OpinionBar.Components.Data_Layer
{
   
    public class ExternalMemberDataLayer
    {
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            }
        }
        #region Save User Click Inforamtion
        /// <summary>
        ///Save User Click Inforamtion
        /// </summary>
        /// <param name="QgId">QuotaGroupId</param>
        /// <param name="uid">UserGuid</param>
        /// <param name="qid">ProjectId</param>
        /// <param name="Rid">ReferrerId</param>
        /// <param name="Soruce">Source</param>
        /// <param name="SubId">SubId</param>
        /// <param name="IsNew">IsNew</param>
        /// <param name="UserTrafficTypeId">UserTrafficOd</param>
        /// <param name="MobiledeviceModel">DeviceModel</param>
        /// <param name="BrowserInfo">BrowserInfo</param>
        /// <param name="AgentInfo">AgentInfo</param>
        /// <param name="IpAddress">IpAddress</param>
        /// <param name="RelevantId">RelevantId</param>
        /// <param name="RelevantScore">RelevantScore</param>
        /// <param name="FpfScores">FpfScore</param>
        /// <param name="FraudPfScore">FraudPfScore</param>
        /// <param name="OldSurveyInvitationId">OldSurveyInvitationd</param>
        /// <returns></returns>
        public Surveys SaveClickInformation(string QgId, string uid, int qid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                          string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore,
                          string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl)
        {
            Surveys oExternalQuotaEntities = new Surveys();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[external_member_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", uid);
                cm.Parameters.AddWithValue("@target_id", qid);
                //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                //{
                //    cm.Parameters.AddWithValue("org_id",);
                //}
                //else
                //{
                //    db.AddInParameter(dbCommand, "org_id", DbType.Int32, MemberIdentity.Client.ClientId);
                //}
                //Added on 1/24/2014 for Mobile device detection:
                cm.Parameters.AddWithValue("@user_device_type_id", UserTrafficTypeId);
                cm.Parameters.AddWithValue("@mobile_device_model", MobiledeviceModel);
                //Added on 4/8/2014 to save the relevant Id & relevant Score :
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@fpf_scores", FpfScores);
                cm.Parameters.AddWithValue("browser_info", BrowserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@fraud_profile_score", FraudProfilefScore);
                //Added on 02/16/2015 for isnew param relevant
                cm.Parameters.AddWithValue("@is_new", IsNew);
                cm.Parameters.AddWithValue("@e_rm", e_rm);
                cm.Parameters.AddWithValue("@e_rl", e_rl);
                cm.Parameters.AddWithValue("@zipcode", oExternalQuotaEntities.Zipcode);
                cm.Parameters.AddWithValue("@gender", oExternalQuotaEntities.Gender);
                cm.Parameters.AddWithValue("@dob", oExternalQuotaEntities.DOB);
                cm.Parameters.AddWithValue("@ethnicity", oExternalQuotaEntities.Ethnicity);
                cm.Parameters.AddWithValue("@survey_taking_ip", IpAddress);
                cm.Parameters.AddWithValue("@fed_response_id", fed_response_id);
                cm.Parameters.AddWithValue("@ecost", ecost);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.RedirectUrl = Convert.ToString(dr["redirect_url"]);
                        }
                        if (dr["survey_user_type_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.SurveyUserTypeIds = Convert.ToString(dr["survey_user_type_id"]);
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.CountyId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["project_status_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.ProjectStatusId = Convert.ToInt32(dr["project_status_id"]);
                        }
                        if (dr["target_type_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.TargetTypeId = Convert.ToInt32(dr["target_type_id"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        //if (dr["is_stand_alone_partner"] != DBNull.Value)
                        //{
                        //    oExternalQuotaEntities.IsStandalone = Convert.ToBoolean(dr["is_stand_alone_partner"]);
                        //}
                        //if (dr["is_email_invitation"] != DBNull.Value)
                        //{
                        //    oExternalQuotaEntities.IsEmailInvitationEnable = Convert.ToBoolean(dr["is_email_invitation"]);
                        //}
                        //if (dr["is_sms_invitation"] != DBNull.Value)
                        //{
                        //    oExternalQuotaEntities.IsSmsInvitation = Convert.ToBoolean(dr["is_sms_invitation"]);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cn.Close();
            }
            return oExternalQuotaEntities;
        }

        #endregion
    }
}
