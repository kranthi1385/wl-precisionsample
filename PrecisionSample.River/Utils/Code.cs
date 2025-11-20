using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.River.Web.River.Utils;
using Members.PrecisionSample.Componente.Entities;

namespace Members.PrecisionSample.River.Web.River.Utils
{
    public class Code
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="objEntity"></param>
        /// <param name="extrainformation"></param>
        /// <returns></returns>


       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="oUser"></param>
        /// <param name="extraInformation"></param>
        /// <returns></returns>
        public static string PEReplaceMents(string url, User oUser, string extraInformation)
        {
            url = url.Replace(Names.PersonalizationElements.DmaCode, oUser.DmaId.ToString());
            url = url.Replace(Names.PersonalizationElements.UserId, oUser.UserId.ToString());
            url = url.Replace(Names.PersonalizationElements.UserGuid, oUser.UserGuid.ToString());
            url = url.Replace(Names.PersonalizationElements.UserPerkGuid, extraInformation);
            url = url.Replace(Names.PersonalizationElements.PerkGuid, extraInformation);
            url = url.Replace(Names.PersonalizationElements.ReferrerId, extraInformation);
            url = url.Replace(Names.PersonalizationElements.ReferrerSubId, extraInformation);
            url = url.Replace(Names.PersonalizationElements.UserInvitationGuid, extraInformation);
            url = url.Replace(Names.PersonalizationElements.UserInvitationStripGuid, CreateOfferStripGuid(extraInformation));
            url = url.Replace(Names.PersonalizationElements.UserPerkStripGuid, CreateOfferStripGuid(extraInformation));
            url = url.Replace(Names.PersonalizationElements.Sha1, HashCode(oUser.EmailAddress));
            url = url.Replace(Names.PersonalizationElements.EmailAddress, oUser.EmailAddress);

            if (url.Contains(Names.PersonalizationElements.SNVariable))
            {
                url = url.Replace(Names.PersonalizationElements.SNVariable, GetSNvariable(oUser.UserId.ToString()));
            }
            if (url.Contains(Names.PersonalizationElements.Dvariable))
            {

                url = url.Replace(Names.PersonalizationElements.Dvariable, GetDvariable(oUser.UserId.ToString()));
            }

            //Added on 12/14/2011
            url = url.Replace(Names.PersonalizationElements.Email, oUser.EmailAddress);
            url = url.Replace(Names.PersonalizationElements.Fn, oUser.FirstName);
            url = url.Replace(Names.PersonalizationElements.Ln, oUser.FirstName);
            if (!string.IsNullOrEmpty(oUser.Address1))
            {
                url = url.Replace(Names.PersonalizationElements.Add1, oUser.Address1);
            }
            else
            {
                url = url.Replace(Names.PersonalizationElements.Add1, "");
            }
            url = url.Replace(Names.PersonalizationElements.City, oUser.City);
            url = url.Replace(Names.PersonalizationElements.State, oUser.StateName);
            url = url.Replace(Names.PersonalizationElements.ZipCode, oUser.ZipCode);
            url = url.Replace(Names.PersonalizationElements.DOB, oUser.DateOfBirth.ToString("MM/dd/yyyy"));
            url = url.Replace(Names.PersonalizationElements.Country, oUser.CountryName);
            if (oUser.Gender.ToString().ToLower() == "m")
            {
                url = url.Replace(Names.PersonalizationElements.Gender, "m");
            }
            else if (oUser.Gender.ToString().ToLower() == "f")
            {
                url = url.Replace(Names.PersonalizationElements.Gender, "f");
            }
            //Added on 7/11/2013 to Add Age Parameter
            url = url.Replace(Names.PersonalizationElements.Age, oUser.Age.ToString());
            //End method
            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string CreateOfferStripGuid(string guid)
        {
            return guid.Replace("-", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HashCode(string str)
        {
            string rethash = "";
            try
            {

                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string GetDvariable(string information)
        {
            Members.PrecisionSample.Components.Data_Layer.UserDataServices oManager = new Members.PrecisionSample.Components.Data_Layer.UserDataServices();
            string dvariable = oManager.GetDvariable(Convert.ToInt32(information));
            return dvariable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static string GetSNvariable(string information)
        {
            Members.PrecisionSample.Components.Data_Layer.UserDataServices oManager = new Members.PrecisionSample.Components.Data_Layer.UserDataServices();
            string snvariable = oManager.GetSNvariable(Convert.ToInt32(information));
            return snvariable;
        }

        #endregion
    }
}
