using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class InterstitialResponse
    {
        public string RedirectURL { get; set; } = string.Empty;
        public Guid UserInvitationGuid { get; set; } = Guid.Empty;
        public Guid UserGuid { get; set; } = Guid.Empty;
        public string FirstClick { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string UserLanguage { get; set; } = string.Empty;
    }
}
