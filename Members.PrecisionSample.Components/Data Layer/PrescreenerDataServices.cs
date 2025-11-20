using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class PrescreenerDataServices
    {

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PrescreenerConnectionString"].ConnectionString);
        #region public varables Connection
        /// <summary>
        /// Connection string Url
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return Convert.ToString(ConfigurationManager.ConnectionStrings["PrescreenerConnectionString"]);
            }
        }

        ProfileQuestionDataLayer oPfDataServer = new ProfileQuestionDataLayer();
        #endregion

        #region Get User MobileNumber
        /// <summary>
        /// Check User Mobile Number
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        /// <returns></returns>
        public Surveys GetUserMobileNumber(Guid UserGuid, Guid UserInvitationGuid, int ClientId)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServer = new UserDataServices();
            string constr = string.Empty;
            SqlConnection cn = new SqlConnection();
            if (UserGuid == UserInvitationGuid)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
                cn.ConnectionString = constr;
            }
            else
            {
                constr = oDataServer.GetConnectionString(null, null, ClientId);
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            }

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_step5_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["is_mobileno_from_ps"] != DBNull.Value)
                        {
                            oSurvey.IsMobileNumberExist = Convert.ToInt32(dr["is_mobileno_from_ps"]);
                        }
                        if (dr["is_verity_required"] != DBNull.Value)
                        {
                            oSurvey.IsrequireVerityValidatedMembers = Convert.ToBoolean(dr["is_verity_required"]);
                        }
                        if (dr["org_logo"] != DBNull.Value)
                        {
                            oSurvey.OrgLogo = Convert.ToString(dr["org_logo"]);
                        }
                        if (dr["is_standalone_partner"] != DBNull.Value)
                        {
                            oSurvey.IsStandalone = Convert.ToBoolean(dr["is_standalone_partner"]);
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

        #region Save User MobileNumber
        /// <summary>
        /// Check User Mobile Number
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        ///<param name="MobileNumber">Mobile Number</param>
        /// <returns></returns>
        public Surveys UpdateUserMobileNumber(Guid UserGuid, int ClientId, Guid UserInvitationGuid, string MobileNumber)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            string constr = string.Empty;
            if (UserGuid == UserInvitationGuid)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
                cn.ConnectionString = constr;
            }
            else
            {
                constr = oDataServer.GetConnectionString(null, null, ClientId);
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            }
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_step5_mobileps_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@phone_no", MobileNumber);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oSurvey.UserInvitationId = new Guid(dr["current_guid"].ToString());
                        }
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oSurvey.RedirectUrl = Convert.ToString(dr["redirect_url"].ToString());
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

        #region Get project selected languages
        /// <summary>
        /// Get project selected languages
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="UserInvitationGuid">User Invitation Guid</param>
        /// <returns></returns>
        public Question GetProjectSelectedLanguages(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            Question oQuestion = new Question();
            UserDataServices oDataServer = new UserDataServices();
            string constr = string.Empty;
            SqlConnection cn = new SqlConnection();
            if (UserGuid == UserInvitationGuid)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
                cn.ConnectionString = constr;
            }
            else
            {
                constr = oDataServer.GetConnectionString(null, null, ClientId);
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            }

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_2_survey_click_step6_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        Options objOptions = new Options();
                        if (dr["current_guid"] != DBNull.Value)
                        {
                            oQuestion.UserInvitationGuid = new Guid(dr["current_guid"].ToString());
                        }

                        if (dr["header"] != DBNull.Value)
                        {
                            objOptions.LanguageHeader = Convert.ToString(dr["header"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32((dr["option_id"]));
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString((dr["option_text"]));
                        }
                        //if (dr["pending_question_count"] != DBNull.Value)
                        //{
                        //    objOptions.PendingQuestionCount = Convert.ToInt32(dr["pending_questions_count"]);
                        //}
                        if (dr["source"] != DBNull.Value)
                        {
                            objOptions.Source = Convert.ToString(dr["source"]);
                        }
                        if (dr["zip_code"] != DBNull.Value)
                        {
                            objOptions.ZIPCode = Convert.ToString(dr["zip_code"]);
                        }
                        if (dr["is_ipsos_project"] != DBNull.Value)
                        {
                            objOptions.IsIpsosProject = Convert.ToBoolean(dr["is_ipsos_project"]);
                        }
                        oQuestion.SelectedOptions.Add(objOptions);
                        //lstSelectedOptions.Add(objOptions);
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
            return oQuestion;
        }
        #endregion

        #region Get Prescreener Questions
        /// <summary>
        /// Get Prescreener Questions
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <param name="Xml">Answer XMl</param>
        /// <param name="QueryName">QueryName</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="QId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="Type">Type</param>
        /// <returns></returns>
        public List<PSquestion> GetQuestions(Guid UserGuid, int ClientId, Guid UserInvitationGuid, string Xml, string QueryName, int LanguageId, int QId, int SortOrder, string Type, int GDPRValue)
        {
            List<PSquestion> lstQuestion = new List<PSquestion>();
            UserDataServices oDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            string constr = string.Empty;
            if (UserGuid == UserInvitationGuid)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
                cn.ConnectionString = constr;
            }
            else
            {
                constr = oDataServer.GetConnectionString(null, null, ClientId);
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            }
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(QueryName, cn);
                cm.CommandType = CommandType.StoredProcedure;
                if (LanguageId != 0)
                {
                    cm.Parameters.AddWithValue("@language_id", LanguageId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@language_id", DBNull.Value);
                }
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                if (Type == "save")
                {
                    cm.Parameters.AddWithValue("@xml", Xml);
                    cm.Parameters.AddWithValue("@current_question_id", QId);
                    cm.Parameters.AddWithValue("@current_question_sort_order", SortOrder);
                    cm.Parameters.AddWithValue("@gdpr", GDPRValue);
                }

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    List<PSoptions> lstOptions = new List<PSoptions>();
                    List<PSquestion> lstChildQuestions = new List<PSquestion>();
                    List<PSoptions> lstChildQuestionsOptions = new List<PSoptions>();
                    List<PSquestion> lstChldQoMaping = new List<PSquestion>();
                    List<PSquestion> lstQuestionResponses = new List<PSquestion>();
                    List<PSquestion> lstSubChildQuestions = new List<PSquestion>();
                    List<PSoptions> lstSubChildQuestionsOptions = new List<PSoptions>();
                    while (dr.Read())
                    {
                        PSquestion oQuestion = new PSquestion();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            oQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            oQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            oQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["min_no_options"] != DBNull.Value)
                        {
                            oQuestion.MinQuestionsCount = Convert.ToInt32(dr["min_no_options"]);
                        }
                        if (dr["max_no_options"] != DBNull.Value)
                        {
                            oQuestion.MaxQuestionsCount = Convert.ToInt32(dr["max_no_options"]);
                        }
                        if (dr["sort_order"] != DBNull.Value)
                        {
                            oQuestion.CurrentSortOrder = Convert.ToInt32(dr["sort_order"]);
                        }
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oQuestion.RedirectUrl = Convert.ToString(dr["redirect_url"]);
                        }
                        if (dr["session_count"] != DBNull.Value)
                        {
                            oQuestion.SessionCount = Convert.ToInt32(dr["session_count"]);
                        }
                        lstQuestion.Add(oQuestion);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildQuestions Get
                        PSquestion objChildQuestion = new PSquestion();

                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }

                        if (dr["question_text"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["min_no_options"] != DBNull.Value)
                        {
                            objChildQuestion.MinQuestionsCount = Convert.ToInt32(dr["min_no_options"]);
                        }

                        if (dr["max_no_options"] != DBNull.Value)
                        {
                            objChildQuestion.MaxQuestionsCount = Convert.ToInt32(dr["max_no_options"]);
                        }
                        if (dr["sort_order"] != DBNull.Value)
                        {
                            objChildQuestion.CurrentSortOrder = Convert.ToInt32(dr["sort_order"]);
                        }
                        lstChildQuestions.Add(objChildQuestion);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        PSquestion oquestion = new PSquestion();
                        PSoptions objOptions = new PSoptions();
                        foreach (PSquestion qu in lstQuestion)
                        {
                            if (qu.QuestionId == Convert.ToInt32(dr["question_id"]))
                            {
                                oquestion = qu;
                                break;
                            }

                        }
                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {

                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        oquestion.OptionTypeId = objOptions.OptionTypeId;
                        lstOptions.Add(objOptions);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildOptions
                        PSoptions objOptions = new PSoptions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        if (dr["special_grouping_id"] != DBNull.Value)
                        {
                            objOptions.SpecialGroupingId = Convert.ToInt32(dr["special_grouping_id"]);
                        }
                        lstChildQuestionsOptions.Add(objOptions);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //mapping for parent Question and child Question
                        PSquestion objChildQoOptionMaping = new PSquestion();
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }

                        if (dr["option_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.OptionId = Convert.ToInt32(dr["option_id"]);
                        }

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        lstChldQoMaping.Add(objChildQoOptionMaping);

                    }

                    //mapping data insert into parent question options
                    if (lstChldQoMaping.Count > 0)
                    {
                        foreach (PSoptions parentOptions in lstOptions)
                        {
                            List<int> lstChildHideQst = new List<int>();
                            foreach (PSquestion parentQomaping in lstChldQoMaping)
                            {
                                if (parentQomaping.OptionId == parentOptions.OptionId)
                                {
                                    lstChildHideQst.Add(parentQomaping.QuestionId);

                                }
                            }
                            parentOptions.ListChildQuestionId = lstChildHideQst;

                        }
                    }

                    foreach (PSquestion oQst in lstQuestion)
                    {
                        if (lstOptions.Count > 0)
                        {
                            oQst.OptionList = lstOptions;
                        }
                        if (lstChildQuestions.Count > 0)
                        {
                            oQst.ChildQuestionList = lstChildQuestions;
                        }
                    }

                    foreach (PSquestion childQuestion in lstChildQuestions)
                    {
                        List<PSoptions> lstChildMapedOptions = new List<PSoptions>();
                        foreach (PSoptions childQuestionOptions in lstChildQuestionsOptions)
                        {
                            if (childQuestionOptions.QuestionId == childQuestion.QuestionId)
                            {
                                lstChildMapedOptions.Add(childQuestionOptions);
                            }
                        }
                        if (lstChildMapedOptions.Count > 0)
                        {
                            childQuestion.OptionList = lstChildMapedOptions;
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
            return lstQuestion;
        }
        #endregion

        #region Get Prescreener Question List By ProjectId
        /// <summary>
        /// Get User Prescree Questions
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public List<PSquestion> GetPSQuestions(Guid UserGuid, int ClientId, int LanguageId, Guid UserInvitationGuid)
        {
            List<PSquestion> lstQuestions = new List<PSquestion>();
            return lstQuestions = GetQuestions(UserGuid, ClientId, UserInvitationGuid, "", "[user].[prescreener_pending_question_get]", LanguageId, 0, 0, "getqst", 0);
        }

        #endregion

        #region SaveUserPrescreenerptions
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <param name="QuestionId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <returns></returns>
        public List<PSquestion> SaveUserPrescreenerOptions(string Xml, Guid UserGuid, int ClientId, Guid UserInvitationGuid, int QuestionId, int SortOrder, int LanguageId, int GDPRValue)
        {
            List<PSquestion> lstQuestions = new List<PSquestion>();
            return lstQuestions = GetQuestions(UserGuid, ClientId, UserInvitationGuid, Xml, "[user].[prescreener_responses_save]", LanguageId, QuestionId, SortOrder, "save", GDPRValue);
        }
        #endregion

        #region Get Project Reward Details
        /// <summary>
        /// Get Project Reward Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public Surveys GetProjectRewardDetails(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            string constr = string.Empty;
            if (UserGuid == UserInvitationGuid)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
                cn.ConnectionString = constr;
            }
            else
            {
                constr = oDataServer.GetConnectionString(null, null, ClientId);
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            }

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[project_details_by_user_invitation_guid]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["project_id"] != DBNull.Value)
                        {
                            oSurvey.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        if (dr["survey_name"] != DBNull.Value)
                        {
                            oSurvey.SurveyName = dr["survey_name"].ToString();
                        }
                        if (dr["survey_length"] != DBNull.Value)
                        {
                            oSurvey.SurveyLength = Convert.ToInt32(dr["survey_length"]);
                        }
                        if (dr["member_reward_text"] != DBNull.Value)
                        {
                            oSurvey.SurveyCompletereward = dr["member_reward_text"].ToString();
                        }
                        if (dr["org_type_id"] != DBNull.Value)
                        {
                            oSurvey.OrganizationTypeId = Convert.ToInt32(dr["org_type_id"]);
                        }
                        if (dr["is_enable_recaptcha"] != DBNull.Value)
                        {
                            oSurvey.EnableRecaptcha = dr["is_enable_recaptcha"].ToString();
                        }
                        if (dr["member_url"] != DBNull.Value)
                        {
                            oSurvey.MemberUrl = Convert.ToString(dr["member_url"]);
                        }
                        if (dr["uig"] != DBNull.Value)
                        {
                            oSurvey.User2PerkGuid = new Guid(Convert.ToString(dr["uig"]));
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
            return oSurvey;
        }
        #endregion


        #region get invitation guid
        /// <summary>
        ///  get invitation guid
        /// </summary>
        /// <param name="uig"></param>
        public ProfileQuestions GetInvitationGuid(Guid uig, int ClientId)
        {
            ProfileQuestions objprofile = new ProfileQuestions();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[user_invitation_guid_by_step6_guid_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", uig);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_invitation_guid"] != DBNull.Value)
                        {
                            objprofile.UserInvitationGuid = new Guid(Convert.ToString(reader["user_invitation_guid"]));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cn.Close();
            }
            return objprofile;
        }
        #endregion

        #region Get Profile Prescreener
        /// <summary>
        /// Get Prescreener Questions
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="ClientId">ClientId</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetProfilePrescreener(Guid UserGuid, int ClientId, Guid UserInvitationGuid)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top1_question_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 420;
                cm.Parameters.AddWithValue("@invitation_guid", UserInvitationGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    List<ProfileQuestions> lstParentQuestion = new List<ProfileQuestions>();
                    List<ProfileOptions> lstOptions = new List<ProfileOptions>();
                    List<ProfileQuestions> lstChildQuestions = new List<ProfileQuestions>();
                    List<ProfileOptions> lstChildQuestionsOptions = new List<ProfileOptions>();
                    List<ProfileQuestions> lstChldQoMaping = new List<ProfileQuestions>();
                    List<ProfileQuestions> lstQuestionResponses = new List<ProfileQuestions>();
                    List<ProfileQuestions> lstSubChildQuestions = new List<ProfileQuestions>();
                    List<ProfileOptions> lstSubChildQuestionsOptions = new List<ProfileOptions>();
                    ProfileQuestions oQstOrgInfo = new ProfileQuestions();
                    //patrent Questions get
                    while (dr.Read())
                    {
                        ProfileQuestions objQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objQuestion.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["is_autopostback"] != DBNull.Value)
                        {
                            objQuestion.AutoPostBack = Convert.ToInt32(dr["is_autopostback"]);
                        }
                        if (dr["question_hide"] != DBNull.Value)
                        {
                            objQuestion.QuestionHide = Convert.ToBoolean(dr["question_hide"]);
                        }
                        lstParentQuestion.Add(objQuestion);
                    }

                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildQuestions Get
                        ProfileQuestions objChildQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }

                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objChildQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objChildQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        lstChildQuestions.Add(objChildQuestion);

                    }
                    //Subchild Questions Get
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildQuestions Get
                        ProfileQuestions objSubChildQuestion = new ProfileQuestions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }

                        if (dr["question_type_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionTypeId = Convert.ToInt32(dr["question_type_id"]);
                        }
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objSubChildQuestion.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }
                        if (dr["question_text"] != DBNull.Value)
                        {
                            objSubChildQuestion.QuestionText = Convert.ToString(dr["question_text"]);
                        }
                        if (dr["question_display_type"] != DBNull.Value)
                        {
                            objSubChildQuestion.OptionDisplay = Convert.ToString(dr["question_display_type"]);
                        }
                        lstSubChildQuestions.Add(objSubChildQuestion);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //Parent Options

                        ProfileQuestions oquestion = new ProfileQuestions();
                        ProfileOptions objOptions = new ProfileOptions();
                        foreach (ProfileQuestions qu in lstParentQuestion)
                        {
                            if (qu.QuestionId == Convert.ToInt32(dr["question_id"]))
                            {
                                oquestion = qu;
                                break;
                            }
                        }
                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        oquestion.OptionList.Add(objOptions);
                        lstOptions.Add(objOptions);
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildOptions
                        ProfileOptions objOptions = new ProfileOptions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        if (dr["special_grouping_id"] != DBNull.Value)
                        {
                            objOptions.SpecialGroupingId = Convert.ToInt32(dr["special_grouping_id"]);
                        }
                        lstChildQuestionsOptions.Add(objOptions);

                    }
                    //subChildOptions
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //ChildOptions
                        ProfileOptions objSubChildjOptions = new ProfileOptions();

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        if (dr["option_text"] != DBNull.Value)
                        {
                            objSubChildjOptions.OptionText = Convert.ToString(dr["option_text"]);
                        }
                        if (dr["parent_option_id"] != DBNull.Value)
                        {
                            objSubChildjOptions.ParentOptionId = Convert.ToInt32(dr["parent_option_id"]);
                        }
                        lstSubChildQuestionsOptions.Add(objSubChildjOptions);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //mapping for parent Question and child Question
                        ProfileQuestions objChildQoOptionMaping = new ProfileQuestions();
                        if (dr["parent_question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.ParentQuestionId = Convert.ToInt32(dr["parent_question_id"]);
                        }

                        if (dr["option_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.OptionId = Convert.ToInt32(dr["option_id"]);
                        }

                        if (dr["question_id"] != DBNull.Value)
                        {
                            objChildQoOptionMaping.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        lstChldQoMaping.Add(objChildQoOptionMaping);

                    }
                    //dr.NextResult();
                    //while (dr.Read())
                    //{
                    //    //Totla Questions Response
                    //    ProfileQuestions objQuestionResponses = new ProfileQuestions();
                    //    if (dr["question_id"] != DBNull.Value)
                    //    {
                    //        objQuestionResponses.QuestionId = Convert.ToInt32(dr["question_id"]);
                    //    }
                    //    if (dr["option_id"] != DBNull.Value)
                    //    {
                    //        objQuestionResponses.OptionId = Convert.ToInt32(dr["option_id"]);
                    //    }
                    //    lstQuestionResponses.Add(objQuestionResponses);
                    //}
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //get the org information
                        if (dr["question_id"] != DBNull.Value)
                        {
                            oQstOrgInfo.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["org_info"] != DBNull.Value)
                        {
                            oQstOrgInfo.OrgInfo = Convert.ToString(dr["org_info"]);
                        }
                        if (dr["is_fraud"] != DBNull.Value)
                        {
                            oQstOrgInfo.IsFraud = Convert.ToBoolean(dr["is_fraud"]);
                        }
                        if (dr["is_show_ccpa"] != DBNull.Value)
                        {
                            oQstOrgInfo.IsShowCCPA = Convert.ToBoolean(dr["is_show_ccpa"]);
                        }
                        if (dr["language_id"] != DBNull.Value)
                        {
                            oQstOrgInfo.LanguageID = Convert.ToInt32(dr["language_id"]);
                        }
                        if (dr["user_invitation_guid"] != DBNull.Value)
                        {
                            oQstOrgInfo.UserInvitationGuid = new Guid(Convert.ToString(dr["user_invitation_guid"]));
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oQstOrgInfo.CountryID = Convert.ToInt32(dr["country_id"]);
                        }
                    }
                    if (lstParentQuestion.Count == 0)
                    {
                        lstParentQuestion.Add(oQstOrgInfo);
                    }
                    else
                    {
                        lstParentQuestion[0].OrgInfo = oQstOrgInfo.OrgInfo;
                        lstParentQuestion[0].LanguageID = oQstOrgInfo.LanguageID;
                        lstParentQuestion[0].CountryID = oQstOrgInfo.CountryID;
                    }
                    //Loop for Parent Questions responses
                    foreach (ProfileQuestions objParentQuestionsResponses in lstParentQuestion)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objParentQuestionsResponses.QuestionId == objResponses.QuestionId)
                            {
                                if (objParentQuestionsResponses.QuestionTypeId == 3)
                                {
                                    objParentQuestionsResponses.ResponseOptionList.Add(objResponses);
                                }
                                else
                                {
                                    objParentQuestionsResponses.OptionId = objResponses.OptionId;
                                }
                            }
                        }
                    }
                    //Loop for Child Questions Responses

                    foreach (ProfileQuestions objChildQuestionsResponses in lstChildQuestions)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objChildQuestionsResponses.QuestionId == objResponses.QuestionId)
                            {
                                if (objChildQuestionsResponses.QuestionTypeId == 3)
                                {
                                    objChildQuestionsResponses.ResponseOptionList.Add(objResponses);
                                }
                                else
                                {
                                    objChildQuestionsResponses.OptionId = objResponses.OptionId;
                                }
                            }
                        }
                    }
                    //Loop for SubChild Questions Responses

                    foreach (ProfileQuestions objSubChildResponses in lstSubChildQuestions)
                    {
                        foreach (ProfileQuestions objResponses in lstQuestionResponses)
                        {
                            if (objSubChildResponses.QuestionId == objResponses.QuestionId)
                            {
                                objSubChildResponses.OptionId = objResponses.OptionId;
                            }
                        }
                    }
                    // childquestions and options inserting into parent questions
                    foreach (ProfileQuestions childQuestion in lstChildQuestions)
                    {
                        foreach (ProfileQuestions parentQuestion in lstParentQuestion)
                        {
                            if (childQuestion.ParentQuestionId == parentQuestion.QuestionId)
                            {
                                foreach (ProfileOptions childQuestionOptions in lstChildQuestionsOptions)
                                {
                                    if (childQuestionOptions.QuestionId == childQuestion.QuestionId)
                                    {
                                        childQuestion.OptionList.Add(childQuestionOptions);
                                    }

                                }
                                parentQuestion.ChildQuestionList.Add(childQuestion);

                            }
                        }

                        //}

                    }

                    //subchildquestions and options inserting into Childquestions


                    foreach (ProfileQuestions SubChildQuestions in lstSubChildQuestions)
                    {
                        foreach (ProfileQuestions ChildQuestons in lstChildQuestions)
                        {
                            if (ChildQuestons.QuestionId == SubChildQuestions.ParentQuestionId)
                            {
                                foreach (ProfileOptions SubchildQuestionOptions in lstSubChildQuestionsOptions)
                                {
                                    if (SubChildQuestions.QuestionId == SubchildQuestionOptions.QuestionId)
                                    {
                                        SubChildQuestions.OptionList.Add(SubchildQuestionOptions);
                                    }
                                }
                                ChildQuestons.ChildQuestionList.Add(SubChildQuestions);
                                ChildQuestons.SubChildOptions = lstSubChildQuestionsOptions;

                            }
                        }
                    }
                    //mapping data insert into parent question options
                    foreach (ProfileOptions parentOptions in lstOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == parentOptions.OptionId)
                            {
                                parentOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //Mapping hide options list tot child options
                    foreach (ProfileOptions chOptions in lstChildQuestionsOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == chOptions.OptionId)
                            {
                                chOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //Mapping hide options list tot child options
                    foreach (ProfileOptions subChOptions in lstSubChildQuestionsOptions)
                    {
                        foreach (ProfileQuestions parentQomaping in lstChldQoMaping)
                        {
                            if (parentQomaping.OptionId == subChOptions.OptionId)
                            {
                                subChOptions.ListChildQuestionId.Add(parentQomaping.QuestionId);

                            }

                        }

                    }
                    //add responses to vechile question
                    foreach (ProfileQuestions _parentquestions in lstParentQuestion)
                    {
                        if (_parentquestions.QuestionId == 175)
                        {
                            foreach (ProfileQuestions objResponses in lstQuestionResponses)
                            {
                                _parentquestions.ResponseOptionList.Add(objResponses);
                            }
                        }
                    }
                    return lstParentQuestion;

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
        }
        #endregion

        #region Save Profile Prescreener
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public string SaveProfilePrescreener(string listXml, Guid UserGuid, Guid uig, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                 int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                 string RobotFlag, string ErrorMessage, int ClientId, string DetectedLangCode, string RelatedConfidenceScore, string RepeatedWordsPct)
        {
            string memberUrl = "";

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top1_question_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
                cm.Parameters.AddWithValue("@user_invitation_guid", uig);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@real_answer_text", ResponseText);
                cm.Parameters.AddWithValue("@ra1", Rq1);
                cm.Parameters.AddWithValue("@ra2", Rq2);
                cm.Parameters.AddWithValue("@ra3", Rq3);
                cm.Parameters.AddWithValue("@ra4", Rq4);
                cm.Parameters.AddWithValue("@real_answer_score", RealAnswerScore);
                cm.Parameters.AddWithValue("@bad_words", BadWordsFlag);
                cm.Parameters.AddWithValue("@bad_phrase_flag", BadPhraseFlag);
                cm.Parameters.AddWithValue("@garbage_words_flag", GarbageWordsFlag);
                cm.Parameters.AddWithValue("@non_engaged_flag ", NonEngagedFlag);
                cm.Parameters.AddWithValue("@pasted_text_flag", PastedTextFlag);
                cm.Parameters.AddWithValue("@robot_flag", RobotFlag);
                cm.Parameters.AddWithValue("@error_msg", ErrorMessage);
                cm.Parameters.AddWithValue("@detected_language_code", DetectedLangCode);
                cm.Parameters.AddWithValue("@related_confidence_score", RelatedConfidenceScore);
                cm.Parameters.AddWithValue("@repeated_words_pct", RepeatedWordsPct);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["member_url"] != null)
                    {
                        memberUrl = Convert.ToString(oreader["member_url"]);
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
            return memberUrl;

        }
        #endregion

        #region GetCookieData
        /// <summary>
        /// Get Project Reward Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public Surveys GetCookieData(Guid UserGuid, int ClientId)
        {
            Surveys oSurvey = new Surveys();
            UserDataServices oDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            string constr = string.Empty;
            constr = oDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[cookie_data_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["is_accessmill_cookie_dropped"] != DBNull.Value)
                        {
                            oSurvey.AccessMillCookie = Convert.ToBoolean(dr["is_accessmill_cookie_dropped"]);
                        }
                        if (dr["is_comscore_cookie_dropped"] != DBNull.Value)
                        {
                            oSurvey.ComScoreCookie = Convert.ToBoolean(dr["is_comscore_cookie_dropped"]);
                        }
                        if (dr["is_resonate_cookie_dropped"] != DBNull.Value)
                        {
                            oSurvey.ResonateCookie = Convert.ToBoolean(dr["is_resonate_cookie_dropped"]);
                        }
                        if (dr["is_tapad_cookie_dropped"] != DBNull.Value)
                        {
                            oSurvey.TapadCookie = Convert.ToBoolean(dr["is_tapad_cookie_dropped"]);
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
            return oSurvey;
        }
        #endregion

        #region Save User Cookie
        /// <summary>
        /// Get Project Reward Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <returns></returns>
        public void SaveUserCookie(Guid UserGuid, int ClientId, bool ResonateCookie, bool TapadCookie)
        {
            UserDataServices oDataServer = new UserDataServices();
            SqlConnection cn = new SqlConnection();
            string constr = string.Empty;
            constr = oDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[ccpa_cookie_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("is_resonate_cookie_dropped", ResonateCookie);
                cm.Parameters.AddWithValue("is_tapad_cookie_dropped", TapadCookie);
                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region Save Geo Data
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public Surveys saveZipcode(Guid ug, Guid uig, string zip, int ClientId)
        {
            Surveys osurveys = new Surveys();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_geo_ip2_info_save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", uig);
                cm.Parameters.AddWithValue("@user_guid", ug);
                cm.Parameters.AddWithValue("@zip_code", zip);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["redirect_url"] != DBNull.Value)
                    {
                        osurveys.RedirectUrl = Convert.ToString(oreader["redirect_url"]);
                    }
                    if (oreader["user_invitation_guid"] != DBNull.Value)
                    {
                        osurveys.ActualInvitationGuid = new Guid(Convert.ToString(oreader["user_invitation_guid"]));
                    }
                    if (oreader["zip_radius"] != DBNull.Value)
                    {
                        osurveys.ZipRadius = Convert.ToDecimal(oreader["zip_radius"]);
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
            return osurveys;

        }
        #endregion
    }
}
