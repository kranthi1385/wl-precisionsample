using Members.PrecisionSample.Common.Security;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Data_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Web.Filters;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using NLog;
using System.Security.Cryptography;

namespace Members.PrecisionSample.Web.Controllers.partner
{
    [RoutePrefix("Reg")]
    public class RegController : BaseController
    {
        // GET: Reg
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            return View();
        }

        #region Reg Action method 
        [ValidateJsonAntiForgeryToken]
        public ActionResult signup()
        {
            MemberEntity objMemberEntity = new MemberEntity();
            if (Identity.Current != null)
            {
                ViewBag.UserId = Identity.Current.UserData.UserId;
                ViewBag.UserGuid = Identity.Current.UserData.UserGuid;
            }
            return View("/Views/Partner/reg.cshtml", objMemberEntity);

        }
        #endregion

        #region Relevent Action method 
        [ValidateJsonAntiForgeryToken]
        public ActionResult Rel()
        {
            return View("/Views/Partner/rel.cshtml");

        }
        #endregion

        #region Home Action method 
        [ValidateJsonAntiForgeryToken]
        public ActionResult Home()
        {
            User objMemberEntity = new User();
            return View("/Views/Partner/hm.cshtml");

        }
        #endregion

        #region RC Action method 
        [ValidateJsonAntiForgeryToken]
        public ActionResult RC()
        {
            return View("/Views/Partner/rc.cshtml");

        }
        #endregion

