using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Members.PrecisionSample.Components.Data_Layer
{
    class ReferrerDataServer
    {
        #region Get traking Details

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        public Referrer GetReferrerTrackingDetails(int referrer_id)
        {
            Referrer oUser = new Referrer();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[referrer].[ReferrerTrackingList_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("referrer_id", referrer_id);
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

                    //HttpContext.Current.Session["UserSession"] = oUser;
                    while (reader.Read())
                    {

                        if (reader["doi_tracking_pixcel"] != DBNull.Value)
                        {
                            oUser.DoiPixel = reader["doi_tracking_pixcel"].ToString();
                        }
                        if (reader["soi_tracking_pixcel"] != DBNull.Value)
                        {
                            oUser.SoiPixel = reader["soi_tracking_pixcel"].ToString();
                        }
                        oUser.TrackingType = reader["tracking_type"].ToString();
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
            return oUser;
        }
        #endregion

        #region getladingpage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="referrer_id"></param>
        /// <returns></returns>
        public string GetLandingpageUrl(int referrer_id)
        {
            string url = string.Empty;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[referrer].[ReferrerLandingPage_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("referrer_id", referrer_id);
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
                        if (reader["landing_url"] != DBNull.Value)
                        {
                            url = reader["landing_url"].ToString();
                        }
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
            return url;
            #endregion
        }
    }
}
