using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
    public class Rewards
    {
        #region public Variables

        private List<RewardHistory> _lstRewards = new List<RewardHistory>();
        private List<RewardsRedeemedHistory> _lstRedeem = new List<RewardsRedeemedHistory>();
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public decimal TotalRewardAmout { get; set; }
        public int Mode { get; set; }
        public int RedemptionId { get; set; }
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
        public decimal AccountBalanceLocalCurrency { get; set; }
        public decimal TotalEarningsLocalCurrency { get; set; }
        public decimal TotalRedemptionsLocalCurrency { get; set; }
        public string CurrencyNotation { get; set; }
        public decimal LocalCurrency { get; set; }

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
        public List<RewardsRedeemedHistory> LstRedeemptionHistory
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
        #endregion
    }
    public class RewardHistory
    {
        #region public Variables
        public string CreateDt { get; set; }
        public string Descripion { get; set; }
        public decimal RewardAmount { get; set; }
        public string RewardText { get; set; }
        public decimal RedemptionAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal Level1Reward { get; set; }
        public decimal Level2Reward { get; set; }
        public string Status { get; set; }
        public string RewardExpiryDt { get; set; }

        #endregion

    }
    public class RewardsRedeemedHistory
    {
        public int RedeemptionId { get; set; }
        public int UserId { get; set; }
        public string RedemptionType { get; set; }
        public decimal RedemptionAmount { get; set; }
        public string CreateDt { get; set; }
        public string GiftcardName { get; set; }
        public string Descripion { get; set; }
        public string ApiResponse { get; set; }

    }
}
