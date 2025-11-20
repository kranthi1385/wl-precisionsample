using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Data_Layer;
using System.Configuration;

namespace Members.PrecisionSample.Web.Registration
{
    public partial class optpage3 : System.Web.UI.Page
    {
        #region public methods
        /// <summary>
        /// User Guid
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
        #endregion

        #region Page Load
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User oUser = new User();
                string url = string.Empty;
                UserManager oUserManager = new UserManager();
                oUser = oUserManager.GetUserData(UserGuid.ToString());
                url = GetUrl1();
                if (oUser.CountryId == 231)
                {
                    string s = string.Empty;
                    s = @"<iframe id=""iff-parentiframe"" src=" + @"""" + url + @""" runat=""server"" height=""300px"" width=""800px""   scrolling=""no"" frameborder=""0"" ></iframe> ";
                    lit1.Text = s;
                    //img1.Visible = true;
                }
                else
                {
                    //Need to Redirect direclty to Offers page.
                    Response.Redirect(url);
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
        }
        #endregion

        #region Button Events
        /// <summary>
        /// image2 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void img2_Clcik(object sender, ImageClickEventArgs e)
        {
            User oUser = new User();
            oUser.UserId = oUser.UserId;
            oUser.RegistrationStep = "D";
            UserManager oManger = new UserManager();
            oManger.UserRegistrationStepUpdate(oUser);
            Response.Redirect("/Rg/Offers.aspx?ug=" + oUser.UserGuid.ToString());
        }
        #endregion

        #region GetUrl1
        /// <summary>
        /// get url1
        /// </summary>
        /// <returns></returns>
        private string GetUrl1()
        {
            string url = string.Empty;

            User oUser = new User();
            oUser.UserId = oUser.UserId;
            //oUser.RegistrationStep = "B";
            //UserManager omanger = new UserManager();
            //omanger.UserRegistrationStepUpdate(oUser);
            string phone1 = string.Empty;
            string prefix = string.Empty;
            string phone2 = string.Empty;
            string phone3 = string.Empty;
            string gender = string.Empty;
            string[] dob = Convert.ToString(oUser.DOB).Split('/');
            string dob1 = oUser.DOB.ToString("yyyy-MM-dd");
            string dob2 = oUser.DOB.ToString("MM/dd/yyyy");

            string[] dob3 = dob1.Split('-');
            string yyyy = dob3[0];
            string mm = dob3[1];
            string dd = dob3[2];

            string memberurl = ConfigurationManager.AppSettings["MemberPath"].ToString();
            string _address1 = oUser.Address1;
            string[] s = @"<,>,#,%,{,},|,\,^,~,[,],`".Split(',');
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (_address1.Contains(s[i]))
                {
                    _address1 = _address1.Replace(s[i], "");
                }
            }
            if (!string.IsNullOrEmpty(oUser.PhoneNumber))
            {
                if (oUser.PhoneNumber.Length > 9)
                {
                    phone1 = oUser.PhoneNumber.Substring(0, 3);
                    phone2 = oUser.PhoneNumber.Substring(3, 3);
                    phone3 = oUser.PhoneNumber.Substring(6, 4);
                }
                else
                {
                    phone1 = string.Empty;
                    phone2 = string.Empty;
                    phone3 = string.Empty;
                }
            }
            else
            {
                phone1 = string.Empty;
                phone2 = string.Empty;
                phone3 = string.Empty;
            }

            if (oUser.CountryId == 15 || oUser.CountryId == 229 || oUser.CountryId == 38)  //australia
            {
                url = ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + oUser.UserGuid.ToString();
            }
            else if (oUser.CountryId == 231)  // USA
            {

                url = "http://ads.ifficient.com/embedded?pubid=1009&srcid=2358&first=" +
                Server.UrlEncode(oUser.FirstName) + "&last=" + Server.UrlEncode(oUser.LastName) + "&email=" + Server.UrlEncode(oUser.EmailAddress) +
                    "&add1=" + Server.UrlEncode(_address1) + "&add2=" + Server.UrlEncode(oUser.Address2) +
                    "&city=" + Server.UrlEncode(oUser.City) + "&state=" + Server.UrlEncode(oUser.StateCode.Replace(" ", "").TrimEnd()) +
                    "&zip=" + Server.UrlEncode(oUser.ZipCode) + "&phone=" + Server.UrlEncode(oUser.PhoneNumber) + "&gender=" + Server.UrlEncode(oUser.Gender) + "&subid1=" + oUser.RefferId.ToString() +
                    "&dob=" + Server.UrlEncode(dob2) + "&rurl=" + Server.UrlEncode(ConfigurationManager.AppSettings["MemberPath"].ToString()) + "/Rg/offers.aspx?ug=" + Server.UrlEncode(oUser.UserGuid.ToString()) + "&isTest=n";

                //  url = "http://magnumapi.ifficient.com/inner.aspx?" +
                //"email=" + UserDetails.EmailAddress + "&first=" + UserDetails.FirstName +
                //        "&last=" + UserDetails.LastName + "&gender=" + UserDetails.Gender + "&dob=" + dob2 +
                //        "&add1=" + Server.UrlEncode(_address1) + "&addr2=" + Server.UrlEncode(UserDetails.Address2) + "&city=" + UserDetails.City +
                //         "&state=" + UserDetails.StateCode.Replace(" ", "").TrimEnd() + "&zip=" + UserDetails.ZipCode + "&istest=false" +
                //         "&phone=" + UserDetails.PhoneNumber + "&country=" + UserDetails.CountryCode.Trim() + "&pubid=1376&rurl=" + ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + UserDetails.UserGuid.ToString();
            }
            //For any other country
            else
            {
                url = ConfigurationManager.AppSettings["MemberPath"].ToString() + "/Rg/offers.aspx?ug=" + oUser.UserGuid.ToString();
            }
            return url;
        }
        #endregion
    }
}