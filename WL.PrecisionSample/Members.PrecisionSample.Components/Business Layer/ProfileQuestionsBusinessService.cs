using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ProfileQuestionsBusinessService
    {
        ProfileQuestionsDataService objQuestionDl = new ProfileQuestionsDataService();

        #region Getquestions
        /// <summary>
        /// get questions list
        /// </summary>
        /// <param name="ProfileId">ProfileId</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> Getquestions(Guid ProfileId, Guid UserGuid)
        {
            return objQuestionDl.Getquestions(ProfileId, UserGuid);
        }
        #endregion

        #region Getquestions
        /// <summary>
        /// get questions for WLables
        /// </summary>
        /// <param name="ProfileId">ProfileId</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsForWLables(Guid ProfileId, Guid UserGuid, string MemberLanguage)
        {
            return objQuestionDl.GetquestionsForWLables(ProfileId, UserGuid, MemberLanguage);
        }
        #endregion

        #region GetPendingQuestions
        /// <summary>
        /// get pending questions list
        /// </summary>
        /// <param name="ProfileId">ProfileId</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetPendingQuestions(Guid ProfileId, Guid UserGuid)
        {
            return objQuestionDl.GetPendingQuestions(ProfileId, UserGuid);
        }
        #endregion

        #region SaveOptions
        /// <summary>
        /// save options
        /// </summary>
        /// <param name="listXml">listXml</param>
        public void SaveOptions(String listXml)
        {
            objQuestionDl.SaveOptions(listXml);
        }

        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// get questions for top10
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10(Guid UserGuid, Guid UserInvitationGuid)
        {
            return objQuestionDl.GetquestionsforTop10(UserGuid, UserInvitationGuid);
        }
        #endregion

        #region GetquestionsforTop10
        /// <summary>
        /// get questions for top10 for WLables
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public List<ProfileQuestions> GetquestionsforTop10ForWLables(Guid UserGuid, Guid UserInvitationGuid, string MemberLanguage)
        {
            return objQuestionDl.GetquestionsforTop10ForWLables(UserGuid, UserInvitationGuid, MemberLanguage);
        }
        #endregion

        #region Top10SaveOptions
        /// <summary>
        /// top10 save options
        /// </summary>
        /// <param name="listXml">listXml</param>
        public int Top10SaveOptions(string listXml, Guid UserInvitationGuid, Guid UserGuid, string ResponseText, string Rq1, string Rq2, string Rq3, string Rq4,
                                       int RealAnswerScore, string BadWordsFlag, string BadPhraseFlag, string GarbageWordsFlag, string NonEngagedFlag, string PastedTextFlag,
                                       string RobotFlag, string ErrorMessage)
        {
            return objQuestionDl.Top10SaveOptions(listXml, UserInvitationGuid, UserGuid, ResponseText, Rq1, Rq2, Rq3, Rq4, RealAnswerScore, BadWordsFlag, BadPhraseFlag, GarbageWordsFlag, NonEngagedFlag, PastedTextFlag,
                                       RobotFlag, ErrorMessage);
        }
        #endregion

        #region Top10SaveOptions and Get Key:

        /// <summary>
        /// top10 save options and get key
        /// </summary>
        /// <param name="listXml">listXml</param>
        /// <param name="UserInvitationGuid">UserInvitationGuid</param>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        public string Top10SaveOptionsAndGetKey(string listXml, Guid UserInvitationGuid, Guid UserGuid)
        {
            return objQuestionDl.Top10SaveOptionsAndGetKey(listXml, UserInvitationGuid, UserGuid);
        }
        #endregion

        #region Fetch Top 10 Pending Profile Questions

        /// <summary>
        /// get top10 pending profiles questions
        /// </summary>
        /// <param name="profileId">profileId</param>
        /// <returns></returns>
        public List<Question> GetTop10PendingProfileQuestions(Guid userguid, Guid user2projectguid)
        {
            ProfileQuestionsDataService oServer = new ProfileQuestionsDataService();
            return oServer.GetTop10PendingProfileQuestions(userguid, user2projectguid);
        }
        #endregion

        #region Top 10 Pending Profile Questions Insert

        /// <summary>
        /// get top10 pending profile questions insert
        /// </summary>
        /// <param name="xml">xml</param>
        public void Top10PendingProfileQuestionsInsert(string xml, Guid user2projectguid)
        {
            ProfileQuestionsDataService oServer = new ProfileQuestionsDataService();
            oServer.Top10PendingProfileQuestionsInsert(xml, user2projectguid);
        }
        #endregion

        #region Save Challange Question Response
        public int SaveChallangeQuestionResponse(string ChallangeScore, string ChallangeId, Guid UserGuid, Guid SurveyInivitationGuid, string Option1, string Option2, string Option3, string ErrorMsg)
        {
            return objQuestionDl.SaveChallangeQuestionResponse(ChallangeScore, ChallangeId, UserGuid, SurveyInivitationGuid, Option1, Option2, Option3, ErrorMsg);
        }
        #endregion

        #region Get Verity ChallengeQuestions
        public List<VerityEnhancedQuestions> GetVerityChallengeQuestions(Guid UserGuid, Guid UserInvitationGuid)
        {
            return objQuestionDl.GetVerityChallengeQuestions(UserGuid, UserInvitationGuid);
        }
        #endregion


        #region Insert top 10page member skip page click log
        public int InsertTop10PageSkipLog(Guid UserGuid, Guid UserInvitationGuid)
        {
            return objQuestionDl.InsertTop10PageSkipLog(UserGuid, UserInvitationGuid);
        }
        #endregion
    }
}
