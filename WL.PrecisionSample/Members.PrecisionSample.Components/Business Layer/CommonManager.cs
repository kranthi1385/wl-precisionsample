using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
namespace Members.PrecisionSample.Components.Business_Layer
{
    public class CommonManager
    {
        CommonDataServer objDataServer = new CommonDataServer();

        #region Get All Avaliable Ethnicites
        /// <summary>
        /// Get All Ethnicites
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Ethnicity> GetEthinicity(string LanguageCode)
        {
            return objDataServer.GetEthinicity(LanguageCode);
        }
        #endregion

        #region Get Language List
        public List<Options> GetLanguageList()
        {
            return objDataServer.GetLanguageList();
        }
        #endregion

        #region Get All Avaliable States
        /// <summary>
        /// Get Country wise states
        /// </summary>
        /// <param name="CountryId">CountryId</param>
        /// <returns></returns>
        public List<States> GetStatesList(int CountryId, string LanguageCode)
        {
            return objDataServer.GetStatesList(CountryId, LanguageCode);
        }
        #endregion
        #region Get landing page urls
        /// <summary>
        /// Get landing page urls
        /// </summary>
        /// <param name="CountryId">CountryId</param>
        /// <returns></returns>
        public String GetLandingpageUrl(int referrerId)
        {
            return objDataServer.GetLandingpageUrl(referrerId);
        }
        #endregion

        #region Get Country & States List

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public CountryAndState GetCountrysAndStates(int ClientId)
        {
            return objDataServer.GetCountrysAndStates(ClientId);
        }
        #endregion

        #region Update Language Code
        /// <summary>
        ///  Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        public void UpdateLanguageCode(User oUser, string LangCode,string RequestUrl)
        {
            objDataServer.UpdateLanguageCode(oUser, LangCode, RequestUrl);
        }
        #endregion

        #region Get Languages
        /// <summary>
        /// Get Languages
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Language> GetObLang()
        {
            return objDataServer.GetObLang();
        }
        #endregion
    }
}
