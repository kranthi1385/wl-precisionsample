using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ValidRequest
    {
        public int UserId { get; set; }
        public string UserInvitationId { get; set; }
        public string ClientIP { get; set; }
        public string UserInvitationGuid { get; set; }
        public int ClientId { get; set; }
        public string UserGuid { get; set; }
        public string IpqsJSON { get; set; }
        public string DeviceType { get; set; }
        public string SessionId { get; set; }
    }
}
