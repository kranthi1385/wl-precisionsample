using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;

namespace Members.OpinionBar.Components.Business_Layer
{
    public class PerksManager
    {
        #region public variables
        PerksDataServices oPerksDataServices = new PerksDataServices();
        #endregion
        #region Get User ShowMe offers
        /// <summary>
        ///  Get User ShowMe offers
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<Surveys> GetShowMeSurveysList(string UserGuid, int ClientId)
        {
            return oPerksDataServices.GetShowMeSurveysList(UserGuid, ClientId);
        }
        #endregion

        #region Get PerkDetails
        /// <summary>
        /// Get PerkDetails
        /// </summary>
        /// <param name="PerkGuid">PerkGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public Perk GetPerkDetails(string PerkGuid, string UserGuid)
        {
            return oPerksDataServices.GetPerkDetails(PerkGuid, UserGuid);
        }
        #endregion

        #region GetPerkCompletedDate1
        /// <summary>
        /// GetPerkCompletedDate1
        /// </summary>
        /// <param name="project_guid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetPerkCompletedDate1(string PerkGuid, int UserId, string UserGuid, int ClientId)
        {
            return oPerksDataServices.GetPerkCompletedDate1(PerkGuid, UserId, UserGuid, ClientId);
        }
        #endregion

        #region InsertClickDate1
        /// <summary>
        /// GetPerkCompletedDate1
        /// </summary>
        /// <param name="project_guid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Perk InsertClickDate1(string PerkGuid, int UserId, string src, string UserGuid, int ClientId)
        {
            return oPerksDataServices.InsertClickDate1(PerkGuid, UserId, UserGuid, src, ClientId);
        }
        #endregion

    }
}
