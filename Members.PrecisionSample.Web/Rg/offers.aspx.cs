using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using System.Configuration;
using System.Web.Security;

namespace Members.PrecisionSample.Web.Registration
{
    public partial class offers : System.Web.UI.Page
    {
        #region Private Variables
        /// <summary>
        /// Private Variables
        /// </summary>
        private string _url1 = string.Empty;
        private string _url2 = string.Empty;
        private string _url3 = string.Empty;
        #endregion

        #region Public Members
        /// <summary>
        /// Public Members
        /// </summary>
        public Guid UserGuid
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
        /// Public members
        /// </summary>
        public string Url1
        {
            get
            {
                return _url1;
            }
            set
            {
                _url1 = value;
            }
        }
        /// <summary>
        /// public members
        /// </summary>
        public string Url2
        {
            get
            {
                return _url2;
            }
            set
            {
                _url2 = value;
            }
        }
        /// <summary>
        /// public members
        /// </summary>
        public string Url3
        {
            get
            {
                return _url3;
            }
            set
            {
                _url3 = value;
            }
        }
        #endregion

        #region page-load
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //code added on 10/22/2013
            Response.Redirect("/Rg/sm.aspx?ug=" + UserGuid.ToString());
            //code added on9/22/2010
            User oUser2 = new User();
            UserManager oUserManager = new UserManager();
            oUser2 = oUserManager.GetUserData(UserGuid.ToString());
            oUser2.RegistrationStep = "E";
            UserManager oManger1 = new UserManager();
            oManger1.UserRegistrationStepUpdate(oUser2);
            DoLogin(UserGuid.ToString());
            //endcode

            if (!IsPostBack)
            {
                User oUser1 = new User();
                oUser1 = oUserManager.GetUserData(UserGuid.ToString());
                //lblEmailAddress.Text = UserDetails.EmailAddress;
                lblFirstName.Text = oUser1.FirstName;
                oUserManager.GetUserData(UserGuid.ToString());
                //User objUser = new User();
                //objUser.UserId = UserDetails.UserId;
                ///objUser.RegistrationStep = "D";
                //UserManager oManger = new UserManager();
                //oManger.UserRegistrationStepUpdate(objUser);
                lblFirstName.Text = oUser1.FirstName;
                if (oUser1.CountryId == 231)
                {
                    //lblCountry.Text = "Uinted States Of America";
                }
                else if (oUser1.CountryId == 229)//uk
                {
                    //lblCountry.Text = "Uinted Kingdom";
                }
                else if (oUser1.CountryId == 15)//aus
                {
                    //lblCountry.Text = "Australia";
                }
                else if (oUser1.CountryId == 38)//ca
                {
                    //lblCountry.Text = "Canada";
                }

                string[] dob = Convert.ToString(oUser1.DOB).Split('/');
                int year = Convert.ToInt32(dob[2].Substring(0, 4).ToString());
                int presentyear = Convert.ToInt32(DateTime.Now.Year.ToString());
                //lblAge.Text = Convert.ToString(presentyear - year);
                if (oUser1.Gender.ToLower() == "m")
                {
                    //lblGender.Text = "Male";
                }
                else if (oUser1.Gender.ToLower() == "f")
                {
                    //lblGender.Text = "FeMale";
                }

            }
            PerksManager oPerksManager = new PerksManager();
            List<Perks> oPerks = new List<Perks>();
            oPerks = oPerksManager.GetPage4OfferDetails(oUser2.UserId);
            int count = oPerks.Count;
            int i = 0;
            if (i < count)
            {
                i++;
                Url1 = "/ei/sc1.aspx?s=w&pid=" + oPerks[0].PerkGuid.ToString();
                //aOffer1.HRef = oPerks[0].PerkUrl;
                imgOffer1.ImageUrl = ConfigurationManager.AppSettings["ManagePath"].ToString() + "/uploads/" + oPerks[0].Page4ImageLogo;
            }
            if (i < count)
            {
                i++;
                Url2 = "/ei/sc1.aspx?s=w&pid=" + oPerks[1].PerkGuid.ToString();
                //aOffer2.HRef = oPerks[1].PerkUrl;
                imgOffer2.ImageUrl = ConfigurationManager.AppSettings["ManagePath"].ToString() + "/uploads/" + oPerks[1].Page4ImageLogo;
            }
            if (i < count)
            {
                Url3 = "/ei/sc1.aspx?s=w&pid=" + oPerks[2].PerkGuid.ToString();
                //aOffer3.HRef = oPerks[2].PerkUrl;
                imgOffer3.ImageUrl = ConfigurationManager.AppSettings["ManagePath"].ToString() + "/uploads/" + oPerks[2].Page4ImageLogo;
            }

        }
        #endregion

        #region Button Events
        /// <summary>
        /// Button Events
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void btnOffer1_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url1;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }

        /// <summary>
        /// Button offer2 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void btnOffer2_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url2;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }
        /// <summary>
        /// button offer3 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void btnOffer3_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url3;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }
        /// <summary>
        /// image offer 1 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void imgbtnOffer1_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url1;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }
        /// <summary>
        /// image offer 2 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void imgbtnOffer2_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url2;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }
        /// <summary>
        /// image offer 3 click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void imgbtnOffer3_Click(object sender, ImageClickEventArgs e)
        {
            string url = Url3;
            ClientScript.RegisterStartupScript(this.GetType(), "OpenWin", "<script>openNewWin('" + url + "')</script>");
        }

        /// <summary>
        /// button continue click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnContinue_Click(object sender, ImageClickEventArgs e)
        {
            User objUser = new User();
            objUser.UserId = objUser.UserId;
            objUser.RegistrationStep = "D";
            UserManager oManger = new UserManager();
            oManger.UserRegistrationStepUpdate(objUser);
            DoLogin(objUser.UserGuid.ToString());
            Response.Redirect("/Rg/sm.aspx?ug=" + objUser.UserGuid.ToString());
            // Response.Redirect("/rg/profile.aspx?pid="+ConfigurationManager.AppSettings["BasicProfileGuid"].ToString()+"&ug=" + UserDetails.UserGuid.ToString());
            // Response.Redirect("/rg/sm.aspx?ug=" + UserDetails.UserGuid.ToString());
        }

        /// <summary>
        /// Do Login
        /// </summary>
        /// <param name="userName">username</param>
        public void DoLogin(string userName)
        {
            // get the cookie
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(userName, false);
            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                // issue date
                DateTime oDateBegin = DateTime.Now;

                // expiry date (Set default to 60 mts)
                DateTime oDateEnd = oDateBegin.AddMinutes(60);

                // get the logged in user
                // MembershipUser currentUser = System.Web.Security.Membership.GetUser(ticket.Name, true);

                Members.PrecisionSample.Components.Entities.User oUser = new Members.PrecisionSample.Components.Entities.User();
                Guid user_id = Guid.Empty;

                // create a new ticket for each request
                FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(1, userName, oDateBegin, oDateEnd, false, userName.ToString());
                cookie.Value = FormsAuthentication.Encrypt(newticket);
                Context.Response.Cookies.Set(cookie);
            }
        }
        #endregion
    }
}