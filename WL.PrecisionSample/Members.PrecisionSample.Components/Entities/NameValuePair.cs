using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class NameValuePair
    {
        public int LandingId;
        public string LandingName;

        public int Key { get; set; }
        public string Value { get; set; }
        public string CommonKey { get; set; }
        public bool IsSelected = true;
    }
}
