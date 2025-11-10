using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class LeadDataServer
    {
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
        public List<PSquestion> GetQuestions(Guid LeadGuid, string Xml, string QueryName, int QId, int SortOrder, string Type)
        {
            List<PSquestion> lstQuestion = new List<PSquestion>();
            SqlConnection cn = new SqlConnection("Data Source=10.200.10.224;Initial Catalog=ps2_user3;uid=mg_webapp;pwd=AybG8YvEm5WN;");
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(QueryName, cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@lead_guid", LeadGuid);
                if (Type == "save")
                {
                    cm.Parameters.AddWithValue("@xml", Xml);
                    cm.Parameters.AddWithValue("@current_question_id", QId);
                    cm.Parameters.AddWithValue("@current_question_sort_order", SortOrder);
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
                        if (dr["sort_order"] != DBNull.Value)
                        {
                            oQuestion.CurrentSortOrder = Convert.ToInt32(dr["sort_order"]);
                        }
                        if (dr["redirect_url"] != DBNull.Value)
                        {
                            oQuestion.RedirectUrl = Convert.ToString(dr["redirect_url"]);
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
        public List<PSquestion> GetPSQuestions(Guid LeadGuid)
        {
            List<PSquestion> lstQuestions = new List<PSquestion>();
            return lstQuestions = GetQuestions(LeadGuid, "", "[user].[lead_pending_profile_question_get]", 0, 0, "getqst");
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
        public List<PSquestion> SaveUserPrescreenerOptions(string Xml, Guid LeadGuid, int QuestionId, int SortOrder)
        {
            List<PSquestion> lstQuestions = new List<PSquestion>();
            return lstQuestions = GetQuestions(LeadGuid, Xml, "[user].[lead_responses_save]", QuestionId, SortOrder, "save");
        }
        #endregion

        #region Get Step2 Details
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="LeadGuid">UserGuid</param>
        /// <returns></returns>
        public User GetStep2Details(Guid LeadGuid)
        {
            User ouser = new User();
            SqlConnection cn = new SqlConnection("Data Source=10.200.10.224;Initial Catalog=ps2_user3;uid=mg_webapp;pwd=AybG8YvEm5WN;");
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[lead_user_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("lead_guid", LeadGuid);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["email_address"] != DBNull.Value)
                        {
                            ouser.EmailAddress = Convert.ToString(dr["email_address"]);
                        }
                        if (dr["first_name"] != DBNull.Value)
                        {
                            ouser.FirstName = Convert.ToString(dr["first_name"]);
                        }
                        if (dr["last_name"] != DBNull.Value)
                        {
                            ouser.LastName = Convert.ToString(dr["last_name"]);
                        }
                        if (dr["make"] != DBNull.Value)
                        {
                            ouser.Make = Convert.ToString(dr["make"]);
                        }
                        if (dr["address1"] != DBNull.Value)
                        {
                            ouser.Address1 = Convert.ToString(dr["address1"]);
                        }
                        if (dr["zip_code"] != DBNull.Value)
                        {
                            ouser.ZipCode = Convert.ToString(dr["zip_code"]);
                        }
                        if (dr["state"] != DBNull.Value)
                        {
                            ouser.StateId = Convert.ToInt32(dr["state"]);
                        }
                        if (dr["coutry"] != DBNull.Value)
                        {
                            ouser.CountryId = Convert.ToInt32(dr["coutry"]);
                        }
                        if (dr["gender"] != DBNull.Value)
                        {
                            ouser.Gender = Convert.ToString(dr["gender"]);
                        }
                        if (dr["start_text"] != DBNull.Value)
                        {
                            ouser.StarPageText = Convert.ToString(dr["start_text"]);
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
            return ouser;
        }
        #endregion
    }
}
