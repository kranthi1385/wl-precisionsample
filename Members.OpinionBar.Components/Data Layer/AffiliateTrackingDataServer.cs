using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;

namespace Members.OpinionBar.Components.Data_Layer
{
   public class AffiliateTrackingDataServer
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
        #region Data Fetch - Gets the tracking details of Affiliates
        /// <summary>
        /// GetTracking Details
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public List<AffiliateTrackingEntities> GetTrackingDetails(int ReferrerId, int userid, int cid)
        {
            User objUser = new User();
            //SqlConnection cn = new SqlConnection();
            //cn.ConnectionString = ConnectionString;
            List<AffiliateTrackingEntities> lstATE = new List<AffiliateTrackingEntities>();
            UserDataServices objDataServer = new UserDataServices();
            string constr = string.Empty;
            if (cid != 0)
            {
                constr = objDataServer.GetConnectionString(null, null, cid);
            }
            else
            {
                constr = objDataServer.GetConnectionString(null, ReferrerId, null);
            }
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[referrer_trackingdetails_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("referrer_id", ReferrerId);
                cm.Parameters.AddWithValue("user_id", userid);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }

                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AffiliateTrackingEntities oATE = new AffiliateTrackingEntities();
                        if (reader["tracking_type_id"] != DBNull.Value)
                        {
                            oATE.TrackingType = Convert.ToChar(reader["tracking_type_id"]);
                        }
                        if (reader["soi_tracking_pixel"] != DBNull.Value)
                        {
                            oATE.TrackingDetails = reader["soi_tracking_pixel"].ToString();
                        }
                        if (reader["firepixel"] != DBNull.Value)
                        {
                            oATE.FirePixel = Convert.ToInt32(reader["firepixel"]);
                        }
                        if (reader["referrer_pixel_type_id"] != DBNull.Value)
                        {
                            oATE.PixelTypeId = Convert.ToInt32(reader["referrer_pixel_type_id"]);
                        }

                        if (reader["callback_type_id"] != DBNull.Value)
                        {
                            oATE.CallbacktypeId = Convert.ToInt32(reader["callback_type_id"]);
                        }
                        lstATE.Add(oATE);
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
            return lstATE;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public List<AffiliateTrackingEntities> Step2GetTrackingDetails(int ReferrerId, int userid)
        {
            List<AffiliateTrackingEntities> lstATE = new List<AffiliateTrackingEntities>();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[referrer].[Step1ReferrerTrackingDetails_Get]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("referrer_id", ReferrerId);
                cmd.Parameters.AddWithValue("user_id", userid);
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
                        AffiliateTrackingEntities oATE = new AffiliateTrackingEntities();
                        if (reader["tracking_type_id"] != DBNull.Value)
                        {
                            oATE.TrackingType = Convert.ToChar(reader["tracking_type_id"]);
                        }
                        if (reader["soi_tracking_pixel"] != DBNull.Value)
                        {
                            oATE.TrackingDetails = reader["soi_tracking_pixel"].ToString();
                        }
                        if (reader["firepixel"] != DBNull.Value)
                        {
                            oATE.FirePixel = Convert.ToInt32(reader["firepixel"]);
                        }
                        lstATE.Add(oATE);
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
            return lstATE;
        }

        #endregion

    }
}
