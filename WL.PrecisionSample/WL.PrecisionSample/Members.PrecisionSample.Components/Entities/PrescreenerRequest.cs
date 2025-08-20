using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class PrescreenerRequest
    {
        public string UserInvitationGuid { get; set; }
        public string UserGuid { get; set; }
        public int LanguageId { get; set; }
        public int CurrentQuestionId { get; set; }
        public int ClientId { get; set; }
        public int CurrentQuestionSortOrder { get; set; }
        public string XML { get; set; }
        public int GDPR { get; set; }
        public string CurrentExecution { get; set; } = string.Empty;
        public bool IsSavePrescreenerResponse { get; set; }
    }
}
