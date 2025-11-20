using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Members.OpinionBar.Components.Data_Layer
{
    public class CommonDataServer
    {
        #region ConnectionString
        /// <summary>
        /// Connectionstring
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        #endregion
        #region Get Country & States List
        /// <summary>
        /// GetCountrysAndStates
        /// </summary>
        /// <returns></returns>

        public CountryAndStates GetCountrysAndStates()
        {
            CountryAndStates oCountryAndState = new CountryAndStates();
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
                cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        Country oCuntry = new Country();
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oCuntry.CId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            oCuntry.CC = Convert.ToString(dr["country_code"].ToString());
                        }
                        if (dr["country_name"] != DBNull.Value)
                        {
                            oCuntry.CN = Convert.ToString(dr["country_name"]);
                        }
                        if (dr["code"] != DBNull.Value)
                        {
                            oCuntry.C = Convert.ToString(dr["code"].ToString());
                        }
                        if (dr["country_namefor_partner"] != DBNull.Value)
                        {
                            oCuntry.CNP = Convert.ToString(dr["country_namefor_partner"].ToString());
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
                            oStates.SId = Convert.ToInt32(dr["state_id"]);
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oStates.CId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["state_code"] != DBNull.Value)
                        {
                            oStates.SC = Convert.ToString(dr["state_code"]);
                        }
                        if (dr["state_name"] != DBNull.Value)
                        {
                            oStates.SN = Convert.ToString(dr["state_name"]);
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
                SqlCommand cm = new SqlCommand("[lookup].[ethnicity_list_get]", cn);
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

        #region Get Continents and Countries List
        /// <summary>
        /// Get Continents and Countries List
        /// </summary>
        /// <returns></returns>
        public List<Continent> GetContientList()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[continents_countries_list_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    List<Continent> lstContinents = new List<Continent>();
                    List<Country> lstCountries = new List<Country>();
                    while (dr.Read())
                    {
                        Continent objContient = new Continent();
                        if (dr["continent_name"] != DBNull.Value)
                        {
                            objContient.ContinentName = Convert.ToString(dr["continent_name"].ToString());
                        }
                        lstContinents.Add(objContient);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        Country objCountry = new Country();
                        if (dr["continent_name"] != DBNull.Value)
                        {
                            objCountry.ContinentName = Convert.ToString(dr["continent_name"].ToString());
                        }
                        if (dr["country_name"] != DBNull.Value)
                        {
                            objCountry.CN = Convert.ToString(dr["country_name"].ToString());
                        }
                        if (dr["language_code"] != DBNull.Value)
                        {
                            objCountry.LC = Convert.ToString(dr["language_code"].ToString());
                        }
                        lstCountries.Add(objCountry);
                    }
                    foreach (Continent objContinent in lstContinents)
                    {
                        List<Country> lstContinentByCountries = new List<Country>();
                        foreach (Country objCountry in lstCountries)
                        {
                            if (objContinent.ContinentName.ToLower() == objCountry.ContinentName.ToLower())
                            {
                                lstContinentByCountries.Add(objCountry);

                            }
                        }
                        if (lstContinentByCountries.Count > 0)
                        {
                            objContinent.LstCountries = lstContinentByCountries;
                        }

                    }
                    return lstContinents;
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

        }
        #endregion

        #region Update Language Code
        /// <summary>
        /// Update Language Code
        /// </summary>
        /// <param name="LanguageText">Language Text</param>
        public string UpdateLanguageCode(User oUser, string LanguageText, string RequestUrl)
        {
            string lang = string.Empty;
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
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["language_translation_text"] != DBNull.Value)
                        {
                            lang = (reader["language_translation_text"].ToString());
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
    }
}
