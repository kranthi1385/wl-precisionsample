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
using Members.PrecisionSample.River.Web.Services;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;
using NLog;

namespace Members.PrecisionSample.River.Web.Services
{
    public partial class RiverService : System.Web.UI.Page
    {

        #region private variables
        private Guid _userGUID = Guid.Empty;
        private Guid _redeemptionGUID = Guid.Empty;
        private Guid _profileGuid = Guid.Empty;
        private Guid _quotaGroupGuid = Guid.Empty;
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

        public string CallbackURL
        {
            get
            {
                if (Request.QueryString["url"] != null)
                    return Convert.ToString(Request.QueryString["url"]);
                else
                    return null;
            }
        }


        public string TransactionId
        {
            get
            {
                if (Request.QueryString["trans_id"] != null)
                    return Convert.ToString(Request.QueryString["trans_id"]);
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

        public Guid QuotaGroupGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["qgid"]))
                {
                    _quotaGroupGuid = new Guid(Request.QueryString["qgid"].ToString());
                    return _quotaGroupGuid;
                }
                else
                {
                    _quotaGroupGuid = Guid.Empty;
                    return _quotaGroupGuid;
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
        public Guid UserInvitationGuid
        {
            get
            {
                if (Request.Params["uig"] != null)
                    return new Guid(Request.Params["uig"].ToString());
                else
                    return Guid.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid UserStatusGuid
        {
            get
            {
                if (Request.Params["usg"] != null)
                    return new Guid(Request.Params["usg"].ToString());
                else
                    return Guid.Empty;
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

                if (!string.IsNullOrEmpty(Request.Params["rid"]))
                    return Convert.ToInt32(Request.Params["rid"]);
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
                    return string.Empty;
            }
        }

        public string TxId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["txid"]))
                    return Convert.ToString(Request.QueryString["txid"]);
                else
                    return string.Empty;
            }
        }

