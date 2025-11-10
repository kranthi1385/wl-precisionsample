using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PSoptions
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public bool IsChecked { get; set; }
        public int ParentOptionId { get; set; }
        public List<int> ListChildQuestionId { get; set; }
        public int OptionTypeId { get; set; }
        public int SpeicalOptionId { get; set; }
        public int SpecialGroupingId { get; set; }
        public int SortOrder { get; set; }
        public int IsTermOption { get; set; }
        #endregion
    }
}
