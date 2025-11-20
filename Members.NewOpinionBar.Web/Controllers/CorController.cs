using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using Members.NewOpinionBar.Web.Filters;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;


namespace Members.NewOpinionBar.Web.Controllers
{
    [RoutePrefix("Cor")]
    public class CorController : BaseController
    {
        UserManager objManager = new UserManager();
        // GET: Cor
        public ActionResult Index()
        {
            return View();
        }

        #region Save User Details
        /// <summary>
        /// Save User Details
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>
        [Route("extuserInsert")]
        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public string extuserInsert(User oUser)
        {
            User objUser = new OpinionBar.Components.Entities.User();

            objUser.IpAddress = IpAddress;
            objUser.ReferrerUrl = RefererUrl;
            objUser.DomainUrl = Request.Url.Host;
            objUser.Dob = oUser.Month + "/" + oUser.Day + "/" + oUser.Year;
            objUser.CountryId = oUser.CountryId;
            objUser.EthnicityId = oUser.EthnicityId;
            objUser.ZipCode = oUser.ZipCode;
            objUser.Gender = oUser.Gender;
            objUser.rfid = oUser.rfid; //referrer guid .
            objUser.SubId3 = oUser.SubId3; //we wil pass external Member ID
            objUser.rfid = oUser.rfid; //referrer guid
            objUser.ExtId = oUser.ExtId; //Pass Org Guid
            objUser.SubId2 = oUser.SubId2; //we wil pass external Member ID
            objUser.TransactionId = oUser.TransactionId; //Pass Org Guid
            UserManager objManager = new UserManager();
            Guid _userguid = objManager.RouteruserInsert(objUser);

            return _userguid.ToString();

        }
        #endregion


