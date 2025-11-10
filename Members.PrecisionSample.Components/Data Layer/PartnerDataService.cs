using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class PartnerDataService
    {
        #region GetSurveyStatus
        /// <summary>
        /// 
        /// </summary>
        /// <param name="User2ProjectGuid"></param>
        /// <returns></returns>
        public Partner GetSurveyStatus(Guid UserInvitationGuid, int ClientId)
        {
            Partner objEntity = new Partner();
            SqlConnection cn = new SqlConnection();
            string constr = GetConnectionString(ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[member_surveydetails_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                if (UserInvitationGuid != Guid.Empty)
                {
                    cm.Parameters.AddWithValue("@user_Invitation_guid", UserInvitationGuid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@user_Invitation_guid", DBNull.Value);
                }

                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            objEntity.OrgId = Convert.ToInt32(reader["org_id"]);
                        }
                        if (reader["sub_id"] != DBNull.Value)
                        {
                            objEntity.SubId = Convert.ToString(reader["sub_id"]);
                        }
                        if (reader["sub_id3"] != DBNull.Value)
                        {
                            objEntity.SubId3 = Convert.ToString(reader["sub_id3"]);
                        }
                        if (reader["project_id"] != DBNull.Value)
                        {
                            objEntity.ProjectId = Convert.ToInt32(reader["project_id"]);
                        }
                        if (reader["member_reward"] != DBNull.Value)
                        {
                            objEntity.MemberReward = Convert.ToDecimal(reader["member_reward"].ToString());
                        }
                        if (reader["partner_reward"] != DBNull.Value)
                        {
                            objEntity.PartnerRevenueShare = Convert.ToDecimal(reader["partner_reward"]);
                        }
                        if (reader["preliminary_status_dt"] != DBNull.Value)
                        {
                            objEntity.PrilmCompleteDate = Convert.ToDateTime(reader["preliminary_status_dt"]);
                        }
                        if (reader["preliminary_status_id"] != DBNull.Value)
                        {
                            objEntity.PreliminaryStatusId = Convert.ToInt32(reader["preliminary_status_id"]);
                        }
                        if (reader["is_posted"] != DBNull.Value)
                        {
                            objEntity.IsPixelFired = Convert.ToBoolean(reader["is_posted"]);
                        }
                        if (reader["user_invitation_id"] != DBNull.Value)
                        {
                            objEntity.User2ProjectId = Convert.ToInt64(reader["user_invitation_id"]);
                        }
                        if (reader["tx_id"] != DBNull.Value)
                        {
                            objEntity.TxId = Convert.ToString(reader["tx_id"]);
                        }
                        if (reader["loi"] != DBNull.Value)
                        {
                            objEntity.ActualLoi = Convert.ToInt32(reader["loi"]);
                        }
                        if (reader["survey_completed_dt"] != DBNull.Value)
                        {
                            objEntity.SurveyCompleteDate = Convert.ToString(reader["survey_completed_dt"]);
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            objEntity.UserId = Convert.ToInt32(reader["user_id"]);
                        }
                        if (reader["rid"] != DBNull.Value)
                        {
                            objEntity.Rid = Convert.ToString(reader["rid"]);
                        }
                        if (reader["survey_name"] != DBNull.Value)
                        {
                            objEntity.SurveynName = Convert.ToString(reader["survey_name"]);
                        }
                        if (reader["redirect_url"] != DBNull.Value)
                        {
                            objEntity.PartnerRedirectUrl = Convert.ToString(reader["redirect_url"]);
                        }
                        if (reader["is_show_end_page"] != DBNull.Value)
                        {
                            objEntity.IsShowEndPage = Convert.ToBoolean(reader["is_show_end_page"]);
                        }
                        if (reader["source"] != DBNull.Value)
                        {
                            objEntity.Source = Convert.ToString(reader["source"]);
                        }
                        if (reader["is_s2s_endpage"] != DBNull.Value)
                        {
                            objEntity.IsS2SEndpage = Convert.ToBoolean(reader["is_s2s_endpage"]);
                        }
                        if (reader["status"] != DBNull.Value)
                        {
                            objEntity.Status = Convert.ToString(reader["status"]);
                        }
                        if (reader["postback_url"] != DBNull.Value)
                        {
                            objEntity.PostbackUrl = Convert.ToString(reader["postback_url"]);
                        }
                        if (reader["term_url"] != DBNull.Value)
                        {
                            objEntity.TermUrl = Convert.ToString(reader["term_url"]);
                        }
                        if (reader["is_popup"] != DBNull.Value)
                        {
                            objEntity.IsPopUp = Convert.ToBoolean(reader["is_popup"]);
                        }
                        if (reader["org_type_id"] != DBNull.Value)
                        {
                            objEntity.OrgTypeId = Convert.ToInt32(reader["org_type_id"]);
                        }
                        if (reader["home_page_url"] != DBNull.Value)
                        {
                            objEntity.HomePageUrl = reader["home_page_url"].ToString();
                        }
                        if (reader["hash_type"] != DBNull.Value)
                        {
                            objEntity.HashType = reader["hash_type"].ToString();
                        }
                        if (reader["hash_key"] != DBNull.Value)
                        {
                            objEntity.Hashkey = reader["hash_key"].ToString();
                        }
                        if (reader["hash_params"] != DBNull.Value)
                        {
                            objEntity.HashParams = reader["hash_params"].ToString();
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objEntity.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if(reader["postback_body"] != DBNull.Value)
                        {
                            objEntity.PostbackBody = reader["postback_body"].ToString();
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
            return objEntity;
        }


        #endregion


        #region Pixel Fire status Update
        public void APIResponseUpdate(Guid UserInvitationGuid, string response)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            try
            {
                SqlCommand cm = new SqlCommand("[partner].[apipostresponse_update]", cn);
                cn.Open();
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@api_response", response);
                cm.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion


        #region GetQutoaGuid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitaionGuid"></param>
        /// <returns></returns>
        public string GetQutoaGuid(string invitaionGuid)
        {
            string quotdGroupGuid = string.Empty;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString());
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("pms.quotaguid_by_invitationguid_get", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", new Guid(invitaionGuid));

                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["target_guid"] != DBNull.Value)
                        {
                            quotdGroupGuid = reader["target_guid"].ToString();
                        }
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
            return quotdGroupGuid;
        }
        #endregion

        #region GetOrgid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public string GetOrgId(string userGuid)
        {
            string orgId = string.Empty;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString());
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("pms.orgid_by_userguid_get", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", new Guid(userGuid));

                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            orgId = (reader["org_id"]).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                cn.Close();
            }
            return orgId;
        }
        #endregion


        #region GetConnectionString
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public string GetConnectionString(int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            string connection = string.Empty;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@client_id", ClientId);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["s_name"] != DBNull.Value)
                        {
                            connection = dr["s_name"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
            return connection;
        }
        #endregion
    }
}
