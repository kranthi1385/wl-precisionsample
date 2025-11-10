using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
   public  class RewardsRedeemedHistory
    {
        public string RedemptionType { get; set; }
        public decimal RedemptionAmount { get; set; }
        public string CreateDt { get; set; }

        public string Descripion { get; set; }

    }
}
