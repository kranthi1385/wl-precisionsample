using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class User
    {
        #region Public  Variables

        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int CountryId { get; set; }
        public string Country_id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Dob { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int EthnicityId { get; set; }
        public string Gender { get; set; }
        public int StateId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int RefferId { get; set; }
        public int FriendId { get; set; }
        public string IpAddress { get; set; }
        public string SubId2 { get; set; }
        public int rfid { get; set; }
        public string SubId3 { get; set; }
        public string ReferrerUrl { get; set; }
        public bool DOI { get; set; }
        public string RegistrationStep { get; set; }
        public string RegistrationDate { get; set; }
        public int Age { get; set; }
        public DateTime LeadDate { get; set; }
        public int SplitFlag { get; set; }
        public int HitId { get; set; }
        public int ClickId { get; set; }
        public bool IsFraud { get; set; }
        public bool IsDnc { get; set; }
        public string LanguageCode { get; set; }
        public string CncReason { get; set; }
        public string OrgLogo { get; set; }
        public string OrgName { get; set; }
        public string AboutusText { get; set; }
        public string MemberUrl { get; set; }
        public int OrgId { get; set; }
        public string RelevantId { get; set; }
        public int RelevantScore { get; set; }
        public int RelevantProfileScore { get; set; }
        public string VerityId { get; set; }
        public int VerityScore { get; set; }
        public string FpfScore { get; set; }
        public string TotalScore { get; set; }
        public int GeoCorrelationFlag { get; set; }
        public int Rid { get; set; }
        public string ExtId { get; set; }
        public string Country2Ip { get; set; }
        public string FacebookId { get; set; }
        public string AccessToken { get; set; }
        public int FbPreference = 2;
        public string SurveyStatus { get; set; }
        public string ExtMemberInfo { get; set; }
        public bool IsDoi { get; set; }
        public int OptTiburonSplitFlag { get; set; }
        public int IsDoiRequired { get; set; }
        public int Is_doi_pixel_fired { get; set; }
        public int Is_soi_pixel_fired { get; set; }
        public int SurveyClicksCount { get; set; }
        public int DmaId { get; set; }
        public bool Is_verity_required { get; set; }
        public bool Is_relevantid_required { get; set; }
        public bool Is_relevantid_required_for_project { get; set; }
        public string ExtMemberId { get; set; }
        public decimal PartnerRevshare { get; set; }
        public string EthinicityType { get; set; }
        public string ChallengeId { get; set; }
        public int IsMobileNoFromPs { get; set; }
        public int CpaCount { get; set; }
        public bool IsAllinBoxPosted { get; set; }
        public int CaptchaFlag { get; set; }
        public string DncReason { get; set; }
        public string FbUsername { get; set; }
        public string ProjectName { get; set; }
        public string TransactionId { get; set; }
        public string IsNew { get; set; }
        public string DomainUrl { get; set; }
        public bool VerityDOBFail { get; set; }
        public string ChallengeQuestion1 { get; set; }
        public string ChallengeQuestion2 { get; set; }
        public string ChallengeQuestion3 { get; set; }
        public string ChallengeOption1 { get; set; }
        public string ChallengeOption2 { get; set; }
        public string ChallengeOption3 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Ethnicity { get; set; }
        public int LanguageId { get; set; }
        public string CreateDate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string StarPageText { get; set; }
        public string geodata { get; set; }
        public bool ispeerlytype2 { get; set; }
        public int RouterReferrerId { get; set; }
        public string RouterSubId2 { get; set; }
        public int RouterSubReferrerId { get; set; }
        public string Rdjson { get; set; }
        public string RedirectURL { get; set; }
        public bool IsGDPRCompliance { get; set; }
        public string CleanIDJson { get; set; }
        #endregion

    }
    public class ApiCall
    {
        public string recipient { get; set; }
        public string type = "transactional";
        public string source = "Unsubscribe";
    }
    public class Recipients
    {
        public List<ApiCall> recipients = new List<ApiCall>();
    }
}
