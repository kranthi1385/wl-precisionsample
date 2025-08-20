using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Common.Security;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;


namespace Members.PrecisionSample.Components.Data_Layer
{
    public class RiverDataServices
    {
        #region Connection
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString3"].ToString();
            }
        }

        #endregion

        #region Get User Account Details

        public MemberEntity GetMemberDetails(Guid UserGuid)
        {

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            MemberEntity oMemberEntity = new MemberEntity();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[user_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                if (UserGuid != Guid.Empty)
                {
                    cm.Parameters.AddWithValue("@user_guid", UserGuid);
                }
                else
                {
                    cm.Parameters.AddWithValue("@user_guid", DBNull.Value);
                }
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email_address"] != DBNull.Value)
                        {
                            oMemberEntity.EmailAddress = Convert.ToString(reader["email_address"]);
                        }

                        if (reader["country_id"] != DBNull.Value)
                        {
                            oMemberEntity.CountryCode = Convert.ToString(reader["country_id"]);
                        }

                        if (reader["gender"] != DBNull.Value)
                        {
                            oMemberEntity.Gender = Convert.ToString(reader["gender"]);
                        }

                        if (reader["dob"] != DBNull.Value)
                        {
                            oMemberEntity.Dob = Convert.ToString(reader["dob"]);
                        }

                        if (reader["zip_code"] != DBNull.Value)
                        {
                            oMemberEntity.ZipCode = Convert.ToString(reader["zip_code"]);
                        }
                        if (reader["ethnicity_id"] != DBNull.Value)
                        {
                            oMemberEntity.EthnicityId = Convert.ToInt32(reader["ethnicity_id"]);
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            oMemberEntity.UserGuid = new Guid(reader["user_guid"].ToString());
                        }
                        if (reader["first_name"] != DBNull.Value)
                        {
                            oMemberEntity.FirstName = Convert.ToString(reader["first_name"]);
                        }
                        if (reader["last_name"] != DBNull.Value)
                        {
                            oMemberEntity.LastName = Convert.ToString(reader["last_name"]);
                        }
                        if (reader["address1"] != DBNull.Value)
                        {
                            oMemberEntity.Address1 = Convert.ToString(reader["address1"]);
                        }
                        if (reader["city"] != DBNull.Value)
                        {
                            oMemberEntity.City = Convert.ToString(reader["city"]);
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
            return oMemberEntity;
        }
        #endregion

        #region Save User Details
        public string SaveUserDetails(MemberEntity oMemberEntity)
        {
            string UserId = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[user_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", oMemberEntity.UserGuid);
                cm.Parameters.AddWithValue("@country_id", oMemberEntity.CountryCode);
                cm.Parameters.AddWithValue("@first_name", oMemberEntity.FirstName);
                cm.Parameters.AddWithValue("@last_name", oMemberEntity.LastName);
                cm.Parameters.AddWithValue("@email_address", oMemberEntity.EmailAddress);
                cm.Parameters.AddWithValue("@address1", oMemberEntity.Address1);
                cm.Parameters.AddWithValue("@city", oMemberEntity.City);
                cm.Parameters.AddWithValue("@gender", oMemberEntity.Gender);
                cm.Parameters.AddWithValue("@dob", oMemberEntity.Dob);
                cm.Parameters.AddWithValue("@zip_code", oMemberEntity.ZipCode);
                cm.Parameters.AddWithValue("@ethnicity_id", oMemberEntity.EthnicityId);
                cm.Parameters.AddWithValue("@geo_ip2_content", oMemberEntity.GeoIp2Content);
                using (IDataReader dr = cm.ExecuteReader())
                {

                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            UserId = Convert.ToString(dr["user_guid"]);
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
            return UserId;

        }
        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid)
        {
            List<ProfileQuestions> lstParentQuestion = new List<ProfileQuestions>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top10_questions_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.CommandTimeout = 120;
                using (IDataReader dr = cm.ExecuteReader())
                {
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
            return lstParentQuestion;
        }
        #endregion

        #region Top10SaveOptions
        /// <summary>
        /// Top10SaveOptions
        /// </summary>
        /// <param name="listXml"></param>
        public void Top10SaveOptions(string listXml)
        {

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top10_questions_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
                cm.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }


        }
        #endregion

        #region Insert User Details
        /// <summary>
        /// Insert User Details
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        public string InsertUserDetails(User oUser)
        {
            string UserId = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("river.user_insert", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region
                cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                cm.Parameters.AddWithValue("@referrer_id", oUser.Rid);
                cm.Parameters.AddWithValue("@sub_referrer_id", oUser.SubId2);
                cm.Parameters.AddWithValue("@sub_id", oUser.ExtId);
                cm.Parameters.AddWithValue("@country_code", oUser.CountryCode);
                cm.Parameters.AddWithValue("@ip_address", oUser.IpAddress);
                cm.Parameters.AddWithValue("@transaction_id", oUser.TransactionId);
                #endregion
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            UserId = Convert.ToString(dr["user_guid"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            UserId = UserId + ";" + Convert.ToString(dr["country_code"]);
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
            return UserId;
        }
        #endregion

        #region Insert User Details
        public string CheckMemberExistence(User oUser)
        {
            string UserId = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("river.user_check", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@email_address", oUser.EmailAddress);
                cm.Parameters.AddWithValue("@referrer_id", oUser.Rid);
                cm.Parameters.AddWithValue("@sub_id", oUser.ExtId);
                cm.Parameters.AddWithValue("@transaction_id", oUser.TransactionId);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            UserId = Convert.ToString(dr["user_guid"]);
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
            return UserId;
        }
        #endregion
        #region Update Relevant and VerityData
        /// <summary>
        ///  Update Relevant and VerityData
        /// </summary>
        /// <param name="oUser"></param>
        public void UpdateRelevantandVerityData(User oUser)
        {
            //User oUser = new User();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[user_relevant_info_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", oUser.UserGuid);
                cm.Parameters.AddWithValue("@relevant_id", oUser.RelevantId);
                cm.Parameters.AddWithValue("@relevant_score", oUser.RelevantScore);
                cm.Parameters.AddWithValue("@relevant_profile_score", oUser.RelevantProfileScore);
                cm.Parameters.AddWithValue("@fpf_scores", oUser.FpfScore);
                cm.Parameters.AddWithValue("@is_new", oUser.IsNew);
                cm.Parameters.AddWithValue("@rdjson", oUser.Rdjson);
                //if (oUser.GeoCorrelationFlag == -1)
                //    cm.Parameters.AddWithValue("@geo_corelation_flag", DBNull.Value);

                //else
                //    cm.Parameters.AddWithValue("@geo_corelation_flag", oUser.GeoCorrelationFlag);
                //if (oUser.VerityScore == -1)
                //{
                //    cm.Parameters.AddWithValue("@verity_score", DBNull.Value);
                //    cm.Parameters.AddWithValue("@verity_id", DBNull.Value);
                //}
                //else
                //{
                //    cm.Parameters.AddWithValue("@verity_score", oUser.VerityScore);
                //    cm.Parameters.AddWithValue("@verity_id", oUser.VerityId);
                //}

                cm.ExecuteReader();
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

        #region Get Project Details
        /// <summary>
        /// GetProjectDetails
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public string GetProjectDetails(Guid UserGuid)
        {
            string ReleventPageURL = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            User oUser = new User();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[top1_survey_matched_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["entry_page_url"] != DBNull.Value)
                        {
                            ReleventPageURL = Convert.ToString(reader["entry_page_url"]);
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
            return ReleventPageURL;
        }
        #endregion

        #region Get Survey Details
        /// <summary>
        /// Get Survey URL
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="QuotaGroupGuid"></param>
        /// <param name="RelevantId"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="FPFScore"></param>
        /// <param name="IpAddress"></param>
        /// <param name="BrowserInfo"></param>
        /// <param name="AgentInfo"></param>
        /// <param name="IsNew"></param>
        /// <param name="RelevantFlag"></param>
        /// <param name="Ip2ContryFlag"></param>
        /// <returns></returns>
        public string GetSurveyURL(Guid UserGuid, Guid QuotaGroupGuid, string RelevantId, int RelevantScore, string FPFScore, string IpAddress, string BrowserInfo, string AgentInfo, int IsNew, int RelevantFlag, int Ip2ContryFlag)
        {
            string ReleventPageURL = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            User oUser = new User();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[survey_url_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;

                cm.Parameters.AddWithValue("@quota_group_guid", QuotaGroupGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@fpf_scores", FPFScore);
                cm.Parameters.AddWithValue("@browser_info", BrowserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@is_new", IsNew);
                cm.Parameters.AddWithValue("@relevant_flag", RelevantFlag);
                cm.Parameters.AddWithValue("@ip2country_flag", Ip2ContryFlag);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (reader["redirect_url"] != DBNull.Value)
                        {
                            ReleventPageURL = Convert.ToString(reader["redirect_url"]);
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
            return ReleventPageURL;
        }
        #endregion

        #region Insert UserInvitaiton
        /// <summary>
        /// Insert User Invitaiton
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="QuotaGroupGuid"></param>
        /// <param name="RelevantId"></param>
        /// <param name="RelevantScore"></param>
        /// <param name="FPFScore"></param>
        /// <param name="IpAddress"></param>
        /// <param name="BrowserInfo"></param>
        /// <param name="AgentInfo"></param>
        /// <param name="IsNew"></param>
        /// <param name="RelevantFlag"></param>
        /// <param name="Ip2ContryFlag"></param>
        /// <returns></returns>
        public River InsertUserInvitaiton(Guid UserGuid, Guid QuotaGroupGuid, string RelevantId, int RelevantScore, string FPFScore, string IpAddress, string BrowserInfo, string AgentInfo, int IsNew, int RelevantFlag, int Ip2ContryFlag, int UserTrafficType, string DeviceModel, string CleanID)
        {
            string ReleventPageURL = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            River oRiver = new River();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[user_invitation_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@target_guid", QuotaGroupGuid);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@user_traffic_type_id", UserTrafficType);
                cm.Parameters.AddWithValue("@mobile_device_model", DeviceModel);
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                cm.Parameters.AddWithValue("@relevant_id", RelevantId);
                cm.Parameters.AddWithValue("@relevant_score", RelevantScore);
                cm.Parameters.AddWithValue("@fpf_scores", FPFScore);
                cm.Parameters.AddWithValue("@browser_info", BrowserInfo);
                cm.Parameters.AddWithValue("@agent_info", AgentInfo);
                cm.Parameters.AddWithValue("@is_new", IsNew);
                cm.Parameters.AddWithValue("@relevant_flag", RelevantFlag);
                cm.Parameters.AddWithValue("@ip2country_flag", Ip2ContryFlag);
                cm.Parameters.AddWithValue("@json", CleanID);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["redirect_url"] != DBNull.Value)
                        {
                            oRiver.RedirectUrl = Convert.ToString(reader["redirect_url"]);
                        }
                        if (reader["user_invitation_id"] != DBNull.Value)
                        {
                            oRiver.UserInvitationId = Convert.ToInt64(reader["user_invitation_id"]);
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            oRiver.UserId = Convert.ToInt32(reader["user_id"]);
                        }
                        if (reader["target_id"] != DBNull.Value)
                        {
                            oRiver.TargetId = Convert.ToInt32(reader["target_id"]);
                        }
                        if (reader["activity_dt"] != DBNull.Value)
                        {
                            oRiver.ActivityDate = Convert.ToDateTime(reader["activity_dt"]);
                        }
                        if (reader["ip_address"] != DBNull.Value)
                        {
                            oRiver.IpAddress = Convert.ToString(reader["ip_address"]);
                        }
                        if (reader["org_id"] != DBNull.Value)
                        {
                            oRiver.OrgId = Convert.ToInt32(reader["org_id"]);
                        }
                        if (reader["internal_member"] != DBNull.Value)
                        {
                            oRiver.InternalMember = Convert.ToInt32(reader["internal_member"]);
                        }
                        if (reader["user_invitation_guid"] != DBNull.Value)
                        {
                            oRiver.UserInvitationGuid = new Guid(reader["user_invitation_guid"].ToString());
                        }
                        if (reader["project_id"] != DBNull.Value)
                        {
                            oRiver.ProjectId = Convert.ToInt32(reader["project_id"]);
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
            return oRiver;
        }
        #endregion

        #region Get SurveyUrl
        /// <summary>
        /// Get Survey Url
        /// </summary>
        /// <param name="UserInvitationGuid"></param>
        /// <returns></returns>
        public string GetSurveyUrl(Guid UserInvitationGuid)
        {
            string RedirectUrl = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            User oUser = new User();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[pms].[prescreener_surveyurl_by_userinvitationguid_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitationGuid);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (reader["active_project_url"] != DBNull.Value)
                        {
                            RedirectUrl = Convert.ToString(reader["active_project_url"]);
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
            return RedirectUrl;
        }
        #endregion

        #region Update UserInvitation Details
        /// <summary>
        /// Update UserInvitation Details
        /// </summary>
        /// <param name="UserInvitaionGuid"></param>
        /// <param name="UserStatusGuid"></param>
        /// <returns></returns>
        public string UpdateUserInvitationDetails(Guid UserInvitaionGuid, Guid UserStatusGuid)
        {
            //User oUser = new User();
            string PixelUrl = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[river].[user_invitation_status_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_status_guid", UserStatusGuid);
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitaionGuid);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["prelim_complete_pixel"] != DBNull.Value)
                        {
                            PixelUrl = Convert.ToString(dr["prelim_complete_pixel"]);
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
            return PixelUrl;
        }

        #endregion

        #region ClickInsert

        /// <summary>
        /// Click Insert
        /// </summary>
        /// <param name="Rid"></param>
        /// <param name="Sid"></param>
        /// <param name="ReferrerUrl"></param>
        /// <param name="IpAddress"></param>
        /// <param name="SubId3"></param>
        /// <returns></returns>
        public int ClickInsert(int Rid, string Sid, string ReferrerUrl, string IpAddress, string SubId3, string TransactionId)
        {
            //User oUser = new User();
            int ClickId = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[ams].[click_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", Rid);
                cm.Parameters.AddWithValue("@sub_id2", Sid);
                cm.Parameters.AddWithValue("@referrer_url", ReferrerUrl);
                cm.Parameters.AddWithValue("@ip_address", IpAddress);
                cm.Parameters.AddWithValue("@sub_id3", SubId3);
                cm.Parameters.AddWithValue("@transaction_id", TransactionId);

                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["click_id"] != DBNull.Value)
                        {
                            ClickId = Convert.ToInt32(dr["click_id"]);
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
            return ClickId;
        }

        #endregion


        #region Insert Lead

        /// <summary>
        /// Insert Lead
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="SubReferrerCode"></param>
        /// <param name="PhoneNo"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        public string InsertLead(int ReferrerId, string SubReferrerCode, string PhoneNo, string Ipaddress)
        {
            string _result = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[dbo].[lead_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", ReferrerId);
                cm.Parameters.AddWithValue("@sub_referrer_code", SubReferrerCode);
                cm.Parameters.AddWithValue("@phone_number", PhoneNo);
                cm.Parameters.AddWithValue("@ip_address", Ipaddress);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            _result = Convert.ToString(dr["result"]);
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
            return _result;

        }
        #endregion

        #region Insert top 10page member skip page click log
        /// <summary>
        /// Insert Top10 PageSkipLog
        /// </summary>
        /// <param name="UserGuid"></param>
        public void InsertTop10PageSkipLog(Guid UserGuid)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top10_skip]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.ExecuteNonQuery();

            }

            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                cn.Close();
            }

        }
        #endregion

        #region inserting the postback transactions
        /// <summary>
        /// InsertPosback Transactions
        /// </summary>
        /// <param name="UserInvitaionGuid"></param>
        /// <param name="UserGuid"></param>
        /// <param name="ApiResponse"></param>
        /// <param name="PixelUrl"></param>
        public void InsertPosbackTransactions(Guid UserInvitaionGuid, Guid UserGuid, string ApiResponse, string PixelUrl)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[river_postback_pixel_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@user_invitation_guid", UserInvitaionGuid);
                cm.Parameters.AddWithValue("@pixel_url", PixelUrl);
                cm.Parameters.AddWithValue("@api_response", ApiResponse);
                cm.ExecuteReader();
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

        public int GetReferrerDetails(int ReferrerId, string SubReferrer)
        {
            int _result = 0;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[api].[user_insertion_related_data_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@referrer_id", ReferrerId);
                if (!string.IsNullOrEmpty(SubReferrer))
                    cm.Parameters.AddWithValue("@sub_referrer_code", SubReferrer);
                else
                    cm.Parameters.AddWithValue("@sub_referrer_code", string.Empty);
                cm.Parameters.AddWithValue("@is_sub_affliate", 1);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["sub_referrer_id"] != DBNull.Value)
                        {
                            _result = Convert.ToInt32(dr["sub_referrer_id"]);
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
            return _result;
        }


        public void SurveyActivityInsert(River oRiver)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringActivity"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_activity_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_invitation_id", oRiver.UserInvitationId);
                cm.Parameters.AddWithValue("@internal_member", oRiver.InternalMember);
                cm.Parameters.AddWithValue("@user_id", oRiver.UserId);
                cm.Parameters.AddWithValue("@user_invitation_guid", oRiver.UserInvitationGuid);
                cm.Parameters.AddWithValue("@project_id", oRiver.ProjectId);
                cm.Parameters.AddWithValue("@target_id", oRiver.TargetId);
                cm.Parameters.AddWithValue("@activity_type_id", oRiver.ActivityTypeId);
                cm.Parameters.AddWithValue("@activity_dt", oRiver.ActivityDate);
                cm.Parameters.AddWithValue("@org_id", oRiver.OrgId);
                cm.Parameters.AddWithValue("@ip_address", oRiver.IpAddress);
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
    }
}
