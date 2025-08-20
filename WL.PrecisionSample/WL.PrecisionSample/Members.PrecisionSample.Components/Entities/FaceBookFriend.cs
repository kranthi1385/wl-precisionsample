using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
   public class FaceBookFriend
    {
        #region public Variables
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string EmailAddress { get; set; }
        
        public Int32 Mode { get; set; }
       
        #endregion
    }
}
