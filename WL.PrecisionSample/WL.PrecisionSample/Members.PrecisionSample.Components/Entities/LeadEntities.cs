using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
   public  class LeadEntities
    {
        #region Public Variables
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        public string TransactionKey { get; set; }
        public decimal Amount { get; set; }
        public int AdvertiserId { get; set; }

        #endregion

    }
}
