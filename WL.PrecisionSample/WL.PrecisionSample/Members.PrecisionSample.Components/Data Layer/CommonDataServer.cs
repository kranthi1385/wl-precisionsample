using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Members.PrecisionSample.Components.Business_Layer;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class CommonDataServer
    {
        #region ConnectionString
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }

        #endregion

        #region Get Ethinicity List
        /// <summary>
        /// Get All Avaliable Ethnicity Types
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Ethnicity> GetEthinicity(string LanguageCode)
        {
            List<Ethnicity> lstEthinicty = new List<Ethnicity>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[ethnicity_list_get_sdl]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@language_name", LanguageCode);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Ethnicity oEthinicty = new Ethnicity();
                        if (dr["ethnicity_id"] != DBNull.Value)
                        {
                            oEthinicty.EthnicityId = Convert.ToInt32(dr["ethnicity_id"]);
                        }

                        if (dr["ethnicity_type"] != DBNull.Value)
                        {
                            oEthinicty.EthnicityType = Convert.ToString(dr["ethnicity_type"]);
                        }

                        lstEthinicty.Add(oEthinicty);
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
            return lstEthinicty;
        }
        #endregion

        #region Get Ethinicity List
        /// <summary>
        /// Get All Avaliable Ethnicity Types
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Options> GetLanguageList()
        {
            List<Options> lstOptions = new List<Options>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[language_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Options objOptions = new Options();
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        lstOptions.Add(objOptions);
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
            return lstOptions;
        }
        #endregion

        #region Data Fetch - States
        /// <summary>
        /// Get Country wise states
        /// </summary>
        /// <param name="countryId">CountryId</param>
        /// <returns></returns>
        public List<States> GetStatesList(int countryId, string LanguageCode)
        {
            List<States> lstStates = new List<States>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[statenames_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@country_ids", countryId);
                cm.Parameters.AddWithValue("@language_name", LanguageCode);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        States oStates = new States();
                        if (dr["state_id"] != DBNull.Value)
                        {
                            oStates.StateId = Convert.ToInt32(dr["state_id"]);
                        }

                        if (dr["state_name"] != DBNull.Value)
                        {
                            oStates.StateName = Convert.ToString(dr["state_name"]);
                        }

                        lstStates.Add(oStates);
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
            return lstStates;
        }

        #endregion

        #region getladingpage
        /// <summary>
        /// GetLanding pageUrl
        /// </summary>
        /// <param name="referrer_id"></param>
        /// <returns></returns>
        public string GetLandingpageUrl(int referrer_id)
        {
            string url = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[referrer].[referrerlandingpage_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", referrer_id);
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["landing_url"] != DBNull.Value)
                        {
                            url = dr["landing_url"].ToString();
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
            return url;
        }
        #endregion

        #region Get Country & States List
        /// <summary>
        /// GetCountrysAndStates
        /// </summary>
        /// <returns></returns>

        public CountryAndState GetCountrysAndStates(int ClientId)
        {
            CountryAndState oCountryAndState = new CountryAndState();
            List<Country> oCountryList = new List<Country>();
            List<States> oStatesList = new List<States>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[country_states_list_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                if (ClientId != 0)
                {
                    cm.Parameters.AddWithValue("org_id", ClientId);
                }
                else
                {
                    cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        Country oCuntry = new Country();
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oCuntry.CountryId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            oCuntry.CountryCode = Convert.ToString(dr["country_code"].ToString());
                        }
                        if (dr["country_name"] != DBNull.Value)
                        {
                            oCuntry.CountryName = Convert.ToString(dr["country_name"]);
                        }
                        if (dr["code"] != DBNull.Value)
                        {
                            oCuntry.Code = Convert.ToString(dr["code"].ToString());
                        }
                        if (dr["country_namefor_partner"] != DBNull.Value)
                        {
                            oCuntry.CountryNameforPartner = Convert.ToString(dr["country_namefor_partner"].ToString());
                        }
                        oCountryList.Add(oCuntry);
                    }
                    oCountryAndState.CountryList = oCountryList;
                    dr.NextResult();
                    while (dr.Read())
                    {
                        States oStates = new States();

                        if (dr["state_id"] != DBNull.Value)
                        {
                            oStates.StateId = Convert.ToInt32(dr["state_id"]);
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oStates.CountryId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["state_code"] != DBNull.Value)
                        {
                            oStates.StateCode = Convert.ToString(dr["state_code"]);
                        }
                        if (dr["state_name"] != DBNull.Value)
                        {
                            oStates.StateName = Convert.ToString(dr["state_name"]);
                        }
                        oStatesList.Add(oStates);
                    }

                    oCountryAndState.StateList = oStatesList;
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
            return oCountryAndState;
        }
        #endregion

        #region Update Language Code
        /// <summary>
        /// Update Language Code
        /// </summary>
        /// <param name="LanguageText">Language Text</param>
        public void UpdateLanguageCode(User oUser, string LanguageText, string RequestUrl)
        {
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(RequestUrl);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("api.update_language", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", oUser.UserId);
                cmd.Parameters.AddWithValue("@language_translation_text", LanguageText);
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

        #region Get Languages
        /// <summary>
        /// Get Languages
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Language> GetObLang()
        {
            List<Language> lstLanguage = new List<Language>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[ob_lang_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Language oLanguage = new Language();
                        if (dr["language_id"] != DBNull.Value)
                        {
                            oLanguage.LanguageId = Convert.ToInt32(dr["language_id"]);
                        }

                        if (dr["language_name"] != DBNull.Value)
                        {
                            oLanguage.LanguageName = Convert.ToString(dr["language_name"]);
                        }

                        lstLanguage.Add(oLanguage);
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
            return lstLanguage;
        }
        #endregion
    }
}
