using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;

namespace Members.OpinionBar.Components.Business_Layer
{
   public class ExternalMembersManager
    {
        ExternalMemberDataLayer oDataLayer = new ExternalMemberDataLayer();
        public Surveys SaveClickInformation(string QgId, string mid, int pid, string Rid, string Source, string SubId, int IsNew, int UserTrafficTypeId, string MobiledeviceModel, string BrowserInfo,
                            string AgentInfo, string IpAddress, string RelevantId, int RelevantScore, string FpfScores, int FraudProfilefScore, string OldSurveyInvitationId, string fed_response_id, decimal ecost, string e_rm, string e_rl)
        {
            return oDataLayer.SaveClickInformation(QgId, mid, pid, Rid, Source, SubId, IsNew, UserTrafficTypeId, MobiledeviceModel, BrowserInfo, AgentInfo, IpAddress, RelevantId,
                              RelevantScore, FpfScores, FraudProfilefScore, OldSurveyInvitationId, fed_response_id, ecost, e_rm, e_rl);
        }
    }
}
