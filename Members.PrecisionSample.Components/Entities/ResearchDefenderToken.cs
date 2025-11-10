using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ResearchDefenderToken
    {
        public class Result
        {
            public string message { get; set; }
            public string token { get; set; }
            public string timestamp { get; set; }
        }
        public class Root
        {
            public List<Result> results { get; set; }
        }
        public class Reviews
        {
            public string q_id { get; set; }
            public string text { get; set; }
            public string s_text_length { get; set; }
        }
    }
}
