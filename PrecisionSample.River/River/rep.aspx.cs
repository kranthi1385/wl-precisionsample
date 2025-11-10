using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Newtonsoft.Json;
using System.Configuration;


namespace Members.PrecisionSample.River.Web.River
{
    public partial class rep : System.Web.UI.Page
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
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserStatusGuid.ToString().ToUpper() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4" || UserStatusGuid.ToString().ToUpper() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00")
                {
                    dvFailureMessage.Visible = true;

                }
                else if (UserStatusGuid.ToString().ToUpper() == "6AC169C6-DF47-4CD1-8F4D-1311F5C5F163")
                {
                    dvSuccessMessage.Visible = true;

                }
                //UpdateUserInvitationDetails
                RiverManager oRiverManager = new RiverManager();
                string Status = oRiverManager.UpdateUserInvitationDetails(UserStatusGuid, UserInvitationGuid);
               string [] _statusvalues = Status.Split(';');
                if(_statusvalues.Length  > 1)
                {
                    if (_statusvalues[0] == ConfigurationManager.AppSettings["wetellsmscampaignId"])
                    {
                        Response.Redirect(ConfigurationManager.AppSettings["Wetellstep2url"] + "&ug=" + _statusvalues[1]);
                    }
                }
                else
                {
                }
               
                if (this.button.Value != "Yes")
                {
                    RiverManager objRiverManager = new RiverManager();
                    var pagedata = oRiverManager.GetProjectDetails(UserGUID);
                    if (pagedata.Contains("rfep.htm"))
                    {

                    }
                    else
                    {
                        Response.Redirect("/River/top10.htm?ug=" + UserGUID);

                    }
                    //writer.Write(JsonConvert.SerializeObject(pagedata));

                }


            }

        }
        #endregion


        #region Button Events
        public void btnYes_click(object sender, EventArgs e)
        {
            Response.Redirect("/river/entry.htm?ug=" + UserGUID);
        }

        public void btnNo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/river/entry.htm?ug=" + UserGUID);
        }
        #endregion
    }
}
