using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class VerityEnhancedQuestions
    {
        #region public variables
        public Guid UserInvitationGuid { get; set; }
        public string QuestionText { get; set; }
        public string ChallengeId { get; set; }

        public string OptionText { get; set; }
        public string AnswerText { get; set; }
        public List<option> OptionList = new List<option>();
        public string SelectedAnsId { get; set; }
        public string RedirectUrl { get; set; }

        #endregion
    }
    public class option
    {
        #region public variables
        public string OptionText { get; set; }
        #endregion
    }
}
