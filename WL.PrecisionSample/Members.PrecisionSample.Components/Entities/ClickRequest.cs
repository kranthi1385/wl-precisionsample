using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class ClickRequest
    {
        public int ClientId { get; set; }
        public string UserGuid { get; set; }
        public int ProjectId { get; set; }
        public string TargetGuid { get; set; }
        public string OldSurveyInvitationId { get; set; }
        public string Source { get; set; }
        public string RId { get; set; }
        public string SubId { get; set; }
        public int UserTrafficTypeId { get; set; }
        public string MobiledeviceModel { get; set; }
        public string BrowserInfo { get; set; }
        public string AgentInfo { get; set; }
        public string IpAddress { get; set; }
        public string FedResId { get; set; }
        public string IPNumber { get; set; }
    }
}
