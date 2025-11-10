using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using Members.NewOpinionBar.Web.Filters;
using Members.PrecisionSample.Common.Security;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Xml;
using System.Net;
using System.IO;
using System.Numerics;
using NLog;

namespace Members.NewOpinionBar.Web.Controllers
{
    [Authorize]
    public class MrController : BaseController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Rewards()
        {
            if (Identity.Current != null)
            {
                ViewBag.OrgName = MemberIdentity.Client.OrgName;
                ViewBag.UserId = Identity.Current.UserData.UserId;
                ViewBag.UserGuid = Identity.Current.UserData.UserGuid;
            }
            return View("/Views/Home/Rewards.cshtml");
        }
        #region Get Rewards Historys
        /// <summary>
        /// Get Rewards History
        /// </summary>
        /// <param name="UserId">UserId</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardsHistory()
        {
            Rewards oRewards = new Rewards();
            //RewardHistory objRewardHistory = new RewardHistory();
            //RewardManager objrewardmanager = new RewardManagerGetRewardData
            //User objuser = new User();
            //oRewards = objrewardmanager.GetRewardsHistory(Convert.ToInt32(Identity.Current.UserData.UserId));
            //List<RewardHistory> lstRewardHistory = new List<RewardHistory>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetRewardsHistory?UserGuid=" + Identity.Current.UserData.UserGuid + "&UserId=" + Identity.Current.UserData.UserId +
                "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (!jsonString.ToLower().Contains("no reward history was found"))
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }
            else
            {
                oRewards = null;
            }
            return Json(oRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Api Response by Id
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetApiResponsebyId(int id)
        {
            RewardManager objrewardmanager = new RewardManager();
            return Json(objrewardmanager.GetApiResponsebtId(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Redeem History
        /// <summary>
        /// Get Redeem History
        /// </summary>
        /// <param name="UserGuid">UserGuid</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRedeemHistory()
        {
            Rewards oRewards = new Rewards();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiurl"].ToString());
            var content = new StringContent(Identity.Current.UserData.UserGuid, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("api/Member/GetRedeemHistory?UserGuid=" + Identity.Current.UserData.UserGuid + "&UserId=" + Identity.Current.UserData.UserId +
                "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            if (!jsonString.ToLower().Contains("no redeem history was found"))
            {
                oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            }
            else
            {
                oRewards = null;
            }
            return Json(oRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public ActionResult Trr()
        {
            return View("/Views/Render/Redeem.cshtml");
        }
        #region Get InserrtRewardRedeemprtions
        /// <summary>
        /// Get InserrtRewardRedeemprtions
        /// </summary>
        /// <param name="amount">amount</param>
        /// <param name="CatalougeGuid">CatalougeGuid</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InsertUsertRewardRedeemprtions(int amount, Guid cg, Guid ug, int cid)
        {
            RewardManager objrewardmanager = new RewardManager();
            return Json(objrewardmanager.InsertUsertRewardRedeemprtions(amount, cg, ug, cid), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get User Guid
        //    /// <summary>
        //    /// Get User Guid
        //    /// </summary>s
        //    /// <returns></returns>

        //    //public JsonResult GetUserGuid()
        //    //{
        //    //    return Json(GetUserGuid(), JsonRequestBehavior.AllowGet);

        //    //    //return Json(pagedata, JsonRequestBehavior.AllowGet);

        //    //}
        #endregion

        #region  Get Reward Data
        /// <summary>
        /// Get Reward Data
        /// </summary>
        /// <returns></returns>

        public JsonResult GetRewardData(int UserId, Guid UserGuid, int ClientId, string AccBalance)
        {
            string userName = string.Empty;
            string password = string.Empty;
            string Result = string.Empty;
            decimal CurrentAcBalance = 0;
            RewardManager oRewardManager = new RewardManager();
            List<Rewards> lstRewards = new List<Rewards>();
            Rewards oRewards = new Rewards();
            lstRewards = oRewardManager.RewardOptionGet(UserId, UserGuid.ToString(), ClientId);
            //oRewards = oRewardManager.GetRewardsHistory(Convert.ToInt32(Identity.Current.UserData.UserId));
            //HttpClient client = new HttpClient();
            //var content = new StringContent("", Encoding.UTF8, "application/json");
            //HttpResponseMessage response1 = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?UserGuid=" + Identity.Current.UserData.UserGuid.ToString() + "&UserId=" + Identity.Current.UserData.UserId +
            //    "&ClientId=" + MemberIdentity.Client.ClientId, content).Result;
            //var jsonString = response1.Content.ReadAsStringAsync().Result;
            //if (jsonString.ToLower() == "no reward history was found")
            //    oRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            if (!string.IsNullOrEmpty(AccBalance))
            {
                CurrentAcBalance = Convert.ToDecimal(AccBalance);
            }
            UserManager objUserManager = new UserManager();
            User objUser = objUserManager.GetUserData(UserGuid.ToString(), null, ClientId);
            if (objUser.OrgId == 73)
            {
                CurrentAcBalance = Convert.ToInt32(CurrentAcBalance / 200);
                for (var k = 0; k < lstRewards.Count; k++)
                {
                    lstRewards[k].MinRedemptionAmount = Convert.ToInt32(lstRewards[k].MinRedemptionAmount / 200);
                }
            }
            if (objUser.OrgId == 111)
            {
                CurrentAcBalance = Convert.ToInt32(CurrentAcBalance / 100);
                for (var k = 0; k < lstRewards.Count; k++)
                {
                    lstRewards[k].MinRedemptionAmount = Convert.ToInt32(lstRewards[k].MinRedemptionAmount / 100);
                }
            }
            if (objUser.OrgId == 385)
            {
                CurrentAcBalance = Convert.ToInt32(CurrentAcBalance / 1000);
                for (var k = 0; k < lstRewards.Count; k++)
                {
                    lstRewards[k].MinRedemptionAmount = Convert.ToInt32(lstRewards[k].MinRedemptionAmount / 1000);
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
                                objReward.RewardName = objRewards.RewardName;
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
                lstDBTRewards = objRewardManager.GetTangoRewards(Identity.Current.UserData.UserGuid.ToString());
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
                        objTRewards.ImageURL = img["130w-326ppi"].InnerText;
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

            for (var i = 0; i < lstTRewards.Count; i++)
            {
                if (lstTRewards[i].Reward.Count != 0 && !string.IsNullOrEmpty(lstTRewards[i].Reward[0].Locale))
                {
                    if (lstTRewards[i].Reward[0].Locale != objUser.Country_id)
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
            return Json(lstTRewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SaveMemberClickedSku
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public ActionResult rc()
        {
            return View("/Views/Home/RewardRedeem.cshtml");
        }
        #endregion

        #region Sku store in session
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public void SetSKUSes(string sku)
        {
            if (!string.IsNullOrEmpty(sku))
            {
                Session["Skum"] = sku.ToString();

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
            const string secret = "6LdOScMUAAAAAFd0ibw163wKaADNLs_xn2tz3Y7m";
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
            var pagadata = Session["SKum"].ToString();
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
                Session["SKum"] = null;
            }
        }
        #endregion

        #region Insert reward redemptions
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult RedeemMemberRewards(string Sku, decimal Ut, int Points, int cid, string name)
        {
            int IpID = 0;
            RewardManager objRewardManager = new RewardManager();
            User objMemberEntity = new User();
            UserManager oManager = new UserManager();
            objMemberEntity = oManager.GetUserData(Identity.Current.UserData.UserGuid, null, cid);
            string[] ipaddress = { };
            string IpCheck = string.Empty;
            BigInteger IpNumber = 0;
            IpCheck = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            logger.Info("RewardReemption|" + HttpContext.Request.Headers["X-Forwarded-For"].ToString() + "|" + Identity.Current.UserData.UserGuid);
            if (!string.IsNullOrEmpty(IpCheck))
            {
                ipaddress = IpCheck.Split(',');
                IpNumber = Dot2LongIP(ipaddress[0]);
                IpID = objRewardManager.RedeemMemberRewards(Sku, Ut, objMemberEntity.UserId, Points, objMemberEntity.OrgId, objMemberEntity.FirstName, objMemberEntity.EmailAddress, new Guid(Identity.Current.UserData.UserGuid), ipaddress[0], IpNumber.ToString(), name);
            }
            return Json(IpID, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get reward details
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetRewardDetails(string sku, int cid, string name)
        {
            RewardManager obj = new RewardManager();
            TRewards objTrewards = obj.GeTangoRewardsBySKU(sku, Identity.Current.UserData.UserGuid, "english", cid, name);
            HttpClient client = new HttpClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?userGuid=" + Identity.Current.UserData.UserGuid + "&userId=" + Convert.ToInt32(Identity.Current.UserData.UserId) +
                "&clientId=" + MemberIdentity.Client.ClientId, content).Result;
            var jsonString = result.Content.ReadAsStringAsync().Result;
            Rewards objRewards = new JavaScriptSerializer().Deserialize<Rewards>(jsonString);
            objTrewards.AccountBalance = objRewards.AccountBalance;
            return Json(objTrewards, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetCtlgDetailsById
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult GetCtlgDetailsById(string cg, int cid)
        {
            RewardManager objRewardManager = new RewardManager();
            Rewards objRewards = new OpinionBar.Components.Entities.Rewards();
            if (!string.IsNullOrEmpty(cg))
                if (new Guid(cg) != Guid.Empty)
                {
                    objRewards = objRewardManager.GetDetailsById(new Guid(cg), new Guid(Identity.Current.UserData.UserGuid), cid);
                    objRewards.RewardDescription = objRewards.RewardDescription.Replace("%%email_address%%", Identity.Current.UserData.EmailAddress);

                    HttpClient client = new HttpClient();
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    var result = client.PostAsync(ConfigurationManager.AppSettings["apiurl"].ToString() + "api/Member/GetRewardsHistory?userGuid=" + Identity.Current.UserData.UserGuid + "&userId=" + Convert.ToInt32(Identity.Current.UserData.UserId) +
                        "&clientId=" + MemberIdentity.Client.ClientId, content).Result;
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

        #region InserrtRewardRedeemprtions
        [HttpGet]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InserrtRewardRedeemprtions(string amount, string cg)
        {
            RewardManager obj = new RewardManager();
            int redeemId = obj.InserrtRewardRedeemprtions(Convert.ToInt32(amount), new Guid(cg), MemberIdentity.Client.ClientId, Identity.Current.UserData.UserGuid,"");
            return Json(redeemId, JsonRequestBehavior.AllowGet);
        }
        #endregion

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
    }
}