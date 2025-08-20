using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PrescreenerLanguageRequest
    {
        public string UserGuid { get; set; }
        public string UserInvitationGuid { get; set; }
        public int ClientId { get; set; }
    }
}
