using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The Email Address field is required")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid EmailAddress.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        public string Password { get; set; }
    }
}
