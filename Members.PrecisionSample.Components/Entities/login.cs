using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class login
    {
        public string ReferrerUrl { get; set; }
        [Required(ErrorMessage = "Invalid Email")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
     

    }
}
