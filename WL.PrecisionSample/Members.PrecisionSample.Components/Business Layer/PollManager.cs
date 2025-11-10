using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class PollManager
    {
        #region CurrentPoll

        /// <summary>
        /// get current poll 
        /// </summary>
        /// <returns></returns>
        public List<Polls> GetCurrentPoll(int userid)
        {
            PollDataServer oPollDataServer = new PollDataServer();
            return oPollDataServer.GetCurrentPoll(userid);
        }

        #endregion

        #region InsertResult
        /// <summary>
        /// get insert result
        /// </summary>
        /// <param name="questionId">questionId</param>
        /// <param name="optionId">optionId</param>
        /// <param name="userid">userid</param>
        public void InsertResult(Int32 questionId, Int32 optionId, int userid)
        {
            PollDataServer oPollDataServer = new PollDataServer();
            oPollDataServer.InsertResult(questionId, optionId, userid);
        }
        #endregion

        #region GetResult
        /// <summary>
        /// get result
        /// </summary>
        /// <param name="questionId">questionId</param>
        /// <returns></returns>
        public List<Polls> GetResult(Int32 questionId)
        {
            PollDataServer oPollDataServer = new PollDataServer();
            return oPollDataServer.GetResult(questionId);
        }
        #endregion
    }
}
