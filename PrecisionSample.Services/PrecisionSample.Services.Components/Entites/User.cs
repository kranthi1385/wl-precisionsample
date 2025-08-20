using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
    public class User
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "EmailAddress is Required")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress = string.Empty;
        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Dob is Required")]
        public string Dob { get; set; }
        [Required(ErrorMessage = "Address1 is Required")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }
        public Guid UserGuid { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public string DomainUrl { get; set; }
        [Required(ErrorMessage = "CountryId is Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "ZipCode is Required")]
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "EthnicityId is Required")]
        public int EthnicityId { get; set; }
        [Required(ErrorMessage = "StateId is Required")]
        public int StateId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [Required(ErrorMessage = "RefferId is Required")]
        public int RefferId { get; set; }
        public string SubId3 { get; set; }
        public string SubId2 { get; set; }
        public string IpAddress { get; set; }
        public string RegistrationStep { get; set; }
        public string ReferrerUrl { get; set; }
        public string FriendId { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public int ClickId { get; set; }
        public int HitId { get; set; }
        public int LanguageId { get; set; }
        public bool IsDnc { get; set; }
        public bool IsCompliance { get; set; }
        public int CampaignID { get; set; }
        public string CustomAttribute { get; set; }
        public int RouterSubReferrerId { get; set; }
    }
}
