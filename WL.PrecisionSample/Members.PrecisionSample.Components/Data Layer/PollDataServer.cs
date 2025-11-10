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
   public class PollDataServer
    {

        #region GetCurrentPoll
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<Polls> GetCurrentPoll(int userid)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[poll].[CurrentPoll_Get]", cn);
            cm.Parameters.AddWithValue("userid", userid);
            
            using (IDataReader reader = cm.ExecuteReader())
            {
                List<Polls> lstpolls = new List<Polls>();
                while (reader.Read())
                {
                    Polls oPolls = new Polls();

                    if (reader["question_id"] != DBNull.Value)
                    {
                        oPolls.QuestionId = Convert.ToInt32(reader["question_id"]);
                    }

                    if (reader["question_text"] != DBNull.Value)
                    {
                        oPolls.QuestionText = reader["question_text"].ToString();
                    }
                    oPolls.Mode = 1;
                    lstpolls.Add(oPolls);
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        Polls oPolls = new Polls();

                        if (reader["option_id"] != DBNull.Value)
                        {
                            oPolls.OptionId = Convert.ToInt32(reader["option_id"]);
                        }
                        if (reader["option_text"] != DBNull.Value)
                        {
                            oPolls.OptionText = reader["option_text"].ToString();
                        }
                        oPolls.Mode = 2;
                        lstpolls.Add(oPolls);
                    }
                }
                return lstpolls;
            }
        }
        #endregion

        #region InsertResult
        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="optionId"></param>
        /// <param name="userid"></param>
        public void InsertResult(Int32 questionId, Int32 optionId, int userid)
        {
            SqlConnection conn = new SqlConnection();
            conn.Open();
            try
            {
                SqlCommand comm = new SqlCommand("[poll].[Result_Insert]", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@questionId", questionId);
                comm.Parameters.AddWithValue("@optionId", optionId);
                comm.Parameters.AddWithValue("@userid", userid);

              comm.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                conn.Close();

            }
        }
        #endregion

        #region GetResult
        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public List<Polls> GetResult(Int32 questionId)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand cm = new SqlCommand("[poll].[Result_Get]", cn);
            cm.Parameters.AddWithValue("question_id", questionId);

            using (IDataReader reader = cm.ExecuteReader())
            {
                List<Polls> lstpolls = new List<Polls>();
                while (reader.Read())
                {
                    Polls oPolls = new Polls();
                    if (reader["option_text"] != DBNull.Value)
                    {
                        oPolls.OptionText = Convert.ToString(reader["option_text"]);
                    }
                    if (reader["polls"] != DBNull.Value)
                    {
                        oPolls.PollCount = Convert.ToInt32(reader["polls"]);
                    }
                    if (reader["totpoll"] != DBNull.Value)
                    {
                        oPolls.TotPollCount = Convert.ToInt32(reader["totpoll"]);
                    }
                    if (reader["percentage"] != DBNull.Value)
                    {
                        oPolls.Percentage = Convert.ToString(reader["percentage"]) + "%";
                    }
                    oPolls.Mode = 1;
                    lstpolls.Add(oPolls);
                }
                return lstpolls;
            }
        }
        #endregion
    }
}
