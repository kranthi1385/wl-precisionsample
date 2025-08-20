using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Newtonsoft.Json;

namespace Members.PrecisionSample.River.Web.River
{
    public partial class rfep : System.Web.UI.Page
    {
        #region private variables
        private Guid _userGUID = Guid.Empty;
        private Guid _userStatusGuid = Guid.Empty;
        private Guid _userInvitationGuid = Guid.Empty;
        #endregion
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string Mode
        {
            get
            {
                if (Request.QueryString["Mode"] != null)
                    return Convert.ToString(Request.QueryString["Mode"]);
                else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserGUID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ug"]))
                {
                    _userGUID = new Guid(Request.QueryString["ug"].ToString());
                    return _userGUID;
                }
                else
                {
                    _userGUID = Guid.Empty;
                    return _userGUID;
                }
            }
        }

        public Guid UserStatusGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["usg"]))
                {
                    _userStatusGuid = new Guid(Request.QueryString["usg"].ToString());
                    return _userStatusGuid;
                }
                else
                {
                    _userStatusGuid = Guid.Empty;
                    return _userStatusGuid;
                }
            }
        }
        public Guid UserInvitationGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["uig"]))
                {
                    _userInvitationGuid = new Guid(Request.QueryString["uig"].ToString());
                    return _userInvitationGuid;
                }
                else
                {
                    _userInvitationGuid = Guid.Empty;
                    return _userInvitationGuid;
                }
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Form["ButtonNo"] != "Yes")   // ButtonNo value is not equal to yes means member coming from bookmark page
                {
                    RiverManager objRiverManager = new RiverManager();
                    string pagedata = objRiverManager.GetProjectDetails(UserGUID);
                    if (pagedata.Contains("rfep.aspx"))
                    {

                    }
                    else
                    {
                        Response.Redirect("/River/top10.htm?ug=" + UserGUID);

                    }
                }
                else { }
            }
        }
    }
}
