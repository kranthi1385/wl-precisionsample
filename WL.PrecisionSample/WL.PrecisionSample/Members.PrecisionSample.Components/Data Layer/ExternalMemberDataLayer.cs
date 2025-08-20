using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Members.PrecisionSample.Components.Business_Layer;
using NLog;

namespace Members.PrecisionSample.Components.Data_Layer
{
    class ExternalMemberDataLayer
    {
        #region Public Variables
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion 

        #region public variables
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            }
        }
        public string ConnectionString1
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        #endregion

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
        public Surveys UpdateExternalMember(string QgId, string uid, string qid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                          string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore,
                          string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl, string IPNumber, bool is_dupe, string external_member_id, int project_id, string external_member_guid)
        {
            Surveys oExternalQuotaEntities = new Surveys();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[ext_member_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", uid);
                if(Guid.TryParse(qid, out Guid target_guid))
                {
                    cm.Parameters.AddWithValue("@target_guid", target_guid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@target_id", qid);
                }
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
                cm.Parameters.AddWithValue("@ip_number", Convert.ToDecimal(IPNumber));
                cm.Parameters.AddWithValue("@is_dupe", is_dupe);
                cm.Parameters.AddWithValue("@external_member_id", external_member_id);
                cm.Parameters.AddWithValue("@project_id", project_id);
                cm.Parameters.AddWithValue("@ext_mem_guid", external_member_guid);
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
                        if (dr["external_partner_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.ExternalPartnerID = Convert.ToInt32(dr["external_partner_id"]);
                        }
                        if (dr["member_guid"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.memberGuid = new Guid(dr["member_guid"].ToString());
                        }
                        if (dr["fed_response_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.FedResponseID = Convert.ToString(dr["fed_response_id"]);
                        }
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
        public Surveys CagSaveClickInformation(string QgId, string uid, int qid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                          string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore,
                          string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl, string IPNumber)
        {
            Surveys oExternalQuotaEntities = new Surveys();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[cag_external_member_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", uid);
                cm.Parameters.AddWithValue("@target_id", qid);
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
                cm.Parameters.AddWithValue("@ip_number", Convert.ToDecimal(IPNumber));
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
                        if (dr["external_partner_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.ExternalPartnerID = Convert.ToInt32(dr["external_partner_id"]);
                        }
                        if (dr["member_guid"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.memberGuid = new Guid(dr["member_guid"].ToString());
                        }
                        if (dr["fed_response_id"] != DBNull.Value)
                        {
                            oExternalQuotaEntities.FedResponseID = Convert.ToString(dr["fed_response_id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex != null)
                {

                    if (!string.IsNullOrEmpty(uid))
                    {
                        logger.Error("simplifymember error|" + ex.InnerException);
                    }
                    else
                    {
                        logger.Error("Lbmember error|" + ex.InnerException);
                    }
                }
                throw (ex);
            }
            finally
            {
                cn.Close();
            }
            return oExternalQuotaEntities;
        }

        #endregion

        #region SendSms
        /// <summary>
        /// SendSms
        /// </summary>
        /// <param name="UsetInviationGuid">UserInvitationGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ProjectId">ProjectId</param>
        /// <param name="MobileNumber">MobileNumber</param>
        /// <param name="SurveyName">SurveyName</param>
        /// <param name="OrgId">OrgId</param>
        public int SendSms(string UserInvitationGuid, string UserGuid, string ProjectId, string MobileNumber, string SurveyName, string OrgId)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[sms_status_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@project_id", ProjectId);
                cm.Parameters.AddWithValue("@mobile_number", MobileNumber);
                cm.Parameters.AddWithValue("@survey_name", SurveyName);
                cm.Parameters.AddWithValue("@org_id", OrgId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            result = Convert.ToInt32(dr["result"]);
                        }
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
            return result;
        }
        #endregion

        public string ExternalMemberByIdGet(Guid extMemGuid)
        {
            string res = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[ExternalMemberById_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@external_member_guid", extMemGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_id"] != DBNull.Value)
                        {
                            res = (dr["user_id"]).ToString();
                        }
                        if (dr["survey_terminate_url"] != DBNull.Value)
                        {
                            res = res + ';' + (dr["survey_terminate_url"]).ToString();
                        }
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
            return res;
        }
        public User RewardAndUserInsert(User oUser)
        {

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[RewardandUser_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                cm.Parameters.AddWithValue("@split_flag", oUser.SplitFlag);
                cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                cm.Parameters.AddWithValue("@state_id", oUser.StateId);
                cm.Parameters.AddWithValue("@first_name", oUser.FirstName);
                cm.Parameters.AddWithValue("@last_name", oUser.LastName);
                cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                cm.Parameters.AddWithValue("@friend_id", oUser.FriendId);
                cm.Parameters.AddWithValue("@ethnicity_id", oUser.EthnicityId);
                cm.Parameters.AddWithValue("@registration_step", oUser.RegistrationStep);
                cm.Parameters.AddWithValue("@is_post_allinbox", oUser.IsAllinBoxPosted);
                cm.Parameters.AddWithValue("@captcha_falg", oUser.CaptchaFlag);
                cm.Parameters.AddWithValue("@click_id", oUser.ClickId);
                cm.Parameters.AddWithValue("@hit_id", oUser.HitId);
                cm.Parameters.AddWithValue("@is_fraud", oUser.IsFraud);
                cm.Parameters.AddWithValue("@cpa_count", oUser.CpaCount);
                cm.Parameters.AddWithValue("@is_dnc", oUser.IsDnc);
                cm.Parameters.AddWithValue("@dnc_reason", oUser.DncReason);
                cm.Parameters.AddWithValue("@opt_tiburon_split_flag", oUser.OptTiburonSplitFlag);
                cm.Parameters.AddWithValue("@country2ip", oUser.Country2Ip);
                if (oUser.FbUsername != string.Empty)
                {
                    cm.Parameters.AddWithValue("@fb_username_mailid", oUser.FbUsername);
                }

                if (oUser.Address1 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@address1", oUser.Address1);
                }
                if (oUser.Address2 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@address2", oUser.Address2);
                }
                cm.Parameters.AddWithValue("@city", oUser.City);
                cm.Parameters.AddWithValue("@zip_code", oUser.ZipCode);
                if (oUser.PhoneNumber != string.Empty)
                {
                    cm.Parameters.AddWithValue("@phone_number", oUser.PhoneNumber);
                }
                cm.Parameters.AddWithValue("@gender", oUser.Gender);
                cm.Parameters.AddWithValue("@dob", oUser.DateOfBirth);
                cm.Parameters.AddWithValue("@password", oUser.Password);
                cm.Parameters.AddWithValue("@create_by", oUser.CreatedBy);
                cm.Parameters.AddWithValue("@update_by", oUser.UpdatedBy);

                if (oUser.RefferId != -1)
                {
                    cm.Parameters.AddWithValue("@sub_id1", oUser.RefferId.ToString());
                }
                else
                {
                    cm.Parameters.AddWithValue("@sub_id1", "-1");
                }
                if (oUser.SubId2 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@sub_id2", oUser.SubId2);
                }
                else
                {
                    cm.Parameters.AddWithValue("@sub_id2", "");
                }

                if (oUser.SubId3 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@sub_id3", oUser.SubId3);
                }
                else
                {
                    cm.Parameters.AddWithValue("@sub_id3", "");
                }

                if (oUser.IpAddress != string.Empty)
                {
                    cm.Parameters.AddWithValue("@ip_address", oUser.IpAddress);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ip_address", "");
                }

                if (oUser.ReferrerUrl != string.Empty)
                {
                    cm.Parameters.AddWithValue("@referrer_url", oUser.ReferrerUrl);
                }
                else
                {
                    cm.Parameters.AddWithValue("@referrer_url", "");
                }
                //Added on 1/30/2012 for FB Connect
                if (oUser.FacebookId != string.Empty)
                {
                    cm.Parameters.AddWithValue("@facebook_id", oUser.FacebookId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@facebook_id", "");
                }
                if (oUser.AccessToken != string.Empty)
                {
                    cm.Parameters.AddWithValue("@facebook_access_token", oUser.AccessToken);
                }
                else
                {
                    cm.Parameters.AddWithValue("@facebook_access_token", "");
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["referrer_id"] != DBNull.Value)
                        {
                            oUser.RefferId = Convert.ToInt32((reader["referrer_id"]));
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            oUser.UserId = Convert.ToInt32((reader["user_id"]));
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            oUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            oUser.CountryId = Convert.ToInt32((reader["country_id"]));
                        }
                        if (reader["state_id"] != DBNull.Value)
                        {
                            oUser.StateId = Convert.ToInt32((reader["state_id"]));
                        }
                        if (reader["first_name"] != DBNull.Value)
                        {
                            oUser.FirstName = Convert.ToString((reader["first_name"]));
                        }
                        if (reader["last_name"] != DBNull.Value)
                        {
                            oUser.LastName = Convert.ToString((reader["last_name"]));
                        }
                        if (reader["email_address"] != DBNull.Value)
                        {
                            oUser.EmailAddress = Convert.ToString((reader["email_address"]));
                        }
                        if (reader["address1"] != DBNull.Value)
                        {
                            oUser.Address1 = Convert.ToString((reader["address1"]));
                        }
                        if (reader["address2"] != DBNull.Value)
                        {
                            oUser.Address2 = Convert.ToString((reader["address2"]));
                        }
                        if (reader["city"] != DBNull.Value)
                        {
                            oUser.City = Convert.ToString((reader["city"]));
                        }
                        if (reader["zip_code"] != DBNull.Value)
                        {
                            oUser.ZipCode = Convert.ToString((reader["zip_code"]));
                        }
                        if (reader["phone_number"] != DBNull.Value)
                        {
                            oUser.PhoneNumber = Convert.ToString((reader["phone_number"]));
                        }
                        if (reader["gender"] != DBNull.Value)
                        {
                            oUser.Gender = Convert.ToString((reader["gender"]));
                        }
                        if (reader["password"] != DBNull.Value)
                        {
                            oUser.Password = Convert.ToString((reader["password"]));

                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            oUser.Dob = Convert.ToString((reader["dob"]));

                        }
                        if (reader["create_by"] != DBNull.Value)
                        {
                            oUser.CreatedBy = Convert.ToString((reader["create_by"]));

                        }
                        if (reader["update_by"] != DBNull.Value)
                        {
                            oUser.UpdatedBy = Convert.ToString((reader["update_by"]));

                        }
                        if (reader["ethnicity_id"] != DBNull.Value)
                        {
                            oUser.EthnicityId = Convert.ToInt32((reader["ethnicity_id"]));
                        }
                        if (reader["state_code"] != DBNull.Value)
                        {
                            oUser.StateCode = reader["state_code"].ToString();
                        }
                        if (reader["country_code"] != DBNull.Value)
                        {
                            oUser.CountryCode = reader["country_code"].ToString();
                        }
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
            return oUser;
        }
        public ExtMemberGuidChk ExtMemberInsert(string mid, string pid)
        {
            ExtMemberGuidChk res = new ExtMemberGuidChk();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[ext_member_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", mid);
                if (Guid.TryParse(pid.ToString(), out Guid target_guid))
                {
                    cm.Parameters.AddWithValue("@target_guid", target_guid); 
                }
                else
                {
                    cm.Parameters.AddWithValue("@target_id", pid);
                }
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["is_dupe"] != DBNull.Value)
                        {
                            res.IsDupe = Convert.ToBoolean(dr["is_dupe"]);
                        }
                        if (dr["external_member_id"] != DBNull.Value)
                        {
                            res.ExternalMemberId = Convert.ToInt32(dr["external_member_id"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            res.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["org_id"] != DBNull.Value)
                        {
                            res.OrgId = Convert.ToInt32(dr["org_id"]);
                        }
                        if (dr["external_member_guid"] != DBNull.Value)
                        {
                            res.ExternalMemberGuid = new Guid(dr["external_member_guid"].ToString());
                        }

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
            return res;
        }

        public string ExternalMemberActivityInsert(string external_member_id, int project_id, string pid, string ipAddress, string frid, decimal ecost, string mobiledeviceModel, int userTrafficTypeId, int org_id, string external_member_guid)
        {
            string res = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[external_member_activity_insert_v1]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@external_member_id", external_member_id);
                if (Guid.TryParse(pid.ToString(), out Guid target_guid))
                {
                    cm.Parameters.AddWithValue("@target_guid", target_guid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@target_id", pid);
                }
                cm.Parameters.AddWithValue("@project_id", project_id);
                cm.Parameters.AddWithValue("@survey_taking_ip", ipAddress);
                cm.Parameters.AddWithValue("@ecost", ecost);
                cm.Parameters.AddWithValue("@fed_response_id", frid);
                cm.Parameters.AddWithValue("@mobile_device_model", mobiledeviceModel);
                cm.Parameters.AddWithValue("@user_device_type_id", userTrafficTypeId);
                cm.Parameters.AddWithValue("@org_id", org_id);
                cm.Parameters.AddWithValue("@external_member_guid", new Guid(external_member_guid));
                cm.ExecuteNonQuery();
                res = "success";
            }
            catch (Exception ex)
            {
                throw (ex);
                res = "failed";
            }
            finally
            {
                cn.Close();
            }
            return res;
        }

        #region CleanIDDataInsert
        public string CleanIDDataInsert(string uig, string ug, int pid, string extmid, int cid, string json,string IpqsJSON, string sessionId, string sessionResponse)
        {
            string SurveyURL = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                //cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
                SqlCommand cmd = new SqlCommand("[pms].[clean_id_data_insert]", cn);
                cmd.Parameters.AddWithValue("@user_id", extmid);
                cmd.Parameters.AddWithValue("@external_member_id", extmid);
                cmd.Parameters.AddWithValue("@external_member_guid", uig);
                cmd.Parameters.AddWithValue("@json", json);
                cmd.Parameters.AddWithValue("@project_id", pid);
                cmd.Parameters.AddWithValue("@org_id", cid);
                cmd.Parameters.AddWithValue("@ipqs_json", IpqsJSON);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    cmd.Parameters.AddWithValue("@session_id", sessionId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@session_id", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(sessionResponse))
                {
                    cmd.Parameters.AddWithValue("@session_response", sessionResponse);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@session_response", DBNull.Value);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.ExecuteNonQuery();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["rdurl"] != DBNull.Value)
                        {
                            SurveyURL = Convert.ToString(dr["rdurl"]);
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
            return SurveyURL;
        }
        #endregion
    }
}
