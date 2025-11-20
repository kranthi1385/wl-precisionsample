using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;

namespace Members.PrecisionSample.Components.Business_Layer
{
    class FaceBookFriendManager
    {
        #region Get FaceBook Users
        /// <summary>
        /// Get FaceBook Users
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<FaceBookFriend> GetFaceBookUsers(string userIds)
        {
            FaceBookFriendDataServer oFaceBookFriendDataServer = new FaceBookFriendDataServer();
            return oFaceBookFriendDataServer.GetFaceBookUsers(userIds);
        }
        #endregion

        #region Friend Insert

        /// <summary>
        /// Friend Insert
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public List<FaceBookFriend> FriendInsert(int userId, string xml)
        {
            FaceBookFriendDataServer oFaceBookFriendDataServer = new FaceBookFriendDataServer();
            return oFaceBookFriendDataServer.FriendInsert(userId, xml);
        }
        #endregion
    }
}
