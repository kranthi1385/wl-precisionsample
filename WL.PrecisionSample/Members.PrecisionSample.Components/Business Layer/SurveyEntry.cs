using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
namespace Members.PrecisionSample.Components.Business_Layer
{
    public class SurveyEntry
    {
        SurveyEntryDataLayer oServer = new SurveyEntryDataLayer();
        public SEntry saveEntry(string uig, int pid, Decimal cost, int FedProjectId)
        {
            return oServer.saveEntry(uig, pid, cost, FedProjectId);
        }


        public string GetFedEntryUrl(string mid, Guid? eid, int? project, decimal? ecost)
        {

            return oServer.GetFedEntryUrl(mid, eid, project, Convert.ToDecimal(ecost));
        }

        public string GetRedirectUrl(Guid ug, int project, Guid key, string subid, string IpAddress)
        {
            return oServer.GetRedirectUrl(ug, project, key, subid, IpAddress);
        }

        public List<Question> GetQuestion(Guid UserGuid, int ProjectId)
        {
            return oServer.GetQuestion(UserGuid, ProjectId);
        }

        public List<Question> SaveResponse(int qid, string otext, int optId, Guid Ug, int ClientId, string zip, int ProjectId)
        {
            return oServer.SaveResponse(qid, otext, optId, Ug, ClientId, zip, ProjectId);
        }
    }
}
