using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Common.Security;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;

namespace Members.NewOpinionBar.Web.Controllers
{
    [Authorize]
    public class TafController : Controller
    {
        #region Index
        /// <summary>
        /// Friend Invitation
        /// </summary>
        /// <returns></returns>
        // GET: Taf
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Friend Invite
        /// <summary>
        /// Friend Invite
        /// </summary>
        /// <returns></returns>
        public ActionResult Invite()
        {
            ViewBag.OrgName = MemberIdentity.Client.OrgName;
            return View("/Views/Home/Invite.cshtml");
        }
        #endregion

        #region Invite Friends
        public JsonResult InviteFriends(int camID, string tid, List<Friend> data)
        {
            string xml = "<friends>";
            foreach (Friend name in data)
            {
                xml += "<friend>";
                xml += "<friend_first_name>" + name.FriendFirstName + "</friend_first_name>";
                xml += "<friend_email_address>" + name.FriendEmailAddress + "</friend_email_address>";
                xml += "</friend>";
            }
            xml += "</friends>";
            FriendManager objFriendManager = new FriendManager();
            User objuser = new User();
            objFriendManager.FriendInsertList(Convert.ToInt32(Identity.Current.UserData.UserId), xml, MemberIdentity.Client.ClientId, camID, tid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}