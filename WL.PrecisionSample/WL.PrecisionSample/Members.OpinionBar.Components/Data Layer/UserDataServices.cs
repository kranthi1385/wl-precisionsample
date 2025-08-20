using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Members.OpinionBar.Components.Data_Layer
{

    public class UserDataServices
    {
        #region Connection String
        public string ConnectionStringEmail
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringEmail"].ToString();
            }
        }
        public string ConnectionString1
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
        public string ConnectionString3
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            }
        }
        #endregion

        #region Get Countrybased on Ip Address

        /// <summary>
        /// Get Country Code from ipaddress
        /// </summary>
        /// <param name="ipaddress">ipaddress</param>
        /// <returns></returns>
        public string GetCountryForIp(string ipaddress)
        {
            string countrycode = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionStringIP;

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[dbo].[Ip2Country_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@ip_addr", ipaddress);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["country_code"] != DBNull.Value)
                    {
                        countrycode = oreader["country_code"].ToString();
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
            return countrycode;
        }
        #endregion

        //#region Forget Password
        ///// <summary>
        ///// Forget Password
        ///// </summary>
        ///// <param name="mailType"></param>
        ///// <param name="ouser"></param>
        ///// <param name="customAttribute"></param>
        //public void ForgotPwd(User oUser)
        //{
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = ConnectionStringEmail;
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("[transactional].[campaign.campaignitem_insert]", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("email_address", oUser.EmailAddress);
        //        //cmd.Parameters.AddWithValue("org_id", ouser.OrgId);
        //        //cmd.Parameters.AddWithValue("campaign_id", campaignId);
        //        //cmd.Parameters.AddWithValue("custom_attibute", CustomAttribute);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        //#endregion

        #region getladingpage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referrer_id"></param>
        /// <returns></returns>
        public string GetLandingpageUrl(int referrer_id)
        {
            string url = string.Empty;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            conn.Open();
            try
            {
                SqlCommand comm = new SqlCommand("[referrer].[referrerlandingpage_get]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@referrer_id", referrer_id);
                comm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["landing_url"] != DBNull.Value)
                        {
                            url = Convert.ToString(reader["landing_url"]);
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                conn.Close();
            }
            return url;
        }
        #endregion

        #region Get Client Details By Rid
        /// <summary>
        /// Get Client Details By Rid
        /// </summary>
        /// <param name="RId">rid</param>
        /// <returns></returns>
        public Client GetClientDetailsByRid(string MemberUrl, int? RId = null, int? ClientId = null)
        {
            Client oClient = new Client();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString1;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[client_details_get]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("referrer_id", RId);
                cmd.Parameters.AddWithValue("client_id", ClientId);
                if (!string.IsNullOrEmpty(MemberUrl))
                {
                    cmd.Parameters.AddWithValue("domain_url", MemberUrl);
                }
                else
                {
                    cmd.Parameters.AddWithValue("domain_url", DBNull.Value);
                }

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["org_id"] != DBNull.Value)
                        {
                            oClient.ClientId = Convert.ToInt32(dr["org_id"]);
                        }
                        if (dr["org_logo"] != DBNull.Value)
                        {
                            oClient.OrgLogo = Convert.ToString(dr["org_logo"]);
                        }
                        if (dr["org_name"] != DBNull.Value)
                        {
                            oClient.OrgName = Convert.ToString(dr["org_name"]);
                        }
                        if (dr["member_url"] != DBNull.Value)
                        {
                            oClient.MemberUrl = Convert.ToString(dr["member_url"]);
                        }
                        if (dr["referrer_id"] != DBNull.Value)
                        {
                            oClient.Referrerid = Convert.ToInt32(dr["referrer_id"]);
                        }
                        if (dr["org_type_id"] != DBNull.Value)
                        {
                            oClient.OrgTypeId = Convert.ToInt32(dr["org_type_id"]);
                        }
                        if (dr["member_url"] != DBNull.Value)
                        {
                            oClient.MemberUrl = dr["member_url"].ToString();
                        }
                        if (dr["emailaddress"] != DBNull.Value)
                        {
                            oClient.Emailaddress = dr["emailaddress"].ToString();
                        }
                        if (dr["mg_login_path"] != DBNull.Value)
                        {
                            oClient.MgLoginPath = dr["mg_login_path"].ToString();
                        }
                        if (dr["password"] != DBNull.Value)
                        {
                            oClient.Password = dr["password"].ToString();
                        }
                        if (dr["copyright_year"] != DBNull.Value)
                        {
                            oClient.CopyrightYear = Convert.ToInt32(dr["copyright_year"]);
                        }
                        if (dr["postal_address"] != DBNull.Value)
                        {
                            oClient.Address = dr["postal_address"].ToString();
                        }
                        if (dr["aboutus_text"] != DBNull.Value)
                        {
                            oClient.AboutusText = dr["aboutus_text"].ToString();
                        }
                        if (dr["theem"] != DBNull.Value)
                        {
                            oClient.StyleSheettheme = dr["theem"].ToString();
                        }
                        if (dr["home_page_url"] != DBNull.Value)
                        {
                            oClient.HomePageURL = dr["home_page_url"].ToString();
                        }
                        if (dr["is_popup"] != DBNull.Value)
                        {
                            oClient.IsPopUp = Convert.ToBoolean(dr["is_popup"].ToString());
                        }
                        if (dr["is_profile_pixel"] != DBNull.Value)
                        {
                            oClient.IsProfilePixel = Convert.ToBoolean(dr["is_profile_pixel"].ToString());
                        }
                        if (dr["is_survey_pixel"] != DBNull.Value)
                        {
                            oClient.IsSurveyPixel = Convert.ToBoolean(dr["is_survey_pixel"].ToString());
                        }
                        if (dr["profile_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileClickPixelUrl = dr["profile_click_pixel_url"].ToString();
                        }
                        if (dr["survey_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyClickPixelUrl = dr["survey_click_pixel_url"].ToString();
                        }
                        if (dr["profile_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileCompletePixelUrl = dr["profile_complete_pixel_url"].ToString();
                        }
                        if (dr["survey_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyCompletePixelUrl = dr["survey_complete_pixel_url"].ToString();
                        }
                        if (dr["is_verity_required"] != DBNull.Value)
                        {
                            oClient.VerityRequired = Convert.ToBoolean(dr["is_verity_required"].ToString());
                        }
                        if (dr["is_relevantid_required"] != DBNull.Value)
                        {
                            oClient.RelevantIdRequired = Convert.ToBoolean(dr["is_relevantid_required"].ToString());
                        }
                        if (dr["IsStep1Enable"] != DBNull.Value)
                        {
                            oClient.IsStep1Enable = Convert.ToBoolean(dr["IsStep1Enable"]);
                        }
                        if (dr["is_standalone_partner"] != DBNull.Value)
                        {
                            oClient.IsStandalone = Convert.ToBoolean(dr["is_standalone_partner"]);
                        }
                        if (dr["is_show_rewards"] != DBNull.Value)
                        {
                            oClient.IsRewardsShow = Convert.ToBoolean(dr["is_show_rewards"]);
                        }
                        if (dr["partner_survey_term_url"] != DBNull.Value)
                        {
                            oClient.PartnerTerminateUrl = Convert.ToString(dr["partner_survey_term_url"]);
                        }
                        if (dr["is_top10_enable"] != DBNull.Value)
                        {
                            oClient.IsTop10Enable = Convert.ToBoolean(dr["is_top10_enable"]);
                        }
                        if (dr["is_email_invitation"] != DBNull.Value)
                        {
                            oClient.IsEmailInvitationEnable = Convert.ToBoolean(dr["is_email_invitation"]);
                        }
                        if (dr["is_sms_invitation"] != DBNull.Value)
                        {
                            oClient.IsSmsInvitation = Convert.ToBoolean(dr["is_sms_invitation"]);
                        }
                        if (dr["mg_step2_path"] != DBNull.Value)
                        {
                            oClient.MgStep2Path = Convert.ToString(dr["mg_step2_path"]);
                        }
                        if (dr["reward_text_type"] != DBNull.Value)
                        {
                            oClient.RewardTextType = Convert.ToString(dr["reward_text_type"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                con.Close();
            }
            return oClient;
        }
        #endregion

        #region Email Address Check
        /// <summary>
        /// Email Address Check
        /// </summary>
        /// <param name="EmailAddress">emailaddress</param>
        /// <param name="ClientId">clientid</param>
        /// <returns></returns>
        public User EmailAddressCheck(string EmailAddress, int ClientId)
        {
            User ouser = new User();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[emailaddress_check]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["count"] != DBNull.Value)
                        {
                            ouser.CpaCount = Convert.ToInt32(dr["count"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return ouser;
        }
        #endregion

        #region UserLogin Check

        /// <summary>
        /// UserLogin Check
        /// </summary>
        /// <param name="MailId">EmailAddress</param>
        /// <param name="Passwrod">Password</param>
        /// <returns></returns>
        public User LoginCheck(string MailId, string Passwrod, string host, int ClientId)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(host);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            //SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = GetConnectionString(host);
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_login]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@email_address", MailId);
                cm.Parameters.AddWithValue("@password", Passwrod);
                cm.Parameters.AddWithValue("@org_id", ClientId);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if (reader["pwd_expired"] != DBNull.Value)
                        {
                            objUser.pwdExpired = Convert.ToBoolean(reader["pwd_expired"]);
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
            return objUser;
        }

        #endregion

        #region Get UserData
        /// <summary>
        /// Get User Data by userGuid
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ClientId">ClientiId</param>
        /// <returns></returns>
        public User GetUserDataEmail(string EmailAddress, int ClientId, string ip, string IpNumber)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_details_by_emailaddress]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
                cm.Parameters.AddWithValue("ip_address", ip);
                cm.Parameters.AddWithValue("ip_number", IpNumber);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["first_name"] != DBNull.Value)
                        {
                            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            objUser.UserId = Convert.ToInt32(reader["user_id"]);
                        }
                        if (reader["password"] != DBNull.Value)
                        {
                            objUser.Password = Convert.ToString(reader["password"]);
                        }
                        if (reader["email_address"] != DBNull.Value)
                        {
                            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        }
                        if (reader["last_name"] != DBNull.Value)
                        {
                            objUser.LastName = Convert.ToString(reader["last_name"]);
                        }
                        if (reader["ip_address"] != DBNull.Value)
                        {
                            objUser.IpAddress = Convert.ToString(reader["ip_address"]);
                        }
                        if (reader["create_dt"] != DBNull.Value)
                        {
                            objUser.CreateDate = Convert.ToString(reader["create_dt"]);
                        }
                        if (reader["org_id"] != DBNull.Value)
                        {
                            objUser.OrgId = Convert.ToInt32(reader["org_id"]);
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            objUser.CountryId = Convert.ToInt32(reader["country_id"]);
                        }
                        if (reader["user_language_id"] != DBNull.Value)
                        {
                            objUser.LanguageId = Convert.ToInt32(reader["user_language_id"]);
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
            return objUser;
        }

        #endregion

        #region Forget Password
        /// <summary>
        /// Forget Password
        /// </summary>
        /// <param name="mailType"></param>
        /// <param name="ouser"></param>
        /// <param name="customAttribute"></param>
        public void ForgetPassword(User ouser, int campaignId, int rId, string CustomAttribute)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionStringEmail;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[transactional].[campaign.campaignitem_insert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("email_address", ouser.EmailAddress);
                cmd.Parameters.AddWithValue("org_id", ouser.OrgId);
                cmd.Parameters.AddWithValue("campaign_id", campaignId);
                cmd.Parameters.AddWithValue("custom_attibute", CustomAttribute);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region Get UserData
        /// <summary>
        /// Get User Data by userGuid
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ClientId">ClientiId</param>
        /// <returns></returns>
        public User GetUserData(string UserGuid, int? Rid, int? ClientId)
        {
            User objUser = new User();
            //SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = ConnectionString;
            string constr = string.Empty;
            UserDataServices objDataServer = new UserDataServices();
            if (Rid != null)
            {
                constr = objDataServer.GetConnectionString(null, Rid, null);
            }
            if (ClientId != null)
            {
                constr = objDataServer.GetConnectionString(null, null, ClientId);
            }
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@user_name", UserGuid);
                cm.Parameters.AddWithValue("@org_id", ClientId);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["first_name"] != DBNull.Value)
                        {
                            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            objUser.UserId = Convert.ToInt32(reader["user_id"]);
                        }
                        //if (reader["password"] != DBNull.Value)
                        //{
                        //    objUser.Password = Convert.ToString(reader["password"]);
                        //}
                        if (reader["first_name"] != DBNull.Value)
                        {
                            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        }
                        if (reader["last_name"] != DBNull.Value)
                        {
                            objUser.LastName = Convert.ToString(reader["last_name"]);
                        }
                        if (reader["email_address"] != DBNull.Value)
                        {
                            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        }
                        if (reader["age"] != DBNull.Value)
                        {
                            objUser.Age = Convert.ToInt32(reader["age"]);
                        }
                        if (reader["zip_code"] != DBNull.Value)
                        {
                            objUser.ZipCode = Convert.ToString(reader["zip_code"]);
                        }
                        if (reader["gender"] != DBNull.Value)
                        {
                            objUser.Gender = Convert.ToString(reader["gender"]);
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            objUser.DateOfBirth = Convert.ToDateTime(reader["dob"]);
                            string date = objUser.DateOfBirth.ToString("MM/dd/yyyy");
                            string[] str = date.Split('/');
                            if (str.Length >= 2)
                            {
                                objUser.Day = str[1];
                            }
                            if (str.Length >= 3)
                            {
                                objUser.Year = str[2];
                            }
                            objUser.Month = str[0];
                        }                     
                        if (reader["phone_number"] != DBNull.Value)
                        {
                            objUser.PhoneNumber = Convert.ToString(reader["phone_number"]);
                        }
                        if (reader["address1"] != DBNull.Value)
                        {
                            objUser.Address1 = Convert.ToString(reader["address1"]);
                        }
                        if (reader["address2"] != DBNull.Value)
                        {
                            objUser.Address2 = Convert.ToString(reader["address2"]);
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            objUser.CountryId = Convert.ToInt32(reader["country_id"]);
                        }
                        if (reader["state_id"] != DBNull.Value)
                        {
                            objUser.StateId = Convert.ToInt32(reader["state_id"]);
                        }
                        if (reader["state_code"] != DBNull.Value)
                        {
                            objUser.StateCode = Convert.ToString(reader["state_code"]);
                        }
                        if (reader["country_code"] != DBNull.Value)
                        {
                            objUser.Country_id = Convert.ToString(reader["country_code"]);
                        }
                        if (reader["city"] != DBNull.Value)
                        {
                            objUser.City = Convert.ToString(reader["city"]);
                        }
                        if (reader["registration_step"] != DBNull.Value)
                        {
                            objUser.RegistrationStep = Convert.ToString(reader["registration_step"]);
                        }
                        //if (reader["registration_date"] != DBNull.Value)
                        //{
                        //    objUser.RegistrationDate = Convert.ToString(reader["registration_date"]);
                        //}
                        if (reader["ethnicity_id"] != DBNull.Value)
                        {
                            objUser.EthnicityId = Convert.ToInt32(reader["ethnicity_id"]);
                        }
                        if (reader["state_name"] != DBNull.Value)
                        {
                            objUser.StateName = Convert.ToString(reader["state_name"]);
                        }
                        if (reader["ip_address"] != DBNull.Value)
                        {
                            objUser.IpAddress = Convert.ToString(reader["ip_address"]);
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            objUser.SubId2 = Convert.ToString(reader["sub_id2"]);
                        }
                        if (reader["referrer_id"] != DBNull.Value)
                        {
                            objUser.RefferId = Convert.ToInt32(reader["referrer_id"]);
                        }
                        if (reader["split_flag"] != DBNull.Value)
                        {
                            objUser.SplitFlag = Convert.ToInt32(reader["split_flag"]);
                        }
                        if (reader["facebook_id"] != DBNull.Value)
                        {
                            objUser.FacebookId = Convert.ToString(reader["facebook_id"]);
                        }
                        if (reader["fb_access_token"] != DBNull.Value)
                        {
                            objUser.AccessToken = Convert.ToString(reader["fb_access_token"]);
                        }
                        //if (reader["fb_preference"] != DBNull.Value)
                        //{
                        //    objUser.FbPreference = Convert.ToInt32(reader["fb_preference"]);
                        //}
                        if (reader["survey_status"] != DBNull.Value)
                        {
                            objUser.SurveyStatus = Convert.ToString(reader["survey_status"]);
                        }
                        if (reader["external_member_info"] != DBNull.Value)
                        {
                            objUser.ExtMemberInfo = Convert.ToString(reader["external_member_info"]);
                        }
                        if (reader["hit_id"] != DBNull.Value)
                        {
                            objUser.HitId = Convert.ToInt32(reader["hit_id"]);
                        }
                        //if (reader["is_doi"] != DBNull.Value)
                        //{
                        //    objUser.IsDoi = Convert.ToBoolean(reader["is_doi"]);
                        //}
                        //if (reader["opt_tiburon_split_flag"] != DBNull.Value)
                        //{
                        //    objUser.OptTiburonSplitFlag = Convert.ToInt32(reader["opt_tiburon_split_flag"]);
                        //}
                        if (reader["is_fraud"] != DBNull.Value)
                        {
                            objUser.IsFraud = Convert.ToBoolean(reader["is_fraud"]);
                        }
                        if (reader["country2ip"] != DBNull.Value)
                        {
                            objUser.Country2Ip = Convert.ToString(reader["country2ip"]);
                        }
                        if (reader["is_doi_required"] != DBNull.Value)
                        {
                            objUser.IsDoiRequired = Convert.ToInt32(reader["is_doi_required"]);
                        }
                        if (reader["country_name"] != DBNull.Value)
                        {
                            objUser.CountryName = Convert.ToString(reader["country_name"]);
                        }
                        if (reader["is_doi_pixel_fired"] != DBNull.Value)
                        {
                            objUser.Is_doi_pixel_fired = Convert.ToInt32(reader["is_doi_pixel_fired"]);
                        }
                        if (reader["is_soi_pixel_fired"] != DBNull.Value)
                        {
                            objUser.Is_soi_pixel_fired = Convert.ToInt32(reader["is_soi_pixel_fired"]);
                        }
                        if (reader["survey_clicks_count"] != DBNull.Value)
                        {
                            objUser.SurveyClicksCount = Convert.ToInt32(reader["survey_clicks_count"]);
                        }
                        //Added on 6/15/2013 to display DMA ID
                        if (reader["dma_id"] != DBNull.Value)
                        {
                            objUser.DmaId = Convert.ToInt32(reader["dma_id"]);
                        }

                        if (reader["org_id"] != DBNull.Value)
                        {
                            objUser.OrgId = Convert.ToInt32(reader["org_id"]);
                        }
                        if (reader["sub_id3"] != DBNull.Value)
                        {
                            objUser.SubId3 = reader["sub_id3"].ToString();
                        }
                        //Added on 03/26/2014 for relevant & verity Implementation
                        if (reader["relevant_score"] != DBNull.Value)
                        {
                            objUser.RelevantScore = Convert.ToInt32(reader["relevant_score"]);
                        }
                        if (reader["relevant_profile_score"] != DBNull.Value)
                        {
                            objUser.RelevantProfileScore = Convert.ToInt32(reader["relevant_profile_score"]);
                        }
                        if (reader["relevant_id"] != DBNull.Value)
                        {
                            objUser.RelevantId = reader["relevant_id"].ToString();
                        }
                        if (reader["verity_id"] != DBNull.Value)
                        {
                            objUser.VerityId = reader["verity_id"].ToString();
                        }
                        if (reader["verity_score"] != DBNull.Value)
                        {
                            objUser.VerityScore = Convert.ToInt32(reader["verity_score"]);
                        }
                        if (reader["verity_geo_flag"] != DBNull.Value)
                        {
                            objUser.GeoCorrelationFlag = Convert.ToInt32(reader["verity_geo_flag"]);
                        }
                        if (reader["is_verity_required"] != DBNull.Value)
                        {
                            objUser.Is_verity_required = Convert.ToBoolean(reader["is_verity_required"]);
                        }
                        if (reader["is_relevantid_required"] != DBNull.Value)
                        {
                            objUser.Is_relevantid_required = Convert.ToBoolean(reader["is_relevantid_required"]);
                        }

                        if (reader["partner_rev_share"] != DBNull.Value)
                        {
                            objUser.PartnerRevshare = Convert.ToDecimal(reader["partner_rev_share"]);
                        }
                        if (reader["ethnicity_type"] != DBNull.Value)
                        {
                            objUser.EthinicityType = Convert.ToString(reader["ethnicity_type"]);
                        }

                        if (reader["challenge_id"] != DBNull.Value)
                        {
                            objUser.ChallengeId = Convert.ToString(reader["challenge_id"]);
                        }
                        if (reader["is_mobileno_from_ps"] != DBNull.Value)
                        {
                            objUser.IsMobileNoFromPs = Convert.ToInt32(reader["is_mobileno_from_ps"]);
                        }
                        //Added by Sandy on 06/29/2016 
                        if (reader["isrequire_relevantid_validated_members"] != DBNull.Value)
                        {
                            objUser.Is_relevantid_required_for_project = Convert.ToBoolean(reader["isrequire_relevantid_validated_members"]);
                        }
                        if (reader["user_language_id"] != DBNull.Value)
                        {
                            objUser.LanguageId = Convert.ToInt32(reader["user_language_id"]);
                        }
                        if (reader["language_iso_chars"] != DBNull.Value)
                        {
                            objUser.LanguageCode = Convert.ToString(reader["language_iso_chars"]);
                        }
                        if (reader["language_name"] != DBNull.Value)
                        {
                            objUser.LanguageName = Convert.ToString(reader["language_name"]);
                        }
                        if (reader["is_dnc"] != DBNull.Value)
                        {
                            objUser.IsDnc = Convert.ToBoolean(reader["is_dnc"]);
                        }
                        if (reader["is_ccpa_compliance"] != DBNull.Value)
                        {
                            objUser.IsCompliance = Convert.ToBoolean(reader["is_ccpa_compliance"]);
                        }
                        //if (reader["reward_id"] != DBNull.Value)
                        //{
                        //    objUser.ChooseYourReward = Convert.ToInt32(reader["reward_id"]);
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
            return objUser;
        }

        #endregion

        #region DeleteUserData
        public void DeleteUserDataEmail(string UserGuid, int Rid, string SubId3, string Reason, int campaign_id, string custom_attr)
        {
            User oUser = new User();
            string constring = string.Empty;
            UserDataServices oDataServer = new UserDataServices();
            constring = oDataServer.GetConnectionString(null, Rid, null);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString(); ;
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("[user].[user_delete_insert]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@referrer_id", Rid);
                cm.Parameters.AddWithValue("@ext_member_id", SubId3);
                cm.Parameters.AddWithValue("@reason", Reason);
                cm.Parameters.AddWithValue("@campaign_id", campaign_id);
                cm.Parameters.AddWithValue("@custom_attribute", custom_attr);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region DeleteUserData
        public int DeleteUserData(string UserGuid, int Rid, string SubId3)
        {
            int result = 0;
            User oUser = new User();
            string constring = string.Empty;
            UserDataServices oDataServer = new UserDataServices();
            constring = oDataServer.GetConnectionString(null, Rid, null);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString(); ;
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("[user].[user_delete_confirmation]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
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
                con.Close();
            }
            return result;
        }
        #endregion

        #region Get Verity Information
        /// <summary>
        /// Get User Verity Information
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public string GetVerityInformation(string UserGuid, int CId)
        {
            User ouser = new User();
            string verityInformation = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, CId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_verityapi_call_info_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["verity_info"] != DBNull.Value)
                        {
                            verityInformation = Convert.ToString(dr["verity_info"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return verityInformation;
        }
        #endregion

        #region Update Relevant Score For SDL And Members
        /// <summary>
        /// ReleventUpdate
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="PfScore"></param>
        /// <param name="FpfScore"></param>
        /// <param name="RelevantId"></param>
        /// <returns></returns>
        public int ReleventUpdateForSDLOrWL(Guid UserGuid, int RelevantScore, int PfScore, string FpfScore, string RelevantId, int ClientId, string VerityId, int VerityScore, int GeoCorrelationFlag, bool VerityDOBFail, string ipqsJson)
        {
            int Count = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[update_relevant_score]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@pf_score", PfScore);
                cm.Parameters.AddWithValue("@fpf_score", FpfScore);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@verity_id", VerityId);
                cm.Parameters.AddWithValue("@verity_score", VerityScore);
                cm.Parameters.AddWithValue("@geo_corelation_flag", GeoCorrelationFlag);
                cm.Parameters.AddWithValue("@verity_dob_fail", VerityDOBFail);
                cm.Parameters.AddWithValue("@ipqs_json", ipqsJson);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["TotalCount"] != DBNull.Value)
                        {
                            Count = Convert.ToInt32(reader["TotalCount"]);
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
            return Count;
        }
        #endregion

        #region GetConnectionString By Host Or Rid Or ClientId
        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetConnectionString(string host, int? Rid = null, int? ClientId = null)
        {
            string connection = null;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@domain_url", host);
                cm.Parameters.AddWithValue("@referrer_id", Rid);
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
                var output = connection.Split(new[] { ';', ' ' });
                Console.WriteLine(output);
                connection = output[0];
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

        #region User RegistrationStep -Update
        /// <summary>
        /// User registration step update
        /// </summary>
        /// <param name="oUser">ouser</param>
        public void UserRegistrationStepUpdate(User oUser, int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("[user].[UserRegistrationStep_Update]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                #region Adding params for the SQL Proc (IF NOT NULL)
                cmd.Parameters.AddWithValue("user_id", oUser.UserId);
                cmd.Parameters.AddWithValue("registration_step", oUser.RegistrationStep);
                cmd.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                #endregion
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

        #region Update the SOI Pixel Fired status

        /// <summary>
        /// Update soi pixel fired status update
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="roleid">roleid</param>
        /// <param name="ClientId">ClientId</param>
        public void UpdateSOIPixelFiredStatusUpdate(int UserId, int ReferrerId, int ClientId)
        {
            string constring = string.Empty;
            UserDataServices oDataServer = new UserDataServices();
            constring = oDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString(); ;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[user_soi_pixelfiredstatus_update]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", UserId);
                cmd.Parameters.AddWithValue("@referrer_id", ReferrerId);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

        }

        #endregion

        #region Update Language Code
        /// <summary>
        /// Update Language Code
        /// </summary>
        /// <param name="LanguageText">Language Text</param>
        public int GetLangCode(User oUser, string RequestUrl)
        {
            int lang = 0;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(RequestUrl);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[api].[get_language]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", oUser.UserId);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["country_id"] != DBNull.Value)
                        {
                            lang = Convert.ToInt32(reader["country_id"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
            return lang;
        }
        #endregion

        #region downloadUser
        /// <summary>
        /// downloadUser

        public void downloadUser(string path, string UserId, int orgId)
        {
            string constr = string.Empty;
            SqlConnection cn = new SqlConnection();
            List<NameValuePair> lstCnStings = new List<NameValuePair>();
            UserDataServices objDataServer = new UserDataServices();
            constr = objDataServer.GetConnectionString(null, null, orgId);
            //string ConnectionString = "server = 128.136.7.59; database = surveydownline; uid = psdev; pwd = H6T4sultkDjL; Connect Timeout = 120; ";
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                DataTable dt = new DataTable();
                SqlCommand com = new SqlCommand("[user].[data_get_v1]", cn);
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 720;
                List<User> objUserList = new List<User>();
                com.Parameters.AddWithValue("@user_id", UserId);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    DataTable dts = new DataTable();
                    dts.Load(reader);
                    dt.Merge(dts);
                    dt.AcceptChanges();
                }
                //StreamWriter theWriter = new StreamWriter(ConfigurationManager.AppSettings["DownLoadCampaignMembers"].ToString() + "\\test.csv");

                StreamWriter theWriter = new StreamWriter(path + "\\UserDownload.xml");
                foreach (DataRow curRow in dt.Rows)
                {
                    foreach (object curObjectValue in curRow.ItemArray)
                    {
                        theWriter.Write(curObjectValue);
                    }
                    theWriter.WriteLine();
                }
                theWriter.Close();


            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region Save Do Not Sell My Info
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public void SaveDoNotSellMyInfo(string FirstName, string LastName, string EmailAddress, string PrecisionSampleSite, int RequestID, int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].ccpa_compliance_save", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@first_name", FirstName);
                cm.Parameters.AddWithValue("@last_name", LastName);
                cm.Parameters.AddWithValue("@email_address", EmailAddress);
                cm.Parameters.AddWithValue("@precision_sample_site", PrecisionSampleSite);
                cm.Parameters.AddWithValue("@request_id", RequestID);
                cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion


        #region Change Password 
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public int ChangePassword(string OldPassword, string NewPassword, string CnfNewPassword, string ug)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[expire_pwd_change]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@old_password", OldPassword);
                cm.Parameters.AddWithValue("@new_password", NewPassword);
                cm.Parameters.AddWithValue("@user_guid", ug);
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
                cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

        #region Reward validation check
        /// <summary>
        /// Reward validation check
        /// </summary>
        /// <returns></returns>
        public int GetRewardAccessPwd(string ug, string pwd)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();                
                string query = "SELECT COUNT(*) FROM [user].[user] WHERE [user_guid] = @user_guid AND [password] = @password COLLATE Latin1_General_CS_AS AND [is_deleted] = 0";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {                  
                    cm.Parameters.AddWithValue("@user_guid", ug);
                    cm.Parameters.AddWithValue("@password", pwd);                    
                    result = (int)cm.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }            
            return result > 0 ? 1 : 0;
        }
        #endregion

        #region Get UserData
        /// <summary>
        /// Get User Data by userGuid
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ClientId">ClientiId</param>
        /// <returns></returns>
        public LbUser GetExtMemDetails(string leadid)
        {
            LbUser objUser = new LbUser();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_details_by_lead_id_v1]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@leadid", leadid);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["first_name"] != DBNull.Value)
                        {
                            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        }
                        if (reader["email_address"] != DBNull.Value)
                        {
                            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        }
                        if (reader["last_name"] != DBNull.Value)
                        {
                            objUser.LastName = Convert.ToString(reader["last_name"]);
                        }
                        if (reader["zip"] != DBNull.Value)
                        {
                            objUser.ZipCode = Convert.ToString(reader["zip"]);
                        }
                        if (reader["gender"] != DBNull.Value)
                        {
                            objUser.Gender = Convert.ToString(reader["gender"]);
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            objUser.Dob = Convert.ToString(reader["dob"]);
                        }
                        if (reader["phone_no"] != DBNull.Value)
                        {
                            objUser.PhoneNumber = Convert.ToString(reader["phone_no"]);
                        }
                        if (reader["address1"] != DBNull.Value)
                        {
                            objUser.Address1 = Convert.ToString(reader["address1"]);
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            objUser.subid2 = Convert.ToString(reader["sub_id2"]);
                        }
                        if (reader["referrer_id"] != DBNull.Value)
                        {
                            objUser.referrerid = Convert.ToInt32(reader["referrer_id"]);
                        }
                        if (reader["sub_referrer_id"] != DBNull.Value)
                        {
                            objUser.subreferrerid = Convert.ToInt32(reader["sub_referrer_id"]);
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
            return objUser;
        }

        #endregion

        #region Email Address Check
        /// <summary>
        /// Email Address Check
        /// </summary>
        /// <param name="EmailAddress">emailaddress</param>
        /// <param name="ClientId">clientid</param>
        /// <returns></returns>
        public User SubidCheck(string EmailAddress, int ClientId, string subid)
        {
            User ouser = new User();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[emailaddress_subid3_check_wl]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
                cm.Parameters.AddWithValue("sub_id3 ", subid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["count"] != DBNull.Value)
                        {
                            ouser.CpaCount = Convert.ToInt32(dr["count"].ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return ouser;
        }
        #endregion

        #region Update IsVerified
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public int UpdateIsVerified(string UserGUID, int RedemptionID)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[update_reward_redemption_verified]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGUID);
                cm.Parameters.AddWithValue("@redemption_id", RedemptionID);
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
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

        #region pii confirmation
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public string piiconfirm(Guid UserGUID)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_email_confirmatin]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGUID);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            result = Convert.ToString(dr["result"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

        #region Get Pixel Script
        /// <summary>
        /// Get Pixel Script
        /// </summary>
        /// <param name="leadid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<pixel> GetScript(string leadid, int flag)
        {
            List<pixel> objPIxel = new List<pixel>();
            string script = string.Empty;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString3;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[router_url_tracking_pixel_get]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("user_guid", leadid);
                cmd.Parameters.AddWithValue("pixel_type_id", flag);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        pixel Pixel = new pixel();
                        if (dr["pixel_type"] != DBNull.Value)
                        {
                            Pixel.pixeltype = Convert.ToString(dr["pixel_type"]);
                        }
                        if (dr["src"] != DBNull.Value)
                        {
                            Pixel.src = Convert.ToString(dr["src"]);
                        }
                        if (dr["attribute_value"] != DBNull.Value)
                        {
                            Pixel.attributrValue = Convert.ToString(dr["attribute_value"]);
                        }
                        objPIxel.Add(Pixel);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
            return objPIxel;
        }
        #endregion

        #region Get Pixel Script
        /// <summary>
        /// Get Pixel Script
        /// </summary>
        /// <param name="leadid"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public pixel PostbackScript(string leadid, int flag)
        {
            pixel Pixel = new pixel();
            string script = string.Empty;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString3;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[router_url_callback_get]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("user_guid", leadid);
                cmd.Parameters.AddWithValue("pixel_type_id", flag);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    while (dr.Read())
                    {

                        if (dr["callback_url"] != DBNull.Value)
                        {
                            Pixel.PostbackURL = Convert.ToString(dr["callback_url"]);
                            //this will be used to fire Pixels.
                            if (Pixel.PostbackURL.Contains("<img"))
                            {
                                Pixel.postbacktext = Regex.Match(Pixel.PostbackURL, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            }
                            else if (Pixel.PostbackURL.Contains("<iframe"))
                            {
                                Pixel.postbacktext = Regex.Match(Pixel.PostbackURL, "<iframe.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
            return Pixel;
        }
        #endregion

        #region Referrer Clicks Insert

        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <param name="SubId1">SubId1</param>
        /// <param name="SubId2">SubId2</param>
        /// <param name="SubId3">SubId3</param>
        /// <param name="Ipaddress">IpAddress</param>
        /// <param name="Referrerurl">Referrerurl</param>
        /// <param name="PageNo">PageNo</param>
        /// <param name="CpatchaFlag">CpatchaFlag</param>
        /// <param name="CpatchaFlag">CpatchaFlag</param>
        /// <returns></returns>
        public int InserClicks(string SubId1, string SubId2, string SubId3, string Ipaddress, string Referrerurl, int PageNo, int CpatchaFlag, int OpttiburonsplitFlag)
        {
            int icount = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            // string constr = objDataServer.GetConnectionString(null, Convert.ToInt32(SubId1), null);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[click_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding Paramters
                if (SubId1 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@referrer_id", SubId1);
                }

                if (SubId2 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@sub_id2", SubId2);
                }
                else
                {
                    cm.Parameters.AddWithValue("@sub_id2", "");
                }

                if (SubId3 != string.Empty)
                {
                    cm.Parameters.AddWithValue("@sub_id3", SubId3);
                }
                else
                {
                    cm.Parameters.AddWithValue("@sub_id3", "");
                }

                if (Ipaddress != string.Empty)
                {
                    cm.Parameters.AddWithValue("@ip_address", Ipaddress);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ip_address", "");
                }

                if (Referrerurl != string.Empty)
                {
                    cm.Parameters.AddWithValue("referrer_url", Referrerurl);
                }
                else
                {
                    cm.Parameters.AddWithValue("referrer_url", "");
                }
                #endregion
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["click_id"] != DBNull.Value)
                        {
                            icount = Convert.ToInt32(reader["click_id"]);
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
            return icount;


        }

        #endregion

        #region Step1 User Insert
        /// <summary>
        ///  Step1 User Insert
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>
        public Guid RouteruserInsert(User oUser)
        {
            Guid guid = Guid.Empty;
            UserDataServices objDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString());

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[router_user_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                #region Adding params for the SQL Proc (IF NOT NULL)

                cm.Parameters.AddWithValue("@referrer_id", oUser.rfid);
                cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                cm.Parameters.AddWithValue("@ethnicity_id", oUser.EthnicityId);
                cm.Parameters.AddWithValue("@gender", oUser.Gender);
                cm.Parameters.AddWithValue("@zip_code", oUser.ZipCode);
                if (string.IsNullOrEmpty(oUser.Dob))
                {
                    cm.Parameters.AddWithValue("@dob", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@dob", Convert.ToDateTime(oUser.Dob));
                }
                cm.Parameters.AddWithValue("@ip_address", oUser.IpAddress);
                cm.Parameters.AddWithValue("@sub_id3", oUser.SubId3);
                cm.Parameters.AddWithValue("@sub_id2", oUser.SubId2);
                cm.Parameters.AddWithValue("@transaction_id", oUser.TransactionId);
                #endregion

                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            guid = new Guid(reader["user_guid"].ToString());
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
            return guid;
        }
        #endregion

        #region Referrer Clicks Insert

        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <returns></returns>
        public string InsertAffClicks(int rid, string SubID3, string IPAddress, string sid, string fid, string trans_id, string RefererUrl, int isClick)
        {
            string authkey = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            // string constr = objDataServer.GetConnectionString(null, Convert.ToInt32(SubId1), null);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[affiliate_click_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding Paramters
                cm.Parameters.AddWithValue("referrer_id", rid);
                cm.Parameters.AddWithValue("sub_id3", SubID3);
                cm.Parameters.AddWithValue("ip_address", IPAddress);
                cm.Parameters.AddWithValue("sub_id2", sid);
                cm.Parameters.AddWithValue("isclick", isClick);
                if (RefererUrl != string.Empty)
                {
                    cm.Parameters.AddWithValue("referrer_url", RefererUrl);
                }
                else
                {
                    cm.Parameters.AddWithValue("referrer_url", "");
                }
                cm.Parameters.AddWithValue("transaction_id", trans_id);

                #endregion
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["auth_key"] != DBNull.Value)
                        {
                            authkey = Convert.ToString(reader["auth_key"]);
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
            return authkey;


        }

        #endregion

        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop20(Guid UserGUID, string OrgGUID, int ispeerly2)
        {

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[router_top20_questions_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 420;
                cm.Parameters.AddWithValue("@user_guid", UserGUID);
                cm.Parameters.AddWithValue("@org_guid", OrgGUID);
                cm.Parameters.AddWithValue("@ispeerly2", ispeerly2);
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
            string message = "";
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[router_top20_question_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["message"] != null)
                    {
                        message = Convert.ToString(oreader["message"]);
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
            return message;

        }
        #endregion

        #region Get User Profile data
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public string surveygetbyprjid(string prjid, int rid, string IPAddress)
        {

            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionString3;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[router_user_insert_test]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@project_id", prjid);
                cm.Parameters.AddWithValue("@referrer_id", rid);
                cm.Parameters.AddWithValue("@ip_address", IPAddress);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["url"] != DBNull.Value)
                    {
                        result = Convert.ToString(oreader["url"]);
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
            return result;
        }
        #endregion

        #region Referrer Clicks Insert

        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <returns></returns>
        public string GetSurveyURL(int pid, int rid, string IPAddress, int userTrafficTypeId, string mobiledeviceModel, string browserInfo, string AgentInfo, string extid)
        {
            string survey_url = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[router_survey_url_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding Paramters
                cm.Parameters.AddWithValue("@project_id", pid);
                cm.Parameters.AddWithValue("referrer_id", rid);
                cm.Parameters.AddWithValue("ip_address", IPAddress);
                cm.Parameters.AddWithValue("@user_traffic_type_id", userTrafficTypeId);
                cm.Parameters.AddWithValue("@mobile_device_model", mobiledeviceModel);
                cm.Parameters.AddWithValue("@browser_info", browserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("sub_id3", extid);
                #endregion
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["survey_url"] != DBNull.Value)
                        {
                            survey_url = Convert.ToString(reader["survey_url"]);
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
            return survey_url;
        }
        #endregion

        #region Check member Exists
        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <returns></returns>
        public Boolean CheckMemberExist(string ug, int cid)
        {
            Boolean IsExists = false;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_by_org_exists]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding Paramters
                cm.Parameters.AddWithValue("@user_guid", ug);
                cm.Parameters.AddWithValue("@org_id", cid);
                #endregion
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["is_exists"] != DBNull.Value)
                        {
                            IsExists = Convert.ToBoolean(reader["is_exists"]);
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
            return IsExists;
        }
        #endregion
    }
}
