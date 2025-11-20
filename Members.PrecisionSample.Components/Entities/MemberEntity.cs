using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class MemberEntity
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        public int OrgId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Dob { get; set; }
        public int EthnicityId { get; set; }
        public string Gender { get; set; }
        public int StateId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsFraud { get; set; }
        public Guid UserInvitationGuid { get; set; }
        public int RefferId { get; set; }
        public int FbPreference { get; set; }
        public string RelevantId { get; set; }
        public int RelevantScore { get; set; }
        public int RelevantProfileScore { get; set; }
        public string VerityId { get; set; }
        public int VerityScore { get; set; }
        public string FpfScore { get; set; }
        public int GeoCorrelationFlag { get; set; }
        public string SubId3 { get; set; }
        public int Rid { get; set; }
        public string ExtId { get; set; }
        public int Count { get; set; }
        public int SpCount { get; set; } // survey click count
        public string ChallengeId { get; set; }
        public int VerityQstCount { get; set; }
        public string GeoIp2Content { get; set; }
        public string ExtMemberId { get; set; }
        public string DomainUrl { get; set; }
        public string SubId2 { get; set; }
        public string IpAddress { get; set; }
        public string LanguageId { get; set; }
        public string pubId { get; set; }
        #endregion
    }
}
