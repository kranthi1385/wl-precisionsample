using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
   public class Questions
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public string ProfileName { get; set; }
        public string QuestionTypeName { get; set; }
    }
}
