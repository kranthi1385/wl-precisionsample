using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Common.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Members.PrecisionSample.Web.Registration
{
    public partial class np2 : System.Web.UI.Page
    {
        #region public variables
        /// <summary>
        /// Public Variables
        /// </summary>
        public bool _isbvEmail = false;
        public bool _isbvAddress = false;
        #endregion

        #region public methods
        /// <summary>
        /// Public methods
        /// </summary>
        private Guid UserGuid
        {
            get
            {
                if (Request.Params["ug"] != null)
                {
                    return new Guid(Request.Params["ug"].ToString());
                }
                return Guid.Empty;
            }
        }
        /// <summary>
        /// Landing Page
        /// </summary>
        private string Landingpage
        {
            get
            {
                if (Request.Params["lpage"] != null)
                {
                    return Request.Params["lpage"].ToString();
                }
                return string.Empty;
            }
        }
        #endregion

        #region Page Events
        /// <summary>
        /// Page Events
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string message = string.Empty;
                string errormessage = string.Empty;
                string errormessage1 = string.Empty;
                User oUser = new User();
                UserManager oManager = new UserManager();
                oUser = oManager.GetUserData(UserGuid.ToString());
                aspnetForm.Action = "/rg/optpage3.aspx?ug=" + UserGuid.ToString() + "&lpage=" + Landingpage;
                // aspnetForm.Action = "/rg/page3.aspx?ug=" + UserGuid.ToString() + "&lpage=" + Landingpage;
                //Code For AdQuire Snippet
                //if (!string.IsNullOrEmpty(oUser.PhoneNumber) && oUser.PhoneNumber.Replace("-", "").Replace(" ", "").Length >= 10)
                //{
                string rurl = "http://www.surveydownline.com/Rg/optpage3.aspx?ug=" + oUser.UserGuid + "&lpage=" + Landingpage;
                //litScript.Text = @"<script language=""JavaScript"" type=""text/javascript"">
                litScript.Text = @"<script type=""text/javascript"" src=""http://ldsapi.tmginteractive.com/generateplacementscript.aspx?placement=26422800&publisher=817785" +
                                                             "&affid=" + oUser.RefferId +
                                                             "&subid=" + UserGuid.ToString() +
                                                             "&firstname=" + oUser.FirstName +
                                                             "&lastname=" + oUser.LastName +
                                                             "&email=" + oUser.EmailAddress +
                                                             "&address=" + oUser.Address1 +
                                                             "&address2=" + oUser.Address2 +
                                                             "&city=" + oUser.City +
                                                             "&state=" + oUser.StateName +
                                                             "&zip=" + oUser.ZipCode +
                                                             "&phone=" + oUser.PhoneNumber +
                                                             "&dob=" + oUser.DOB +
                                                             "&gender=" + oUser.Gender +
                                                             "&ethnicity=" + oUser.EthinicityType +
                                                             "&redirect=" + rurl + @"""></script>";

                //                       litScript.Text = @"<script language=""JavaScript"" type=""text/javascript"">
                //                        var PDregData = {};
                //                        PDregData.pub_key =" + "'" + "User-" + UserGuid.ToString() + "'" + ";" +
                //                        "PDregData.SubMid =" + "'" + oUser.RefferId.ToString() + "'" + ";" +
                //                        "PDregData.first_name =" + "'" + oUser.FirstName.ToString() + "'" + ";" +
                //                        "PDregData.last_name=" + "'" + oUser.LastName.ToString() + "'" + ";" +
                //                        "PDregData.gender=" + "'" + oUser.Gender.ToString() + "'" + ";" +
                //                        "PDregData.dob=" + "'" + oUser.Dob.ToString("MM-dd-yyyy") + "'" + ";" +
                //                        "PDregData.address1=" + "'" + oUser.Address1 + "'" + ";" +
                //                        "PDregData.address2=" + "'" + oUser.Address2 + "'" + ";" +
                //                        "PDregData.city=" + "'" + oUser.City + "'" + ";" +
                //                        "PDregData.state=" + "'" + oUser.StateCode + "'" + ";" +
                //                        "PDregData.zipcode=" + "'" + oUser.ZipCode + "'" + ";" +
                //                        "PDregData.phone1=" + "'" + oUser.PhoneNumber.Substring(0, 3) + "'" + ";" +
                //                        "PDregData.phone2=" + "'" + oUser.PhoneNumber.Substring(3, 3) + "'" + ";" +
                //                        "PDregData.phone3=" + "'" + oUser.PhoneNumber.Substring(6, 4) + "'" + ";" +
                //                        "PDregData.email=" + "'" + oUser.EmailAddress + "'" + ";" +
                //                    " </script>";
                //}
                //                else
                //                {
                //                    litScript.Text = @"<script language=""JavaScript"" type=""text/javascript"">
                //                        var PDregData = {};
                //                        PDregData.pub_key =" + "'" + "User-" + UserGuid.ToString() + "'" + ";" +
                //                        "PDregData.SubMid =" + "'" + oUser.RefferId.ToString() + "'" + ";" +
                //                        "PDregData.first_name =" + "'" + oUser.FirstName.ToString() + "'" + ";" +
                //                        "PDregData.last_name=" + "'" + oUser.LastName.ToString() + "'" + ";" +
                //                        "PDregData.gender=" + "'" + oUser.Gender.ToString() + "'" + ";" +
                //                        "PDregData.dob=" + "'" + oUser.Dob.ToString("MM-dd-yyyy") + "'" + ";" +
                //                        "PDregData.address1=" + "'" + oUser.Address1 + "'" + ";" +
                //                        "PDregData.address2=" + "'" + oUser.Address2 + "'" + ";" +
                //                        "PDregData.city=" + "'" + oUser.City + "'" + ";" +
                //                        "PDregData.state=" + "'" + oUser.StateCode + "'" + ";" +
                //                        "PDregData.zipcode=" + "'" + oUser.ZipCode + "'" + ";" +
                //                       "PDregData.email=" + "'" + oUser.EmailAddress + "'" + ";" +
                //                   " </script>";
                //                }
                //End Lines of Code


                string Fulladdress = string.Empty;
                //                if (Landingpage == "2step")
                //                {
                //                }
                //                else
                //                {
                //                    litPixelScript.Text = @"<script language=""JavaScript"" type=""text/javascript"">
                //             function cpaDP(){
                //             var ifrm = document.createElement(""IFRAME"");   
                //             ifrm.setAttribute(""src"",""http://www.securepaths.com/pixel.cgi?s=" + oUser.HitId.ToString() + "&p=" +
                //                 oUser.RefferId.ToString() + "&a=" + oUser.SubId2.ToString() + "&cmp=surveydownline&org=hd2d92c5478813a7303b&rt=3_saleJS&stId=" +
                //                 oUser.UserId.ToString() + @"&dem1=""" + " +escape('" + oUser.FirstName + "') +" + @"""" + @"&dem2=""" + " +escape('" + oUser.LastName + "') +" +
                //                @"""" + @"&dem3=""" + " +escape('" + oUser.ZipCode + "') +" + @"""" + @"&c1=""" + " +escape('" + Landingpage + "') +" + @"""" + @"&c2="""
                //                 + " +escape('" + oUser.EmailAddress + "') +" + @"""" + @"&c3=""" + " +escape('" + oUser.Password + "') +" + @"""" + @"&c4=""" + " +escape('" + oUser.Address1 + "'));" + @"
                //            ifrm.style.width = 1+""px""; ifrm.style.height = 1+""px""; ifrm.frameBorder = 0;
                //             document.body.appendChild(ifrm);
                //                            }
                //                cpaDP();
                //                </script>
                //            <noscript>
                //            <img src=""http://www.securepaths.com/pixel.cgi?s=" + oUser.HitId.ToString() + "&p=" + oUser.RefferId.ToString() + "&a=" + oUser.SubId2 + "&cmp=surveydownline&stId=" + oUser.UserId.ToString() + "&org=hd2d92c5478813a7303b&rt=5_saleIMG" + "&dem1=" + Server.UrlEncode(oUser.FirstName) + "&dem2=" + Server.UrlEncode(oUser.LastName) + "&dem3=" + Server.UrlEncode(oUser.ZipCode) + "&c1=" + Server.UrlEncode(Landingpage) + "&c2=" + Server.UrlEncode(oUser.EmailAddress) + "&c3=" + Server.UrlEncode(oUser.Password) + "&c4=" + Server.UrlEncode(oUser.Address1) + @""">
                //                        </noscript>";

                //                }
                if (oUser.Country2Ip == "US" || oUser.Country2Ip == "AU" || oUser.Country2Ip == "UK" || oUser.Country2Ip == "CA" || oUser.Country2Ip == "OI" || string.IsNullOrEmpty(oUser.Country2Ip))
                {
                }
                else
                {
                    Response.Redirect("/misc/thankyou.aspx?ug=" + UserGuid.ToString());
                }
                /*end method*/
                //Firing pixcel for the members not in 2step prcocss only.( split_flag = 0 for normal affilaite).
                //But this Changed we arefiring pixeles for 2 step affialites, also after completing the 2Step.
                if (oUser.SplitFlag == 0 && !oUser.IsFraud && oUser.Is_soi_pixel_fired == 0)
                {                                                   //.. | ..//
                    //Fire Pixel for only SOI Members , where the affilaite is of type SOI Cost Affialite added on 11/16/2011 by sandy.
                    //Fire Pixel for non fraud members
                    AffiliateTrackingManager oATM = new AffiliateTrackingManager();
                    List<AffiliateTrackingEntities> lstATE = new List<AffiliateTrackingEntities>();
                    lstATE = oATM.GetTrackingDetails(oUser.RefferId, oUser.UserId);
                    if (oUser.RefferId != -1 && oUser.RefferId != 368 && oUser.RefferId != 369 && oUser.RefferId != 14723)
                    {
                        for (int i = 0; i < lstATE.Count; i++)
                        {
                            if (lstATE[i].FirePixel > 0 && lstATE[i].PixelTypeId == 1)
                            {
                                //Fire Tp only members from US/UK/CA/AUS only
                                UserManager oUserManager = new UserManager();
                                string countrycode = oUserManager.GetCountryForIp(Request.ServerVariables["REMOTE_ADDR"].ToString());
                                if (countrycode == "US" || countrycode == "AU" || countrycode == "UK" || countrycode == "CA" || countrycode == "O1")
                                {
                                    string pixelurl = Code.PEReplaceMents(lstATE[i].TrackingDetails, oUser, oUser.SubId2);
                                    if (lstATE[i].CallbacktypeId == 1) //Pixel
                                    {
                                        if (lstATE[i].TrackingType == 'I')
                                        {
                                            ltlAffiliateTracking.Text = pixelurl;
                                        }
                                        if (lstATE[i].TrackingType == 'J')
                                        {
                                            Page.RegisterClientScriptBlock("MyScript", pixelurl);
                                        }
                                    }
                                    else if (lstATE[i].CallbacktypeId == 2) //Call back
                                    {
                                        PostRequest(pixelurl);
                                    }
                                    oUserManager.UpdateSOIPixelFiredStatusUpdate(oUser.UserId, oUser.RefferId);
                                }
                                else
                                {
                                    //Update is_fired =0 for the user becuase member is from non US/UK/CA/AUS
                                    oUserManager.UserIsFiredUpdate(oUser.UserId, "normal");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Page PreRender
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
        #endregion

        #region Button Events
        /// <summary>
        /// Button Events
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        /// 
        protected void img1_Click(object sender, ImageClickEventArgs e)
        {
            User oUser = new User();
            UserManager oUserManager = new UserManager();
            oUserManager.GetUserData(UserGuid.ToString());
            oUser.UserId = oUser.UserId;
            oUser.RegistrationStep = "B";
            UserManager omanger = new UserManager();
            omanger.UserRegistrationStepUpdate(oUser);
            if (oUser.RefferId == 82 || oUser.RefferId == 83 || oUser.RefferId == 125)
            {
                oUser.RegistrationStep = "F";
                omanger.UserRegistrationStepUpdate(oUser);
                Response.Redirect("/Rg/profile.aspx?pid=" + ConfigurationManager.AppSettings["BasicProfileGuid"].ToString() + "&ug=" + oUser.UserGuid.ToString());

            }
            else
            {
                Response.Redirect("/rg/optpage3.aspx?ug=" + UserGuid.ToString());
                // Response.Redirect("/rg/page3.aspx?ug=" + UserGuid.ToString()); //--Checking for stage.
            }
        }
        #endregion

        #region Insert the posted UserId

        /// <summary>
        /// Insert Pasted UserId
        /// </summary>
        /// <param name="Userid">UserId</param>
        public void InserUser(int Userid)
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            try
            {
                SqlCommand cm = new SqlCommand("[user].[AllInboxUsers_Insert]", cn);
                cn.Open();
                cm.CommandType = CommandType.StoredProcedure;
                cm.Parameters.AddWithValue("@user_id", Userid);
                cm.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region Post data

        /// <summary>
        /// post data
        /// </summary>
        /// <param name="RequestURL">Requesturl</param>
        /// <param name="LoginCredentials">LoginCredentials</param>
        /// <returns></returns>
        public string PostRequest(string RequestURL, string LoginCredentials)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/x-www-form-urlencoded";
            LoginRequest.ContentLength = data.Length;
            Stream LoginRequestStream = LoginRequest.GetRequestStream();
            LoginRequestStream.Write(data, 0, data.Length);
            LoginRequestStream.Close();

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }
        #endregion

        #region Help methods
        /// <summary>
        /// Help methods
        /// </summary>
        /// <param name="RequestURL">Requesturl</param>
        /// <returns></returns>
        public string PostRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";

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