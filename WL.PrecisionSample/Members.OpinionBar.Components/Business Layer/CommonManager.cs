using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Data_Layer;
namespace Members.OpinionBar.Components.Business_Layer
{
    public class CommonManager
    {
        CommonDataServer objDataServer = new CommonDataServer();
        #region public Information
        CommonDataServer ODataServer = new CommonDataServer();
        #endregion
        #region Get Country & States List

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public CountryAndStates GetCountrysAndStates()
        {

            return objDataServer.GetCountrysAndStates();
        }
        #endregion

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

        #region Get All Avaliable Ethnicites
        /// <summary>
        /// Get All Ethnicites
        /// </summary>
        /// <param name="LanguageCode">LanguageCode</param>
        /// <returns></returns>
        public List<Language> GetObLang()
        {
            return objDataServer.GetObLang();
        }
        #endregion

        #region Get Continents and Countries List
        /// <summary>
        /// Get Continents and Countries List
        /// </summary>
        /// <returns></returns>
        public List<Continent> GetContientList()
        {
            return ODataServer.GetContientList();
        }
        #endregion

        #region Update Language Code
        /// <summary>
        ///  Update Language Code
        /// </summary>
        /// <param name="LangCode">Language Code</param>
        public string UpdateLanguageCode(User oUser, string LangCode, string RequestUrl)
        {
           return objDataServer.UpdateLanguageCode(oUser, LangCode, RequestUrl);
        }
        #endregion
    }
}
