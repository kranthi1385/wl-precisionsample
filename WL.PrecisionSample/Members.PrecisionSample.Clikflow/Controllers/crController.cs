using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Clikflow.Filters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;
using MaxMind.MinFraud;
using MaxMind.MinFraud.Request;
using System.Configuration;
using System.Threading.Tasks;
using System.Net;
using System.Web.Helpers;
using NLog;
using System.IO;
using System.Text;

namespace Members.PrecisionSample.Clikflow.Controllers
{
    public class crController : Controller
    {
        #region public Varialbes
        ClickflowManager oManager = new ClickflowManager();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        string logMessage = string.Empty;
        string formBody = string.Empty;
        string outputMessage = string.Empty;
        bool isEligible = true;
        string baseurl = ConfigurationManager.AppSettings["prodbaseeurl"].ToString();
        string publishablekey = ConfigurationManager.AppSettings["prodpubkey"].ToString();
        string privatekey = ConfigurationManager.AppSettings["prodpvtkey"].ToString();

        #endregion

        // GET: cr
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult SaveRid(string quotaguid, string userguid,string relevantId,string totalscores)
        //{

        //    //DB Call to Business Layer.
        //}

        //public ActionResult start(string qig, string ug, int? cid, string rid, string s, string subId, string osid, string fedresid,
        //  string transid, string project, string sub, string tx_id, string frid, string lid, string sub_id)
        //{
        //    string ipaddress = string.Empty;
        //    string surveyId = string.Empty;
        //    try
        //    {
        //        ipaddress = Request.UserHostAddress;
        //        surveyId = oManager.GetOutSideProjectId(project);
        //        if (!String.IsNullOrEmpty(surveyId))
        //        {
        //            logger.Trace($"getIpsosEligibility | SurveyID : {surveyId}");
        //            isEligible = oManager.IsEligibleForIPSOS(ipaddress, surveyId);
        //            logger.Trace($"getIpsosEligibility | IsEligible :{isEligible} | ProjectID :{project}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Trace("IpsosEligibility" + ex.ToString());
        //    }
        //    if (!string.IsNullOrEmpty(transid))
        //    {
        //        subId = transid;
        //    }
        //    if (!string.IsNullOrEmpty(sub))
        //    {
        //        subId = sub;
        //    }
        //    if (!string.IsNullOrEmpty(sub_id))
        //    {
        //        subId = sub_id;
        //    }
        //    if (!string.IsNullOrEmpty(tx_id))
        //    {
        //        subId = tx_id;
        //    }
        //    int clientid = Convert.ToInt32(cid);
        //    if ((ConfigurationManager.AppSettings["ClickPageSplit"].ToString()) == "1")
        //    {
        //        if (clientid == 0)
        //        {
        //            clientid = oManager.GetOrgidByUserDPV(ug);
        //        }
        //        string u = string.Empty;
        //        int userTrafficTypeId = 2;
        //        string browserInfo = string.Empty;
        //        string mobiledeviceModel = string.Empty;
        //        string ipAddress = string.Empty;
        //        Surveys oSurvey = new Surveys();
        //        u = Request.ServerVariables["HTTP_USER_AGENT"];
        //        Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        //        if (u != null)
        //        {
        //            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
        //            {
        //                userTrafficTypeId = 2;//If Mobile Device Matched:
        //            }
        //            else
        //            {
        //                userTrafficTypeId = 3;//If Non Mobile Device Matched.
        //            }
        //        }
        //        else //If the Request.ServerVariables is NULL 
        //        {
        //            if (Request.UserAgent != null)
        //            {
        //                u = Request.UserAgent;
        //                if (Request.UserAgent.Contains("Android") || Request.UserAgent.Contains("webOS")
        //                 || Request.UserAgent.Contains("iPhone") || Request.UserAgent.Contains("iPad")
        //                 || Request.UserAgent.Contains("iPod") || Request.UserAgent.Contains("BlackBerry") || Request.UserAgent.Contains("Windows Phone"))
        //                {
        //                    userTrafficTypeId = 2;//If the Mobile user Paticiapting the Survey
        //                }
        //                else
        //                {
        //                    userTrafficTypeId = 3;//If the Non Mobile user is participating in Survey.
        //                }
        //            }
        //        }
        //        //To read Browser Proeprties :
        //        System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
        //        if (browser != null)
        //        {
        //            browserInfo = "Type = " + browser.Type + "|"
        //                + "Name = " + browser.Browser + "|"
        //                + "Version = " + browser.Version + "|"
        //                + "Major Version = " + browser.MajorVersion + "|"
        //                + "Minor Version = " + browser.MinorVersion + "|"
        //                + "Platform = " + browser.Platform;
        //        }
        //        //Added on 9/12/2014 to save the Mobile Device Model.
        //        if (Request.UserAgent.Contains("Android"))
        //        {
        //            mobiledeviceModel = "Android";
        //        }
        //        else if (Request.UserAgent.Contains("webOS"))
        //        {
        //            mobiledeviceModel = "webOS";
        //        }
        //        else if (Request.UserAgent.Contains("iPhone"))
        //        {
        //            mobiledeviceModel = "iPhone";
        //        }
        //        else if (Request.UserAgent.Contains("iPad"))
        //        {
        //            mobiledeviceModel = "iPad";
        //        }
        //        else if (Request.UserAgent.Contains("iPod"))
        //        {
        //            mobiledeviceModel = "iPod";
        //        }
        //        else if (Request.UserAgent.Contains("BlackBerry"))
        //        {
        //            mobiledeviceModel = "BlackBerry";
        //        }
        //        else if (Request.UserAgent.Contains("Windows Phone"))
        //        {
        //            mobiledeviceModel = "Windows Phone";
        //        }
        //        String ReaserchDefender = string.Empty;
        //        if ((ConfigurationManager.AppSettings["ReaserchDefenderFlag"].ToString()) == "1")
        //        {
        //            try
        //            {
        //                Stream dataStream;
        //                string Searchurl = ConfigurationManager.AppSettings["search"].ToString();
        //                var url = baseurl + Searchurl;
        //                url = string.Format(url, privatekey);

