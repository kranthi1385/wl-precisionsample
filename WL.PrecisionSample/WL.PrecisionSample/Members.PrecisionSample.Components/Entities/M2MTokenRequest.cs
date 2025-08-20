using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class M2MTokenRequest
    {
        public string grant_type { get; set; } = string.Empty;
        public string client_id { get; set; } = string.Empty;
        public string client_secret { get; set; } = string.Empty;
        public string audience { get; set; } = string.Empty;
    }
}
