using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class ProfileManager
    {
        ProfileDataServer oProfileDataServer = new ProfileDataServer();

        #region getprofiles for WLlables
        /// <summary>
        /// Get User Avliable Profiles
        /// </summary>
        /// <param name="MemberLanguage">Memeber Language</param>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        public List<Profile> GetProfiles(string MemberLanguage, int UserId)
        {
            return oProfileDataServer.GetProfiles(MemberLanguage, UserId);
        }
        #endregion

    }
}
