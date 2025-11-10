using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Data_Layer
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

        #region Data Fetch - Page4Offers
        /// <summary>
        /// GetPage4OfferDetails
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>

        public List<Perks> GetPage4OfferDetails(int user_id)
        {
            List<Perks> lstPerks = new List<Perks>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("[ams].[Page4Offers_Get]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("user_id", user_id);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cmd.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cmd.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Perks oPerks = new Perks();
                        if (reader["perk_url"] != null)
                        {
                            oPerks.PerkUrl = reader["perk_url"].ToString();
                        }
                        if (reader["freebie_logo"] != null)
                        {
                            oPerks.Page4ImageLogo = reader["freebie_logo"].ToString();
                        }
                        if (reader["perk_guid"] != null)
                        {
                            oPerks.PerkGuid = new Guid(reader["perk_guid"].ToString());
                        }
                        lstPerks.Add(oPerks);
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
            return lstPerks;
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
        public Perks GetPerkDetails(string PerkGuid, string UserGuid)
        {
            Perks oPerks = new Perks();
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
        public Perks InsertClickDate1(string PerkGuid, int UserId, string UserGuid, string src, int ClientId)
        {
            Perks oPerk = new Perks();
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
