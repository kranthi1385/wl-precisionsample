using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Data_Layer;
using Members.OpinionBar.Components.Entities;

namespace Members.OpinionBar.Components.Business_Layer
{
    public class RewardManager
    {
        #region Rewards Insert

        /// <summary>
        /// get rewards
        /// </summary>
        /// <param name = "userId" > userId </ param >
        /// < returns ></ returns >
        public Rewards GetRewardsHistory(int userId)
        {
            RewardDataServices oRewardDataServices = new RewardDataServices();
            return oRewardDataServices.GetRewardsHistory(userId);
        }
        #endregion
        public TRewards GeTangoRewardsBySKU(string sku, string userGuid, string lang, int cid, string name)
        {
            RewardDataServices oRewardDataServices = new RewardDataServices();
            return oRewardDataServices.GeTangoRewardsBySKU(sku, userGuid, lang, cid, name);
        }
        #region Get Reddem History

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Rewards GetRedeemHistory(string UserGuid)
        {
            RewardDataServices oServer = new RewardDataServices();
            return oServer.GetRedeemHistory(UserGuid);
        }
        #endregion

        #region insert redemptions

        /// <summary>
        /// get insert reward redeemptions 
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="CatalougeGuid">catalougeguid</param>
        public int InsertUsertRewardRedeemprtions(int amount, Guid CatalougeGuid, Guid userguid, int client_id)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.InsertUsertRewardRedeemprtions(amount, CatalougeGuid, userguid, client_id);
        }
        #endregion

        #region
        public RedeemptionHistory GetApiResponsebtId(int id)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.GetApiResponsebyId(id);
        }
        #endregion

        #region Reward Option Get

        /// <summary>
        /// Reward Option Get
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Rewards> RewardOptionGet(int userId, string userGuid, int clientid)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.RewardOptionGet1(userId, userGuid, clientid);
        }
        #endregion

        #region Get Tango Rewards
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Catalouge_guid">Catalouge_guid</param>
        /// <returns></returns>
        public List<TRewards> GetTangoRewards(string userGuid)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.GetTangoRewards(userGuid);
        }
        #endregion


        //#region
        ///// <summary>
        ///// Get FBCodes For User
        ///// </summary>
        ///// <param name="redemption_id">redemption_id</param>
        ///// <returns></returns>
        //public string GetFBCodesForUser(int redemption_id)
        //{
        //    RewardDataServices oServices = new RewardDataServices();
        //    return oServices.GetFBCodesForUser(redemption_id);
        //}

        //#endregion
        public int RedeemMemberRewards(string Sku, decimal Ut, int UserId, int Points, int OrgId, string FirstName, string EmailAddress, Guid ug, string ip,string IpNumber, string name)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.RedeemMemberRewards(Sku, Ut, UserId, Points, OrgId, FirstName, EmailAddress, ug, ip, IpNumber, name);
        }
        public Rewards GetDetailsById(Guid Catalouge_guid, Guid ug, int ClientId)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.GetDetailsById(Catalouge_guid, ug, ClientId);
        }
        public int InserrtRewardRedeemprtions(int amount, Guid catalougGuid, int orgId, string ug, string Ip)
        {
            RewardDataServices oServices = new RewardDataServices();
            return oServices.InserrtRewardRedeemprtions(amount, catalougGuid, orgId, ug, Ip);
        }
    }
}