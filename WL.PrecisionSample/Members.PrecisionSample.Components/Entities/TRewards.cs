using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class TRewards
    {
        #region public Varibales
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string RewardName { get; set; }
        public string Disclaimer { get; set; }
        public Guid CatalougeGuid { get; set; }
        public Guid UserGuid { get; set; }

        public Decimal AccountBalance { get; set; }

        public string RedeemAmount { get; set; }
        public List<Reward> Reward = new List<Reward>();

        #endregion

    }
    public class Reward
    {
        #region private Varibales

        public string Sku { get; set; }
        public string CurrencyType { get; set; }
        public int UnitPrice { get; set; }
        public string Available { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal Denomination { get; set; }
        public string Locale { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public decimal AccountBalance { get; set; }
        public string RewardName { get; set; }
        public string ImageURL { get; set; }
        public bool IsDisable = true;
        public string IsVariable { get; set; }
        public List<Range> LstRange = new List<Range>();


        #endregion
    }
    public class Range
    {
        public int RewardValue = 0;
        public bool IsDisable = false;

    }
}
