using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class EndLinksDataServer
    {
        #region Connection String
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }

        public string ConnectionStringSurvey
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            }
        }
        public string ConnectionString3
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            }
        }
        #endregion 


        #region Data Fetch -User  Survey Status
        /// <summary>
        /// Data Fetch -User  Survey Status
        /// </summary>
        /// <param name="statusGuid"></param>
        /// <param name="invitationGuid"></param>
        /// <param name="cost"></param>
        /// <param name="federatedProjectId"></param>
        /// <returns></returns>

        public Surveys GetSurveyInvitationStatus(Guid statusGuid, Guid invitationGuid, Decimal cost, int federatedProjectId, string car, string sdv, string ApipartnerStatus)
        {
            Surveys oSurveys = new Surveys();
            string constr = GetConnectionStringByInvitationGuid(invitationGuid, statusGuid, car);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_prelim_status_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@status_guid", statusGuid);
                cm.Parameters.AddWithValue("@invitation_guid", invitationGuid);
                cm.Parameters.AddWithValue("@sdv", sdv);
                cm.Parameters.AddWithValue("@api_partner_status", ApipartnerStatus);
                //Cost & Fed Proejct Id was Added on 3/29/2013 
                if (cost > 0)
                {
                    cm.Parameters.AddWithValue("@fed_project_cost", cost);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_project_cost", DBNull.Value);
                }

                if (federatedProjectId > 0)
                {
                    cm.Parameters.AddWithValue("@fed_project_id", federatedProjectId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_project_id", DBNull.Value);
                }

                SqlDataReader reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["external_member_id"] != DBNull.Value)
                    {
                        oSurveys.ExternalMemberId = reader["external_member_id"].ToString();
                    }
                    if (reader["is_river"] != DBNull.Value)
                    {
                        oSurveys.IsRiver = Convert.ToInt32(reader["is_river"]);
                    }
                    if (reader["user_guid"] != DBNull.Value)
                    {
                        oSurveys.UserGuid = new Guid(reader["user_guid"].ToString());
                    }

                    if (reader["sub_id3"] != DBNull.Value)
                    {
                        oSurveys.SubId3 = reader["sub_id3"].ToString();
                    }
                    if (reader["tx_id"] != DBNull.Value)
                    {
                        oSurveys.Txid = reader["tx_id"].ToString();
                    }
                    if (reader["is_show_top10"] != DBNull.Value)
                    {
                        oSurveys.IsTop10CompleteCheck = Convert.ToBoolean(reader["is_show_top10"]);
                    }
                    if (reader["org_type_id"] != DBNull.Value)
                    {
                        oSurveys.OrganizationTypeId = Convert.ToInt32(reader["org_type_id"]);
                    }
                    if (reader["member_url"] != DBNull.Value)
                    {
                        oSurveys.MemberUrl = Convert.ToString(reader["member_url"]);
                    }
                    if (reader["status_guid"] != DBNull.Value)
                    {
                        oSurveys.StatusGuid = new Guid(reader["status_guid"].ToString());
                    }
                    if (reader["user_id"] != DBNull.Value)
                    {
                        oSurveys.UserId = Convert.ToInt32(reader["user_id"]);
                    }
                    if (reader["target_type_id"] != DBNull.Value)
                    {
                        oSurveys.TartgetTypeId = Convert.ToInt32(reader["target_type_id"]);
                    }
                    if (reader["activity_type_id"] != DBNull.Value)
                    {
                        oSurveys.UserInvitationStatusId = Convert.ToInt32(reader["activity_type_id"]);
                    }
                    if (reader["external_redirect_url"] != DBNull.Value)
                    {
                        oSurveys.ExtRedirectUrl = Convert.ToString(reader["external_redirect_url"]);
                    }
                    if (reader["target_postback_url"] != DBNull.Value)
                    {
                        oSurveys.PostbackURL = Convert.ToString(reader["target_postback_url"]);
                        //this will be used to fire Pixels.
                        if (oSurveys.PostbackURL.Contains("<img"))
                        {
                            oSurveys.postbacktext = Regex.Match(oSurveys.PostbackURL, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                        }
                        else if (oSurveys.PostbackURL.Contains("<iframe"))
                        {
                            oSurveys.postbacktext = Regex.Match(oSurveys.PostbackURL, "<iframe.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                        }
                    }
                    if (reader["pixel_type_id"] != DBNull.Value)
                    {
                        oSurveys.PixelTypeId = Convert.ToInt32(reader["pixel_type_id"]);
                    }
                    if (reader["fed_redirect_url"] != DBNull.Value)
                    {
                        oSurveys.FedRedirectURl = Convert.ToString(reader["fed_redirect_url"]);
                    }
                    if (reader["is_top10_enabled"] != DBNull.Value)
                    {
                        oSurveys.IsTop10Enable = Convert.ToBoolean(reader["is_top10_enabled"]);
                    }
                    if (reader["project_status_id"] != DBNull.Value)
                    {
                        oSurveys.ProjectStatusId = Convert.ToInt32(reader["project_status_id"]);
                    }
                    //Pixel Firing & Activity Information.
                    if (reader["org_id"] != DBNull.Value)
                    {
                        oSurveys.OrgId = Convert.ToInt32(reader["org_id"]);
                    }
                    if (reader["target_id"] != DBNull.Value)
                    {
                        oSurveys.Targetid = Convert.ToInt32(reader["target_id"]);
                    }
                    if (reader["internal_member"] != DBNull.Value)
                    {
                        oSurveys.IsInternalMenber = Convert.ToBoolean(reader["internal_member"]);
                    }
                    if (reader["quota_group_ids"] != DBNull.Value)
                    {
                        oSurveys.MatchedQuotas = Convert.ToString(reader["quota_group_ids"]);
                    }
                    if (reader["user_invitation_id"] != DBNull.Value)
                    {
                        oSurveys.InvitationId = Convert.ToInt64(reader["user_invitation_id"]);
                    }
                    if (reader["survey_complete_reward_amount"] != DBNull.Value)
                    {
                        oSurveys.SurveyCompleteRewardAmount = Convert.ToDecimal(reader["survey_complete_reward_amount"]);
                    }
                    if (reader["member_reward"] != DBNull.Value)
                    {
                        oSurveys.MemberReward = Convert.ToDecimal(reader["member_reward"]);
                    }
                    if (reader["member_reward_points"] != DBNull.Value)
                    {
                        oSurveys.MemberRewardPoints = Convert.ToInt32(reader["member_reward_points"]);
                    }
                    if (reader["partner_revneue_share"] != DBNull.Value)
                    {
                        oSurveys.PartnerRevenueShare = Convert.ToDecimal(reader["partner_revneue_share"]);
                    }
                    if (reader["api_partner_project_cost"] != DBNull.Value)
                    {
                        oSurveys.ApiPartnerProjectCost = Convert.ToDecimal(reader["api_partner_project_cost"]);
                    }
                    if (reader["survey_name"] != DBNull.Value)
                    {
                        oSurveys.SurveyName = Convert.ToString(reader["survey_name"]);
                    }
                    if (reader["project_id"] != DBNull.Value)
                    {
                        oSurveys.ProjectId = Convert.ToInt32(reader["project_id"]);
                    }
                    if (reader["sub_id"] != DBNull.Value)
                    {
                        oSurveys.SubId = Convert.ToString(reader["sub_id"]);
                    }
                    if (reader["survey_taking_ip"] != DBNull.Value)
                    {
                        oSurveys.SurveyTakingIp = Convert.ToString(reader["survey_taking_ip"]);
                    }
                    if (reader["original_project_id"] != DBNull.Value)
                    {
                        oSurveys.OrginalProjectId = Convert.ToInt32(reader["original_project_id"]);
                    }
                    if (reader["redirect_dt"] != DBNull.Value)
                    {
                        oSurveys.RedirectDt = Convert.ToDateTime(reader["redirect_dt"]);
                    }
                    if (reader["source"] != DBNull.Value)
                    {
                        oSurveys.Source = Convert.ToString(reader["source"]);
                    }
                    if (reader["fed_response_id"] != DBNull.Value)
                    {
                        oSurveys.Fedresponseid = new Guid(reader["fed_response_id"].ToString());
                    }
                    if (reader["referrer_id"] != DBNull.Value)
                    {
                        oSurveys.ReferrerId = Convert.ToInt32(reader["referrer_id"]);
                    }
                    if (reader["external_partner_id"] != DBNull.Value)
                    {
                        oSurveys.ExternalPartnerID = Convert.ToInt32(reader["external_partner_id"]);
                    }
                    if (reader["language_code"] != DBNull.Value)
                    {
                        oSurveys.LanguageCode = Convert.ToString(reader["language_code"]);
                    }
                    if (reader["target_country_id"] != DBNull.Value)
                    {
                        oSurveys.TargetCountryID = Convert.ToInt32(reader["target_country_id"]);
                    }
                    if (reader["project_org_id"] != DBNull.Value)
                    {
                        oSurveys.ProjectOrgID = Convert.ToInt32(reader["project_org_id"]);
                    }
                    if (reader["org_guid"] != DBNull.Value)
                    {
                        oSurveys.OrgGuid = new Guid(reader["org_guid"].ToString());
                    }
                    if (reader["project_cost"] != DBNull.Value)
                    {
                        oSurveys.ProjectCost = Convert.ToDecimal(reader["project_cost"]);
                    }
                    if (reader["is_simplify_affiliate"] != DBNull.Value)
                    {
                        oSurveys.Is_simplify_affiliate = Convert.ToBoolean(reader["is_simplify_affiliate"]);
                    }
                    if (reader["is_peerly_2_affiliate"] != DBNull.Value)
                    {
                        oSurveys.Is_peerly2_affiliate = Convert.ToBoolean(reader["is_peerly_2_affiliate"]);
                    }
                    if (reader["e_rm"] != DBNull.Value)
                    {
                        oSurveys.E_RM = Convert.ToString(reader["e_rm"]);
                    }
                    if (reader["e_rl"] != DBNull.Value)
                    {
                        oSurveys.E_Rl = Convert.ToString(reader["e_rl"]);
                    }
                    if (reader["client_key"] != DBNull.Value)
                    {
                        oSurveys.ClientKey = new Guid(reader["client_key"].ToString().ToUpper());
                    }
                    if (reader["encryption_type_id"] != DBNull.Value)
                    {
                        oSurveys.EncryptionTypeID = Convert.ToInt32(reader["encryption_type_id"]);
                    }
                    if (reader["qsf_survey_url"] != DBNull.Value)
                    {
                        oSurveys.QSFSurveyURL = Convert.ToString(reader["qsf_survey_url"]);
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
            return oSurveys;

        }
        #endregion

        #region Survey Activity Insert
        /// <summary>
        /// Survey Activity Insert
        /// </summary>
        /// <param name="oSurveys"></param>
        /// <param name="invitationGuid"></param>
        public bool SurveyActivityInsert(Surveys oSurveys, Guid invitationGuid, string ApipartnerStatus, string ApiclientStatus)
        {
            bool is_quality_term = false;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            SqlCommand cmd;
            cmd = new SqlCommand("[user].[survey_activity_for_target/quotas_matched_insert]", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@user_invitation_guid", invitationGuid);
            cmd.Parameters.AddWithValue("@project_id", oSurveys.ProjectId);
            cmd.Parameters.AddWithValue("@user_id", oSurveys.UserId);
            //dCost & Fed Proejct Id was Added on 3/29/2013 
            cmd.Parameters.AddWithValue("@is_internal_member", oSurveys.IsInternalMenber);
            cmd.Parameters.AddWithValue("@user_invitation_id", oSurveys.InvitationId);
            cmd.Parameters.AddWithValue("@activity_type_id", oSurveys.UserInvitationStatusId);
            cmd.Parameters.AddWithValue("@matched_quotas", oSurveys.MatchedQuotas);
            cmd.Parameters.AddWithValue("@org_id", oSurveys.OrgId);
            cmd.Parameters.AddWithValue("@target_id", oSurveys.Targetid);
            cmd.Parameters.AddWithValue("@api_partner_status", ApipartnerStatus);
            cmd.Parameters.AddWithValue("@api_client_status", ApiclientStatus);
            if (oSurveys.RedirectDt != DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@redirect_dt", oSurveys.RedirectDt);
            }
            else
            {
                cmd.Parameters.AddWithValue("@redirect_dt", DBNull.Value);
            }
            if (oSurveys.Fedresponseid != Guid.Empty)
            {
                cmd.Parameters.AddWithValue("@fed_response_id", oSurveys.Fedresponseid.ToString());
            }
            else
            {
                cmd.Parameters.AddWithValue("@fed_response_id", DBNull.Value);
            }
            cmd.Parameters.AddWithValue("@source", oSurveys.Source);
            cmd.Parameters.AddWithValue("@sub_id", oSurveys.SubId);
            cmd.Parameters.AddWithValue("@ip_address", oSurveys.SurveyTakingIp);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["is_quality_term"] != DBNull.Value)
                        {
                            is_quality_term = Convert.ToBoolean(dr["is_quality_term"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { cn.Close(); }
            return is_quality_term;

            //private bool is_quality_term = false;
            //SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            //try
            //{
            //    cn.Open();
            //    SqlCommand cm = new SqlCommand("[user].[survey_activity_for_target/quotas_matched_insert_wip]", cn);
            //    cm.CommandType = CommandType.StoredProcedure;
            //    cm.Parameters.AddWithValue("@user_invitation_guid", invitationGuid);
            //    cm.Parameters.AddWithValue("@project_id", oSurveys.ProjectId);
            //    cm.Parameters.AddWithValue("@user_id", oSurveys.UserId);
            //    //Cost & Fed Proejct Id was Added on 3/29/2013 
            //    cm.Parameters.AddWithValue("@is_internal_member", oSurveys.IsInternalMenber);
            //    cm.Parameters.AddWithValue("@user_invitation_id", oSurveys.InvitationId);
            //    cm.Parameters.AddWithValue("@activity_type_id", oSurveys.UserInvitationStatusId);
            //    cm.Parameters.AddWithValue("@matched_quotas", oSurveys.MatchedQuotas);
            //    cm.Parameters.AddWithValue("@org_id", oSurveys.OrgId);
            //    cm.Parameters.AddWithValue("@target_id", oSurveys.Targetid);
            //    if (oSurveys.RedirectDt != DateTime.MinValue)
            //    {
            //        cm.Parameters.AddWithValue("@redirect_dt", oSurveys.RedirectDt);
            //    }
            //    else
            //    {
            //        cm.Parameters.AddWithValue("@redirect_dt", DBNull.Value);
            //    }
            //    if (oSurveys.Fedresponseid != Guid.Empty)
            //    {
            //        cm.Parameters.AddWithValue("@fed_response_id", oSurveys.Fedresponseid.ToString());
            //    }
            //    else
            //    {
            //        cm.Parameters.AddWithValue("@fed_response_id", DBNull.Value);
            //    }
            //    cm.Parameters.AddWithValue("@source", oSurveys.Source);
            //    cm.Parameters.AddWithValue("@sub_id", oSurveys.SubId);
            //    cm.Parameters.AddWithValue("@ip_address", oSurveys.SurveyTakingIp);

            //    SqlDataReader reader = cm.ExecuteReader();
            //    while (reader.Read())
            //    {

            //    }

            //    //    cm.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    cn.Close();
            //}


        }
        #endregion

        #region Partner Transaction Insert
        /// <summary>
        ///  Partner Transaction Insert
        /// </summary>
        /// <param name="oSurveys"></param>
        /// <param name="invitationGuid"></param>
        public void PartnerTransInsert(Surveys oSurveys, Guid invitationGuid, decimal cost)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[postback_transaction_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", invitationGuid);
                cm.Parameters.AddWithValue("@activity_type_id", oSurveys.UserInvitationStatusId);
                cm.Parameters.AddWithValue("@org_type_id", oSurveys.OrganizationTypeId);
                cm.Parameters.AddWithValue("@user_id", oSurveys.UserId);
                cm.Parameters.AddWithValue("@org_id", oSurveys.OrgId);
                cm.Parameters.AddWithValue("@target_id", oSurveys.Targetid);
                cm.Parameters.AddWithValue("@is_internal_member", oSurveys.IsInternalMenber);
                cm.Parameters.AddWithValue("@user_invitation_id", oSurveys.InvitationId);
                cm.Parameters.AddWithValue("@survey_complete_reward_amount", oSurveys.SurveyCompleteRewardAmount);
                cm.Parameters.AddWithValue("@member_reward", oSurveys.MemberReward);
                cm.Parameters.AddWithValue("@member_reward_points", oSurveys.MemberRewardPoints);
                cm.Parameters.AddWithValue("@partner_revneue_share", oSurveys.PartnerRevenueShare);
                cm.Parameters.AddWithValue("@api_partner_project_cost", oSurveys.ApiPartnerProjectCost);
                cm.Parameters.AddWithValue("@survey_name", oSurveys.SurveyName);
                cm.Parameters.AddWithValue("@project_id", oSurveys.ProjectId);
                cm.Parameters.AddWithValue("@sub_id", oSurveys.SubId);
                cm.Parameters.AddWithValue("@survey_taking_ip", oSurveys.SurveyTakingIp);
                cm.Parameters.AddWithValue("@original_project_id", oSurveys.OrginalProjectId);
                cm.Parameters.AddWithValue("@referrer_id", oSurveys.ReferrerId);
                if (oSurveys.RedirectDt != DateTime.MinValue)
                {
                    cm.Parameters.AddWithValue("@redirect_dt", oSurveys.RedirectDt);
                }
                else
                {
                    cm.Parameters.AddWithValue("@redirect_dt", DBNull.Value);
                }
                cm.Parameters.AddWithValue("@source", oSurveys.Source);
                if (oSurveys.Fedresponseid != Guid.Empty)
                {
                    cm.Parameters.AddWithValue("@fed_response_id", oSurveys.Fedresponseid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_response_id", DBNull.Value);
                }
                if (cost > 0)
                {
                    //Cost & Fed Proejct Id was Added on 3/29/2013 
                    cm.Parameters.AddWithValue("@fed_project_cost", cost);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_project_cost", 0);
                }

                cm.Parameters.AddWithValue("@user_guid", oSurveys.UserGuid);
                cm.Parameters.AddWithValue("@sub_id3", oSurveys.SubId3);
                cm.Parameters.AddWithValue("@tx_id", oSurveys.Txid);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion


        #region Get Org Info 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public string GetOrgInfo(int clientid)
        {
            string _orgInfo = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[admin].[org_info_by_id_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@org_id", clientid);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["org_info"] != DBNull.Value)
                        {
                            _orgInfo = dr["org_info"].ToString();
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

            return _orgInfo;
        }
        #endregion
        #region Take Another Survey
        /// <summary>
        /// Take Another Survey
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="uig"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public Surveys TakeAnotherSurvey(Guid ug, Guid uig, string source, int top1, int clientid)
        {
            string surveyUrl = string.Empty;
            Surveys oSurvey = new Surveys();
            UserDataServices oservoservices = new UserDataServices();
            string constr = oservoservices.GetConnectionString(null, null, clientid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                SqlCommand cmd = new SqlCommand("[api].[surveys_get]", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("user_guid", ug);
                cmd.Parameters.AddWithValue("old_user_invitation_guid", uig);
                cmd.Parameters.AddWithValue("source", source);
                cmd.Parameters.AddWithValue("@is_top1_survey_required", top1);

                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["survey_url"] != DBNull.Value)
                        {
                            //surveyUrl = dr["survey_url"].ToString();
                            oSurvey.SurveyUrl = dr["survey_url"].ToString();
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            oSurvey.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["survey_name"] != DBNull.Value)
                        {
                            oSurvey.SurveyName = dr["survey_name"].ToString();
                        }
                        if (dr["survey_length"] != DBNull.Value)
                        {
                            oSurvey.SurveyLength = Convert.ToInt32(dr["survey_length"]);
                        }
                        if (dr["reward_text"] != DBNull.Value)
                        {
                            oSurvey.SurveyCompletereward = dr["reward_text"].ToString();
                        }
                        if (dr["click_count"] != DBNull.Value)
                        {
                            oSurvey.Count = Convert.ToInt32(dr["click_count"]);
                        }
                        if (dr["click_count"] != DBNull.Value)
                        {
                            oSurvey.Count = Convert.ToInt32(dr["click_count"]);
                        }
                        if (dr["org_info"] != DBNull.Value)
                        {
                            oSurvey.orgInformation = Convert.ToString(dr["org_info"]);
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

            return oSurvey;
        }
        #endregion


        #region GetConnectionStrings
        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetConnectionString(Guid invitationGuid)
        {
            string _connectionstring = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@user_guid", invitationGuid);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {

                        if (dr["s_name"] != DBNull.Value)
                        {
                            _connectionstring = dr["s_name"].ToString();
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
            return _connectionstring;
        }

        //Connection String Based on the Invitation GUID.
        public string GetConnectionStringByInvitationGuid(Guid invitationGuid, Guid user_status_guid, string car)
        {
            string _connectionstring = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@user_invitation_guid", invitationGuid);
                cm.Parameters.AddWithValue("@user_status_guid", user_status_guid);
                cm.Parameters.AddWithValue("@car", car);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["name"] != DBNull.Value)
                        {
                            _connectionstring = dr["name"].ToString();
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
            return _connectionstring;
        }
        #endregion

        #region Sentry Validation Check
        public SentryEndURL SentryValidationCheck(Guid uig, string sentry_status, int cid)
        {
            SentryEndURL objSentry = new SentryEndURL();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, cid);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            SqlCommand cmd = new SqlCommand("[pms].[user_2_sentry_validation_check]", con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("user_invitation_guid", uig);
            cmd.Parameters.AddWithValue("sentry_status", Convert.ToInt32(sentry_status));
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            objSentry.RedirectUrl = dr["redirect_url"].ToString();
                        }
                        if (dr["user_invitation_guid"] != DBNull.Value)
                        {
                            objSentry.InvitationGUID = new Guid(dr["user_invitation_guid"].ToString());
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
                con.Close();
            }
            return objSentry;
        }
        #endregion

        #region Recptcha Entry Information Get
        /// <summary>
        /// GetRecaptchaEntryInfo
        /// </summary>
        /// <param name="uig"></param>
        /// <param name="ug"></param>
        /// <returns></returns>

        public Recaptcha GetRecaptchaEntryInfo(Guid uig, Guid ug, int clientid)
        {
            Recaptcha objRecaptcha = new Recaptcha();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            SqlCommand cmd = new SqlCommand("[pms].[user_2_survey_recaptcha_entry_info_get]", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("user_invitation_guid", uig);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["has_prescreener_questions"] != DBNull.Value)
                        {
                            objRecaptcha.HasPrescreenerQuestions = Convert.ToBoolean(dr["has_prescreener_questions"].ToString());
                        }
                        if (dr["survey_url"] != DBNull.Value)
                        {
                            objRecaptcha.SurveyUrl = dr["survey_url"].ToString();
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            objRecaptcha.ProjectId = Convert.ToInt32(dr["project_id"].ToString());
                        }
                        if (dr["balancing_type_id"] != DBNull.Value)
                        {
                            objRecaptcha.BalancingTypeId = Convert.ToInt32(dr["balancing_type_id"].ToString());
                        }
                        if (dr["is_internal_member"] != DBNull.Value)
                        {
                            objRecaptcha.IsInternalMember = Convert.ToBoolean(dr["is_internal_member"].ToString());
                        }
                        if (dr["user_invitation_id"] != DBNull.Value)
                        {
                            objRecaptcha.UserInvitationId = Convert.ToInt64(dr["user_invitation_id"].ToString());
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objRecaptcha.UserId = Convert.ToInt32(dr["user_id"].ToString());
                        }
                        if (dr["target_id"] != DBNull.Value)
                        {
                            objRecaptcha.TargetId = Convert.ToInt32(dr["target_id"]);
                        }
                        if (dr["fed_response_id"] != DBNull.Value)
                        {
                            objRecaptcha.FedResponseId = dr["fed_response_id"].ToString();
                        }
                        if (dr["source"] != DBNull.Value)
                        {
                            objRecaptcha.Source = (dr["source"].ToString());
                        }

                        //Added by Rajani G on 6/4/2018.
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            objRecaptcha.Step7Guid = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["user_invitation_guid"] != DBNull.Value)
                        {
                            objRecaptcha.UserInvitationGuid = new Guid(dr["user_invitation_guid"].ToString());
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            objRecaptcha.CountryID = Convert.ToInt32(dr["country_id"].ToString());
                        }
                        if (dr["demand_api_quota_id"] != DBNull.Value)
                        {
                            objRecaptcha.DemandAPIQuotaID = (dr["demand_api_quota_id"].ToString());
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
            return objRecaptcha;

        }
        #endregion

        #region GetQuota Completes/accesses
        /// <summary>
        /// GetQuotaCompletesaccesses
        /// </summary>
        /// <param name="prjId"></param>
        /// <param name="balaneTypeId"></param>
        /// <param name="ug"></param>
        /// <returns></returns>
        public List<Quotas> GetQuotaCompletesaccesses(int prjId, int balaneTypeId, Guid ug, int clientid)
        {
            List<Quotas> lstQuotas = new List<Entities.Quotas>();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            SqlCommand cmd;
            if (balaneTypeId == 1)
                cmd = new SqlCommand("pms.quota_close_or_open_by_completes", cn);
            else
                cmd = new SqlCommand("pms.quota_close_or_open_by_accesses", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("project_id", prjId);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Quotas objQuotas = new Quotas();

                        if (dr["quota_group_id"] != DBNull.Value)
                        {
                            objQuotas.QuotaGroupId = Convert.ToInt32(dr["quota_group_id"].ToString());
                        }
                        if (dr["is_closed"] != DBNull.Value)
                        {
                            objQuotas.IsClosed = Convert.ToBoolean(dr["is_closed"]);
                        }
                        lstQuotas.Add(objQuotas);
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
            return lstQuotas;
        }
        #endregion

        #region Quota status Update
        public string QuotaStatusUpdate(int prjId, string json, int ClientID, Guid uig)
        {
            string surveyURL = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SqlCommand cmd;
            cmd = new SqlCommand("pms.quota_group_open_or_closed_status_update", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("project_id", prjId);
            cmd.Parameters.AddWithValue("quotas_json", json);
            cmd.Parameters.AddWithValue("user_org_id", ClientID);
            cmd.Parameters.AddWithValue("user_invitation_guid", uig);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            surveyURL = Convert.ToString(dr["redirect_url"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { cn.Close(); }
            return surveyURL;
        }
        #endregion

        #region Selected Quotas get
        /// <summary>
        /// GetSelectedQuotas
        /// </summary>
        /// <param name="uig"></param>
        /// <param name="ug"></param>
        /// <param name="json"></param>
        /// <param name="isInternalMem"></param>
        /// <param name="prjId"></param>
        /// <param name="userId"></param>
        /// <param name="userInvitationId"></param>
        /// <returns></returns>
        public Recaptcha GetSelectedQuotas(Guid uig, Guid ug, string json, bool isInternalMem, int prjId, int userId, Int64 userInvitationId, int clientid)
        {
            Recaptcha objRecaptcha = new Recaptcha();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            SqlCommand cmd = new SqlCommand("[user].[user_2_quotas_selected_get]", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("user_invitation_guid", uig);
            cmd.Parameters.AddWithValue("is_internal_member", isInternalMem);
            cmd.Parameters.AddWithValue("project_id", prjId);
            cmd.Parameters.AddWithValue("user_id", userId);
            cmd.Parameters.AddWithValue("user_invitation_id", userInvitationId);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            objRecaptcha.SurveyUrl = dr["redirect_url"].ToString();
                        }
                        if (dr["activity_type_id"] != DBNull.Value)
                        {
                            objRecaptcha.ActivityTypeId = Convert.ToInt32(dr["activity_type_id"].ToString());
                        }
                        if (dr["quota_group_ids"] != DBNull.Value)
                        {
                            objRecaptcha.MatchedQuotas = (dr["quota_group_ids"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { cn.Close(); }
            return objRecaptcha;

        }
        #endregion

        #region Insert Mached Quotas
        /// <summary>
        /// InsertMatchedQuotas
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="prjId"></param>
        /// <param name="userId"></param>
        /// <param name="isInternalMem"></param>
        /// <param name="userInvitationId"></param>
        /// <param name="activityTypeId"></param>
        /// <param name="matchedQuotas"></param>
        /// <param name="uig"></param>
        public void InsertMatchedQuotas(Guid ug, int prjId, int userId, bool isInternalMem, Int64 userInvitationId, int activityTypeId,
            string matchedQuotas, Guid uig, int clientid, int targetId, string fedResponseId)
        {
            SqlConnection cn = new SqlConnection();
            //   string constr = GetConnectionString(ug);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            SqlCommand cmd = new SqlCommand("[user].[survey_activity_for_target/quotas_matched_insert]", cn);
            try
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("user_invitation_guid", uig);
                cmd.Parameters.AddWithValue("project_id", prjId);
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("is_internal_member", isInternalMem);
                cmd.Parameters.AddWithValue("user_invitation_id", userInvitationId);
                cmd.Parameters.AddWithValue("activity_type_id", activityTypeId);
                cmd.Parameters.AddWithValue("matched_quotas", matchedQuotas);
                cmd.Parameters.AddWithValue("org_id", clientid);
                cmd.Parameters.AddWithValue("target_id", targetId);
                cmd.Parameters.AddWithValue("fed_response_id", fedResponseId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region SurveyUrl Get
        /// <summary>
        /// GetSurveyUrl
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="uig"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public SurveyUrl GetSurveyUrl(Guid ug, Guid uig, string source, int clientid)
        {

            SurveyUrl surveyUrl = new SurveyUrl();
            SqlConnection cn = new SqlConnection();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientid);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            SqlCommand cmd = new SqlCommand("[pms].[surveyurl_by_user_invitation_guid_get]", cn);
            cn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("source", source);
            cmd.Parameters.AddWithValue("user_invitation_guid", uig);
            try
            {
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["survey_url"] != DBNull.Value)
                        {
                            surveyUrl.ProjectUrl = Convert.ToString(dr["survey_url"]);
                        }
                        if (dr["client_id"] != DBNull.Value)
                        {
                            surveyUrl.client_id = Convert.ToInt32(dr["client_id"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            surveyUrl.projectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["cpi"] != DBNull.Value)
                        {
                            surveyUrl.cpi = Convert.ToDouble(dr["cpi"]);
                        }
                        if (dr["project_guid"] != DBNull.Value)
                        {
                            surveyUrl.projectGuid = new Guid(dr["project_guid"].ToString());
                        }
                        if (dr["payload"] != DBNull.Value)
                        {
                            surveyUrl.Payload = Convert.ToString(dr["payload"]);
                        }
                        if (dr["respondent_json"] != DBNull.Value)
                        {
                            surveyUrl.RespondentJSON = Convert.ToString(dr["respondent_json"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { cn.Close(); }
            return surveyUrl;
        }
        #endregion

        #region Project Refresh
        public void ProjectRefresh(Guid projectGuid, string projectManagerEmailAddress, string Isr, double cpi)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[bid_clone_project_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@actual_project_guid", projectGuid);
                cm.Parameters.AddWithValue("@email_address", projectManagerEmailAddress);
                cm.Parameters.AddWithValue("@is_r", Isr);
                cm.Parameters.AddWithValue("@cpi", cpi);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region GetCookie
        public string GetCookie(Guid ug, int cid)
        {
            //User ouser = new User();
            string id = string.Empty;
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, cid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("[user].[pendingcookies_get]", cn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@user_guid", ug);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["cookie_ids"] != DBNull.Value)
                        {
                            id = Convert.ToString(reader["cookie_ids"]);
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
            return id;
        }
        #endregion

        #region Save Cookie
        public void saveCookie(Guid ug, string CookieIds, int cid)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, cid);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[cookies_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", ug);
                cm.Parameters.AddWithValue("@cookie_ids", CookieIds);
                cm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }

        }
        #endregion

        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop20(Guid leadguid)
        {

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top20_questions_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 420;
                cm.Parameters.AddWithValue("@external_member_guid", leadguid);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    List<ProfileQuestions> lstParentQuestion = new List<ProfileQuestions>();
                    List<ProfileOptions> lstOptions = new List<ProfileOptions>();
                    List<ProfileQuestions> lstChildQuestions = new List<ProfileQuestions>();
                    List<ProfileOptions> lstChildQuestionsOptions = new List<ProfileOptions>();
                    List<ProfileQuestions> lstChldQoMaping = new List<ProfileQuestions>();
                    List<ProfileQuestions> lstQuestionResponses = new List<ProfileQuestions>();
                    List<ProfileQuestions> lstSubChildQuestions = new List<ProfileQuestions>();
                    List<ProfileOptions> lstSubChildQuestionsOptions = new List<ProfileOptions>();
                    ProfileQuestions oQstOrgInfo = new ProfileQuestions();
                    //patrent Questions get
                    while (dr.Read())
                    {
                        ProfileQuestions objQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objQuestion.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["is_autopostback"] != DBNull.Value)
                        {
                            objQuestion.AutoPostBack = Convert.ToInt32(dr["is_autopostback"]);
                        }
                        if (dr["question_hide"] != DBNull.Value)
                        {
                            objQuestion.QuestionHide = Convert.ToBoolean(dr["question_hide"]);
                        }
                        lstParentQuestion.Add(objQuestion);
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildQuestions Get
                        ProfileQuestions objChildQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }

                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objChildQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        lstChildQuestions.Add(objChildQuestion);

                    }
                    //Subchild Questions Get
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildQuestions Get
                        ProfileQuestions objSubChildQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }

                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objSubChildQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        lstSubChildQuestions.Add(objSubChildQuestion);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //Parent Options

                        ProfileQuestions oquestion = new ProfileQuestions();
                        ProfileOptions objOptions = new ProfileOptions();
                        foreach (ProfileQuestions qu in lstParentQuestion)
                        {
                            if (qu.QuestionId == Convert.ToInt32(dr["question_id"]))
                            {
                                oquestion = qu;
                                break;
                            }
                        }
                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        oquestion.OptionList.Add(objOptions);
                        lstOptions.Add(objOptions);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildOptions
                        ProfileOptions objOptions = new ProfileOptions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        if (dr["special_grouping_id"] != DBNull.Value)
                        {
                            objOptions.SpecialGroupingId = Convert.ToInt32(dr["special_grouping_id"]);
                        }
                        lstChildQuestionsOptions.Add(objOptions);

                    }
                    //subChildOptions
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildOptions
                        ProfileOptions objSubChildjOptions = new ProfileOptions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objSubChildjOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        if (dr["parent_option_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.ParentOptionId = Convert.ToInt32(dr["parent_option_id"]);
                        }
                        lstSubChildQuestionsOptions.Add(objSubChildjOptions);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //mapping for parent Question and child Question
                        ProfileQuestions objChildQoOptionMaping = new ProfileQuestions();
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }

                        if (dr["option_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.OptionId = Convert.ToInt32(dr["option_id"]);
                        }

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        lstChldQoMaping.Add(objChildQoOptionMaping);

                    }
                    //dr.NextResult();
                    //while (dr.Read())
                    //{
                    //    //Totla Questions Response
                    //    ProfileQuestions objQuestionResponses = new ProfileQuestions();
                    //    if (dr["question_id"] != DBNull.Value)
                    //    {
                    //        objQuestionResponses.QuestionId = Convert.ToInt32(dr["question_id"]);
                    //    }
                    //    if (dr["option_id"] != DBNull.Value)
                    //    {
                    //        objQuestionResponses.OptionId = Convert.ToInt32(dr["option_id"]);
                    //    }
                    //    lstQuestionResponses.Add(objQuestionResponses);
                    //}
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //get the org information
                        if (dr["question_id"] != DBNull.Value)
                        {
                            oQstOrgInfo.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["org_info"] != DBNull.Value)
                        {
                            oQstOrgInfo.OrgInfo = Convert.ToString(dr["org_info"]);
                        }

                    }
                    if (lstParentQuestion.Count == 0)
                    {
                        lstParentQuestion.Add(oQstOrgInfo);
                    }
                    else
                    {
                        lstParentQuestion[0].OrgInfo = oQstOrgInfo.OrgInfo;
                    }
                    //Loop for Parent Questions responses
                    foreach (ProfileQuestions objParentQuestionsResponses in lstParentQuestion)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objParentQuestionsResponses.QuestionId == objResponses.QuestionId)
                            {
                                if (objParentQuestionsResponses.QuestionTypeId == 3)
                                {
                                    objParentQuestionsResponses.ResponseOptionList.Add(objResponses);
                                }
                                else
                                {
                                    objParentQuestionsResponses.OptionId = objResponses.OptionId;
                                }
                            }
                        }
                    }
                    //Loop for Child Questions Responses

                    foreach (ProfileQuestions objChildQuestionsResponses in lstChildQuestions)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objChildQuestionsResponses.QuestionId == objResponses.QuestionId)
                            {
                                if (objChildQuestionsResponses.QuestionTypeId == 3)
                                {
                                    objChildQuestionsResponses.ResponseOptionList.Add(objResponses);
                                }
                                else
                                {
                                    objChildQuestionsResponses.OptionId = objResponses.OptionId;
                                }
                            }
                        }
                    }
                    //Loop for SubChild Questions Responses

                    foreach (ProfileQuestions objSubChildResponses in lstSubChildQuestions)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objSubChildResponses.QuestionId == objResponses.QuestionId)
                            {
                                objSubChildResponses.OptionId = objResponses.OptionId;
                            }
                        }
                    }
                    // childquestions and options inserting into parent questions
                    foreach (ProfileQuestions childQuestion in lstChildQuestions)
                    {
                        foreach (ProfileQuestions parentQuestion in lstParentQuestion)
                        {
                            if (childQuestion.ParentQuestionId == parentQuestion.QuestionId)
                            {
                                foreach (ProfileOptions childQuestionOptions in lstChildQuestionsOptions)
                                {
                                    if (childQuestionOptions.QuestionId == childQuestion.QuestionId)
                                    {
                                        childQuestion.OptionList.Add(childQuestionOptions);
                                    }

                                }
                                parentQuestion.ChildQuestionList.Add(childQuestion);

                            }
                        }

                        //}

                    }

                    //subchildquestions and options inserting into Childquestions


                    foreach (ProfileQuestions SubChildQuestions in lstSubChildQuestions)
                    {
                        foreach (ProfileQuestions ChildQuestons in lstChildQuestions)
                        {
                            if (ChildQuestons.QuestionId == SubChildQuestions.ParentQuestionId)
                            {
                                foreach (ProfileOptions SubchildQuestionOptions in lstSubChildQuestionsOptions)
                                {
                                    if (SubChildQuestions.QuestionId == SubchildQuestionOptions.QuestionId)
                                    {
                                        SubChildQuestions.OptionList.Add(SubchildQuestionOptions);
                                    }
                                }
                                ChildQuestons.ChildQuestionList.Add(SubChildQuestions);
                                ChildQuestons.SubChildOptions = lstSubChildQuestionsOptions;

                            }
                        }
                    }
                    //mapping data insert into parent question options
                    foreach (ProfileOptions parentOptions in lstOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == parentOptions.OptionId)
                            {
                                parentOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //Mapping hide options list tot child options
                    foreach (ProfileOptions chOptions in lstChildQuestionsOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == chOptions.OptionId)
                            {
                                chOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //Mapping hide options list tot child options
                    foreach (ProfileOptions subChOptions in lstSubChildQuestionsOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == subChOptions.OptionId)
                            {
                                subChOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //add responses to vechile question
                    foreach (ProfileQuestions _parentquestions in lstParentQuestion)
                    {
                        if (_parentquestions.QuestionId == 175)
                        {
                            foreach (ProfileQuestions objResponses in lstQuestionResponses)
                            {
                                _parentquestions.ResponseOptionList.Add(objResponses);
                            }
                        }
                    }
                    return lstParentQuestion;

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

        }
        #endregion

        #region Top20SaveOptions
        /// <summary>
        ///  Top10SaveOptions
        /// </summary>
        /// <param name="listXml"></param>
        public string Top20SaveOptions(string listXml, Guid UserGuid)
        {
            string memberUrl = "";
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top20_question_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
                cm.Parameters.AddWithValue("@external_member_guid", UserGuid);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["member_url"] != null)
                    {
                        memberUrl = Convert.ToString(oreader["member_url"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }
            return memberUrl;

        }
        #endregion

        #region Insert Exception Data
        /// <summary>
        ///  InsertExceptionData
        /// </summary>
        /// <param name="listXml"></param>
        public void InsertExceptionData(Surveys oSurveys, string response, string key, string url, string json)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[cint_exception_log_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("user_id", oSurveys.UserId);
                cm.Parameters.AddWithValue("project_id", oSurveys.ProjectId);
                cm.Parameters.AddWithValue("org_id", oSurveys.OrgId);
                cm.Parameters.AddWithValue("survey_completed_dt", oSurveys.SurveyCompletedt);
                cm.Parameters.AddWithValue("survey_taking_ip", oSurveys.SurveyTakingIp);
                cm.Parameters.AddWithValue("source", oSurveys.Source);
                cm.Parameters.AddWithValue("loi", oSurveys.SurveyLength);
                cm.Parameters.AddWithValue("survey_name", oSurveys.SurveyName);
                cm.Parameters.AddWithValue("api_response", response);
                cm.Parameters.AddWithValue("api_key", key);
                cm.Parameters.AddWithValue("api_url", url);
                cm.Parameters.AddWithValue("request_json", json);
                cm.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        public string GetUserProfileQueryParameters(string ug, int clientId)
        {
            string queryParams = string.Empty;
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("[user].[get_basic_user_profile_api_query_parameters]", cn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@ug", ug);
                comm.Parameters.AddWithValue("@client_id", clientId);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["profile_params"] != DBNull.Value)
                        {
                            queryParams = Convert.ToString(reader["profile_params"]);
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

            return queryParams;
        }

        #region IsTolunaMemberExist
        public TolunaUser IsTolunaMemberExist(Guid ug, int clientId)
        {
            TolunaUser objUser = new TolunaUser();

            List<TolunaQstOpt> lstQstOpt = new List<TolunaQstOpt>();
            UserDataServices oservices = new UserDataServices();
            string constr = oservices.GetConnectionString(null, null, clientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("[pms].[toluna_user_get]", cn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@user_guid", ug);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["is_toluna_member"] != DBNull.Value)
                        {
                            objUser.IsTolunaMember = Convert.ToBoolean(reader["is_toluna_member"]);
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            objUser.CountryID = Convert.ToInt32(reader["country_id"]);
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            string DOB = Convert.ToDateTime(reader["dob"]).ToString("MM/dd/yyyy");
                            objUser.DOB = DOB.Replace("-", "/");
                        }
                        if (reader["zip_code"] != DBNull.Value)
                        {
                            objUser.ZIPCode = Convert.ToString(reader["zip_code"]);
                        }
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        //ChildQuestions Get
                        TolunaQstOpt objQstOpt = new TolunaQstOpt();
                        if (reader["q_id"] != DBNull.Value)
                        {
                            objQstOpt.QuestionID = Convert.ToInt32(reader["q_id"]);
                        }

                        if (reader["o_id"] != DBNull.Value)
                        {
                            objQstOpt.AnswerID = Convert.ToInt32(reader["o_id"]);
                        }
                        lstQstOpt.Add(objQstOpt);
                    }
                    foreach (TolunaQstOpt objT in lstQstOpt)
                    {
                        objUser.LstQstOpt.Add(objT);
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

            return objUser;
        }
        #endregion

        #region GetClientDetails
        public Client GetClientDetails(int ProjectID)
        {
            int ClientID = 0;
            Client oClient = new Client();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand comm = new SqlCommand("[pms].[get_client_details]", cn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@project_id", ProjectID);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["client_id"] != DBNull.Value)
                        {
                            oClient.ClientId = Convert.ToInt32(reader["client_id"]);
                        }
                        if (reader["language_code"] != DBNull.Value)
                        {
                            oClient.LanguageCode = Convert.ToString(reader["language_code"]);
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

            return oClient;
        }
        #endregion

        #region TolunaUpdateUser
        public void TolunaUpdateUser(Guid UserGuid, int cid)
        {
            UserDataServices oDataServices = new UserDataServices();
            string constr = string.Empty;
            constr = oDataServices.GetConnectionString(null, null, cid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[toluna_user_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.ExecuteNonQuery();
            }

            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion
    }
}
