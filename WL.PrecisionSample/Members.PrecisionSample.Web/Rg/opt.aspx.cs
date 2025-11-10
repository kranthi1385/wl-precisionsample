using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Common.Utils;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net;
using System.IO;

namespace Members.PrecisionSample.Web.Registration
{
    public partial class opt : System.Web.UI.Page
    {
        #region public variables
        /// <summary>
        /// public variables
        /// </summary>
        public bool _isbvEmail = false;
        public bool _isbvAddress = false;
        #endregion

        #region public methods
        /// <summary>
        /// public methods
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
        /// landing page
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
        /// page events
        /// </summary>
        /// <param name="sender">sender</param>
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
                if (Landingpage == "2step")
                {
                }
                else
                {
                    //                    litPixelScript.Text = @"<script language=""JavaScript"" type=""text/javascript"">
                    //                 function cpaDP(){
                    //                 var ifrm = document.createElement(""IFRAME"");   
                    //                 ifrm.setAttribute(""src"",""http://www.securepaths.com/pixel.cgi?s=" + oUser.HitId.ToString() + "&p=" +
                    //                     oUser.RefferId.ToString() + "&a=" + oUser.SubId2.ToString() + "&cmp=surveydownline&org=hd2d92c5478813a7303b&rt=3_saleJS&stId=" +
                    //                     oUser.UserId.ToString() + @"&dem1=""" + " +escape('" + oUser.FirstName + "') +" + @"""" + @"&dem2=""" + " +escape('" + oUser.LastName + "') +" +
                    //                    @"""" + @"&dem3=""" + " +escape('" + oUser.ZipCode + "') +" + @"""" + @"&c1=""" + " +escape('" + Landingpage + "') +" + @"""" + @"&c2="""
                    //                     + " +escape('" + oUser.EmailAddress + "') +" + @"""" + @"&c3=""" + " +escape('" + oUser.Password + "') +" + @"""" + @"&c4=""" + " +escape('" + oUser.Address1 + "'));" + @"
                    //
                    //                    ifrm.style.width = 1+""px""; ifrm.style.height = 1+""px""; ifrm.frameBorder = 0;
                    //                 document.body.appendChild(ifrm);
                    //                                }
                    //                    cpaDP();
                    //                    </script>
                    //                <noscript>
                    //                <img src=""http://www.securepaths.com/pixel.cgi?s=" + oUser.HitId.ToString() + "&p=" + oUser.RefferId.ToString() + "&a=" + oUser.SubId2 + "&cmp=surveydownline&stId=" + oUser.UserId.ToString() + "&org=hd2d92c5478813a7303b&rt=5_saleIMG" + "&dem1=" + Server.UrlEncode(oUser.FirstName) + "&dem2=" + Server.UrlEncode(oUser.LastName) + "&dem3=" + Server.UrlEncode(oUser.ZipCode) + "&c1=" + Server.UrlEncode(Landingpage) + "&c2=" + Server.UrlEncode(oUser.EmailAddress) + "&c3=" + Server.UrlEncode(oUser.Password) + "&c4=" + Server.UrlEncode(oUser.Address1) + @""">
                    //                            </noscript>";

                }
                //if (oUser.Country2Ip == "US")
                //{
                //    Response.Redirect("/rg/np2.aspx?ug=" + UserGuid.ToString() + "&lpage=" + Landingpage);
                //}
                //else if (oUser.Country2Ip == "AU" || oUser.Country2Ip == "UK" || oUser.Country2Ip == "CA" || oUser.Country2Ip == "OI" || string.IsNullOrEmpty(oUser.Country2Ip))
                //{
                //}
                //else
                //{
                //    Response.Redirect("/misc/thankyou.aspx?ug=" + UserGuid.ToString());
                //}

                if (oUser.Country2Ip == "US" || oUser.Country2Ip == "AU" || oUser.Country2Ip == "UK" || oUser.Country2Ip == "CA" || oUser.Country2Ip == "OI" || string.IsNullOrEmpty(oUser.Country2Ip))
                {
                    if (oUser.CountryId == 231)
                    {
                        Response.Redirect("/rg/np2.aspx?ug=" + UserGuid.ToString() + "&lpage=" + Landingpage);
                    }
                }
                else
                {
                    Response.Redirect("/misc/thankyou.aspx?ug=" + UserGuid.ToString());
                }
                string Fulladdress = string.Empty;

