using System;
using System.Collections.Generic;
using System.Configuration;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Members.OpinionBar.Components.Data_Layer
{
   public class PerksDataServices
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

        #region Get ShowMe SurveysList
        /// <summary>
        /// Get ShowMe SurveysList
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<Surveys> GetShowMeSurveysList(string UserGuid, int ClientId)
        {
            List<Surveys> lstStates = new List<Surveys>();
            UserDataServices oDataServices = new UserDataServices();
            string constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[showmesurveyslist_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Surveys OStates = new Surveys();
                        OStates.SurveyName = Convert.ToString(dr["perk_name"]);
                        OStates.PerkGuid = new Guid(Convert.ToString((dr["perk_guid"])));
                        //OStates.perk = Convert.ToString(reader["perk_url"]);
                        OStates.PerkDescription = Convert.ToString(dr["perk_description"]);
                        lstStates.Add(OStates);
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
            return lstStates;
        }
        #endregion


        #region Get PerkDetails
        /// <summary>
        /// Get PerkDetails
        /// </summary>
        /// <param name="perkGuid">PerkGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Perk GetPerkDetails(string PerkGuid, string UserGuid)
        {
            Perk oPerks = new Perk();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[perkgetalis_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@perk_guid", PerkGuid);
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["perk_description"] != DBNull.Value)
                        {
                            oPerks.PerkDescription = Convert.ToString(dr["perk_description"]);
                        }
                        if (dr["perk_name"] != DBNull.Value)
                        {
                            oPerks.PerkName = Convert.ToString(dr["perk_name"]);
                        }

                        if (dr["reward_value"] != DBNull.Value)
                        {
                            oPerks.RewardValue = Convert.ToDecimal(dr["reward_value"]);
                        }
                        if (dr["perk_id"] != DBNull.Value)
                        {
                            oPerks.PerkId = Convert.ToInt32(dr["perk_id"]);
                        }
                        if (dr["perk_type_id"] != DBNull.Value)
                        {
                            oPerks.PerkTypeId = Convert.ToInt32(dr["perk_type_id"]);
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
            return oPerks;
        }
        #endregion

        #region GetPerkCompletedDate1
        /// <summary>
        /// GetPerkCompletedDate1
        /// </summary>
        /// <param name="PerkGuid">PerKGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public string GetPerkCompletedDate1(string PerkGuid, int UserId, string UserGuid, int ClientId)
        {
            string perkCompletedDt = string.Empty;
            UserDataServices oDataServices = new UserDataServices();
            string constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[perkcompleteddate_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@perk_guid", PerkGuid);
                cm.Parameters.AddWithValue("@user_id", UserId);
                //cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        perkCompletedDt = Convert.ToString((dr["perk_completed_dt"]));

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
            return perkCompletedDt;
        }
        #endregion

        #region InsertClickDate1
        /// <summary>
        /// GetPerkCompletedDate1
        /// </summary>
        /// <param name="PerkGuid">PerKGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Perk InsertClickDate1(string PerkGuid, int UserId, string UserGuid, string src, int ClientId)
        {
            Perk oPerk = new Perk();
            string perkCompletedDt = string.Empty;
            UserDataServices oDataServices = new UserDataServices();
            string constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[perkclickdate_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("perk_guid", PerkGuid);
                cm.Parameters.AddWithValue("user_id", UserId);
                cm.Parameters.AddWithValue("source", src);
                cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);

                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_2_perk_guid"] != DBNull.Value)
                        {
                            oPerk.User2PerkGuid = new Guid(reader["user_2_perk_guid"].ToString());
                        }
                        if (reader["perk_url"] != DBNull.Value)
                        {
                            oPerk.PerkUrl = reader["perk_url"].ToString();
                        }
                        if (reader["is_pixel_tracked"] != DBNull.Value)
                        {
                            oPerk.IsPixelTracked = Convert.ToBoolean(reader["is_pixel_tracked"]);
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
            return oPerk;
        }
        #endregion
    }
}
