using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Home
    {
        #region public variables
        /// <summary>
        /// 
        /// </summary>
        public int TotalSurveys { get; set; }
        public decimal TotalRewardforSurvey { get; set; }
        public int SecondReferrals { get; set; }
        public int ThirdReferrals { get; set; }
        public decimal RewardsEarned { get; set; }
        public decimal AccountBalance { get; set; }
        public int IpadEntryCount { get; set; }
        #endregion
    }
}
