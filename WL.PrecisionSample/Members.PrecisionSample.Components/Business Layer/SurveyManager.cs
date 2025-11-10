using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class SurveyManager
    {
        SurveyDataServer oSurveyDataServices = new SurveyDataServer();

        #region Get User Perks

        /// <summary>
        /// get Surveys list
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="password">password</param>
        public List<Perks> GetSurveysList(string userId)
        {

            return oSurveyDataServices.GetSurveysList(userId);
        }

        #endregion

        #region Get Projects Details
        /// <summary>
        /// Get Projects Details
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        public List<Surveys> GetAllAvaliableSurveys(int UserId)
        {
            return oSurveyDataServices.GetAllAvaliableSurveys(UserId);
        }

        public Guid GetInvitationGUIDbyId(string uig)
        {
            return oSurveyDataServices.GetInvitationGUIDbyId(uig);
        }
        #endregion

    }
}
