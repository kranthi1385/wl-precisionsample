using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class LeadManager
    {
        LeadDataServer odataserver = new LeadDataServer();
        #region Get Prescreener Question List By ProjectId
        /// <summary>
        /// Get User Prescree Questions
        /// </summary>
        /// <param name="UserGuid">User Guid</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <returns></returns>
        public List<PSquestion> GetPSQuestions(Guid LeadGuid)
        {
            return odataserver.GetPSQuestions(LeadGuid);
        }
        #endregion

        #region SaveUserPrescreenerptions
        /// <summary>
        /// Save Member prescreener options
        /// </summary>
        /// <param name="Xml">User Response Xml </param>
        /// <param name="UserGuid">UserGuid</param>
        /// <param name="UserInvitationGuid">UserInvationGuid</param>
        /// <param name="QuestionId">QuestionId</param>
        /// <param name="SortOrder">SortOrder</param>
        /// <param name="LanguageId">LanguageId</param>
        /// <returns></returns>
        public List<PSquestion> SaveUserPrescreenerOptions(string Xml, Guid LeadGuid, int QuestionId, int SortOrder)
        {
            return odataserver.SaveUserPrescreenerOptions(Xml, LeadGuid, QuestionId, SortOrder);
        }
        #endregion

        #region Get Step2 Details
        /// <summary>
        /// Get Step2 Details
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public User GetStep2Details(Guid lid)
        {
            User ouser = new User();
            ouser = odataserver.GetStep2Details(lid);
            return ouser;
        }
        #endregion
    }
}
