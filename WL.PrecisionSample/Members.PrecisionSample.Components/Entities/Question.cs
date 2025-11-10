using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Question
    {
        public List<Options> Options = new List<Options>();
        public List<Options> SelectedOptions = new List<Entities.Options>();

        public string QuestionText { get; set; }

        public int QuestionId { get; set; }

        public int QuestionTypeId { get; set; }

        public int OptionId { get; set; }

        public string OptionText { get; set; }

        public bool HasOptions { get; set; }

        public string AnswerText { get; set; }

        public string LastName { get; set; }

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }
        public int VerityScore { get; set; }
        public bool VerityRequired { get; set; }
        public Guid UserInvitationGuid { get; set; }
        public bool StateQuestion { get; set; }
        public bool CityQuestion { get; set; }
        public bool ZIPQuestion { get; set; }
        public string ProjectURL { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
    }
}

