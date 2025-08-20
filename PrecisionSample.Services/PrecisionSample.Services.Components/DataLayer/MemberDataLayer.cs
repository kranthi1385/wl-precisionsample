using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrecisionSample.Services.Components.Entites;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
using Newtonsoft.Json;

namespace PrecisionSample.Services.Components.DataLayer
{
    public class MemberDataLayer
    {
        #region base connection string
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }

        }
        #endregion

        #region Create
        public string Create(Member member)
        {
            string result = string.Empty;
            int orgId = 0;
            string subId2 = string.Empty;
            int subReferrerId = 0;
            int countryId = 0; int stateId = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("api.user_insertion_related_data_get", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", member.Rid);               
                if (string.IsNullOrEmpty(member.Country))
                {
                    cm.Parameters.AddWithValue("@country", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@country", member.Country);
                }
                if (string.IsNullOrEmpty(member.State))
                {
                    cm.Parameters.AddWithValue("@state", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@state", member.State);
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            orgId = Convert.ToInt32(reader["org_id"].ToString());
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            subId2 = (reader["sub_id2"].ToString());
                        }
                        if (reader["sub_referrer_id"] != DBNull.Value)
                        {
                            subReferrerId = Convert.ToInt32(reader["sub_referrer_id"].ToString());
                        }
                        if (reader["state_id"] != DBNull.Value)
                        {
                            stateId = Convert.ToInt32(reader["state_id"].ToString());
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            countryId = Convert.ToInt32(reader["country_id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("Create|DB Error|" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(member.Rid);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_insert]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    #region Parameters
                    cm.Parameters.AddWithValue("@referrer_id", member.Rid);
                    cm.Parameters.AddWithValue("@Ext_MemberID", member.ExtMemberId);
                    if (member.EmailAddress == null)
                        member.EmailAddress = string.Empty;
                    cm.Parameters.AddWithValue("@email_address", member.EmailAddress);
                    cm.Parameters.AddWithValue("@country_id", countryId);
                    cm.Parameters.AddWithValue("@org_id", orgId);
                    cm.Parameters.AddWithValue("@sub_id2", subId2);
                    cm.Parameters.AddWithValue("@sub_referrer_id", subReferrerId);
                    cm.Parameters.AddWithValue("@state_id", stateId);

                    if (string.IsNullOrEmpty(member.FirstName))
                    {
                        cm.Parameters.AddWithValue("@first_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@first_name", member.FirstName);
                    }
                    if (string.IsNullOrEmpty(member.LastName))
                    {
                        cm.Parameters.AddWithValue("@last_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@last_name", member.LastName);
                    }


                    if (string.IsNullOrEmpty(member.Address1))
                    {
                        cm.Parameters.AddWithValue("@address1", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address1", member.Address1);
                    }
                    if (string.IsNullOrEmpty(member.Address2))
                    {
                        cm.Parameters.AddWithValue("@address2", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address2", member.Address2);
                    }
                    if (string.IsNullOrEmpty(member.City))
                    {
                        cm.Parameters.AddWithValue("@city", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@city", member.City);
                    }
                    if (string.IsNullOrEmpty(member.Zip))
                    {
                        cm.Parameters.AddWithValue("@zip_code", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@zip_code", member.Zip);
                    }
                    if (string.IsNullOrEmpty(member.Gender))
                    {
                        cm.Parameters.AddWithValue("@gender", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@gender", member.Gender);
                    }
                    if (string.IsNullOrEmpty(member.Dob))
                    {
                        cm.Parameters.AddWithValue("@dob", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@dob", member.Dob);
                    }
                    if (string.IsNullOrEmpty(member.Ethnicity))
                    {
                        cm.Parameters.AddWithValue("@ethnicity", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ethnicity", member.Ethnicity);
                    }
                    if (member.TxId == string.Empty || member.TxId == null)
                    {
                        cm.Parameters.AddWithValue("@tx_id", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@tx_id", member.TxId);
                    }

                    #endregion
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                            {
                                result = reader["Result"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("Create| rid=" + member.Rid + "EmailAddress=" + member.EmailAddress + "OrgId=" + orgId + "|DB Error|" + ex.Message);
                    result = "<Message> Failed to Create Account. </Message>"; ;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("Create| rid=" + member.Rid + " | Connection string is empty");
                result = "UserGuid does not exist";
            }
            return result;
        }
        #endregion

        #region Create for WL sites
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        public string CreateWL(User oUser)
        {
            string result = string.Empty;
            int orgId = 0;
            string subId2 = string.Empty;
            int subReferrerId = 0;
            int languageCode = 0; string countryCode = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[api].[user_insertion_related_data_get_WL]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", oUser.RefferId);
                if (oUser.CountryId > 0)
                {
                    cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@country_id", DBNull.Value);
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            orgId = Convert.ToInt32(reader["org_id"].ToString());
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            subId2 = (reader["sub_id2"].ToString());
                        }
                        if (reader["sub_referrer_id"] != DBNull.Value)
                        {
                            subReferrerId = Convert.ToInt32(reader["sub_referrer_id"].ToString());
                        }
                        if (reader["language_code"] != DBNull.Value)
                        {
                            languageCode = Convert.ToInt32(reader["language_code"].ToString());
                        }
                        if (reader["country_code"] != DBNull.Value)
                        {
                            countryCode = (reader["country_code"].ToString());
                        }
                    }
                }
                Logging.NLog.ClassLogger.Trace("Create WL|FirstName=" + oUser.FirstName + "LastName=" + oUser.LastName + "|DB Trace|");
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("Create WL| rid=" + oUser.RefferId + "DB Error|" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(oUser.RefferId);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_insert_wl]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    #region Parameters
                    cm.Parameters.AddWithValue("@referrer_id", oUser.RefferId);
                    cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                    cm.Parameters.AddWithValue("@org_id", orgId);
                    cm.Parameters.AddWithValue("@country_code", countryCode);
                    cm.Parameters.AddWithValue("@language_code", languageCode);
                    if (oUser.RouterSubReferrerId > 0)
                    {
                        cm.Parameters.AddWithValue("@sub_referrer_id", oUser.RouterSubReferrerId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@sub_referrer_id", subReferrerId);
                    }
                  
                    if (oUser.CountryId > 0)
                    {
                        cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@country_id", DBNull.Value);
                    }
                    if (oUser.StateId > 0)
                    {
                        cm.Parameters.AddWithValue("@state_id", oUser.StateId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@state_id", DBNull.Value);
                    }
                    if (string.IsNullOrEmpty(oUser.FirstName))
                    {
                        cm.Parameters.AddWithValue("@first_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@first_name", oUser.FirstName);
                    }
                    if (string.IsNullOrEmpty(oUser.LastName))
                    {
                        cm.Parameters.AddWithValue("@last_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@last_name", oUser.LastName);
                    }


                    if (string.IsNullOrEmpty(oUser.Address1))
                    {
                        cm.Parameters.AddWithValue("@address1", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address1", oUser.Address1);
                    }
                    if (string.IsNullOrEmpty(oUser.Address2))
                    {
                        cm.Parameters.AddWithValue("@address2", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address2", oUser.Address2);
                    }
                    if (string.IsNullOrEmpty(oUser.City))
                    {
                        cm.Parameters.AddWithValue("@city", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@city", oUser.City);
                    }
                    if (string.IsNullOrEmpty(oUser.ZipCode))
                    {
                        cm.Parameters.AddWithValue("@zip_code", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@zip_code", oUser.ZipCode);
                    }
                    if (string.IsNullOrEmpty(oUser.Gender))
                    {
                        cm.Parameters.AddWithValue("@gender", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@gender", oUser.Gender);
                    }
                    if (string.IsNullOrEmpty(oUser.Dob))
                    {
                        cm.Parameters.AddWithValue("@dob", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@dob", oUser.Dob);
                    }
                    if (oUser.EthnicityId > 0)
                    {
                        cm.Parameters.AddWithValue("@ethnicity_id", oUser.EthnicityId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ethnicity_id", DBNull.Value);
                    }
                    if (string.IsNullOrEmpty(oUser.IpAddress))
                    {
                        cm.Parameters.AddWithValue("@ip_address", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ip_address", oUser.IpAddress);
                    }
                    if (string.IsNullOrEmpty(oUser.SubId2))
                    {
                        if (string.IsNullOrEmpty(subId2))
                            cm.Parameters.AddWithValue("@sub_id2", DBNull.Value);
                        else
                            cm.Parameters.AddWithValue("@sub_id2", subId2);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@sub_id2", oUser.SubId2);
                    }
                    if (!string.IsNullOrEmpty(oUser.SubId3))
                    {
                        cm.Parameters.AddWithValue("@sub_id3", oUser.SubId3);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@sub_id3", DBNull.Value);
                    }
                    if (string.IsNullOrEmpty(oUser.ReferrerUrl))
                    {
                        cm.Parameters.AddWithValue("@referrer_url", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@referrer_url", oUser.ReferrerUrl);
                    }
                    if (string.IsNullOrEmpty(oUser.Password))
                    {
                        cm.Parameters.AddWithValue("@password", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@password", oUser.Password);
                    }
                    if (string.IsNullOrEmpty(oUser.PhoneNumber))
                    {
                        cm.Parameters.AddWithValue("@phonenumber", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@phonenumber", oUser.PhoneNumber);
                    }
                    if (oUser.ClickId > 0)
                    {
                        cm.Parameters.AddWithValue("@click_id", oUser.ClickId);
                    }
                    if (oUser.HitId > 0)
                    {
                        cm.Parameters.AddWithValue("@hit_id", oUser.HitId);
                    }
                    //frientid
                    if (string.IsNullOrEmpty(oUser.FriendId))
                    {
                        cm.Parameters.AddWithValue("@first_level_referrer_id", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@first_level_referrer_id", oUser.FriendId);
                    }
                    if (oUser.LanguageId > 0)
                    {
                        cm.Parameters.AddWithValue("@language_id", oUser.LanguageId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@language_id", DBNull.Value);
                    }                    
                    #endregion
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                            {
                                result = reader["Result"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("CreateWL | rid=" + oUser.RefferId + "|DB Error|" + ex.Message);
                    result = ex.Message;

                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("CreateWL | rid=" + oUser.RefferId + " |Connection string is empty");
                result = "Rid doesn't exist";
            }
            return result;
        }
        #endregion

        #region Create for Widget sites
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        public string CreateWidget(MemberEntity oUser)
        {
            string result = string.Empty;
            int orgId = 0;
            string subId2 = string.Empty;
            string countryCode = string.Empty;
            int subReferrerId = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[api].[user_insertion_related_data_get_WL]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", oUser.RefferId);
                if (!string.IsNullOrEmpty(oUser.pubId))
                {
                    cm.Parameters.AddWithValue("@sub_referrer_code", oUser.pubId);
                    cm.Parameters.AddWithValue("@is_sub_affliate", oUser.SubAffliateId);
                }
                if (oUser.CountryId > 0)
                {
                    cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@country_id", DBNull.Value);
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            orgId = Convert.ToInt32(reader["org_id"].ToString());
                        }
                        if (reader["country_code"] != DBNull.Value)
                        {
                            countryCode = (reader["country_code"].ToString());
                        }
                        if (reader["sub_referrer_id"] != DBNull.Value)
                        {
                            subReferrerId = Convert.ToInt32(reader["sub_referrer_id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("Create WL| rid=" + oUser.RefferId + "DB Error|" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(oUser.RefferId);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_insert_wl]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    #region Parameters
                    cm.Parameters.AddWithValue("@referrer_id", oUser.RefferId);
                    cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                    cm.Parameters.AddWithValue("@country_code", countryCode);
                    cm.Parameters.AddWithValue("@org_id", orgId);
                    cm.Parameters.AddWithValue("@sub_referrer_id", subReferrerId);
                    if (oUser.CountryId > 0)
                    {
                        cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@country_id", DBNull.Value);
                    }
                    if (oUser.StateId > 0)
                    {
                        cm.Parameters.AddWithValue("@state_id", oUser.StateId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@state_id", DBNull.Value);
                    }
                    if (string.IsNullOrEmpty(oUser.FirstName))
                    {
                        cm.Parameters.AddWithValue("@first_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@first_name", oUser.FirstName);
                    }
                    if (string.IsNullOrEmpty(oUser.LastName))
                    {
                        cm.Parameters.AddWithValue("@last_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@last_name", oUser.LastName);
                    }


                    if (string.IsNullOrEmpty(oUser.Address1))
                    {
                        cm.Parameters.AddWithValue("@address1", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address1", oUser.Address1);
                    }
                    if (string.IsNullOrEmpty(oUser.Address2))
                    {
                        cm.Parameters.AddWithValue("@address2", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address2", oUser.Address2);
                    }
                    if (string.IsNullOrEmpty(oUser.City))
                    {
                        cm.Parameters.AddWithValue("@city", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@city", oUser.City);
                    }
                    if (string.IsNullOrEmpty(oUser.ZipCode))
                    {
                        cm.Parameters.AddWithValue("@zip_code", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@zip_code", oUser.ZipCode);
                    }
                    if (string.IsNullOrEmpty(oUser.Gender))
                    {
                        cm.Parameters.AddWithValue("@gender", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@gender", oUser.Gender);
                    }
                    if (string.IsNullOrEmpty(oUser.Dob))
                    {
                        cm.Parameters.AddWithValue("@dob", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@dob", oUser.Dob);
                    }
                    if (oUser.EthnicityId > 0)
                    {
                        cm.Parameters.AddWithValue("@ethnicity_id", oUser.EthnicityId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ethnicity_id", DBNull.Value);
                    }

                    if (string.IsNullOrEmpty(oUser.SubId2))
                    {

                        cm.Parameters.AddWithValue("@sub_id2", DBNull.Value);

                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@sub_id2", oUser.SubId2);
                    }
                    if (!string.IsNullOrEmpty(oUser.SubId3))
                    {
                        cm.Parameters.AddWithValue("@sub_id3", oUser.SubId3);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@sub_id3", DBNull.Value);
                    }

                    if (string.IsNullOrEmpty(oUser.Password))
                    {
                        cm.Parameters.AddWithValue("@password", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@password", oUser.Password);
                    }
                    if (string.IsNullOrEmpty(oUser.PhoneNumber))
                    {
                        cm.Parameters.AddWithValue("@phonenumber", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@phonenumber", oUser.PhoneNumber);
                    }
                    if (string.IsNullOrEmpty(oUser.IpAddress))
                    {
                        cm.Parameters.AddWithValue("@ip_address", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ip_address", oUser.IpAddress);
                    }
                    if (oUser.LanguageId > 0)
                    {
                        cm.Parameters.AddWithValue("@language_id", oUser.LanguageId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@language_id", DBNull.Value);
                    }
                    #endregion
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                            {
                                result = reader["Result"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("CreateWidget | rid=" + oUser.RefferId + "|DB Error|" + ex.Message);
                    result = ex.Message;

                }
                finally
                {
                    cn.Close();
                }

            }
            else
            {
                Logging.NLog.ClassLogger.Error("CreateWL | rid=" + oUser.RefferId + " |Connection string is empty");
                result = "Rid doesn't exist";
            }
            return result;
        }
        #endregion

        #region Update
        public string Update(UMember member)
        {
            string result = string.Empty;
            int orgId = 0;
            string subId2 = string.Empty;
            int subReferrerId = 0;
            int countryId = 0; int stateId = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("api.user_insertion_related_data_get", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", member.Rid);
                if (string.IsNullOrEmpty(member.Country))
                {
                    cm.Parameters.AddWithValue("@country", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@country", member.Country);
                }
                if (string.IsNullOrEmpty(member.State))
                {
                    cm.Parameters.AddWithValue("@state", DBNull.Value);
                }
                else
                {
                    cm.Parameters.AddWithValue("@state", member.State);
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["org_id"] != DBNull.Value)
                        {
                            orgId = Convert.ToInt32(reader["org_id"].ToString());
                        }
                        if (reader["sub_id2"] != DBNull.Value)
                        {
                            subId2 = (reader["sub_id2"].ToString());
                        }
                        if (reader["sub_referrer_id"] != DBNull.Value)
                        {
                            subReferrerId = Convert.ToInt32(reader["sub_referrer_id"].ToString());
                        }
                        if (reader["state_id"] != DBNull.Value)
                        {
                            stateId = Convert.ToInt32(reader["state_id"].ToString());
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            countryId = Convert.ToInt32(reader["country_id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("Update|rid=" + member.Rid + "|DB Error|" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(member.Rid, null);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_update]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    #region Parameters
                    cm.Parameters.AddWithValue("@referrer_id", member.Rid);
                    cm.Parameters.AddWithValue("@user_guid", member.UserGuid);
                    cm.Parameters.AddWithValue("@Ext_MemberID", member.ExtMemberId);
                    cm.Parameters.AddWithValue("@email_address", member.EmailAddress);
                    cm.Parameters.AddWithValue("@country_id", countryId);
                    cm.Parameters.AddWithValue("@org_id", orgId);
                    cm.Parameters.AddWithValue("@sub_id2", subId2);
                    cm.Parameters.AddWithValue("@sub_referrer_id", subReferrerId);
                    cm.Parameters.AddWithValue("@state_id", stateId);
                    if (string.IsNullOrEmpty(member.FirstName))
                    {
                        cm.Parameters.AddWithValue("@first_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@first_name", member.FirstName);
                    }
                    if (string.IsNullOrEmpty(member.LastName))
                    {
                        cm.Parameters.AddWithValue("@last_name", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@last_name", member.LastName);
                    }


                    if (string.IsNullOrEmpty(member.Address1))
                    {
                        cm.Parameters.AddWithValue("@address1", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address1", member.Address1);
                    }
                    if (string.IsNullOrEmpty(member.Address2))
                    {
                        cm.Parameters.AddWithValue("@address2", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address2", member.Address2);
                    }
                    if (string.IsNullOrEmpty(member.City))
                    {
                        cm.Parameters.AddWithValue("@city", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@city", member.City);
                    }
                    if (string.IsNullOrEmpty(member.Zip))
                    {
                        cm.Parameters.AddWithValue("@zip_code", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@zip_code", member.Zip);
                    }
                    if (string.IsNullOrEmpty(member.Gender))
                    {
                        cm.Parameters.AddWithValue("@gender", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@gender", member.Gender);
                    }
                    if (string.IsNullOrEmpty(member.Dob))
                    {
                        cm.Parameters.AddWithValue("@dob", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@dob", member.Dob);
                    }
                    if (string.IsNullOrEmpty(member.Ethnicity))
                    {
                        cm.Parameters.AddWithValue("@ethnicity", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@ethnicity", member.Ethnicity);
                    }
                    if (member.TxId == string.Empty || member.TxId == null)
                    {
                        cm.Parameters.AddWithValue("@tx_id", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@tx_id", member.TxId);
                    }

                    #endregion
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                            {
                                result = reader["Result"].ToString();
                            }
                        }
                    }
                    Logging.NLog.ClassLogger.Trace("Update|FirstName=" + member.FirstName + "LastName=" + member.LastName + "|DB Trace|");
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("Update|rid=" + member.Rid + "|DB Error|" + ex.Message);
                    result = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("Update|rid=" + member.Rid + "|Connection string is empty");
                result = "Rid does not exist";
            }
            return result;
        }
        #endregion

        #region Update WL
        public string UpdateWL(User oUser)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            int languageCode = 0;
            cn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[api].[user_insertion_related_data_get_WL]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", oUser.RefferId);
                if (oUser.CountryId > 0)
                {
                    cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@country_id", DBNull.Value);
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (reader["language_code"] != DBNull.Value)
                        {
                            languageCode = Convert.ToInt32(reader["language_code"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("Update WL| rid=" + oUser.RefferId + "DB Error|" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(oUser.RefferId, null);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_update_wl]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    #region Parameters
                    cm.Parameters.AddWithValue("@user_id", oUser.UserId);
                    cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                    cm.Parameters.AddWithValue("@country_id", oUser.CountryId);
                    cm.Parameters.AddWithValue("@state_id", oUser.StateId);
                    cm.Parameters.AddWithValue("@first_name", oUser.FirstName);
                    cm.Parameters.AddWithValue("@last_name", oUser.LastName);
                    cm.Parameters.AddWithValue("@city", oUser.City);
                    cm.Parameters.AddWithValue("@zip_code", oUser.ZipCode);
                    cm.Parameters.AddWithValue("@gender", oUser.Gender);
                    cm.Parameters.AddWithValue("@dob", oUser.Dob);
                    cm.Parameters.AddWithValue("@ethnicity_id", oUser.EthnicityId);
                    cm.Parameters.AddWithValue("@password", oUser.Password);
                    cm.Parameters.AddWithValue("@update_by", oUser.UpdatedBy);
                    cm.Parameters.AddWithValue("@phone_number", oUser.PhoneNumber);
                    cm.Parameters.AddWithValue("@is_dnc", oUser.IsDnc);
                    cm.Parameters.AddWithValue("@is_ccpa_compliance", oUser.IsCompliance);                                     
                    if (oUser.LanguageId > 0)
                    {
                        cm.Parameters.AddWithValue("@language_code", oUser.LanguageId);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@language_code", DBNull.Value);
                    }
                   
                    if (string.IsNullOrEmpty(oUser.Address1))
                    {
                        cm.Parameters.AddWithValue("@address1", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address1", oUser.Address1);
                    }
                    if (string.IsNullOrEmpty(oUser.Address2))
                    {
                        cm.Parameters.AddWithValue("@address2", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@address2", oUser.Address2);
                    }
                    if (oUser.CampaignID > 0)
                    {
                        cm.Parameters.AddWithValue("@campaign_id", oUser.CampaignID);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@campaign_id", DBNull.Value);
                    }
                    if (string.IsNullOrEmpty(oUser.CustomAttribute))
                    {
                        cm.Parameters.AddWithValue("@custom_attribute", DBNull.Value);
                    }
                    else
                    {
                        cm.Parameters.AddWithValue("@custom_attribute", oUser.CustomAttribute);
                    }
                    #endregion
                    cm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("UpdateWL |UserGuid=" + oUser.UserGuid + "DB Error|" + ex.Message);
                    result = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("UpdateWL |UserGuid=" + oUser.UserGuid + "|Connection string is empty");
            }
            return result;
        }
        #endregion

        #region updateProfile
        public string UpdateProfile(string xml, string ug, int rid)
        {
            int _orgId = -1;
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            string cs = GetConnectionString(rid, null);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[profile_response_save]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@respone_xml", xml);
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                                Response = reader["Result"].ToString();
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                if (reader["org_id"] != DBNull.Value)
                                {
                                    _orgId = Convert.ToInt32(reader["org_id"]);
                                }
                            }
                        }
                        if (_orgId == 62 || _orgId == 83 || _orgId == 163)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(Response);
                            Response = JsonConvert.SerializeObject(doc);
                        }
                        else
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("UpdateProfile|DB Error|UserGuid=" + ug + "|" + ex.Message);
                    Response = "<Message>Profile Responses Saved Successfully</Message>";
                    if (_orgId == 62 || _orgId == 83 || _orgId == 163)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Response);
                        Response = JsonConvert.SerializeObject(doc);
                    }
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("UpdateProfile|UserGuid=" + ug + "|Connection string is empty");
                Response = "UserGuid does not exist";
            }
            return Response;
        }
        #endregion

        #region resubscribe
        public string Resubscribe(Guid memberGuid, int ClientId)
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            string cs = GetConnectionString(null, ClientId);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_resubscribe]", cn);
                    cm.CommandType = CommandType.StoredProcedure;

                    cm.Parameters.AddWithValue("@user_guid", memberGuid);

                    cm.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["Result"] != DBNull.Value)
                                Response = dr["Result"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("Resubscribe|DB Error|" + ex.Message);
                    Response = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("Resubscribe|UserGuid=" + memberGuid + "|Connection string is empty");
                Response = "UserGuid does not exist";
            }
            return Response;
        }
        #endregion

        #region unsubscribe
        public string Unsubscribe(int? Rid, string UserName)
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            string cs = GetConnectionString(Convert.ToInt32(Rid), null);

            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_unsubscribe]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@referrer_id", Rid);
                    cm.Parameters.AddWithValue("@user_name", UserName);
                    cm.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["Result"] != DBNull.Value)
                                Response = dr["Result"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("Unsubscribe|Rid=" + Rid + "UserName=" + UserName + "|DB Error|" + ex.Message);
                    Response = "<Message> This account was Unsubscribed successfully.</Message>";
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("Unsubscribe|Rid=" + Rid + "|Connection string is empty");
                Response = "<Message> Rid does not exist </Message>";
            }
            return Response;
        }
        #endregion

        #region GetProfile
        public List<Profile> GetPartnerUserProfilesList(string memeberGuid, int ClientId)
        {
            SqlConnection conn = new SqlConnection();
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            List<Profile> lstprofile = new List<Profile>();
            int _orgid = -1;
            string cs = GetConnectionString(null, ClientId);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    conn.ConnectionString = cs;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("[api].[profiles_get]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@user_guid", new Guid(memeberGuid));
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Profile objProfile = new Profile();
                            if (reader["ProfileId"] != DBNull.Value)
                            {
                                objProfile.ProfileId = reader["ProfileId"].ToString();
                            }
                            if (reader["ProfileName"] != DBNull.Value)
                            {
                                objProfile.ProfileName = reader["ProfileName"].ToString();
                            }
                            if (reader["ProfileUrl"] != DBNull.Value)
                            {
                                objProfile.ProfileUrl = reader["ProfileUrl"].ToString();
                            }
                            if (reader["ProfileStatus"] != DBNull.Value)
                            {
                                objProfile.ProfileStatus = reader["ProfileStatus"].ToString();
                                if (objProfile.ProfileStatus == "Complete")
                                {
                                    objProfile.IsSelected = true;
                                }
                                else
                                {
                                    objProfile.IsSelected = false;
                                }
                            }
                            lstprofile.Add(objProfile);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetProfiles|DB Error|" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetProfiles|UserGuid=" + memeberGuid + "|Connection string is empty");
            }
            return lstprofile;
        }
        #endregion

        #region GetSurveys
        public List<Surveys> GetSurveys(string memeberGuid, int? ClientId)
        {
            SqlConnection conn = new SqlConnection();
            string result = string.Empty;
            List<Surveys> lstSurveys = new List<Surveys>();
            string cs = GetConnectionString(null, ClientId);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    conn.ConnectionString = cs;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("[api].[surveys_get]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                    #region Parameters
                    cmd.Parameters.AddWithValue("@user_guid", new Guid(memeberGuid));
                    #endregion
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Surveys objSurvey = new Surveys();
                            if (reader["message"] != DBNull.Value)
                            {
                                objSurvey.Message = reader["message"].ToString();
                            }
                            if (reader["org_id"] != DBNull.Value)
                            {
                                objSurvey.OrgId = Convert.ToInt32(reader["org_id"].ToString());
                            }
                            if (reader["priority_id"] != DBNull.Value)
                            {
                                objSurvey.PriorityId = Convert.ToInt32(reader["priority_id"].ToString());
                            }
                            if (reader["project_id"] != DBNull.Value)
                            {
                                objSurvey.ProjectId = Convert.ToInt32(reader["project_id"].ToString());
                            }
                            if (reader["target_guid"] != DBNull.Value)
                            {
                                objSurvey.TargetGuid = reader["target_guid"].ToString();
                            }
                            if (reader["survey_name"] != DBNull.Value)
                            {
                                objSurvey.SurveyName = reader["survey_name"].ToString();
                            }
                            if (reader["survey_length"] != DBNull.Value)
                            {
                                objSurvey.SurveyLength = Convert.ToInt32(reader["survey_length"].ToString());
                            }
                            if (reader["partner_revenue_share"] != DBNull.Value)
                            {
                                objSurvey.PartnerRevenueShare = Convert.ToDecimal(reader["partner_revenue_share"].ToString());
                            }
                            if (reader["member_reward"] != DBNull.Value)
                            {
                                objSurvey.MemberReward = Convert.ToDecimal(reader["member_reward"].ToString());
                            }
                            if (reader["member_reward_points"] != DBNull.Value)
                            {
                                objSurvey.MemberRewardPoints = Convert.ToDecimal(reader["member_reward_points"].ToString());
                            }
                            if (reader["reward_text"] != DBNull.Value)
                            {
                                objSurvey.RewardText = reader["reward_text"].ToString();
                            }
                            if (reader["survey_user_type_ids"] != DBNull.Value)
                            {
                                objSurvey.SurveyUserTypeIds = reader["survey_user_type_ids"].ToString();
                            }
                            if (reader["survey_url"] != DBNull.Value)
                            {
                                objSurvey.SurveyUrl = reader["survey_url"].ToString();
                            }
                            if (reader["ir"] != DBNull.Value)
                            {
                                objSurvey.Ir = Convert.ToInt32(reader["ir"].ToString());
                            }
                            lstSurveys.Add(objSurvey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetSurveys|UserGuid=" + memeberGuid + "ClientId=" + ClientId + "|DB Error|" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                //if (_orgId == 62 || _orgId == 83 || _orgId == 163) //We are returning Json Object as Response for Juno Wallet and Mv
                //{
                //    if (result == "No Surveys matched your profile.")
                //    {
                //        result = JsonConvert.SerializeObject(result);
                //    }
                //    else
                //    {
                //        XmlDocument doc = new XmlDocument();
                //        doc.LoadXml(result);
                //        result = JsonConvert.SerializeObject(doc);
                //    }
                //}
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetSurveys|UserGuid=" + memeberGuid + "ClientId=" + ClientId + "|Connection string is empty");

            }
            return lstSurveys;
        }
        #endregion


        #region Get Surveys for API Partners.
        #region GetSurveys
        public List<ApiSurveys> GetSurveysforAPIPartnersOnly(string memeberGuid, int? ClientId)
        {
            SqlConnection conn = new SqlConnection();
            string result = string.Empty;
            List<ApiSurveys> lstSurveys = new List<ApiSurveys>();
            string cs = GetConnectionString(null, ClientId);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    conn.ConnectionString = cs;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("[api].[surveys_get]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    #region Parameters

                    cmd.Parameters.AddWithValue("@user_guid", new Guid(memeberGuid));
                    if (ClientId == 63)
                        cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SwagCommandTimeout"]);
                    else if (ClientId == 207)
                        cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["TheoremCommandTimeout"]);
                    else
                        cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["GenCommandTimeout"]);

                    #endregion
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //SurveyName, 
                            //SurveyUrl, 
                            //SurveyLength, 
                            //RewardValue, 
                            //RewardPoints, 
                            //GrossRevenue, 
                            //ProjectId, 
                            //SurveyTrafficType, 
                            //IR
                            ApiSurveys objSurvey = new ApiSurveys();
                            if (reader["survey_name"] != DBNull.Value)
                            {
                                objSurvey.SurveyName = reader["survey_name"].ToString();
                            }
                            if (reader["survey_url"] != DBNull.Value)
                            {
                                objSurvey.SurveyUrl = reader["survey_url"].ToString();
                            }
                            if (reader["survey_length"] != DBNull.Value)
                            {
                                objSurvey.SurveyLength = Convert.ToInt32(reader["survey_length"].ToString());
                            }
                            if (reader["member_reward"] != DBNull.Value)
                            {
                                objSurvey.RewardValue = Convert.ToDecimal(reader["member_reward"].ToString());
                            }
                            if (reader["member_reward_points"] != DBNull.Value)
                            {
                                objSurvey.RewardPoints = Convert.ToDecimal(reader["member_reward_points"].ToString());
                            }
                            if (reader["partner_revenue_share"] != DBNull.Value)
                            {
                                objSurvey.GrossRevenue = Convert.ToDecimal(reader["partner_revenue_share"].ToString());
                            }
                            if (reader["project_id"] != DBNull.Value)
                            {
                                objSurvey.ProjectId = Convert.ToInt32(reader["project_id"].ToString());
                            }
                            if (reader["survey_user_type_ids"] != DBNull.Value)
                            {
                                objSurvey.SurveyTrafficType = reader["survey_user_type_ids"].ToString();
                            }
                            if (reader["ir"] != DBNull.Value)
                            {
                                objSurvey.IR = Convert.ToInt32(reader["ir"].ToString());
                            }

                            if (reader["VerityCheckRequired"] != DBNull.Value)
                            {
                                objSurvey.VerityCheckRequired = reader["VerityCheckRequired"].ToString();
                            }
                            lstSurveys.Add(objSurvey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetSurveysAPI|UserGuid=" + memeberGuid + "ClientId=" + ClientId + "|DB Error|" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetSurveys|UserGuid=" + memeberGuid + "ClientId=" + ClientId + "|Connection string is empty");
            }
            return lstSurveys;
        }
        #endregion
        #endregion

        #region GetSurveyHistory
        public List<SurveyHistory> GetSurveyHistory(string memeberGuid, int ClientId)
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            List<SurveyHistory> lstSurveyHistory = new List<SurveyHistory>();
            string cs = GetConnectionString(null, ClientId);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[surveyshistory_get]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@user_guid", new Guid(memeberGuid));

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SurveyHistory objSurveyHistory = new SurveyHistory();
                            if (reader["org_id"] != DBNull.Value)
                            {
                                objSurveyHistory.OrgId = Convert.ToInt32(reader["org_id"].ToString());
                            }
                            if (reader["user_id"] != DBNull.Value)
                            {
                                objSurveyHistory.UserId = Convert.ToInt32(reader["user_id"].ToString());
                            }
                            if (reader["ext_member_id"] != DBNull.Value)
                            {
                                objSurveyHistory.ExtId = reader["ext_member_id"].ToString();
                            }
                            if (reader["user_guid"] != DBNull.Value)
                            {
                                objSurveyHistory.UserGuid = reader["user_guid"].ToString();
                            }
                            if (reader["survey_name"] != DBNull.Value)
                            {
                                objSurveyHistory.SurveyName = reader["survey_name"].ToString();
                            }
                            if (reader["survey_id"] != DBNull.Value)
                            {
                                objSurveyHistory.SurveyId = Convert.ToInt32(reader["survey_id"].ToString());
                            }
                            if (reader["survey_status"] != DBNull.Value)
                            {
                                objSurveyHistory.SurveyStatus = reader["survey_status"].ToString();
                                objSurveyHistory.SurveyStatus = objSurveyHistory.SurveyStatus.TrimEnd();
                            }
                            if (reader["survey_reward"] != DBNull.Value)
                            {
                                objSurveyHistory.SurveyReward = Convert.ToDecimal(reader["survey_reward"].ToString());
                            }
                            if (reader["click_dt"] != DBNull.Value)
                            {
                                objSurveyHistory.ClickDt = Convert.ToDateTime(reader["click_dt"]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (reader["prelim_status_dt"] != DBNull.Value)
                            {
                                objSurveyHistory.PrelimDt = Convert.ToDateTime(reader["prelim_status_dt"]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            lstSurveyHistory.Add(objSurveyHistory);
                        }
                    }

                }

                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetSurveyHistory|UserGuid=" + memeberGuid + "|DB Error|" + ex.Message);
                    Response = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetSurveyHistory|UserGuid=" + memeberGuid + "|Connection string is empty");
            }

            //if (_orgId == 62 || _orgId == 83 || _orgId == 163) //We are returning Json Object as Response for Juno Wallet
            //{
            //    //XmlDocument doc = new XmlDocument();
            //    //doc.LoadXml(Response);
            //    //Response = JsonConvert.SerializeObject(doc);
            //}

            return lstSurveyHistory;
        }
        #endregion

        #region Data Fetch - Reward History

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="clientId"></param>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public Rewards GetRewardsHistory(int userId, int clientId, Guid userGuid)
        {
            Rewards oRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(null, clientId);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[rms].[reward_history_get_v1]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@user_id", userId);
                    cm.Parameters.AddWithValue("@org_id", clientId);
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["account_balance"] != DBNull.Value)
                            {
                                oRewards.AccountBalance = Convert.ToDecimal(reader["account_balance"]);
                            }
                            if (reader["total_earnings"] != DBNull.Value)
                            {
                                oRewards.TotalEarnings = Convert.ToDecimal(reader["total_earnings"]);
                            }
                            if (reader["total_redemptions"] != DBNull.Value)
                            {
                                oRewards.TotalRedemptions = Convert.ToDecimal(reader["total_redemptions"]);
                            }
                            if (reader["account_balance_local_currency"] != DBNull.Value)
                            {
                                oRewards.AccountBalanceLocalCurrency = Convert.ToDecimal(reader["account_balance_local_currency"]);
                            }
                            if (reader["total_earnings_local_currency"] != DBNull.Value)
                            {
                                oRewards.TotalEarningsLocalCurrency = Convert.ToDecimal(reader["total_earnings_local_currency"]);
                            }
                            if (reader["total_redemptions_local_currency"] != DBNull.Value)
                            {
                                oRewards.TotalRedemptionsLocalCurrency = Convert.ToDecimal(reader["total_redemptions_local_currency"]);
                            }
                            if (reader["currency_notation"] != DBNull.Value)
                            {
                                oRewards.CurrencyNotation = Convert.ToString(reader["currency_notation"]);
                            }
                            if (reader["reward_text"] != DBNull.Value)
                            {
                                oRewards.RewardText = Convert.ToString(reader["reward_text"]);
                            }
                            if (reader["euro_to_local_currency"] != DBNull.Value)
                            {
                                oRewards.LocalCurrency = Convert.ToDecimal(reader["euro_to_local_currency"]);
                            }
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            RewardHistory oRewardHistory = new RewardHistory();
                            if (reader["create_dt"] != DBNull.Value)
                            {
                                oRewardHistory.CreateDt = Convert.ToDateTime(reader["create_dt"]).ToString();
                            }
                            if (reader["description"] != DBNull.Value)
                            {
                                oRewardHistory.Descripion = Convert.ToString(reader["description"]);
                            }

                            if (reader["direct_reward"] != DBNull.Value)
                            {
                                oRewardHistory.RewardAmount = Convert.ToDecimal(reader["direct_reward"]);
                            }
                            if (reader["redemption_amount"] != DBNull.Value)
                            {
                                oRewardHistory.RedemptionAmount = Convert.ToDecimal(reader["redemption_amount"]);
                            }
                            if (reader["balance_amount"] != DBNull.Value)
                            {
                                oRewardHistory.BalanceAmount = Convert.ToDecimal(reader["balance_amount"]);
                            }
                            if (reader["survey_status"] != DBNull.Value)
                            {
                                oRewardHistory.Status = Convert.ToString(reader["survey_status"]);
                            }
                            oRewards.LstRewardHistory.Add(oRewardHistory);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetRewardHistory|UserGuid=" + userGuid + "|DB Error|" + ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetRewardHistory|UserGuid=" + userGuid + "|Connection string is empty");
            }
            return oRewards;
        }

        #endregion

        #region Data Fetch - Redeem History

        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public Rewards GetRedeemHistory(int userId, int clientId, Guid userGuid)
        {
            Rewards oRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = GetConnectionString(null, clientId);
            if (!string.IsNullOrEmpty(cn.ConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[rms].[redemptionhistoryformember]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@user_guid", userGuid);
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RewardsRedeemedHistory oRewardRedeemHistory = new RewardsRedeemedHistory();
                            if (reader["redeemption_id"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.RedeemptionId = Convert.ToInt32(reader["redeemption_id"]);
                            }
                            if (reader["user_id"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.UserId = Convert.ToInt32(reader["user_id"]);
                            }
                            if (reader["redemption_dt"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.CreateDt = Convert.ToDateTime(reader["redemption_dt"]).ToString();
                            }
                            if (reader["catalouge_name"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.RedemptionType = Convert.ToString(reader["catalouge_name"]);
                            }
                            if (reader["redemption_amount"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.RedemptionAmount = Convert.ToDecimal(reader["redemption_amount"]);
                            }
                            if (reader["giftcard_name"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.GiftcardName = Convert.ToString(reader["giftcard_name"]);
                            }
                            if (reader["codes"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.Descripion = Convert.ToString(reader["codes"]);
                            }
                            if (reader["api_response"] != DBNull.Value)
                            {
                                oRewardRedeemHistory.ApiResponse = Convert.ToString(reader["api_response"]);
                            }
                            oRewards.LstRedeemptionHistory.Add(oRewardRedeemHistory);
                        }


                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("GetRedeemHistory|UserGuid=" + userGuid + "|DB Error|" + ex.Message);

                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("GetRedeemHistory|UserGuid=" + userGuid + "|Connection string is empty");
            }
            return oRewards;
        }

        #endregion

        #region User Login
        public User UserLogin(User oUser)
        {
            string _connvalue = string.Empty;
            string result = string.Empty;
            string Response = string.Empty;
            int OrgId = 0;
            SqlConnection cn = new SqlConnection();
            result = UserLoginGetConnectionString(oUser.DomainUrl);
            String[] substrings = result.Split(';');
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith(substrings[0]))
                { _connvalue = ConfigurationManager.AppSettings[key]; break; }
            }
            cn.ConnectionString = _connvalue;
            OrgId = Convert.ToInt32(substrings[1]);
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_login]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                cm.Parameters.AddWithValue("@password", oUser.Password);
                cm.Parameters.AddWithValue("@org_id", OrgId);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_id"] != DBNull.Value)
                        {
                            oUser.UserId = Convert.ToInt32(dr["user_id"].ToString());
                        }
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            oUser.UserGuid = new Guid(dr["user_guid"].ToString());
                        }
                        if (dr["is_active"] != DBNull.Value)
                        {
                            oUser.IsActive = Convert.ToBoolean(dr["is_active"].ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("UserLogin|EmailAddress=" + oUser.EmailAddress + "|DB Error|" + ex.Message);

            }
            return oUser;
        }
        #endregion

        #region GetConnection string based on RId
        internal string GetConnectionString(int? Rid = null, int? ClientId = null)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                if (Rid != null)
                {
                    cm.Parameters.AddWithValue("@referrer_id", Rid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@referrer_id", DBNull.Value);
                }
                if (ClientId != null)
                {
                    cm.Parameters.AddWithValue("@client_id", ClientId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@client_id", DBNull.Value);
                }
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["s_name"] != DBNull.Value)
                        {
                            result = ConfigurationManager.AppSettings[dr["s_name"].ToString()];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("GetConnectionString|DB |Error|" + ex.Message);

            }
            finally
            {
                cn.Close();
            }
            return result;
        }


        internal string UserLoginGetConnectionString(string DomainUrl = "")
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                if (!string.IsNullOrEmpty(DomainUrl))
                {
                    cm.Parameters.AddWithValue("@domain_url", DomainUrl);
                }
                else
                {
                    cm.Parameters.AddWithValue("@domain_url", DBNull.Value);
                }
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["s_name"] != DBNull.Value)
                            Response = dr["s_name"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("UserLoginGetConnectionString|DB Error|" + ex.Message);

            }
            finally
            {
                cn.Close();
            }
            return Response;
        }
        #endregion
        public string IsProjectOpen(int ProjectId)
        {
            SqlConnection conn = new SqlConnection();
            User objuser = new User();
            string Result = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<Result></Result>");
            try
            {
                using (conn)
                {
                    conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                    SqlCommand oSqlCommand = new SqlCommand("[pms].[project_status_get]", conn);
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    oSqlCommand.Parameters.AddWithValue("@project_id", ProjectId);

                    using (IDataReader reader = oSqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sb = new StringBuilder();
                            if (reader["status"] != DBNull.Value)
                            {
                                Result = reader["status"].ToString();
                            }

                            sb.Append("<Result>" + Result + "</Result>");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("IsProjectOpen|ProjectId=" + ProjectId + "|DB Error|" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return sb.ToString();
        }

        public string GetProjectsClosedToday()
        {
            SqlConnection conn = new SqlConnection();
            User objuser = new User();
            string Result = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                using (conn)
                {
                    conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                    SqlCommand oSqlCommand = new SqlCommand("[pms].[closedProject]", conn);
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (IDataReader reader = oSqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (reader["result"] != DBNull.Value)
                            {
                                Result = reader["result"].ToString();
                            }

                            sb.Append("<Result>" + Result + "</Result>");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("GetProjectsClosedToday |DB Error|" + ex.Message);
                //Result = "Time Out";
                sb.Append("<Result></Result>");
            }
            finally
            {
                conn.Close();
            }

            return sb.ToString();
        }

        //I thnink we need to re-enginner this method.
        public string GetMembers(int ProjectId, Guid ApiKey)
        {
            SqlConnection conn = new SqlConnection();
            User objuser = new User();
            string Result = "";

            return Result; //currently no fucntionality is decided, we need to confirm with Partha on this.
            try
            {
                using (conn)
                {
                    conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                    SqlCommand oSqlCommand = new SqlCommand("[api].[get_members_per_project]", conn);
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    oSqlCommand.Parameters.AddWithValue("@client_guid", ApiKey);
                    oSqlCommand.Parameters.AddWithValue("@project_id", ProjectId);

                    using (IDataReader reader = oSqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            if (reader["result"] != DBNull.Value)
                            {
                                Result = reader["result"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("GetMembers|ApiKey=" + ApiKey + "ProjectId=" + ProjectId + " |DB Error|" + ex.Message);
                Result = ex.Message;
            }
            finally
            {
                conn.Close();

            }
            return Result;
        }


        #region updateProfile2
        public string UpdateProfile2(string Json, Guid ug, int rid)
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            string cs = GetConnectionString(rid, null);
            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[profile_json_response_save]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@json", Json);
                    cm.Parameters.AddWithValue("@user_guid", ug);
                    cm.Parameters.AddWithValue("@rid", rid);
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                                Response = reader["Result"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("UpdateProfile2|DB Error|UserGuid=" + ug + "|" + Json + "|" + ex.Message);
                    Response = ex.Message;
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("UpdateProfile2|UserGuid=" + ug + "|" + Json + "|Connection string is empty");
                Response = "UserGuid does not exist";
            }
            return Response;
        }
        #endregion

        #region Delete
        public string Delete(int Rid, string ExtMemberId, Guid UserGuid)
        {
            string Response = string.Empty;
            SqlConnection cn = new SqlConnection();
            string cs = GetConnectionString(Rid, null);

            if (!string.IsNullOrEmpty(cs))
            {
                try
                {
                    cn.ConnectionString = cs;
                    cn.Open();
                    SqlCommand cm = new SqlCommand("[api].[user_delete]", cn);
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.AddWithValue("@referrer_id", Rid);
                    cm.Parameters.AddWithValue("@user_guid", UserGuid);
                    cm.Parameters.AddWithValue("@ext_member_id", ExtMemberId);
                    cm.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Result"] != DBNull.Value)
                                Response = reader["Result"].ToString();
                            if (Response == "0")
                            {
                                Response = "Member doesn't exist with this details.";
                            }
                            else
                            {
                                Response = "This account was Deleted successfully.";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.NLog.ClassLogger.Error("Delete|Rid=" + Rid + "user_guid=" + UserGuid + "Ext_MemberID=" + ExtMemberId + "|DB Error|" + ex.Message);
                    Response = "This account was Deleted successfully.";
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                Logging.NLog.ClassLogger.Error("Delete|Rid=" + Rid + "user_guid=" + UserGuid + "Ext_MemberID=" + ExtMemberId + "|Connection string is empty");
                Response = "Rid does not exist";
            }
            return Response;
        }
        #endregion

        #region Get Questions

        public List<Questions> GetQuestions(int QuestionId)
        {
            SqlConnection conn = new SqlConnection();
            StringBuilder sb = new StringBuilder();
            string Result = string.Empty;
            List<Questions> lstquestion = new List<Questions>();
            //sb.Append("<Result></Result>");
            try
            {
                using (conn)
                {
                    conn.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                    SqlCommand oSqlCommand = new SqlCommand("[api].[questions_get]", conn);
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    oSqlCommand.Parameters.AddWithValue("@question_id", QuestionId);
                    using (IDataReader reader = oSqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Questions objQuestion = new Questions();
                            if (reader["question_id"] != DBNull.Value)
                            {
                                objQuestion.QuestionId = Convert.ToInt32(reader["question_id"].ToString());
                            }
                            if (reader["question_text"] != DBNull.Value)
                            {
                                objQuestion.QuestionText = reader["question_text"].ToString();
                            }
                            if (reader["option_id"] != DBNull.Value)
                            {
                                objQuestion.OptionId = Convert.ToInt32(reader["option_id"].ToString());
                            }
                            if (reader["option_text"] != DBNull.Value)
                            {
                                objQuestion.OptionText = reader["option_text"].ToString();
                            }
                            if (reader["profile_name"] != DBNull.Value)
                            {
                                objQuestion.ProfileName = reader["profile_name"].ToString();
                            }
                            if (reader["question_type_name"] != DBNull.Value)
                            {
                                objQuestion.QuestionTypeName = reader["question_type_name"].ToString();
                            }
                            lstquestion.Add(objQuestion);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logging.NLog.ClassLogger.Error("GetQuestions|QuestionId=" + QuestionId + "|DB Error|" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return lstquestion;
        }
        #endregion
    }
}