        public ActionResult s1(int rid, string txid, string sid, string fid, string trans_id, string fn, string ln, string em, string dob, string lname, string prjid)
        {
            string[] IpAddress = { };
            string IpCheck = string.Empty;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                IpAddress = IpCheck.Split(',');
            }
            string ReferrerUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            }
            RefererUrl = ReferrerUrl;
            string authkey = objManager.InsertAffClicks(rid, txid, IpAddress[0], sid, fid, trans_id, RefererUrl, 0);
            if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower())
            {
                ViewBag.OrgName = "Conversant.Pro";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/Conversant_Logo.png";
                ViewBag.Paragraph0 = "Welcome to Conversant.Pro!";
            }
            else
            {
                ViewBag.OrgName = "Opinionbar";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/opinionbar_logo.png";
                ViewBag.Paragraph0 = "Welcome to Opinionbar!";

            }

            ViewBag.CountryCode = "en";

            return View("/Views/cor/s1.cshtml");
        }

        public ActionResult ps1(int rid, string txid, string sid, string fid, string trans_id, string fn, string ln, string em, string dob, string lname)
        {
            string[] IpAddress = { };
            string IpCheck = string.Empty;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                IpAddress = IpCheck.Split(',');
            }
            string ReferrerUrl = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.AbsoluteUri;
            }
            RefererUrl = ReferrerUrl;
            string authkey = objManager.InsertAffClicks(rid, txid, IpAddress[0], sid, fid, trans_id, RefererUrl, 0);
            if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower())
            {
                ViewBag.OrgName = "Conversant.Pro";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/Conversant_Logo.png";
                ViewBag.Paragraph0 = "Welcome to Conversant.Pro!";
            }
            else
            {
                ViewBag.OrgName = "Opinionbar";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/opinionbar_logo.png";
                ViewBag.Paragraph0 = "Welcome to Opinionbar!";

            }

            ViewBag.CountryCode = "en";

            return View("/Views/cor/ps1.cshtml");
        }
        public ActionResult s2()
        {
            return View();
        }

        public ActionResult p()
        {
            return View("/Views/cor/Top20.cshtml");
        }
        public ActionResult p2()
        {
            return View("/Views/cor/Top10.cshtml");
        }
        public ActionResult lbs()
        {
            return View("/Views/cor/lbmsg.cshtml");
        }
        public ActionResult msglb(int rid, string txid, string into, string sid, string fid, string trans_id, string prjid)
        {
            string[] IpAddress = { };
            string IpCheck = string.Empty;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                IpAddress = IpCheck.Split(',');
            }
            string ReferrerUrl = Request.Url.AbsoluteUri;
            string authkey = objManager.InsertAffClicks(rid, txid, IpAddress[0], sid, fid, trans_id, ReferrerUrl, 0);
            string url = string.Empty;
            if (into == "n" && Convert.ToInt32(prjid) > 0)
            {
                url = surveygetbyprjid(rid, prjid);
                if (!string.IsNullOrEmpty(url)) //redirect to Survey URL.
                {
                    return Redirect(url);
                }
                else
                {
                    return View("/Views/cor/msglb.cshtml");
                }

            }
            else
            {
                return View("/Views/cor/msglb.cshtml");
            }

        }
        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult Top20Get(string ug, string pg, int ispeerly2)
        {
            List<ProfileQuestions> lstQuestion = new List<ProfileQuestions>();
            UserManager objManager = new UserManager();
            var Pagedata = objManager.GetquestionsforTop20(new Guid(ug), pg, ispeerly2);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Top 20 Save
        /// <summary>
        /// Top 200 Profiles Save
        /// </summary>
        /// <param name="leadguid"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public string Top20Save(string ug, string xml)
        {
            string _message = string.Empty;
            UserManager objManager = new UserManager();
            _message = objManager.Top20SaveOptions(xml, new Guid(ug));
            return _message;
        }
        #endregion


        #region Get top1 Survey Matched
        /// <summary>
        /// Save User Details
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>
        public ActionResult surveyget(string userGuid, string authkey, string isp2, string prjid)
        {
            int _orgId = 0;
            string u = string.Empty;
            int userTrafficTypeId = 2;
            HttpClient client = new HttpClient();
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
                    u = Request.UserAgent;
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
            string _url = string.Empty;
            if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower() || authkey.ToLower() == "9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9".ToLower())
            {
                if (authkey.ToLower() == "9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9".ToLower())
                {
                    _orgId = 541; //OB
                }
                else if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower())
                {

                    _orgId = 542;//Conversant
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var content = new StringContent("", Encoding.UTF8, "application/json");
                string Url = ConfigurationManager.AppSettings["gsapiurl"].ToString();
                client.BaseAddress = new Uri(Url);
                //HTTP GET

                var responseTask = client.GetStringAsync("SurveyGetRouter?userGuid=" + userGuid + "&clientId=" + _orgId + "&authKey=" + authkey + "&prjid=" + prjid + "&dtypeid=" + userTrafficTypeId);

                responseTask.Wait();
                var url = responseTask.Result;
                url = url.Replace(@"""", "");
                if (!string.IsNullOrEmpty(prjid) && string.IsNullOrEmpty(url))
                {
                    responseTask = client.GetStringAsync("SurveyGetRouter?userGuid=" + userGuid + "&clientId=" + _orgId + "&authKey=" + authkey + "&prjid=0&dtypeid=" + userTrafficTypeId);
                    responseTask.Wait();
                    url = responseTask.Result;
                    url = url.Replace(@"""", "");
                }
                if (!string.IsNullOrEmpty(url)) //redirect to Survey URL.
                {
                    return Redirect(url);
                }
                else
                {
                    if (isp2 == "t")
                    {
                        return Redirect("/cor/lbs?ug=" + userGuid.ToString() +
                                                       "&pg=" + authkey + "&extid=" + "" + "&pc=na" + "&is_t=h" + "&is_sim=f");
                    }
                    else
                    {
                        return Redirect("/cor/lbs?ug=" + userGuid.ToString() +
                                                       "&pg=" + authkey + "&extid=" + "" + "&pc=na" + "&is_t=h" + "&is_sim=f");
                    }
                    //redirect to LB Message page.

                }

            }

            else
            {
                return Redirect("/cor/psrend?userGuid=" + userGuid + "&authkey=" + authkey);  // redirect to no Surveys found page.
            }
        }
        #endregion

        public string surveygetbyprjid(int rid, string prjid)
        {
            string Url = string.Empty;

            UserManager objManager = new UserManager();
            Url = objManager.surveygetbyprjid(prjid, rid, IpAddress);

            //if (!string.IsNullOrEmpty(Url)) //redirect to Survey URL.
            //{
            //    return Redirect(Url);
            //}
            return Url;
        }


        public ActionResult psrend(string userGuid, string authkey)
        {

            if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower())
            {
                ViewBag.OrgName = "Conversant.Pro";
                ViewBag.Logo = "https://www.opinionetwork.com/Images/Conversant_Logo.png";

            }
            else
            {
                ViewBag.OrgName = "Opinionbar";
                ViewBag.Logo = "https://www.opinionetwork.com/Images/opinionbar_logo.png";
            }

            ViewBag.CountryCode = "en";
            ViewBag.Message = "Thank you for participating!  Unfortunately you didn’t qualify to complete this survey";
            return View("/Views/cor/psrend.cshtml");
        }

        #region Get pixel Script
        /// <summary>
        /// Get pixel Script
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        [Route("GetScript")]
        [HttpGet]
        public JsonResult GetScript(string ug)
        {
            List<pixel> objpixel = new List<pixel>();
            UserManager objUserManager = new UserManager();
            objpixel = objUserManager.GetScript(ug, 3);//survey completes.
            return Json(objpixel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ps2(int rid, string txid, string sid, string fid, string trans_id, string fn, string ln, string em, string dob, string lname, int isClick)
        {
            string[] IpAddress = { };
            string IpCheck = string.Empty;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(IpCheck))
            {
                IpAddress = IpCheck.Split(',');
            }
            string authkey = objManager.InsertAffClicks(rid, txid, IpAddress[0], sid, fid, trans_id, RefererUrl, isClick);
            if (authkey.ToLower() == "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761".ToLower())
            {
                ViewBag.OrgName = "Conversant.Pro";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/Conversant_Logo.png";
                ViewBag.Paragraph0 = "Welcome to Conversant.Pro!";
            }
            else
            {
                ViewBag.OrgName = "Opinionbar";

                ViewBag.Logo = "https://www.opinionetwork.com/Images/opinionbar_logo.png";
                ViewBag.Paragraph0 = "Welcome to Opinionbar!";

            }

            ViewBag.CountryCode = "en";

            return View("/Views/cor/ps2.cshtml");
        }
    }
}