using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class River
    {

        public Int64 UserInvitationId { get; set; }

        public int UserId { get; set; }

        public Guid UserInvitationGuid { get; set; }

        public int ProjectId { get; set; }

        public int TargetId { get; set; }

        public int OrgId { get; set; }

        public string IpAddress { get; set; }

        public string RedirectUrl { get; set; }

        public DateTime ActivityDate { get; set; }

        public int ActivityTypeId { get; set; }

        public int InternalMember { get; set; }

    }
}
