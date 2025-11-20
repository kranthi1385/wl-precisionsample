using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
namespace Members.PrecisionSample.Components.Data_Layer
{
    class RegistrationDataServer
    {
        #region LeadUpdates
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LeadType"></param>
        /// <param name="advertiserid"></param>
        public void LeadUpdates(int UserId, string LeadType, int advertiserid)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[UserLeadsforCoregs_Update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserId);
                cm.Parameters.AddWithValue("@advertiser_id", advertiserid);
                cm.Parameters.AddWithValue("@lead_type", LeadType);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }
                cm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                cn.Close();
            }
        }




        #endregion

        #region LeadRevenueUpdate
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oLeadEntities"></param>
        public void LeadRevenueUpdate(LeadEntities oLeadEntities)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[LeadsRevenue_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("user_id", oLeadEntities.UserId);
                cm.Parameters.AddWithValue("advertiser_id", oLeadEntities.AdvertiserId);
                cm.Parameters.AddWithValue("transaction_key", oLeadEntities.TransactionKey);
                cm.Parameters.AddWithValue("amount", oLeadEntities.Amount);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }
                cm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                cn.Close();
            }
        }

        #endregion
    }
}

