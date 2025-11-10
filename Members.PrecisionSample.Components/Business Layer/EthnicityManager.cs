using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class EthnicityManager
    {
        #region Data Fetch - EthnicityType

        /// <summary>
        /// Get Ethnicity List
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public List<Ethnicity> GetEthnicityList()
        {
            EthnicityDataServices oEthnicityDataServices = new EthnicityDataServices();
            return oEthnicityDataServices.GetEthnicityList();
        }

        #endregion
    }
}
