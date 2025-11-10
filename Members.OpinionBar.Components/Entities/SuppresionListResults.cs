using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Result
    {
        public string recipient { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public string description { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public int subaccount_id { get; set; }
        public bool transactional { get; set; }
        public bool? non_transactional { get; set; }
    }

    public class SuppresionListResults
    {
        public List<Result> results { get; set; }
        public List<object> links { get; set; }
        public int total_count { get; set; }
    }


}
