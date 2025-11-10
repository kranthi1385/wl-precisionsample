using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
   public class Friend
    {
        #region public Variables
        /// <summary>
        /// public variables
        /// </summary>
        public int UserId { get; set; }
        public string FriendFirstName { get; set; }
        public string FriendEmailAddress { get; set; }
        public int Mode { get; set; }
        public string Status { get; set; }
        public decimal RewardAmount { get; set; }
        public decimal Commission { get; set; }
        #endregion
    }
}
