using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Members.PrecisionSample.River.Web.Services
{
    public partial class ClientDetails : Members.PrecisionSample.River.Web.Services.ALservice
    {
       
 #region private Variables
        private Guid _userGUID = Guid.Empty;
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
        #endregion
        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
         protected void Page_Load(object sender, EventArgs e)
        {

            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/json";
        }
        #endregion

        #region Render
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {

            StringWriter output = new StringWriter();
            base.Render(new HtmlTextWriter(output));
            //This is the rendered HTML of your page. Feel free to manipulate it.
            string outputAsString = output.ToString();
            //writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            Response.ContentEncoding = Encoding.UTF8;

            Client ObjClient = new Client();
            ObjClient.ClientId = MemberIdentity.Client.ClientId;
            ObjClient.OrgLogo = MemberIdentity.Client.OrgLogo;
            if (MemberIdentity.Client.ClientId == 70)
            {
                ObjClient.OrgName = "OpinioNetwork";
            }
            else
            {
                ObjClient.OrgName = MemberIdentity.Client.OrgName;
            }
            ObjClient.Referrerid = MemberIdentity.Client.Referrerid;
            ObjClient.MemberUrl = MemberIdentity.Client.MemberUrl;
            ObjClient.Emailaddress = MemberIdentity.Client.Emailaddress;
            ObjClient.MgLoginPath = MemberIdentity.Client.MgLoginPath;
            ObjClient.Password = MemberIdentity.Client.Password;
            ObjClient.CopyrightYear = MemberIdentity.Client.CopyrightYear;
            ObjClient.Address = MemberIdentity.Client.Address;
            ObjClient.AboutusText = MemberIdentity.Client.AboutusText;
            ObjClient.StyleSheettheme = MemberIdentity.Client.StyleSheettheme;
            ObjClient.HomePageURL = MemberIdentity.Client.HomePageURL;
            ObjClient.IsPopUp = MemberIdentity.Client.IsPopUp;
            ObjClient.IsProfilePixel = MemberIdentity.Client.IsProfilePixel;
            ObjClient.IsSurveyPixel = MemberIdentity.Client.IsSurveyPixel;
            ObjClient.ProfileClickPixelUrl = MemberIdentity.Client.ProfileClickPixelUrl;
            ObjClient.SurveyClickPixelUrl = MemberIdentity.Client.SurveyClickPixelUrl;
            ObjClient.ProfileCompletePixelUrl = MemberIdentity.Client.ProfileCompletePixelUrl;
            ObjClient.SurveyCompletePixelUrl = MemberIdentity.Client.SurveyCompletePixelUrl;
            ObjClient.OrgTypeId = MemberIdentity.Client.OrgTypeId;
            ObjClient.IsStep1Enable = MemberIdentity.Client.IsStep1Enable; // Added on 9/26/2014 to Diable Step1 For some API /Social Partners.
            writer.Write(JsonConvert.SerializeObject(ObjClient));
        }
        #endregion
        }
    }

