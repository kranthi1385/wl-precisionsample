using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ExternalQuotaManager
    {

        #region Insert - External Quota Members
        /// <summary>
        /// Insert External Quota Member
        /// </summary>
        /// <param name="oExternalQuotaEntities"></param>
        /// <returns></returns>

        public ExternalQuotaEntities InsertExternalQuotaMember(ExternalQuotaEntities oExternalQuotaEntities)
        {
            ExternalQuotaDataServer oExternalQuotaDataServer = new ExternalQuotaDataServer();
            return oExternalQuotaDataServer.InsertExternalQuotaMember(oExternalQuotaEntities);
        }

        #endregion

        #region Fetch External Member By Id
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extmemid"></param>
        /// <returns></returns>

        public ExternalQuotaEntities ExternalMemberByIdGet(Guid extmemid)
        {
            ExternalQuotaDataServer oExternalQuotaDataServer = new ExternalQuotaDataServer();
            return oExternalQuotaDataServer.ExternalMemberByIdGet(extmemid);
        }

        #endregion
    }
}
