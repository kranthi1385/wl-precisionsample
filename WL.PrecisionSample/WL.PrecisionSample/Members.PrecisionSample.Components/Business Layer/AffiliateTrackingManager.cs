using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Business_Layer
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
        public List<AffiliateTrackingEntities> GetTrackingDetails(int ReferrerId, int userid,int cid)
        {
            AffiliateTrackingDataServer oATD = new AffiliateTrackingDataServer();
            return oATD.GetTrackingDetails(ReferrerId, userid,cid);
        }
        #endregion

        #region Step2GetTrackingDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReferrerId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<AffiliateTrackingEntities> Step2GetTrackingDetails(int ReferrerId, int userid)
        {
            AffiliateTrackingDataServer oATD = new AffiliateTrackingDataServer();
            return oATD.Step2GetTrackingDetails(ReferrerId, userid);
        }
        #endregion
    }
}
