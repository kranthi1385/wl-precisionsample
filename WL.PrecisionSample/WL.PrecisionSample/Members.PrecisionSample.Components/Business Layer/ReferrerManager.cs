using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ReferrerManager
    {
        #region Get traking Details 

        /// <summary>
        /// Get Referrer Tracking Details
        /// </summary>
        /// <param name="referrer_id"></param>
        /// <returns></returns>
        public Referrer GetReferrerTrackingDetails(int referrer_id)
        {
            ReferrerDataServer oServer = new ReferrerDataServer();
            return oServer.GetReferrerTrackingDetails(referrer_id);
        }

        #endregion 

        #region GetLandingPage
        /// <summary>
        /// Get Landing pageUrl
        /// </summary>
        /// <param name="referrerid"></param>
        /// <returns></returns>
        public string GetLandingpageUrl(int referrerid)
        {
            ReferrerDataServer oServer = new ReferrerDataServer();
            return oServer.GetLandingpageUrl(referrerid);
        }
        #endregion
    }
}