                litopt.Text = @"<script type=""text/javascript"" 
                            src=""http://www.clear-request.com/oi/second/12409734.js" + "\"></script>";
                //                if (Convert.ToInt32(oUser.RefferId) == 72)
                //                {
                //                    litopt.Text = @"<script type=""text/javascript"" 
                //                            src=""http://www.clear-request.com/oi/second/14377214.js" + "\"></script>";
                //                }
                //                else
                //                {
                //                    litopt.Text = @"<script type=""text/javascript"" 
                //                            src=""http://www.clear-request.com/oi/second/12409734.js" + "\"></script>";
                //                }
                hfShow1.Value = oUser.OptTiburonSplitFlag.ToString();
                hfcountry.Value = oUser.CountryId.ToString();
                string _address = oUser.Address1;
                string[] s = @"<,>,#,%,{,},|,\,^,~,[,],`".Split(',');
                for (int i = 0; i <= s.Length - 1; i++)
                {
                    if (_address.Contains(s[i]))
                    {
                        _address = _address.Replace(s[i], "");
                    }
                }

                /*code added on 4/18/2011*/
                if (oUser.RefferId == 72 || oUser.RefferId == 323 || oUser.RefferId == 359 || oUser.RefferId == 363 || oUser.RefferId == 368 || oUser.RefferId == 369)
                {
                    lit1.Text = @"<script type=""text/javascript"" 
                                                                    src=""http://dm.tmginteractive.com/JSCoReg/surveydownline/SurveydownlineMultiOffers.aspx?id=547128&first_name=" + oUser.FirstName + "&last_name=" + oUser.LastName + "&dob=" + oUser.DOB.ToString("yyyy-MM-dd") + "&email=" + oUser.EmailAddress +
                                "&phone=" + oUser.PhoneNumber + "&state=" + oUser.StateCode + "&zipcode=" + oUser.ZipCode + "&country=" + oUser.CountryCode.Trim() + "&gender=" + oUser.Gender + "&pub=" + oUser.UserGuid.ToString() + "&subpub=" + "&address=" + _address + "&city=" + oUser.City + "\"></script>";
                }
                else
                {
                    lit1.Text = @"<script type=""text/javascript"" 
                                                                    src=""http://dm.tmginteractive.com/JSCoReg/surveydownline/SurveydownlineMultiOffers.aspx?id=175552&first_name=" + oUser.FirstName + "&last_name=" + oUser.LastName + "&dob=" + oUser.DOB.ToString("yyyy-MM-dd") + "&email=" + oUser.EmailAddress +
                                "&phone=" + oUser.PhoneNumber + "&state=" + oUser.StateCode + "&zipcode=" + oUser.ZipCode + "&country=" + oUser.CountryCode.Trim() + "&gender=" + oUser.Gender + "&pub=" + oUser.UserGuid.ToString() + "&subpub=" + "&address=" + _address + "&city=" + oUser.City + "\"></script>";
                }

