using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class Page2LeadsManager
    {
        #region Page2LeadsEntities
        /// <summary>
        /// get page2 leads 
        /// </summary>
        /// <param name="Range">Range</param>
        /// <param name="StartDate">StartDate</param>
        /// <param name="EndDate">EndDate</param>
        /// <returns></returns>
        public Page2LeadsEntities GetPage2Leads(string Range, string StartDate, string EndDate)
        {
            Page2LeadsDataServer oPage2LeadsDataServer = new Page2LeadsDataServer();
            return oPage2LeadsDataServer.GetPage2Leads(Range, StartDate, EndDate);
        }
        #endregion
    }
}
