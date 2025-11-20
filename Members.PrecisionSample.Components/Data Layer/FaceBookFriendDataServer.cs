using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    class FaceBookFriendDataServer
    {
        #region ConnectionString
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"].ToString();
            }
        }
        #endregion

        #region Get FaceBook Users

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<FaceBookFriend> GetFaceBookUsers(string userIds)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[facebook].[FB_Users_Get]", cn);
            #region Adding params for the SQL Proc
            cm.Parameters.AddWithValue("userids",userIds);
            #endregion

            using (IDataReader reader = cm.ExecuteReader())
            {
                List<FaceBookFriend> lstFaceBookFriends = new List<FaceBookFriend>();
                while (reader.Read())
                {
                    FaceBookFriend oFaceBookFriends = new FaceBookFriend();
                    oFaceBookFriends.Id = Convert.ToString((reader["id"]));
                    lstFaceBookFriends.Add(oFaceBookFriends);
                }
                return lstFaceBookFriends;
            }
        }

        #endregion

        #region Friend Insert

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<FaceBookFriend> FriendInsert(int userId, string xml)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[facebook].[Friends_Insert]", cn);
            #region Adding params for the SQL Proc (IF NOT NULL)
            cm.Parameters.AddWithValue("user_id",userId);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
            {
                cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
            }
            else
            {
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
            }

            if (xml != string.Empty)
            {
                cm.Parameters.AddWithValue("xml", MemberIdentity.Client.ClientId);
            }

            #endregion

            using (IDataReader reader = cm.ExecuteReader())
            {
                List<FaceBookFriend> lstFriend = new List<FaceBookFriend>();
                while (reader.Read())
                {
                    FaceBookFriend oFaceBookFriend = new FaceBookFriend();

                    if (reader["friend_first_name"] != DBNull.Value)
                    {
                        oFaceBookFriend.FirstName = reader["friend_first_name"].ToString();
                    }

                    if (reader["friend_last_name"] != DBNull.Value)
                    {
                        oFaceBookFriend.LastName = reader["friend_last_name"].ToString();
                    }
                    oFaceBookFriend.Mode = 1;
                    lstFriend.Add(oFaceBookFriend);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        FaceBookFriend oFaceBookFriend = new FaceBookFriend();

                        if (reader["friend_first_name"] != DBNull.Value)
                        {
                            oFaceBookFriend.FirstName = reader["friend_first_name"].ToString();
                        }

                        if (reader["friend_last_name"] != DBNull.Value)
                        {
                            oFaceBookFriend.LastName = reader["friend_last_name"].ToString();
                        }
                        oFaceBookFriend.Mode = 2;
                        lstFriend.Add(oFaceBookFriend);
                    }
                }
                return lstFriend;
            }
        }
        #endregion
    }
}
