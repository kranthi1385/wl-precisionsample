using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Configuration;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class Page2LeadsDataServer
    {
        #region GetPage2Leads
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Range"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public Page2LeadsEntities GetPage2Leads(string Range, string StartDate, string EndDate)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[user].[UserLeads_Get]", cn);
            cm.Parameters.AddWithValue("range", Range);
            cm.Parameters.AddWithValue("start_date", StartDate);
            cm.Parameters.AddWithValue("end_date", EndDate);
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
            {
                cm.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
            }
            else
            {
                cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
            }
            Page2LeadsEntities oP2L = new Page2LeadsEntities();
            using (IDataReader reader = cm.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (reader["debt"] != DBNull.Value)
                    {
                        oP2L.Debt = Convert.ToInt32(reader["debt"]);
                    }
                    if (reader["credit"] != DBNull.Value)
                    {
                        oP2L.Credit = Convert.ToInt32(reader["credit"]);
                    }
                    if (reader["bank"] != DBNull.Value)
                    {
                        oP2L.Bank = Convert.ToInt32(reader["bank"]);
                    }
                    if (reader["loan"] != DBNull.Value)
                    {
                        oP2L.Loan = Convert.ToInt32(reader["loan"]);
                    }

                }
                return oP2L;
            }
        }
        #endregion
    }
}
