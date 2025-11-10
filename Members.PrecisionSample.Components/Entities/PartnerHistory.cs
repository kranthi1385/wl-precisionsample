using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members.PrecisionSample.Components.Entities
{
    public class PartnerHistory
    {
        #region Public variables

        public List<Profile> _lstProfileHistory = new List<Profile>();


        public List<Surveys> _lstSurveys = new List<Surveys>();


        public List<RewardsHistory> _lstTotalRewards = new List<RewardsHistory>();

        public List<Rewards> _lstRewardHistory = new List<Rewards>();

        public List<Rewards> _lstRewardCatalog = new List<Rewards>();
        public List<SurveyRedeemptionHistroy> _lstSurveyHistory = new List<SurveyRedeemptionHistroy>();

        public List<SurveyRedeemptionHistroy> _lstRedeemptionHistory = new List<SurveyRedeemptionHistroy>();


        public string _accessToken = string.Empty;
        public int _redeemAmount = 0;

        public decimal _partnerRevShare = 0;


        #endregion

        #region public variables

        /// <summary>
        /// 
        /// </summary>
        public decimal PartnerRevShare { get; set; }
        public List<Profile> LstProfileHistory { get; set; }
        public List<Surveys> LstSurveys { get; set; }
        public List<RewardsHistory> LstTotalRewards { get; set; }
        public List<Rewards> LstRewardHistory { get; set; }
        public List<Rewards> LstRewardCatalog { get; set; }
        public string AccessToken { get; set; }
        public int RedeemAmount { get; set; }
        public List<SurveyRedeemptionHistroy> LstSurveyHistory { get; set; }
        public List<SurveyRedeemptionHistroy> LstRedeemptionHistory { get; set; }
        #endregion
    }

    public class RewardsHistory
    {
        #region Public variables

       public string _totalEarnings = "0.00";

        public string _totalAccountBalance = "0.00";

        public string _totalRedemptions = "0.00";
        public string _totalPendignBalance = "0.00";


        #endregion

        #region public variables

        public string TotalEarnings { get; set; }
        public string TotalAccountBalance { get; set; }
        public string TotalRedemptions { get; set; }
        public string TotalPendignBalance { get; set; }

        #endregion
    }


    public class SurveyRedeemptionHistroy
    {
        #region public variables
        public string _createDt = string.Empty;
        public string _rewardAmount = string.Empty;
        public string _descripion = string.Empty;
        public string _redemptionType = string.Empty;
        public string _status = string.Empty;
        public int _catalogue_id = 0;
        public string _rewardName = string.Empty;
        private string _rewardDescription = string.Empty;
        private string _rewardLogo = string.Empty;
        private string _codeName = string.Empty;
        private string _redeemedAmount = string.Empty;
        private int _sortOrder = 0;
        private int _isDisplay = 0;
        private int _projectId = 0;


        #endregion

        #region public variables
        public string CreateDt
        {
            get { return _createDt; }
            set { _createDt = value; }
        }
        public string RewardAmount
        {
            get { return _rewardAmount; }
            set { _rewardAmount = value; }
        }
        public string Descripion
        {
            get { return _descripion; }
            set { _descripion = value; }
        }
        public string RedemptionType
        {
            get { return _redemptionType; }
            set { _redemptionType = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int Catalogue_id
        {
            get { return _catalogue_id; }
            set { _catalogue_id = value; }
        }
        public string RewardName
        {
            get { return _rewardName; }
            set { _rewardName = value; }
        }

        public string RewardDescription
        {
            get { return _rewardDescription; }
            set { _rewardDescription = value; }
        }

        public string RewardLogo
        {
            get { return _rewardLogo; }
            set { _rewardLogo = value; }
        }
        public string CodeName
        {
            get { return _codeName; }
            set { _codeName = value; }
        }
        public string RedeemedAmount
        {
            get { return _redeemedAmount; }
            set { _redeemedAmount = value; }
        }
        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }
        public int IsDisplay
        {
            get { return _isDisplay; }
            set { _isDisplay = value; }
        }

        public int ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }
        #endregion
    }
}
