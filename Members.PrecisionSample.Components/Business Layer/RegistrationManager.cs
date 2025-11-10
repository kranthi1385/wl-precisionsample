using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
namespace Members.PrecisionSample.Components.Business_Layer
{
    public class RegistrationManager
    {
        #region LeadUpdates
        /// <summary>
        /// Lead Updates
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LeadType"></param>
        /// <param name="advertiserid"></param>
        public void LeadUpdates(int UserId, string LeadType, int advertiserid)
        {
            RegistrationDataServer oRegistrationDataServer = new RegistrationDataServer();
            oRegistrationDataServer.LeadUpdates(UserId, LeadType, advertiserid);
        }

        #endregion

        #region LeadRevenueUpdate
        /// <summary>
        /// Lead Revenue Update
        /// </summary>
        /// <param name="oLeadEntities"></param>
        public void LeadRevenueUpdate(LeadEntities oLeadEntities)
        {
            RegistrationDataServer oRegistrationDataServer = new RegistrationDataServer();
            oRegistrationDataServer.LeadRevenueUpdate(oLeadEntities);
        }
        #endregion
    }
}
