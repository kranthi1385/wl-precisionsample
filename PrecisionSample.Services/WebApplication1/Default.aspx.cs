using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetConnectionString(4, null);
            ServiceReference1.v2SoapClient obj1 = new ServiceReference1.v2SoapClient();
         
            string result = obj1.Update("08BF3839-7AF5-4DBF-ACEF-4FE269F6FAF2", 4, "1", "1", "india", "ts", "chandu", "k", "chanduramnaidu@gmail.com", "500072", null, "1991-11-19", "asdfadsf1", "asfasdf", "1", "hyderabad");

            //psapi.v2 obj = new psapi.v2();
            //string s = obj.c(1, "1", "1", "india", "ts", "chandu", "k", "chanduramnaidu@gmail.com", "500072",null, "1991-11-19", "asdfadsf", "asfasdf", "Asia", "hyderabad");

        }
        internal string GetConnectionString(int? Rid = null, string Memberguid = "")
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "server = 34.224.211.160; database = precisionsample.2.0; uid = SA; pwd = dev1@dms; Connect Timeout = 120;";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_connection_string_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                if (Rid != null)
                {
                    cm.Parameters.AddWithValue("@referrer_id", Rid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@referrer_id", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(Memberguid))
                {
                    cm.Parameters.AddWithValue("@user_guid", Memberguid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@user_guid", DBNull.Value);
                }
                cm.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {

                        if (dr["s_name"] != DBNull.Value)
                        {
                            return ConfigurationManager.AppSettings[dr["s_name"].ToString()];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

    }
}