        public string EmailAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Email"]))
                    return Convert.ToString(Request.QueryString["Email"]);
                else
                    return string.Empty;
            }

        }

        public string FirstName
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["fn"]))
                    return Convert.ToString(Request.QueryString["fn"]);
                else
                    return string.Empty;
            }
        }

        public string LastName
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["ln"]))
                    return Convert.ToString(Request.QueryString["ln"]);
                else
                    return string.Empty;
            }
        }
        public string Email
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["ea"]))
                    return Convert.ToString(Request.QueryString["ea"]);
                else
                    return string.Empty;
            }
        }

        public string Gender
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["g"]))
                    return Convert.ToString(Request.QueryString["g"]);
                else
                    return string.Empty;
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

        public string DateOfBirth
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["date"]))
                    return Convert.ToString(Request.QueryString["date"]);
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RelevantScore
        {
            get
            {
                if (Request.Params["score"] != null)
                {
                    return Convert.ToInt32(Request.Params["score"]);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int RelevantProfileScore
        {
            get
            {
                if (Request.Params["pscore"] != null)
                {
                    return Convert.ToInt32(Request.Params["pscore"]);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>

        public string Rdjson
        {
            get
            {
                if (Request.Params["rdjson"] != null)
                {
                    return Request.Params["rdjson"].ToString();
                }
                return string.Empty;
            }
        }

        //public string Token
        //{
        //    get
        //    {
        //        if (Request.Params["token"] != null)
        //        {
        //            return Request.Params["token"].ToString();
        //        }
        //        return string.Empty;
        //    }
        //}

        //public string PassParam
        //{
        //    get
        //    {
        //        if (Request.Params["passParam"] != null)
        //        {
        //            return Request.Params["passParam"].ToString();
        //        }
        //        return string.Empty;
        //    }
        //}

        //public string GeoLonLat
        //{
        //    get
        //    {
        //        if (Request.Params["geoLonLat"] != null)
        //        {
        //            return Request.Params["geoLonLat"].ToString();
        //        }
        //        return string.Empty;
        //    }
        //}

        public string Project
        {
            get
            {
                if (Request.Params["project"] != null)
                {
                    return Request.Params["project"].ToString();
                }
                return string.Empty;
            }
        }

        public string RelevantId
        {
            get
            {
                if (Request.Params["rid"] != null)
                {
                    return Request.Params["rid"].ToString();
                }
                return string.Empty;
            }
        }
        public string FpfScores
        {
            get
            {
                if (Request.Params["fpfscores"] != null)
                {
                    return Request.Params["fpfscores"].ToString();
                }
                return string.Empty;
            }
        }

        public string IsNew
        {
            get
            {
                if (Request.Params["isnew"] != null)
                {
                    return Request.Params["isnew"].ToString();
                }
                return string.Empty;
            }

        }

        public int CallbackTypeId
        {
            get
            {

                if (!string.IsNullOrEmpty(Request.Params["type"]))
                    return Convert.ToInt32(Request.Params["type"]);
                else
                    return 0;
            }
        }

        public string Json
        {
            get
            {
                if (Request.Params["json"] != null)
                {
                    return Request.Params["json"].ToString();
                }
                return string.Empty;
            }
        }

        public string CleanID
        {
            get
            {
                if (Request.Params["cleanID"] != null)
                {
                    return Request.Params["cleanID"].ToString();
                }
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

        #region GetMethod
        //public string GetrdJson(string RequestURL, string token)
        //{
        //    String rdjson = string.Empty;
        //    StreamReader sr = null;
        //    try
        //    {
        //        ASCIIEncoding encoding = new ASCIIEncoding();
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(RequestURL);
        //        Request.Method = "GET";
        //        Request.Headers.Add("token", token);
        //        Request.ContentType = "application/json; charset=UTF-8";
        //        HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
        //        sr = new StreamReader(Response.GetResponseStream());
        //        rdjson = sr.ReadToEnd();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        //sr.Close();
        //    }

        //    return rdjson;
        //}
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
            // Get all Survey Questions
            if (Mode == "BindData")
            {
                MemberEntity objUserDeialscCheck = new MemberEntity();
                writer.Write(JsonConvert.SerializeObject(objUserDeialscCheck));
            }

            if (Mode == "rivercallback")
            {
                RiverManager oRiverManager = new RiverManager();
                string PostBackURL = string.Empty;
                PostBackURL = CallbackURL.Replace("|", "&");
                if (CallbackTypeId == 2)
                {
                    if (!string.IsNullOrEmpty(CallbackURL))
                    {
                        try
                        {
                            ASCIIEncoding encoding = new ASCIIEncoding();
                            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(PostBackURL);
                            LoginRequest.Method = "GET";
                            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
                            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
                            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
                            String strMainPage = sr.ReadToEnd();
                            sr.Close();
                            oRiverManager.InsertPostbackTransaction(PostBackURL, UserGUID, UserInvitationGuid, strMainPage);
                        }
                        catch (Exception ex)
                        {
                            oRiverManager.InsertPostbackTransaction(PostBackURL, UserGUID, UserInvitationGuid, ex.ToString());
                        }
                    }
                }
                else
                {
                    oRiverManager.InsertPostbackTransaction(PostBackURL, UserGUID, UserInvitationGuid, null);
                }

            }
            else if (Mode == "CheckEmailAddress")
            {
                string errormessage = string.Empty;
                string lblMessageBt = string.Empty;
                UserManager oUserManager = new UserManager();
                //var icount = oUserManager.EmailAddressCheck(Email);

                try
                {
                    //"https://api.briteverify.com/emails/verify.xml?email[address]=" + txtEmailAddress.Text + "&apikey=164aeab9-e759-4559-9b46-00deaf6cd5f5"
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    errormessage = PostRequest("https://www.xverify.com/services/emails/verify/?email=" + Email + "&type=xml&apikey=" + ConfigurationManager.AppSettings["Xveirfy"].ToString() + "&domain=surveydownline.com");
                    if (errormessage.Contains(@">valid</status>") || errormessage.Contains(@">unknown</status>"))
                    {
                        Session["message"] = "accepted";
                        lblMessageBt = "accepted";

                    }
                    else
                    {
                        Session["message"] = "rejected";
                        lblMessageBt = "rejected";
                        // txtEmailAddress.Focus();
                    }
                }
                catch (Exception ex)
                {
                    Session["message"] = "rejected";
                    lblMessageBt = "rejected";
                    throw (ex);
                }

                var Pagedata = lblMessageBt;
                writer.Write(JsonConvert.SerializeObject(Pagedata));
            }


            else if (Mode == "GetMemberDetails")
            {
                MemberEntity objUserDeialscCheck = new MemberEntity();
                RiverManager oRiverManager = new RiverManager();
                objUserDeialscCheck = oRiverManager.GetMemberDetails(UserGUID);
                writer.Write(JsonConvert.SerializeObject(objUserDeialscCheck));
            }
            else if (Mode == "GetCountryandStates")
            {
                CountryAndState objCountriesandStates = new CountryAndState();
                OpinionPartnerManager objManger = new OpinionPartnerManager();
                var Pagedata = objManger.GetCountrysAndStates("english");
                writer.Write(JsonConvert.SerializeObject(Pagedata));
            }

            else if (Mode == "GetEthinicity")
            {
                Ethnicity objEthinicity = new Ethnicity();
                OpinionPartnerManager objManger = new OpinionPartnerManager();
                var Pagedata = objManger.GetEthinicity("english");
                writer.Write(JsonConvert.SerializeObject(Pagedata));

            }
            else if (Mode == "SaveUserDetails")
            {
                MemberEntity oMemberEntity = new MemberEntity();
                RiverManager oRiverManager = new RiverManager();
                System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                oMemberEntity = jsonSerializer.Deserialize<Members.PrecisionSample.Components.Entities.MemberEntity>(Request.Form[0]);
                oMemberEntity.Dob = DOB;
                string pagedata = oRiverManager.SaveUserDetails(oMemberEntity);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }

            else if (Mode == "InsertClickes")
            {
                User oUser = new User();
                RiverManager oRiverManager = new RiverManager();
                string ReferrerUrl = string.Empty;
                string IpAddress = string.Empty;
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
                ReferrerUrl = ReferrerUrl.Replace("?fn=", "");
                IpAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                if (TxId == "undefined")
                    oUser.ExtId = string.Empty;
                else
                    oUser.ExtId = TxId;
                if (TransactionId == "undefined")
                    oUser.TransactionId = string.Empty;
                else
                    oUser.TransactionId = TransactionId;
                if (Sid == "undefined")
                    oUser.SubId3 = string.Empty;
                else
                    oUser.SubId3 = Sid;
                var pagedata = oRiverManager.ClickInsert(Rid, oUser.SubId3, ReferrerUrl, IpAddress, oUser.ExtId, oUser.TransactionId);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }
            else if (Mode == "MemberExistence")
            {

                RiverManager oRiverManager = new RiverManager();
                User oUser = new User();
                oUser.EmailAddress = EmailAddress;
                oUser.Rid = Rid;
                if (TxId == "undefined")
                    oUser.ExtId = string.Empty;
                else
                    oUser.ExtId = TxId;
                if (TransactionId == "undefined")
                    oUser.TransactionId = string.Empty;
                else
                    oUser.TransactionId = TransactionId;
                oUser.CountryCode = GetCountry();
                string pagedata = oRiverManager.CheckMemberExistence(oUser);
                if (pagedata == null && pagedata == string.Empty)
                {
                    pagedata = oRiverManager.InsertUserDetails(oUser);
                    pagedata = pagedata.Split(';')[0];
                }
                WriteCookie(EmailAddress, Rid);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }


            else if (Mode == "FBInsertUserDetails")
            {
                User oUser = new User();
                oUser.FirstName = FirstName;
                oUser.LastName = LastName;
                oUser.EmailAddress = EmailAddress;
                oUser.Rid = Rid;
                if (TxId == "undefined")
                    oUser.ExtId = string.Empty;
                else
                    oUser.ExtId = TxId;
                if (TransactionId == "undefined")
                    oUser.TransactionId = string.Empty;
                else
                    oUser.TransactionId = TransactionId;
                if (Gender != string.Empty)
                    oUser.Gender = Gender.ToUpper();
                oUser.CountryCode = GetCountry();
                if (Sid == "undefined")
                    oUser.SubId3 = string.Empty;
                else
                    oUser.SubId3 = Sid;
                oUser.IpAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
                RiverManager oRiverManager = new RiverManager();
                oUser.SubId2 = Convert.ToString(oRiverManager.GetReferrerDetails(oUser.Rid, oUser.SubId3));
                string result = oRiverManager.InsertUserDetails(oUser);
                if (result != string.Empty && result != null)
                    WriteCookie(EmailAddress, Rid);
                writer.Write(JsonConvert.SerializeObject(result));

            }

            else if (Mode == "SaveRiddata")
            {
                RiverManager oRiverManager = new RiverManager();
                User objUser = new User();
                //objUser = ObjUserManager.GetUserData(UserGuid.ToString());
                objUser.UserGuid = UserGUID;
                objUser.RelevantProfileScore = RelevantProfileScore;
                objUser.RelevantScore = RelevantScore;
                objUser.RelevantId = RelevantId;
                objUser.FpfScore = FpfScores;
                objUser.CleanIDJson = CleanID;
                if (IsNew.ToLower() == "true")
                    objUser.IsNew = "true";
                else
                    objUser.IsNew = "false";
                //string RequestURL = string.Empty;
                //RequestURL = System.Configuration.ConfigurationManager.AppSettings["ResearchDefender"].ToString();
                //RequestURL = string.Format(RequestURL, UserGUID, Project, QuotaGroupGuid);
                //string rdJson = GetrdJson(RequestURL, Token);
                //objUser.Rdjson = rdJson;

                //string verityId = string.Empty;
                //int verityScore = 0;
                //int GeoCorrelationFlag = 0;
                //if (RelevantScore > 30)
                //{
                //    //If the Member fails the relevant and to insert all Verity values as NULL , so we need to oass -1 to Data Server lable and pass DBNULL.Value
                //    //Other wise it stores 0 in Verity Score{ with calling API , we chould not assign 0 Value }
                //    objUser.VerityId = verityId;
                //    objUser.VerityScore = -1;
                //    objUser.GeoCorrelationFlag = -1;
                oRiverManager.UpdateRelevantandVerityData(objUser);
                //}
                //else
                //{
                //if (objUser.Is_verity_required && _referrerId == -1)
                //{
                //    //Added on 07/18/2014 for verity implementation on registration page
                //    if (objUser.CountryId == 231) //Verity is Only For USA Members
                //    {
                //        com.imperium.verity.RespSvc client = new SurveyDownLine.Web.com.imperium.verity.RespSvc();
                //        com.imperium.verity.ValidationResultsPlus8 vr = new SurveyDownLine.Web.com.imperium.verity.ValidationResultsPlus8();

                //        com.imperium.verity.AuthHeader authentication = new SurveyDownLine.Web.com.imperium.verity.AuthHeader();
                //        authentication.Username = ConfigurationManager.AppSettings["VerityUserName"].ToString();
                //        authentication.Password = ConfigurationManager.AppSettings["VerityPassword"].ToString();
                //        client.AuthHeaderValue = authentication;
                //        //Call the Verity+ webservice and receive response
                //        if (!string.IsNullOrEmpty(objUser.FirstName) && !string.IsNullOrEmpty(objUser.LastName) &&
                //           !string.IsNullOrEmpty(objUser.Address1) && !string.IsNullOrEmpty(objUser.ZipCode) && objUser.Dob.ToString("yyyyMM") != "190001")
                //        {
                //            vr = client.ValidateDataPlus8(ConfigurationManager.AppSettings["VerityClientId"].ToString(),
                //                objUser.FirstName, objUser.LastName, objUser.Address1, objUser.Address2, objUser.ZipCode, objUser.CountryCode.Trim(),
                //                objUser.Dob.ToString("yyyyMM"), "", "U", "N", "N", Request.ServerVariables["REMOTE_ADDR"].ToString());
                //            verityScore = Convert.ToInt32(vr.Score);
                //            verityId = vr.VID;
                //            try
                //            {
                //                if (!string.IsNullOrEmpty(vr.GeoCorrelationFlag))
                //                    GeoCorrelationFlag = Convert.ToInt32(vr.GeoCorrelationFlag);
                //                else
                //                    GeoCorrelationFlag = -1;
                //            }
                //            catch
                //            {
                //                GeoCorrelationFlag = -1;
                //            }
                //            objUser.VerityId = verityId;
                //            objUser.VerityScore = verityScore;
                //            objUser.GeoCorrelationFlag = GeoCorrelationFlag;
                //            //update member relevant & verity score
                //            ObjUserManager.UpdateRelevantandVerityData(objUser);
                //        }
                //        else
                //        {
                //            //If we do not have the member demogrpahcis collected on Landing page, even thouh we have required feilds,
                //            //then we need to mark them fraud.
                //            objUser.VerityId = verityId;
                //            objUser.VerityScore = -1;
                //            objUser.GeoCorrelationFlag = -1;
                //            ObjUserManager.UpdateRelevantandVerityData(objUser);
                //        }
                //    }
                //    else
                //    {
                //        //For Non US Members, Do not Make Verity Call, so Pass all valid Verity Scores, so that we can handle in SP.
                //        //If we pass null values member will be marked as fraud.
                //        objUser.VerityId = verityId;
                //        objUser.VerityScore = 5;
                //        objUser.GeoCorrelationFlag = 1;
                //        ObjUserManager.UpdateRelevantandVerityData(objUser);
                //    }
                //}
                //else
                //{
                //    //If we pass null values member will be marked as fraud.
                //    objUser.VerityId = verityId;
                //    objUser.VerityScore = 5;
                //    objUser.GeoCorrelationFlag = 1;
                //    ObjUserManager.UpdateRelevantandVerityData(objUser);
                //}
                //}
            }

            else if (Mode == "GetEmailFromCoockie")
            {
                var pagedata = GetEmailfromcookie();
                writer.Write(JsonConvert.SerializeObject(pagedata));

            }

            if (Mode == "UpdateUserInvitationDetails")
            {
                RiverManager oRiverManager = new RiverManager();
                var pagedata = oRiverManager.UpdateUserInvitationDetails(UserStatusGuid, UserInvitationGuid);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }

            else if (Mode == "GetProjectDetails")
            {
                RiverManager oRiverManager = new RiverManager();
                var pagedata = oRiverManager.GetProjectDetails(UserGUID);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }
            else if (Mode == "InsertUserInvitation")
            {
                int _isnew = 0;
                int _user_traffic_type = 2;
                RiverManager oRiverManager = new RiverManager();
                Components.Entities.River oRiver = new Components.Entities.River();
                string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                int _relevantflag = Convert.ToInt32(ConfigurationManager.AppSettings["RiverRelevantCheckFlag"]);
                int _ip2countryflag = Convert.ToInt32(ConfigurationManager.AppSettings["RiverIp2CounterCheckFlag"]);
                string u = string.Empty;
                u = Request.ServerVariables["HTTP_USER_AGENT"];
                Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (u != null)
                {
                    if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                    {
                        //If Mobile Device Matched:
                        _user_traffic_type = 2;
                    }
                    else
                    {
                        //If Non Mobile Device Matched.
                        _user_traffic_type = 3;
                    }
                }
                else //If the Request.ServerVariables is NULL 
                {
                    if (Request.UserAgent != null)
                    {
                        u = Request.UserAgent;
                        if (Request.UserAgent.Contains("Android")
                         || Request.UserAgent.Contains("webOS")
                         || Request.UserAgent.Contains("iPhone")
                         || Request.UserAgent.Contains("iPad")
                         || Request.UserAgent.Contains("iPod")
                         || Request.UserAgent.Contains("BlackBerry")
                         || Request.UserAgent.Contains("Windows Phone"))
                        {
                            //If the Mobile user Paticiapting the Survey
                            _user_traffic_type = 2;
                        }
                        else
                        {
                            //If the Non Mobile user is participating in Survey.
                            _user_traffic_type = 3;
                        }
                    }
                }
                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                string browser_info = string.Empty;
                if (browser != null)
                {
                    browser_info = "Type = " + browser.Type + "|"
                        + "Name = " + browser.Browser + "|"
                        + "Version = " + browser.Version + "|"
                        + "Major Version = " + browser.MajorVersion + "|"
                        + "Minor Version = " + browser.MinorVersion + "|"
                        + "Platform = " + browser.Platform;
                }

                string mobiledevice = string.Empty;
                //Added on 9/12/2014 to save the Mobile Device Model.
                if (Request.UserAgent.Contains("Android"))
                {
                    mobiledevice = "Android";
                }
                else if (Request.UserAgent.Contains("webOS"))
                {
                    mobiledevice = "webOS";
                }
                else if (Request.UserAgent.Contains("iPhone"))
                {
                    mobiledevice = "iPhone";
                }
                else if (Request.UserAgent.Contains("iPad"))
                {
                    mobiledevice = "iPad";
                }
                else if (Request.UserAgent.Contains("iPod"))
                {
                    mobiledevice = "iPod";
                }
                else if (Request.UserAgent.Contains("BlackBerry"))
                {
                    mobiledevice = "BlackBerry";
                }
                else if (Request.UserAgent.Contains("Windows Phone"))
                {
                    mobiledevice = "Windows Phone";
                }
                if (IsNew.ToLower() == "true")
                    _isnew = 1;
                else
                    _isnew = 0;
                //string RequestURL = string.Empty;
                //RequestURL = System.Configuration.ConfigurationManager.AppSettings["ResearchDefender"].ToString();
                //RequestURL = string.Format(RequestURL, UserGUID, Project, QuotaGroupGuid, GeoLonLat, PassParam);
                //string Json = GetrdJson(RequestURL, Token);
                oRiver = oRiverManager.InsertUserInvitaiton(UserGUID, QuotaGroupGuid, RelevantId, RelevantScore, FpfScores, ip, browser_info, u, _isnew, _relevantflag, _ip2countryflag, _user_traffic_type, mobiledevice, CleanID);
                //inserting into activity table
                oRiverManager.SurveyActivityInsert(oRiver);
                writer.Write(JsonConvert.SerializeObject(oRiver.RedirectUrl));
            }

            else if (Mode == "GetSurveyUrl")
            {
                RiverManager oRiverManager = new RiverManager();
                var pagedata = oRiverManager.GetSurveyUrl(UserInvitationGuid);
                writer.Write(JsonConvert.SerializeObject(pagedata));
            }

        }


        #endregion


        //Read From Cookie
        public string GetEmailfromcookie()
        {
            string email_address = string.Empty;
            string sCookieName = "FandFAccountName";
            HttpCookie cookie = Request.Cookies.Get(sCookieName);
            if (cookie != null)
            {
                string sAccountName = cookie.Values["AccountName"].ToString();
                if (!string.IsNullOrEmpty(sAccountName))
                {
                    email_address = sAccountName;
                }
            }
            return email_address;
        }


        //Write a Cookie:
        //Then Show Email Text Box, on Conitnue Click
        public void WriteCookie(string email_address_text_box, int rid)
        {
            string email_address = string.Empty;
            string sCookieName = "FandFAccountName";
            HttpCookie accountNameCookie = new HttpCookie(sCookieName);
            Response.Cookies.Remove(sCookieName);
            Response.Cookies.Add(accountNameCookie);
            accountNameCookie.Values.Add("AccountName", email_address_text_box);
            accountNameCookie.Values.Add("Refferer", Convert.ToString(Rid));
            email_address = email_address_text_box;
        }

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

        public string GetCountry()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            string country = string.Empty;
            UserManager objMnager = new UserManager();
            return country = objMnager.GetCountryForIp(ip);
        }

        #region Post data

        /// <summary>
        /// </summary>
        /// <param name="RequestURL"></param>
        /// <param name="LoginCredentials"></param>
        /// <returns></returns>

        public string PostRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            //string postData = LoginCredentials;
            //byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            //LoginRequest.ContentType = "application/x-www-form-urlencoded";
            //LoginRequest.ContentLength = data.Length;
            //Stream LoginRequestStream = LoginRequest.GetRequestStream();
            //LoginRequestStream.Write(data, 0, data.Length);
            //LoginRequestStream.Close();

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        #endregion

    }
}
