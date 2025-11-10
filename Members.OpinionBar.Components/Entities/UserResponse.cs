using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
   public class UserResponse
    {
        public string message { get; set; }
        public string UserGuid { get; set; }
        public string UserId { get; set; }
        public string CountryCode { get; set; }
    }

    public class RootObject
    {
        public UserResponse result { get; set; }
    }
}