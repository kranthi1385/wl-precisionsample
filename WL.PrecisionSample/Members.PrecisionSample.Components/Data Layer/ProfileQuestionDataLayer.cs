using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class ProfileQuestionDataLayer
    {

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }

        #region Get Top1 pre prescreener questions
        /// <summary>
        /// Get Top1 pre prescreener questions
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public List<Question> GetQuestion(Guid Uig, Guid Ug, int ProjectId, int TargetId, int UserId, int ClientId)
        {
            string sp = string.Empty;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            List<Question> questlst = new List<Question>();
            if (Ug == Uig)
                sp = "[pms].[user_2_survey_click_step2_external]";
            else
                sp = "[pms].[user_2_survey_click_step2_insert]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@invitation_guid", Uig);
                cm.Parameters.AddWithValue("@project_id", ProjectId);
                cm.Parameters.AddWithValue("@target_id", TargetId);
                cm.Parameters.AddWithValue("@user_id", UserId);
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
                    if (oreader["current_guid"] != DBNull.Value)
                    {
                        obj.UserInvitationGuid = new Guid(oreader["current_guid"].ToString());
                    }
                    if (oreader["state_question"] != DBNull.Value)
                    {
                        obj.StateQuestion = Convert.ToBoolean(oreader["state_question"]);
                    }
                    if (oreader["city_question"] != DBNull.Value)
                    {
                        obj.CityQuestion = Convert.ToBoolean(oreader["city_question"]);
                    }
                    if (oreader["zip_question"] != DBNull.Value)
                    {
                        obj.ZIPQuestion = Convert.ToBoolean(oreader["zip_question"]);
                    }
                    if (oreader["verity_score"] != DBNull.Value)
                    {
                        obj.VerityScore = Convert.ToInt32(oreader["verity_score"]);
                    }
                    if (oreader["verity_required"] != DBNull.Value)
                    {
                        obj.VerityRequired = Convert.ToBoolean(oreader["verity_required"]);
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
                    questlst.Add(obj);
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
            return questlst;
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
        public List<Question> SaveResponse(Guid Uig, int qid, string otext, int optId, Guid Ug, int ClientId, string address1, string address2, string city, string zip)
        {
            string sp = string.Empty;
            List<Question> lst = new List<Question>();
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
            //SqlDataReader oreader;
            if (Uig == Ug)
                sp = "[pms].[user_2_survey_click_step2_pii_save_ext]";
            else
                sp = "[pms].[user_2_survey_click_step2_pii_save]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
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
                cm.Parameters.AddWithValue("@invitation_guid", Uig);
                cm.Parameters.AddWithValue("@address1", address1);
                cm.Parameters.AddWithValue("@address2", address2);
                cm.Parameters.AddWithValue("@city", city);
                cm.Parameters.AddWithValue("@zip", zip);
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
                        if (oreader["state_question"] != DBNull.Value)
                        {
                            obj.StateQuestion = Convert.ToBoolean(oreader["state_question"]);
                        }
                        if (oreader["city_question"] != DBNull.Value)
                        {
                            obj.CityQuestion = Convert.ToBoolean(oreader["city_question"]);
                        }
                        if (oreader["zip_question"] != DBNull.Value)
                        {
                            obj.ZIPQuestion = Convert.ToBoolean(oreader["zip_question"]);
                        }
                        if (oreader["verity_score"] != DBNull.Value)
                        {
                            obj.VerityScore = Convert.ToInt32(oreader["verity_score"]);
                        }
                        if (oreader["verity_required"] != DBNull.Value)
                        {
                            obj.VerityRequired = Convert.ToBoolean(oreader["verity_required"]);
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
                        lst.Add(obj);
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
            return lst;

            //if user has some more basic profile question
        }
        #endregion



        #region Save Verity Repsonse
        /// <summary>
        /// Save Verity Repsonse
        /// </summary>
        /// <param name="Ug"></param>
        /// <param name="Uig"></param>
        /// <param name="verityScore"></param>
        /// <param name="verityId"></param>
        /// <param name="geoCorrelationFlag"></param>
        public void SaveVerityRepsonse(Guid Ug, Guid Uig, int verityScore, string verityId, int geoCorrelationFlag, int clientId)
        {
            string sp = string.Empty;
            DateTime DateofBirth = DateTime.MinValue;
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, clientId);
            List<User> lst = new List<User>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            if (Ug == Uig)
                sp = "[user].[userdetails_insert_ext]";
            else
                sp = "[user].[userdetails_insert]";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sp, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", Ug);
                cm.Parameters.AddWithValue("@verity_score", verityScore);
                cm.Parameters.AddWithValue("@verity_id", verityId);
                cm.Parameters.AddWithValue("@geoCorrelationFlag", geoCorrelationFlag);
                cm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                cn.Close();
            }

        }
        #endregion


        #region GetquestionsforTop10
        /// <summary>
        /// GetquestionsforTop10
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid, int ClientId)
        {

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top10_questions_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 420;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (IDataReader dr = cm.ExecuteReader())
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

                    }
                    if (lstParentQuestion.Count == 0)
                    {
                        lstParentQuestion.Add(oQstOrgInfo);
                    }
                    else
                    {
                        lstParentQuestion[0].OrgInfo = oQstOrgInfo.OrgInfo;
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

        #region Top10SaveOptions
        /// <summary>
        ///  Top10SaveOptions
        /// </summary>
        /// <param name="listXml"></param>
        public string Top10SaveOptions(string listXml, Guid UserGuid, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                 int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                 string RobotFlag, string ErrorMessage, int ClientId)
        {
            string memberUrl = "";

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[top10_questions_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
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

        #region Get Full Profile Questions
        /// <summary>
        /// GetProfileQuestions
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetProfileQuestions(Guid ProfileId, Guid UserGuid, int ClientId)
        {

            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[profile_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@profile_guid", ProfileId);
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (IDataReader dr = cm.ExecuteReader())
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
                        ProfileQuestions oqOptions = new ProfileQuestions();
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
                    dr.NextResult();
                    while (dr.Read())
                    {
                        //Totla Questions Response
                        ProfileQuestions objQuestionResponses = new ProfileQuestions();
                        if (dr["question_id"] != DBNull.Value)
                        {
                            objQuestionResponses.QuestionId = Convert.ToInt32(dr["question_id"]);
                        }
                        if (dr["option_id"] != DBNull.Value)
                        {
                            objQuestionResponses.OptionId = Convert.ToInt32(dr["option_id"]);
                        }
                        lstQuestionResponses.Add(objQuestionResponses);
                    }
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
                        if (dr["language_id"] != DBNull.Value)
                        {
                            oQstOrgInfo.LanguageID = Convert.ToInt32(dr["language_id"]);
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
                                if (objChildQuestionsResponses.QuestionTypeId == 3 || objChildQuestionsResponses.QuestionTypeId == 13)
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

        #region Save Full Profile Options
        /// <summary>
        /// Save Full Profile Options
        /// </summary>
        /// <param name="listXml"></param>
        public void ProfileSave(string listXml, Guid UserGuid, int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[profile_Save]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@xml", listXml);
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

        #region Get Profile Responses
        /// <summary>
        /// Get Profile Responses For FusionCash and Vindale
        /// </summary>
        /// <param name="UserGuid">UsetGuid</param>
        /// <param name="ClientId">ClientId</param>
        /// <param name="PfId">ProfileId</param>
        /// <returns></returns>
        public string GetProfileResponse(Guid UserGuid, int ClientId, string PfId)
        {
            string result = string.Empty;
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[user_profileresponses_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@pfid", PfId);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    //patrent Questions get
                    while (dr.Read())
                    {
                        if (dr["result"] != DBNull.Value)
                        {
                            result = Convert.ToString(dr["result"]);
                        }
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
            return result;
        }
        #endregion

        #region GetProfilePixelDetails
        /// <summary>
        /// GetProfilePixelDetails
        /// </summary>
        /// <param name="UserGuid">userGuid</param>
        /// <param name="ProfileGuid">ProfileGuid</param>
        /// <returns></returns>
        public UserEntity GetProfilePixelDetails(Guid UserGuid, int ClientId, string ProfileGuid)
        {
            UserEntity objUserEntity = new UserEntity();
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[profile_pixeldetails_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@profile_guid", ProfileGuid);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    if (oreader["sub_id3"] != DBNull.Value)
                    {
                        objUserEntity.AccessCode = Convert.ToString(oreader["sub_id3"]);
                    }
                    if (oreader["profile_name"] != DBNull.Value)
                    {
                        objUserEntity.ProfileName = Convert.ToString(oreader["profile_name"]);
                    }
                    if (oreader["profile_id"] != DBNull.Value)
                    {
                        objUserEntity.ProfileId = Convert.ToInt32(oreader["profile_id"]);
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

            return objUserEntity;
        }
        #endregion

        #region gdprCompliancesave
        /// <summary>
        /// Save Full Profile Options
        /// </summary>
        /// <param name="listXml"></param>
        public void gdprCompliancesave(int UserID, int ClientId)
        {
            SqlConnection cn = new SqlConnection();
            UserDataServices objDataServer = new UserDataServices();
            string constr = objDataServer.GetConnectionString(null, null, ClientId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[gdpr_acceptance_update]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UserID);
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

    }
}