        //                // two req params sy_nr and sn_ud
        //                string prmts = "?sy_nr=" + project + "&sn_ud=" + ug + "&coordinates=1&wd=1";
        //                url = url + prmts;

        //                string resptoken = GetToken(baseurl, publishablekey);
        //                WebResponse response = oManager.Get(url, resptoken);
        //                dataStream = response.GetResponseStream();
        //                StreamReader reader = new StreamReader(dataStream);
        //                ReaserchDefender = reader.ReadToEnd();
        //                //string resp = oManager.ResearchDefenderSearchInsert(ReaserchDefender);

        //            }
        //            catch (Exception ex)
        //            {

        //                logger.Trace("Research Defender" + ex.ToString());
        //                return null;
        //            }
        //        }

        //        ipAddress = Request.UserHostAddress;
        //        BigInteger IpNumber = Dot2LongIP(ipAddress);
        //        oSurvey = oManager.InsertClick(qig, ug, Convert.ToInt32(project), clientid, rid, s, subId, userTrafficTypeId, mobiledeviceModel, browserInfo,
        //                                                Request.UserAgent, ipAddress, osid, fedresid, IpNumber.ToString(), ReaserchDefender, isEligible);
        //        if (oSurvey.RedirectUrl == "5CE933ED-9891-4CD7-8AC6-B529C58C6B55")
        //        {
        //            //device failed
        //            var deviceMatchedCount = 0;
        //            var trafficTypes = oSurvey.SurveyUserTypeIds.Split(';');
        //            for (int i = 0; i < trafficTypes.Length; i++)
        //            {
        //                //If the Project Both devices then 
        //                if (trafficTypes[i] == "1")
        //                {
        //                    deviceMatchedCount = deviceMatchedCount + 1;
        //                }
        //                else
        //                    if (trafficTypes[i] == userTrafficTypeId.ToString())
        //                {
        //                    deviceMatchedCount = deviceMatchedCount + 1;
        //                }
        //            }
        //            //If the Mobile User is on Non Mobile Survey.
        //            if (deviceMatchedCount == 0 && userTrafficTypeId == 2)
        //            {
        //                //if (oSurvey.IsStandalone)
        //                //{
        //                return Redirect("https://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + oSurvey.ActualInvitationGuid + "&ug=" + ug + "&pid=" + oSurvey.ProjectId + "&cid=" + oSurvey.OrgId); //redirect to endpage
        //                //}
        //                //else
        //                //{
        //                //    return Redirect("https://www.opinionetwork.com/reg/home?ug=" + ug + "&cid=" + clientid); //redirect to mobile survey page
        //                //}
        //            }
        //            //If Desktop User on a Mobile Survey.
        //            if (deviceMatchedCount == 0 && userTrafficTypeId == 3)
        //            {
        //                if (oSurvey.CountyCode.ToLower() == "us")
        //                {
        //                    if (oSurvey.IsStandalone)
        //                    { // check standalone partner
        //                        if (oSurvey.IsEmailInvitationEnable == true || oSurvey.IsSmsInvitation == false) //Standalone partner not having email invitation redirect to endpage. Added 06/15/2016
        //                        {
        //                            return Redirect("https://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + oSurvey.ActualInvitationGuid + "&ug=" + ug + "&pid=" + oSurvey.ProjectId + "&cid=" + oSurvey.OrgId); //redirect to endpage
        //                        }
        //                        return Redirect("/cr/sms?uig=" + oSurvey.ActualInvitationGuid + "&ug=" + ug + "&cid=" + clientid +
        //                        "&pid=" + oSurvey.ProjectId + "&tid=" + oSurvey.Targetid + "&usid=" + oSurvey.UserId + "&sn=" + oSurvey.SurveyName);
        //                    }
        //                    else
        //                    {
        //                        return Redirect("/cr/sms?uig=" + oSurvey.ActualInvitationGuid + "&ug=" + ug + "&cid=" + clientid +
        //                       "&pid=" + oSurvey.ProjectId + "&tid=" + oSurvey.Targetid + "&usid=" + oSurvey.UserId + "&sn=" + oSurvey.SurveyName);
        //                    }
        //                }
        //                else
        //                {
        //                    //$scope.isNonUsMember = true;
        //                    return Redirect("http://e.opinionetwork.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&uig=" + oSurvey.ActualInvitationGuid + "&ug=" + ug + "&pid=" + oSurvey.ProjectId + "&cid=" + oSurvey.OrgId); //redirect to endpage
        //                }
        //            }
        //            if (deviceMatchedCount > 0 && string.IsNullOrEmpty(oSurvey.RedirectUrl))
        //            {

