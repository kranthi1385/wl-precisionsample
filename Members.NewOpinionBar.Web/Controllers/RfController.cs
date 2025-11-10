using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using Members.PrecisionSample.Common.Security;

namespace Members.NewOpinionBar.Web.Controllers
{
    [Authorize]
    public class RfController : BaseController
    {
        #region Index
        // GET: Downline
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Downline
        /// <summary>
        /// Downline
        /// </summary>
        /// <returns></returns>
        public ActionResult Downline()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.UserId = Identity.Current.UserData.UserId;
            }
            return View("/Views/Home/DownLine.cshtml");
        }
        #endregion

        #region Friend Information
        /// <summary>
        /// Friend Information
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FriendInformation()
        {
            List<Friend> ofriend = new List<Friend>();
            FriendManager objFriendManager = new FriendManager();
            User objuser = new User();
            ofriend = objFriendManager.FriendInformation(Convert.ToInt32(Identity.Current.UserData.UserId), MemberIdentity.Client.ClientId);
            return Json(ofriend, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Friend List
        /// <summary>
        /// Friend List
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FriendList()
        {
            List<Friend> ofriend = new List<Friend>();
            FriendManager oFriendManager = new FriendManager();
            User objuser = new User();
            ofriend = oFriendManager.FriendList(Convert.ToInt32(Identity.Current.UserData.UserId), MemberIdentity.Client.ClientId);
            return Json(ofriend, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}