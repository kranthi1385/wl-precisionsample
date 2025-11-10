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
using System.Text.RegularExpressions;

namespace Members.PrecisionSample.River.Web
{
    public partial class al : System.Web.UI.Page
    {
        #region properties

        public string PhoneNo
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pn"]))
                {
                    return Request.QueryString["pn"].ToString();
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!string.IsNullOrEmpty(PhoneNo) && ReferrerId > 0)
                {
                    bool _isvalid = Regex.IsMatch(PhoneNo, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");

                    if (_isvalid)
                    {
                        string TrumpiaKey = ConfigurationManager.AppSettings["TrumpiaKey"].ToString();
                        string RiverLandingapgeUrl = ConfigurationManager.AppSettings["RiverLandingPage"].ToString();
                        try
                        {
                            SMSBusinessManager oSMSManager = new SMSBusinessManager();
                            TrumpiaServiceManager oTrumpiaService = new TrumpiaServiceManager();

                            RiverManager oManager = new RiverManager();
                            string LeadGuid = oManager.InsertLead(ReferrerId, SubReferrerCode, PhoneNo, Request.ServerVariables["REMOTE_ADDR"].ToString());
                            if (LeadGuid.Length == 36)
                            {
                                RiverLandingapgeUrl = RiverLandingapgeUrl.Replace("%%referrer_id%%", ReferrerId.ToString()).Replace("%%sub_referrer_code%%", SubReferrerCode).Replace("%%sub_id%%", LeadGuid);
                                string _surveyURl = string.Empty;
                                _surveyURl = oTrumpiaService.MakeTinyUrl(RiverLandingapgeUrl);
                                //Code Added on 8/17/2015 to show Isay.com in SMS 
                                _surveyURl = _surveyURl.Replace("tinyurl.com", "isay.co");
                                //  bool isSuccess = false;
                                string smsMesg = ConfigurationManager.AppSettings["SMSText"].ToString() + _surveyURl;
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
                                        Response.Write("success");
                                    }
                                    else
                                    {
                                        //Failure
                                        Response.Write("fail");
                                    }
                                }
                                else
                                {
                                    //Failure
                                    Response.Write("fail");
                                }
                            }
                            else
                            {
                                Response.Write("fail");
                            }
                        }
                        catch
                        {
                            //Failure
                            Response.Write("fail");
                        }
                    }
                    else
                    {
                        Response.Write("fail");
                    }
                }
                else
                {
                    Response.Write("fail");
                }
            }
        }
    }
}