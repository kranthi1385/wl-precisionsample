using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Members.PrecisionSample.Components.Data_Layer
{
    public class FacebookDataServer
    {
        #region Getting User Information for Home Page

        /// <summary>
        /// get home page details
        /// </summary>
        /// <param name="emailAddress">emailAddress</param>
        /// <returns></returns>
        public Home GetHomePageDetails(int userId)
        {
            Home objUser = new Home();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[HomePageInfo_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {

                    if (oreader["first_level_friends_count"] != DBNull.Value)
                    {
                        objUser.secondReferrals = Convert.ToInt32(oreader["first_level_friends_count"]);
                    }
                    if (oreader["second_level_friends_count"] != DBNull.Value)
                    {
                        objUser.thirdReferrals = Convert.ToInt32(oreader["second_level_friends_count"]);
                    }
                    if (oreader["reward_referrering"] != DBNull.Value)
                    {
                        objUser.rewardsEarned = Convert.ToDecimal(oreader["reward_referrering"]);
                    }
                    if (oreader["reward_total"] != DBNull.Value)
                    {
                        objUser.accountBalance = Convert.ToDecimal(oreader["reward_total"]);
                    }
                    if (oreader["surveys_pending"] != DBNull.Value)
                    {
                        objUser.totalSurveys = Convert.ToInt32(oreader["surveys_pending"]);

                    }
                    if (oreader["pending_surveys_reward"] != DBNull.Value)
                    {
                        objUser.totalRewardforSurvey = Convert.ToDecimal(oreader["pending_surveys_reward"]);

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
            return objUser;
        }
        #endregion

        #region top3 perks

        /// <summary>
        /// get top 3 surveys
        /// </summary>
        /// <returns></returns>
        public List<Perks> GetTop3Surveys(int userId)
        {
            List<Perks> lstPerks1 = new List<Perks>();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[HomePageInfo_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader oreader = cm.ExecuteReader();
                #region Adding params for the SQL Proc
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                #endregion

                while (oreader.Read())
                {
                    Perks oPerks = new Perks();
                    oPerks.PerkName = Convert.ToString((oreader["perk_name"]));
                    lstPerks1.Add(oPerks);
                }
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
            return lstPerks1;
        }



        #endregion

        #region getprofiles

        /// <summary>
        /// get user profiles
        /// </summary>
        /// <returns></returns>
        public List<Profile> GetProfiles(int userId)
        {
            List<Profile> objProfileList = new List<Profile>();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[HomePageInfo_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                SqlDataReader oreader = cm.ExecuteReader();

                while (oreader.Read())
                {
                    Profile oProfile = new Profile();
                    if (oreader["profile_name"] != DBNull.Value)
                    {
                        oProfile.ProfileName = oreader["profile_name"].ToString();
                    }
                    if (oreader["profile_id"] != DBNull.Value)
                    {
                        oProfile.ProfileId = Convert.ToInt32(oreader["profile_id"]);
                    }
                    if (oreader["count"] != DBNull.Value)
                    {
                        oProfile.Count = Convert.ToInt32(oreader["count"]);
                    }

                    objProfileList.Add(oProfile);
                }

            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
            return objProfileList;
        }
        #endregion

        #region  Surveys list
        /// <summary>
        /// get perks list 
        /// </summary>
        /// <param name="countryId">countryId</param>
        /// <returns></returns>
        public List<Perks> GetPerksList(int userId)
        {
            List<Perks> lstStates = new List<Perks>();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[HomePageInfo_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                #region Adding params for the SQL Proc
                cm.Parameters.AddWithValue("@user_id", userId);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                #endregion

                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    Perks OStates = new Perks();
                    if (oreader["survey_name"] != DBNull.Value)
                    {
                        OStates.PerkName = oreader["survey_name"].ToString();
                    }
                    if (oreader["survey_guid"] != DBNull.Value)
                    {
                        OStates.PerkGuid = new Guid(oreader["survey_guid"].ToString());
                    }
                    if (oreader["perk_url"] != DBNull.Value)
                    {
                        OStates.PerkUrl = oreader["perk_url"].ToString();
                    }
                    if (oreader["perk_description"] != DBNull.Value)
                    {
                        OStates.PerkDescription = oreader["perk_description"].ToString();
                    }
                    if (oreader["survey_url"] != DBNull.Value)
                    {
                        OStates.SurveyUrl = oreader["survey_url"].ToString();
                    }
                    if (oreader["survey_description"] != DBNull.Value)
                    {
                        OStates.SurveyDescription = oreader["survey_description"].ToString();
                    }
                    if (oreader["reward_value"] != DBNull.Value)
                    {
                        OStates.RewardValue = Convert.ToInt32(oreader["reward_value"]);
                    }
                    if (oreader["click_dt"] != DBNull.Value)
                    {
                        OStates.PerkClickDt = oreader["click_dt"].ToString();
                    }
                    if (oreader["offer_completed_dt"] != DBNull.Value)
                    {
                        OStates.PerkCompletedDt = oreader["offer_completed_dt"].ToString();
                    }
                    if (oreader["survey_completed_dt"] != DBNull.Value)
                    {
                        OStates.SurveyCompletedDt = oreader["survey_completed_dt"].ToString();
                    }
                    if (oreader["imagetype"] != DBNull.Value)
                    {
                        OStates.Type = oreader["imagetype"].ToString();
                    }
                    if (oreader["status"] != DBNull.Value)
                    {
                        OStates.Status = oreader["status"].ToString();
                    }
                    lstStates.Add(OStates);
                }
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
            return lstStates;
        }
        #endregion

        #region freebies list

        /// <summary>
        /// get freebies list of user
        /// </summary>
        /// <returns></returns>
        public List<Surveys> GetFreebiesList(int userId)
        {
            List<Surveys> lstStates = new List<Surveys>();
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[FreebiesList_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding params for the SQL Proc
                cm.Parameters.AddWithValue("@user_id", userId);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }

                #endregion

                SqlDataReader oreader = cm.ExecuteReader();
                while (oreader.Read())
                {
                    Surveys OStates = new Surveys();
                    if (oreader["perk_name"] != DBNull.Value)
                    {
                        OStates.SurveyName = oreader["perk_name"].ToString();
                    }
                    if (oreader["perk_guid"] != DBNull.Value)
                    {
                        OStates.PerkGuid = new Guid(oreader["perk_guid"].ToString());
                    }
                    if (oreader["logo"] != DBNull.Value)
                    {
                        OStates.Logo = oreader["logo"].ToString();
                    }
                    if (oreader["perk_description"] != DBNull.Value)
                    {
                        OStates.PerkDescription = oreader["perk_description"].ToString();
                    }
                    lstStates.Add(OStates);
                }
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
            return lstStates;
        }


        #endregion


        #region send FBinvitation to user

        /// <summary>
        /// get facebook app user check
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public void Facebokappusercheck(Guid userGuid, string facebookId, string accesstoken)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[dbo].[FaceBookUser_Check]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding params for the SQL Proc
                cm.Parameters.AddWithValue("@user_guid", userGuid);
                cm.Parameters.AddWithValue("@facebook_id", facebookId);
                cm.Parameters.AddWithValue("@fb_access_token", accesstoken);
                cm.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion



        #region IserrtRewardRedeemprtions

        /// <summary>
        /// get Insert Reward Redeemprtions
        /// </summary>
        /// <param name="amount">amount</param>
        public void IserrtRewardRedeemprtions(int userId, int amount)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[Reward_Redeem]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                #region Adding params for the SQL Proc (IF NOT NULL)

                cm.Parameters.AddWithValue("user_id", userId);
                cm.Parameters.AddWithValue("redeem_amount", amount);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                }
                #endregion
                cm.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion
    }
    #endregion
}