                if (oUser.OptTiburonSplitFlag == 1)
                {
                    divTurbo.Visible = false;
                    divopt.Visible = true;
                }
                else
                {
                    divTurbo.Visible = true;
                    divopt.Visible = true;
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
                            if (lstATE[i].FirePixel > 0 && lstATE[i].PixelTypeId == 1) //Fire SOI Pixel only this page.
                            {
                                //Fire Tp only members from US/UK/CA/AUS only
                                UserManager oUserManager = new UserManager();
                                string countrycode = oUserManager.GetCountryForIp(Request.ServerVariables["REMOTE_ADDR"].ToString());
                                if (countrycode == "US" || countrycode == "AU" || countrycode == "UK" || countrycode == "CA" || countrycode == "O1")
                                {
                                    string pixelurl = Code.PEReplaceMents(lstATE[i].TrackingDetails, oUser, oUser.SubId2);
                                    if (lstATE[i].TrackingType == 'I')
                                    {
                                        ltlAffiliateTracking.Text = pixelurl;
                                    }
                                    if (lstATE[i].TrackingType == 'J')
                                    {
                                        Page.RegisterClientScriptBlock("MyScript", pixelurl);
                                    }

                                    //Update SOI Pixel fired stauts 
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
        /// page prerender
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //    this.Form.Attributes.Add("onsubmit", "return oi_send();");
        }
        #endregion

        #region Button Events
        /// <summary>
        /// image click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void img1_Click(object sender, ImageClickEventArgs e)
        {
            User oUser = new User();
            oUser.UserId = oUser.UserId;
            oUser.RegistrationStep = "C";
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
                // Response.Redirect("/rg/page3.aspx?ug=" + UserGuid.ToString());

                /*Code added on 4/7/2011 donasked to redirect it from page 2it self for AUS members*/
                //if (UserDetails.CountryId == 15)
                //{
                //    string dob1 = UserDetails.Dob.ToString("yyyy-MM-dd");
                //    string dob2 = UserDetails.Dob.ToString("MM/dd/yyyy");
                //    //showing 100% smiley media to other than USA members.
                //    Response.Redirect("http://ian.smileymedia.com/r2/?placementID=surveydownline" +
                //               "&email=" + UserDetails.EmailAddress + "&fname=" + UserDetails.FirstName +
                //               "&lname=" + UserDetails.LastName + "&gender=" + UserDetails.Gender + "&dob=" + dob1 +
                //               "&addr=" + UserDetails.Address1 + "&addr2=" + UserDetails.Address2 + "&city=" + UserDetails.City +
                //                "&state=" + UserDetails.StateCode + "&zip=" + UserDetails.ZipCode +
                //                "&hphone=" + UserDetails.PhoneNumber + "&mphone=" + UserDetails.PhoneNumber +
                //               "&country=" + UserDetails.CountryCode + "&pubSubID=" + UserDetails.UserId + "&destURL=" + ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + UserDetails.UserGuid.ToString());
                //}
                //else
                //{
                //    Response.Redirect("/rg/optpage3.aspx?ug=" + UserDetails.UserGuid.ToString());
                //}
                /*end Comments*/

                /*code commented on 4/7/2011 to show page3 offers */
                //string dob1 = UserDetails.Dob.ToString("yyyy-MM-dd");
                //string dob2 = UserDetails.Dob.ToString("MM/dd/yyyy");
                //if (UserDetails.CountryId == 231)
                //{
                //    // showing ifficient 100 percent to US members added on 11/12/2010
                //    Response.Redirect("http://magnumapi.ifficient.com/serve.aspx?" +
                //     "email=" + UserDetails.EmailAddress + "&first=" + UserDetails.FirstName +
                //             "&last=" + UserDetails.LastName + "&gender=" + UserDetails.Gender + "&dob=" + dob2 +
                //             "&add1=" + UserDetails.Address1 + "&addr2=" + UserDetails.Address2 + "&city=" + UserDetails.City +
                //              "&state=" + UserDetails.StateCode + "&zip=" + UserDetails.ZipCode + "&istest=false" +
                //              "&phone=" + UserDetails.PhoneNumber + "&country=" + UserDetails.CountryCode + "&pubid=1376&rurl=" + ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + UserDetails.UserGuid.ToString());
                //}
                //else
                //{
                //    //showing 100% smiley media to other than USA members.
                //    Response.Redirect("http://ian.smileymedia.com/r2/?placementID=surveydownline" +
                //               "&email=" + UserDetails.EmailAddress + "&fname=" + UserDetails.FirstName +
                //               "&lname=" + UserDetails.LastName + "&gender=" + UserDetails.Gender + "&dob=" + dob1 +
                //               "&addr=" + UserDetails.Address1 + "&addr2=" + UserDetails.Address2 + "&city=" + UserDetails.City +
                //                "&state=" + UserDetails.StateCode + "&zip=" + UserDetails.ZipCode +
                //                "&hphone=" + UserDetails.PhoneNumber + "&mphone=" + UserDetails.PhoneNumber +
                //               "&country=" + UserDetails.CountryCode + "&pubSubID=" + UserDetails.UserId + "&destURL=" + ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + UserDetails.UserGuid.ToString());
                //}
                /*end commented */
            }
        }
        #endregion

        #region Insert the posted UserId
        /// <summary>
        /// Insert User
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
        /// post request
        /// </summary>
        /// <param name="RequestURL">requesturl</param>
        /// <param name="LoginCredentials">logincredentials</param>
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
    }
}