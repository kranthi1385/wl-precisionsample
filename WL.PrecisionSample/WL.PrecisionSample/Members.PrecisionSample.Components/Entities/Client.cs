using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Client
    {
        #region public variables

        public string MemberUrl { get; set; }
        public int ClientId { get; set; }
        public string OrgName { get; set; }
        public string OrgLogo { get; set; }
        public int Referrerid { get; set; }
        public string Emailaddress { get; set; }
        public string MgLoginPath { get; set; }
        public string Password { get; set; }
        public int CopyrightYear { get; set; }
        public string Address { get; set; }
        public string AboutusText { get; set; }
        public string StyleSheettheme { get; set; }
        public string HomePageURL { get; set; }
        public bool IsPopUp { get; set; }
        public bool IsProfilePixel { get; set; }
        public bool IsSurveyPixel { get; set; }
        public string ProfileClickPixelUrl { get; set; }
        public string SurveyClickPixelUrl { get; set; }
        public string SurveyCompletePixelUrl { get; set; }
        public bool VerityRequired { get; set; }
        public bool RelevantIdRequired { get; set; }
        public string ProfileCompletePixelUrl { get; set; }
        public int OrgTypeId { get; set; }
        public Guid UserGuid { get; set; }
        public bool IsStep1Enable { get; set; }
        public bool IsStandalone { get; set; }
        public bool IsRewardsShow { get; set; }
        public string PartnerTerminateUrl { get; set; }
        public bool IsTop10Enable { get; set; }
        public bool IsEmailInvitationEnable { get; set; }
        public bool IsSmsInvitation { get; set; }
        public string MgStep2Path { get; set; }
        public string RewardTextType { get; set; }
        public string LanguageCode { get; set; }
        public bool IsEnablelogin { get; set; }

        #endregion
    }

    public class APInfo
    {
        public int APIPartnerId;
        public string APIPartnerName;
        public string ProdAPIURL;
        public string UserName;
        public string Password;
    }

    public class IpsosEligibility
    {
        public bool isEligible { get; set; }
        public List<string> substatuses { get; set; }
    }

    public class PartnerClientInfo
    {
        public int ClientID { get; set; }
        public string OutsidePanelProjectID { get; set; }
    }
}
