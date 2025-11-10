using Members.OpinionBar.Components.Data_Layer;
using Members.OpinionBar.Components.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Business_Layer
{
    public class ProfileQuestionBusinessService
    {
        ProfileQuestionDataLayer objDataServer = new ProfileQuestionDataLayer();

        #region Get Top1 pre prescreener questions
        /// <summary>
        /// Get Top1 pre prescreener questions
        /// </summary>
        /// <param name="uig"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public List<Question> GetQuestion(Guid Uig, Guid Ug, int ProjectId, int TargetId, int UserId, int cid)
        {
            return objDataServer.GetQuestion(Uig, Ug, ProjectId, TargetId, UserId, cid);
        }
        #endregion

        #region Save Answers And Return Question
        /// <summary>
        /// Save Answers And Return Question
        /// </summary>
        /// <param name="Uig"></param>
        /// <param name="qid"></param>
        /// <param name="otext"></param>
        /// <param name="optId"></param>
        /// <param name="Ug"></param>
        /// <returns></returns>
        public List<Question> SaveResponse(Guid Uig, int qid, string otext, int optId, Guid Ug, int clientid)
        {
            return objDataServer.SaveResponse(Uig, qid, otext, optId, Ug, clientid);
        }
        #endregion

        //#region Get User Data
        ///// <summary>
        /////  Get User Data
        ///// </summary>
        ///// <param name="Ug"></param>
        ///// <returns></returns>
        //public User GetUserData(Guid Ug)
        //{
        //    return objDataServer.GetUserData(Ug);
        //}
        //#endregion

        # region Save Verity Repsonse
        /// <summary>
        /// Save Verity Repsonse
        /// </summary>
        /// <param name="Ug"></param>
        /// <param name="Uig"></param>
        /// <param name="verityScore"></param>
        /// <param name="verityId"></param>
        /// <param name="geoCorrelationFlag"></param>
        public void SaveVerityRepsonse(Guid Ug, Guid Uig, int verityScore, string verityId, int geoCorrelationFlag, int clientId)
        {
            objDataServer.SaveVerityRepsonse(Ug, Uig, verityScore, verityId, geoCorrelationFlag, clientId);
        }
        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid, int ClientId)
        {
            return objDataServer.GetquestionsforTop10(UserGuid, ClientId);
        }
        #endregion

        #region Top10SaveOptions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listXml"></param>
        public string Top10SaveOptions(string listXml, Guid UserGuid, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                       int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                       string RobotFlag, string ErrorMessage, int ClientId)
        {
            return objDataServer.Top10SaveOptions(listXml, UserGuid, ResponseText, Rq1, Rq2, Rq3, Rq4, RealAnswerScore, BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag,
                                       RobotFlag, ErrorMessage, ClientId);
        }
        #endregion

        #region Get Profile questions
        /// <summary>
        /// Get Profile questions
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public List<ProfileQuestions> GetProfileQuestions(Guid ProfileId, Guid UserGuid, int Clientid, int? SelectedCountryId)
        {
            return objDataServer.GetProfileQuestions(ProfileId, UserGuid, Clientid, SelectedCountryId);
        }

        #endregion

        #region Save Profile Options
        /// <summary>
        /// Save Profile Options
        /// </summary>
        /// <param name="listXml">Profile Data Xml</param>
        /// <param name="UserGuid">user Guid</param>
        public void ProfileSave(String listXml, Guid UserGuid, int ClientId)
        {
            objDataServer.ProfileSave(listXml, UserGuid, ClientId);
        }

        #endregion

        #region Get Profile Responses
        /// <summary>
        /// Get Profile Responses For FusionCash and Vindale
        /// </summary>
        /// <param name="UserGuid">UsetGuid</param>
        /// <param name="ClientId">ClientId</param>
        /// <param name="PfId">ProfileId</param>
        /// <returns></returns>
        public string GetProfileResponse(Guid UserGuid, int ClientId, string PfId)
        {
            return objDataServer.GetProfileResponse(UserGuid, ClientId, PfId);
        }
        #endregion

        #region  Get Profile PixelDetails
        /// <summary>
        /// Get Profile Responses For FusionCash and Vindale
        /// </summary>
        /// <param name="UserGuid">UsetGuid</param>
        /// <param name="ClientId">ClientId</param>
        /// <param name="PfId">ProfileId</param>
        /// <returns></returns>
        public UserEntity GetProfilePixelDetails(Guid UserGuid, int ClientId, string ProfileId)
        {
            return objDataServer.GetProfilePixelDetails(UserGuid, ClientId, ProfileId);
        }
        #endregion
    }
}
