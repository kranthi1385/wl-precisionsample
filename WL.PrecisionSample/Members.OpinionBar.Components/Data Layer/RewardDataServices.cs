using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Members.OpinionBar.Components.Data_Layer
{
    public class RewardDataServices
    {
        #region ConnectionString
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
        public string ConnectionStringEmail
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringEmail"].ToString();
            }
        }
        #endregion

        #region Data Fetch - Reward History

        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public Rewards GetRewardsHistory(int userId)
        {
            Rewards oRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConnectionString;
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[Reward_History_Get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader reader = cm.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["account_balance"] != DBNull.Value)
                        {
                            oRewards.AccountBalance = Convert.ToDecimal(reader["account_balance"]);
                        }
                        if (reader["total_earnings"] != DBNull.Value)
                        {
                            oRewards.TotalEarnings = Convert.ToDecimal(reader["total_earnings"]);
                        }
                        if (reader["total_redemptions"] != DBNull.Value)
                        {
                            oRewards.TotalRedemptions = Convert.ToDecimal(reader["total_redemptions"]);
                        }

                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        RewardHistory oRewardHistory = new RewardHistory();

                        // oRewards.ProjectName = reader["project_name"].ToString();

                        if (reader["reward_dt"] != DBNull.Value)
                        {
                            oRewardHistory.CreateDt = Convert.ToDateTime(reader["reward_dt"]).ToString();
                        }
                        if (reader["description"] != DBNull.Value)
                        {
                            oRewardHistory.Descripion = Convert.ToString(reader["description"]);
                        }

                        if (reader["direct_reward"] != DBNull.Value)
                        {
                            oRewardHistory.RewardAmount = Convert.ToDecimal(reader["direct_reward"]);
                        }
                        if (reader["redemption_amount"] != DBNull.Value)
                        {
                            oRewardHistory.RedemptionAmount = Convert.ToDecimal(reader["redemption_amount"]);
                        }
                        if (reader["balance_amount"] != DBNull.Value)
                        {
                            oRewardHistory.BalanceAmount = Convert.ToDecimal(reader["balance_amount"]);
                        }
                        if (reader["level_1"] != DBNull.Value)
                        {
                            oRewardHistory.Level1Reward = Convert.ToDecimal(reader["level_1"]);
                        }
                        if (reader["level_2"] != DBNull.Value)
                        {
                            oRewardHistory.Level2Reward = Convert.ToDecimal(reader["level_2"]);
                        }
                        if (reader["reward_expiry_dt"] != DBNull.Value)
                        {
                            oRewardHistory.RewardExpiryDt = Convert.ToDateTime(reader["reward_expiry_dt"]).ToString();
                        }
                        if (reader["status"] != DBNull.Value)
                        {
                            oRewardHistory.Status = Convert.ToString(reader["status"]);
                        }
                        //oRewards.LstRewardHistory.Add(oRewardHistory);
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
            return oRewards;
        }

        #endregion

        #region Data Fetch - Redeemed History
        /// <summary>
        /// Get Redeem History
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Rewards GetRedeemHistory(string UserGuid)
        {
            Rewards oRewards = new Rewards();
            RewardDataServices oServer = new RewardDataServices();
            string constr = "";// oServer.GetConnectionString(UserGuid);
            SqlConnection cn = new SqlConnection();
            if (!string.IsNullOrEmpty(constr))
                cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            else
                cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();

            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[redemptionhistoryformember]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandTimeout = 1140;
                cm.Parameters.AddWithValue("user_guid", UserGuid);
                using (SqlDataReader reader = cm.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        RewadrsRedeemedHistory oRewardsRedeemedHistory = new RewadrsRedeemedHistory();
                        if (reader["redemption_amount"] != DBNull.Value)
                        {
                            oRewardsRedeemedHistory.RedemptionAmount = Convert.ToDecimal(reader["redemption_amount"]);

                        }
                        if (reader["redemption_dt"] != DBNull.Value)
                        {
                            oRewardsRedeemedHistory.CreateDt = Convert.ToDateTime(reader["redemption_dt"]).ToString();
                        }
                        if (reader["catalouge_name"] != DBNull.Value)
                        {
                            oRewardsRedeemedHistory.RedemptionType = reader["catalouge_name"].ToString();
                        }
                        if (reader["codes"] != DBNull.Value)
                        {
                            oRewardsRedeemedHistory.Descripion = reader["codes"].ToString();
                        }
                        //oRewards.LstRedeemptionHistory.Add(oRewardsRedeemedHistory);
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
            return oRewards;
        }



        #endregion

        #region IserrtRewardRedeemprtions

        /// <summary>
        /// Inserrt Reward Redeemprtions
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="catalougGuid">catalougguid</param>
        public int InsertUsertRewardRedeemprtions(int amount, Guid catalougGuid, Guid userguid, int client_id)
        {
            string constr = string.Empty;
            int redemption_id = 0;
            if (client_id > -2)
            {
                UserDataServices oservices = new UserDataServices();
                constr = oservices.GetConnectionString(null, null, client_id);
            }

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            SqlCommand cm = new SqlCommand("[rms].[Reward_Redeem_user]", cn);
            cm.CommandType = CommandType.StoredProcedure;

            #region Adding params for the SQL Proc (IF NOT NULL)
            cm.Parameters.AddWithValue("@user_guid", userguid);
            cm.Parameters.AddWithValue("@redeem_amount", Convert.ToDecimal(amount));
            cm.Parameters.AddWithValue("@catalouge_guid", catalougGuid);
            #endregion
            cn.Open();
            using (IDataReader reader = cm.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader["redemption_id"] != DBNull.Value)
                        {
                            redemption_id = Convert.ToInt32(reader["redemption_id"]);
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
                return redemption_id;
            }

        }


        #endregion

        #region Get Api Response by Id
        public RedeemptionHistory GetApiResponsebyId(int id)
        {
            RedeemptionHistory redeemptionHistory = new RedeemptionHistory();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            cn.Open();
            SqlCommand cm = new SqlCommand("[rms].[get_api_response_by_id]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@id", id);
            using(SqlDataReader reader = cm.ExecuteReader())
            {
                try
                {
                    while (reader.Read())
                    {
                        if (reader["redeemption_id"] != DBNull.Value)
                        {
                            redeemptionHistory.redeemptionId = Convert.ToInt32(reader["redeemption_id"]);
                        }
                        if (reader["user_id"] != DBNull.Value)
                        {
                            redeemptionHistory.userId = Convert.ToInt32(reader["user_id"]);
                        }
                        if (reader["api_response"] != DBNull.Value)
                        {
                            redeemptionHistory.apiResponse = (reader["api_response"]).ToString();
                        }                        
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                finally
                {

                }
                return redeemptionHistory;
            }           
        }
        #endregion

        #region Get Tango Rewards

        /// <summary>
        /// Get Tango Rewards
        /// </summary>

        /// <returns></returns>
        public List<TRewards> GetTangoRewards(string userGuid)
        {
            List<TRewards> lstTRewards = new List<TRewards>();
            RewardDataServices oServer = new RewardDataServices();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[user].[tangoReward_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cm.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        TRewards objTRewards = new TRewards();

                        if (reader["Name"] != DBNull.Value)
                        {
                            objTRewards.Name = (reader["Name"]).ToString();
                        }
                        if (reader["category"] != DBNull.Value)
                        {
                            objTRewards.Category = (reader["category"]).ToString();
                        }
                        lstTRewards.Add(objTRewards);
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

            return lstTRewards;
        }
        #endregion

        #region Reward Option Get

        /// <summary>
        /// Reward Option Get
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Rewards> RewardOptionGet1(int userId, string UserGuid, int clientid)
        {
            List<Rewards> objRewardOptionList = new List<Rewards>();

            UserDataServices oserver = new UserDataServices();
            string constr = oserver.GetConnectionString(null, null, clientid);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[constr].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[usercatalouges_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", userId);
                //cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rewards oRewardOption = new Rewards();
                        if (reader["catalouge_name"] != null)
                        {
                            oRewardOption.RewardName = Convert.ToString(reader["catalouge_name"]);
                        }
                        if (reader["catalouge_description"] != null)
                        {
                            oRewardOption.RewardDescription = Convert.ToString(reader["catalouge_description"]);
                        }
                        if (reader["minimum_reward_amount"] != null)
                        {
                            oRewardOption.MinRedemptionAmount = Convert.ToInt32(reader["minimum_reward_amount"]);
                        }
                        if (reader["catalouge_id"] != null)
                        {
                            oRewardOption.CatalogueId = Convert.ToInt32(reader["catalouge_id"]);
                        }
                        if (reader["catalouge_logo"] != null)
                        {
                            oRewardOption.RewardLogo = Convert.ToString(reader["catalouge_logo"]);
                        }
                        if (reader["catalouge_guid"] != null)
                        {
                            oRewardOption.CatalogueGuid = new Guid(reader["catalouge_guid"].ToString());
                        }
                        objRewardOptionList.Add(oRewardOption);
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
            return objRewardOptionList;
        }
        #endregion

        #region IserrtMemberRewardRedeemprtions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        public int RedeemMemberRewards(string Sku, decimal Ut, int UserId, int Points, int OrgId, string FirstName, string EmailAddress, Guid ug, string ip, string IpNumber, string name)
        {
            string Email = string.Empty;
            string CustomAttributes = string.Empty;
            int OrgID = 0;
            string UserGUID = string.Empty;
            decimal amount = 0;
            int redemptionID = 0;
            string firstName = string.Empty;
            int languageID = 0;
            int campaignID = 0;
            int countryID = 0;
            string currency = string.Empty;
            int IpCheck = 0;
            Rewards objRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            UserDataServices oserver = new UserDataServices();
            string con = oserver.GetConnectionString(null, null, OrgId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[con].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[user_tango_redemption_insert_ob]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(OrgId));
                cm.Parameters.AddWithValue("@user_id", Convert.ToInt32(UserId));
                cm.Parameters.AddWithValue("@first_name", FirstName);
                cm.Parameters.AddWithValue("@email_address", EmailAddress);
                cm.Parameters.AddWithValue("@amount_redeemed", Convert.ToDecimal(Ut));
                cm.Parameters.AddWithValue("@amount_redeemed_in_points", Convert.ToInt32(Points));
                cm.Parameters.AddWithValue("@sku", Convert.ToString(Sku));
                cm.Parameters.AddWithValue("@ip_address", Convert.ToString(ip));
                cm.Parameters.AddWithValue("@ip_number", IpNumber);
                cm.Parameters.AddWithValue("@giftcard_name", name);
                //cm.ExecuteNonQuery();
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email_address"] != DBNull.Value)
                        {
                            Email = (reader["email_address"].ToString());
                        }
                        if (reader["user_guid"] != DBNull.Value)
                        {
                            UserGUID = (reader["user_guid"].ToString());
                        }
                        if (reader["org_id"] != DBNull.Value)
                        {
                            OrgID = Convert.ToInt32(reader["org_id"].ToString());
                        }
                        if (reader["redemption_amount"] != DBNull.Value)
                        {
                            amount = Convert.ToDecimal(reader["redemption_amount"].ToString());
                        }
                        if (reader["redemption_id"] != DBNull.Value)
                        {
                            redemptionID = Convert.ToInt32(reader["redemption_id"].ToString());
                        }
                        if (reader["first_name"] != DBNull.Value)
                        {
                            firstName = Convert.ToString(reader["first_name"].ToString());
                        }
                        if (reader["language_id"] != DBNull.Value)
                        {
                            languageID = Convert.ToInt32(reader["language_id"].ToString());
                        }
                        if (reader["country_id"] != DBNull.Value)
                        {
                            countryID = Convert.ToInt32(reader["country_id"].ToString());
                        }
                        if (reader["ip_check"] != DBNull.Value)
                        {
                            IpCheck = Convert.ToInt32(reader["ip_check"].ToString());
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
            if (IpCheck == 1)
            {
                cn = new SqlConnection();
                cn.ConnectionString = ConnectionStringEmail;
                if (languageID == 27)
                {
                    campaignID = 1901;
                }
                else if (languageID == 36)
                {
                    campaignID = 1902;
                }
                else if (languageID == 44)
                {
                    campaignID = 1903;
                }
                else if (languageID == 51)
                {
                    campaignID = 1904;
                }
                else if (languageID == 69)
                {
                    campaignID = 1905;
                }
                else if (languageID == 70)
                {
                    campaignID = 1906;
                }
                else if (languageID == 80)
                {
                    campaignID = 1907;
                }
                else if (languageID == 110)
                {
                    campaignID = 1908;
                }
                else if (languageID == 111)
                {
                    campaignID = 1909;
                }
                else if (languageID == 120)
                {
                    campaignID = 1910;
                }
                else if (languageID == 140)
                {
                    campaignID = 1911;
                }
                else
                {
                    campaignID = 1900;
                }
                if (countryID == 231)
                {
                    currency = "$";
                }
                else if (countryID == 229)
                {
                    currency = "£";
                }
                else if (countryID != 231 && countryID != 229)
                {
                    currency = "€";
                }
                CustomAttributes = "redeemed_amount:" + currency + amount + ";user_guid:" + UserGUID + ";redemption_id:" + redemptionID + ";first_name:" + firstName;
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("[transactional].[campaign.campaignitem_insert]", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("campaign_id", campaignID);
                    cmd.Parameters.AddWithValue("email_address", Email);
                    cmd.Parameters.AddWithValue("custom_attibute", CustomAttributes);
                    cmd.Parameters.AddWithValue("org_id", OrgID);
                    cmd.ExecuteNonQuery();
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
            return IpCheck;
        }


        #endregion

        #region Data Fetch - GeTangoRewardsBySKU

        /// <summary>
        /// GeTangoRewardsBySKU
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="userGuid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public TRewards GeTangoRewardsBySKU(string sku, string userGuid, string lang, int clientId, string name)
        {
            TRewards objTangoReward = new Entities.TRewards();
            SqlConnection cn = new SqlConnection();
            UserDataServices oServices = new UserDataServices();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[tango_rewards_by_sku_get]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@sku", sku);
                cm.Parameters.AddWithValue("@language_name", lang);
                cm.Parameters.AddWithValue("@reward_name", name);
                if (name != "")
                {
                    objTangoReward.RewardName = name;
                }
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["reward_name"] != DBNull.Value)
                        {
                            objTangoReward.RewardName = Convert.ToString(reader["reward_name"]);
                        }
                        if (reader["disclaimer"] != DBNull.Value)
                        {
                            objTangoReward.Disclaimer = Convert.ToString(reader["disclaimer"]).ToString();
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
            return objTangoReward;
        }

        #endregion

        #region GetCatalouge Details By GUid

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Catalouge_guid"></param>
        /// <returns></returns>
        public Rewards GetDetailsById(Guid Catalouge_guid, Guid ug, int ClientId)
        {

            Rewards oRewardOption = new Rewards();
            UserDataServices oserver = new UserDataServices();
            string con = oserver.GetConnectionString(null, null, ClientId);
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            cn.Open();
            SqlCommand cm = new SqlCommand("[rms].[Get_Catalogues_Details_By_Guid]", cn);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.AddWithValue("@catalouge_guid", Catalouge_guid);
            cm.Parameters.AddWithValue("@org_id", MemberIdentity.Client.ClientId);
            using (SqlDataReader reader = cm.ExecuteReader())
            {
                try
                {

                    while (reader.Read())
                    {
                        if (reader["catalouge_id"] != DBNull.Value)
                        {
                            oRewardOption.CatalogueId = Convert.ToInt32(reader["catalouge_id"]);
                        }
                        if (reader["catalouge_name"] != DBNull.Value)
                        {
                            oRewardOption.RewardName = (reader["catalouge_name"]).ToString();
                        }
                        if (reader["catalouge_description"] != DBNull.Value)
                        {
                            oRewardOption.RewardDescription = (reader["catalouge_description"]).ToString();
                        }
                        if (reader["minimum_reward_amount"] != DBNull.Value)
                        {
                            oRewardOption.MinRedemptionAmount = Convert.ToInt32(reader["minimum_reward_amount"]);
                        }
                        if (reader["catalouge_logo"] != DBNull.Value)
                        {
                            oRewardOption.RewardLogo = (reader["catalouge_logo"]).ToString();
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
            }
            return oRewardOption;
        }
        #endregion

        #region IserrtRewardRedeemprtions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="catalougGuid"></param>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public int InserrtRewardRedeemprtions(int amount, Guid catalougGuid, int orgId, string ug,string Ip )
        {
            int redemId = 0;
            Rewards objRewards = new Rewards();
            SqlConnection cn = new SqlConnection();
            UserDataServices oDataService = new UserDataServices();
            string con = oDataService.GetConnectionString(null, null, orgId);
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[con].ToString();
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand("[rms].[Reward_Redeem_user]", cn);
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@org_id", Convert.ToInt32(orgId));
                cm.Parameters.AddWithValue("@user_guid", ug);
                cm.Parameters.AddWithValue("@redeem_amount", amount);
                cm.Parameters.AddWithValue("@catalouge_guid", catalougGuid);
                cm.Parameters.AddWithValue("@ip_address", Ip);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {

                        if (dr["redemption_id"] != DBNull.Value)
                        {
                            redemId = Convert.ToInt32(dr["redemption_id"].ToString());
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
            return redemId;

        }


        #endregion
    }
}
