using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using NLog;

namespace Members.PrecisionSample.Web.Controllers
{
    public class PartnerController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        string logMessage = string.Empty;
        string formBody = string.Empty;
        string outputMessage = string.Empty;

        // GET: Partner
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult end(Guid? usg, Guid uig, Guid ug, int cid)
        {
            Partner oPartner = new Partner();
            PartnerManager oManager = new PartnerManager();
            oPartner = oManager.GetSurveyStatus(uig, cid);
            string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            //We need to change this According to the Org, based on Ug ot somemtjhing.
            ViewBag.PartnerLogo = "https://www.opinionetwork.com/images/pre_footer_150x74.jpg";
            if (oPartner.IsShowEndPage)
            {
                if (usg.ToString().ToLower() == "6AC169C6-DF47-4CD1-8F4D-1311F5C5F163".ToLower() || oPartner.PreliminaryStatusId == 1 || oPartner.PreliminaryStatusId == 24 || oPartner.PreliminaryStatusId == 25 || oPartner.PreliminaryStatusId == 41)
                {
                    ViewBag.Message = "Congratulations! You've completed this survey and will receive your reward.";
                    ViewBag.Value = 1;
                }
                else if (usg.ToString().ToLower() == "EC9AD2BB-A92B-4781-87C1-5D3B505F6CD3".ToLower())
                {
                    ViewBag.Message = "We're sorry, it appears you have already participated in this survey";
                    ViewBag.Value = 2;
                }
                else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                {
                    ViewBag.Message = "Thank you for participating. Unfortunately you do not match the desired profile required to complete this survey.";
                    ViewBag.Value = 2;
                }
                else if (usg.ToString().ToLower() == "50AD6CC9-9228-496F-B936-7D0E0973E60A".ToLower())
                {
                    ViewBag.Message = "Thank you for participating. Unfortunately you do not match the desired profile required to complete this survey.";
                    ViewBag.Value = 2;
                }
                else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                {
                    ViewBag.Message = "Thank you for participating. Unfortunately you do not match the desired profile required to complete this survey.";
                    ViewBag.Value = 2;
                }
                // TFS ID 9900 -- Update messaging on CST redirect
                //else if (usg.ToString().ToLower() == "D5F04CF6-50AB-4617-9B0F-95B23A07488C".ToLower() || usg.ToString().ToLower() == "B75A1590-2786-45F9-A5E3-656AE1C13724".ToLower())
                //{
                //    ViewBag.Message = "You have been terminated from this survey because our client identified you as a poor quality respondent. This means that you either took the survey too quickly or were not providing well considered responses. Your account will be deactivated after two (2) poor quality terminations.";
                //    ViewBag.Value = 2;
                //}
                else if (usg.ToString().ToLower() == "B69641EE-9E79-4256-9F99-805BEF59D814".ToLower() || usg.ToString().ToLower() == "B2519C83-BEFB-4FC6-83DF-D5E9A6CA121C".ToLower())
                {
                    ViewBag.Value = 2;
                    ViewBag.Message = "Thank you for participating. We're sorry, these surveys are not available for you. Please try one of our other survey options.";
                }

                else if (usg.ToString().ToLower() == "FDD44BE3-977E-4490-AFB8-475982660429".ToLower())
                {
                    ViewBag.Value = 2;
                    ViewBag.Message = "Thank you for participating. It appears you are not currently in the country listed on your account. Due to this, we're unable to allow access to the survey. If you are away from your home country, please do not participate in further surveys until you return.";
                }
                else if (usg.ToString().ToLower() == "2BC664BA-94DD-41E8-B7E1-251A90105119".ToLower())
                {
                    ViewBag.Value = 2;
                    ViewBag.Message = "Thank you, you have now completed step one of this process. Please follow the instructions you were given by our client and complete the process by confirming your email address or participating in the follow up interview. Once the second step is completed, you will receive your reward.";
                }
                else
                {
                    if (usg == Guid.Empty)
                    {
                        ViewBag.Value = 2;
                        ViewBag.Message = "Thank you for participating! Unfortunately the survey you're attempting to access has been closed or you previously participated.";
                    }
                    else
                    {
                        ViewBag.Message = "Thank you for participating. Unfortunately you do not match the desired profile required to complete this survey.";
                        ViewBag.Value = 2;
                    }
                }
                logMessage = Request.Url.AbsoluteUri.ToString() + "|" + usg + "|" + uig;
                logger.Trace(logMessage);
                return View("/views/partner/newpsr.cshtml");
            }
            else
            {
                logMessage = Request.Url.AbsoluteUri.ToString() + "|" + usg + "|" + uig;
                logger.Trace(logMessage);
                string url = Redirections(usg, uig, ug, cid);
                return Redirect(url);
            }

        }

