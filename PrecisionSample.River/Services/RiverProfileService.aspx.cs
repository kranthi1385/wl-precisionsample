using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using System.Configuration;
using Members.PrecisionSample.River.Web.River.Utils;
using System.Net;
using System.Security.Cryptography;
namespace Members.PrecisionSample.River.Web.Services
{
    public partial class RiverProfileService : Members.PrecisionSample.River.Web.BasePage.BasePage
    {


        #region private variables
        private Guid _userGUID = Guid.Empty;
        private Guid _redeemptionGUID = Guid.Empty;
        private Guid _profileGuid = Guid.Empty;
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

        /// <summary>
        /// 
        /// </summary>
        public string DOB
        {
            get
            {
                if (Request.QueryString["date"] != null)
                    return Convert.ToString(Request.QueryString["date"]);
                else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ProfileId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pid"]))
                {
                    _profileGuid = new Guid(Request.QueryString["pid"].ToString());
                    return _profileGuid;
                }
                else
                {
                    _profileGuid = new Guid(ConfigurationManager.AppSettings["BasicProfileGuid"].ToString());
                    return _profileGuid;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserInvitationGuid
        {
            get
            {
                if (Request.Params["uid"] != null)
                    return Convert.ToString(Request.Params["uid"].ToString());
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PageType
        {
            get
            {
                if (Request.QueryString["ptype"] != null)
                    return Convert.ToString(Request.QueryString["ptype"]);
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string Status
        {
            get
            {
                if (Request.Params["s"] != null)
                    return Convert.ToString(Request.Params["s"]);
                else
                    return string.Empty;
            }
        }

        public int Rid
        {
            get
            {
                if (Request.Params["rid"] != null)
                    return Convert.ToInt32(Request.QueryString["rid"]);
                else
                    return 0;
            }
        }

        public string Sid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["sid"]))
                    return Convert.ToString(Request.QueryString["sid"]);
                else
                    return null;
            }
        }

        public string TxId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["txid"]))
                    return Convert.ToString(Request.QueryString["txid"]);
                else
                    return null;
            }
        }

        public string EmailAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Email"]))
                    return Convert.ToString(Request.QueryString["Email"]);
                else
                    return null;
            }

        }

        public string FirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["fn"]))
                    return Convert.ToString(Request.QueryString["fn"]);
                else
                    return null;
            }
        }

        public string LastName
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["ln"]))
                    return Convert.ToString(Request.QueryString["ln"]);
                else
                    return null;
            }
        }

        public string Gender
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["g"]))
                    return Convert.ToString(Request.QueryString["g"]);
                else
                    return null;
            }
        }

        public string Country
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["loc"]))
                    return Convert.ToString(Request.QueryString["loc"]);
                else
                    return string.Empty;
            }
        }

        #endregion
        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //System.Diagnostics.Debugger.Break();
            //
            // Setup the appropriate content and encoding type
            //
            Response.ContentEncoding = Encoding.UTF8;
            if (Mode == "Top10Questions")
            {
                List<ProfileQuestions> lstQuestion = new List<ProfileQuestions>();
                Members.PrecisionSample.Components.Business_Layer.RiverManager objQuestionBl = new Members.PrecisionSample.Components.Business_Layer.RiverManager();
                var Pagedata = objQuestionBl.GetquestionsforTop10(UserGUID);
                writer.Write(JsonConvert.SerializeObject(Pagedata));
            }
            else if (Mode == "OptionsSaveTop10")
            {

                SurveyUrl objUrl = new SurveyUrl();
                List<ProfileQuestions> objQuestionSave = new List<ProfileQuestions>();
                Members.PrecisionSample.Components.Business_Layer.RiverManager objQustSaveBl = new Members.PrecisionSample.Components.Business_Layer.RiverManager();
                //System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //objQuestionSave = jsonSerializer.Deserialize<List<SurveyDownLine.Components.Entities.ProfileQuestions>>(Request.Form[0]);
                //string listXml = BuildAnswerXml(objQuestionSave);
                objQustSaveBl.Top10SaveOptions(Request.Form[0]);

            }
            else if (Mode == "Inserttop10skiplog")
            {
                Members.PrecisionSample.Components.Business_Layer.RiverManager objQuestionBl = new Members.PrecisionSample.Components.Business_Layer.RiverManager();
                objQuestionBl.InsertTop10PageSkipLog(UserGuid);
                writer.Write(1);
            }
            else if (Mode == "GetSurveyUrl")
            {
                string _message = string.Empty;
                string url = string.Empty;
                try
                {
                    User oUser = new User();
                    Members.PrecisionSample.Components.Business_Layer.UserManager ObjUserManager = new Members.PrecisionSample.Components.Business_Layer.UserManager();
                    Members.PrecisionSample.Components.Entities.User objUser = new Members.PrecisionSample.Components.Entities.User();
                    string Result = ObjUserManager.GetSurveyUrlByInvitationGuid(UserInvitationGuid);
                    UserManager oManager = new UserManager();
                    int ClientId = Convert.ToInt32(ConfigurationManager.AppSettings["RiverOrgId"].ToString());
                    oUser = oManager.GetUserData(UserGUID.ToString(),null, ClientId);
                    url = Code.PEReplaceMents(Result, oUser, UserInvitationGuid.ToString());
                    //New Logic Implemneted for MD5 hash on 11/26/2014 by sandy.
                    if (url.Contains("&pid=prs&") & url.Contains("%%hash%%"))
                    {
                        string invokeurl = url;
                        string[] s1 = invokeurl.Split('?');
                        string[] s2 = s1[1].Split('&');
                        string _invokesid = string.Empty;
                        string _invokepid = string.Empty;
                        string _invokekey = ConfigurationManager.AppSettings["InvokeMD5hash"].ToString();
                        string _invokeuserid = UserInvitationGuid.ToString();
                        string _md5 = string.Empty;
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (s2[i].StartsWith("sid="))
                            {
                                _invokesid = s2[i].Split('=')[1];
                            }
                            if (s2[i].StartsWith("pid="))
                            {
                                _invokepid = s2[i].Split('=')[1];
                            }
                        }
                        //Hash the Value in Combination of all these Params with secret key
                        _md5 = md5(_invokesid + _invokepid + _invokeuserid + _invokekey);
                        invokeurl = invokeurl.Replace("%%hash%%", _md5);
                        url = invokeurl;
                    }
                    _message = "pass";
                }
                catch
                {
                    _message = "fail";
                }
                Session[UserGUID.ToString()] = null;
                if (_message == "pass")
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        if (Status == "ip2countryfail")
                        {
                            url = "/partner/newpsr.aspx?s=ip2countryfail&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                        }
                        else if (Status == "overflow")
                        {
                            url = "/partner/newpsr.aspx?s=overflow&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                        }

                        else if (Status == "quotafull")
                        {
                            url = "/partner/newpsr.aspx?s=quotafull&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                        }
                        else if (Status == "offernotexists")
                        {
                            url = "/partner/newpsr.aspx?s=offernotexists&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                        }
                        else if (string.IsNullOrEmpty(Status))
                        {
                            url = "/partner/newpsr.aspx?s=offernotexists&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                        }
                    }
                    else
                    {
                        url = "/partner/newpsr.aspx?s=offernotexists&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                    }
                }
                else
                {
                    url = "/partner/newpsr.aspx?s=offernotexists&uig=" + UserInvitationGuid.ToString() + "&ug=" + UserGUID.ToString();
                }

                writer.Write(JsonConvert.SerializeObject(url));
            }
        }

        #endregion

        public string md5(string plainText)
        {
            MD5 enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }
            return str.ToString();
        }

    }

}
