using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class FacebookManager
    {
        #region GetHomePageInfo

        /// <summary>
        /// get home page details of user
        /// </summary>
        /// <param name="UserGuid">userguid</param>
        public Home GetHomePageDetails(int userId)
        {
            FacebookDataServer oservice = new FacebookDataServer();
            return oservice.GetHomePageDetails(userId);
        }
        #endregion

        #region Get Top3 Uncliked perks

        /// <summary>
        /// get top3 surveys of user
        /// </summary>
        /// <returns></returns>
        public List<Perks> GetTop3Surveys(int userId)
        {
            FacebookDataServer oService = new FacebookDataServer();
            return oService.GetTop3Surveys(userId);
        }

        #endregion

        #region GetProfiles

        /// <summary>
        /// get profiles of user
        /// </summary>
        /// <returns></returns>
        public List<Profile> GetProfiles(int userId)
        {
            FacebookDataServer odataserver = new FacebookDataServer();
            return odataserver.GetProfiles(userId);
        }
        #endregion

        #region Get User Perks
        /// <summary>
        /// get perks list of user
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        public List<Perks> GetPerksList(int userId)
        {
            FacebookDataServer oUserDataServices = new FacebookDataServer();
            return oUserDataServices.GetPerksList(userId);
        }

        #endregion

        #region Get freebies list

        /// <summary>
        /// get freebies list of user
        /// </summary>
        /// <param name="userId">userid</param>
        /// <returns></returns>
        public List<Surveys> GetFreebiesList(int userId)
        {
            FacebookDataServer oUserDataServices = new FacebookDataServer();
            return oUserDataServices.GetFreebiesList(userId);
        }


        #endregion

        #region facebook user check
        /// <summary>
        /// get facebook app user check
        /// </summary>
        /// <param name="userGuid">userguid</param>
        /// <param name="facebookId">facebookid</param>
        /// <returns></returns>

        public void Facebokappusercheck(Guid userGuid, string facebookId, string accesstoken)
        {
            FacebookDataServer oserver = new FacebookDataServer();
            oserver.Facebokappusercheck(userGuid, facebookId, accesstoken);
        }

        #endregion

        #region redeem amount
        /// <summary>
        /// insert reward redeemption
        /// </summary>
        /// <param name="userGuid">userguid</param>
        /// <param name="facebookId">facebookid</param>
        /// <returns></returns>

        public void IserrtRewardRedeemprtions(int userId, int amount)
        {
            FacebookDataServer oServices = new FacebookDataServer();
            oServices.IserrtRewardRedeemprtions(userId, amount);
        }

        #endregion
    }
}
