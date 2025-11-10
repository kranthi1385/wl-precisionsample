using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Members.PrecisionSample.Components.Business_Layer;
using System.Configuration;
using NLog;
using System.Numerics;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Members.PrecisionSample.Web.Controllers
{
    public class EController : BaseController
    {
        #region public Varialbes
        ExternalMembersManager oManager = new ExternalMembersManager();
        UserManager oUserManager = new UserManager();
        string requestUrl = string.Empty;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        // GET: Ext
        public ActionResult Index()
        {
            return View();
        }

        #region Ext
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public ActionResult Ext(string mid, string pstest, string pid)
        {
            if (pstest == "1")
            {
                var url = ConfigurationManager.AppSettings["PrescreenerUrl"].ToString();
                Response.Redirect(url + "ug=" + mid + "&uig=" + mid.ToString() + "&pid=" + pid + "&cid=-1");
            }
            return View("/Views/E/External.cshtml");
        }
        #endregion

        #region Extget - Insert external Members details
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public JsonResult ExtInsert(string mid, string pid)
        {
            ExtMemberGuidChk oExtmbrguid = new ExtMemberGuidChk();
            oExtmbrguid = oManager.ExtMemberInsert(mid, pid);
            logger.Info("insertextInfo|" + Request.Url.AbsoluteUri.ToString() + "|mid:" + mid + "|pid:" + pid.ToString());
            return Json(oExtmbrguid, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region CAG Ext  ( samplify ) 
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public void CagExt(int pid, string mid)
        {
            string _redirecturl = string.Empty;
            //For LB or Simipify we wil directly insert members into external table , and no security checks to continur to prescreener page.
            string u = string.Empty;
            int userTrafficTypeId = 2;
            string browserInfo = string.Empty;
            string mobiledeviceModel = string.Empty;
            string ipAddress = string.Empty;
            int relevantScore = 0;
            string fpfScores = string.Empty;
            int fraudPfScore = 0;
            Surveys oSurvey = new Surveys();

            u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (u != null)
            {
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    //If Mobile Device Matched:
                    userTrafficTypeId = 2;
                }
                else
                {
                    //If Non Mobile Device Matched.
                    userTrafficTypeId = 3;
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
                        userTrafficTypeId = 2;
                    }
                    else
                    {
                        //If the Non Mobile user is participating in Survey.
                        userTrafficTypeId = 3;
                    }
                }
            }

            //To read Browser Proeprties :
            System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            if (browser != null)
            {
                browserInfo = "Type = " + browser.Type + "|"
                    + "Name = " + browser.Browser + "|"
                    + "Version = " + browser.Version + "|"
                    + "Major Version = " + browser.MajorVersion + "|"
                    + "Minor Version = " + browser.MinorVersion + "|"
                    + "Platform = " + browser.Platform;
            }


            //Added on 9/12/2014 to save the Mobile Device Model.
            if (Request.UserAgent.Contains("Android"))
            {
                mobiledeviceModel = "Android";
            }
            else if (Request.UserAgent.Contains("webOS"))
            {
                mobiledeviceModel = "webOS";
            }
            else if (Request.UserAgent.Contains("iPhone"))
            {
                mobiledeviceModel = "iPhone";
            }
            else if (Request.UserAgent.Contains("iPad"))
            {
                mobiledeviceModel = "iPad";
            }
            else if (Request.UserAgent.Contains("iPod"))
            {
                mobiledeviceModel = "iPod";
            }
            else if (Request.UserAgent.Contains("BlackBerry"))
            {
                mobiledeviceModel = "BlackBerry";
            }
            else if (Request.UserAgent.Contains("Windows Phone"))
            {
                mobiledeviceModel = "Windows Phone";
            }
            ipAddress = Request.UserHostAddress;
            BigInteger Ipnumber = oUserManager.GetIPNumber(ipAddress);
            relevantScore = 0;
            fpfScores = "0;0";
            fraudPfScore = 0;
            oSurvey = oManager.CagSaveClickInformation("", mid, pid, "", "", "", 1, userTrafficTypeId, mobiledeviceModel, browserInfo,
                                                           Request.UserAgent, ipAddress, "", relevantScore, fpfScores, fraudPfScore, "", "", 0, "", "", Ipnumber.ToString());
            //Log to Save Exteernal Info.
            logger.Info("cagextinfo|" + Request.Url.AbsoluteUri.ToString() + "|routed:" + oSurvey.RedirectUrl);

            Response.Redirect(oSurvey.RedirectUrl);


        }
        #endregion


        #region LB Ext  ( for LB members only. ) 
        /// <summary>
        /// Step2
        /// </summary>
        /// <returns></returns>
        public void LBExt(int pid)
        {
            string _redirecturl = string.Empty;
            //For LB or Simipify we wil directly insert members into external table , and no security checks to continur to prescreener page.
            string u = string.Empty;
            int userTrafficTypeId = 2;
            string browserInfo = string.Empty;
            string mobiledeviceModel = string.Empty;
            string ipAddress = string.Empty;
            int relevantScore = 0;
            string fpfScores = string.Empty;
            int fraudPfScore = 0;
            Surveys oSurvey = new Surveys();

            u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (u != null)
            {
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    //If Mobile Device Matched:
                    userTrafficTypeId = 2;
                }
                else
                {
                    //If Non Mobile Device Matched.
                    userTrafficTypeId = 3;
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
                        userTrafficTypeId = 2;
                    }
                    else
                    {
                        //If the Non Mobile user is participating in Survey.
                        userTrafficTypeId = 3;
                    }
                }
            }

            //To read Browser Proeprties :
            System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            if (browser != null)
            {
                browserInfo = "Type = " + browser.Type + "|"
                    + "Name = " + browser.Browser + "|"
                    + "Version = " + browser.Version + "|"
                    + "Major Version = " + browser.MajorVersion + "|"
                    + "Minor Version = " + browser.MinorVersion + "|"
                    + "Platform = " + browser.Platform;
            }


            //Added on 9/12/2014 to save the Mobile Device Model.
            if (Request.UserAgent.Contains("Android"))
            {
                mobiledeviceModel = "Android";
            }
            else if (Request.UserAgent.Contains("webOS"))
            {
                mobiledeviceModel = "webOS";
            }
            else if (Request.UserAgent.Contains("iPhone"))
            {
                mobiledeviceModel = "iPhone";
            }
            else if (Request.UserAgent.Contains("iPad"))
            {
                mobiledeviceModel = "iPad";
            }
            else if (Request.UserAgent.Contains("iPod"))
            {
                mobiledeviceModel = "iPod";
            }
            else if (Request.UserAgent.Contains("BlackBerry"))
            {
                mobiledeviceModel = "BlackBerry";
            }
            else if (Request.UserAgent.Contains("Windows Phone"))
            {
                mobiledeviceModel = "Windows Phone";
            }
            ipAddress = Request.UserHostAddress;
            BigInteger Ipnumber = oUserManager.GetIPNumber(ipAddress);
            relevantScore = 0;
            fpfScores = "0;0";
            fraudPfScore = 0;
            oSurvey = oManager.CagSaveClickInformation("", "", pid, "", "", "", 1, userTrafficTypeId, mobiledeviceModel, browserInfo,
                                                           Request.UserAgent, ipAddress, "", relevantScore, fpfScores, fraudPfScore, "", "", 0, "", "", Ipnumber.ToString());
            //Log to Save Exteernal Info.
            logger.Info("LBextInfo|" + Request.Url.AbsoluteUri.ToString() + "|routed:" + oSurvey.RedirectUrl);

            Response.Redirect(oSurvey.RedirectUrl);


        }
        #endregion

        #region GetMethod
        public string GetrdJson(string RequestURL, string token)
        {
            String rdjson = string.Empty;
            StreamReader sr = null;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(RequestURL);
                Request.Method = "GET";
                Request.Headers.Add("token", token);
                Request.ContentType = "application/json; charset=UTF-8";
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                sr = new StreamReader(Response.GetResponseStream());
                rdjson = sr.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //sr.Close();
            }

            return rdjson;
        }
        #endregion


        #region ExtUpdate  
        /// <summary>
        /// Save User Click Information
        /// </summary>
        /// <param name="qg">QuotaGroupId</param>
        /// <param name="mid">UserGuid</param>
        /// <param name="pid">TargetId</param>
        /// <param name="rid">ReferrerId</param>
        /// <param name="source">Source</param>
        /// <param name="subId">SubId</param>
        /// <param name="isNew">IsNew</param>
        /// <param name="rvId">RelevantId</param>
        /// <param name="osId">OldSurveyInvitationId</param>
        /// <param name="tScore">TotalScores</param>
        /// <returns></returns>
        //[ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("ExtUpdate")]
        public JsonResult ExtUpdate(string qg, string mid, string pid, string rid, string source, string subId, int isNew, string rvId, string osId, string tScore, string frid, decimal ecost, string e_rm, string e_rl, bool is_dupe, string external_member_id, int project_id, int org_id, string external_member_guid, int hash, string country,string age, string gender, string income, string ethnicity, string hhi, string email, string education, string hispanic)
        {
            Surveys oSurvey = new Surveys();
            if (hash == 1)
            {
                oSurvey.FedRedirectURl = $"https://e.reachcollective.com/e/psr?usg=813E7F65-64FE-4162-A7A3-33549A857953&uig={qg}";
                return Json(oSurvey, JsonRequestBehavior.AllowGet);
            }
            //string RequestURL = string.Empty;
            //RequestURL = System.Configuration.ConfigurationManager.AppSettings["ResearchDefender"].ToString();
            //RequestURL = string.Format(RequestURL, mid, project_id, external_member_guid);
            //string rdJson = GetrdJson(RequestURL, token);
            //logger.Info("rdjson|" + rdJson);
            string u = string.Empty;
            int userTrafficTypeId = 2;
            string browserInfo = string.Empty;
            string mobiledeviceModel = string.Empty;
            string ipAddress = string.Empty;
            int relevantScore = 0;
            string fpfScores = string.Empty;
            int fraudPfScore = 0;
            u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (u != null)
            {
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    //If Mobile Device Matched:
                    userTrafficTypeId = 2;
                }
                else
                {
                    //If Non Mobile Device Matched.
                    userTrafficTypeId = 3;
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
                        userTrafficTypeId = 2;
                    }
                    else
                    {
                        //If the Non Mobile user is participating in Survey.
                        userTrafficTypeId = 3;
                    }
                }
            }

            //To read Browser Proeprties :
            System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            if (browser != null)
            {
                browserInfo = "Type = " + browser.Type + "|"
                    + "Name = " + browser.Browser + "|"
                    + "Version = " + browser.Version + "|"
                    + "Major Version = " + browser.MajorVersion + "|"
                    + "Minor Version = " + browser.MinorVersion + "|"
                    + "Platform = " + browser.Platform;
            }


            //Added on 9/12/2014 to save the Mobile Device Model.
            if (Request.UserAgent.Contains("Android"))
            {
                mobiledeviceModel = "Android";
            }
            else if (Request.UserAgent.Contains("webOS"))
            {
                mobiledeviceModel = "webOS";
            }
            else if (Request.UserAgent.Contains("iPhone"))
            {
                mobiledeviceModel = "iPhone";
            }
            else if (Request.UserAgent.Contains("iPad"))
            {
                mobiledeviceModel = "iPad";
            }
            else if (Request.UserAgent.Contains("iPod"))
            {
                mobiledeviceModel = "iPod";
            }
            else if (Request.UserAgent.Contains("BlackBerry"))
            {
                mobiledeviceModel = "BlackBerry";
            }
            else if (Request.UserAgent.Contains("Windows Phone"))
            {
                mobiledeviceModel = "Windows Phone";
            }
            ipAddress = Request.UserHostAddress;
            BigInteger Ipnumber = oUserManager.GetIPNumber(ipAddress);
            if (!string.IsNullOrEmpty(tScore))
            {
                relevantScore = Convert.ToInt32(tScore.Split(';')[0]);
                fpfScores = tScore.Split(';')[1];
                fraudPfScore = Convert.ToInt32(tScore.Split(';')[2]);
            }
            if (is_dupe == false) ///for the first click.
            {
                //Insert the Click Actiivty , Proejct 2 IP entry on PS2 DB.
                var res = oManager.ExternalMemberActivityInsert(external_member_id, project_id, pid, ipAddress, frid, ecost, mobiledeviceModel, userTrafficTypeId, org_id, external_member_guid);
            }
            oSurvey = oManager.UpdateExternalMember(qg, mid, pid, rid, source, subId, isNew, userTrafficTypeId, mobiledeviceModel, browserInfo,
                                                           Request.UserAgent, ipAddress, rvId, relevantScore, fpfScores, fraudPfScore, osId, frid, ecost, e_rm, e_rl, Ipnumber.ToString(), is_dupe, external_member_id,
                                                           project_id, external_member_guid, country, age, gender, income, ethnicity, hhi, email, education, hispanic);
            //Log to Save Exteernal Info.
            logger.Info("saveextInfo|" + Request.Url.AbsoluteUri.ToString() + "|routed:" + oSurvey.RedirectUrl);
            return Json(oSurvey, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public void s(string rid, string sid, string fid, int? rcheckr, string txid, string trans_id, string fn, string ln, string em, string dob, string lname)
        {

            //For Normal Affilaite.
            string ReferrerUrl = string.Empty;

            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            }
            if (!string.IsNullOrEmpty(fid))
            {
                FriendId = Convert.ToInt32(fid);
            }
            RefererUrl = ReferrerUrl;
            if (rid != string.Empty)
            {
                ReferrerIds = (rid + "/" + sid + "/" + txid + "/" + trans_id + "///").Split('/');
            }
            else
            {
                requestUrl = GetAbsoluteUrl();
                ReferrerIds = (MemberIdentity.Client.Referrerid + "////").Split('/');
            }
            if (string.IsNullOrEmpty(rid))
            {
                Response.Redirect("/e/login");
            }
            else
            {
                string url = oUserManager.GetLandingpageUrl(Convert.ToInt32(rid));
                if (!string.IsNullOrEmpty(url))
                {
                    url = url.Replace("%%referrer_id%%", rid.ToString());
                    if (sid != null)
                    {
                        url = url.Replace("%%sub_id%%", sid.ToString());
                    }
                    else
                    {
                        url = url.Replace("%%sub_id%%", string.Empty);
                    }

                    if (txid != null)
                    {
                        url = url.Replace("%%external_member_id%%", txid.ToString());
                    }
                    else
                    {
                        url = url.Replace("%%external_member_id%%", string.Empty);
                    }

                    if (lname != null)
                    {
                        url = url.Replace("%%language%%", lname.ToString());

                    }
                    else
                    {
                        url = url.Replace("%%language%%", "english");
                    }
                    url = url.Replace("%%app_id%%", ConfigurationManager.AppSettings["AppId"].ToString());
                    url = url.Replace("%%app_name%%", ConfigurationManager.AppSettings["AppName"].ToString());
                    url = url.Replace("%%transaction_id%%", trans_id);

                    if (rcheckr == 1) //we have added this to Switch Off Relevant & Verity check for members.
                    {
                        Response.Redirect(url + "?rcheckr=1");
                    }
                    else
                    {
                        if (url.Contains("authkey")) //means Conversant/OB Router affilaite.
                        {
                            //We need to inser hits.
                            UserManager oManager = new UserManager();
                            int clickId = oManager.InserClicks(ReferrerIds[0], ReferrerIds[1], ReferrerIds[2], IpAddress, RefererUrl, 1, 0, 0);
                        }
                        else
                        {
                            //added on 23/6/2015 to pass additional paramaters like fn,ln.am,dob to url
                            if (!string.IsNullOrEmpty(fn))
                            {
                                url = url + "&fn=" + fn;
                            }
                            else
                            {
                                url = url + "&fn=";
                            }
                            if (!string.IsNullOrEmpty(ln))
                            {
                                url = url + "&ln=" + ln;
                            }
                            if (!string.IsNullOrEmpty(em))
                            {
                                url = url + "&em=" + em;
                            }
                            if (!string.IsNullOrEmpty(dob))
                            {
                                url = url + "&dob=" + dob;
                            }
                        }
                        Response.Redirect(url);
                    }
                }
                else
                {
                    Response.Redirect("/e/login");
                }
            }

        }

        /// <summary>
        /// Redirect to member login page
        /// </summary>
        public void Login()
        {
            string redirectUrl = Request.Url.AbsoluteUri;
            if (redirectUrl.ToLower().Contains("http://widget.reachcollective.com") || redirectUrl.ToLower().Contains("https://widget.reachcollective.com") || redirectUrl.ToLower().Contains("http://opinionetwork.com")
               || redirectUrl.ToLower().Contains("https://widget.reachcollective.com") || redirectUrl.ToLower().Contains("http://s.opinionetwork.com"))
            {
                Response.Redirect("http://widget.reachcollective.com/hm.html");
            }
            else
            {
                string _rurl = Request.Params[0];
                string actualParam = "";
                string path = string.Empty;
                if (Request.RawUrl.Split('?').Length > 1)
                {
                    actualParam = Request.RawUrl.Split('?')[1];
                }
                path = MemberIdentity.Client.MgLoginPath + "?" + actualParam;
                Response.Redirect(path);
            }
        }

        #region Clean ID Validations
        public ActionResult Vld()
        {
            return View("/Views/E/Vld.cshtml");
        }
        #endregion

        #region CleanIDDataInsert
        [HttpPost]
        [Route("CleanIDDataInsert")]
        public string CleanIDDataInsert(string uig, string ug, int pid, string extmid, int cid, string json, string sessionId = null)
        {
            //string IpqsJSON;
            //Request.InputStream.Position = 0;
            //using (var reader = new StreamReader(Request.InputStream))
            //{
            //    IpqsJSON = reader.ReadToEnd();
            //}
            logger.Info("InvitationID: " + extmid + " | UserGUID: " + ug + " | ProjectID: " + pid + " | Clean ID JSON | " + json + " | Verisoul SessionId | " + sessionId);
            return oManager.CleanIDDataInsert(uig, ug, pid, extmid, cid, json, sessionId);
        }
        #endregion
    }
}