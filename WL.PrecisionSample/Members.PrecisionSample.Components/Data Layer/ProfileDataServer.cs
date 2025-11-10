using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data.SqlClient;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class ProfileDataServer
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

        #region getprofiles for WLlables
        /// <summary>
        /// Get User Avliable Profiles
        /// </summary>
        /// <param name="MemberLanguage">Memeber Language</param>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        public List<Profile> GetProfiles(string MemberLanguage, int UserId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            List<Profile> objProfileList = new List<Profile>();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[ProfilesList_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", Convert.ToInt32(UserId));
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                cm.Parameters.AddWithValue("@language_name", MemberLanguage);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                   
                    while (dr.Read())
                    {
                        Profile oProfile = new Profile();
                        if (dr["profile_name"] != DBNull.Value)
                        {
                            oProfile.ProfileName = dr["profile_name"].ToString();
                        }
                        if (dr["profile_id"] != DBNull.Value)
                        {
                            oProfile.ProfileId = Convert.ToInt32(dr["profile_id"]);
                        }
                        if (dr["count"] != DBNull.Value)
                        {
                            oProfile.Count = Convert.ToInt32(dr["count"]);
                        }

                        objProfileList.Add(oProfile);
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
            return objProfileList;
        }


        #endregion
    }
}
