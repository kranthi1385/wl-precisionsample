using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class ProfileOptions
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public int QuestionId { get; set; }
        public string QuesitonText { get; set; }
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public bool IsChecked { get; set; }
        public int ParentQuestionId { get; set; }
        public int ParentOptionId { get; set; }
        public int SpeicalOptionId { get; set; }

        public int SpecialGroupingId { get; set; }

        public List<int> ListChildQuestionId = new List<int>();
        #endregion
    }
}
