using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
namespace Members.PrecisionSample.Components.Business_Layer
{
    class SuppressManager
    {
        #region Data Insert

        /// <summary>
        /// get Insert emailaddress
        /// </summary>
        /// <param name="countryId">countryId</param>
        /// <returns></returns>
        public void InsertEmailAddress(string strEmailAddress)
        {
            SuppressionDataServer oStatesDataServices = new SuppressionDataServer();
            oStatesDataServices.InsertEmailAddress(strEmailAddress);
        }

        #endregion
    }
}
