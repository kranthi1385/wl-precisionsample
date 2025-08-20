using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PSquestion
    {
        #region public variables
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int QuestionTypeId { get; set; }
        public int ParentQuestionId { get; set; }
        public int OptionId { get; set; }
        public string QuestionShortName { get; set; }
        public string OptionText { get; set; }
        public List<PSoptions> OptionList { get; set; }
        public List<PSquestion> ChildQuestionList { get; set; }
        public List<PSquestion> SelectedChildQuestionList { get; set; }
        public string OptionDisplay { get; set; }
        public int UserId { get; set; }
        public List<PSquestion> ResponseOptionList = new List<PSquestion>();
        public List<PSoptions> SubChildOptions = new List<PSoptions>();
        public int OptionTypeId { get; set; }

        public List<ProfileOptions> SpecialOptinLst = new List<ProfileOptions>();
        public string RedirectUrl { get; set; }
        public int SessionCount { get; set; }
        public string SuccessMessage { get; set; }
        public bool IsCompleted { get; set; }
        public int MinQuestionsCount { get; set; }
        public int MaxQuestionsCount { get; set; }
        public int CurrentSortOrder { get; set; }
        public bool RandomizedOptions { get; set; }
        public int TermOptionsNeeded { get; set; }
        public int NonTermOptionsNeeded { get; set; }
        #endregion
    }
}
