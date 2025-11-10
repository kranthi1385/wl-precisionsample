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
    public partial class ALservice : System.Web.UI.Page
    {

        #region private Variables

        /// <summary>
        /// 
        /// </summary>
        public string Mode
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["mode"]))
                {
                    return Request.QueryString["mode"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNo
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["phno"]))
                {
                    return Request.QueryString["phno"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReferrerId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["rid"]))
                {
                    return Convert.ToInt32(Request.QueryString["rid"]);
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
        public string SubReferrerCode
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
                {
                    return Request.QueryString["sid"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LeadGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["lid"]))
                {
                    return Request.QueryString["lid"].ToString();
                }
                else
                {
                    return string.Empty;
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

            SMSBusinessManager oSMSManager = new SMSBusinessManager();
            TrumpiaServiceManager oTrumpiaService = new TrumpiaServiceManager();


            #region Insert Lead
            if (Mode == "insertlead")
            {
                RiverManager oManager = new RiverManager();
                var PageData = oManager.InsertLead(ReferrerId, SubReferrerCode, PhoneNo, Request.ServerVariables["REMOTE_ADDR"].ToString());
                writer.Write(JsonConvert.SerializeObject(PageData));
            }

            #endregion

            #region Send SMS
            if (Mode == "sendsms") //We need to Send SMS to the Give Number from We-tell.
            {
                var PageData = string.Empty;
                string TrumpiaKey = ConfigurationManager.AppSettings["TrumpiaKey"].ToString();
                string RiverLandingapgeUrl = string.Empty;
                if (ReferrerId == 14420 || ReferrerId == 14666)
                    RiverLandingapgeUrl = ConfigurationManager.AppSettings["RiverLandingPageForOtherRid"].ToString();
                else
                    RiverLandingapgeUrl = ConfigurationManager.AppSettings["RiverLandingPage"].ToString();

                try
                {
                    RiverLandingapgeUrl = RiverLandingapgeUrl.Replace("%%referrer_id%%", ReferrerId.ToString()).Replace("%%sub_referrer_code%%", SubReferrerCode).Replace("%%sub_id%%", LeadGuid);
                    string _surveyURl = string.Empty;
                    _surveyURl = oTrumpiaService.MakeTinyUrl(RiverLandingapgeUrl);
                    //Code Added on 8/17/2015 to show Isay.com in SMS 
                    _surveyURl = _surveyURl.Replace("tinyurl.com", "isay.co");
                    //  bool isSuccess = false;
                    string smsMesg = ConfigurationManager.AppSettings["SMSText"].ToString() + _surveyURl + ". " + ConfigurationManager.AppSettings["SMSUnsubscribetext"].ToString();
                    smsMesg = HttpUtility.HtmlEncode(smsMesg);
                    string trumpiaXML = oTrumpiaService.GetContactId(TrumpiaKey, "NotificationFirstName", "NotificationLastname", PhoneNo.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""), "mTelligence");
                    if (oTrumpiaService.ParseRespone(trumpiaXML, TrumpiaKey))
                    {
                        string trumpiaId = oSMSManager.GetTrupiaId(trumpiaXML);
                        string response = oTrumpiaService.SendToContact("test Message", smsMesg, "mTelligence", trumpiaId, TrumpiaKey, ConfigurationManager.AppSettings["DisplayName"].ToString());
                        string resId = oSMSManager.GetResponseId(response);
                        // Sending SMS by contactId
                        if (oTrumpiaService.CheckResponse(resId))
                        {
                            //Success
                            PageData = "success";
                            writer.Write(JsonConvert.SerializeObject(PageData));
                        }
                        else
                        {
                            //Failure
                            PageData = "checkresponsefail";
                            writer.Write(JsonConvert.SerializeObject(PageData));
                        }
                    }
                    else
                    {
                        //Failure
                        PageData = "parseresponsefail";
                        writer.Write(JsonConvert.SerializeObject(PageData));
                    }
                }
                catch ( Exception  ex)
                {
                    //Failure
                    if (ex.InnerException != null)
                    {
                        PageData = ex.InnerException.ToString() + "," + ex.Message;
                    }
                    else
                    {
                        PageData = ex.Message;
                    }
                    writer.Write(JsonConvert.SerializeObject(PageData));
                }

            }
            #endregion

        }

        #endregion

    }
}
