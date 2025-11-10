using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
   public class RewardHistory
    {
        #region public Variables
        public string CreateDt { get; set; }
        public string Descripion { get; set; }
        public decimal RewardAmount { get; set; }
        public decimal RedemptionAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal Level1Reward { get; set; }
        public decimal Level2Reward { get; set; }
        public string Status { get; set; }
        public string RewardExpiryDt { get; set; }

        #endregion
    }
}
