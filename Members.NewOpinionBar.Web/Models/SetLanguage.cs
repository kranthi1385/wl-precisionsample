using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
namespace Members.NewOpinionBar.Web.Models
{
    public class SetLanguage
    {
        #region Check selected Language Existed in Avaliable Countries Lsit
        /// <summary>
        /// Check selected Language Existed in Avaliable Countries Lsit
        /// </summary>
        /// <param name="continentName">Continent Name</param>
        /// <param name="langCode">language Code</param>
        /// <returns></returns>
        public static bool IsLanguageAvailable(string continentName, string langCode)
        {
            CommonManager oMananger = new CommonManager();
            List<Continent> lstContinent = new List<Continent>();
            lstContinent = oMananger.GetContientList();
            List<Country> lstCounties = new List<Country>();
            foreach (Continent objContinent in lstContinent)
            {
                if (continentName.ToLower() == objContinent.ContinentName.ToLower())
                {
                    lstCounties = objContinent.LstCountries;
                }
            }
            if (lstCounties.Count > 0)
            {
                return lstCounties.Where(a => a.LC.Equals(langCode)).FirstOrDefault() != null ? true : false;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Set SeT Defualt Language
        public static string GetDefaultLanguage()
        {
            return "en";
        }
        #endregion

        #region Set Language Cookie
        /// <summary>
        /// Set Language Cookie
        /// </summary>
        /// <param name="continentName">Continent Name</param>
        /// <param name="langCode">language Code</param>
        public void SetLanguageCookie(string continentName, string languageCode)
        {
            try
            {
                if (!IsLanguageAvailable(continentName, languageCode)) languageCode = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(languageCode);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie langCookie = new HttpCookie("culture", languageCode);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch (Exception) { }
        }
        #endregion
    }
}