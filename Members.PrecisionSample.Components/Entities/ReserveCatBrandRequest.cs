using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ReserveCatBrandRequest
    {
        public string respondentId { get; set; } = string.Empty;
        public string respondentLocale { get; set; } = string.Empty;
        public string bucketId { get; set; } = string.Empty;
    }
}
