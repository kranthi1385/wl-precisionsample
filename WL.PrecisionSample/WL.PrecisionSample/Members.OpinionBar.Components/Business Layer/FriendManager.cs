using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;


namespace Members.OpinionBar.Components.Business_Layer
{
   public class FriendManager
    {
        #region Fetch FriendList-Information

        /// <summary>
        /// get friend information from friend
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Friend> FriendInformation(int UserId, int ClientId)
        {
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendInformation(UserId, ClientId);
        }

        #endregion

        #region Friend Insert

        /// <summary>
        /// get friend inserted
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public List<Friend> FriendInsertList(int UserId, string xml, int ClientId,int CampaignID,string Subject)
        {
            List<Friend> ofriend = new List<Friend>();
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendInsert(UserId, xml, ClientId, CampaignID, Subject);
        }

        #endregion
        #region FriendInsert

        /// <summary>
        /// FriendInsert
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Friend> FriendList(int UserId, int ClientId)
        {
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendList(UserId, ClientId);
        }

        #endregion
    }


}
