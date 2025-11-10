using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace Members.PrecisionSample.Components.Data_Layer
{
    public class SurveyEntryDataLayer
    {
        #region Connection String
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        #endregion

        #region Save Entry
        /// <summary>
        /// Save Entry
        /// </summary>
        /// <param name="uig"></param>
        /// <param name="pid"></param>
        /// <param name="cost"></param>
        /// <param name="FedProjectId"></param>
        /// <returns></returns>
        public SEntry saveEntry(string uig, int pid, Decimal cost, int FedProjectId)
        {
            SEntry oEntry = new SEntry();
            Guid _uig = Guid.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices odataService = new UserDataServices();
            //try
            //{
            //    _uig = new Guid(uig);
            //    //string constr = odataService.GetConnectionString(_uig, -1);
            //    cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            //}
            //catch
            //{
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringSurvey"].ToString();
            //}
            try
            {
                SqlCommand cmd = new SqlCommand("[pms].[fed_entryurl_insert]", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_invitation_guid", uig);
                cmd.Parameters.AddWithValue("@project_id", pid);
                cmd.Parameters.AddWithValue("@fedresponse_id", FedProjectId);
                cmd.Parameters.AddWithValue("@ecost", cost);
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["survey_url"] != DBNull.Value)
                        {
                            oEntry.SurveyUrl = dr["survey_url"].ToString();
                        }
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
            return oEntry;
        }
        #endregion


        public string GetFedEntryUrl(string mid, Guid? eid, int? project, decimal? ecost)
        {
            string Url = string.Empty;
            EndLinksDataServer oserver = new EndLinksDataServer();
            SqlConnection cn = new SqlConnection();
            Guid _invitationGUID = Guid.Empty;
            string constr = "";
            try
            {
                //If the Strip GUID came from fed, we need to convert as Real GUID.
                if (mid.Length == 32)
                {
                    mid = mid.ToString().Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
                }
                _invitationGUID = new Guid(mid);
                constr = oserver.GetConnectionStringByInvitationGuid(new Guid(mid), Guid.Empty, "");

                //If no connection string found.
                if (string.IsNullOrEmpty(constr))
                {
                    //So treating like a External Member.
                    constr = "ConnectionString3";
                }
            }
            catch
            {
                //So treating like a External Member.
                constr = "ConnectionString3";
            }



            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                SqlCommand cmd = new SqlCommand("[pms].[fed_entry_url_get]", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mid", mid);
                cmd.Parameters.AddWithValue("@project", project);
                cmd.Parameters.AddWithValue("@eid", eid);
                cmd.Parameters.AddWithValue("@ecost", ecost);
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            Url = dr["redirect_url"].ToString();
                        }
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
            return Url;
        }


        public string GetRedirectUrl(Guid ug, int project, Guid key, string subid, string IpAddress)
        {
            string url = string.Empty;
            SqlConnection cn = new SqlConnection();
            ClickflowDataServer oManager = new ClickflowDataServer();
            int ClientId = oManager.GetOrgidByUserDPV(ug.ToString());
            UserDataServices odataservices = new UserDataServices();
            string constr = odataservices.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                SqlCommand cmd = new SqlCommand("[user].[get_projects_api_entry_url_get]", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_guid", ug);
                cmd.Parameters.AddWithValue("@project_id", project);
                cmd.Parameters.AddWithValue("@key", key);
                cmd.Parameters.AddWithValue("@sub_id", subid);
                cmd.Parameters.AddWithValue("@ip_address", IpAddress);
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["survey_url"] != DBNull.Value)
                        {
                            url = dr["survey_url"].ToString();
                        }
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
            return url;
        }

        #region Get PII Data

        #region Get Top1 pre prescreener questions
        /// <summary>
        /// Get Top1 pre prescreener questions
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public List<Question> GetQuestion(Guid Ug, int ProjectId)
        {
            //string sp = string.Empty;
            List<Question> lstQst = new List<Question>();
            UserDataServices objDataServer = new UserDataServices();
            ClickflowDataServer oManager = new ClickflowDataServer();
            int ClientId = oManager.GetOrgidByUserDPV(Ug.ToString());
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[projects_api_user_pii_data_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", Ug);
                cm.Parameters.AddWithValue("@project_id", ProjectId);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    Question obj = new Question();
                    if (oreader["question_id"] != DBNull.Value)
                    {
                        obj.QuestionId = Convert.ToInt32(oreader["question_id"]);
                    }
                    if (oreader["question_text"] != DBNull.Value)
                    {
                        obj.QuestionText = oreader["question_text"].ToString();
                    }
                    if (oreader["question_type_id"] != DBNull.Value)
                    {
                        obj.QuestionTypeId = Convert.ToInt32(oreader["question_type_id"]);
                    }
                    if (oreader["has_options"] != DBNull.Value)
                    {
                        obj.HasOptions = Convert.ToBoolean(oreader["has_options"]);
                    }
                    if (oreader["zip_question"] != DBNull.Value)
                    {
                        obj.ZIPQuestion = Convert.ToBoolean(oreader["zip_question"]);
                    }
                    if (oreader["org_id"] != DBNull.Value)
                    {
                        obj.ClientId = Convert.ToInt32(oreader["org_id"]);
                    }
                    if (obj.HasOptions || obj.QuestionId == 4)
                    {
                        oreader.NextResult();
                        List<Options> optlst = new List<Options>();
                        while (oreader.Read())
                        {
                            Options optobj = new Options();
                            if (oreader["option_id"] != DBNull.Value)
                            {
                                optobj.OptionId = Convert.ToInt32(oreader["option_id"]);
                            }
                            if (oreader["option_text"] != DBNull.Value)
                            {
                                optobj.OptionText = oreader["option_text"].ToString();
                            }
                            optlst.Add(optobj);
                        }
                        obj.Options = optlst;
                    }
                    lstQst.Add(obj);
                }

            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return lstQst;
        }
        #endregion

        #region Save Answers And Return Question
        /// <summary>
        /// Save Answers And Return Question
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="qid"></param>
        /// <param name="otext"></param>
        /// <param name="optId"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public List<Question> SaveResponse(int qid, string otext, int optId, Guid Ug, int ClientId, string zip, int ProjectID)
        {
            string sp = string.Empty;
            List<Question> lstQst = new List<Question>();
            if (qid == 10)
            {
                try
                {
                    DateTime d = DateTime.Parse(otext);
                    otext = d.ToString("MM/dd/yyyy");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            //save funtionality

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[projects_api_user_pii_data_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", Ug);
                cm.Parameters.AddWithValue("@question_id", qid);
                cm.Parameters.AddWithValue("@option_text", otext);
                if (optId != 0)
                {
                    cm.Parameters.AddWithValue("@option_id", optId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@option_id", DBNull.Value);
                }
                cm.Parameters.AddWithValue("@zip", zip);
                cm.Parameters.AddWithValue("@project_id", ProjectID);
                using (IDataReader oreader = cm.ExecuteReader())
                {
                    while (oreader.Read())
                    {
                        Question obj = new Question();
                        if (oreader["question_id"] != DBNull.Value)
                        {
                            obj.QuestionId = Convert.ToInt32(oreader["question_id"]);
                        }
                        if (oreader["question_text"] != DBNull.Value)
                        {
                            obj.QuestionText = oreader["question_text"].ToString();
                        }
                        if (oreader["question_type_id"] != DBNull.Value)
                        {
                            obj.QuestionTypeId = Convert.ToInt32(oreader["question_type_id"]);
                        }
                        if (oreader["has_options"] != DBNull.Value)
                        {
                            obj.HasOptions = Convert.ToBoolean(oreader["has_options"]);
                        }
                        if (oreader["zip_question"] != DBNull.Value)
                        {
                            obj.ZIPQuestion = Convert.ToBoolean(oreader["zip_question"]);
                        }
                        if (oreader["org_id"] != DBNull.Value)
                        {
                            obj.ClientId = Convert.ToInt32(oreader["org_id"]);
                        }
                        if (obj.HasOptions || obj.QuestionId == 4)
                        {
                            oreader.NextResult();
                            List<Options> optlst = new List<Options>();
                            while (oreader.Read())
                            {
                                Options optobj = new Options();
                                if (oreader["option_id"] != DBNull.Value)
                                {
                                    optobj.OptionId = Convert.ToInt32(oreader["option_id"]);
                                }
                                if (oreader["option_text"] != DBNull.Value)
                                {
                                    optobj.OptionText = oreader["option_text"].ToString();
                                }
                                optlst.Add(optobj);
                            }
                            obj.Options = optlst;

                        }
                        lstQst.Add(obj);
                    }
                }

            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }
            return lstQst;
        }
        #endregion

        #endregion
    }
}