        //                return Redirect("/cr/valid?uig=" + oSurvey.UserInvitationId + "&ug=" + ug + "&cid=" + clientid +
        //                     "&pid=" + oSurvey.ProjectId + "&tid=" + oSurvey.Targetid + "&usid=" + oSurvey.UserId + "&usrinvid=" + oSurvey.UsrInvitaitonID.ToString() + "&cstring=" + oSurvey.Cstring + "&lid=" + oSurvey.lid);

        //            }
        //        }
        //        else if (!string.IsNullOrEmpty(oSurvey.RedirectUrl))
        //        {
        //            return Redirect(oSurvey.RedirectUrl + "&pid=" + oSurvey.ProjectId.ToString() + "&ug=" + ug + "&cid=" + clientid.ToString());
        //        }
        //        else
        //        {
        //            return Redirect("/cr/valid?uig=" + oSurvey.UserInvitationId + "&ug=" + ug + "&cid=" + clientid +
        //                 "&pid=" + oSurvey.ProjectId + "&tid=" + oSurvey.Targetid + "&usid=" + oSurvey.UserId + "&usrinvid=" + oSurvey.UsrInvitaitonID.ToString() + "&cstring=" + oSurvey.Cstring + "&lid=" + oSurvey.lid);
        //        }
        //        return Redirect("/reg/pii?uig=" + oSurvey.UserInvitationId + "&ug=" + ug + "&cid=" + clientid +
        //                "&pid=" + oSurvey.ProjectId + "&tid=" + oSurvey.Targetid + "&usid=" + oSurvey.UserId);
        //    }
        //    else
        //    {
        //        return View("/Views/pages/cr.cshtml");//Render the View.
        //    }
        //}