        #region GetEmailfromcookie
        [Route("GetEmailfromcookie")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        //Read From Cookie
        public string GetEmailfromcookie(string cookieId)
        {
            string email_address = string.Empty;
            string sCookieName = cookieId;
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
        #endregion

        #region WriteCookie
        [Route("WriteCookie")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public void WriteCookie(string ea)
        {
            string email_address = string.Empty;
            string sCookieName = ea;
            HttpCookie accountNameCookie = new HttpCookie(sCookieName);
            Response.Cookies.Remove(sCookieName);
            Response.Cookies.Add(accountNameCookie);
            accountNameCookie.Values.Add("AccountName", ea);
            email_address = ea;

        }
        #endregion

        #region SaveMemberClickedSku
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public ActionResult RedeemClick(string lc)
        {
            if (!string.IsNullOrEmpty(lc))
            {
                ViewBag.recaptchalangCode = lc;
            }
            else
            {
                ViewBag.recaptchalangCode = "en";
            }
            return View("/Views/Partner/trr.cshtml");
        }
        #endregion

        #region CheckMemberDetails
        [Route("CheckMemberDetails")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult CheckMemberDetails(string r, string ExtId)
        {
            UserManager objUserManager = new UserManager();
            var Pagedata = objUserManager.UserDeialscCheck(Convert.ToInt32(r), ExtId, Request.Url.Host);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CheckMemberByEmail
        [Route("CheckMemberByEmail")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult CheckMemberByEmail(string r, string EmailAddress)
        {
            UserManager objUserManager = new UserManager();
            var Pagedata = objUserManager.UserDeialscCheckByEmailAddress(Convert.ToInt32(r), EmailAddress, Request.Url.Host);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BindData
        [Route("BindData")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        //Read From Cookie
        public JsonResult BindData()
        {
            return Json(new MemberEntity(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region client details
        [Route("ClientDetails")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetClientDetails(string ug, int cid)
        {
            Client objClient = new Client();
            UserManager objManager = new UserManager();

            objClient = objManager.GetClientDetailsByRid(null, null, cid);

            if (objClient.ClientId == 70)
            {
                objClient.OrgName = "OpinioNetwork";
            }
            else
            {
                //objClient.OrgName = MemberIdentity.Client.OrgName;
            }
            //objClient.IsRewardsShow = MemberIdentity.Client.IsRewardsShow; //Added on 05/20/2015 for show rewards to partner;
            return Json(objClient, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Widget Domain Details
        [Route("GetWidgetDomainDetails")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetWidgetDomainDetails(int rid)
        {
            UserDataServices oDataServer = new UserDataServices();
            var pagadata = oDataServer.GetClientDetailsByRid(null, rid, null);
            return Json(pagadata, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sku store in session
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public void SetSKUSes(string sku)
        {
            if (!string.IsNullOrEmpty(sku))
            {
                Session["Sku"] = sku.ToString();

            }
        }
        #endregion

        #region Validate Captcha
        /// <summary>
        /// Validate Captcha
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult ValidateCaptcha(Guid ug, Guid uig)
        {
            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            const string secret = "6LeYOw8UAAAAAFWJ2pPIS7D7_YQ1AQxC_6RfRSJ1";
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" + response,
                secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (captchaResponse.success)
            {

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region get Sku store from session
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetSKUSes()
        {
            var pagadata = Session["SKu"].ToString();
            return Json(pagadata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sku clear in session
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public void ClearSKUSes(string sku)
        {
            if (!string.IsNullOrEmpty(sku))
            {
                Session["SKu"] = null;
            }
        }
        #endregion

        #region Insert reward redemptions
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult RedeemMemberRewards(string Sku, decimal Ut, int Points, Guid ug, int cid)
        {
            string ip = string.Empty;
            ip = Request.UserHostAddress;
            RewardManager objRewardManager = new RewardManager();
            User objMemberEntity = new User();
            UserManager oManager = new UserManager();
            objMemberEntity = oManager.GetUserData(ug.ToString(), null, cid);
            objRewardManager.RedeemMemberRewards(Sku, Ut, objMemberEntity.UserId, Points, cid, objMemberEntity.FirstName, objMemberEntity.EmailAddress, ug, ip);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get reward details
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardDetails(string ug, string sku, int cid, string name)
        {
            RewardManager obj = new RewardManager();
            TRewards objTrewards = obj.GeTangoRewardsBySKU(sku, ug, "english", cid, name);
            User objMemberEntity = new User();
            UserManager objManager = new UserManager();

            objMemberEntity = objManager.GetUserData(ug.ToString(), null, cid);
            HttpClient client = new HttpClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?UserGuid=" + objMemberEntity.UserGuid + "&UserId=" + Convert.ToInt32(objMemberEntity.UserId) +
                "&ClientId=" + cid, content).Result;
            var jsonString = result.Content.ReadAsStringAsync().Result;
            Rewards objRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            objTrewards.AccountBalance = objRewards.AccountBalance;
            return Json(objTrewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Survey List
        [Route("GetUserSurveyList")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetUserSurveyList(string ug, int cid)
        {
            string u = string.Empty;
            int userTrafficTypeId = 2;
            string browserInfo = string.Empty;
            string mobiledeviceModel = string.Empty;
            string ipAddress = string.Empty;
            string fpfScores = string.Empty;
            ipAddress = Request.UserHostAddress;
            Logger.Trace($"Partner Controller - IP Address: {ipAddress}");
            List<Surveys> lstSurveys = new List<Surveys>();
            //lstSurvey = oSurveyManager.GetAllAvaliableSurveys(Convert.ToInt32(UsId));

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            //var content = new StringContent(ug, Encoding.UTF8, "application/json");
            //HttpResponseMessage response = client.PostAsync("api/Member/GetSurveys?UserGuid=" + ug + "&ClientId=" + cid.ToString(), content).Result;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var content = new StringContent("", Encoding.UTF8, "application/json");
            string Url = ConfigurationManager.AppSettings["gsapiurl"].ToString();
            client.BaseAddress = new Uri(Url);
            //HTTP GET
            var responseTask = client.GetStringAsync("SurveysGet?userGuid=" + ug + "&clientId=" + cid.ToString() + "&ipAddress=" + ipAddress);
            responseTask.Wait();
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
            var jsonString = responseTask.Result;
            if (jsonString.Contains("No Survey"))
            {
                lstSurveys = null;
            }
            else
            {
                lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
                lstSurveys = lstSurveys.Where(x => x.SurveyUserTypeIds.Contains(userTrafficTypeId.ToString())).ToList();
            }

            return Json(lstSurveys, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Profile List
        [Route("GetProfileList")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetProfileList(string ug, int cid)
        {
            List<Profile> lstProfiles = new List<Profile>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(ug, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetProfile?UserGuid=" + ug + "&ClientId=" + cid.ToString(), content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString.Contains("No Profile List was found."))
            {
                lstProfiles = null;
            }
            else
            {
                lstProfiles = new JavaScriptSerializer().Deserialize<List<Profile>>(jsonString);
            }

            return Json(lstProfiles, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetSurveyHistory
        [Route("GetSurveyHistory")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetSurveyHistory(string ug, int cid)
        {
            List<SurveyHistory> lstSurveyHistory = new List<SurveyHistory>();
            //lstSurvey = oSurveyManager.GetAllAvaliableSurveys(Convert.ToInt32(UsId));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(ug, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetSurveysHistory?UserGuid=" + ug + "&ClientId=" + cid.ToString(), content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString.Contains("No Survey"))
            {
                lstSurveyHistory = null;
            }
            else
            {
                lstSurveyHistory = new JavaScriptSerializer().Deserialize<List<SurveyHistory>>(jsonString);
            }

            return Json(lstSurveyHistory, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get reward details
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetCtlgDetailsById(string ug, string cg, int cid)
        {
            RewardManager objRewardManager = new RewardManager();
            Rewards objRewards = new Components.Entities.Rewards();
            if (!string.IsNullOrEmpty(cg))
                if (new Guid(cg) != Guid.Empty)
                {
                    objRewards = objRewardManager.GetDetailsById(new Guid(cg), new Guid(Identity.Current.UserData.UserGuid), cid);
                    objRewards.RewardDescription = objRewards.RewardDescription.Replace("%%email_address%%", Identity.Current.UserData.EmailAddress);
                    User objMemberEntity = new User();
                    UserManager omanager = new UserManager();
                    objMemberEntity = omanager.GetUserData(ug.ToString(), null, cid);
                    HttpClient client = new HttpClient();
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?UserGuid=" + objMemberEntity.UserGuid + "&ClientId="
                        + cid.ToString() + "&UserId=" + Convert.ToInt32(objMemberEntity.UserId), content).Result;
                    var jsonString = result.Content.ReadAsStringAsync().Result;
                    Rewards objRewards1 = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
                    objRewards.AccountBalance = objRewards1.AccountBalance;
                    objRewards.LstRewardAmount = new List<RewardAmount>();
                    for (int i = 1; i <= Convert.ToDecimal(objRewards.AccountBalance) / objRewards.MinRedemptionAmount; i++)
                    {
                        RewardAmount obj = new RewardAmount();
                        obj.Key = (objRewards.MinRedemptionAmount * i).ToString();
                        obj.Value = (objRewards.MinRedemptionAmount * i).ToString();
                        objRewards.LstRewardAmount.Add(obj);

                    }
                }
            return Json(objRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion
        //#region InserrtRewardRedeemprtions
        //[HttpGet]
        //[ValidateJsonAntiForgeryToken]
        //public JsonResult InserrtRewardRedeemprtions(string amount, string cg)
        //{
        //    RewardManager obj = new RewardManager();
        //    User objMemberEntity = new User();
        //    objMemberEntity = GetUserDetails(Identity.Current.UserData.UserGuid.ToString());

        //    int redeemId = obj.InserrtRewardRedeemprtions(Convert.ToInt32(amount), new Guid(cg), Convert.ToInt32(objMemberEntity.UserId), MemberIdentity.Client.ClientId, objMemberEntity.UserGuid.ToString());
        //    return Json(redeemId, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region  Get Reward Data
        /// <summary>
        /// Get Reward Data
        /// </summary>
        /// <returns></returns>
        [Route("GetRewardData")]
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardData(string ug, int cid)
        {
            string userName = string.Empty;
            string password = string.Empty;
            string Result = string.Empty;
            decimal CurrentAcBalance = 0;
            User objMemberEntity = new User();
            UserManager oManager = new UserManager();

            objMemberEntity = oManager.GetUserData(ug.ToString(), null, cid);
            RewardManager oRewardManager = new RewardManager();
            List<Rewards> lstRewards = new List<Rewards>();
            Rewards oRewards = new Rewards();
            lstRewards = oRewardManager.RewardOptionGet(Convert.ToInt32(objMemberEntity.UserId), objMemberEntity.UserGuid.ToString(), cid);

            //oRewards = oRewardManager.GetRewardsHistory(Convert.ToInt32(Identity.Current.UserData.UserId));
            HttpClient client = new HttpClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?userGuid=" + ug + "&userId=" + Convert.ToInt32(objMemberEntity.UserId) +
                "&clientId=" + objMemberEntity.OrgId.ToString(), content).Result;
            var jsonString = response1.Content.ReadAsStringAsync().Result;

            // "No Reward History was found"


            // oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);

            if (jsonString.Contains("Reward History"))
            {
                oRewards = null;
            }
            else
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }


            if (oRewards != null)
            {
                CurrentAcBalance = Convert.ToDecimal(oRewards.AccountBalance);
                if (objMemberEntity.OrgId == 73 || objMemberEntity.OrgId == 111)
                {
                    CurrentAcBalance = Convert.ToInt32(CurrentAcBalance / 200);
                    for (var k = 0; k < lstRewards.Count; k++)
                    {
                        lstRewards[k].MinRedemptionAmount = Convert.ToInt32(lstRewards[k].MinRedemptionAmount / 200);
                    }
                }
            }

            string CountryCode = string.Empty;
            RewardManager objRewardManager = new RewardManager();
            XmlDocument doc = new XmlDocument();
            userName = "PrecisionSample";
            password = "phiUTYnta!Gmyx?ojesplYb?H$lOyBtXUMJzqcXp@TGv";
            List<TRewards> lstTRewards = new List<TRewards>();
            List<TRewards> lstUSTRewards = new List<TRewards>();
            List<TRewards> lstDBTRewards = new List<TRewards>();
            if (lstRewards.Count > 0)
            {
                foreach (Rewards objRewards in lstRewards)
                {
                    TRewards objTRewards = new TRewards();
                    if (objRewards.RewardName.Contains("PayPal"))
                    {
                        List<Reward> lstReward = new List<Reward>();

                        if (Convert.ToInt32(CurrentAcBalance) >= objRewards.MinRedemptionAmount)
                        {
                            var j = 0;
                            objTRewards.Category = "Retail Gift Card";
                            objTRewards.Description = objRewards.RewardName;
                            objTRewards.CatalougeGuid = objRewards.CatalogueGuid;
                            // objTRewards.UserGuid = new Guid(Identity.Current.UserData.UserGuid);
                            // objTRewards.ImageURL = "/uploads/" + objRewards.RewardLogo;
                            objTRewards.ImageURL = ConfigurationManager.AppSettings["PaypalLogo"].ToString();
                            for (var i = 20; i <= Convert.ToInt32(CurrentAcBalance); i++)
                            {
                                Reward objReward = new Reward();
                                objReward.Description = objRewards.RewardName;
                                objReward.AccountBalance = CurrentAcBalance;
                                objReward.CurrencyCode = "USD";
                                objReward.CurrencyType = "USD";
                                objReward.Denomination = i;
                                objReward.IsVariable = "FIXED_VALUE";
                                objReward.IsDisable = false;

                                j = j + 1;
                                i = i + 19;

                                objTRewards.Reward.Add(objReward);
                            }
                        }
                        else
                        {
                            objTRewards.Category = "Retail Gift Card";
                            objTRewards.Description = objRewards.RewardName;
                            objTRewards.CatalougeGuid = objRewards.CatalogueGuid;
                            //objTRewards.UserGuid = new Guid(Identity.Current.UserData.UserGuid);
                            //objTRewards.ImageURL = "/uploads/" + objRewards.RewardLogo;
                            objTRewards.ImageURL = ConfigurationManager.AppSettings["PaypalLogo"].ToString();
                            Reward objReward = new Reward();
                            objReward.Description = objRewards.RewardName;
                            objReward.AccountBalance = CurrentAcBalance;
                            objReward.CurrencyCode = "USD";
                            objReward.CurrencyType = "USD";
                            objReward.Denomination = 20;
                            objReward.IsVariable = "FIXED_VALUE";
                            objReward.IsDisable = true;
                            objTRewards.Reward.Add(objReward);
                        }

                        lstTRewards.Add(objTRewards);

                    }
                    else
                    {
                        if (objRewards.RewardName.Contains("Magazine"))
                        {
                            if (CurrentAcBalance >= objRewards.MinRedemptionAmount)
                            {
                                var j = 0;
                                objTRewards.Category = "Retail Gift Card";
                                objTRewards.Description = objRewards.RewardName;
                                objTRewards.CatalougeGuid = objRewards.CatalogueGuid;
                                //objTRewards.UserGuid = new Guid(Identity.Current.UserData.UserGuid);
                                // objTRewards.ImageURL = "/uploads/" + objRewards.RewardLogo;
                                objTRewards.ImageURL = ConfigurationManager.AppSettings["MagazineLogo"].ToString();
                                for (var i = 10; i <= Convert.ToInt32(CurrentAcBalance); i++)
                                {
                                    Reward objReward = new Reward();
                                    objReward.Description = objRewards.RewardName;
                                    objReward.AccountBalance = CurrentAcBalance;
                                    objReward.CurrencyCode = "USD";
                                    objReward.CurrencyType = "USD";
                                    objReward.Denomination = i;
                                    objReward.IsVariable = "FIXED_VALUE";
                                    objReward.IsDisable = false;
                                    j = j + 1;
                                    i = i + 9;
                                    objTRewards.Reward.Add(objReward);
                                }
                            }
                            else
                            {
                                objTRewards.Category = "Retail Gift Card";
                                objTRewards.Description = objRewards.RewardName;
                                objTRewards.CatalougeGuid = objRewards.CatalogueGuid;
                                //objTRewards.UserGuid = new Guid(Identity.Current.UserData.UserGuid);
                                //objTRewards.ImageURL = "/uploads/" + objRewards.RewardLogo;
                                objTRewards.ImageURL = ConfigurationManager.AppSettings["MagazineLogo"].ToString();
                                Reward objReward = new Reward();
                                objReward.Description = objRewards.RewardName;
                                objReward.AccountBalance = CurrentAcBalance;
                                objReward.CurrencyCode = "USD";
                                objReward.CurrencyType = "USD";
                                objReward.Denomination = 10;
                                objReward.IsVariable = "FIXED_VALUE";
                                objReward.IsDisable = true;
                                objTRewards.Reward.Add(objReward);
                            }
                            lstTRewards.Add(objTRewards);
                        }
                    }
                }
            }
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.tangocard.com/raas/v2/catalogs?verbose=true");
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
            //request.Headers.Add("Authorization", "Basic " + authInfo);
            //NetworkCredential myCreds = new NetworkCredential(UserName, Password);
            request.Method = "GET";
            //request.ContentType = "application/xml; charset=utf-8";
            request.Accept = "application/json";
            request.Headers.Add("Authorization", "Basic " + encoded);
            try
            {
                lstDBTRewards = objRewardManager.GetTangoRewards(ug.ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    Result = reader.ReadToEnd();

                }
                doc = JsonConvert.DeserializeXmlNode(Result, "root");
                XmlNodeList xnList = doc.SelectNodes("/root/brands");


                foreach (XmlNode xn in xnList)
                {
                    TRewards objTRewards = new TRewards();
                    objTRewards.Description = xn["brandName"].InnerText;
                    //objTRewards.Description = xn["description"].InnerText;
                    //objTRewards.ImageURL = xn["imageUrls"].InnerXml;
                    XmlNodeList xnListImg = xn.SelectNodes("imageUrls");
                    foreach (XmlNode img in xnListImg)
                    {
                        objTRewards.ImageURL = img["_x0031_30w-326ppi"].InnerText;
                    }
                    //objTRewards.UserGuid = new Guid(Identity.Current.UserData.UserGuid);
                    //XmlNodeList InnerxnList = xn.SelectNodes("/root/brands/rewards");
                    XmlNodeList InnerxnList = xn.ChildNodes;


                    foreach (XmlNode InnerXN in InnerxnList)
                    {
                        if (InnerXN.Name == "items")
                        {

                            //if ((Convert.ToInt32(InnerXN["unit_price"].InnerText) != -1 && (Convert.ToDecimal(InnerXN["unit_price"].InnerText)) / 100 >= 10 && (Convert.ToDecimal(InnerXN["unit_price"].InnerText)) / 100 <= 100) || Convert.ToInt32(InnerXN["unit_price"].InnerText) == -1)
                            if (Convert.ToString(InnerXN["valueType"].InnerText) == "VARIABLE_VALUE" || Convert.ToString(InnerXN["valueType"].InnerText) == "FIXED_VALUE")
                            {
                                Reward objReward = new Reward();
                                objReward.RewardName = objTRewards.Description;
                                objReward.ImageURL = objTRewards.ImageURL;


                                if (InnerXN.SelectSingleNode("utid") != null)
                                    objReward.Sku = InnerXN["utid"].InnerText;

                                if (InnerXN.SelectSingleNode("rewardName") != null)
                                    objReward.Description = InnerXN["rewardName"].InnerText;

                                //if (InnerXN.SelectSingleNode("currency_type") != null)
                                //    objReward.CurrencyType = InnerXN["currency_type"].InnerText;

                                if (InnerXN.SelectSingleNode("currencyCode") != null)
                                    objReward.CurrencyCode = InnerXN["currencyCode"].InnerText;

                                if (InnerXN.SelectSingleNode("faceValue") != null)
                                    objReward.Denomination = Convert.ToDecimal(InnerXN["faceValue"].InnerText);

                                if (InnerXN.SelectSingleNode("status") != null)
                                    objReward.Available = InnerXN["status"].InnerText;

                                if (InnerXN.SelectSingleNode("minValue") != null)
                                    objReward.MinPrice = Convert.ToDecimal(InnerXN["minValue"].InnerText);

                                if (InnerXN.SelectSingleNode("maxValue") != null)
                                    objReward.MaxPrice = Convert.ToDecimal(InnerXN["maxValue"].InnerText);

                                if (InnerXN.SelectSingleNode("denomination") != null)
                                    objReward.Denomination = Convert.ToDecimal(InnerXN["denomination"].InnerText);
                                if (InnerXN.SelectSingleNode("valueType") != null)
                                    objReward.IsVariable = Convert.ToString(InnerXN["valueType"].InnerText);
                                //XmlNodeList InnerXnCon = InnerXN.SelectNodes("countries");
                                //foreach (XmlNode con in InnerXnCon)
                                //{
                                //    objReward.Locale = con["countries"].InnerText;
                                //}
                                if (objReward.Sku == "U623217" || objReward.Sku == "U330753" || objReward.Sku == "U667894" || objReward.Sku == "U761382" || objReward.Sku == "U320784" || objReward.Sku == "U055524" || objReward.Sku == "U471536" || objReward.Sku == "U640032" || objReward.Sku == "U714697" || objReward.Sku == "U052196" || objReward.Sku == "U590244")
                                {
                                    objReward.Locale = "US";
                                }
                                else
                                {
                                    if (InnerXN.SelectSingleNode("countries") != null)
                                        objReward.Locale = InnerXN["countries"].InnerText;
                                }
                                objReward.AccountBalance = CurrentAcBalance;

                                if (objReward.Sku != "U016495" && objReward.Sku != "U556729" && objReward.Sku != "U569744" && objReward.Sku != "U798603" && objReward.Sku != "U624418")
                                {
                                    List<Range> LstRange = new List<Range>();
                                    if (Convert.ToString(InnerXN["valueType"].InnerText) == "VARIABLE_VALUE")
                                    {
                                        if ((objReward.MinPrice) < CurrentAcBalance && objReward.MaxPrice >= 100 && CurrentAcBalance >= 100)
                                        {
                                            if ((objReward.MinPrice) <= 10)
                                            {
                                                Range objRange = new Range();
                                                objRange.RewardValue = 10;
                                                LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                LstRange.Add(objRange3);
                                            }
                                            else if ((objReward.MinPrice) <= 20)
                                            {

                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                LstRange.Add(objRange3);
                                            }
                                            else if ((objReward.MinPrice) <= 25)
                                            {

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 25;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 50;
                                                LstRange.Add(objRange2);

                                            }
                                            else if ((objReward.MinPrice) <= 40)
                                            {
                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                //Range objRange1 = new Range();
                                                //objRange1.RewardValue = 20;
                                                //objRange1.IsDisable = true;
                                                //LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                LstRange.Add(objRange3);
                                            }

                                        }
                                        else if ((objReward.MinPrice) < CurrentAcBalance && objReward.MaxPrice >= 40 && CurrentAcBalance >= 40)
                                        {
                                            if ((objReward.MinPrice) <= 10)
                                            {
                                                Range objRange = new Range();
                                                objRange.RewardValue = 10;
                                                LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);

                                            }
                                            else if ((objReward.MinPrice) <= 20)
                                            {

                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);

                                            }
                                            else if ((objReward.MinPrice) <= 25)
                                            {
                                                if (CurrentAcBalance < 50)
                                                {
                                                    Range objRange1 = new Range();
                                                    objRange1.RewardValue = 25;
                                                    LstRange.Add(objRange1);

                                                    Range objRange2 = new Range();
                                                    objRange2.RewardValue = 50;
                                                    objRange2.IsDisable = true;
                                                    LstRange.Add(objRange2);
                                                }
                                                else
                                                {
                                                    Range objRange1 = new Range();
                                                    objRange1.RewardValue = 25;
                                                    LstRange.Add(objRange1);

                                                    Range objRange2 = new Range();
                                                    objRange2.RewardValue = 50;
                                                    LstRange.Add(objRange2);
                                                }

                                            }
                                            else if ((objReward.MinPrice) <= 40)
                                            {
                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                //Range objRange1 = new Range();
                                                //objRange1.RewardValue = 20;
                                                //objRange1.IsDisable = true;
                                                //LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);

                                            }

                                        }
                                        else if ((objReward.MinPrice) < CurrentAcBalance && objReward.MaxPrice >= 25 && CurrentAcBalance >= 25)
                                        {
                                            if ((objReward.MinPrice) <= 10)
                                            {
                                                Range objRange = new Range();
                                                objRange.RewardValue = 10;
                                                LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);
                                            }
                                            else if ((objReward.MinPrice) <= 20)
                                            {
                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);
                                            }
                                            else if ((objReward.MinPrice) <= 25)
                                            {
                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 25;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 50;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                            }
                                        }
                                        else if ((objReward.MinPrice) < CurrentAcBalance && objReward.MaxPrice >= 20 && CurrentAcBalance >= 20)
                                        {
                                            if ((objReward.MinPrice) <= 10)
                                            {
                                                Range objRange = new Range();
                                                objRange.RewardValue = 10;
                                                LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);
                                            }
                                            else if ((objReward.MinPrice) <= 20)
                                            {
                                                //Range objRange = new Range();
                                                //objRange.RewardValue = 10;
                                                //objRange.IsDisable = true;
                                                //LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);
                                            }
                                        }
                                        else if ((objReward.MinPrice) < CurrentAcBalance && objReward.MaxPrice >= 10 && CurrentAcBalance >= 10)
                                        {
                                            Range objRange = new Range();
                                            objRange.RewardValue = 10;
                                            LstRange.Add(objRange);

                                            Range objRange1 = new Range();
                                            objRange1.RewardValue = 20;
                                            objRange1.IsDisable = true;
                                            LstRange.Add(objRange1);

                                            Range objRange2 = new Range();
                                            objRange2.RewardValue = 40;
                                            objRange2.IsDisable = true;
                                            LstRange.Add(objRange2);

                                            Range objRange3 = new Range();
                                            objRange3.RewardValue = 100;
                                            objRange3.IsDisable = true;
                                            LstRange.Add(objRange3);
                                        }
                                        else
                                        {
                                            if ((objReward.MinPrice) == 25)
                                            {
                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 25;
                                                objRange1.IsDisable = true;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 50;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);
                                            }
                                            else
                                            {
                                                Range objRange = new Range();
                                                objRange.RewardValue = 10;
                                                objRange.IsDisable = true;
                                                LstRange.Add(objRange);

                                                Range objRange1 = new Range();
                                                objRange1.RewardValue = 20;
                                                objRange1.IsDisable = true;
                                                LstRange.Add(objRange1);

                                                Range objRange2 = new Range();
                                                objRange2.RewardValue = 40;
                                                objRange2.IsDisable = true;
                                                LstRange.Add(objRange2);

                                                Range objRange3 = new Range();
                                                objRange3.RewardValue = 100;
                                                objRange3.IsDisable = true;
                                                LstRange.Add(objRange3);
                                            }
                                        }
                                        objReward.LstRange = LstRange;
                                        objTRewards.Reward.Add(objReward);
                                        lstTRewards.Add(objTRewards);
                                    }
                                    else if (Convert.ToString(InnerXN["valueType"].InnerText) == "FIXED_VALUE")
                                    {
                                        if (Convert.ToDecimal(InnerXN["faceValue"].InnerText) >= 10)
                                        {
                                            if ((CurrentAcBalance >= (Convert.ToDecimal(InnerXN["faceValue"].InnerText)) && Convert.ToDecimal(InnerXN["faceValue"].InnerText) != -1 && (Convert.ToDecimal(InnerXN["faceValue"].InnerText)) >= 10))
                                            {
                                                objReward.IsDisable = false;
                                            }

                                            Range objRange = new Range();
                                            objRange.IsDisable = true;
                                            objRange.RewardValue = 10;
                                            LstRange.Add(objRange);

                                            Range objRange1 = new Range();
                                            objRange1.RewardValue = 20;
                                            objRange1.IsDisable = true;
                                            LstRange.Add(objRange1);

                                            Range objRange2 = new Range();
                                            objRange2.RewardValue = 40;
                                            objRange2.IsDisable = true;
                                            LstRange.Add(objRange2);

                                            Range objRange3 = new Range();
                                            objRange3.RewardValue = 100;
                                            objRange3.IsDisable = true;
                                            LstRange.Add(objRange3);

                                            objReward.LstRange = LstRange;
                                            objTRewards.Reward.Add(objReward);
                                            objTRewards.Reward = objTRewards.Reward.OrderBy(r => r.Denomination).ToList();
                                            lstTRewards.Add(objTRewards);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            objMemberEntity.Country_id = (objMemberEntity.Country_id == "UK" ? "GB" : objMemberEntity.Country_id);
            for (var i = 0; i < lstTRewards.Count; i++)
            {
                if (lstTRewards[i].Reward.Count != 0 && lstTRewards[i].Reward[0].Locale != string.Empty)
                {
                    if (lstTRewards[i].Reward[0].Locale != objMemberEntity.Country_id)
                    {
                        lstTRewards.Remove(lstTRewards[i]);

                        if (i == 0)
                            i = 0;
                        else
                            i--;
                    }

                }

            }

            for (int i = 0; i < lstDBTRewards.Count; i++)
            {
                int count = 0;
                for (int j = 0; j < lstTRewards.Count; j++)
                {
                    if (lstDBTRewards[i].Name == lstTRewards[j].Description)
                    {
                        count = count + 1;
                        if (count > 1)
                        {
                            lstTRewards.Remove(lstTRewards[j]);

                        }
                    }
                    if (lstDBTRewards[i].Name == lstTRewards[j].Description)
                    {
                        lstTRewards[j].Category = lstDBTRewards[i].Category;
                        break;
                    }

                }
            }

            List<TRewards> lstTRDupewards = new List<TRewards>();
            //lstTRewards.Sort();
            Int32 index = 0;
            while (index < lstTRewards.Count - 1)
            {
                if (lstTRewards[index].Description == lstTRewards[index + 1].Description)
                    lstTRewards.RemoveAt(index);
                else
                    index++;
            }
            Int32 dupIndex = 0;
            while (dupIndex < lstTRewards.Count - 1)
            {
                if (lstTRewards[dupIndex].Description == lstTRewards[dupIndex + 1].Description)
                    lstTRewards.RemoveAt(dupIndex);
                else
                    dupIndex++;
            }
            //lstTRDupewards = lstTRewards;
            //for (int i = 0; i < lstTRDupewards.Count; i++)
            //{
            //    int count = 0;
            //    for (int j = 0; j < lstTRewards.Count; j++)
            //    {
            //        if (lstTRDupewards[i].Description == lstTRewards[j].Description)
            //        {
            //            count = count + 1;
            //            if (count > 1)
            //            {
            //                lstTRewards.Remove(lstTRewards[j]);

            //            }
            //        }
            //        //if (lstDBTRewards[i].Description == lstTRewards[j].Description)
            //        //{
            //        //    lstTRewards[j].Category = lstDBTRewards[i].Category;

            //        //}

            //    }
            //}
            //RewardManager objrewardmanager = new RewardManager();
            //objrewardmanager.RewardOptionGet();
            //var pagedata = objrewardmanager.GetRewardsHistory(Convert.ToInt32(Identity.Current.UserData.UserId));
            //RewardOptionGet objRewardOptionGet = new RewardOptionGet();

            //var jsonString = response1.Content.ReadAsStringAsync().Result;
            //oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);


            return Json(lstTRewards, JsonRequestBehavior.AllowGet);

            //Reward History
        }
        #endregion

        #region Get Rewards Historys
        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardsHistory(string ug, int cid)
        {
            Rewards oRewards = new Rewards();
            User objMemberEntity = new User();
            UserManager oManager = new UserManager();

            objMemberEntity = oManager.GetUserData(ug.ToString(), null, cid);
            RewardManager oRewardManager = new RewardManager();
            //List<Rewards> lstRewards = new List<Rewards>();
            //lstRewards = oRewardManager.RewardOptionGet(Convert.ToInt32(objMemberEntity.UserId), objMemberEntity.UserGuid.ToString());
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(ug, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetRewardsHistory?UserGuid=" + ug + "&ClientId=" + cid.ToString() + "&UserId=" + Convert.ToInt32(objMemberEntity.UserId) +
                "&clientId=" + objMemberEntity.OrgId.ToString(), content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString.Contains("Reward History"))
            {
                oRewards = null;
            }
            else
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }
            return Json(oRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Redeem History
        /// <summary>
        /// Get Redeem History
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRedeemHistory(string ug, int cid)
        {
            Rewards oRewards = new Rewards();
            User objMemberEntity = new User();
            UserManager oManager = new UserManager();

            objMemberEntity = oManager.GetUserData(ug.ToString(), null, cid);
            RewardManager oRewardManager = new RewardManager();
            //List<Rewards> lstRewards = new List<Rewards>();
            //lstRewards = oRewardManager.RewardOptionGet(Convert.ToInt32(objMemberEntity.UserId), objMemberEntity.UserGuid.ToString());
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(ug, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetRedeemHistory?UserGuid=" + ug + "&ClientId=" + cid.ToString() + "&UserId=" + Convert.ToInt32(objMemberEntity.UserId) +
                "&clientId=" + objMemberEntity.OrgId.ToString(), content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (jsonString.Contains("Redeem History"))
            {
                oRewards = null;
            }
            else
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }
            return Json(oRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Support
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult SendEmail(string uname, string email, string comment)
        {
            var Pagedata = SendEmails(uname, email, comment);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public int SendEmails(string FromName, string FromEmailAddress, string body)
        {
            string toAddresses = ConfigurationManager.AppSettings["ZendeskToAddress"].ToString();
            //string ccAddresses = string.Empty;
            string subject = "Contact Us From " + FromEmailAddress;
            //string body = "hi test";
            int result = 1;
            try
            {
                var smtp = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPHost"].ToString(),
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["mailingPort"]),
                    EnableSsl = false
                };
                MailMessage message = new MailMessage();

                message.From = new MailAddress(FromEmailAddress);
                message.Sender = new MailAddress(toAddresses);
                body = body.Replace("\n\n", "<br />");
                message.To.Add(toAddresses);

                // subject
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;

                // message body
                message.BodyEncoding = Encoding.UTF8;
                //send message            
                try
                {
                    smtp.Send(message);
                    result = 1;
                }
                catch (Exception ex)
                {
                    result = 0;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                result = 0;
            }
            return result;
        }


        #region save
        [Route("save")]
        [HttpPost]
        // [ValidateJsonAntiForgeryToken]
        public JsonResult Save(MemberEntity objMemberEntity)
        {
            User oFCUser = new User();
            UserDataServices objDataServer = new UserDataServices();
            //Pass ReferrerID and cehck that Verity is Required.
            Client objClient = new Client();
            UserManager objManager = new UserManager();
            objClient = objManager.GetClientDetailsByRid(null, objMemberEntity.RefferId, null);
            //objMemberEntity.VerityId = verityId;
            //objMemberEntity.VerityScore = verityScore;
            //objMemberEntity.GeoCorrelationFlag = GeoCorrelationFlag;
            objMemberEntity.SubId2 = objMemberEntity.SubId3;
            if (string.IsNullOrEmpty(objMemberEntity.ExtId))
            {
                objMemberEntity.SubId3 = objMemberEntity.EmailAddress;
            }
            else
            {
                objMemberEntity.SubId3 = objMemberEntity.ExtId;
            }
            objMemberEntity.IpAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
            objMemberEntity.DomainUrl = Request.Url.Host;
            HttpClient httpClient = new HttpClient();
            var userContent = JsonConvert.SerializeObject(objMemberEntity);
            var content = new StringContent(userContent, Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/CreateWidget", content).Result;
            if (result.ReasonPhrase.ToLower() == "ok")
            {
                var jsonString = result.Content.ReadAsStringAsync().Result;
                // To convert an XML node contained in string xml into a JSON string   
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(jsonString.Replace('"', ' ').Trim());
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                RootObject oUserRes = new JavaScriptSerializer().Deserialize<RootObject>(jsonText);
                return Json(oUserRes.result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

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

        #region RelevantUpdate
        [HttpGet]
        public JsonResult RelevantUpdate(string ts, string ug, int pfscore, string rvid, string c, int cid, string userId, string sessionId = null)
        {
            int RelevantScore = 0;
            string FpfScores = string.Empty;
            UserManager obj = new Components.Business_Layer.UserManager();
            //string RequestURL = string.Empty;
            //RequestURL = System.Configuration.ConfigurationManager.AppSettings["RDefender"].ToString();
            //RequestURL = string.Format(RequestURL, ug, cid, geoLonLat, passParam);
            //string rdjson = GetrdJson(RequestURL, token);
            Logger.Info("Verisoul Session Id|" + sessionId);
            if (!string.IsNullOrEmpty(ts))
            {
                RelevantScore = Convert.ToInt32(ts.Split(';')[0]);
                FpfScores = ts.Split(';')[1];
            }
            var Pagedata = obj.ReleventUpdate(new Guid(ug), RelevantScore, pfscore, FpfScores, rvid, cid, userId, sessionId);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get UserDetails
        /// <summary>
        /// Get UserDetails
        /// </summary>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult GetUserDetails(string ug, int cid)
        {
            UserManager oManger = new UserManager();
            User objUser = new User();
            objUser = oManger.GetUserData(ug, null, cid);
            return Json(objUser, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Member LogIn Check
        /// <summary>
        /// Login Member validate method
        /// </summary>
        /// <param name="oUser">User Object</param>
        /// <returns></returns>

        [ValidateJsonAntiForgeryToken]
        [HttpPost]
        public JsonResult LogIn(string email, string psw, int rid)
        {
            UserManager oManger = new UserManager();
            string host = Request.Url.Host;
            Client oClient = new Client();
            oClient = oManger.GetClientDetailsByRid(null, rid, null);
            User oUser = new Components.Entities.User();
            oUser = oManger.WidgetLoginCheck(email, psw, host, oClient.ClientId);
            Logger.Trace($"Login Details: {oUser.UserGuid} | rid= {rid} | cid= {oClient.ClientId} | email= {email} | pswd= {psw}");
            if (oUser.UserGuid != Guid.Empty)
            {
                DoLogin(oUser.UserGuid);
                oUser.OrgId = oClient.ClientId;
                return Json(oUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get User Data
        [HttpGet]
        public JsonResult GetUserDataEmailWidget(string EmailAddress, int rid)
        {
            UserManager oManger = new UserManager();
            User oUser = new User();
            Client oClient = new Client();
            oClient = oManger.GetClientDetailsByRid(null, rid, null);
            UserManager oManager = new UserManager();
            oUser = oManager.GetUserDataEmail(EmailAddress, oClient.ClientId);
            oUser.OrgLogo = oClient.OrgLogo;
            oUser.OrgName = oClient.OrgName;
            oUser.MemberUrl = oClient.MemberUrl;
            return Json(oUser, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Email Address Valid
        [HttpGet]
        public JsonResult emailAddressvaild(string EmailAddress, int rid)
        {
            UserManager oManger = new UserManager();
            User oUser = new User();
            Client oClient = new Client();
            oClient = oManger.GetClientDetailsByRid(null, rid, null);
            UserManager oManager = new UserManager();
            oUser = oManager.emailAddressvaild(EmailAddress, oClient.ClientId);                   
            return Json(oUser, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region Forgot Password
        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpPost]
        public void ForgetPassword(int campid, User objuser, int rid, string CustomAttribute)
        {
            UserManager oManager = new UserManager();
            oManager.ForgetPassword(objuser, campid, rid, CustomAttribute);
        }
        #endregion
    }
}