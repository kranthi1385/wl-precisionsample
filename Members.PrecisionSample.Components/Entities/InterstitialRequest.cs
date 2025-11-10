using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class InterstitialRequest
    {
        public Guid UserGuid { get; set; } = Guid.Empty;
        public Guid UserInvitationGuid { get; set; } = Guid.Empty;
        public string ShowRecaptcha { get; set; }
        public int ClientId { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string FirstClick { get; set; }
        public int ProjectId { get; set; }
        public string SentryStatus { get; set; } = string.Empty;
        public string RequestUriQuery { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public bool IsSaveRecaptcha { get; set; } = false;
    }
}
