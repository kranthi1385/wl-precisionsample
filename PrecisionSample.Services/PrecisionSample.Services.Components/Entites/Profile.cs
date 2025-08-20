using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrecisionSample.Services.Components.Entites
{
    public class Profile
    {
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileStatus { get; set; }
        public bool IsSelected { get; set; }
    }
}
