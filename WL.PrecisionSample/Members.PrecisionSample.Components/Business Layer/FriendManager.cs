using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class FriendManager
    {
        #region Friend Insert

        /// <summary>
        /// get friend inserted
        /// </summary>
        /// <param name="userId">userid</param>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public List<Friend> FriendInsertList(int UserId, string xml,int ClientId)
        {
            List<Friend> ofriend = new List<Friend>();
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendInsert(UserId, xml, ClientId);
        }

        #endregion

        #region Fetch FriendList-Information

        /// <summary>
        /// get friend information from friend
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Friend> FriendInformation(int UserId,int ClientId)
        {
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendInformation(UserId, ClientId);
        }

        #endregion

        #region FriendInsert

        /// <summary>
        /// FriendInsert
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public List<Friend> FriendList(int UserId,int ClientId)
        {
            FriendDataServices oFriendDataServices = new FriendDataServices();
            return oFriendDataServices.FriendList(UserId, ClientId);
        }

        #endregion

        //#region UnSubscribe Friend
        ///// <summary>
        ///// get Unsubscribe Friend using EmailAddress
        ///// </summary>
        ///// <param name="EmailAddress">EmailAddress</param>

        //public void UnsubscribeFriend(string EmailAddress)
        //{
        //    FriendDataServices oFriendDataServices = new FriendDataServices();
        //    oFriendDataServices.UnsubscribeFriend(EmailAddress);
        //}
        //#endregion

        //#region Fetch FriendList-Information

        ///// <summary>
        ///// get friend information from friend
        ///// </summary>
        ///// <param name="userId">userId</param>
        ///// <returns></returns>
        //public List<Friend> FriendList(int UserId)
        //{
        //    FriendDataServices oFriendDataServices = new FriendDataServices();
        //    return oFriendDataServices.FriendList(UserId);
        //}

        //#endregion
    }
}
