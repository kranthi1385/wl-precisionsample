using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using NLog;
namespace Members.PrecisionSample.Components.Data_Layer
{
    public class OpinionPartnerDataServer
    {
        #region Connection
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }

        #endregion

        #region User Detais Check 
        /// <summary>
        /// objUserDeialscCheck
        /// </summary>
        /// <param name="Rid"></param>
        /// <param name="ExtId"></param>
        /// <returns></returns>
        public MemberEntity objUserDeialscCheck(int Rid, string ExtId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            MemberEntity objMember = new MemberEntity();
            try
            {
                
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[user_exist_checkbyridandexternalmemberid]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(Rid));
                if (ExtId != "")
                {
                    cm.Parameters.AddWithValue("@ext_id", ExtId);
                }
                else
                {
                    cm.Parameters.AddWithValue("@ext_id", ExtId);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objMember.UserGuid = new Guid(dr["user_guid"].ToString());
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objMember.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["totalcount"] != DBNull.Value)
                        {
                            objMember.Count = Convert.ToInt32(dr["totalcount"]);
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
            return objMember;
        }
        #endregion

        #region UserDeials Check By EmailAddress
        /// <summary>
        /// objUserDeialscCheckByEmailAddress
        /// </summary>
        /// <param name="Rid"></param>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>
        public MemberEntity objUserDeialscCheckByEmailAddress(int Rid, string EmailAddress)
        {
            MemberEntity objMember = new MemberEntity();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
               
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[user_exist_checkbyridandemailaddress]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(Rid));
                if (EmailAddress != "")
                {
                    cm.Parameters.AddWithValue("@email_address", EmailAddress);
                }
                else
                {
                    cm.Parameters.AddWithValue("@email_address", DBNull.Value);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objMember.UserGuid = new Guid(dr["user_guid"].ToString());
                        }
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objMember.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["totalcount"] != DBNull.Value)
                        {
                            objMember.Count = Convert.ToInt32(dr["totalcount"]);
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
            return objMember;

        }
        #endregion

