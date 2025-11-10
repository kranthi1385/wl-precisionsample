using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Web.UI.WebControls;
using Members.PrecisionSample.River.Web;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;

namespace Members.PrecisionSample.River.Web.BasePage
{
    public class BasePage : System.Web.UI.Page
    {
        #region Propreties
        /// <summary>
        /// 
        /// </summary>

        private Guid _userGuid = Guid.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string IpAddress
        {
            get
            {
                return Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>

        public Guid UserGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ug"]))
                {
                    _userGuid = new Guid(Request.QueryString["ug"].ToString());
                    return _userGuid;
                }
                else
                {
                    _userGuid = Guid.Empty;
                    return _userGuid;
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        public Client ClientDetails
        {
            get
            {
                if (Session["ClientDetails"] != null)
                {
                    return Session["ClientDetails"] as Client;
                }
                return new Client();
            }
            set
            {
                Session["ClientDetails"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>

        //public User UserDetails
        //{
        //    get
        //    {
        //        if (Session["User"] != null)
        //        {
        //            return Session["User"] as User;
        //        }
        //        return new User();
        //    }
        //    set
        //    {
        //        Session["User"] = value;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public User GetUserDetails(string userName)
        {
            //if (Session["User"] == null)
            //{
            //    UserManager oUserManager = new UserManager();
            //    Session["User"] = oUserManager.GetUserData(userName);
            //}
            //else if (UserDetails.EmailAddress.ToLower() != userName.ToLower() &&
            //    UserDetails.UserGuid.ToString().ToLower() != userName.ToLower() &&
            //     UserDetails.UserId.ToString().ToLower() != userName.ToLower())
            //{
            //    UserManager oUserManager = new UserManager();
            //    Session["User"] = oUserManager.GetUserData(userName);
            //}
            return Session["User"] as User;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Client GetClientDetails()
        {
            if (Session["ClientDetails"] == null)
            {
                //Code to Get the Client Id By Passing hosturl 
                Members.PrecisionSample.Components.Business_Layer.UserManager oUserManager = new Members.PrecisionSample.Components.Business_Layer.UserManager();
                Session["ClientDetails"] = oUserManager.GetPartnerOrgIdByMemberUrl(Request.Url.OriginalString.ToString().Replace(Request.Url.PathAndQuery, "").Replace(":80", ""), UserGuid);
            }
            else if (ClientDetails.UserGuid.ToString().ToLower() != UserGuid.ToString().ToLower())
            {
                //Code to Get the Client Id By Passing hosturl 
                Members.PrecisionSample.Components.Business_Layer.UserManager oUserManager = new Members.PrecisionSample.Components.Business_Layer.UserManager();
                Session["ClientDetails"] = oUserManager.GetPartnerOrgIdByMemberUrl(Request.Url.OriginalString.ToString().Replace(Request.Url.PathAndQuery, "").Replace(":80", ""), UserGuid);
            }
            return Session["ClientDetails"] as Client;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>



        #endregion Propreties

        #region Page Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            //GetUserDetails(UserGuid.ToString());
            GetClientDetails();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void onLoad(object sender, EventArgs e)
        {
            base.OnLoad(e);
        }


        #endregion
    }
}
