using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Members.PrecisionSample.Components.Business_Layer
{
    public class PartnerManager
    {

        PartnerDataService oPartnerData = new PartnerDataService();

        #region GetSurveyStatus
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserInvitationGuid"></param>
        /// <returns></returns>
        public Partner GetSurveyStatus(Guid UserInvitationGuid, int ClientId)
        {
            return oPartnerData.GetSurveyStatus(UserInvitationGuid, ClientId);
        }
        #endregion

        #region Pixel Fire status Update
        public void APIResponseUpdate(Guid UserInvitationGuid, string response)
        {
            oPartnerData.APIResponseUpdate(UserInvitationGuid, response);
        }

        #endregion

        #region GetQutoaGuid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitaionGuid"></param>
        /// <returns></returns>
        public string GetQutoaGuid(string invitaionGuid)
        {
            return oPartnerData.GetQutoaGuid(invitaionGuid);
        }
        #endregion
        #region GetOrgid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public string GetOrgId(string userGuid)
        {
            return oPartnerData.GetOrgId(userGuid);
        }
        #endregion
    }
}
