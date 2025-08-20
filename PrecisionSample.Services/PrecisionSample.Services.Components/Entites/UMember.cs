using System;
using System.ComponentModel.DataAnnotations;

namespace PrecisionSample.Services.Components.Entites
{
    public class UMember
    {
        [Required(ErrorMessage = "ExtMemberId is Required")]
        public string ExtMemberId { get; set; }

        [Required(ErrorMessage = "RId is Required")]
        public int? Rid { get; set; }
        public string TxId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        public string Zip { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Ethnicity { get; set; }
        public string City { get; set; }
        public string DomainUrl { get; set; }
        [Required(ErrorMessage = "UserGuid is Required")]
        public Guid? UserGuid { get; set; }
    }
}
