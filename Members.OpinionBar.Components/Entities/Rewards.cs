using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class Rewards
    {
        #region public Variables
        private List<RewardHistory> _lstRewards = new List<RewardHistory>();
        private List<RewadrsRedeemedHistory> _lstRedeem = new List<RewadrsRedeemedHistory>();
        public int RedeemptionId { get; set; }
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public decimal TotalRewardAmout { get; set; }
        public int Mode { get; set; }
        public DateTime RewardExpiryDt { get; set; }
        public decimal AccountBalance { get; set; }
        public Guid UserGuid { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalRedemptions { get; set; }
        //new redemptions based
        public int CatalogueId { get; set; }
        public string RewardName { get; set; }
        public string RewardDescription { get; set; }
        public string RewardLogo { get; set; }
        public Guid CatalogueGuid { get; set; }
        public int MinRedemptionAmount { get; set; }
        public DateTime RewardExpiredDt { get; set; }
        public string CreatedDt { get; set; }
        public string CodeName { get; set; }
        public string Descripion { get; set; }
        public string EndDt { get; set; }
        public string RewardText { get; set; }
        public int IpCheck { get; set; }
        public decimal AccountBalanceLocalCurrency { get; set; }
        public decimal TotalEarningsLocalCurrency { get; set; }
        public decimal TotalRedemptionsLocalCurrency { get; set; }
        public string CurrencyNotation { get; set; }
        public decimal LocalCurrency { get; set; }
        public List<RewardAmount> LstRewardAmount { get; set; }
        public List<RewardHistory> LstRewardHistory
        {
            get
            {
                return _lstRewards;
            }

            set
            {
                _lstRewards = value;
            }
        }
        public List<RewadrsRedeemedHistory> LstRedeemptionHistory
        {
            get
            {
                return _lstRedeem;
            }

            set
            {
                _lstRedeem = value;
            }
        }
        public List<string> RedeemptionInstructions = new List<string>();
        #endregion

    }
    public class RewardAmount
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class RedeemptionHistory
    {
        public int redeemptionId { get; set; }
        public int userId { get; set; }
        public string apiResponse { get; set; }
    }


}