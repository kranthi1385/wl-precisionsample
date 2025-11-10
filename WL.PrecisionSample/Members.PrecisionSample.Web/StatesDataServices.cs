using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Web;

namespace Members.PrecisionSample.Components.Data_Layer
{
    class StatesDataServices
    {
        #region Data Fetch - States
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<States> GetStatesList(int countryId)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[geo].[StateName_Get]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            //Added by sandy on 06/19/2015 
            string language_name = "english"; //By default to English.
            if (Members.PrecisionSample.Components.Business_Layer.MemberIdentity.Client.ClientId == 111)
            {
                if (HttpContext.Current.Session["LQLanguageName"] != null)
                {
                    language_name = HttpContext.Current.Session["LQLanguageName"].ToString();
                }
                else
                {
                    //if (Identity.Current != null)
                    //{
                    //    language_name = Identity.Current.UserData.UserLanguage;
                    //}

                }
            }

            #region Adding params for the SQL Proc

            cm.Parameters.AddWithValue("@country_id",countryId);
            cm.Parameters.AddWithValue("@language_name",language_name);

            //db.AddInParameter(dbCommand, "org_id", DbType.Int32, -1);

            #endregion

            using (IDataReader reader = cm.ExecuteReader())
            {
                List<States> lstStates = new List<States>();
                while (reader.Read())
                {
                    States OStates = new States();
                    OStates.StateId = Convert.ToInt32(reader["state_id"]);
                    OStates.StateName = Convert.ToString((reader["state_name"]));
                    if (reader["state_code"] != DBNull.Value)
                    {
                        OStates.StateCode = Convert.ToString(reader["state_code"]);
                    }
                    else
                    {
                    }
                    lstStates.Add(OStates);
                }
                return lstStates;
            }
        }

        #endregion

        #region Data Fetch - States
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<States> GetStatesLists(string strCountryIds)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[geo].[StateNames_Get]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            #region Adding params for the SQL Proc

            cm.Parameters.AddWithValue("@country_ids",strCountryIds);
            // db.AddInParameter(dbCommand, "org_id", DbType.Int32, -1);

            #endregion

            using (IDataReader reader = cm.ExecuteReader())
            {
                List<States> lstStates = new List<States>();
                while (reader.Read())
                {
                    States OStates = new States();
                    OStates.StateId = Convert.ToInt32(reader["state_id"]);
                    OStates.StateName = Convert.ToString((reader["state_name"]));
                    lstStates.Add(OStates);
                }
                return lstStates;
            }
        }

        #endregion

        #region Data Fetch - Countries
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<States> GetCountriesList()
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[geo].[CountryList_Get]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            using (IDataReader reader = cm.ExecuteReader())
            {
                List<States> lstStates = new List<States>();
                while (reader.Read())
                {
                    States OStates = new States();
                    OStates.CountryId = Convert.ToInt32(reader["country_id"]);
                    OStates.CountryName = Convert.ToString((reader["country_name"]));
                    lstStates.Add(OStates);
                }
                return lstStates;
            }
        }
        #endregion
    }
}