        #region Save
        /// <summary>
        /// save
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public MemberEntity save(MemberEntity objUser)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[UserDetails_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(objUser.Rid));
                cm.Parameters.AddWithValue("@sub_id", objUser.SubId3);
                cm.Parameters.AddWithValue("@extmid", objUser.ExtId);
                cm.Parameters.AddWithValue("@firstname", objUser.FirstName);
                cm.Parameters.AddWithValue("@lastname", objUser.LastName);
                cm.Parameters.AddWithValue("@email_address", objUser.EmailAddress);
                cm.Parameters.AddWithValue("@address1", objUser.Address1);
                cm.Parameters.AddWithValue("@address2", objUser.Address2);
                cm.Parameters.AddWithValue("@country_id", objUser.CountryCode);
                cm.Parameters.AddWithValue("@state_id", objUser.StateCode);
                cm.Parameters.AddWithValue("@city", objUser.City);
                cm.Parameters.AddWithValue("@zip_code", objUser.ZipCode);
                cm.Parameters.AddWithValue("@phone_number", objUser.PhoneNumber);
                cm.Parameters.AddWithValue("@gender", objUser.Gender);
                cm.Parameters.AddWithValue("@dob", objUser.Dob);
                cm.Parameters.AddWithValue("@ethnicity_id", objUser.EthnicityId);
                cm.Parameters.AddWithValue("@verity_id", objUser.VerityId);
                cm.Parameters.AddWithValue("@verity_score", objUser.VerityScore);
                cm.Parameters.AddWithValue("@geocorrelationFlag", objUser.GeoCorrelationFlag);
                cm.Parameters.AddWithValue("@create_by", objUser.EmailAddress);
                cm.Parameters.AddWithValue("@update_by", objUser.EmailAddress);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objUser.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(dr["user_guid"].ToString());
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
            return objUser;
        }
        #endregion

        #region for Save Member Details
        /// <summary>
        /// SaveMemberDetails
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>

        public MemberEntity SaveMemberDetails(MemberEntity objUser)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[UserDetails_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(objUser.Rid));
                cm.Parameters.AddWithValue("@extmid", objUser.ExtId);
                cm.Parameters.AddWithValue("@firstname", objUser.FirstName);
                cm.Parameters.AddWithValue("@lastname", objUser.LastName);
                cm.Parameters.AddWithValue("@email_address", objUser.EmailAddress);
                cm.Parameters.AddWithValue("@address1", objUser.Address1);
                cm.Parameters.AddWithValue("@address2", objUser.Address2);
                cm.Parameters.AddWithValue("@country_id", objUser.CountryCode);
                cm.Parameters.AddWithValue("@state_id", objUser.StateCode);
                cm.Parameters.AddWithValue("@city", objUser.City);
                cm.Parameters.AddWithValue("@zip_code", objUser.ZipCode);
                cm.Parameters.AddWithValue("@phone_number", objUser.PhoneNumber);
                cm.Parameters.AddWithValue("@gender", objUser.Gender);
                cm.Parameters.AddWithValue("@dob", objUser.Dob);
                cm.Parameters.AddWithValue("@ethnicity_id", objUser.EthnicityId);
                cm.Parameters.AddWithValue("@verity_id", objUser.VerityId);
                cm.Parameters.AddWithValue("@verity_score", objUser.VerityScore);
                cm.Parameters.AddWithValue("@geocorrelationFlag", objUser.GeoCorrelationFlag);
                cm.Parameters.AddWithValue("@create_by", objUser.EmailAddress);
                cm.Parameters.AddWithValue("@update_by", objUser.EmailAddress);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_id"] != DBNull.Value)
                        {
                            objUser.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            objUser.UserGuid = new Guid(dr["user_guid"].ToString());
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
            return objUser;
        }
        #endregion

        //#region GetMemberDetails List

        //public List<PartnerHistory> GetUserDetailsList(Guid UserGuid, int OrgId)
        //{
        //    int TotalAmount = 0;
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    try
        //    {
        //        cn.Open();

        //        SqlCommand cm = new SqlCommand("[partner].[UserProfilesAndSurveysAndRewardsHistory_Get]", cn);
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.Parameters.AddWithValue("@user_guid", UserGuid);
        //        cm.Parameters.AddWithValue("@org_id", OrgId);
        //        using (IDataReader dr = cm.ExecuteReader())
        //        {

        //            List<PartnerHistory> lstUserDetals = new List<PartnerHistory>();
        //            PartnerHistory objPartnerHIstroy = new PartnerHistory();

        //            while (dr.Read())
        //            {
        //                Profile objProfile = new Profile();
        //                if (dr["ProfileId"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileId = Convert.ToInt32(dr["ProfileId"]);
        //                }
        //                if (dr["ProfileName"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileName = Convert.ToString(dr["ProfileName"]);
        //                }
        //                if (dr["ProfileUrl"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileUrl = Convert.ToString(dr["ProfileUrl"]);
        //                }
        //                if (dr["ProfileStatus"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileStatus = Convert.ToString(dr["ProfileStatus"]);
        //                }
        //                objPartnerHIstroy.LstProfileHistory.Add(objProfile);

        //            }
        //            dr.NextResult();
        //            while (dr.Read())
        //            {
        //                Surveys objSurveys = new Surveys();
        //                if (dr["SurveyName"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyName = Convert.ToString(dr["SurveyName"]);
        //                }
        //                if (dr["SurveyUrl"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyUrl = Convert.ToString(dr["SurveyUrl"]);
        //                }
        //                if (dr["SurveyLength"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyLength = Convert.ToInt32(dr["SurveyLength"]);
        //                }
        //                if (dr["RewardValue"] != DBNull.Value)
        //                {
        //                    objSurveys.Survey_complete_reward_amount = Convert.ToDecimal(dr["RewardValue"]);
        //                }
        //                //else
        //                //{
        //                //    objSurveys.Survey_complete_reward_amount = 0;
        //                //}
        //                objPartnerHIstroy.LstSurveys.Add(objSurveys);
        //            }

        //            dr.NextResult();
        //            while (dr.Read())
        //            {
        //                RewardsHistory objRewardHistroy = new RewardsHistory();
        //                if (dr["account_balance"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalAccountBalance = dr["account_balance"].ToString();
        //                    TotalAmount = Convert.ToInt32(objRewardHistroy.TotalAccountBalance);
        //                }
        //                if (dr["total_earnings"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalEarnings = dr["total_earnings"].ToString();
        //                }
        //                if (dr["total_redemptions"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalRedemptions = dr["total_redemptions"].ToString();
        //                }
        //                objPartnerHIstroy.LstTotalRewards.Add(objRewardHistroy);

        //            }

        //            dr.NextResult();

        //            while (dr.Read())
        //            {
        //                Rewards oRewards = new Rewards();

        //                // oRewards.ProjectName = dr["project_name"].ToString();

        //                if (dr["reward_dt"] != DBNull.Value)
        //                {
        //                    oRewards.RewardDt = Convert.ToDateTime(dr["reward_dt"]);
        //                    oRewards.CreatedDt = oRewards.RewardDt.ToString("MM/dd/yyyy");

        //                }
        //                if (dr["description"] != DBNull.Value)
        //                {
        //                    oRewards.Descripion = Convert.ToString(dr["description"]);
        //                }

        //                if (dr["direct_reward"] != DBNull.Value)
        //                {
        //                    oRewards.RewardAmount = Convert.ToDecimal(dr["direct_reward"]);
        //                }
        //                if (dr["level_1"] != DBNull.Value)
        //                {
        //                    oRewards.Level1Reward = Convert.ToDecimal(dr["level_1"]);
        //                }
        //                if (dr["level_2"] != DBNull.Value)
        //                {
        //                    oRewards.Level2Reward = Convert.ToDecimal(dr["level_2"]);
        //                }
        //                if (dr["redemption_amount"] != DBNull.Value)
        //                {
        //                    oRewards.RedemptionAmount = Convert.ToDecimal(dr["redemption_amount"]);
        //                }
        //                if (dr["balance_amount"] != DBNull.Value)
        //                {
        //                    oRewards.BalanceAmount = Convert.ToDecimal(dr["balance_amount"]);
        //                }
        //                if (dr["reward_expiry_dt"] != DBNull.Value)
        //                {
        //                    oRewards.RewardExpiredDt = Convert.ToDateTime(dr["reward_expiry_dt"]);
        //                    oRewards.EndDt = oRewards.RewardExpiredDt.ToString("MM/dd/yyyy");
        //                }
        //                if (dr["status"] != DBNull.Value)
        //                {
        //                    oRewards.Status = Convert.ToString(dr["status"]);
        //                }
        //                objPartnerHIstroy.LstRewardHistory.Add(oRewards);
        //            }

        //            dr.NextResult();
        //            while (dr.Read())
        //            {
        //                Rewards oRewardOption = new Rewards();
        //                if (dr["catalouge_name"] != null)
        //                {
        //                    oRewardOption.RewardName = Convert.ToString(dr["catalouge_name"]);
        //                }
        //                if (dr["catalouge_description"] != null)
        //                {
        //                    oRewardOption.RewardDescription = Convert.ToString(dr["catalouge_description"]);
        //                }
        //                if (dr["minimum_reward_amount"] != null)
        //                {
        //                    oRewardOption.MinRedemptionAmount = Convert.ToInt32(dr["minimum_reward_amount"]);
        //                }
        //                if (dr["catalouge_id"] != null)
        //                {
        //                    oRewardOption.CatalogueId = Convert.ToInt32(dr["catalouge_id"]);
        //                }
        //                if (dr["catalouge_logo"] != null)
        //                {
        //                    oRewardOption.RewardLogo = Convert.ToString(dr["catalouge_logo"]);
        //                }
        //                if (dr["catalouge_guid"] != null)
        //                {
        //                    oRewardOption.CatalogueGuid = new Guid(dr["catalouge_guid"].ToString());
        //                }
        //                if (TotalAmount != 0)
        //                {
        //                    oRewardOption.TotalRewardAmout = TotalAmount;
        //                }
        //                objPartnerHIstroy.LstRewardCatalog.Add(oRewardOption);
        //            }

        //            lstUserDetals.Add(objPartnerHIstroy);
        //            return lstUserDetals;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }

        //}
        //#endregion

        #region For all memberlist show in single page

        #region GetSurveyList
        /// <summary>
        /// GetSurveyList
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public List<PartnerHistory> GetSurveyList(Guid UserGuid, int OrgId, string ConnectionstringName)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionstringName].ToString();
            List<PartnerHistory> lstUserSurveyList = new List<PartnerHistory>();
            PartnerHistory objSurveyList = new PartnerHistory();
            try
            {
                cn.Open();

                SqlCommand cm = new SqlCommand("[partner].[UserSurveys_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@org_id", OrgId);

                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Surveys objSurveys = new Surveys();
                        if (dr["SurveyName"] != DBNull.Value)
                        {
                            objSurveys.SurveyName = Convert.ToString(dr["SurveyName"]);
                        }
                        if (dr["SurveyUrl"] != DBNull.Value)
                        {
                            objSurveys.SurveyUrl = Convert.ToString(dr["SurveyUrl"]);
                        }
                        if (dr["SurveyLength"] != DBNull.Value)
                        {
                            objSurveys.SurveyLength = Convert.ToInt32(dr["SurveyLength"]);
                        }
                        if (dr["RewardValue"] != DBNull.Value)
                        {
                            objSurveys.SurveyCompletereward = dr["RewardValue"].ToString();
                        }
                        if (dr["RewardPoints"] != DBNull.Value)
                        {
                            objSurveys.RewardPoints = Convert.ToInt32(dr["RewardPoints"]);
                        }
                        if (dr["org_id"] != DBNull.Value)
                        {
                            objSurveys.OrgId = Convert.ToInt32(dr["org_id"]);
                        }
                        if (dr["rewards_in_dollar"] != DBNull.Value)
                        {
                            objSurveys.IsRewardsInDollar = Convert.ToInt32(dr["rewards_in_dollar"]);

                        }
                        if (dr["reward_text"] != DBNull.Value)
                        {
                            objSurveys.RewardText = Convert.ToString(dr["reward_text"]);
                        }
                        objSurveyList.LstSurveys.Add(objSurveys);
                    }
                    lstUserSurveyList.Add(objSurveyList);
                    Entities.NLog.ClassLogger.Warn(UserGuid + "|" + ConnectionstringName + "|GetSurveysResponse|");



                }
            }
            catch (Exception ex)
            {
                Entities.NLog.ClassLogger.Fatal(UserGuid + "|" + ConnectionstringName + "|" + ex + "|GetSurveysResponnse|");
            }
            finally
            {
                cn.Close();
            }
            return lstUserSurveyList;
        }
        #endregion

        #region GetProfileLIst
        /// <summary>
        /// GetProfileList
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public List<PartnerHistory> GetProfileList(Guid UserGuid, int OrgId, decimal partnerRevShare)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            List<PartnerHistory> lstUserProfilrList = new List<PartnerHistory>();
            PartnerHistory objProfileList = new PartnerHistory();

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[UserProfiles_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@org_id", OrgId);

                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Profile objProfile = new Profile();
                        if (dr["ProfileId"] != DBNull.Value)
                        {
                            objProfile.ProfileId = Convert.ToInt32(dr["ProfileId"]);
                        }
                        if (dr["ProfileName"] != DBNull.Value)
                        {
                            objProfile.ProfileName = Convert.ToString(dr["ProfileName"]);
                        }
                        if (dr["ProfileUrl"] != DBNull.Value)
                        {
                            objProfile.ProfileUrl = Convert.ToString(dr["ProfileUrl"]);
                        }
                        if (dr["ProfileStatus"] != DBNull.Value)
                        {
                            objProfile.ProfileStatus = Convert.ToString(dr["ProfileStatus"]);
                        }
                        objProfileList.LstProfileHistory.Add(objProfile);

                    }
                    objProfileList.PartnerRevShare = partnerRevShare; //we are setting the partner rev share based on ifram partner 70-30 or 50-50 rev share.
                    lstUserProfilrList.Add(objProfileList);
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
            return lstUserProfilrList;
        }
        #endregion

        #region RewardList

        /// <summary>
        /// GetRewardList
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public List<PartnerHistory> GetRewardList(Guid UserGuid, int OrgId)
        {
            string TotalAmount = string.Empty;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            List<PartnerHistory> lstUserDetals = new List<PartnerHistory>();

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[UserRewardsHistoryAndCatalogues_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (IDataReader dr = cm.ExecuteReader())
                {

                    PartnerHistory objPartnerHIstroy = new PartnerHistory();

                    while (dr.Read())
                    {
                        RewardsHistory objRewardHistroy = new RewardsHistory();
                        if (dr["account_balance"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalAccountBalance = dr["account_balance"].ToString();
                            TotalAmount = objRewardHistroy.TotalAccountBalance;
                        }
                        if (dr["total_earnings"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalEarnings = dr["total_earnings"].ToString();
                        }
                        if (dr["total_redemptions"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalRedemptions = dr["total_redemptions"].ToString();
                        }
                        if (dr["pending_amount"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalPendignBalance = dr["pending_amount"].ToString();
                        }
                        objPartnerHIstroy.LstTotalRewards.Add(objRewardHistroy);

                    }

                    //dr.NextResult();

                    //while (dr.Read())
                    //{
                    //    Rewards oRewards = new Rewards();

                    //    // oRewards.ProjectName = dr["project_name"].ToString();

                    //    if (dr["reward_dt"] != DBNull.Value)
                    //    {
                    //        oRewards.RewardDt = Convert.ToDateTime(dr["reward_dt"]);
                    //        oRewards.CreatedDt = oRewards.RewardDt.ToString("MM/dd/yyyy");

                    //    }
                    //    if (dr["description"] != DBNull.Value)
                    //    {
                    //        oRewards.Descripion = Convert.ToString(dr["description"]);
                    //    }

                    //    if (dr["direct_reward"] != DBNull.Value)
                    //    {
                    //        oRewards.RewardAmount = Convert.ToDecimal(dr["direct_reward"]);
                    //    }
                    //    if (dr["level_1"] != DBNull.Value)
                    //    {
                    //        oRewards.Level1Reward = Convert.ToDecimal(dr["level_1"]);
                    //    }
                    //    if (dr["level_2"] != DBNull.Value)
                    //    {
                    //        oRewards.Level2Reward = Convert.ToDecimal(dr["level_2"]);
                    //    }
                    //    if (dr["redemption_amount"] != DBNull.Value)
                    //    {
                    //        oRewards.RedemptionAmount = Convert.ToDecimal(dr["redemption_amount"]);
                    //    }
                    //    if (dr["balance_amount"] != DBNull.Value)
                    //    {
                    //        oRewards.BalanceAmount = Convert.ToDecimal(dr["balance_amount"]);
                    //    }
                    //    if (dr["reward_expiry_dt"] != DBNull.Value)
                    //    {
                    //        oRewards.RewardExpiredDt = Convert.ToDateTime(dr["reward_expiry_dt"]);
                    //        oRewards.EndDt = oRewards.RewardExpiredDt.ToString("MM/dd/yyyy");
                    //    }
                    //    if (dr["status"] != DBNull.Value)
                    //    {
                    //        oRewards.Status = Convert.ToString(dr["status"]);
                    //    }
                    //    objPartnerHIstroy.LstRewardHistory.Add(oRewards);
                    //}

                    dr.NextResult();
                    while (dr.Read())
                    {
                        Rewards oRewardOption = new Rewards();
                        if (dr["catalouge_name"] != null)
                        {
                            oRewardOption.RewardName = Convert.ToString(dr["catalouge_name"]);
                        }
                        if (dr["catalouge_description"] != null)
                        {
                            oRewardOption.RewardDescription = Convert.ToString(dr["catalouge_description"]);
                        }
                        if (dr["minimum_reward_amount"] != null)
                        {
                            oRewardOption.MinRedemptionAmount = Convert.ToInt32(dr["minimum_reward_amount"]);
                        }
                        if (dr["catalouge_id"] != null)
                        {
                            oRewardOption.CatalogueId = Convert.ToInt32(dr["catalouge_id"]);
                        }
                        if (dr["catalouge_logo"] != null)
                        {
                            oRewardOption.RewardLogo = Convert.ToString(dr["catalouge_logo"]);
                        }
                        if (dr["catalouge_guid"] != null)
                        {
                            oRewardOption.CatalogueGuid = new Guid(dr["catalouge_guid"].ToString());
                        }
                        if (TotalAmount != "0")
                        {
                            oRewardOption.TotalRewardAmout = Convert.ToDecimal(TotalAmount);
                        }
                        objPartnerHIstroy.LstRewardCatalog.Add(oRewardOption);
                    }

                    lstUserDetals.Add(objPartnerHIstroy);
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
            return lstUserDetals;
        }
        #endregion

        #region Get member survey history and Get member redeemption histroy
        /// <summary>
        /// GetSurveyAndRedeemptionHistory
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="OrgId"></param>
        /// <returns></returns>

        public List<PartnerHistory> GetSurveyAndRedeemptionHistory(Guid UserGuid, int OrgId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            List<PartnerHistory> lstUserDetals = new List<PartnerHistory>();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[iframe].[ReweradsHistory_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                using (IDataReader dr = cm.ExecuteReader())
                {
                  
                    PartnerHistory objPartnerHIstroy = new PartnerHistory();
                    while (dr.Read())
                    {
                        SurveyRedeemptionHistroy objRewardHistory = new SurveyRedeemptionHistroy();
                        if (dr["reward_payment_dt"] != DBNull.Value)
                        {
                            objRewardHistory.CreateDt = Convert.ToString(dr["reward_payment_dt"]);
                        }
                        if (dr["survey_name"] != DBNull.Value)
                        {
                            objRewardHistory.Descripion = Convert.ToString(dr["survey_name"]);
                        }
                        if (dr["reward_amount"] != DBNull.Value)
                        {
                            objRewardHistory.RewardAmount = dr["reward_amount"].ToString();
                        }

                        if (dr["status"] != DBNull.Value)
                        {
                            objRewardHistory.Status = Convert.ToString(dr["status"]);
                        }
                        if (dr["project_id"] != DBNull.Value)
                        {
                            objRewardHistory.ProjectId = Convert.ToInt32(dr["project_id"]);
                        }
                        objPartnerHIstroy.LstSurveyHistory.Add(objRewardHistory);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        SurveyRedeemptionHistroy objRedeemHistory = new SurveyRedeemptionHistroy();

                        if (dr["redemption_dt"] != DBNull.Value)
                        {
                            objRedeemHistory.CreateDt = Convert.ToString(dr["redemption_dt"]);
                        }

                        if (dr["redemption_amount"] != DBNull.Value)
                        {
                            objRedeemHistory.RedeemedAmount = dr["redemption_amount"].ToString();
                        }
                        if (dr["catalouge_name"] != DBNull.Value)
                        {
                            objRedeemHistory.RewardName = Convert.ToString(dr["catalouge_name"]);
                        }
                        if (dr["code_name"] != DBNull.Value)
                        {
                            objRedeemHistory.CodeName = Convert.ToString(dr["code_name"]);
                        }

                        objPartnerHIstroy.LstRedeemptionHistory.Add(objRedeemHistory);

                    }
                    lstUserDetals.Add(objPartnerHIstroy);
                  

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
              return lstUserDetals;
        }
        #endregion

        #endregion

        //#region GetSurveyList
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="UserGuid"></param>
        ///// <param name="OrgId"></param>
        ///// <returns></returns>
        //public List<Surveys> GetUserSurveyList(Guid UserGuid, int OrgId)
        //{
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    List<Surveys> lstUserSurveyList = new List<Surveys>();
        //    try
        //    {
        //        cn.Open();

        //        SqlCommand cm = new SqlCommand("[partner].[UserSurveys_Get]", cn);
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.Parameters.AddWithValue("@user_guid", UserGuid);
        //        cm.Parameters.AddWithValue("@org_id", OrgId);

        //        using (IDataReader dr = cm.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                Surveys objSurveys = new Surveys();
        //                if (dr["SurveyName"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyName = Convert.ToString(dr["SurveyName"]);
        //                }
        //                if (dr["SurveyUrl"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyUrl = Convert.ToString(dr["SurveyUrl"]);
        //                }
        //                if (dr["SurveyLength"] != DBNull.Value)
        //                {
        //                    objSurveys.SurveyLength = Convert.ToInt32(dr["SurveyLength"]);
        //                }
        //                if (dr["RewardValue"] != DBNull.Value)
        //                {
        //                    objSurveys.Survey_complete_reward_amount = Convert.ToDecimal(dr["RewardValue"]);
        //                }
        //                lstUserSurveyList.Add(objSurveys);
        //            }

        //            return lstUserSurveyList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }

        //}
        //#endregion

        //#region GetProfileLIst

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="UserGuid"></param>
        ///// <param name="OrgId"></param>
        ///// <returns></returns>
        //public List<Profile> GetUserProfileList(Guid UserGuid, int OrgId)
        //{
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    try
        //    {
        //        cn.Open();
        //        List<Profile> lstUserProfileList = new List<Profile>();
        //        SqlCommand cm = new SqlCommand("[partner].[UserProfiles_Get]", cn);
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.Parameters.AddWithValue("@user_guid", UserGuid);
        //        cm.Parameters.AddWithValue("@org_id", OrgId);

        //        using (IDataReader dr = cm.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                Profile objProfile = new Profile();
        //                if (dr["ProfileId"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileId = Convert.ToInt32(dr["ProfileId"]);
        //                }
        //                if (dr["ProfileName"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileName = Convert.ToString(dr["ProfileName"]);
        //                }
        //                if (dr["ProfileUrl"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileUrl = Convert.ToString(dr["ProfileUrl"]);
        //                }
        //                if (dr["ProfileStatus"] != DBNull.Value)
        //                {
        //                    objProfile.ProfileStatus = Convert.ToString(dr["ProfileStatus"]);
        //                }
        //                lstUserProfileList.Add(objProfile);

        //            }
        //            return lstUserProfileList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //}
        //#endregion

        //#region RewardList

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="UserGuid"></param>
        ///// <param name="OrgId"></param>
        ///// <returns></returns>
        //public List<PartnerHistory> GetUserRewardList(Guid UserGuid, int OrgId)
        //{
        //    int TotalAmount = 0;
        //    SqlConnection cn = new SqlConnection();
        //    cn.ConnectionString = ConnectionString;
        //    try
        //    {
        //        cn.Open();

        //        SqlCommand cm = new SqlCommand("[partner].[UserRewardsHistoryAndCatalogues_Get]", cn);
        //        cm.CommandType = CommandType.StoredProcedure;
        //        cm.Parameters.AddWithValue("@user_guid", UserGuid);
        //        cm.Parameters.AddWithValue("@org_id", OrgId);
        //        using (IDataReader dr = cm.ExecuteReader())
        //        {

        //            List<PartnerHistory> lstUserDetals = new List<PartnerHistory>();
        //            PartnerHistory objPartnerHIstroy = new PartnerHistory();

        //            while (dr.Read())
        //            {
        //                RewardsHistory objRewardHistroy = new RewardsHistory();
        //                if (dr["account_balance"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalAccountBalance = Convert.ToDecimal(dr["account_balance"]);
        //                    TotalAmount = Convert.ToInt32(objRewardHistroy.TotalAccountBalance);
        //                }
        //                if (dr["total_earnings"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalEarnings = Convert.ToDecimal(dr["total_earnings"]);
        //                }
        //                if (dr["total_redemptions"] != DBNull.Value)
        //                {
        //                    objRewardHistroy.TotalRedemptions = Convert.ToDecimal(dr["total_redemptions"]);
        //                }
        //                objPartnerHIstroy.LstTotalRewards.Add(objRewardHistroy);

        //            }

        //            dr.NextResult();

        //            while (dr.Read())
        //            {
        //                Rewards oRewards = new Rewards();

        //                // oRewards.ProjectName = dr["project_name"].ToString();

        //                if (dr["reward_dt"] != DBNull.Value)
        //                {
        //                    oRewards.RewardDt = Convert.ToDateTime(dr["reward_dt"]);
        //                    oRewards.CreatedDt = oRewards.RewardDt.ToString("MM/dd/yyyy");

        //                }
        //                if (dr["description"] != DBNull.Value)
        //                {
        //                    oRewards.Descripion = Convert.ToString(dr["description"]);
        //                }

        //                if (dr["direct_reward"] != DBNull.Value)
        //                {
        //                    oRewards.RewardAmount = Convert.ToDecimal(dr["direct_reward"]);
        //                }
        //                if (dr["level_1"] != DBNull.Value)
        //                {
        //                    oRewards.Level1Reward = Convert.ToDecimal(dr["level_1"]);
        //                }
        //                if (dr["level_2"] != DBNull.Value)
        //                {
        //                    oRewards.Level2Reward = Convert.ToDecimal(dr["level_2"]);
        //                }
        //                if (dr["redemption_amount"] != DBNull.Value)
        //                {
        //                    oRewards.RedemptionAmount = Convert.ToDecimal(dr["redemption_amount"]);
        //                }
        //                if (dr["balance_amount"] != DBNull.Value)
        //                {
        //                    oRewards.BalanceAmount = Convert.ToDecimal(dr["balance_amount"]);
        //                }
        //                if (dr["reward_expiry_dt"] != DBNull.Value)
        //                {
        //                    oRewards.RewardExpiredDt = Convert.ToDateTime(dr["reward_expiry_dt"]);
        //                    oRewards.EndDt = oRewards.RewardExpiredDt.ToString("MM/dd/yyyy");
        //                }
        //                if (dr["status"] != DBNull.Value)
        //                {
        //                    oRewards.Status = Convert.ToString(dr["status"]);
        //                }
        //                objPartnerHIstroy.LstRewardHistory.Add(oRewards);
        //            }

        //            dr.NextResult();
        //            while (dr.Read())
        //            {
        //                Rewards oRewardOption = new Rewards();
        //                if (dr["catalouge_name"] != null)
        //                {
        //                    oRewardOption.RewardName = Convert.ToString(dr["catalouge_name"]);
        //                }
        //                if (dr["catalouge_description"] != null)
        //                {
        //                    oRewardOption.RewardDescription = Convert.ToString(dr["catalouge_description"]);
        //                }
        //                if (dr["minimum_reward_amount"] != null)
        //                {
        //                    oRewardOption.MinRedemptionAmount = Convert.ToInt32(dr["minimum_reward_amount"]);
        //                }
        //                if (dr["catalouge_id"] != null)
        //                {
        //                    oRewardOption.CatalogueId = Convert.ToInt32(dr["catalouge_id"]);
        //                }
        //                if (dr["catalouge_logo"] != null)
        //                {
        //                    oRewardOption.RewardLogo = Convert.ToString(dr["catalouge_logo"]);
        //                }
        //                if (dr["catalouge_guid"] != null)
        //                {
        //                    oRewardOption.CatalogueGuid = new Guid(dr["catalouge_guid"].ToString());
        //                }
        //                if (TotalAmount != 0)
        //                {
        //                    oRewardOption.TotalRewardAmout = TotalAmount;
        //                }
        //                objPartnerHIstroy.LstRewardCatalog.Add(oRewardOption);
        //            }

        //            lstUserDetals.Add(objPartnerHIstroy);
        //            return lstUserDetals;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }

        //}
        //#endregion

        #region GetMemberCatalouge and RewardHistory Details by Guid
        /// <summary>
        /// Get Cataloug And RewardData
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="CatalougeGuid"></param>
        /// <param name="OrgId"></param>
        /// <param name="EmailAddress"></param>
        /// <returns></returns>

        public List<PartnerHistory> GetCatalougAndRewardData(Guid UserGuid, Guid CatalougeGuid, int OrgId, string EmailAddress)
        {

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            List<PartnerHistory> lstUserDetals = new List<PartnerHistory>();
            try
            {
                cn.Open();

                SqlCommand cm = new SqlCommand("[partner].[Get_Cataloguesandreward_Details_By_Guid]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_guid", UserGuid);
                cm.Parameters.AddWithValue("@catalouge_guid", CatalougeGuid);
                cm.Parameters.AddWithValue("@org_id", OrgId);
                using (IDataReader dr = cm.ExecuteReader())
                {

                  
                    PartnerHistory objPartnerHIstroy = new PartnerHistory();
                    while (dr.Read())
                    {
                        Rewards oRewardOption = new Rewards();
                        if (dr["catalouge_name"] != null)
                        {
                            oRewardOption.RewardName = Convert.ToString(dr["catalouge_name"]);
                        }
                        if (dr["catalouge_description"] != null)
                        {
                            oRewardOption.RewardDescription = Convert.ToString(dr["catalouge_description"]);
                            if (EmailAddress != null || EmailAddress != string.Empty)
                            {
                                oRewardOption.RewardDescription = oRewardOption.RewardDescription.Replace("%%email_address%%", EmailAddress);
                            }
                            else
                            {
                                oRewardOption.RewardDescription.Replace("%%email_address%%", "");
                            }

                        }
                        if (dr["minimum_reward_amount"] != null)
                        {
                            oRewardOption.MinRedemptionAmount = Convert.ToInt32(dr["minimum_reward_amount"]);
                        }
                        if (dr["catalouge_id"] != null)
                        {
                            oRewardOption.CatalogueId = Convert.ToInt32(dr["catalouge_id"]);
                        }
                        if (dr["catalouge_logo"] != null)
                        {
                            oRewardOption.RewardLogo = Convert.ToString(dr["catalouge_logo"]);
                        }
                        objPartnerHIstroy.LstRewardCatalog.Add(oRewardOption);

                    }
                    dr.NextResult();

                    while (dr.Read())
                    {
                        RewardsHistory objRewardHistroy = new RewardsHistory();
                        if (dr["account_balance"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalAccountBalance = Convert.ToString(dr["account_balance"]);
                        }
                        if (dr["total_earnings"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalEarnings = Convert.ToString(dr["total_earnings"]);
                        }
                        if (dr["total_redemptions"] != DBNull.Value)
                        {
                            objRewardHistroy.TotalRedemptions = Convert.ToString(dr["total_redemptions"]);
                        }
                        objPartnerHIstroy.LstTotalRewards.Add(objRewardHistroy);

                    }

                    lstUserDetals.Add(objPartnerHIstroy);
                   
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
            return lstUserDetals;
        }
        #endregion

        #region IserrtRewardRedeemprtions
        /// <summary>
        /// Reward Redeemprtions
        /// </summary>
        /// <param name="amount"></param>
        public Rewards RewardRedeemprtions(int amount, Guid catalougGuid, int UserId)
        {
            Rewards objRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[iframe].[Reward_Redeem]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", Convert.ToInt32(UserId));
                cm.Parameters.AddWithValue("@redeem_amount", amount);
                cm.Parameters.AddWithValue("@catalouge_guid", catalougGuid);
                using (IDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["redemption_id"] != DBNull.Value)
                        {
                            objRewards.RedemptionId = Convert.ToInt32(reader["redemption_id"].ToString());
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
            return objRewards;
        }


        #endregion

        #region IserrtMemberRewardRedeemprtions
        /// <summary>
        /// Redeem Member Rewards
        /// </summary>
        /// <param name="amount"></param>
        public void RedeemMemberRewards(string Sku, decimal Ut, int UserId, int Points, int OrgId, string FirstName, string EmailAddress)
        {
            string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            Rewards objRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[user_tango_redemption_insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(OrgId));
                cm.Parameters.AddWithValue("@user_id", Convert.ToInt32(UserId));
                cm.Parameters.AddWithValue("@first_name", FirstName);
                cm.Parameters.AddWithValue("@email_address", EmailAddress);
                cm.Parameters.AddWithValue("@amount_redeemed", Convert.ToDecimal(Ut));
                cm.Parameters.AddWithValue("@amount_redeemed_in_points", Convert.ToInt32(Points));
                cm.Parameters.AddWithValue("@sku", Convert.ToString(Sku));
                cm.Parameters.AddWithValue("@ip_address", Convert.ToString(ip));
                cm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cn.Close();
            }
        }


        #endregion


        #region Get Country & States List

        /// <summary>
        /// GetCountrysAndStates
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public CountryAndState GetCountrysAndStates(string LanguageName)
        {
            CountryAndState oCountryAndState = new CountryAndState();
            List<Country> oCountryList = new List<Country>();
            List<States> oStatesList = new List<States>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[country_states_list_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                //cm.Parameters.AddWithValue("@language_name", LanguageName);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgaNizationId"]))
                {
                    cm.Parameters.AddWithValue("org_id", Convert.ToInt32(ConfigurationManager.AppSettings["OrgaNizationId"].ToString()));
                }
                else
                {
                    cm.Parameters.AddWithValue("org_id", MemberIdentity.Client.ClientId);
                }
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        Country oCuntry = new Country();
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oCuntry.CountryId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["country_code"] != DBNull.Value)
                        {
                            oCuntry.CountryCode = Convert.ToString(dr["country_code"].ToString());
                        }
                        if (dr["country_name"] != DBNull.Value)
                        {
                            oCuntry.CountryName = Convert.ToString(dr["country_name"]);
                        }
                        if (dr["code"] != DBNull.Value)
                        {
                            oCuntry.Code = Convert.ToString(dr["code"].ToString());
                        }
                        if (dr["country_namefor_partner"] != DBNull.Value)
                        {
                            oCuntry.CountryNameforPartner = Convert.ToString(dr["country_namefor_partner"].ToString());
                        }
                        oCountryList.Add(oCuntry);
                    }
                    oCountryAndState.CountryList = oCountryList;
                    dr.NextResult();
                    while (dr.Read())
                    {
                        States oStates = new States();

                        if (dr["state_id"] != DBNull.Value)
                        {
                            oStates.StateId = Convert.ToInt32(dr["state_id"]);
                        }
                        if (dr["country_id"] != DBNull.Value)
                        {
                            oStates.CountryId = Convert.ToInt32(dr["country_id"]);
                        }
                        if (dr["state_code"] != DBNull.Value)
                        {
                            oStates.StateCode = Convert.ToString(dr["state_code"]);
                        }
                        if (dr["state_name"] != DBNull.Value)
                        {
                            oStates.StateName = Convert.ToString(dr["state_name"]);
                        }
                        oStatesList.Add(oStates);
                    }

                    oCountryAndState.StateList = oStatesList;
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
            return oCountryAndState;
        }
        #endregion

        #region  SaveUserDetails
        /// <summary>
        /// SaveDetails
        /// </summary>
        /// <param name="oMemberEntity"></param>
        /// <returns></returns>

        public MemberEntity SaveDetails(MemberEntity oMemberEntity)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            var rid = 14176;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[partner].[UserDetails_Insert]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@rid", Convert.ToInt32(rid));
                cm.Parameters.AddWithValue("@extmid", oMemberEntity.ExtId);
                cm.Parameters.AddWithValue("@country_id", oMemberEntity.CountryName);
                cm.Parameters.AddWithValue("@gender", oMemberEntity.Gender);
                if (oMemberEntity.Dob != string.Empty)
                {
                    cm.Parameters.AddWithValue("@dob", oMemberEntity.Dob);
                }
                else
                {
                    cm.Parameters.AddWithValue("@dob", DBNull.Value);
                }
                cm.Parameters.AddWithValue("@create_by", oMemberEntity.ExtId);
                cm.Parameters.AddWithValue("@update_by", oMemberEntity.ExtId);

                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (dr["user_id"] != DBNull.Value)
                        {
                            oMemberEntity.UserId = Convert.ToInt32(dr["user_id"]);
                        }
                        if (dr["user_guid"] != DBNull.Value)
                        {
                            oMemberEntity.UserGuid = new Guid(dr["user_guid"].ToString());
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

        #region Get Ethinicity List
        /// <summary>
        /// GetEthinicity
        /// </summary>
        /// <param name="Language_name"></param>
        /// <returns></returns>

        public List<Ethnicity> GetEthinicity(string Language_name)
        {
            Ethnicity objEthinicity = new Ethnicity();
            List<Ethnicity> lstEthinicty = new List<Ethnicity>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                //User objUser = new User();
                cn.Open();
                SqlCommand cm = new SqlCommand("[lookup].[ethnicity_list_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@language_name", Language_name);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Ethnicity oEthinicty = new Ethnicity();
                        if (dr["ethnicity_id"] != DBNull.Value)
                        {
                            oEthinicty.EthnicityId = Convert.ToInt32(dr["ethnicity_id"]);
                        }

                        if (dr["ethnicity_type"] != DBNull.Value)
                        {
                            oEthinicty.EthnicityType = Convert.ToString(dr["ethnicity_type"]);
                        }

                        lstEthinicty.Add(oEthinicty);
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
            return lstEthinicty;
        }
        #endregion
        #region  for getting redeem CouponNames
        /// <summary>
        /// RedeemCoupons
        /// </summary>
        /// <param name="RedeemptionGuId"></param>
        /// <param name="UId"></param>
        /// <returns></returns>
        public List<Rewards> RedeemCoupons(Guid RedeemptionGuId, int UId)
        {

            List<Rewards> lstRewards = new List<Rewards>();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {

                cn.Open();
                SqlCommand cm = new SqlCommand("[iframe].[user_codes_redeemed]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", UId);
                cm.Parameters.AddWithValue("@redemption_guid", RedeemptionGuId);
                using (IDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Rewards objReward = new Rewards();
                        if (dr["code_name"] != DBNull.Value)
                        {
                            objReward.CodeName = Convert.ToString(dr["code_name"]);
                        }
                        lstRewards.Add(objReward);

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
            return lstRewards;
        }
        #endregion
    }
}
