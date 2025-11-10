using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
   public class Perk
    {
        #region Public Variables

        public string PerkName { get; set; }
        public string PerkUrl { get; set; }
        public string PerkDescription { get; set; }
        public Guid PerkGuid { get; set; }
        public string Status { get; set; }
        public string PerkClickDt { get; set; }
        public string PerkCompletedDt { get; set; }
        public string SurveyCompletedDt { get; set; }
        public string SurveyName { get; set; }
        public string Rowno { get; set; }
        public string Page4ImageLogo { get; set; }
        public int PerkId { get; set; }
        public Guid User2PerkGuid { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyUrl { get; set; }
        public Decimal RewardValue { get; set; }
        public string Type { get; set; }
        public bool IsPixelTracked { get; set; }
        public bool IsMaskUrl { get; set; }
        public int PerkTypeId { get; set; }
        public int SurveyLength { get; set; }
        public int Projectid { get; set; }
        #endregion
    }
}
