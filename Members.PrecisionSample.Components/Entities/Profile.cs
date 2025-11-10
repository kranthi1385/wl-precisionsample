using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public class Profile
    {
        #region public variable
        /// <summary>
        /// 
        /// </summary>
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileStatus { get; set; }
        public bool IsChecked { get; set; }
        public int Count { get; set; }

        #endregion
    }
}
