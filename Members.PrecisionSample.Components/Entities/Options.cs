using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Options
    {
        public string Empty { get; set; }

        public int QuestionId { get; set; }


        public int OptionId { get; set; }

        public string OptionText { get; set; }

        public bool IschildHide { get; set; }

        public string LanguageHeader { get; set; }

        /// Pending Question Count
        /// </summary>
        public int PendingQuestionCount { get; set; }

        public string LogoUrl { get; set; }

        public int PrjAPIFlag { get; set; }

        public string UserInfo { get; set; }

        public string Source { get; set; }
        public int CountryID { get; set; }
        public string ZIPCode { get; set; }
        public bool IsIpsosProject { get; set; }
    }
}
