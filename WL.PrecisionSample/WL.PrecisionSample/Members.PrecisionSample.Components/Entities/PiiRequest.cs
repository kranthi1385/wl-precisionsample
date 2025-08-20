using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PiiRequest
    {
        public bool IsFirstQuestion { get; set; } = true;
        public int UserId { get; set; }
        public long UserInvitationId { get; set; }
        public string UserInvitationGuid { get; set; }
        public string ClientIP { get; set; }
        public bool IsSaveResponse { get; set; }
        public string UserGuid { get; set; }
        public int ProjectId { get; set; }
        public int TargetId { get; set; }
        public int ClientId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string DeviceType { get; set; }
    }
    public class PiiResponse
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
