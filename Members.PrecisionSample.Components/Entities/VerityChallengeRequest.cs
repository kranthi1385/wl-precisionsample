using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class VerityChallengeRequest
    {
        public int UserId { get; set; }
        public bool IsSaveResponse { get; set; }
        public string UserInvitationGuid { get; set; }
        public int ClientId { get; set; }
        public string ChallengeId { get; set; }
        public string SelectedAnswer1 { get; set; }
        public string SelectedAnswer2 { get; set; }
        public string SelectedAnswer3 { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public int ProjectId { get; set; }
        public int TargetId { get; set; }
        public string UserGuid { get; set; }
        public string IpqsJSON { get; set; }
        public string DeviceType { get; set; }
        public string ClientIP { get; set; }
    }
    public class VerityChallengeResponse
    {
        public string UserInvitationGuid { get; set; }
        public string ChallengeId { get; set; }
        public string QuestionText { get; set; }
        public string OptionText { get; set; }
        public string RedirectUrl { get; set; }
    }
}
