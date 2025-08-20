using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class UserDataServices
    {
        #region ConnectionString

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionStringActivity
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
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
        public string ConnectionStringEmail
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringEmail"].ToString();
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
                        if (reader["password"] != DBNull.Value)
                        {
                            objUser.Password = Convert.ToString(reader["password"]);
                        }
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
                        if (reader["is_dnc"] != DBNull.Value)
                        {
                            objUser.IsDnc = Convert.ToBoolean(reader["is_dnc"]);
                        }
                        if (reader["language_iso_chars"] != DBNull.Value)
                        {
                            objUser.LanguageCode = Convert.ToString(reader["language_iso_chars"]);
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

        #region Unsubscribe User Dnc
        /// <summary>
        /// Unsubscribe User Dnc
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="ClientId">clientid</param>
        public void UnsubUserDnc(string userId, int ClientId)
        {
            string constring = string.Empty;
            UserDataServices oDataServer = new UserDataServices();
            constring = oDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString(); ;
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("[user].[user_dnc_insert]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", userId);
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

        #region User Email Dnc Insert
        /// <summary>
        /// Unsubscribe User Dnc
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="ClientId">clientid</param>
        public void UserEmailDncInsert(string EmailAddress, int ClientId, int Referrerid)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionStringEmail;
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("[user].[user_dnc_email]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
                cm.Parameters.AddWithValue("referrer_id", Referrerid);
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

        #region
        public void DeleteUserData(string UserGuid, int Rid, string SubId3)
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
                SqlCommand cm = new SqlCommand("[api].[user_delete]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@ext_member_id", SubId3);
                cm.Parameters.AddWithValue("@referrer_id", Rid);
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
        #region Get survey Url By invitation Guid
        /// <summary>
        /// GetSurvey Url By InvitationGuid
        /// </summary>
        /// <param name="userInvitationGuid"></param>
        /// <returns></returns>
        public string GetSurveyUrlByInvitationGuid(string userInvitationGuid)
        {
            UserDataServices objDataServer = new UserDataServices();
            string Result = string.Empty;
            SqlConnection con = new SqlConnection();
            string constr = objDataServer.GetConnectionString1(userInvitationGuid);
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                con.Open();
                SqlCommand cm = new SqlCommand("[pms].[SurveyURL_by_UserInvitationGUID_Get]", con);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("user_invitation_guid", userInvitationGuid);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["active_project_url"] != DBNull.Value)
                        {

                            Result = reader["active_project_url"].ToString();
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
            return Result;
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
                //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                //{
                //    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                //}
                //else
                //{
                cm.Parameters.AddWithValue("@org_id", ClientId);
                //}
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        //        if (reader["user_id"] != DBNull.Value)
                        //        {
                        //            objUser.UserId = Convert.ToInt32(reader["user_id"]);
                        //        }
                        //        //if (reader["password"] != DBNull.Value)
                        //        //{
                        //        //    objUser.Password = Convert.ToString(reader["password"]);
                        //        //}
                        //        if (reader["first_name"] != DBNull.Value)
                        //        {
                        //            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        //        }
                        //        if (reader["last_name"] != DBNull.Value)
                        //        {
                        //            objUser.LastName = Convert.ToString(reader["last_name"]);
                        //        }
                        //        if (reader["email_address"] != DBNull.Value)
                        //        {
                        //            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        //        }

                        //        if (reader["zip_code"] != DBNull.Value)
                        //        {
                        //            objUser.ZipCode = Convert.ToString(reader["zip_code"]);
                        //        }
                        //        if (reader["gender"] != DBNull.Value)
                        //        {
                        //            objUser.Gender = Convert.ToString(reader["gender"]);
                        //        }
                        //        if (reader["dob"] != DBNull.Value)
                        //        {
                        //            objUser.DOB = Convert.ToDateTime(reader["dob"]);
                        //        }
                        //        if (reader["phone_number"] != DBNull.Value)
                        //        {
                        //            objUser.PhoneNumber = Convert.ToString(reader["phone_number"]);
                        //        }
                        //        if (reader["address1"] != DBNull.Value)
                        //        {
                        //            objUser.Address1 = Convert.ToString(reader["address1"]);
                        //        }
                        //        if (reader["address2"] != DBNull.Value)
                        //        {
                        //            objUser.Address2 = Convert.ToString(reader["address2"]);
                        //        }
                        //        if (reader["country_id"] != DBNull.Value)
                        //        {
                        //            objUser.CountryId = Convert.ToInt32(reader["country_id"]);
                        //        }
                        //        if (reader["state_id"] != DBNull.Value)
                        //        {
                        //            objUser.StateId = Convert.ToInt32(reader["state_id"]);
                        //        }
                        //        if (reader["city"] != DBNull.Value)
                        //        {
                        //            objUser.City = Convert.ToString(reader["city"]);
                        //        }
                        //        if (reader["registration_step"] != DBNull.Value)
                        //        {
                        //            objUser.RegistrationStep = Convert.ToString(reader["registration_step"]);
                        //        }
                        //        if (reader["registration_date"] != DBNull.Value)
                        //        {
                        //            objUser.RegistrationDate = Convert.ToString(reader["registration_date"]);
                        //        }
                        //        if (reader["ethnicity_id"] != DBNull.Value)
                        //        {
                        //            objUser.EthnicityId = Convert.ToInt32(reader["ethnicity_id"]);
                        //        }
                        //        if (reader["state_name"] != DBNull.Value)
                        //        {
                        //            objUser.StateName = Convert.ToString(reader["state_name"]);
                        //        }
                        //        if (reader["ip_address"] != DBNull.Value)
                        //        {
                        //            objUser.IpAddress = Convert.ToString(reader["ip_address"]);
                        //        }
                        //        if (reader["referrer_id"] != DBNull.Value)
                        //        {
                        //            objUser.RefferId = Convert.ToInt32(reader["referrer_id"]);
                        //        }
                        //        if (reader["is_verity_required"] != DBNull.Value)
                        //        {
                        //            objUser.Is_verity_required = Convert.ToBoolean(reader["is_verity_required"]);
                        //        }
                        //        if (reader["is_relevantid_required"] != DBNull.Value)
                        //        {
                        //            objUser.Is_relevantid_required = Convert.ToBoolean(reader["is_relevantid_required"]);
                        //        }
                        //        //added on 11/18/2014
                        //        if (reader["verity_score"] != DBNull.Value)
                        //        {
                        //            objUser.VerityScore = Convert.ToInt32(reader["verity_score"]);
                        //        }
                        //        if (reader["is_fraud"] != DBNull.Value)
                        //        {
                        //            objUser.IsFraud = Convert.ToBoolean(reader["is_fraud"]);
                        //        }

                        //        if (reader["verity_geo_flag"] != DBNull.Value)
                        //        {
                        //            objUser.GeoCorrelationFlag = Convert.ToInt32(reader["verity_geo_flag"]);
                        //        }
                        //        if (reader["relevant_score"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantScore = Convert.ToInt32(reader["relevant_score"]);
                        //        }
                        //        if (reader["relevant_profile_score"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantProfileScore = Convert.ToInt32(reader["relevant_profile_score"]);
                        //        }
                        //        if (reader["relevant_id"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantId = Convert.ToString(reader["relevant_id"]);
                        //        }

                        //        if (reader["verity_id"] != DBNull.Value)
                        //        {
                        //            objUser.VerityId = Convert.ToString(reader["verity_id"]);
                        //        }
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

        #region UserLogin Check

        /// <summary>
        /// UserLogin Check
        /// </summary>
        /// <param name="MailId">EmailAddress</param>
        /// <param name="Passwrod">Password</param>
        /// <returns></returns>
        public User WidgetLoginCheck(string MailId, string Passwrod, string host, int ClientId)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(host, 0, ClientId);
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
                //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                //{
                //    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                //}
                //else
                //{
                cm.Parameters.AddWithValue("@org_id", ClientId);
                //}
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        //        if (reader["user_id"] != DBNull.Value)
                        //        {
                        //            objUser.UserId = Convert.ToInt32(reader["user_id"]);
                        //        }
                        //        //if (reader["password"] != DBNull.Value)
                        //        //{
                        //        //    objUser.Password = Convert.ToString(reader["password"]);
                        //        //}
                        //        if (reader["first_name"] != DBNull.Value)
                        //        {
                        //            objUser.FirstName = Convert.ToString(reader["first_name"]);
                        //        }
                        //        if (reader["last_name"] != DBNull.Value)
                        //        {
                        //            objUser.LastName = Convert.ToString(reader["last_name"]);
                        //        }
                        //        if (reader["email_address"] != DBNull.Value)
                        //        {
                        //            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        //        }

                        //        if (reader["zip_code"] != DBNull.Value)
                        //        {
                        //            objUser.ZipCode = Convert.ToString(reader["zip_code"]);
                        //        }
                        //        if (reader["gender"] != DBNull.Value)
                        //        {
                        //            objUser.Gender = Convert.ToString(reader["gender"]);
                        //        }
                        //        if (reader["dob"] != DBNull.Value)
                        //        {
                        //            objUser.DOB = Convert.ToDateTime(reader["dob"]);
                        //        }
                        //        if (reader["phone_number"] != DBNull.Value)
                        //        {
                        //            objUser.PhoneNumber = Convert.ToString(reader["phone_number"]);
                        //        }
                        //        if (reader["address1"] != DBNull.Value)
                        //        {
                        //            objUser.Address1 = Convert.ToString(reader["address1"]);
                        //        }
                        //        if (reader["address2"] != DBNull.Value)
                        //        {
                        //            objUser.Address2 = Convert.ToString(reader["address2"]);
                        //        }
                        //        if (reader["country_id"] != DBNull.Value)
                        //        {
                        //            objUser.CountryId = Convert.ToInt32(reader["country_id"]);
                        //        }
                        //        if (reader["state_id"] != DBNull.Value)
                        //        {
                        //            objUser.StateId = Convert.ToInt32(reader["state_id"]);
                        //        }
                        //        if (reader["city"] != DBNull.Value)
                        //        {
                        //            objUser.City = Convert.ToString(reader["city"]);
                        //        }
                        //        if (reader["registration_step"] != DBNull.Value)
                        //        {
                        //            objUser.RegistrationStep = Convert.ToString(reader["registration_step"]);
                        //        }
                        //        if (reader["registration_date"] != DBNull.Value)
                        //        {
                        //            objUser.RegistrationDate = Convert.ToString(reader["registration_date"]);
                        //        }
                        //        if (reader["ethnicity_id"] != DBNull.Value)
                        //        {
                        //            objUser.EthnicityId = Convert.ToInt32(reader["ethnicity_id"]);
                        //        }
                        //        if (reader["state_name"] != DBNull.Value)
                        //        {
                        //            objUser.StateName = Convert.ToString(reader["state_name"]);
                        //        }
                        //        if (reader["ip_address"] != DBNull.Value)
                        //        {
                        //            objUser.IpAddress = Convert.ToString(reader["ip_address"]);
                        //        }
                        //        if (reader["referrer_id"] != DBNull.Value)
                        //        {
                        //            objUser.RefferId = Convert.ToInt32(reader["referrer_id"]);
                        //        }
                        //        if (reader["is_verity_required"] != DBNull.Value)
                        //        {
                        //            objUser.Is_verity_required = Convert.ToBoolean(reader["is_verity_required"]);
                        //        }
                        //        if (reader["is_relevantid_required"] != DBNull.Value)
                        //        {
                        //            objUser.Is_relevantid_required = Convert.ToBoolean(reader["is_relevantid_required"]);
                        //        }
                        //        //added on 11/18/2014
                        //        if (reader["verity_score"] != DBNull.Value)
                        //        {
                        //            objUser.VerityScore = Convert.ToInt32(reader["verity_score"]);
                        //        }
                        //        if (reader["is_fraud"] != DBNull.Value)
                        //        {
                        //            objUser.IsFraud = Convert.ToBoolean(reader["is_fraud"]);
                        //        }

                        //        if (reader["verity_geo_flag"] != DBNull.Value)
                        //        {
                        //            objUser.GeoCorrelationFlag = Convert.ToInt32(reader["verity_geo_flag"]);
                        //        }
                        //        if (reader["relevant_score"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantScore = Convert.ToInt32(reader["relevant_score"]);
                        //        }
                        //        if (reader["relevant_profile_score"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantProfileScore = Convert.ToInt32(reader["relevant_profile_score"]);
                        //        }
                        //        if (reader["relevant_id"] != DBNull.Value)
                        //        {
                        //            objUser.RelevantId = Convert.ToString(reader["relevant_id"]);
                        //        }

                        //        if (reader["verity_id"] != DBNull.Value)
                        //        {
                        //            objUser.VerityId = Convert.ToString(reader["verity_id"]);
                        //        }
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

        #region Get ClientId Based on MemberUrl
        /// <summary>
        /// Get Organization Deatils By hosturl
        /// </summary>
        /// <param name="hosturl">HostUlr</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Client GetPartnerOrgIdByMemberUrl(string hosturl, Guid UserGuid)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(hosturl);
            if (!string.IsNullOrEmpty(constr))
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            else
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            //int _clientid = 0;
            Client oClient = new Client();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[admin].[ClientDetailbyURL_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@member_url", hosturl);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["ClientId"] != DBNull.Value)
                        {
                            oClient.ClientId = Convert.ToInt32(reader["ClientId"]);
                        }
                        if (reader["org_logo"] != DBNull.Value)
                        {
                            oClient.OrgLogo = reader["org_logo"].ToString();
                        }
                        if (reader["org_name"] != DBNull.Value)
                        {
                            oClient.OrgName = reader["org_name"].ToString();
                        }
                        if (reader["referrer_id"] != DBNull.Value)
                        {
                            oClient.Referrerid = Convert.ToInt32(reader["referrer_id"]);
                        }
                        if (reader["org_type_id"] != DBNull.Value)
                        {
                            oClient.OrgTypeId = Convert.ToInt32(reader["org_type_id"]);
                        }
                        if (reader["member_url"] != DBNull.Value)
                        {
                            oClient.MemberUrl = reader["member_url"].ToString();
                        }
                        if (reader["emailaddress"] != DBNull.Value)
                        {
                            oClient.Emailaddress = reader["emailaddress"].ToString();
                        }
                        if (reader["mg_login_path"] != DBNull.Value)
                        {
                            oClient.MgLoginPath = reader["mg_login_path"].ToString();
                        }
                        if (reader["password"] != DBNull.Value)
                        {
                            oClient.Password = reader["password"].ToString();
                        }
                        if (reader["copyright_year"] != DBNull.Value)
                        {
                            oClient.CopyrightYear = Convert.ToInt32(reader["copyright_year"]);
                        }
                        if (reader["postal_address"] != DBNull.Value)
                        {
                            oClient.Address = reader["postal_address"].ToString();
                        }
                        if (reader["aboutus_text"] != DBNull.Value)
                        {
                            oClient.AboutusText = reader["aboutus_text"].ToString();
                        }
                        if (reader["theem"] != DBNull.Value)
                        {
                            oClient.StyleSheettheme = reader["theem"].ToString();
                        }
                        if (reader["home_page_url"] != DBNull.Value)
                        {
                            oClient.HomePageURL = reader["home_page_url"].ToString();
                        }
                        if (reader["is_popup"] != DBNull.Value)
                        {
                            oClient.IsPopUp = Convert.ToBoolean(reader["is_popup"].ToString());
                        }
                        if (reader["is_profile_pixel"] != DBNull.Value)
                        {
                            oClient.IsProfilePixel = Convert.ToBoolean(reader["is_profile_pixel"].ToString());
                        }
                        if (reader["is_survey_pixel"] != DBNull.Value)
                        {
                            oClient.IsSurveyPixel = Convert.ToBoolean(reader["is_survey_pixel"].ToString());
                        }
                        if (reader["profile_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileClickPixelUrl = reader["profile_click_pixel_url"].ToString();
                        }
                        if (reader["survey_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyClickPixelUrl = reader["survey_click_pixel_url"].ToString();
                        }
                        if (reader["profile_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileCompletePixelUrl = reader["profile_complete_pixel_url"].ToString();
                        }
                        if (reader["survey_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyCompletePixelUrl = reader["survey_complete_pixel_url"].ToString();
                        }
                        if (reader["is_verity_required"] != DBNull.Value)
                        {
                            oClient.VerityRequired = Convert.ToBoolean(reader["is_verity_required"].ToString());
                        }
                        if (reader["is_relevantid_required"] != DBNull.Value)
                        {
                            oClient.RelevantIdRequired = Convert.ToBoolean(reader["is_relevantid_required"].ToString());
                        }
                        if (reader["IsStep1Enable"] != DBNull.Value)
                        {
                            oClient.IsStep1Enable = Convert.ToBoolean(reader["IsStep1Enable"]);
                        }
                        if (reader["is_standalone_partner"] != DBNull.Value)
                        {
                            oClient.IsStandalone = Convert.ToBoolean(reader["is_standalone_partner"]);
                        }
                        if (reader["is_show_rewards"] != DBNull.Value)
                        {
                            oClient.IsRewardsShow = Convert.ToBoolean(reader["is_show_rewards"]);
                        }
                        if (reader["partner_survey_term_url"] != DBNull.Value)
                        {
                            oClient.PartnerTerminateUrl = Convert.ToString(reader["partner_survey_term_url"]);
                        }
                        if (reader["is_top10_enable"] != DBNull.Value)
                        {
                            oClient.IsTop10Enable = Convert.ToBoolean(reader["is_top10_enable"]);
                        }
                        if (reader["is_email_invitation"] != DBNull.Value)
                        {
                            oClient.IsEmailInvitationEnable = Convert.ToBoolean(reader["is_email_invitation"]);
                        }
                        if (reader["is_sms_invitation"] != DBNull.Value)
                        {
                            oClient.IsSmsInvitation = Convert.ToBoolean(reader["is_sms_invitation"]);
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
            return oClient;
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

        #region User Insert

        /// <summary>
        /// User Insert
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="password">password</param>
        public User UserInsert(User oUser)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[User_Insert]", cn);
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
                using (IDataReader reader = cm.ExecuteReader())
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
                            oUser.DateOfBirth = Convert.ToDateTime((reader["dob"]));

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
                            oUser.Country_id = reader["country_code"].ToString();
                        }
                        if (reader["fb_username_mailid"] != DBNull.Value)
                        {
                            oUser.FbUsername = reader["fb_username_mailid"].ToString();
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            oUser.SubId2 = reader["sub_id2"].ToString();
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
        #endregion

        #region Users List
        /// <summary>
        /// Get all users list
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            List<User> lstUser = new List<User>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[dbo].[User_details]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User objUser = new User();
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
                        if (reader["zip_code"] != DBNull.Value)
                        {
                            objUser.ZipCode = Convert.ToString(reader["zip_code"]);
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
                        if (reader["city"] != DBNull.Value)
                        {
                            objUser.City = Convert.ToString(reader["city"]);
                        }
                        lstUser.Add(objUser);
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                cn.Close();
            }
            return lstUser;
        }
        #endregion

        #region Getting User Information for Home Page
        /// <summary>
        ///  Getting User Information for Home Page
        /// </summary>
        /// <returns></returns>
        public Home GetHomePageDetails(int UserId, int ClientId)
        {
            Home objUser = new Home();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[HomePageInfo_Get]", cn);
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

                        if (reader["first_level_friends_count"] != DBNull.Value)
                        {
                            objUser.SecondReferrals = Convert.ToInt32(reader["first_level_friends_count"]);
                        }
                        if (reader["second_level_friends_count"] != DBNull.Value)
                        {
                            objUser.ThirdReferrals = Convert.ToInt32(reader["second_level_friends_count"]);
                        }
                        if (reader["reward_referrering"] != DBNull.Value)
                        {
                            objUser.RewardsEarned = Convert.ToDecimal(reader["reward_referrering"]);
                        }
                        if (reader["reward_total"] != DBNull.Value)
                        {
                            objUser.AccountBalance = Convert.ToDecimal(reader["reward_total"]);
                        }
                        if (reader["surveys_pending"] != DBNull.Value)
                        {
                            objUser.TotalSurveys = Convert.ToInt32(reader["surveys_pending"]);

                        }
                        if (reader["pending_surveys_reward"] != DBNull.Value)
                        {
                            objUser.TotalRewardforSurvey = Convert.ToDecimal(reader["pending_surveys_reward"]);

                        }
                        if (reader["ipad_entry_count"] != DBNull.Value)
                        {
                            objUser.IpadEntryCount = Convert.ToInt32(reader["ipad_entry_count"]);
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

            return objUser;
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

        #region Dvariable
        /// <summary>
        /// GetDvariable
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetDvariable(int UserId)
        {
            string _countryIds = string.Empty;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[DVariable_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserId);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["sn_variable"] != DBNull.Value)
                        {
                            _countryIds = Convert.ToString(reader["sn_variable"]);
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
            return _countryIds;
        }
        #endregion

        //#region Reason ForUnsubscribing User

        ///// <summary>
        ///// Get Dvariable
        ///// </summary>
        ///// <param name="UserGuid">userguid</param>
        //public string GetDvariable()
        //{
        //    string dvar = string.Empty;
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    cn.Open();
        //    SqlCommand cmd = new SqlCommand("[pms].[DVariable_Get]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("user_id", Convert.ToInt32(Identity.Current.UserData.UserId));
        //    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
        //    {
        //        cmd.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
        //    }
        //    else
        //    {
        //        cmd.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
        //    }
        //    using (IDataReader reader = cmd.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            if (reader["dvariable"] != DBNull.Value)
        //            {
        //                dvar = Convert.ToString(reader["dvariable"]);
        //            }

        //        }

        //    }
        //    return dvar;
        //}

        //#endregion

        //#region GetTEvariable
        ///// <summary>
        ///// Get TEvariable
        ///// </summary>
        ///// <param name="userId">userid</param>
        ///// <returns></returns>
        //public string GetTEvariable(int userId)
        //{
        //    string tevar = string.Empty;
        //    DateTime dt = DateTime.Now;
        //    int _surveyscompleted = 0;
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    cn.Open();
        //    SqlCommand cmd = new SqlCommand("[pms].[TEVariable_Get]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("user_id", Convert.ToInt32(Identity.Current.UserData.UserId));
        //    using (IDataReader reader = cmd.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            if (reader["create_dt"] != DBNull.Value)
        //            {
        //                dt = Convert.ToDateTime(reader["create_dt"]);
        //            }
        //            if (reader["totalsurveys"] != DBNull.Value)
        //            {
        //                _surveyscompleted = Convert.ToInt32(reader["totalsurveys"]);
        //            }
        //        }
        //    }
        //    tevar = dt.ToString("yyyyMMDD") + "-" + _surveyscompleted.ToString();
        //    return tevar;
        //}
        //#endregion

        #region SN Variable
        /// <summary>
        /// GetSNvariable
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetSNvariable(int UserId)
        {
            string _countryIds = string.Empty;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[SNVariable_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserId);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["sn_variable"] != DBNull.Value)
                        {
                            _countryIds = Convert.ToString(reader["sn_variable"]);
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
            return _countryIds;
        }
        #endregion


        //#region SNVariable

        ///// <summary>
        ///// get SNvariable
        ///// </summary>
        ///// <param name="UserGuid">userguid</param>
        //public string GetSNvariable()
        //{
        //    string dvar = string.Empty;
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    cn.Open();
        //    SqlCommand cmd = new SqlCommand("[pms].[SNVariable_Get]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("user_id", Convert.ToInt32(Identity.Current.UserData.UserId));
        //    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
        //    {
        //        cmd.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
        //    }
        //    else
        //    {
        //        cmd.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
        //    }
        //    using (IDataReader reader = cmd.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            if (reader["sn_variable"] != DBNull.Value)
        //            {
        //                dvar = Convert.ToString(reader["sn_variable"]);
        //            }
        //        }

        //    }
        //    return dvar;
        //}

        //#endregion

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

        #region GetConnectionString by userGuid
        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetConnectionString1(string UserGuid)
        {
            string connection = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
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

        #region Step1 User Insert
        /// <summary>
        ///  Step1 User Insert
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>
        public Guid Step1UserInsert(User oUser, string Host)
        {
            Guid guid = Guid.Empty;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(Host, null, null);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[Step1_User_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                #region Adding params for the SQL Proc (IF NOT NULL)
                if (oUser.FirstName != null)
                {
                    cm.Parameters.AddWithValue("first_name", oUser.FirstName);
                }
                else
                {
                    cm.Parameters.AddWithValue("first_name", string.Empty);
                }
                if (oUser.LastName != null)
                {
                    cm.Parameters.AddWithValue("last_name", oUser.LastName);
                }
                else
                {
                    cm.Parameters.AddWithValue("last_name", string.Empty);
                }
                cm.Parameters.AddWithValue("email_address", oUser.EmailAddress);
                cm.Parameters.AddWithValue("is_dnc", oUser.IsDnc);
                cm.Parameters.AddWithValue("is_fraud", oUser.IsFraud);
                cm.Parameters.AddWithValue("cpa_count", oUser.CpaCount);
                cm.Parameters.AddWithValue("dnc_reason", oUser.DncReason);
                cm.Parameters.AddWithValue("ip_address", oUser.IpAddress);
                cm.Parameters.AddWithValue("referring_url", oUser.ReferrerUrl);
                cm.Parameters.AddWithValue("click_id", oUser.ClickId);
                cm.Parameters.AddWithValue("hit_id", oUser.HitId);
                cm.Parameters.AddWithValue("country2ip", oUser.Country2Ip);
                //Newly Added on 1/30/2012 By Sandy for Facebook Connecct.
                cm.Parameters.AddWithValue("@facebook_id", oUser.FacebookId);
                cm.Parameters.AddWithValue("@fb_access_token", oUser.AccessToken);
                //added by anwesh, for inserting country_id
                cm.Parameters.AddWithValue("country_id", oUser.CountryId);
                cm.Parameters.AddWithValue("org_id", oUser.OrgId);
                //db.AddInParameter(dbCommand, "city", DbType.String, oUser.City);

                if (oUser.DateOfBirth == DateTime.MinValue)
                {
                    cm.Parameters.AddWithValue("@dob", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@dob", oUser.DateOfBirth);
                }
                cm.Parameters.AddWithValue("@gender", oUser.Gender);
                //End Lines of code.
                if (oUser.RefferId != -1)
                {
                    cm.Parameters.AddWithValue("referrer_id", oUser.RefferId.ToString());
                }
                else
                {
                    cm.Parameters.AddWithValue("referrer_id", "-1");
                }
                if (oUser.SubId2 != string.Empty)
                {
                    cm.Parameters.AddWithValue("sub_id2", oUser.SubId2);
                }
                else
                {
                    cm.Parameters.AddWithValue("sub_id2", "");
                }

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

        #region Step1 User Data Get
        /// <summary>
        /// Step1UserDataGet
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public User Step1UserDataGet(Guid userGuid, string host)
        {
            User objUser = new User();
            User oUser = new User();
            //SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = ConnectionString;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(host);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("[user].[Step1_User_Get]", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1140;
                cmd.Parameters.AddWithValue("user_guid", userGuid);


                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_id"] != DBNull.Value)
                        {
                            objUser.UserId = Convert.ToInt32(reader["user_id"]);
                        }
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
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            objUser.SubId2 = Convert.ToString(reader["sub_id2"]);
                        }
                        if (reader["referrer_id"] != DBNull.Value)
                        {
                            objUser.RefferId = Convert.ToInt32(reader["referrer_id"]);
                        }
                        if (reader["is_fraud"] != DBNull.Value)
                        {
                            objUser.IsFraud = Convert.ToBoolean(reader["is_fraud"]);
                        }
                        if (reader["is_dnc"] != DBNull.Value)
                        {
                            objUser.IsDnc = Convert.ToBoolean(reader["is_dnc"]);
                        }
                        if (reader["cpa_count"] != DBNull.Value)
                        {
                            objUser.CpaCount = Convert.ToInt32(reader["cpa_count"]);
                        }
                        if (reader["dnc_reason"] != DBNull.Value)
                        {
                            objUser.DncReason = Convert.ToString(reader["dnc_reason"]);
                        }
                        if (reader["ip_address"] != DBNull.Value)
                        {
                            objUser.IpAddress = Convert.ToString(reader["ip_address"]);
                        }
                        if (reader["referring_url"] != DBNull.Value)
                        {
                            objUser.ReferrerUrl = Convert.ToString(reader["referring_url"]);
                        }
                        if (reader["click_id"] != DBNull.Value)
                        {
                            objUser.ClickId = Convert.ToInt32(reader["click_id"]);
                        }
                        if (reader["hit_id"] != DBNull.Value)
                        {
                            objUser.HitId = Convert.ToInt32(reader["hit_id"]);
                        }
                        //Added on 1/30/2011 for FB Connect 
                        // gender,	facebook_id,		fb_access_token,		dob
                        if (reader["gender"] != DBNull.Value)
                        {
                            objUser.Gender = Convert.ToString(reader["gender"]);
                        }
                        if (reader["facebook_id"] != DBNull.Value)
                        {
                            objUser.FacebookId = Convert.ToString(reader["facebook_id"]);
                        }
                        if (reader["fb_access_token"] != DBNull.Value)
                        {
                            objUser.AccessToken = Convert.ToString(reader["fb_access_token"]);
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            objUser.DateOfBirth = Convert.ToDateTime(reader["dob"]);
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            objUser.CountryId = Convert.ToInt32(reader["country_id"]);
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

        #region Check Iframe Details by Rid and ExtId Combination
        /// <summary>
        ///  Check Iframe Details by Rid and ExtId Combination
        /// </summary>
        /// <param name="Rid">Rid</param>
        /// <param name="ExtId">ExtId</param>
        /// <param name="host">Host</param>
        /// <returns></returns>
        public MemberEntity UserDeialscCheck(int Rid, string ExtId, string host)
        {
            MemberEntity objMember = new MemberEntity();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, Rid, null);
            if (constr != string.Empty)
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();

            try
            {

                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[user_exist_checkbyridandexternalmemberid]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(Rid));
                if (ExtId != "")
                {
                    cm.Parameters.AddWithValue("@ext_id", ExtId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ext_id", ExtId);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objMember.UserGuid = new Guid(dr["user_guid"].ToString());
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objMember.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["totalcount"] != DBNull.Value)
                        {
                            objMember.Count = Convert.ToInt32(dr["totalcount"]);
                        }
                        if (dr["org_id"] != DBNull.Value)
                        {
                            objMember.OrgId = Convert.ToInt32(dr["org_id"]);
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
            return objMember;
        }
        #endregion

        #region Check Iframe Details by Rid and EmailAddress Combination
        /// <summary>
        /// Check Iframe Details by Rid and EmailAddress Combination
        /// </summary>
        /// <param name="Rid">Rid</param>
        /// <param name="EmailAddress">EmailAddress</param>
        /// <param name="Host">HostAddress</param>
        /// <returns></returns>
        public MemberEntity UserDeialscCheckByEmailAddress(int Rid, string EmailAddress, string Host)
        {
            MemberEntity objMember = new MemberEntity();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, Rid, null);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {

                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[user_exist_checkbyridandemailaddress]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(Rid));
                if (EmailAddress != "")
                {
                    cm.Parameters.AddWithValue("@email_address", EmailAddress);
                }
                else
                {
                    cm.Parameters.AddWithValue("@email_address", DBNull.Value);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objMember.UserGuid = new Guid(dr["user_guid"].ToString());
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objMember.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["totalcount"] != DBNull.Value)
                        {
                            objMember.Count = Convert.ToInt32(dr["totalcount"]);
                        }
                        if (dr["client_id"] != DBNull.Value)
                        {
                            objMember.OrgId = Convert.ToInt32(dr["client_id"]);
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
            return objMember;
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
        #region Update Relevant Score
        /// <summary>
        /// ReleventUpdate
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="PfScore"></param>
        /// <param name="FpfScore"></param>
        /// <param name="RelevantId"></param>
        /// <returns></returns>
        public int ReleventUpdate(Guid UserGuid, int RelevantScore, int PfScore, string FpfScore, string RelevantId, int ClientId, string ipqsJson)
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

        #region Get IpAddress Count
        /// <summary>
        /// Get IpAddress Count
        /// </summary>
        /// <param name="IpAddress">IpAddress</param>
        /// <returns></returns>
        public int Step1GetIpAddressCount(string IpAddress)
        {
            int count = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString1;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[IpAddressCount_Check]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["count"] != DBNull.Value)
                        {
                            count = Convert.ToInt32(dr["count"]);
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
            return count;
        }
        #endregion

        #region GetUserDetials For PeanutLabs
        /// <summary>
        /// GetUserDetials For PeanutLabs
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetUserDetailsForPeanutLabs(Guid UserGuid, int clientid)
        {
            string result = string.Empty;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, clientid);
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            conn.Open();
            try
            {
                SqlCommand comm = new SqlCommand("[user].[pl_api_pii_details]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["final_query"] != DBNull.Value)
                        {
                            result = Convert.ToString(reader["final_query"]);
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
            return result;
        }

        #endregion

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

        #region getladingpage for Router
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referrer_id"></param>
        /// <returns></returns>
        public string GetRouterLandingpageUrl(int referrer_id, string txid)
        {
            string url = string.Empty;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            conn.Open();
            try
            {
                SqlCommand comm = new SqlCommand("[referrer].[cp_ob_referrerlandingpage_get]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@referrer_id", referrer_id);
                comm.Parameters.AddWithValue("@txid", txid);
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
        #region Change Password 
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public string ChangePassword(string NewPassword, string email)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[widget_pwd_change]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                //cm.Parameters.AddWithValue("@old_password", OldPassword);
                cm.Parameters.AddWithValue("@new_password", NewPassword);
                cm.Parameters.AddWithValue("@email_address", email);
                //cm.Parameters.AddWithValue("@rid", rid);
                //cm.Parameters.AddWithValue("@current_Date", currentDate);
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

        #region Rest password
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public int SendresetLink(int orgid, int rid, string extmid)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[widget_reset_password]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@org_id", orgid);
                cm.Parameters.AddWithValue("@rid", rid);
                cm.Parameters.AddWithValue("@email_address", extmid);
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

        #region Get UserData
        /// <summary>
        /// Get User Data by userGuid
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ClientId">ClientiId</param>
        /// <returns></returns>
        public User GetUserDataEmail(string EmailAddress, int ClientId)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[mail].[campaign.campaignitem_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
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


        #region email Address vaild
        /// <summary>
        /// vaid email address by email
        /// </summary>
        /// <param name="EmailAddress">EmailAddress</param>
        /// <param name="ClientId">ClientId</param>
        /// <returns></returns>
        public User emailAddressvaild(string EmailAddress, int ClientId)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[partner_emailaddress_check]", cn);
                cm.CommandType = CommandType.StoredProcedure;                
                cm.Parameters.AddWithValue("email_address", EmailAddress);
                cm.Parameters.AddWithValue("org_id", ClientId);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {                        
                        if (reader["email_address"] != DBNull.Value)
                        {
                            objUser.EmailAddress = Convert.ToString(reader["email_address"]);
                        }                       
                        if (reader["org_id"] != DBNull.Value)
                        {
                            objUser.OrgId = Convert.ToInt32(reader["org_id"]);
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

        #region Send Email
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="fromaddress"></param>
        /// <param name="comments"></param>
        //public void SendEmail(string fromaddress, string comments)
        //{
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = ConnectionString1;
        //    try
        //    {
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("[user].[contact_us_email]", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@from_email", fromaddress);
        //        cmd.Parameters.AddWithValue("@email_subject", comments);
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
        #endregion

        #region Cancel User Account
        public void CancelUser(Guid UserGuid, int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_delete]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region Get Step2 Details
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="LeadGuid">UserGuid</param>
        /// <returns></returns>
        public User GetStep2Details(string LeadGuid, int ClientId)
        {
            User ouser = new User();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[lead_user_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("lead_guid", LeadGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["email_address"] != DBNull.Value)
                        {
                            ouser.EmailAddress = Convert.ToString(dr["email_address"]);
                        }
                        if (dr["first_name"] != DBNull.Value)
                        {
                            ouser.FirstName = Convert.ToString(dr["first_name"]);
                        }
                        if (dr["last_name"] != DBNull.Value)
                        {
                            ouser.LastName = Convert.ToString(dr["last_name"]);
                        }
                        if (dr["make"] != DBNull.Value)
                        {
                            ouser.Make = Convert.ToString(dr["make"]);
                        }
                        if (dr["address1"] != DBNull.Value)
                        {
                            ouser.Address1 = Convert.ToString(dr["address1"]);
                        }
                        if (dr["zip_code"] != DBNull.Value)
                        {
                            ouser.ZipCode = Convert.ToString(dr["zip_code"]);
                        }
                        if (dr["state"] != DBNull.Value)
                        {
                            ouser.StateId = Convert.ToInt32(dr["state"]);
                        }
                        if (dr["city"] != DBNull.Value)
                        {
                            ouser.City = Convert.ToString(dr["city"]);
                        }
                        if (dr["coutry"] != DBNull.Value)
                        {
                            ouser.CountryId = Convert.ToInt32(dr["coutry"]);
                        }
                        if (dr["gender"] != DBNull.Value)
                        {
                            ouser.Gender = Convert.ToString(dr["gender"]);
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
            return ouser;
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
                        if (dr["is_enable_login"] != DBNull.Value)
                        {
                            oClient.IsEnablelogin = Convert.ToBoolean(dr["is_enable_login"]);
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

        #region SaveLinkedinData
        /// <summary>
        ///  SaveLinkedinData
        /// </summary>
        /// <param name="oUser">User</param>
        /// <returns></returns>
        public string SaveLinkedinData(string json)
        {
            string id = string.Empty;
            UserDataServices objDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[linkedin_date_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("@json", json);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["id"] != DBNull.Value)
                        {
                            id = Convert.ToString(dr["id"].ToString());
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

        #region Get Linkedin Data
        /// <summary>
        /// Get Linkedin Data
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public User GetLinkedinData(int id)
        {
            User objUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[linkedin_data_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@id", id);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
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
            return objUser;
        }
        #endregion

        public List<PSquestion> GetPollQuestions(int UsId)
        {
            List<PSquestion> lstQuestions = new List<PSquestion>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[poll_questions_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UsId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    List<PSoptions> lstOptions = new List<PSoptions>();
                    List<PSquestion> lstChildQuestions = new List<PSquestion>();
                    List<PSoptions> lstChildQuestionsOptions = new List<PSoptions>();
                    List<PSquestion> lstChldQoMaping = new List<PSquestion>();
                    List<PSquestion> lstQuestionResponses = new List<PSquestion>();
                    List<PSquestion> lstSubChildQuestions = new List<PSquestion>();
                    List<PSoptions> lstSubChildQuestionsOptions = new List<PSoptions>();
                    while (dr.Read())
                    {
                        PSquestion oQuestion = new PSquestion();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            oQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            oQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            oQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        lstQuestions.Add(oQuestion);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        PSquestion oquestion = new PSquestion();
                        PSoptions objOptions = new PSoptions();
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
                        oquestion.OptionTypeId = objOptions.OptionTypeId;
                        lstOptions.Add(objOptions);
                    }
                    foreach (PSquestion oQst in lstQuestions)
                    {
                        if (lstOptions.Count > 0)
                        {
                            oQst.OptionList = lstOptions;
                        }
                        if (lstChildQuestions.Count > 0)
                        {
                            oQst.ChildQuestionList = lstChildQuestions;
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
            return lstQuestions;
        }

        #region Save Poll Options
        /// <summary>
        /// Save Poll Options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <param name="QuestionId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <returns></returns>
        public string SavePollOptions(string Xml, int UsId, int QuestionId, int SortOrder)
        {
            string PollData = "";
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString3;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[poll_questions_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UsId);
                cm.Parameters.AddWithValue("@xml", Xml);
                cm.Parameters.AddWithValue("@current_question_id", QuestionId);
                cm.Parameters.AddWithValue("@current_question_sort_order", SortOrder);
                using (SqlDataReader dr = cm.ExecuteReader())
                {

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr["poll"] != DBNull.Value)
                                PollData = dr["poll"].ToString();
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
            return PollData;
        }
        #endregion

        #region Save Do Not Sell My Info
        /// <summary>
        /// Save Do Not Sell My Info
        /// </summary>
        /// <returns></returns>
        public int SaveDoNotSellMyInfo(string FirstName, string LastName, string EmailAddress, string PrecisionSampleSite, int RequestID, string RequestName, int ClientId)
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
            return 0;
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

        #region GetCAG Activity 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public int RouteruserActivityGet(int user_id)
        {
            int redirect_2_reg = 0;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = ConnectionStringActivity;
            cn.ConnectionString = constr;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[router_user_activity_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", user_id);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["redirect_2_reg"] != null)
                    {
                        redirect_2_reg = Convert.ToInt32(oreader["redirect_2_reg"]);
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
            return redirect_2_reg;

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

        #region Referrer Clicks Insert

        /// <summary>
        /// Insert Clicks
        /// </summary>
        /// <returns></returns>
        public string InsertAffClicks(int rid, string SubID3, string IPAddress, string sid, string fid, string trans_id, string RefererUrl)
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