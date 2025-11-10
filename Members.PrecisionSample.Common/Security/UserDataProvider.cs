using System;
using System.Collections.Generic;
using System.Text;

namespace Members.PrecisionSample.Common.Security
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UserDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        protected UserDataProvider() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract UserData Retrieve(Guid userID);
     
    }

}
