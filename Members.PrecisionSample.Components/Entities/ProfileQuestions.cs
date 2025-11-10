using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ProfileQuestions
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<ProfileOptions> OptionList = new List<ProfileOptions>();
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int QuestionTypeId { get; set; }
        public List<ProfileQuestions> ChildQuestionList = new List<ProfileQuestions>();
        public List<ProfileQuestions> SelectedChildQuestionList = new List<ProfileQuestions>();
        public int ParentQuestionId { get; set; }
        public string OptionDisplay { get; set; }
        public int UserId { get; set; }
        public int AutoPostBack { get; set; }
        public List<ProfileQuestions> ResponseOptionList = new List<ProfileQuestions>();
        public List<ProfileOptions> SubChildOptions = new List<ProfileOptions>();
        //public List<ProfileOptions> _specialOptinLst = new List<ProfileOptions>();
        public bool QuestionHide { get; set; }
        public string OrgInfo { get; set; }
        public Guid UserInvitationGuid { get; set; }
        public bool IsFraud { get; set; }
        public bool IsShowCCPA { get; set; }
        public int LanguageID { get; set; }
        public int CountryID { get; set; }
        #endregion

    }
}
