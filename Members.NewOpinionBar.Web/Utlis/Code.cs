using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Members.OpinionBar.Components.Entities;
using Members.OpinionBar.Components.Business_Layer;
using Members.PrecisionSample.Common.Utils;
namespace Members.NewOpinionBar.Web.Utlis
{
    public class Code : System.Web.UI.Page
    {

        #region Methods

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
            url = url.Replace(Names.PersonalizationElements.FedResponseId, extraInformation);
            url = url.Replace(Names.PersonalizationElements.UserInvitationStripGuid, CreateOfferStripGuid(extraInformation));
            url = url.Replace(Names.PersonalizationElements.UserPerkStripGuid, CreateOfferStripGuid(extraInformation));
            url = url.Replace(Names.PersonalizationElements.Sha1, HashCode(oUser.EmailAddress));
            url = url.Replace(Names.PersonalizationElements.EmailAddress, oUser.EmailAddress);
            url = url.Replace(Names.PersonalizationElements.SubId3, oUser.SubId3); //Txid is added 11/14/2015
            url = url.Replace(Names.PersonalizationElements.UserInvitationID, CreateOfferStripGuid(extraInformation)); //Added on 7/10/2012 to Accept UserInvitationID also

            //if (url.Contains(Names.PersonalizationElements.SNVariable))
            //{
            //    url = url.Replace(Names.PersonalizationElements.SNVariable, GetSNvariable(oUser.UserId.ToString()));
            //}
            //if (url.Contains(Names.PersonalizationElements.Dvariable))
            //{

            //    url = url.Replace(Names.PersonalizationElements.Dvariable, GetDvariable(oUser.UserId.ToString()));
            //}

            //if (url.Contains(Names.PersonalizationElements.TE))
            //{

            //    url = url.Replace(Names.PersonalizationElements.Dvariable, GetTEvariable(oUser.UserId));
            //}

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
            if (!string.IsNullOrEmpty(guid))
            {
                return guid.Replace("-", "");
            }
            else
            {
                return guid;
            }

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="information"></param>
        ///// <returns></returns>
        //public static string GetDvariable(string information)
        //{
        //    UserManager oManager = new UserManager();
        //    string dvariable = oManager.GetDvariable();
        //    return dvariable;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public static string GetTEvariable(int userId)
        //{
        //    UserManager oManager = new UserManager();
        //    string dvariable = oManager.GetTEvariable(userId);
        //    return dvariable;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="information"></param>
        ///// <returns></returns>
        //public static string GetSNvariable(string information)
        //{
        //    UserManager oManager = new UserManager();
        //    string snvariable = oManager.GetSNvariable();
        //    return snvariable;
        //}



        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="objEntity"></param>
        /// <param name="extrainformation"></param>
        /// <returns></returns>

        public static string PixelReplacements(string url, UserEntity objEntity, string extrainformation)
        {
            url = url.Replace(Attribute.personalizationElements.AccessCode, objEntity.AccessCode);
            //Fill all :
            // Profile Related
            url = url.Replace(Attribute.personalizationElements.ProfileName, objEntity.ProfileName);
            url = url.Replace(Attribute.personalizationElements.ProfileId, objEntity.ProfileId.ToString());
            url = url.Replace(Attribute.personalizationElements.ProfileClickDt, objEntity.ProfileClickDate);
            url = url.Replace(Attribute.personalizationElements.ProfileCompletedDt, extrainformation);
            // Survey Related
            url = url.Replace(Attribute.personalizationElements.SurveyId, objEntity.surveyId.ToString());
            url = url.Replace(Attribute.personalizationElements.SurveyName, objEntity.surveynName);
            if (!string.IsNullOrEmpty(objEntity.SurveyClickDate))
            {
                url = url.Replace(Attribute.personalizationElements.SurveyClickDt, objEntity.SurveyClickDate.ToString());
            }
            if (!string.IsNullOrEmpty(objEntity.surveyCompleteDate))
            {
                url = url.Replace(Attribute.personalizationElements.SurveyCompletedDt, objEntity.surveyCompleteDate.ToString());
            }
            url = url.Replace(Attribute.personalizationElements.MemberReward, objEntity.memberReward.ToString());
            url = url.Replace(Attribute.personalizationElements.PartnerRewardAmount, objEntity.partnerReward.ToString());
            UserManager objUserManager = new UserManager();
            url = url.Replace(Attribute.personalizationElements.Md5, objUserManager.md5(objEntity.AccessCode + extrainformation));
            return url;
        }
        #endregion

    }
}