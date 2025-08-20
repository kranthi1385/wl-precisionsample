using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrecisionSample.Services.Components.Entites;
using PrecisionSample.Services.Components.DataLayer;

namespace PrecisionSample.Services.Components.BussinessLayer
{
    public class MemberBussinesslayer
    {
        MemberDataLayer memeberobj = new MemberDataLayer();
        public string Create(Member member)
        {
            return memeberobj.Create(member);
        }

        public string CreateWL(User oUser)
        {
            return memeberobj.CreateWL(oUser);
        }

        public string CreateWidget(MemberEntity oUser)
        {
            return memeberobj.CreateWidget(oUser);
        }

        public string Update(UMember member)
        {
            return memeberobj.Update(member);
        }

        public string UpdateWL(User oUser)
        {
            return memeberobj.UpdateWL(oUser);
        }
        public string Unsubscribe(int Rid, string UserName)
        {
            return memeberobj.Unsubscribe(Rid, UserName);

        }

        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public Rewards GetRewardsHistory(int userId, int clientId, Guid userGuid)
        {
            return memeberobj.GetRewardsHistory(userId, clientId, userGuid);
        }
        public Rewards GetRedeemHistory(int userId, int clientId, Guid userGuid)
        {
            return memeberobj.GetRedeemHistory(userId, clientId, userGuid);
        }
        public string Reubscribe(Guid memberGuid, int ClientId)
        {
            return memeberobj.Resubscribe(memberGuid, ClientId);
        }
        public List<Profile> GetPartnerUserProfilesList(string memberGuid, int ClientId)
        {
            return memeberobj.GetPartnerUserProfilesList(memberGuid, ClientId);

        }
        public List<Surveys> GetSurveys(string memberGuid, int ClientId)
        {
            return memeberobj.GetSurveys(memberGuid, ClientId);
        }

        #region Get Surveys for API Partners.

        public List<ApiSurveys> GetSurveysforAPIPartnersOnly(string memeberGuid, int? ClientId)
        {
            return memeberobj.GetSurveysforAPIPartnersOnly(memeberGuid, ClientId);
        }

        #endregion

        public List<SurveyHistory> GetSurveyHistory(string memberGuid, int ClientId)
        {
            return memeberobj.GetSurveyHistory(memberGuid, ClientId);
        }
        public string UpdateProfile(string XmlResponse, string ug, int rid)
        {
            return memeberobj.UpdateProfile(XmlResponse, ug, rid);
        }

        public User UserLogin(User oUser)
        {
            return memeberobj.UserLogin(oUser);
        }
        public string IsProjectOpen(int ProjectId)
        {
            return memeberobj.IsProjectOpen(ProjectId);
        }
        public string GetProjectsClosedToday()
        {
            return memeberobj.GetProjectsClosedToday();
        }
        public string GetMembers(int ProjectId, Guid ApiKey)
        {
            return memeberobj.GetMembers(ProjectId, ApiKey);
        }

        public string UpdateProfile2(string json, Guid ug, int rid)
        {
            return memeberobj.UpdateProfile2(json, ug, rid);
        }
        public string Delete(int Rid, string ExtMemberId, Guid UserGuid)
        {
            return memeberobj.Delete(Rid, ExtMemberId, UserGuid);
        }
        public List<Questions> GetQuestions(int QuestionId)
        {
            return memeberobj.GetQuestions(QuestionId);
        }
    }
}
