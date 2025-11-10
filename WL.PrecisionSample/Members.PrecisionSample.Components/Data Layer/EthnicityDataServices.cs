using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
   public class EthnicityDataServices
    {
        #region Data Fetch - Ethnicity

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Ethnicity> GetEthnicityList()
        {
            List<Ethnicity> lstEthnicity = new List<Ethnicity>();
            #region to read Language Cookie

            //LOgic to read the value  form Session First , If that session is null then  read from Member Identity.
            //Added by sandy on 06/19/2015 
            //string language_name = "english"; //By default to English.
            //if (SurveyDownLine.Components.Business_Services.MemberIdentity.Client.ClientId == 111)
            //{
            //    if (HttpContext.Current.Session["LQLanguageName"] != null)
            //    {
            //        language_name = HttpContext.Current.Session["LQLanguageName"].ToString();
            //    }
            //    else
            //    {
            //        language_name = Identity.Current.UserData.UserLanguage;
            //    }
            //}

            //End lines of codes to Read Language from Cookie.
            #endregion
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[geo].[Ethnicity_List_Get]", cn);
                //cm.Parameters.AddWithValue("language_name", language_name);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        Ethnicity OEthnicity = new Ethnicity();
                        OEthnicity.EthnicityId = Convert.ToInt32((reader["ethnicity_id"]));
                        OEthnicity.EthnicityType = Convert.ToString((reader["ethnicity_type"]));
                        lstEthnicity.Add(OEthnicity);
                    }
                  
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
            return lstEthnicity;
        }

        #endregion
    }
}
