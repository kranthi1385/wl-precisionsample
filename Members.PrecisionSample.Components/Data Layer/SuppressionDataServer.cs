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
    class SuppressionDataServer
    {
        #region Data Fetch - States
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public void InsertEmailAddress(string strEmailAddress)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[user].[suppress_Insert]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            #region Adding params for the SQL Proc

            cm.Parameters.AddWithValue("@email_address",strEmailAddress);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
            {
                cm.Parameters.AddWithValue("org_id",Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
            }
            else
            {
                cm.Parameters.AddWithValue("org_id",MemberIdentity.Client.ClientId);
            }

            #endregion

            cm.ExecuteNonQuery();
        }

        #endregion
    }
}
