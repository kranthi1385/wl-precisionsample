using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using NLog;
using System.Net;
using System.IO;

namespace Members.PrecisionSample.Components.Data_Layer
{


    public class ClickflowDataServer
    {

        #region public variables
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        ProfileQuestionDataLayer oPfDataServer = new ProfileQuestionDataLayer();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Save User Click Inforamtion
        /// <summary>
        ///Save User Click Inforamtion
        /// </summary>
        /// <param name="QgId">QuotaGroupId</param>
        /// <param name="UgId">UserGuid</param>
        /// <param name="PrjId">ProjectId</param>
        /// <param name="Rid">ReferrerId</param>
        /// <param name="Soruce">Source</param>
        /// <param name="SubId">SubId</param>
        /// <param name="IsNew">IsNew</param>
        /// <param name="UserTrafficTypeId">UserTrafficOd</param>
        /// <param name="MobiledeviceModel">DeviceModel</param>
        /// <param name="BrowserInfo">BrowserInfo</param>
        /// <param name="AgentInfo">AgentInfo</param>
        /// <param name="IpAddress">IpAddress</param>
        /// <param name="RelevantId">RelevantId</param>
        /// <param name="RelevantScore">RelevantScore</param>
        /// <param name="FpfScores">FpfScore</param>
        /// <param name="FraudPfScore">FraudPfScore</param>
        /// <param name="OldSurveyInvitationId">OldSurveyInvitationd</param>
        /// <returns></returns>
        public Surveys SaveClickInformation(string QgId, string UgId, int PrjId, int ClientId, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                          string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore, string OldSurveyInvitationId, string Vid, int Vscore, string fedresid, string geodata, string IPriskScore, string IPNumber)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServices = new UserDataServices();
            string constr = string.Empty;
            constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_step1_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UgId);
                cm.Parameters.AddWithValue("@target_guid", QgId);
                cm.Parameters.AddWithValue("@project_id", PrjId);
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                cm.Parameters.AddWithValue("@source", Source);
                cm.Parameters.AddWithValue("@sub_id", SubId);
                cm.Parameters.AddWithValue("@user_traffic_type_id", UserTrafficTypeId);
                cm.Parameters.AddWithValue("@mobile_device_model", MobiledeviceModel);
                cm.Parameters.AddWithValue("@browser_info", BrowserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@fpf_scores", FpfScores);
                cm.Parameters.AddWithValue("@ip_risk_score", Convert.ToDecimal(IPriskScore));
                cm.Parameters.AddWithValue("@ip_number", Convert.ToDecimal(IPNumber));
                cm.Parameters.AddWithValue("@is_new", IsNew);
                cm.Parameters.AddWithValue("@vid", Vid);
                cm.Parameters.AddWithValue("@geo_ip2_content", geodata);
                if (Vscore != 0)
                {
                    cm.Parameters.AddWithValue("@vscore", Vscore);
                }
                else
                {
                    cm.Parameters.AddWithValue("@vscore", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(OldSurveyInvitationId))
                {
                    cm.Parameters.AddWithValue("@old_survey_invtation_id", OldSurveyInvitationId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@old_survey_invtation_id", DBNull.Value);
                }
                cm.Parameters.AddWithValue("@rid", Rid);
                if (!string.IsNullOrEmpty(fedresid))
                {
                    cm.Parameters.AddWithValue("@fed_response_id", fedresid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_response_id", DBNull.Value);
                }
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oSurvey.RedirectUrl = Convert.ToString(dr["redirect_url"]);
                        }
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["survey_user_type_ids"] != DBNull.Value)
                        {
                            oSurvey.SurveyUserTypeIds = Convert.ToString(dr["survey_user_type_ids"]);
                        }
                        if (dr["is_stand_alone_partner"] != DBNull.Value)
                        {
                            oSurvey.IsStandalone = Convert.ToBoolean(dr["is_stand_alone_partner"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            oSurvey.CountyCode = Convert.ToString(dr["country_code"]);
                        }
                        if (dr["survey_name"] != DBNull.Value)
                        {
                            oSurvey.SurveyName = Convert.ToString(dr["survey_name"]);
                        }
                        if (dr["org_id"] != DBNull.Value)
                        {
                            oSurvey.OrgId = Convert.ToInt32(dr["org_id"]);
                        }

                        if (dr["is_email_invitation"] != DBNull.Value)
                        {
                            oSurvey.IsEmailInvitationEnable = Convert.ToBoolean(dr["is_email_invitation"]);
                        }
                        if (dr["is_sms_invitation"] != DBNull.Value)
                        {
                            oSurvey.IsSmsInvitation = Convert.ToBoolean(dr["is_sms_invitation"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            oSurvey.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["target_id"] != DBNull.Value)
                        {
                            oSurvey.Targetid = Convert.ToInt32(dr["target_id"]);
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            oSurvey.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["actual_invitation_guid"] != DBNull.Value)
                        {
                            oSurvey.ActualInvitationGuid = new Guid(dr["actual_invitation_guid"].ToString());
                        }

                        if (dr["org_logo"] != DBNull.Value)
                        {
                            oSurvey.OrgLogo = dr["org_logo"].ToString();
                        }
                        if (dr["language_code"] != DBNull.Value)
                        {
                            oSurvey.lid = dr["language_code"].ToString();
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
            return oSurvey;
        }

        #endregion

        #region SendSms
        /// <summary>
        /// SendSms
        /// </summary>
        /// <param name="UsetInviationGuid">UserInvitationGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="ProjectId">ProjectId</param>
        /// <param name="MobileNumber">MobileNumber</param>
        /// <param name="SurveyName">SurveyName</param>
        /// <param name="OrgId">OrgId</param>
        public int SendSms(string UserInvitationGuid, string UserGuid, string ProjectId, string MobileNumber, string SurveyName, string OrgId)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[sms_status_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@project_id", ProjectId);
                cm.Parameters.AddWithValue("@mobile_number", MobileNumber);
                cm.Parameters.AddWithValue("@survey_name", SurveyName);
                cm.Parameters.AddWithValue("@org_id", OrgId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            result = Convert.ToInt32(dr["result"]);
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
            return result;
        }
        #endregion

        #region checkradius
        /// <summary>
        /// checkradius
        /// </summary>
        /// <param name="ug"></param>
        /// <param name="geodata"></param>
        /// <returns></returns>
        public int checkradius(string ug, string geodata)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", ug);
                cm.Parameters.AddWithValue("@user_guid", geodata);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            result = Convert.ToInt32(dr["result"]);
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
            return result;
        }
        #endregion

        #region GetUserViertyDetails
        /// <summary>
        /// Get User VerityDetails
        /// </summary>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public Surveys GetUserVerityDetails(Guid UserGuid, int ClientId, Guid UserInvitationGuid, int pid, int tid, int usid)
        {
            string sp = string.Empty;
            Surveys oSurvey = new Surveys();
            UserDataServices odataservices = new UserDataServices();
            string constr = odataservices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            if (UserGuid == UserInvitationGuid)
                sp = "[pms].[user_2_survey_click_step3_external]";
            else
                sp = "[pms].[user_2_survey_click_step3_insert]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@project_id", pid);
                cm.Parameters.AddWithValue("@target_id", tid);
                cm.Parameters.AddWithValue("@user_id", usid);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["verity_string"] != DBNull.Value)
                        {
                            oSurvey.VerityDetails = Convert.ToString(dr["verity_string"]);
                        }
                        if (dr["verity_score"] != DBNull.Value)
                        {
                            oSurvey.VerityScore = Convert.ToInt32(dr["verity_score"]);
                        }
                        if (dr["challenge_id"] != DBNull.Value)
                        {
                            oSurvey.ChallengeId = Convert.ToString(dr["challenge_id"]);
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
            return oSurvey;
        }
        #endregion

        #region Save Veriy
        /// <summary>
        /// Save Verity
        /// </summary>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="VerityScore">VerityScore</param>
        /// <param name="VerityId">VerityId</param>
        /// <param name="ChallangeId">ChallangeId</param>
        /// <param name="GeoCorrelationFlag">GeoCorrekationFlag</param>
        /// <param name="QstText1">QuestionText1</param>
        /// <param name="QstText2">QuestionText2</param>
        /// <param name="QstText3">QuestionText3</param>
        /// <param name="OptText1">OptionText1</param>
        /// <param name="OptText2">OptionText2</param>
        /// <param name="OptText3">OptionText3</param>
        /// <param name="VerityDOBFail">VerityDOBFail</param>
        /// <returns></returns>
        public Surveys SaveVerityQuestions(Guid UserInvitationGuid, Guid UserGuid, int ClientId, int VerityScore, string VerityId, string ChallangeId, int GeoCorrelationFlag, string QstText1, string QstText2,
           string QstText3, string OptText1, string OptText2, string OptText3, bool VerityDoBFail)
        {
            string sp = string.Empty;
            Surveys oSurvey = new Surveys();
            UserDataServices odataservices = new UserDataServices();
            string constr = odataservices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            if (UserGuid == UserInvitationGuid)
                sp = "[pms].[user_2_survey_click_step3_verity_save_ext]";
            else
                sp = "[pms].[user_2_survey_click_step3_verity_save]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@verity_id", VerityId);
                cm.Parameters.AddWithValue("@verity_score", VerityScore);
                cm.Parameters.AddWithValue("@geo_corelation_flag", GeoCorrelationFlag);
                cm.Parameters.AddWithValue("@challenge_id", ChallangeId);
                cm.Parameters.AddWithValue("@question_text1", QstText1);
                cm.Parameters.AddWithValue("@question_text2", QstText2);
                cm.Parameters.AddWithValue("@question_text3", QstText3);
                cm.Parameters.AddWithValue("@option_text1", OptText1);
                cm.Parameters.AddWithValue("@option_text2", OptText2);
                cm.Parameters.AddWithValue("@option_text3", OptText3);
                cm.Parameters.AddWithValue("@verity_dob_fail", VerityDoBFail);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["redirected_url"] != DBNull.Value)
                        {
                            oSurvey.RedirectUrl = Convert.ToString(dr["redirected_url"]);
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
            return oSurvey;
        }
        #endregion

        #region Get Verity ChallengeQuestions
        /// <summary>
        /// Get Verity ChallengeQuestions
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public List<VerityEnhancedQuestions> GetVerityQuestions(Guid InvitationGuid, Guid UserGuid, int ClientId, int pid, int tid, int usid, string IPriskScore)
        {
            string sp = string.Empty;
            List<VerityEnhancedQuestions> lstEnachancedQuestions = new List<VerityEnhancedQuestions>();
            UserDataServices oDataServer = new UserDataServices();
            string constr = oDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            if (UserGuid == InvitationGuid)
                sp = "[pms].[user_2_survey_click_step4_external]";
            else
                sp = "[pms].[user_2_survey_click_step4_insert]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", InvitationGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@project_id", pid);
                cm.Parameters.AddWithValue("@target_id", tid);
                cm.Parameters.AddWithValue("@user_id", usid);
                cm.Parameters.AddWithValue("@ip_risk_score", Convert.ToDecimal(IPriskScore));
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    VerityEnhancedQuestions objEnhancedqst = new VerityEnhancedQuestions();
                    if (oreader["current_guid"] != null)
                    {
                        objEnhancedqst.UserInvitationGuid = new Guid(oreader["current_guid"].ToString());
                    }
                    if (oreader["challenge_id"] != null)
                    {
                        objEnhancedqst.ChallengeId = Convert.ToString(oreader["challenge_id"]);
                    }
                    if (oreader["question_text"] != null)
                    {
                        objEnhancedqst.QuestionText = Convert.ToString(oreader["question_text"]);
                    }
                    if (oreader["option_text"] != null)
                    {
                        objEnhancedqst.OptionText = Convert.ToString(oreader["option_text"]);
                    }
                    if (oreader["redirect_url"] != null)
                    {
                        objEnhancedqst.RedirectUrl = Convert.ToString(oreader["redirect_url"]);
                    }

                    lstEnachancedQuestions.Add(objEnhancedqst);
                }
            }

            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }
            return lstEnachancedQuestions;
        }
        #endregion

        #region Save Challange Question Response
        /// <summary>
        /// Save Verity Challenge Questions
        /// </summary>
        /// <param name="ChallangeScore">ChallangeScore</param>
        /// <param name="ChallangeId">ChallengeId</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitaitonGuid</param>
        /// <param name="Option1">Option1</param>
        /// <param name="Option2">Option2</param>
        /// <param name="Option3">Option3</param>
        /// <param name="ErrorMsg">Error</param>
        /// <returns></returns>
        public Surveys SaveChallangeQuestionResponse(string ChallangeScore, string ChallangeId, Guid UserGuid, int ClientId, Guid UserInvitationGuid, string Option1, string Option2, string Option3, string ErrorMsg)
        {
            string sp = string.Empty;
            Surveys oSurvey = new Surveys();
            List<VerityEnhancedQuestions> lstEnachancedQuestions = new List<VerityEnhancedQuestions>();
            UserDataServices odataserver = new UserDataServices();
            string constr = odataserver.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            if (UserGuid == UserInvitationGuid)
                sp = "[pms].[user_2_survey_click_step4_cv_save_ext]";
            else
                sp = "[pms].[user_2_survey_click_step4_cv_save]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@challenge_score", ChallangeScore);
                cm.Parameters.AddWithValue("@option1", Option1);
                cm.Parameters.AddWithValue("@option2", Option2);
                cm.Parameters.AddWithValue("@option3", Option3);
                cm.Parameters.AddWithValue("@error_message", ErrorMsg);
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["current_guid"] != DBNull.Value)
                    {
                        oSurvey.UserInvitationId = new Guid(oreader["current_guid"].ToString());
                    }
                    if (oreader["redirect_url"] != DBNull.Value)
                    {
                        oSurvey.RedirectUrl = Convert.ToString(oreader["redirect_url"]);
                    }
                }
            }

            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }
            return oSurvey;
        }
        #endregion

        //#region skip verity questions
        ///// <summary>
        ///// Skip Verity Enchance Questions
        ///// </summary>
        ///// <param name="UserGuid">UserGuid</param>
        ///// <param name="UserInvitationGuid">UserInvitatinGuid</param>
        //public void skipverityquestions(Guid UserGuid, Guid UserInvitationGuid)
        //{
        //    Surveys oSurvey = new Surveys();
        //    List<VerityEnhancedQuestions> lstEnachancedQuestions = new List<VerityEnhancedQuestions>();
        //    string constr = oPfDataServer.GetConnectionString(UserGuid);
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConfigurationManager.connectionStrings[constr].ToString();
        //    try
        //    {
        //        cn.Open();
        //        SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_step4_cv_save]", cn);
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
        //        cm.ExecuteNonQuery();
        //    }

        //    catch (Exception Ex)
        //    {
        //        throw (Ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //}
        //#endregion
        #region Get OrgId by userDPV
        /// <summary>
        /// Get Connectionsttring by userguids
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public int GetOrgidByUserDPV(string UserGuid)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_orgid_by_dpv_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                if (!string.IsNullOrEmpty(UserGuid))
                {
                    cm.Parameters.AddWithValue("@user_guid", UserGuid);
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
                        if (dr["org_id"] != DBNull.Value)
                        {
                            result = Convert.ToInt32(dr["org_id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion
        #region
        public int insertIpRiskScore(string Ipaddress, string IPriskScore)
        {
            int result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_ip_score_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@ip_address", Ipaddress);
                cm.Parameters.AddWithValue("@ip_risk_score", IPriskScore);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

        #region
        public string GetIpRiskScore(string Ipaddress)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_ipaddress_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@ip_address", Ipaddress);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["ip_risk_score"] != DBNull.Value)
                        {
                            result = Convert.ToString(dr["ip_risk_score"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return result;
        }
        #endregion

        #region Insert Survey Click

        public Surveys InsertClick(string QgId, string UgId, int PrjId, int ClientId, string Rid, string Source, string SubId, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                          string AgentInfo, string IpAddress, string OldSurveyInvitationId, string fedresid, string IPNumber, string ResearchDefender, bool isEligible)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServices = new UserDataServices();
            string constr = string.Empty;
            constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            oSurvey.Cstring = constr.Replace("ConnectionString", "");
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UgId);
                cm.Parameters.AddWithValue("@target_guid", QgId);
                cm.Parameters.AddWithValue("@project_id", PrjId);
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                cm.Parameters.AddWithValue("@ip_number", Convert.ToDecimal(IPNumber));
                cm.Parameters.AddWithValue("@source", Source);
                cm.Parameters.AddWithValue("@sub_id", SubId);
                cm.Parameters.AddWithValue("@user_traffic_type_id", UserTrafficTypeId);
                cm.Parameters.AddWithValue("@mobile_device_model", MobiledeviceModel);
                cm.Parameters.AddWithValue("@browser_info", BrowserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@json", ResearchDefender);
                cm.Parameters.AddWithValue("@ipsos_eligibility", isEligible);
                if (!string.IsNullOrEmpty(OldSurveyInvitationId))
                {
                    cm.Parameters.AddWithValue("@old_survey_invtation_id", OldSurveyInvitationId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@old_survey_invtation_id", DBNull.Value);
                }
                if (!string.IsNullOrEmpty(fedresid))
                {
                    cm.Parameters.AddWithValue("@fed_response_id", fedresid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@fed_response_id", DBNull.Value);
                }
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oSurvey.RedirectUrl = Convert.ToString(dr["redirect_url"]);
                        }
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["survey_user_type_ids"] != DBNull.Value)
                        {
                            oSurvey.SurveyUserTypeIds = Convert.ToString(dr["survey_user_type_ids"]);
                        }
                        if (dr["is_stand_alone_partner"] != DBNull.Value)
                        {
                            oSurvey.IsStandalone = Convert.ToBoolean(dr["is_stand_alone_partner"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            oSurvey.CountyCode = Convert.ToString(dr["country_code"]);
                        }
                        if (dr["survey_name"] != DBNull.Value)
                        {
                            oSurvey.SurveyName = Convert.ToString(dr["survey_name"]);
                        }
                        if (dr["org_id"] != DBNull.Value)
                        {
                            oSurvey.OrgId = Convert.ToInt32(dr["org_id"]);
                        }
                        if (dr["is_email_invitation"] != DBNull.Value)
                        {
                            oSurvey.IsEmailInvitationEnable = Convert.ToBoolean(dr["is_email_invitation"]);
                        }
                        if (dr["is_sms_invitation"] != DBNull.Value)
                        {
                            oSurvey.IsSmsInvitation = Convert.ToBoolean(dr["is_sms_invitation"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            oSurvey.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["target_id"] != DBNull.Value)
                        {
                            oSurvey.Targetid = Convert.ToInt32(dr["target_id"]);
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            oSurvey.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["actual_invitation_guid"] != DBNull.Value)
                        {
                            oSurvey.ActualInvitationGuid = new Guid(dr["actual_invitation_guid"].ToString());
                        }
                        if (dr["org_logo"] != DBNull.Value)
                        {
                            oSurvey.OrgLogo = dr["org_logo"].ToString();
                        }
                        if (dr["language_code"] != DBNull.Value)
                        {
                            oSurvey.lid = dr["language_code"].ToString();
                        }
                        if (dr["user_invitation_id"] != DBNull.Value)
                        {
                            oSurvey.UsrInvitaitonID = Convert.ToInt64(dr["user_invitation_id"]);
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
            return oSurvey;
        }
        #endregion

        #region insertrelevantData
        public string InsertRelevantData(Guid UserinvitationGuid, string RelevantId, int RelevantScore, string FPFscore, int FraudProfileScore,
            Boolean IsNew, string Browserinfo, string AgentInfo, int UserID, int ClientId, string geodata)
        {
            string Redirecturl = string.Empty;
            string constr = string.Empty;
            UserDataServices oDataServices = new UserDataServices();
            constr = oDataServices.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_validations_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserinvitationGuid);
                cm.Parameters.AddWithValue("@geo_ip2_content", geodata);
                cm.Parameters.AddWithValue("@user_id", UserID);
                cm.Parameters.AddWithValue("@browser_info", Browserinfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@fpf_scores", FPFscore);
                cm.Parameters.AddWithValue("@fraud_profile_score", FraudProfileScore);
                cm.Parameters.AddWithValue("@is_new", IsNew);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            Redirecturl = Convert.ToString(dr["redirect_url"]);
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
            return Redirecturl;
        }
        #endregion

        public RelevantCheck CheckRelevantScore(Int64 userInvitationId, int connectionId)
        {
            UserDataServices oDataServices = new UserDataServices();
            string constr = string.Empty;
            constr = ConfigurationManager.ConnectionStrings[$"ConnectionString{connectionId}"].ToString();
            SqlConnection cn = new SqlConnection(constr);
            RelevantCheck check = null;

            try
            {
                cn.Open();
                //cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
                SqlCommand cmd = new SqlCommand("[pms].[user_2_survey_click_validations_check]", cn);
                cmd.Parameters.AddWithValue("@user_invitation_id", userInvitationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    check = new RelevantCheck();
                    if (dr["relevant_score"] != DBNull.Value)
                    {
                        check.RelevantScore = Convert.ToInt32(dr["relevant_score"]);
                    }
                    if (dr["redirect_url"] != DBNull.Value)
                    {
                        check.URL = dr["redirect_url"].ToString();
                    }
                    if (dr["zip_radius"] != DBNull.Value)
                    {
                        check.ZipRadius = Convert.ToInt32(dr["zip_radius"]);
                    }
                    if (dr["is_gdpr_compliance"] != DBNull.Value)
                    {
                        check.IsGDPRCompliance = Convert.ToBoolean(dr["is_gdpr_compliance"]);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return check;
        }

        public User RdjsonInsert(int userid, string uid, int cid, string uig, string CleanIDJson, string IPQSJson)
        {
            User objUser = new User();
            UserDataServices oDataServices = new UserDataServices();
            string constr = string.Empty;
            constr = oDataServices.GetConnectionString(null, null, cid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("[pms].[reaserch_defender_search_insert]", cn);
                cmd.Parameters.AddWithValue("@user_id", userid);
                cmd.Parameters.AddWithValue("@user_invitation_id", uid);
                cmd.Parameters.AddWithValue("@user_invitation_guid", uig);
                cmd.Parameters.AddWithValue("@clean_id_json", CleanIDJson);
                cmd.Parameters.AddWithValue("@ipqs_json", IPQSJson);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.ExecuteNonQuery();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["rdurl"] != DBNull.Value)
                        {
                            objUser.RedirectURL = Convert.ToString(dr["rdurl"]);
                        }
                        if (dr["is_gdpr_compliance"] != DBNull.Value)
                        {
                            objUser.IsGDPRCompliance = Convert.ToBoolean(dr["is_gdpr_compliance"]);
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
            return objUser;
        }

        #region Get Client Details By Rid
        /// <summary>
        /// Get Client Details By Rid
        /// </summary>
        /// <param name="RId">rid</param>
        /// <returns></returns>
        public Client GetClientDetailsByRid(string MemberUrl, int? RId = null, int? ClientId = null)
        {
            Client oClient = new Client();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[user].[client_details_get]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("referrer_id", RId);
                cmd.Parameters.AddWithValue("client_id", ClientId);
                if (!string.IsNullOrEmpty(MemberUrl))
                {
                    cmd.Parameters.AddWithValue("domain_url", MemberUrl);
                }
                else
                {
                    cmd.Parameters.AddWithValue("domain_url", DBNull.Value);
                }

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["org_id"] != DBNull.Value)
                        {
                            oClient.ClientId = Convert.ToInt32(dr["org_id"]);
                        }
                        if (dr["org_logo"] != DBNull.Value)
                        {
                            oClient.OrgLogo = Convert.ToString(dr["org_logo"]);
                        }
                        if (dr["org_name"] != DBNull.Value)
                        {
                            oClient.OrgName = Convert.ToString(dr["org_name"]);
                        }
                        if (dr["member_url"] != DBNull.Value)
                        {
                            oClient.MemberUrl = Convert.ToString(dr["member_url"]);
                        }
                        if (dr["referrer_id"] != DBNull.Value)
                        {
                            oClient.Referrerid = Convert.ToInt32(dr["referrer_id"]);
                        }
                        if (dr["org_type_id"] != DBNull.Value)
                        {
                            oClient.OrgTypeId = Convert.ToInt32(dr["org_type_id"]);
                        }
                        if (dr["member_url"] != DBNull.Value)
                        {
                            oClient.MemberUrl = dr["member_url"].ToString();
                        }
                        if (dr["emailaddress"] != DBNull.Value)
                        {
                            oClient.Emailaddress = dr["emailaddress"].ToString();
                        }
                        if (dr["mg_login_path"] != DBNull.Value)
                        {
                            oClient.MgLoginPath = dr["mg_login_path"].ToString();
                        }
                        if (dr["password"] != DBNull.Value)
                        {
                            oClient.Password = dr["password"].ToString();
                        }
                        if (dr["copyright_year"] != DBNull.Value)
                        {
                            oClient.CopyrightYear = Convert.ToInt32(dr["copyright_year"]);
                        }
                        if (dr["postal_address"] != DBNull.Value)
                        {
                            oClient.Address = dr["postal_address"].ToString();
                        }
                        if (dr["aboutus_text"] != DBNull.Value)
                        {
                            oClient.AboutusText = dr["aboutus_text"].ToString();
                        }
                        if (dr["theem"] != DBNull.Value)
                        {
                            oClient.StyleSheettheme = dr["theem"].ToString();
                        }
                        if (dr["home_page_url"] != DBNull.Value)
                        {
                            oClient.HomePageURL = dr["home_page_url"].ToString();
                        }
                        if (dr["is_popup"] != DBNull.Value)
                        {
                            oClient.IsPopUp = Convert.ToBoolean(dr["is_popup"].ToString());
                        }
                        if (dr["is_profile_pixel"] != DBNull.Value)
                        {
                            oClient.IsProfilePixel = Convert.ToBoolean(dr["is_profile_pixel"].ToString());
                        }
                        if (dr["is_survey_pixel"] != DBNull.Value)
                        {
                            oClient.IsSurveyPixel = Convert.ToBoolean(dr["is_survey_pixel"].ToString());
                        }
                        if (dr["profile_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileClickPixelUrl = dr["profile_click_pixel_url"].ToString();
                        }
                        if (dr["survey_click_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyClickPixelUrl = dr["survey_click_pixel_url"].ToString();
                        }
                        if (dr["profile_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.ProfileCompletePixelUrl = dr["profile_complete_pixel_url"].ToString();
                        }
                        if (dr["survey_complete_pixel_url"] != DBNull.Value)
                        {
                            oClient.SurveyCompletePixelUrl = dr["survey_complete_pixel_url"].ToString();
                        }
                        if (dr["is_verity_required"] != DBNull.Value)
                        {
                            oClient.VerityRequired = Convert.ToBoolean(dr["is_verity_required"].ToString());
                        }
                        if (dr["is_relevantid_required"] != DBNull.Value)
                        {
                            oClient.RelevantIdRequired = Convert.ToBoolean(dr["is_relevantid_required"].ToString());
                        }
                        if (dr["IsStep1Enable"] != DBNull.Value)
                        {
                            oClient.IsStep1Enable = Convert.ToBoolean(dr["IsStep1Enable"]);
                        }
                        if (dr["is_standalone_partner"] != DBNull.Value)
                        {
                            oClient.IsStandalone = Convert.ToBoolean(dr["is_standalone_partner"]);
                        }
                        if (dr["is_show_rewards"] != DBNull.Value)
                        {
                            oClient.IsRewardsShow = Convert.ToBoolean(dr["is_show_rewards"]);
                        }
                        if (dr["partner_survey_term_url"] != DBNull.Value)
                        {
                            oClient.PartnerTerminateUrl = Convert.ToString(dr["partner_survey_term_url"]);
                        }
                        if (dr["is_top10_enable"] != DBNull.Value)
                        {
                            oClient.IsTop10Enable = Convert.ToBoolean(dr["is_top10_enable"]);
                        }
                        if (dr["is_email_invitation"] != DBNull.Value)
                        {
                            oClient.IsEmailInvitationEnable = Convert.ToBoolean(dr["is_email_invitation"]);
                        }
                        if (dr["is_sms_invitation"] != DBNull.Value)
                        {
                            oClient.IsSmsInvitation = Convert.ToBoolean(dr["is_sms_invitation"]);
                        }
                        if (dr["mg_step2_path"] != DBNull.Value)
                        {
                            oClient.MgStep2Path = Convert.ToString(dr["mg_step2_path"]);
                        }
                        if (dr["reward_text_type"] != DBNull.Value)
                        {
                            oClient.RewardTextType = Convert.ToString(dr["reward_text_type"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                con.Close();
            }
            return oClient;
        }
        #endregion

        #region GetApiInfo
        public APInfo GetApInfo()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConnectionString;
            APInfo info = null;
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[partner].[get_api_info]", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    info = new APInfo();
                    if (dr["api_partner_id"] != DBNull.Value)
                    {
                        info.APIPartnerId = Convert.ToInt32(dr["api_partner_id"]);
                    }
                    if (dr["api_partner_name"] != DBNull.Value)
                    {
                        info.APIPartnerName = dr["api_partner_name"].ToString();
                    }
                    if (dr["prod_api_url"] != DBNull.Value)
                    {
                        info.ProdAPIURL = dr["prod_api_url"].ToString();
                    }
                    if (dr["api_user_name"] != DBNull.Value)
                    {
                        info.UserName = dr["api_user_name"].ToString();
                    }
                    if (dr["api_password"] != DBNull.Value)
                    {
                        info.Password = dr["api_password"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error occured while getting GetApInfo detials|" + ex);
            }
            finally
            {
                con.Close();
            }
            return info;
        }

        #endregion

        #region GetOutSideProjectId
        public string GetOutSideProjectId(string ProjectId)
        {
            string outside_panel_project_id = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[outside_panel_project_id_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@project_id", ProjectId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        if (dr["outside_panel_project_id"] != DBNull.Value)
                        {
                            outside_panel_project_id = Convert.ToString(dr["outside_panel_project_id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                cn.Close();
            }
            return outside_panel_project_id;
        }
        #endregion
    }
}
