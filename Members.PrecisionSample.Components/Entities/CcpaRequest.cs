using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    class CcpaRequest
    {
        public string UserGuid { get; set; }
        public string UserInvitationGuid { get; set; }
        public int ClientId { get; set; }
        public bool IsResonateCookieDropped { get; set; }
        public bool IsTapadCookieDropped { get; set; }
        public bool IsSaveCookie { get; set; }
    }
}
