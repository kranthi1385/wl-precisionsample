using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;

namespace Members.OpinionBar.Components.Business_Layer
{
   public class AffiliateTrackingManager
    {
        #region GetTrackingDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<AffiliateTrackingEntities> GetTrackingDetails(int ReferrerId, int userid, int cid)
        {
            AffiliateTrackingDataServer oATD = new AffiliateTrackingDataServer();
            return oATD.GetTrackingDetails(ReferrerId, userid, cid);
        }
        #endregion
    }
}
