using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Configuration;
using System.Data.SqlClient;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class FriendDataServices
    {
        #region ConnectionString
        public string ConnectionString
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
        #endregion

        #region Friend Insert

        /// <summary>
        /// friend insert
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public List<Friend> FriendInsert(int UserId, string xml, int ClientId)
        {
            List<Friend> lstFriend = new List<Friend>();
            string constring = string.Empty;
            UserDataServices oDataServer = new UserDataServices();
            constring = oDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings[constring].ToString();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[friends_insert]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                #region Adding params for the SQL Proc (IF NOT NULL)
                cmd.Parameters.AddWithValue("user_id", UserId);
                cmd.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                if (xml != string.Empty)
                {
                    cmd.Parameters.AddWithValue("xml", xml);
                }
                #endregion

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Friend oFriend = new Friend();

                        if (reader["friend_first_name"] != DBNull.Value)
                        {
                            oFriend.FriendFirstName = reader["friend_first_name"].ToString();
                        }

                        if (reader["friend_email_address"] != DBNull.Value)
                        {
                            oFriend.FriendEmailAddress = reader["friend_email_address"].ToString();
                        }
                        oFriend.Mode = 1;
                        lstFriend.Add(oFriend);
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            Friend oFriend = new Friend();

                            if (reader["friend_first_name"] != DBNull.Value)
                            {
                                oFriend.FriendFirstName = reader["friend_first_name"].ToString();
                            }

                            if (reader["friend_email_address"] != DBNull.Value)
                            {
                                oFriend.FriendEmailAddress = reader["friend_email_address"].ToString();
                            }
                            oFriend.Mode = 2;
                            lstFriend.Add(oFriend);
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
            return lstFriend;
        }


        #endregion

        #region Fetch FriendList-Information

        /// <summary>
        /// friend information
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        public List<Friend> FriendInformation(int UserId, int ClientId)
        {
            List<Friend> lstFriend = new List<Friend>();
            List<NameValuePair> lstCnStings = new List<NameValuePair>();
            UserDataServices oUserDataServer = new UserDataServices();
            string constr = oUserDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[admin].[connection_strings_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        NameValuePair objCnList = new NameValuePair();
                        if (dr["name"] != DBNull.Value)
                        {
                            objCnList.CommonKey = Convert.ToString(dr["name"]);
                        }
                        if (dr["value"] != DBNull.Value)
                        {
                            objCnList.Value = Convert.ToString(dr["value"]);
                        }
                        lstCnStings.Add(objCnList);
                    }

                }
                for (int i = 0; i < lstCnStings.Count(); i++)
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings[lstCnStings[i].CommonKey].ToString();
                    con.Open();
                    SqlCommand com = new SqlCommand("[user].[friends_get]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@user_id", UserId);
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                    {
                        com.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                    }
                    else
                    {
                        com.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                    }
                    using (IDataReader reader = com.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Friend oFriend = new Friend();
                            if (reader["friend_first_name"] != DBNull.Value)
                            {
                                oFriend.FriendFirstName = reader["friend_first_name"].ToString();
                            }
                            if (reader["friend_email_address"] != DBNull.Value)
                            {
                                oFriend.FriendEmailAddress = reader["friend_email_address"].ToString();
                            }
                            if (reader["status"] != DBNull.Value)
                            {
                                oFriend.Status = reader["status"].ToString();
                            }
                            if (reader["commission"] != DBNull.Value)
                            {
                                oFriend.Commission = Convert.ToDecimal(reader["commission"]);
                            }
                            lstFriend.Add(oFriend);
                        }
                    }
                    con.Close();
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
            return lstFriend;
        }
        #endregion


        #region FriendList-Information

        /// <summary>
        /// Friend List
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        public List<Friend> FriendList(int UserId, int ClientId)
        {
            List<Friend> lstFriend = new List<Friend>();
            List<NameValuePair> lstCnStings = new List<NameValuePair>();
            UserDataServices oUserServices = new UserDataServices();
            string constr = oUserServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[admin].[connection_strings_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        NameValuePair objCnList = new NameValuePair();
                        if (dr["name"] != DBNull.Value)
                        {
                            objCnList.CommonKey = Convert.ToString(dr["name"]);
                        }
                        if (dr["value"] != DBNull.Value)
                        {
                            objCnList.Value = Convert.ToString(dr["value"]);
                        }
                        lstCnStings.Add(objCnList);
                    }

                }
                for (int i = 0; i < lstCnStings.Count(); i++)
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = ConfigurationManager.ConnectionStrings[lstCnStings[i].CommonKey].ToString();
                    con.Open();
                    SqlCommand com = new SqlCommand("[user].[friends_get]", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@user_id", UserId);
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                    {
                        com.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                    }
                    else
                    {
                        com.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                    }


                    using (IDataReader reader = com.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Friend oFriend = new Friend();
                            if (reader["friend_first_name"] != DBNull.Value)
                            {
                                oFriend.FriendFirstName = reader["friend_first_name"].ToString();
                            }
                            lstFriend.Add(oFriend);
                        }
                    }
                    con.Close();
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
            return lstFriend;
        }
        #endregion


    }
}
