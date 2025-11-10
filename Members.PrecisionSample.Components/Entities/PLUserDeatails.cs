using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Members.PrecisionSample.Components.Entities
{
    public class PLUserDeatails
    {
        #region public variables
        public int UserId { get; set; }
        public string PLUserId { get; set; }
        public string CountryCode { get; set; }
        public int Gender { get; set; }
        public string Dob { get; set; }
        public string Zipcode { get; set; }
        public List<PLProfielData> LstPlProfileData { get; set; }
        #endregion
    }

    public class PLProfielData
    {
        #region public variables
        public string PlQuestionCode { get; set; }
        public string PlOptionCode { get; set; }
        #endregion
    }
}
