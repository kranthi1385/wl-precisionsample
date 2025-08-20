using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
    public class Member
    {
        //Members Object
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
        string emailAddress = string.Empty;
        public string EmailAddress { get { return emailAddress; } set { emailAddress = value; } }
        public string Zip { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Ethnicity { get; set; }
        public string City { get; set; }
        public string DomainUrl { get; set; }
        public Guid? UserGuid { get; set; }
    }
}
