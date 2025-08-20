using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    class PrescreenerProfileRequest

    {

        public string UserGuid { get; set; }

        public int UserId { get; set; }

        public string UserInvitationGuid { get; set; }

        public int ClientId { get; set; }

        public bool IsSaveResponse { get; set; }

        public string XML { get; set; }

    }

}