        public ActionResult start(string qig, string ug, int? cid, string rid, string s, string subId, string osid, string fedresid,
  string transid, string project, string sub, string tx_id, string frid, string lid, string sub_id)
        {
            if (!string.IsNullOrEmpty(transid))
            {
                subId = transid;
            }
            if (!string.IsNullOrEmpty(sub))
            {
                subId = sub;
            }
            if (!string.IsNullOrEmpty(sub_id))
            {
                subId = sub_id;
            }
            if (!string.IsNullOrEmpty(tx_id))
            {
                subId = tx_id;
            }
            int clientid = Convert.ToInt32(cid);

            if (clientid == 0)
            {
                clientid = oManager.GetOrgidByUserDPV(ug);
            }
            string u = string.Empty;
            int userTrafficTypeId = 2;
            u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|android|ipad|playbook|silk|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (u != null)
            {
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    userTrafficTypeId = 2;//If Mobile Device Matched:
                }
                else
                {
                    userTrafficTypeId = 3;//If Non Mobile Device Matched.
                }
            }
            else //If the Request.ServerVariables is NULL 
            {
                if (Request.UserAgent != null)
                {
                    if (Request.UserAgent.Contains("Android") || Request.UserAgent.Contains("webOS")
                     || Request.UserAgent.Contains("iPhone") || Request.UserAgent.Contains("iPad")
                     || Request.UserAgent.Contains("iPod") || Request.UserAgent.Contains("BlackBerry") || Request.UserAgent.Contains("Windows Phone"))
                    {
                        userTrafficTypeId = 2;//If the Mobile user Paticiapting the Survey
                    }
                    else
                    {
                        userTrafficTypeId = 3;//If the Non Mobile user is participating in Survey.
                    }
                }
            }
            //To read Browser Proeprties :
            System.Web.HttpBrowserCapabilitiesBase browser = Request.Browser;
            string browserInfo = oManager.BrowserInfo(browser);
            //Added on 9/12/2014 to save the Mobile Device Model.
            string mobiledeviceModel = oManager.DeviceTypeCheck(Request.UserAgent);
            string ipAddress = HttpContext.Request.UserHostAddress;
            BigInteger IpNumber = Dot2LongIP(ipAddress);
            string RedirectUrl = oManager.InsertClick(qig, ug, Convert.ToInt32(project), clientid, rid, s, subId, userTrafficTypeId, mobiledeviceModel, browserInfo,
                                                    Request.UserAgent, ipAddress, osid, fedresid, IpNumber.ToString());
            return Redirect(RedirectUrl);
        }


