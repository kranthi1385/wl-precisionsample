using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class CaptchaResponse
    {
        public bool success { get; set; }
        public List<string> ErrorCodes { get; set; }
    }

}
