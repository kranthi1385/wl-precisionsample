using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class CountryAndState
    {
        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public List<Country> CountryList
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<States> StateList
        {
            get; set;
        }
        #endregion
    }
}