        #region Save User Click Information
        /// <summary>
        /// Save User Click Information
        /// </summary>
        /// <param name="qg">QuotaGroupId</param>
        /// <param name="ug">UserGuid</param>
        /// <param name="prjId"ProjectId></param>
        /// <param name="rid">ReferrerId</param>
        /// <param name="source">Source</param>
        /// <param name="subId">SubId</param>
        /// <param name="isNew">IsNew</param>
        /// <param name="rvId">RelevantId</param>
        /// <param name="osId">OldSurveyInvitationId</param>
        /// <param name="tScore">TotalScores</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("save")]
        public JsonResult save(string qg, string ug, int prjId, int cid, string rid, string source, string subId, int isNew, string rvId, string osId, string tScore, string vid, int vscore, string fedresid, User radiusData)
        {
            var radius = JObject.Parse(JsonConvert.SerializeObject(radiusData));
            string geodata = radius["geodata"].ToString();
            //client id is null then get organization id by userguid
            if (cid == 0)
            {
                cid = oManager.GetOrgidByUserDPV(ug);
            }
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
            BigInteger IpNumber = Dot2LongIP(ipAddress);
            string IPriskScore = string.Empty;
            string ReferrerUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.ToString();
            }
            //if (ConfigurationManager.AppSettings["MinfraudAPI"].ToString() == "1")
            //{
            //    try
            //    {
            //        var output = MinFraudAsync(ipAddress, Request.UserAgent);
            //        dynamic data = JsonConvert.SerializeObject(output.Result);
            //        var result = JObject.Parse(data);
            //        IPriskScore = result.Result.RiskScore;
            //        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + ipAddress + "|" + IPriskScore + "|" + result;
            //        logger.Trace(logMessage);
            //    }
            //    catch (Exception ex)
            //    {
            //        IPriskScore = "0.00";
            //        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + ipAddress + "|" + IPriskScore + "|" + ex.InnerException.ToString();
            //        logger.Trace(logMessage);
            //    }
            //}
            //else
            //{
            IPriskScore = "0.00";
            //}
            relevantScore = Convert.ToInt32(tScore.Split(';')[0]);
            fpfScores = tScore.Split(';')[1];
            fraudPfScore = Convert.ToInt32(tScore.Split(';')[2]);
            oSurvey = oManager.SaveClickInformation(qg, ug, prjId, cid, rid, source, subId, isNew, userTrafficTypeId, mobiledeviceModel, browserInfo,
                                                           Request.UserAgent, ipAddress, rvId, relevantScore, fpfScores, fraudPfScore, osId, vid, vscore, fedresid, geodata, IPriskScore, IpNumber.ToString());
            return Json(oSurvey, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SendSms
        /// <summary>
        /// Send Sms
        /// </summary>
        /// <param name="uig">UserInvitationGuid</param>
        /// <param name="ug">UserGuid</param>
        /// <param name="prjId">ProjectId</param>
        /// <param name="mobileNum">MobileNumber</param>
        /// <param name="surveyName">SurveyName</param>
        /// <param name="orgId">OrgId</param>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("SendSms")]
        public int SendSms(string uig, string ug, string prjId, string mobileNum, string surveyName, string orgId)
        {
            return oManager.SendSms(uig, ug, prjId, mobileNum, surveyName, orgId);
        }
        #endregion

        #region checkradius
        /// <summary>
        /// checkradius
        /// </summary>
        /// <param name="radiusData"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("checkradius")]
        public int checkradius(User radiusData)
        {

            var radius = JObject.Parse(JsonConvert.SerializeObject(radiusData));
            return oManager.checkradius(radius["UserGuid"].ToString(), radius["geodata"].ToString());
        }
        #endregion

        public ActionResult Error()
        {
            return View("/Views/pages/error.cshtml");
        }

        #region IpAddress2IPnumber
        public BigInteger Dot2LongIP(string ipv6)
        {
            System.Net.IPAddress address;
            System.Numerics.BigInteger ipnum;

            if (System.Net.IPAddress.TryParse(ipv6, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();
                if (System.BitConverter.IsLittleEndian)
                {
                    System.Collections.Generic.List<byte> byteList = new System.Collections.Generic.List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }
                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = System.BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += System.BitConverter.ToUInt64(addrBytes, 0);
                }
                else
                {
                    //IPv4
                    ipnum = System.BitConverter.ToUInt32(addrBytes, 0);
                }
                return ipnum;
            }
            else
            {
                return 0;
            }

        }
        #endregion

        #region Minfraud API
        public async Task<dynamic> MinFraudAsync(String Ipaddress, string UserAgent)
        {
            var transaction = new Transaction(
                device: new Device(
                    ipAddress: System.Net.IPAddress.Parse(Ipaddress),
                    userAgent: UserAgent,
                    acceptLanguage: "en-US,en;q=0.8"
                )
            );
            int AccountID = Convert.ToInt32(ConfigurationManager.AppSettings["MaxmindAccountID"]);
            string Key = ConfigurationManager.AppSettings["Maxmindkey"].ToString();
            using (var client = new WebServiceClient(AccountID, Key))
            {
                // Use `InsightsAsync` if querying Insights
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var response = client.ScoreAsync(transaction);
                response.Wait();
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.Converters.Add(new IPConverter());
                var json = JsonConvert.SerializeObject(response, Formatting.Indented, jsonSettings);// json string
                dynamic output = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                return output;
            }
        }
        #endregion

        public ActionResult Valid()
        {
            return View("/Views/pages/vld.cshtml");//Render the View.
        }
        public ActionResult relevent()
        {
            return View("/Views/pages/r.cshtml");//Render the View.
        }
        public ActionResult SMS()
        {
            return View("/Views/pages/sms.cshtml");
        }
        public JsonResult Check(Int64 userId, int cnsid)
        {
            //call the check sp
            //if null check again else redirect to next page.
            RelevantCheck check = oManager.Check(userId, cnsid);
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        [Route("InsertValidations")]
        public JsonResult InsertValidations(int usid, Guid uig, int isNew, string rvId, string tScore, int cid, User radiusData)
        {
            string browserInfo = string.Empty;
            Surveys oSurvey = new Surveys();
            var radius = JObject.Parse(JsonConvert.SerializeObject(radiusData));
            string geodata = radius["geodata"].ToString();
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
            int relevantScore = 0;
            string fpfScores = string.Empty;
            int fraudPfScore = 0;
            relevantScore = Convert.ToInt32(tScore.Split(';')[0]);
            fpfScores = tScore.Split(';')[1];
            fraudPfScore = Convert.ToInt32(tScore.Split(';')[2]);
            oSurvey.RedirectUrl = oManager.InsertRelevantData(uig, rvId, relevantScore, fpfScores, fraudPfScore, Convert.ToBoolean(isNew), browserInfo, Request.UserAgent, usid, cid, geodata);
            return Json(oSurvey, JsonRequestBehavior.AllowGet);
        }
        public string GetToken(string url, string publishkey)
        {
            try
            {
                Stream dataStream;
                string tokenurl = ConfigurationManager.AppSettings["token"].ToString();
                url = url + tokenurl;
                url = string.Format(url, publishkey);
                WebResponse response = oManager.Get(url, null);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                Console.WriteLine(reader.ReadToEnd());
                ResearchDefenderToken.Root myDeserializedClass = JsonConvert.DeserializeObject<ResearchDefenderToken.Root>(responseFromServer);
                string token = myDeserializedClass.results[0].token;
                return token;

            }
            catch (Exception ex)
            {
                logger.Trace("Research Defender GetToken Method" + ex.ToString());
                return null;
            }
        }

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

        //public JsonResult RdjsonInsert(int userid, string uid, int cid, string uig, string ug, int pid, string cleanIDJson,string IpqsJSON)
        //{
        //    //call the check sp
        //    //if null check again else redirect to next page.
        //    User objUser = new User();
        //    //string RequestURL = string.Empty;
        //    //RequestURL = System.Configuration.ConfigurationManager.AppSettings["ResearchDefender"].ToString();
        //    //RequestURL = string.Format(RequestURL, ug, pid, uig, geoLonLat, passParam);
        //    //string json = GetrdJson(RequestURL, token);
        //    //string _ipqsPostbackurl = ConfigurationManager.AppSettings["IPQSPostbackAPI"].ToString();
        //    //_ipqsPostbackurl = string.Format(_ipqsPostbackurl, uid);
        //    //string IpqsJSON = PostMethod(_ipqsPostbackurl);
        //    logger.Info("Invitation GUID:" + uig + "User GUID:" + ug + "| Clean ID JSON |" + cleanIDJson + "| IPQS JSON |" + IpqsJSON);
        //    objUser = oManager.RdjsonInsert(userid, uid, cid, uig, cleanIDJson, IpqsJSON);
        //    return Json(objUser, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult RdjsonInsert(int userid, string uid, int cid, string uig, string ug, string dvtype, string sessionId = null, string dfiqJson = null)
        {
            User userObj = new User();
            logger.Info("Invitation GUID:" + uig + "User GUID:" + ug + "| Verisoul Session |" + sessionId + "| DFIQ Json |" + dfiqJson);
            string clientIp = Request.ServerVariables["REMOTE_ADDR"].ToString();
            userObj.RedirectURL = oManager.RdjsonInsert(userid, uid, cid, uig, ug, clientIp,dvtype,sessionId, dfiqJson);
            return Json(userObj, JsonRequestBehavior.AllowGet);
        }


        #region Get org details	
        [HttpGet]
        public JsonResult GetCurrentDomainDetails(int cid)
        {
            Client oClient = new Client();
            var uri = new Uri(Request.Url.OriginalString.ToString());
            string Hosturl = uri.Scheme + "://" + uri.Host;
            oClient = oManager.GetClientDetailsByRid(Hosturl, null, cid);
            return Json(oClient, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public string PostMethod(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = RequestURL;
            byte[] data = encoding.GetBytes(postData);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/json; charset=UTF-8";
            LoginRequest.ContentLength = data.Length;
            LoginRequest.Timeout = 2000;
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
    }
}