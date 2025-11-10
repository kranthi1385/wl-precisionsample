using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class RelevantCheck
    {
        public int RelevantScore { get; set; }
        public int ZipRadius { get; set; }
        public string URL { get; set; }
        public bool IsGDPRCompliance { get; set; }
    }
}
