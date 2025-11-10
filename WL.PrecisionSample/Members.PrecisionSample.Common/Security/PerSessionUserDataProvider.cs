using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace Members.PrecisionSample.Common.Security
{
    public class PerSessionUserDataProvider : UserDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private const string _userDataSessionKey = "PerSessionUserDataProvider:UserData";

        /// <summary>
        /// 
        /// </summary>
        private PerSessionUserDataProvider() : base() { }

        public static readonly PerSessionUserDataProvider Instance = new PerSessionUserDataProvider();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override UserData Retrieve(Guid userID)
        {
            //  see if it is in session
            UserData userData = HttpContext.Current.Session[PerSessionUserDataProvider._userDataSessionKey] as UserData;
            if (userData == null)
            {
                userData = new UserData(GetUserIdentity(userID));
                HttpContext.Current.Session[PerSessionUserDataProvider._userDataSessionKey] = userData;
            }

            return userData;
        }


        #region Getting User Data - Identity
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUserIdentity(Guid userID)
        {
            string userIdentityData = string.Empty;
            Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            string pathQuery = myuri.PathAndQuery;
            string hostName = myuri.ToString().Replace(pathQuery, "");

            SqlConnection cn = new SqlConnection();
            string constring = GetConnectionString(hostName, null, null);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString();
            cn.Open();
            SqlCommand cm = new SqlCommand("[user].[identitydata_get]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@user_id", userID);
            try
            {
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["UserData"] != DBNull.Value)
                    {
                        userIdentityData = oreader["UserData"].ToString();
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
            //userIdentityData = "-1;6BF764C5-50D8-46C8-BA5B-46EE6A5205FA;SurveyDownline;http://www.surveydownline.com|14002;80B0E2BE-8BAF-48FE-BAB2-67C65AF4691D;sandeep;Mididoddi;28201;M;123456;sandeep@precisionsoftech.com;english|";
            return userIdentityData;

        }
        /// <summary>
        /// 
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
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
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
    }
}
