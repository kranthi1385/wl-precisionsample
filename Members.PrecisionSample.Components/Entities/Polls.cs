using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Polls
    {
        public  Int32 QuestionId { get; set; }
        public Int32 OptionId { get; set; }
        public string QuestionText { get; set; }
        public string OptionText { get; set; }
        public Int32 Mode { get; set; }
        public Int32 PollCount { get; set; }
        public Int32 TotPollCount { get; set; }
        public string Percentage { get; set; }
    }
}