        public string Redirections(Guid? usg, Guid uig, Guid ug, int cid)
        {
            string url = string.Empty;
            Partner oPartner = new Partner();
            PartnerManager oManager = new PartnerManager();
            string ReferrerUrl = string.Empty;
            string _postback = string.Empty;
            if (Request.UrlReferrer != null)
            {
                ReferrerUrl = Request.UrlReferrer.ToString();
            }
            oPartner = oManager.GetSurveyStatus(uig, cid);
            int CompletionType = 0;
            if (oPartner.IsS2SEndpage)
            {
                if (!string.IsNullOrEmpty(oPartner.PostbackUrl))
                {
                    string message = string.Empty;
                    if (!string.IsNullOrEmpty(oPartner.HashType))
                    {
                        if (oPartner.HashType.Contains("Sha1hash"))
                        {
                            string value = Sha1hash(oPartner.HashParams, oPartner.Hashkey);
                            oPartner.PostbackUrl = oPartner.PostbackUrl.Replace("%%hash%%", value);
                        }
                        if (oPartner.HashType.Contains("md5"))
                        {
                            string value = md5(oPartner.HashParams);
                            oPartner.PostbackUrl = oPartner.PostbackUrl.Replace("%%hash%%", value);
                        }
                        if (oPartner.HashType.Contains("Sha256hash"))
                        {
                            string value = Sha256hash(oPartner.HashParams, oPartner.Hashkey);
                            oPartner.PostbackUrl = oPartner.PostbackUrl.Replace("%%hash%%", value);
                        }
                    }
                    try
                    {
                        if (cid == 423 || cid == 672)
                        {
                            message = PostMethod(oPartner.PostbackUrl);
                        }
                        else if (!string.IsNullOrEmpty(oPartner.PostbackBody))
                        {
                            message = PostMethodBody(oPartner.PostbackUrl, oPartner.PostbackBody);
                        }
                        else
                        {
                            message = PostRequest1(oPartner.PostbackUrl);
                        }
                        oManager.APIResponseUpdate(uig, message);
                        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + oPartner.PostbackUrl + "|" + message;
                        logger.Trace(logMessage);
                    }
                    catch (Exception ex)
                    {
                        oManager.APIResponseUpdate(uig, ex.ToString());
                    }
                }
            }
            if (oPartner.OrgTypeId == 5)
            {
                if (oPartner.Source.ToLower() == "e" || oPartner.Source.ToLower() == "a" || oPartner.Source.ToLower() == "m")
                {
                    if (!string.IsNullOrEmpty(oPartner.PartnerRedirectUrl))
                    {
                        url = oPartner.PartnerRedirectUrl;
                    }
                    else if (!string.IsNullOrEmpty(oPartner.HomePageUrl))
                    {
                        url = oPartner.HomePageUrl;
                    }
                    else if (oPartner.IsPopUp)
                    {
                        url = "popup";
                    }
                    else
                    {
                        url = oPartner.HomePageUrl;
                    }
                }
            }
            else
            {
                if (oPartner.OrgId == 63)
                {
                    decimal revenue = 0;
                    if (usg.ToString() == "overflow" || usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        CompletionType = 2;
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        CompletionType = 3; // disqualify
                    }
                    else if (oPartner.PreliminaryStatusId == 1)
                    {
                        CompletionType = 1; // complete
                        revenue = oPartner.PartnerRevenueShare;
                    }
                    else
                    {
                        CompletionType = 3;
                    }

                    if (!string.IsNullOrEmpty(oPartner.Rid)) // For re-contacts:
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithCompleteURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithOQURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                    }
                    else
                    {
                        //Logic added on 10/3/2014 :
                        if (string.IsNullOrEmpty(oPartner.SubId))
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {

                                url = ConfigurationManager.AppSettings["SwagbucksCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                            }
                        }

                        string RequestedUrl = string.Empty;
                        string Signature = ConfigurationManager.AppSettings["SwagbucksSecretKey"].ToString()
                            + ":" + "actual_loi=" + oPartner.ActualLoi
                            + ":" + "apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                            + ":" + "completion_type=" + CompletionType
                            + ":" + "project_id=" + oPartner.ProjectId
                            + ":" + "request_date=" + oPartner.SurveyCompleteDate
                            + ":" + "revenue=" + revenue
                            + ":" + "trans_id=" + oPartner.SubId;

                        byte[] bytes = Encoding.UTF8.GetBytes(Signature);
                        SHA256Managed algo = new SHA256Managed();
                        byte[] hashBytes = algo.ComputeHash(bytes);
                        var final = System.Convert.ToBase64String(hashBytes);
                        final = final.Replace("+", "-");
                        final = final.Replace("/", "_");
                        final = final.Replace("=", "");

                        OrgStatus objOrgStatus = new OrgStatus();
                        string PostUrl = ConfigurationManager.AppSettings["SwagbucksRedirectURL"].ToString();
                        string LoginCredentials = "project_id=" + oPartner.ProjectId
                           + "&request_date=" + oPartner.SurveyCompleteDate + "&signature=" + final
                           + "&apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                           + "&completion_type=" + CompletionType
                           + "&actual_loi=" + oPartner.ActualLoi
                           + "&trans_id=" + oPartner.SubId
                           + "&revenue=" + revenue;
                        string message = GetRequest(PostUrl + LoginCredentials);
                        JavaScriptSerializer obj = new JavaScriptSerializer();
                        OrgStatus.RootObject myDeserializedObj = obj.Deserialize<OrgStatus.RootObject>(message);
                        //log enabling 
                        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + PostUrl + LoginCredentials + "|" + message;
                        logger.Trace(logMessage);
                        if (myDeserializedObj.data != null)
                        {
                            RequestedUrl = myDeserializedObj.data.url;
                            if (!string.IsNullOrEmpty(RequestedUrl))
                            {
                                try
                                {
                                    url = myDeserializedObj.data.url;
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                if (oPartner.PreliminaryStatusId == 1) //Complete
                                {
                                    url = ConfigurationManager.AppSettings["SwagbucksCompleteURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                                {
                                    url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                                {
                                    url = ConfigurationManager.AppSettings["SwagbucksOQURL"].ToString();
                                }
                                else
                                {
                                    url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["SwagbucksTermURL"].ToString();
                            }
                        }
                    }
                }
                else if (oPartner.OrgId == 70)
                {
                    decimal revenue = 0;
                    if (usg.ToString() == "overflow" || usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        CompletionType = 2;
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        CompletionType = 3; // disqualify
                    }
                    else if (oPartner.PreliminaryStatusId == 1)
                    {
                        CompletionType = 1; // complete
                        revenue = oPartner.PartnerRevenueShare;
                    }
                    else
                    {
                        CompletionType = 3;
                    }
                    //Logic added on 10/3/2014 :
                    if (string.IsNullOrEmpty(oPartner.SubId))
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["YsenseCompleteURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["YsenseOQURL"].ToString();
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                        }
                    }
                    string RequestedUrl = string.Empty;
                    string Signature = ConfigurationManager.AppSettings["SwagbucksSecretKey"].ToString()
                        + ":" + "actual_loi=" + oPartner.ActualLoi
                        + ":" + "apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                        + ":" + "completion_type=" + CompletionType
                        + ":" + "project_id=" + oPartner.ProjectId
                        + ":" + "request_date=" + oPartner.SurveyCompleteDate
                        + ":" + "revenue=" + revenue
                        + ":" + "trans_id=" + oPartner.SubId;

                    byte[] bytes = Encoding.UTF8.GetBytes(Signature);
                    SHA256Managed algo = new SHA256Managed();
                    byte[] hashBytes = algo.ComputeHash(bytes);
                    var final = System.Convert.ToBase64String(hashBytes);
                    final = final.Replace("+", "-");
                    final = final.Replace("/", "_");
                    final = final.Replace("=", "");

                    OrgStatus objOrgStatus = new OrgStatus();
                    string PostUrl = ConfigurationManager.AppSettings["YsensePostback"].ToString();
                    string LoginCredentials = "project_id=" + oPartner.ProjectId
                       + "&request_date=" + oPartner.SurveyCompleteDate + "&signature=" + final
                       + "&apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                       + "&completion_type=" + CompletionType
                       + "&actual_loi=" + oPartner.ActualLoi
                       + "&trans_id=" + oPartner.SubId
                       + "&revenue=" + revenue;
                    string message = GetRequest(PostUrl + LoginCredentials);
                    JavaScriptSerializer obj = new JavaScriptSerializer();
                    OrgStatus.RootObject myDeserializedObj = obj.Deserialize<OrgStatus.RootObject>(message);
                    //log enabling 
                    logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + PostUrl + LoginCredentials + "|" + message;
                    logger.Trace(logMessage);
                    if (myDeserializedObj.data != null)
                    {
                        RequestedUrl = myDeserializedObj.data.url;
                        if (!string.IsNullOrEmpty(RequestedUrl))
                        {
                            try
                            {
                                url = myDeserializedObj.data.url;
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["YsenseCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["YsenseOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["YsenseCompleteURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["YsenseOQURL"].ToString();
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["YsenseTermURL"].ToString();
                        }
                    }
                }
                else if (oPartner.OrgId == 494)
                {
                    decimal revenue = 0;
                    if (usg.ToString() == "overflow" || usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        CompletionType = 2;
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        CompletionType = 3; // disqualify
                    }
                    else if (oPartner.PreliminaryStatusId == 1)
                    {
                        CompletionType = 1; // complete
                        revenue = oPartner.PartnerRevenueShare;
                    }
                    else
                    {
                        CompletionType = 3;
                    }
                    //Logic added on 10/3/2014 :
                    if (string.IsNullOrEmpty(oPartner.SubId))
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["MyPointsCompleteURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["MyPointsOQURL"].ToString();
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                        }
                    }
                    string RequestedUrl = string.Empty;
                    string Signature = ConfigurationManager.AppSettings["SwagbucksSecretKey"].ToString()
                        + ":" + "actual_loi=" + oPartner.ActualLoi
                        + ":" + "apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                        + ":" + "completion_type=" + CompletionType
                        + ":" + "project_id=" + oPartner.ProjectId
                        + ":" + "request_date=" + oPartner.SurveyCompleteDate
                        + ":" + "revenue=" + revenue
                        + ":" + "trans_id=" + oPartner.SubId;

                    byte[] bytes = Encoding.UTF8.GetBytes(Signature);
                    SHA256Managed algo = new SHA256Managed();
                    byte[] hashBytes = algo.ComputeHash(bytes);
                    var final = System.Convert.ToBase64String(hashBytes);
                    final = final.Replace("+", "-");
                    final = final.Replace("/", "_");
                    final = final.Replace("=", "");

                    OrgStatus objOrgStatus = new OrgStatus();
                    string PostUrl = ConfigurationManager.AppSettings["MyPointsPostback"].ToString();
                    string LoginCredentials = "project_id=" + oPartner.ProjectId
                       + "&request_date=" + oPartner.SurveyCompleteDate + "&signature=" + final
                       + "&apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                       + "&completion_type=" + CompletionType
                       + "&actual_loi=" + oPartner.ActualLoi
                       + "&trans_id=" + oPartner.SubId
                       + "&revenue=" + revenue;
                    string message = GetRequest(PostUrl + LoginCredentials);
                    JavaScriptSerializer obj = new JavaScriptSerializer();
                    OrgStatus.RootObject myDeserializedObj = obj.Deserialize<OrgStatus.RootObject>(message);
                    //log enabling 
                    logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + PostUrl + LoginCredentials + "|" + message;
                    logger.Trace(logMessage);
                    if (myDeserializedObj.data != null)
                    {
                        RequestedUrl = myDeserializedObj.data.url;
                        if (!string.IsNullOrEmpty(RequestedUrl))
                        {
                            try
                            {
                                url = myDeserializedObj.data.url;
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["MyPointsCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["MyPointsOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["MyPointsCompleteURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["MyPointsOQURL"].ToString();
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["MyPointsTermURL"].ToString();
                        }
                    }
                }
                else if (oPartner.OrgId == 127)
                {
                    if (oPartner.PreliminaryStatusId == 1) // for Success
                    {
                        try
                        {
                            string TORRewards = string.Empty;
                            Torreward objRewards = new Torreward();
                            objRewards.PID = oPartner.TxId;
                            objRewards.COMID = ConfigurationManager.AppSettings["CompanyIdentifier"].ToString();
                            objRewards.RWD = oPartner.MemberReward.ToString();
                            objRewards.TRANSID = Convert.ToString(uig);
                            objRewards.CPROJID = Convert.ToString(oPartner.ProjectId);
                            objRewards.IsCurrency = 1;
                            objRewards.Status = ConfigurationManager.AppSettings["OpinionroomRewardStatus"].ToString();

                            //Serialize to create a JSON String
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonstring = JsonConvert.SerializeObject(objRewards);
                            TORRewards = TORRewards + '{' + @"""" + "Data" + @""":" + jsonstring + '}';
                            string ResponseMessage = PutRequest(ConfigurationManager.AppSettings["OpinionroomApiUrl"].ToString(), TORRewards);
                            logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + TORRewards + "|" + ResponseMessage;
                            logger.Trace(logMessage);
                            //try
                            //{
                            //    SurveyManager objManger = new SurveyManager();
                            //    //objManger.TorEndPageLog(uig, oPartner.ProjectId, TORRewards, ResponseMessage);
                            //}
                            //catch
                            //{
                            //}
                            if (ResponseMessage.Contains("successfully applied"))
                            {
                                //https://theopinionroom.com/Home/SR/?st=CPP
                                url = ConfigurationManager.AppSettings["OpinionroomSuccessUrl"] + ConfigurationManager.AppSettings["OpinionroomRewardStatus"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["OpinionroomSuccessUrl"] + ConfigurationManager.AppSettings["OpinionroomRewardStatus"].ToString();
                            }
                        }
                        catch
                        {
                            url = ConfigurationManager.AppSettings["OpinionroomSuccessUrl"] + ConfigurationManager.AppSettings["OpinionroomRewardStatus"].ToString();
                        }
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        //https://theopinionroom.com/Home/SR?COMID=L6j5x1&PID=<our respondent id>&TRANSID=<your unique transaction id>&st=DQ&CPROJID=<your project ID>
                        url = ConfigurationManager.AppSettings["OpinionroomRedirectUrl"] + ConfigurationManager.AppSettings["CompanyIdentifier"].ToString() + "&PID=" + oPartner.TxId + "&TRANSID=" + Convert.ToString(uig) + "&st=DQ&CPROJID=" + oPartner.ProjectId;
                    }
                    else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        url = ConfigurationManager.AppSettings["OpinionroomRedirectUrl"] + ConfigurationManager.AppSettings["CompanyIdentifier"].ToString() + "&PID=" + oPartner.TxId + "&TRANSID=" + Convert.ToString(uig) + "&st=QF&CPROJID=" + oPartner.ProjectId;
                    }
                    else
                    {
                        url = ConfigurationManager.AppSettings["OpinionroomRedirectUrl"] + ConfigurationManager.AppSettings["CompanyIdentifier"].ToString() + "&PID=" + oPartner.TxId + "&TRANSID=" + Convert.ToString(uig) + "&st=DQ&CPROJID=" + oPartner.ProjectId;
                    }
                }
                else if (oPartner.OrgId == 375)
                {
                    decimal revenue = 0;
                    if (usg.ToString() == "overflow" || usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        CompletionType = 2;
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        CompletionType = 3; // disqualify
                    }
                    else if (oPartner.PreliminaryStatusId == 1)
                    {
                        CompletionType = 1; // complete
                        revenue = oPartner.PartnerRevenueShare;
                    }
                    else
                    {
                        CompletionType = 3;
                    }

                    if (!string.IsNullOrEmpty(oPartner.Rid)) // For re-contacts:
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithCompleteURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithOQURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                    }
                    else
                    {
                        //Logic added on 10/3/2014 :
                        if (string.IsNullOrEmpty(oPartner.SubId))
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["IBPCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["IBPOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                            }
                        }

                        string RequestedUrl = string.Empty;
                        string Signature = ConfigurationManager.AppSettings["SwagbucksSecretKey"].ToString()
                            + ":" + "actual_loi=" + oPartner.ActualLoi
                            + ":" + "apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                            + ":" + "completion_type=" + CompletionType
                            + ":" + "project_id=" + oPartner.ProjectId
                            + ":" + "request_date=" + oPartner.SurveyCompleteDate
                            + ":" + "revenue=" + revenue
                            + ":" + "trans_id=" + oPartner.SubId;

                        byte[] bytes = Encoding.UTF8.GetBytes(Signature);
                        SHA256Managed algo = new SHA256Managed();
                        byte[] hashBytes = algo.ComputeHash(bytes);
                        var final = System.Convert.ToBase64String(hashBytes);
                        final = final.Replace("+", "-");
                        final = final.Replace("/", "_");
                        final = final.Replace("=", "");

                        OrgStatus objOrgStatus = new OrgStatus();
                        string PostUrl = ConfigurationManager.AppSettings["IBPPostback"].ToString();
                        string LoginCredentials = "project_id=" + oPartner.ProjectId
                           + "&request_date=" + oPartner.SurveyCompleteDate + "&signature=" + final
                           + "&apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                           + "&completion_type=" + CompletionType
                           + "&actual_loi=" + oPartner.ActualLoi
                           + "&trans_id=" + oPartner.SubId
                           + "&revenue=" + revenue;
                        string message = GetRequest(PostUrl + LoginCredentials);
                        JavaScriptSerializer obj = new JavaScriptSerializer();
                        OrgStatus.RootObject myDeserializedObj = obj.Deserialize<OrgStatus.RootObject>(message);
                        //log enabling 
                        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + PostUrl + LoginCredentials + "|" + message;
                        logger.Trace(logMessage);
                        if (myDeserializedObj.data != null)
                        {
                            RequestedUrl = myDeserializedObj.data.url;
                            if (!string.IsNullOrEmpty(RequestedUrl))
                            {
                                try
                                {
                                    url = myDeserializedObj.data.url;
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                if (oPartner.PreliminaryStatusId == 1) //Complete
                                {
                                    url = ConfigurationManager.AppSettings["IBPCompleteURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                                {
                                    url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                                {
                                    url = ConfigurationManager.AppSettings["IBPOQURL"].ToString();
                                }
                                else
                                {
                                    url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["IBPCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["IBPOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["IBPTermURL"].ToString();
                            }
                        }
                    }
                }
                else if (oPartner.OrgId == 374)
                {
                    decimal revenue = 0;
                    if (usg.ToString() == "overflow" || usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                    {
                        CompletionType = 2;
                    }
                    else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                    {
                        CompletionType = 3; // disqualify
                    }
                    else if (oPartner.PreliminaryStatusId == 1)
                    {
                        CompletionType = 1; // complete
                        revenue = oPartner.PartnerRevenueShare;
                    }
                    else
                    {
                        CompletionType = 3;
                    }

                    if (!string.IsNullOrEmpty(oPartner.Rid)) // For re-contacts:
                    {
                        if (oPartner.PreliminaryStatusId == 1) //Complete
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithCompleteURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithOQURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                        else
                        {
                            url = ConfigurationManager.AppSettings["SwagbucksRIDwithTermURL"].ToString() + "&RID=" + oPartner.Rid;
                        }
                    }
                    else
                    {
                        //Logic added on 10/3/2014 :
                        if (string.IsNullOrEmpty(oPartner.SubId))
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["IBDCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["IBDOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                            }
                        }

                        string RequestedUrl = string.Empty;
                        string Signature = ConfigurationManager.AppSettings["SwagbucksSecretKey"].ToString()
                            + ":" + "actual_loi=" + oPartner.ActualLoi
                            + ":" + "apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                            + ":" + "completion_type=" + CompletionType
                            + ":" + "project_id=" + oPartner.ProjectId
                            + ":" + "request_date=" + oPartner.SurveyCompleteDate
                            + ":" + "revenue=" + revenue
                            + ":" + "trans_id=" + oPartner.SubId;

                        byte[] bytes = Encoding.UTF8.GetBytes(Signature);
                        SHA256Managed algo = new SHA256Managed();
                        byte[] hashBytes = algo.ComputeHash(bytes);
                        var final = System.Convert.ToBase64String(hashBytes);
                        final = final.Replace("+", "-");
                        final = final.Replace("/", "_");
                        final = final.Replace("=", "");

                        OrgStatus objOrgStatus = new OrgStatus();
                        string PostUrl = ConfigurationManager.AppSettings["IBDPostback"].ToString();
                        string LoginCredentials = "project_id=" + oPartner.ProjectId
                           + "&request_date=" + oPartner.SurveyCompleteDate + "&signature=" + final
                           + "&apik=" + ConfigurationManager.AppSettings["SwagbucksEndpointKey"].ToString()
                           + "&completion_type=" + CompletionType
                           + "&actual_loi=" + oPartner.ActualLoi
                           + "&trans_id=" + oPartner.SubId
                           + "&revenue=" + revenue;
                        string message = GetRequest(PostUrl + LoginCredentials);
                        JavaScriptSerializer obj = new JavaScriptSerializer();
                        OrgStatus.RootObject myDeserializedObj = obj.Deserialize<OrgStatus.RootObject>(message);
                        //log enabling 
                        logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + PostUrl + LoginCredentials + "|" + message;
                        logger.Trace(logMessage);
                        if (myDeserializedObj.data != null)
                        {
                            RequestedUrl = myDeserializedObj.data.url;
                            if (!string.IsNullOrEmpty(RequestedUrl))
                            {
                                try
                                {
                                    url = myDeserializedObj.data.url;
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                if (oPartner.PreliminaryStatusId == 1) //Complete
                                {
                                    url = ConfigurationManager.AppSettings["IBDCompleteURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                                {
                                    url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                                }
                                else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                                {
                                    url = ConfigurationManager.AppSettings["IBDOQURL"].ToString();
                                }
                                else
                                {
                                    url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (oPartner.PreliminaryStatusId == 1) //Complete
                            {
                                url = ConfigurationManager.AppSettings["IBDCompleteURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower()) //terms
                            {
                                url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                            }
                            else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                            {
                                url = ConfigurationManager.AppSettings["IBDOQURL"].ToString();
                            }
                            else
                            {
                                url = ConfigurationManager.AppSettings["IBDTermURL"].ToString();
                            }
                        }
                    }
                }
                else if (oPartner.HashType.Contains("Sha1hash"))
                {
                    string value = Sha1hash(oPartner.HashParams, oPartner.Hashkey);
                    url = oPartner.PartnerRedirectUrl.Replace("%%hash%%", value);
                }
                else if (oPartner.HashType.Contains("md5"))
                {
                    string value = md5(oPartner.HashParams);
                    url = oPartner.PartnerRedirectUrl.Replace("%%hash%%", value);
                }
                else if (oPartner.HashType.Contains("Sha256hash"))
                {
                    string value = Sha256hash(oPartner.HashParams, oPartner.Hashkey);
                    url = oPartner.PartnerRedirectUrl.Replace("%%hash%%", value);
                }
                else if (string.IsNullOrEmpty(oPartner.PartnerRedirectUrl))
                {
                    url = oPartner.HomePageUrl;
                }
                else
                {
                    url = oPartner.PartnerRedirectUrl;
                }
            }

            logMessage = ReferrerUrl + "|" + Request.Url.AbsoluteUri.ToString() + "|" + url + "|";
            logger.Trace(logMessage);
            return url;
        }

        public string PostRequest1(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

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

        public string PostMethodBody(string RequestURL, string PostbackBody)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = PostbackBody;
            byte[] data = encoding.GetBytes(postData);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/json; charset=UTF-8";
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

        #region Encryption

        #region Sha1 Hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public string Sha1hash(string str, string key)
        {
            string rethash = "";
            try
            {
                byte[] secret = UTF8Encoding.UTF8.GetBytes(key);
                HMACSHA1 myhmacsha1 = new HMACSHA1(secret);
                byte[] byteArray = Encoding.ASCII.GetBytes(str);
                MemoryStream stream = new MemoryStream(byteArray);
                byte[] hashValue = myhmacsha1.ComputeHash(stream);
                string k = Convert.ToBase64String(hashValue);
                rethash = k.Replace("+", "-").Replace("/", "_").Replace("=", "");
                //rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }

        #endregion

        #region Md5 hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string md5(string plainText)
        {
            MD5 enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
            }
            return str.ToString().ToLower();
        }

        #endregion

        #endregion

        public string GetRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";
            string UserName = ConfigurationManager.AppSettings["SwagbucksUserName"].ToString();
            string Password = ConfigurationManager.AppSettings["SwagbucksPassword"].ToString();
            byte[] authBytes = Encoding.UTF8.GetBytes((UserName + ":" + Password).ToCharArray());
            LoginRequest.Headers.Add("Authorization", "BASIC " + Convert.ToBase64String(authBytes));
            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        #region Put Request For Opinionroom
        public string PutRequest(string postUrl, string LoginCredentials)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/json; charset=UTF-8";
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

        #region Sha256Hash
        public string Sha256hash(string str, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(str);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return ByteToString(hashmessage);
        }

        public string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
        #endregion
    }
}