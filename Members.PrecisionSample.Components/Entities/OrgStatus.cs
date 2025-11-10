using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class OrgStatus
    {
        public class data
        {
            public string url { get; set; }
        }

        public class RootObject
        {
            public string message { get; set; }
            public string status { get; set; }
            public data data { get; set; }
        }
    }
}
