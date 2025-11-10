using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PriRequest
    {
        public bool NeedOrgLogo { get; set; } = false;
        public int ClientId { get; set; }
        public string UserInvitationGuid { get; set; }
        public string ClientIP { get; set; }
        public string UserGuid { get; set; } = null;
        public int UserId { get; set; }
        public string HostUrl { get; set; } = null;
        public long UserInvitationId { get; set; }
        public string DeviceType { get; set; }
    }
}
