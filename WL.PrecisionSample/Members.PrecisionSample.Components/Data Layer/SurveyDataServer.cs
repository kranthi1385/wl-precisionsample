using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Common.Security;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;


namespace Members.PrecisionSample.Components.Data_Layer
{
    public class SurveyDataServer
    {
        #region ConnectionString
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        public string ConnectionStringIP
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIP"].ToString();
            }
        }

        public string ConnectionStringActivity
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            }
        }

        #endregion

        #region Get all avaliable surveys
        /// <summary>
        /// Get all avaliable surveys
        /// </summary>
        /// <param name="UserId">userid</param>
        /// <returns></returns>
        public List<Perks> GetSurveysList(string UserId)
        {
            List<Perks> lstPerks = new List<Perks>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[ProjectsList_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserId);

                #region Adding params for the SQL Proc
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                #endregion
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Perks objPerks = new Perks();
                        if (reader["survey_name"] != DBNull.Value)
                        {
                            objPerks.PerkName = Convert.ToString(reader["survey_name"]);
                        }
                        if (reader["survey_guid"] != DBNull.Value)
                        {
                            objPerks.PerkGuid = new Guid(reader["survey_guid"].ToString());
                        }
                        if (reader["perk_url"] != DBNull.Value)
                        {
                            objPerks.PerkUrl = Convert.ToString(reader["perk_url"]);
                        }
                        if (reader["perk_description"] != DBNull.Value)
                        {
                            objPerks.PerkDescription = Convert.ToString(reader["perk_description"]);
                        }
                        if (reader["survey_url"] != DBNull.Value)
                        {
                            objPerks.SurveyUrl = Convert.ToString(reader["survey_url"]);
                        }
                        if (reader["survey_url"] != DBNull.Value)
                        {
                            objPerks.SurveyUrl = Convert.ToString(reader["survey_url"]);
                        }
                        if (reader["survey_description"] != DBNull.Value)
                        {
                            objPerks.SurveyDescription = Convert.ToString(reader["survey_description"]);
                        }
                        if (reader["reward_value"] != DBNull.Value)
                        {
                            objPerks.RewardValue = Convert.ToDecimal(reader["reward_value"]);
                        }
                        if (reader["click_dt"] != DBNull.Value)
                        {
                            objPerks.PerkClickDt = Convert.ToString(reader["click_dt"]);
                        }
                        if (reader["offer_completed_dt"] != DBNull.Value)
                        {
                            objPerks.PerkCompletedDt = Convert.ToString(reader["offer_completed_dt"]);
                        }
                        if (reader["survey_completed_dt"] != DBNull.Value)
                        {
                            objPerks.SurveyCompletedDt = Convert.ToString(reader["survey_completed_dt"]);
                        }
                        if (reader["imagetype"] != DBNull.Value)
                        {
                            objPerks.Type = Convert.ToString(reader["imagetype"]);
                        }
                        if (reader["status"] != DBNull.Value)
                        {
                            objPerks.Status = Convert.ToString(reader["status"]);
                        }
                        if (reader["survey_length"] != DBNull.Value)
                        {
                            objPerks.SurveyLength = Convert.ToInt32(reader["survey_length"]);
                        }
                        if (reader["project_id"] != DBNull.Value)
                        {
                            objPerks.Projectid = Convert.ToInt32(reader["project_id"]);
                        }
                        lstPerks.Add(objPerks);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return lstPerks;
        }
        #endregion

        #region Get Projects Details
        /// <summary>
        /// Get Projects Details
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        public List<Surveys> GetAllAvaliableSurveys(int UserId)
        {
            List<Surveys> lstSurveys = new List<Surveys>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[NewSurveys_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserId);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Surveys oSurveys = new Surveys();
                        if (reader["project_name"] != DBNull.Value)
                        {
                            oSurveys.SurveyName = reader["project_name"].ToString();
                        }
                        if (reader["Survey_complete_reward_amount"] != DBNull.Value)
                        {
                            oSurveys.SurveyCompleteRewardAmount = Convert.ToDecimal(reader["Survey_complete_reward_amount"]);
                        }
                        lstSurveys.Add(oSurveys);
                    }

                }
            }
            catch
            {

            }
            finally
            {
                cn.Close();
            }
            return lstSurveys;

        }

        #endregion

        #region Get User Invitation GUID by Id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uig"></param>
        /// <returns></returns>
        public Guid GetInvitationGUIDbyId(string uig)
        {
            Guid _uig = Guid.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionStringActivity;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[get_user_invitaiton_guid_by_invitation_id]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_id", uig);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Surveys oSurveys = new Surveys();
                        if (reader["user_invitation_guid"] != DBNull.Value)
                        {
                            _uig = new Guid(reader["user_invitation_guid"].ToString());
                        }
                    }

                }
            }
            catch
            {

            }
            finally
            {
                cn.Close();
            }
            return _uig;
        }
        #endregion
    }
}
