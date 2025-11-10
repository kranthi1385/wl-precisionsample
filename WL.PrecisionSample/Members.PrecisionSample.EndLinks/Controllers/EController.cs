using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Web.Controllers;
using Members.PrecisionSample.Web.Filters;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using Members.PrecisionSample.Web.Utlis;
using NLog;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Members.PrecisionSample.EndLinks.Controllers
{
    public class EController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public readonly int CaptchaMinClients = Convert.ToInt16(ConfigurationManager.AppSettings["CapchaClientMin"]);
        public readonly int CaptchaMaxClients = Convert.ToInt16(ConfigurationManager.AppSettings["CapchaClientMax"]);
        EndLinksManager oManager = new EndLinksManager();
        string logMessage = string.Empty;
        string formBody = string.Empty;
        string outputMessage = string.Empty;
        // GET: End
        //public ActionResult psr(string id)
        //{
        //    ViewBag.IsPixel = true;
        //    ViewBag.PixelUrl = "http://www.google.com";
        //    ViewBag.Message = "You will be redirected in 5.4.3.2.1 seconds.";
        //    return View("/Views/pages/psr.cshtml");
        //}




        public ActionResult tas(Guid ug, Guid uig, Guid? usg, int cid)
        {
            string surveyURL = string.Empty;
            string ipAddress = string.Empty;
            Surveys oSurvey = new Surveys();
            //oSurvey = oManager.TakeAnotherSurvey(ug, uig, "t", 1, cid);
            //to get top 1 survey based on device type TFS ID -- 6560,6613
            string u = string.Empty;
            u = Request.ServerVariables["HTTP_USER_AGENT"];
            ipAddress = Request.UserHostAddress;
            oSurvey = oManager.GetRouterSurveyURL(ug, uig, "t", cid, u, ipAddress);
            string[] orgInfo;
            if (string.IsNullOrEmpty(oSurvey.orgInformation)) //Means if the Member has no pending Surveys we do not get the Org Info.
            {
                //We need an extra call based on the Org Id.
                EndLinksManager objManager = new EndLinksManager();
                oSurvey.orgInformation = objManager.GetOrgInfo(cid);
                orgInfo = oSurvey.orgInformation.Split(';');
            }
            else
            {
                orgInfo = oSurvey.orgInformation.Split(';');
            }
            //if the status guid is not passed then we are setting the default status value to Terminate
            if (usg == null || usg == Guid.Empty)
            {
                usg = new Guid("F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4");
            }
            //logic to check if the member partner is standlaone or not.
            if (Convert.ToInt32(orgInfo[2]) == 1)
            {
                return Redirect(orgInfo[0] + "/partner/end?ug=" + ug + "&uig=" + uig + "&usg=" + usg + "&cid=" + cid.ToString());
            }
            else
            {

                if (Convert.ToString(usg).ToLower() == "6ac169c6-df47-4cd1-8f4d-1311f5c5f163" || Convert.ToString(usg).ToLower() == "e999a83c-f5c0-4cde-bee1-6557b6fa001f")//if the member has completed the survey redirect him to the end page
                {
                    //For SDl/WL/Op sites.
                    if (Convert.ToInt32(orgInfo[3]) == -1 || Convert.ToInt32(orgInfo[3]) == 1 || Convert.ToInt32(orgInfo[3]) == 3)
                    {
                        return Redirect(orgInfo[0] + "/ms/surveys?ug=" + ug + "&uig=" + uig + "&usg=" + usg + "&cid=" + cid.ToString());
                    }
                    else
                    {
                        //For Partner/Widget we need to reidrect to ON site pages.
                        return Redirect(orgInfo[0] + "/partner/end?ug=" + ug + "&uig=" + uig + "&usg=" + usg + "&cid=" + cid.ToString());
                    }
                }
                else// if the member has not completed the survey then we need to show him to take another survey
                {
                    //We can show the Org Logo from Org Inforamtion.
                    if (!string.IsNullOrEmpty(oSurvey.SurveyUrl))// if there is another survey for the member then show the survey
                    {
                        ViewBag.PartnerLogo = orgInfo[1].ToString();
                        ViewBag.SurveyUrl = HttpUtility.HtmlDecode(oSurvey.SurveyUrl);
                        ViewBag.OrgInfo = oSurvey.orgInformation;
                        return View("/Views/pages/Tas.cshtml", oSurvey);
                    }
                    else// if there are no surveys available for them then we need to redirect the member to their respective end pages.
                    {
                        //For SDl/WL/Op sites.
                        if (Convert.ToInt32(orgInfo[3]) == -1 || Convert.ToInt32(orgInfo[3]) == 1 || Convert.ToInt32(orgInfo[3]) == 3)
                        {
                            return Redirect(orgInfo[0] + "/ms/surveys?ug=" + ug + "&uig=" + uig + "&usg=" + usg + "&cid=" + cid.ToString());
                        }
                        else
                        {
                            return Redirect(orgInfo[0] + "/partner/end?ug=" + ug + "&uig=" + uig + "&usg=" + usg + "&cid=" + cid.ToString());
                            //For Partner/Widget we need to reidrect to ON site pages.

                        }
                    }
                }
            }

        }


        public ActionResult test1(string ug)
        {
            return View("/Views/pages/Tas.cshtml");
        }
        //#region Take Another Survey
        ///// <summary>
        ///// Take Another Survey
        ///// </summary>
        ///// <param name="ug"></param>
        ///// <param name="uig"></param>
        ///// <returns></returns>
        //public Surveys TakeAnotherSurvey(Guid ug, Guid uig)
        //{

        //    return oManager.TakeAnotherSurvey(ug, uig, "t", 1);
        //}
        //#endregion

        #region Data Fetch -User  Survey Status
        /// <summary>
        /// GetSurveyInvitationStatus
        /// </summary>
        /// <param name="StatusGuid"></param>
        /// <returns></returns>
        public ActionResult psr(Guid usg, string uig, Decimal? cost, int? FedProjectId, string Redirect, string car, string sdv, string sh, string ps_s_hash, string _s, string hash, string ps_rstatus, string marketplace_status, string client_staus, string hk, string isc, string rdDupe, string rdThreat)
        {
            ViewBag.IsPixel = false;
            Guid _uig = Guid.Empty;
            if (!string.IsNullOrEmpty(ps_s_hash))
            {
                sh = ps_s_hash;
            }
            if (!string.IsNullOrEmpty(_s))
            {
                sh = _s;
            }
            if (!string.IsNullOrEmpty(hash))
            {
                sh = hash;
            }
            if (!string.IsNullOrEmpty(hk))  //For RepData API - 7119
            {
                sh = hk;
            }
            if (!string.IsNullOrEmpty(isc) || !string.IsNullOrEmpty(rdDupe) || !string.IsNullOrEmpty(rdThreat))
            {
                ps_rstatus = string.Join(",", new[] { isc, rdDupe, rdThreat }.Where(s => !string.IsNullOrEmpty(s)));
                client_staus = string.Join(",", new[] { !string.IsNullOrEmpty(isc) ? "isc" : null,
                                            !string.IsNullOrEmpty(rdDupe) ? "rdDupe" : null,
                                            !string.IsNullOrEmpty(rdThreat) ? "rdThreat" : null }
                                            .Where(s => s != null));
            }
            //Code added for TFS ID 7744
            if (!string.IsNullOrEmpty(marketplace_status))
            {
                ps_rstatus = marketplace_status;
            }
            if (client_staus == "70" || client_staus == "80")
                usg = new Guid("F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4");
            //Based on the Length of the Invitation GUID we need to determine weather it is Stripped out GUID or Invitation ID.
            if (!string.IsNullOrEmpty(uig))
            {
                if (uig.Length == 36)
                {
                    _uig = new Guid(uig);
                }
                else if (uig.Length > 36)
                {
                    if (uig.Contains("0000"))
                    {
                        _uig = new Guid(uig.Replace("0000", ""));
                    }
                    else
                    {
                        _uig = new Guid(uig);
                    }
                }
                //For Stripped Out GUID.
                else if (uig.ToString().Length == 32)
                {
                    string _prepareGuid = uig.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
                    _uig = new Guid(_prepareGuid);
                }
                //For User Invitation ID.
                else if (Request.QueryString["uig"].ToString().Length < 32)
                {
                    try
                    {
                        SurveyManager objManager = new SurveyManager();
                        _uig = objManager.GetInvitationGUIDbyId(Request.QueryString["uig"]);   //Get the User Invitation GUID 
                    }
                    catch
                    {
                        _uig = Guid.Empty;
                    }
                }
                else
                {
                    _uig = Guid.Empty;
                }
            }
            //Here we need to one Conidtion, if they Pass User Invitation ID 
            Surveys oSurveys = new Surveys();
            if (cost == null)
            {
                cost = 0;
            }

            if (FedProjectId == null)
            {
                FedProjectId = -1;
            }
            if (!string.IsNullOrEmpty(uig) && _uig != Guid.Empty)
            {
                oSurveys = oManager.GetSurveyInvitationStatus(usg, _uig, Convert.ToDecimal(cost), Convert.ToInt32(FedProjectId), car, sdv, ps_rstatus);
                if (!string.IsNullOrEmpty(Redirect))
                {
                }
                else
                {
                    Redirect = "y"; //Default we should not route to FED.
                    //We need to redirect to FED added by sandy on 1/11/2018
                }

                //Bera /respondent/complete
                if (oSurveys.ClientKey.ToString().ToUpper() == "CBEAF507-56B4-465A-A6B9-0B06F4FF595E")
                {
                    RespondentComplete objRespondentComplete = new RespondentComplete()
                    {
                        respondentId = _uig.ToString().ToUpper(),
                        status = 8,
                        surveyId = oSurveys.ProjectId.ToString()
                    };
                    if (oSurveys.UserInvitationStatusId == 1)
                    {
                        objRespondentComplete.status = 2;
                    }
                    else if (new List<int> { 2, 7, 15, 18, 23 }.Contains(oSurveys.UserInvitationStatusId))
                    {
                        objRespondentComplete.status = 4;
                    }
                    else if (new List<int> { 4, 9, 10, 19, 22, 28, 31, 34, 37, 38, 39, 41, 42, 46, 48, 49, 50, 51, 54, 55, 56, 57, 58, 59, 60, 61, 63, 64, 65, 66, 67 }.Contains(oSurveys.UserInvitationStatusId))
                    {
                        objRespondentComplete.status = 6;
                    }
                    else if (new List<int> { 5, 14, 20, 29 }.Contains(oSurveys.UserInvitationStatusId))
                    {
                        objRespondentComplete.status = 7;
                    }
                    else
                    {
                        objRespondentComplete.status = 8;
                    }
                    string apiUrl = ConfigurationManager.AppSettings["BersAPIURL"].ToString();
                    M2MToken objM2MToken = new M2MToken();
                    objM2MToken = oManager.GetM2MToken();
                    string token = objM2MToken.access_token;

                    RespondentComplete(objRespondentComplete, token, apiUrl);
                }
                if (string.IsNullOrEmpty(sh) && (oSurveys.EncryptionTypeID == 1 || oSurveys.EncryptionTypeID == 5) && ((new List<int> { 1, 2, 4, 8, 41 }).Contains(oSurveys.UserInvitationStatusId)))
                {
                    string pureSpectrumLucidClientKeys = ConfigurationManager.AppSettings["PureSpectrumLucidClientKeys"];
                    string nrgClientKey = ConfigurationManager.AppSettings["NRGClientKey"];
                    List<string> pureSpectrumLucid = pureSpectrumLucidClientKeys.Split(new[] { ',' }).Select(k => k.Trim().ToUpper()).ToList();
                    List<string> nrgClient = nrgClientKey.Split(new[] { ',' }).Select(k => k.Trim().ToUpper()).ToList();
                    bool isPureSpectrumLucid = pureSpectrumLucid.Contains(oSurveys.ClientKey.ToString().ToUpper()) && !string.IsNullOrEmpty(marketplace_status);
                    bool isNRG = nrgClientKey.Contains(oSurveys.ClientKey.ToString().ToUpper()) && (new List<int> { 2, 4 }).Contains(oSurveys.UserInvitationStatusId);
                    //if (new List<string> { "B3B9C439-7EF1-4298-A59B-404476914214", "3C3B92A2-1E61-4C6C-AC95-EA54DB85313C" }.Contains(oSurveys.ClientKey.ToString().ToUpper()) && !string.IsNullOrEmpty(marketplace_status))
                    //{
                    //}
                    //else if (new List<string> { "210AC340-ABD0-4FDD-95A4-65E8B7A14502" }.Contains(oSurveys.ClientKey.ToString().ToUpper()) && ((new List<int> { 2, 4, 8, 41 }).Contains(oSurveys.UserInvitationStatusId)))
                    //{
                    //}
                    if (!(isPureSpectrumLucid || isNRG))
                    {
                        oSurveys.UserInvitationStatusId = 54; //Encryption not matching
                        logMessage = Request.Url.AbsoluteUri + " | Encryption Not Matching |" + sh;
                        logger.Trace(logMessage);
                    }
                }
                if (!string.IsNullOrEmpty(sh) && oSurveys.EncryptionTypeID == 1 && ((new List<int> { 1, 2, 4, 8, 41 }).Contains(oSurveys.UserInvitationStatusId)))
                {
                    string hash1 = string.Empty;
                    if (new List<string> { "B3B9C439-7EF1-4298-A59B-404476914214", "3C3B92A2-1E61-4C6C-AC95-EA54DB85313C" }.Contains(oSurveys.ClientKey.ToString().ToUpper()))
                    {
                        string TolunaHashKey = ConfigurationManager.AppSettings["TolunaHashKey"].ToString();
                        string url = Request.Url.AbsoluteUri;
                        int position = url.IndexOf("&sh=");
                        string result = url.Substring(0, position) + "&";
                        hash1 = HMACSha256hash(result, TolunaHashKey);
                    }
                    else if (oSurveys.ClientKey.ToString().ToUpper() == "C8D1750A-F0E8-47A4-9D13-9BB70C7FEE67")
                    {
                        string RepdataHashKey = ConfigurationManager.AppSettings["RepDataAPIKey"].ToString();
                        string url = Request.Url.AbsoluteUri;
                        int position = url.IndexOf("&hk=");
                        string result = url.Substring(0, position) + "&sk=" + RepdataHashKey;
                        hash1 = sha256hash(result);
                    }
                    else if (oSurveys.ClientKey.ToString().ToUpper() == "B9501FA7-4F81-4BDC-91BA-9E53372839AB")
                    {
                        string ParadigmHashKey = ConfigurationManager.AppSettings["ParadigmHashKey"].ToString();
                        string baseUrl = Request.Url.AbsoluteUri;
                        if (baseUrl.Contains("&hash="))
                        {
                            baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("&hash="));
                        }
                        string stringToHash = baseUrl + ParadigmHashKey;
                        hash1 = sha256hash(stringToHash);
                    }
                    else if (oSurveys.ClientKey.ToString().ToUpper() == "0577B06A-46DF-4ED4-84F8-8D418905F953")
                    {
                        string OcucomHashKey = ConfigurationManager.AppSettings["OcucomHashKey"].ToString();
                        string baseUrl = Request.Url.AbsoluteUri;
                        if (baseUrl.Contains("&hash="))
                        {
                            baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("&hash="));
                        }
                        string stringToHash = baseUrl + OcucomHashKey;
                        hash1 = sha256hash(stringToHash);
                    }
                    else
                    {
                        hash1 = HMACSha256hash(uig, oSurveys.ClientKey.ToString().ToUpper());
                    }
                    if (!string.Equals(hash1.ToUpper(), sh.ToUpper()))
                    {
                        oSurveys.UserInvitationStatusId = 54; //Encryption not matching
                        logMessage = Request.Url.AbsoluteUri + " | Encryption Not Matching |" + sh + "|" + hash1;
                        logger.Trace(logMessage);
                    }
                }
                if ((!string.IsNullOrEmpty(sh) && oSurveys.EncryptionTypeID == 5 && ((new List<int> { 1, 2, 4, 8, 41 }).Contains(oSurveys.UserInvitationStatusId))) || oSurveys.ProjectId == 618185)
                {
                    string url = Request.Url.AbsoluteUri;
                    int position = url.Contains("&hash=") ? url.IndexOf("&hash=") : -1;
                    string DynataClientKey = ConfigurationManager.AppSettings["DynataClientKey"].ToString();
                    string hash1 = string.Empty;
                    if (oSurveys.ClientKey.ToString().ToUpper() == "20188179-6072-48C1-889D-7BF6099905DD")
                    {
                        hash1 = PureSpectrumSHA1(oSurveys.ClientKey.ToString().ToUpper());
                    }
                    else if (oSurveys.ClientKey.ToString().ToUpper() == DynataClientKey || oSurveys.ProjectId == 618185)
                    {
                        string DynataKey = ConfigurationManager.AppSettings["DynataHashKey"].ToString();
                        string result = url.Substring(0, position) + DynataKey;
                        hash1 = Sha1hash(result);
                    }
                    else if (new List<String> { "2031316C-3997-4E17-9CB8-133A008668BF", "B0DCB092-3FCF-49D2-9AA5-5663E09C6AA1" }.Contains(oSurveys.ClientKey.ToString().ToUpper()))
                    {
                        hash1 = ForstaHMACSHA1(string.Empty, oSurveys.ClientKey.ToString().ToUpper(), null);
                    }
                    else if (oSurveys.ClientKey.ToString().ToUpper() == "F7CA9B9C-E451-4A65-A605-E92DB81D3965")
                    {
                        string Fedkey = ConfigurationManager.AppSettings["FedHashKey"].ToString();
                        string result = url.Substring(0, position) + "&";
                        hash1 = HMACSha1hash(result, Fedkey);
                    }
                    else
                    {
                        hash1 = HMACSha1hash(uig, oSurveys.ClientKey.ToString().ToUpper());
                    }

                    if (!string.Equals(hash1, sh))
                    {
                        oSurveys.UserInvitationStatusId = 54; //Encryption not matching
                        logMessage = Request.Url.AbsoluteUri + " | Encryption Not Matching |" + sh + "|" + hash1;
                        logger.Trace(logMessage);
                    }
                }
                //Insert Member Survey Target & Quota Activity
                bool _is_quality_term = oManager.SurveyActivityInsert(oSurveys, _uig, ps_rstatus, client_staus);
                //Insert Member Partner Post back info to fire pixels.
                oManager.PartnerTransInsert(oSurveys, _uig, Convert.ToDecimal(cost));
                // TFS ID 10898 -- Allow QSF in except Dupes into the survey "837884"
                if (!string.IsNullOrEmpty(oSurveys.QSFSurveyURL))
                {
                    return this.Redirect(oSurveys.QSFSurveyURL);
                }
                // if the meber is river member
                if (oSurveys.IsRiver == 1)
                {
                    Response.Redirect(ConfigurationManager.AppSettings["RiverEndPage"] + "usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() + "&ug=" + oSurveys.UserGuid.ToString() +
                        "&usid=" + oSurveys.UserId.ToString() + "&project=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString());
                }
                else
                {
                    //If the Member is not an External Members, means ( May be a SDL/API/WL Member).
                    if (oSurveys.TartgetTypeId != 2)
                    {
                        if (oSurveys.OrganizationTypeId == 2 || oSurveys.OrganizationTypeId == 4) //For all API  &   --> Social  added on 1/22/2016 by sandy .
                        {
                            if (Redirect.ToString() == "y" && oSurveys.UserInvitationStatusId != 1 && oSurveys.Fedresponseid != Guid.Empty)
                            {
                                RedirectToFedPages(oSurveys, _uig);
                            }
                            else
                            {
                                if (oSurveys.ProjectStatusId == 2)
                                {
                                    if (oSurveys.UserInvitationStatusId == 2 || oSurveys.UserInvitationStatusId == 4 || oSurveys.UserInvitationStatusId == 5 || oSurveys.UserInvitationStatusId == 6 || oSurveys.UserInvitationStatusId == 23)
                                    {
                                        Response.Redirect(ConfigurationManager.AppSettings["endlinksurl"].ToString() + "/e/tas?usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() + "&ug=" + oSurveys.UserGuid.ToString()
                                        + "&usid=" + oSurveys.UserId.ToString() + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString() + "&cc=" + oSurveys.LanguageCode.ToString());
                                    }
                                    else
                                    {
                                        Response.Redirect(oSurveys.MemberUrl + "/partner/end?usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&usid=" + oSurveys.UserId.ToString()
                                           + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString());
                                    }
                                    //}
                                }
                                else
                                {
                                    Response.Redirect(oSurveys.MemberUrl + "/partner/end?usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4&uig=" + _uig.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&usid=" + oSurveys.UserId.ToString()
                                        + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString());
                                }
                            }
                        }
                        //We have same solution for SDL/WL/OP panels.
                        else if (oSurveys.OrganizationTypeId == -1 || oSurveys.OrganizationTypeId == 1 || oSurveys.OrganizationTypeId == 3) //For SDL Panel
                        {
                            string _finalurl = oSurveys.ExtRedirectUrl;
                            if (Redirect.ToString() == "y" && oSurveys.UserInvitationStatusId != 1 && oSurveys.Fedresponseid != Guid.Empty)
                            {
                                RedirectToFedPages(oSurveys, _uig);
                            }
                            else
                            {
                                //If the Project is Open
                                if (oSurveys.ProjectStatusId == 2)
                                {
                                    //If the Quota Type is External Registration.
                                    if (oSurveys.TargetTypeId == 4)
                                    {
                                        //Fire Pixel for the Post Back URL for External registration Type Quota.
                                        if (!string.IsNullOrEmpty(oSurveys.PostbackURL) && (oSurveys.UserInvitationStatusId == 1 || oSurveys.UserInvitationStatusId == 2
                                            || oSurveys.UserInvitationStatusId == 4))
                                        {
                                            if (oSurveys.PixelTypeId == 1) //For Pixels. 
                                            {
                                                if (oSurveys.PostbackURL.Contains("<img"))
                                                {
                                                    string _text = Regex.Match(oSurveys.PostbackURL, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                                    if (!string.IsNullOrEmpty(_text))
                                                    {
                                                        try
                                                        {
                                                            PostRequest(_text);
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                                else if (oSurveys.PostbackURL.Contains("<iframe"))
                                                {
                                                    string _text = Regex.Match(oSurveys.PostbackURL, "<iframe.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                                    if (!string.IsNullOrEmpty(_text))
                                                    {
                                                        try
                                                        {
                                                            PostRequest(_text);
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        PostRequest(oSurveys.PostbackURL);
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }

                                            }
                                            else if (oSurveys.PixelTypeId == 2) //For Call backs
                                            {
                                                PostRequest(oSurveys.PostbackURL);
                                            }

                                        }

                                        //Redirect to Survey Compelte URL .
                                        _finalurl = _finalurl.Replace("%%user_invitation_guid%%", oSurveys.ExternalMemberGUID.ToString());
                                        _finalurl = _finalurl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                                        Response.Redirect(_finalurl);

                                    }
                                    else
                                    {
                                        Guid profileid = Guid.Empty;
                                        ProfileManager oManager = new ProfileManager();


                                        //Conversant/ OB Router Logic.
                                        if (oSurveys.OrgId == 541 || oSurveys.OrgId == 542)
                                        {
                                            int _redirect_2_reg = 0;

                                            string _pg = string.Empty;
                                            string _routerurl = string.Empty;
                                            string _panelname = string.Empty;

                                            if (oSurveys.OrgId == 541)
                                            {
                                                _pg = "9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9"; //OB 
                                                _routerurl = ConfigurationManager.AppSettings["OBRouterSurveyPage"].ToString();
                                                _panelname = "OB Router";
                                            }
                                            else
                                            {
                                                _pg = "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761";// Conversant
                                                _routerurl = ConfigurationManager.AppSettings["CPRouterSurveyPage"].ToString();
                                                _panelname = "Conversant Router";
                                            }

                                            //Post backs for Router, Added by Rajani G on 5/27/2021 on Survey Compeltes.
                                            if (oSurveys.UserInvitationStatusId == 1 && oSurveys.PixelTypeId == 2 && !string.IsNullOrEmpty(oSurveys.PostbackURL))
                                            {
                                                string _response = string.Empty;
                                                try
                                                {
                                                    _response = PostRequest(oSurveys.PostbackURL);
                                                    logMessage = _panelname + " | Calls backs Firing |" + oSurveys.PostbackURL + "|" + _response;
                                                    logger.Trace(logMessage);
                                                }
                                                catch (Exception ex)
                                                {
                                                    logMessage = _panelname + " | Calls backs Firing |" + oSurveys.PostbackURL + "|" + ex.ToString();
                                                    logger.Trace(logMessage);
                                                }
                                            }



                                            if (oSurveys.UserInvitationStatusId == 1 || oSurveys.UserInvitationStatusId == 2 || oSurveys.UserInvitationStatusId == 4)
                                            {
                                                //Means member has a client survey status. then redicted to LBmesage page.
                                                _redirect_2_reg = 1;
                                            }
                                            else
                                            {
                                                //get the sessions counts of Prescrener term, if have 5 then reidrect to LB Success Message Page.
                                                UserManager objManager = new UserManager();
                                                _redirect_2_reg = objManager.RouteruserActivityGet(oSurveys.UserId);

                                            }

                                            if (_redirect_2_reg >= 1)
                                            {
                                                //Means member has a client survey status. then redicted to LBmesage page.
                                                string _ishow = "f";
                                                if (oSurveys.Is_simplify_affiliate)
                                                {
                                                    _ishow = "t";
                                                }
                                                string lbspagename = "lbs";
                                                if (oSurveys.Is_peerly2_affiliate)
                                                {
                                                    lbspagename = "msglb";//setting peerly 2 lb sucess page
                                                }
                                                if (oSurveys.UserInvitationStatusId == 1)
                                                {

                                                    Response.Redirect(_routerurl + "/cor/" + lbspagename + "?ug=" + oSurveys.UserGuid.ToString() +
                                                    "&pg=" + _pg + "&extid=" + oSurveys.SubId3 + "&pc=" + oSurveys.UserGuid.ToString() + oSurveys.ProjectCost.ToString() + "&is_t=h" + "&is_sim=" + _ishow);

                                                }
                                                else
                                                {

                                                    Response.Redirect(_routerurl + "/cor/" + lbspagename + "?ug=" + oSurveys.UserGuid.ToString() +
                                                        "&pg=" + _pg + "&extid=" + oSurveys.SubId3 + "&pc=na" + "&is_t=h" + "&is_sim=" + _ishow);
                                                }
                                            }
                                            else
                                            {
                                                //reidrect to top 1 Survey Mathed SP.
                                                Response.Redirect(_routerurl + "/cor/surveyget?userGuid=" + oSurveys.UserGuid.ToString() + "&authKey=" + _pg);
                                            }
                                        }

                                        if (ConfigurationManager.AppSettings["SurveyEndPageRoutingOn"] == "1")
                                        {
                                            //Show TOp10page after Endpage. Added 06/13/2016
                                            //if (oSurveys.IsTop10CompleteCheck)
                                            //{
                                            //    Response.Redirect(oSurveys.MemberUrl + "/rg/top10" + "?ug=" + oSurveys.UserGuid.ToString() + "&uig=" + _uig.ToString()
                                            //      + "&usg=" + oSurveys.StatusGuid.ToString() + "&cid=" + oSurveys.OrgId.ToString());
                                            //}
                                            //else
                                            //{
                                            if (oSurveys.UserInvitationStatusId != 2 && oSurveys.UserInvitationStatusId != 4 && oSurveys.UserInvitationStatusId != 5 && oSurveys.UserInvitationStatusId != 6 && oSurveys.UserInvitationStatusId != 23) //ii
                                            {
                                                Response.Redirect(oSurveys.MemberUrl + "/ms/surveys?usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&cid=" + oSurveys.OrgId.ToString());
                                            }
                                            else
                                            {
                                                Response.Redirect("/e/tas?ug=" + oSurveys.UserGuid.ToString() + "&usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() +
                                                    "&usid=" + oSurveys.UserId.ToString() + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString() + "&cc=" + oSurveys.LanguageCode.ToString());
                                            }
                                            //}
                                            //}
                                        }
                                        else
                                        {
                                            //Show TOp10page after Endpage. Added 06/13/2016
                                            //if (oSurveys.IsTop10CompleteCheck)
                                            //{
                                            //    Response.Redirect(oSurveys.MemberUrl + "/rg/top10" + "?ug=" + oSurveys.UserGuid.ToString() + "&uig=" + _uig.ToString() + "&usg=" + oSurveys.StatusGuid.ToString()
                                            //        + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString());
                                            //}
                                            //else
                                            //{
                                            if (oSurveys.UserInvitationStatusId != 2 && oSurveys.UserInvitationStatusId != 4 && oSurveys.UserInvitationStatusId != 5 && oSurveys.UserInvitationStatusId != 6 && oSurveys.UserInvitationStatusId != 23) //ii
                                            {
                                                Response.Redirect(oSurveys.MemberUrl + "/ms/surveys?usg=" + oSurveys.StatusGuid.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&cid=" + oSurveys.OrgId.ToString());
                                            }
                                            else
                                            {
                                                Response.Redirect("/e/tas?ug=" + oSurveys.UserGuid.ToString() + "&usg=" + oSurveys.StatusGuid.ToString() + "&uig=" + _uig.ToString() + "&usid=" + oSurveys.UserId.ToString()
                                                    + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString() + "&cc=" + oSurveys.LanguageCode.ToString());
                                            }
                                            //}
                                        }
                                    }
                                }
                                else
                                {
                                    if (oSurveys.OrgId == 541 || oSurveys.OrgId == 542) //to handle paused proejct.
                                    {
                                        string _pg1 = string.Empty;

                                        if (oSurveys.OrgId == 541)
                                        {
                                            _pg1 = "9EE17B1A-6882-4CAE-9AED-3C4D4A92DFA9"; //OB 
                                        }
                                        else
                                        {
                                            _pg1 = "71CC02CD-B8A5-4A5A-AF4D-9988B2F21761";// Conversant
                                        }
                                        //reidrect to top 1 Survey Mathed SP.
                                        Response.Redirect(ConfigurationManager.AppSettings["RouterSurveyPage"].ToString() + "/cor/surveyget?userGuid=" + oSurveys.UserGuid.ToString() + "&authKey=" + _pg1);
                                    }
                                    else
                                    {
                                        Response.Redirect(oSurveys.MemberUrl + "/ms/surveys?usg=" + oSurveys.StatusGuid.ToString() + "&ug=" + oSurveys.UserGuid.ToString() + "&uig=" + _uig.ToString() + "&eid=" + oSurveys.Fedresponseid.ToString() + "&usid=" + oSurveys.UserId.ToString()
                                        + "&pid=" + oSurveys.ProjectId + "&cid=" + oSurveys.OrgId.ToString());
                                    }
                                }
                            }
                        }
                    }
                    //If the Member is an External Member.
                    else
                    {
                        string sFinalUrl = string.Empty;
                        string _url = string.Empty;
                        sFinalUrl = oSurveys.ExtRedirectUrl;
                        #region Task 7474: SHA-1 Hashing Guide
                        if (oSurveys.Fedresponseid != Guid.Empty)
                        {
                            string Fedkey = ConfigurationManager.AppSettings["FedHashKey1"].ToString();
                            if (oSurveys.UserInvitationStatusId == 1)
                            {
                                _url = ConfigurationManager.AppSettings["FederatedQSuccessURL"].ToString();
                            }
                            else if (oSurveys.UserInvitationStatusId == 2 || oSurveys.UserInvitationStatusId == 7)
                            {
                                _url = ConfigurationManager.AppSettings["FederatedTerminateURL"].ToString();
                            }
                            else if (oSurveys.UserInvitationStatusId == 4)
                            {
                                _url = ConfigurationManager.AppSettings["FederatedOverQuotaURL"].ToString();
                            }
                            else
                            {
                                //Redirect to FED  with Term Status, if the Member Fails Verity/Relevant/Any Security Reasons. Added on 8/13/2014.
                                _url = ConfigurationManager.AppSettings["FederatedQDuplicateURL"].ToString();
                            }
                            _url = _url.Replace("%%EID%%", oSurveys.Fedresponseid.ToString());
                            _url = _url.Replace("%%user_invitation_guid%%", oSurveys.ExternalMemberGUID.ToString());
                            _url = _url.Replace("%%user_id%%", oSurveys.ExternalMemberId);
                            _url = _url + '&';
                            string encryptedURL = HMACSha1hash(_url, Fedkey);
                            _url = $"{_url}hash={encryptedURL}";
                            if (!string.IsNullOrEmpty(_url))
                            {
                                Response.Redirect(_url);
                                logMessage = "FED redirects with hash |" + _url;
                                logger.Trace(logMessage);
                            }
                            else
                            {
                                ViewBag.Message = "Thank you for participating! Unfortunately the survey you're attempting to access has been closed.";
                            }

                            #endregion
                        }
                        else if (oSurveys.ExternalPartnerID == 210 || oSurveys.ExternalPartnerID == 293) //CINT
                        {
                            string _cintPostback = ConfigurationManager.AppSettings["CINTPostBack"].ToString();

                            string _response = string.Empty;
                            //  string _body = @"{""id"":""%%external_member_id%%"",""status"": %%status%%}";
                            int status = 0;
                            //  _body = _body.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                            if (oSurveys.UserInvitationStatusId == 1)//complete
                            {
                                status = 5;
                            }
                            else if (oSurveys.UserInvitationStatusId == 2)//terminate
                            {
                                status = 2;

                            }
                            else if (oSurveys.UserInvitationStatusId == 4 || oSurveys.UserInvitationStatusId == 46)//Over quota
                            {
                                status = 3;
                            }
                            else if (oSurveys.UserInvitationStatusId == 5 || (_is_quality_term = true && oSurveys.UserInvitationStatusId == 7) || oSurveys.UserInvitationStatusId == 8 || oSurveys.UserInvitationStatusId == 9 || oSurveys.UserInvitationStatusId == 10 || oSurveys.UserInvitationStatusId == 11 ||
                                oSurveys.UserInvitationStatusId == 12 || oSurveys.UserInvitationStatusId == 13 || oSurveys.UserInvitationStatusId == 14 || oSurveys.UserInvitationStatusId == 18 || oSurveys.UserInvitationStatusId == 19 ||
                                oSurveys.UserInvitationStatusId == 20 || oSurveys.UserInvitationStatusId == 22 || oSurveys.UserInvitationStatusId == 23 || oSurveys.UserInvitationStatusId == 26 || oSurveys.UserInvitationStatusId == 28 ||
                                oSurveys.UserInvitationStatusId == 29 || oSurveys.UserInvitationStatusId == 31 || oSurveys.UserInvitationStatusId == 32 || oSurveys.UserInvitationStatusId == 33 || oSurveys.UserInvitationStatusId == 34 ||
                                oSurveys.UserInvitationStatusId == 48 || oSurveys.UserInvitationStatusId == 49)
                            {
                                status = 4;
                            }
                            else
                            {
                                status = 2;
                            }
                            //string _body = Newtonsoft.Json.JsonConvert.SerializeObject(new List<dynamic>() { new { id = oSurveys.ExternalMemberId, status = status } });
                            string _body = JsonConvert.SerializeObject(new { id = oSurveys.ExternalMemberId, status = status });
                            try
                            {
                                int CountryID = oSurveys.TargetCountryID;
                                int ProjectOrgID = oSurveys.ProjectOrgID;
                                string key = string.Empty;

                                if (ProjectOrgID == -1)
                                    key = ConfigurationManager.AppSettings["CINTPrecisionSample"];
                                else
                                {
                                    if (CountryID == 231)
                                        key = ConfigurationManager.AppSettings["CINTMetrixlabUSA"];
                                    else if (CountryID == 229)
                                        key = ConfigurationManager.AppSettings["CINTMetrixLabCUK"];
                                    else
                                        key = ConfigurationManager.AppSettings["CINTMetrixLabCUKEUR"];
                                }
                                try
                                {
                                    string _cintResponse = ConfigurationManager.AppSettings["CINTResponse"].ToString();
                                    var response = CINTGetRequest(_cintResponse + oSurveys.ExternalMemberId.ToString(), key);
                                    var jsonResponse = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(response);
                                    int CintStatus = jsonResponse["status"] != null ? (int)jsonResponse["status"] : -1;
                                    if (CintStatus == 1)
                                    {
                                        _response = CINTPostRequest(_cintPostback, _body, key);
                                        logMessage = "CINT Pixel Firing |" + _cintPostback + "|" + key + "|" + _body + "|" + _response;
                                        logger.Trace(logMessage);
                                    }
                                    else
                                    {
                                        logMessage = "CINT Pixel Skipped (Cint Status != 1) | Cint Status: " + CintStatus + " | " + _cintResponse + oSurveys.ExternalMemberId.ToString();
                                        logger.Trace(logMessage);
                                    }
                                    //_response = CINTPostRequest(_cintPostback, _body, key);
                                    //logMessage = "CINT Pixel Firing |" + _cintPostback + "|" + key + "|" + _body + "|" + _response;
                                    //logger.Trace(logMessage);
                                }
                                catch (Exception ex)
                                {
                                    oManager.InsertExceptionData(oSurveys, ex.ToString(), key, _cintPostback, _body);
                                }
                            }
                            catch (Exception ex)
                            {
                                logMessage = "CINT Pixel Firing |" + _cintPostback + "|" + _body + "|" + ex.ToString();
                                logger.Trace(logMessage);
                            }

                            if (!string.IsNullOrEmpty(oSurveys.ExtRedirectUrl))
                            {
                                sFinalUrl = oSurveys.ExtRedirectUrl;
                                sFinalUrl = sFinalUrl.Replace("%%user_invitation_guid%%", oSurveys.ExternalMemberGUID.ToString());
                                sFinalUrl = sFinalUrl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                                Response.Redirect(sFinalUrl);
                            }
                        }
                        else if (oSurveys.ExternalPartnerID == 231)
                        {
                            //Local blox Members redirection to OB or Conversant.pro

                            //71CC02CD - B8A5 - 4A5A - AF4D - 9988B2F21761-- > conversant 450
                            //9EE17B1A - 6882 - 4CAE - 9AED - 3C4D4A92DFA9-- > OB  491

                            if (oSurveys.UserInvitationStatusId == 1) //Survey Success
                            {
                                Response.Redirect("/e/lbs?leadid=" + uig.ToString() + "&pg=" + oSurveys.OrgGuid.ToString());
                            }
                            else
                            {
                                //Survey term/ oq/ QSF
                                Response.Redirect("/e/top20?leadid=" + uig.ToString() + "&pg=" + oSurveys.OrgGuid.ToString());
                            }
                        }

                        else if (oSurveys.ExternalPartnerID == 239 || oSurveys.ExternalPartnerID == 240) //samplify Exernal Partners added by Rajani G  
                        {
                            //Local blox Members redirection to OB or Conversant.pro

                            //71CC02CD - B8A5 - 4A5A - AF4D - 9988B2F21761-- > conversant 450
                            //9EE17B1A - 6882 - 4CAE - 9AED - 3C4D4A92DFA9-- > OB  491

                            if (oSurveys.UserInvitationStatusId == 1) //Survey Success
                            {
                                Response.Redirect("/e/lbs?leadid=" + uig.ToString() + "&pg=" + oSurveys.OrgGuid.ToString() + "&extid=" + oSurveys.ExternalMemberId + "&pc=" + uig.ToString() + oSurveys.ProjectCost.ToString() + "&is_t=h");
                            }

                            else
                            {
                                Response.Redirect("/e/top20?leadid=" + uig.ToString() + "&pg=" + oSurveys.OrgGuid.ToString() + "&issam=t");
                            }

                            //else if (oSurveys.UserInvitationStatusId == 2 || oSurveys.UserInvitationStatusId == 4)
                            //{
                            //    //Survey term/ oq/  should be showing top 10 Page.
                            //    Response.Redirect("/e/top20?leadid=" + uig.ToString() + "&pg=" + oSurveys.OrgGuid.ToString()+"&issam=t");
                            //}
                            //else
                            //{
                            //    ViewBag.Message = "Thank you for participating!  Unfortunately you didn’t qualify to complete this survey.";
                            //}
                        }
                        else if (oSurveys.ExternalPartnerID == Convert.ToInt32(ConfigurationManager.AppSettings["TestsetPureSpectrumId"].ToString()))
                        {
                            if (oSurveys.UserInvitationStatusId == 1)
                            {
                                string Key = ConfigurationManager.AppSettings["TestsetPureSpectrumkey"].ToString();
                                string finalurl = oSurveys.ExtRedirectUrl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                                string result = Sha1hash(finalurl + Key);
                                finalurl = finalurl + "&ps_hash=" + result;
                                logMessage = "External Partner Testset-PureSpectrum |" + finalurl + " | hashing : " + result;
                                logger.Trace(logMessage);
                                Response.Redirect(finalurl);
                            }
                            else
                            {
                                string finalurl = oSurveys.ExtRedirectUrl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                                Response.Redirect(finalurl);
                            }
                        }
                        else if(oSurveys.ExternalPartnerID == 295)
                        {
                            string Key = ConfigurationManager.AppSettings["ThreeHyphensKey"].ToString();
                            string finalurl = oSurveys.ExtRedirectUrl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);
                            string result = Sha1hash(finalurl + Key);
                            finalurl = finalurl + "&z=" + result;
                            logMessage = "External Partner Three-Hyphens |" + finalurl + " | hashing : " + result;
                            logger.Trace(logMessage);
                            Response.Redirect(finalurl);
                        }
                        else
                        {
                            //Fire Pixel for the Post Back URL for External Type Quota.

                            if (oSurveys.ProjectStatusId == 2 && !string.IsNullOrEmpty(oSurveys.PostbackURL) &&
                               (oSurveys.UserInvitationStatusId == 1 || oSurveys.UserInvitationStatusId == 2
                                            || oSurveys.UserInvitationStatusId == 4))
                            {
                                string response = string.Empty;
                                if (oSurveys.PixelTypeId == 1) //For Pixels. 
                                {
                                    if (oSurveys.PostbackURL.Contains("<img"))
                                    {
                                        string _text = Regex.Match(oSurveys.PostbackURL, "<img.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                        if (!string.IsNullOrEmpty(_text))
                                        {
                                            try
                                            {
                                                response = PostRequest(_text);
                                                logMessage = "External Partner |" + _text + "|" + response;
                                                logger.Trace(logMessage);
                                            }
                                            catch (Exception ex)
                                            {
                                                logMessage = "External Partner |" + _text + "|" + ex.ToString();
                                                logger.Trace(logMessage);
                                            }
                                        }
                                    }
                                    else if (oSurveys.PostbackURL.Contains("<iframe"))
                                    {
                                        string _text = Regex.Match(oSurveys.PostbackURL, "<iframe.*?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                                        if (!string.IsNullOrEmpty(_text))
                                        {
                                            try
                                            {
                                                response = PostRequest(_text);
                                                logMessage = "External Partner |" + _text + "|" + response;
                                                logger.Trace(logMessage);
                                            }
                                            catch (Exception ex)
                                            {
                                                logMessage = "External Partner |" + _text + "|" + ex.ToString();
                                                logger.Trace(logMessage);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //PostRequest(oSurveys.PostbackURL);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else if (oSurveys.PixelTypeId == 2) //Fire Call back or Post Back pixels:
                                {

                                    //Call back logic for Router on Survey Complete.
                                    string _response = string.Empty;
                                    try
                                    {
                                        _response = PostRequest(oSurveys.PostbackURL);
                                        logMessage = "External Partner Pixel Firing |" + oSurveys.PostbackURL + "|" + response;
                                        logger.Trace(logMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        logMessage = "External Partner Pixel Firing |" + oSurveys.PostbackURL + "|" + ex.ToString();
                                        logger.Trace(logMessage);
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(oSurveys.ExtRedirectUrl))
                            {
                                sFinalUrl = oSurveys.ExtRedirectUrl;
                                sFinalUrl = sFinalUrl.Replace("%%user_invitation_guid%%", oSurveys.ExternalMemberGUID.ToString());
                                sFinalUrl = sFinalUrl.Replace("%%external_member_id%%", oSurveys.ExternalMemberId);

                                //Added by Chandu on 14/12/2015 for New QuestionMindShare.Com External Partner
                                sFinalUrl = sFinalUrl.Replace("%%RM%%", oSurveys.E_RM);
                                sFinalUrl = sFinalUrl.Replace("%%RL%%", oSurveys.E_Rl);


                                //If we Have Pixel only then we need to wait for 5 misn to fire pixel for the survey complete and redirect to External survey complete URL.
                                if (!string.IsNullOrEmpty(oSurveys.PostbackURL) && oSurveys.PixelTypeId == 1 && oSurveys.UserInvitationStatusId == 1)
                                {
                                    //litPartnerpixel.Text = oSurveys.PostbackURL; // is to fire Pixel.
                                    ViewBag.IsPixel = true;
                                    ViewBag.PixelUrl = oSurveys.PostbackURL;
                                    //lblText.Style.Add(HtmlTextWriterStyle.Color, "Green");
                                    ViewBag.Message = "You will be redirected in 5.4.3.2.1 seconds.";
                                    Response.Redirect(sFinalUrl);
                                }
                                else
                                {
                                    //For Dynata Quality Term
                                    if (_is_quality_term == true && oSurveys.ExternalPartnerID == 236 && oSurveys.UserInvitationStatusId == 7)
                                    {
                                        string _qualityTermurl = ConfigurationManager.AppSettings["DynataQualityTermURL"].ToString();
                                        _qualityTermurl = _qualityTermurl.Replace("%%RM%%", oSurveys.E_RM);
                                        _qualityTermurl = _qualityTermurl.Replace("%%RL%%", oSurveys.E_Rl);
                                        Response.Redirect(_qualityTermurl);
                                    }
                                    else
                                    {
                                        Response.Redirect(sFinalUrl);
                                    }
                                }
                            }
                            else
                            {
                                if (oSurveys.UserInvitationStatusId == 1 || oSurveys.UserInvitationStatusId == 41)
                                {
                                    //lblText.Style.Add(HtmlTextWriterStyle.Color, "Green");
                                    ViewBag.Message = "Congratulations!  You’ve successfully completed this survey and will receive your reward.";
                                }
                                else if (oSurveys.UserInvitationStatusId == 2)
                                {
                                    ViewBag.Message = "Thank you for participating!  Unfortunately you didn’t qualify to complete this survey.";
                                }
                                else if (oSurveys.UserInvitationStatusId == 4)
                                {
                                    ViewBag.Message = "Sorry! the Quota for this survey was full.";
                                }
                                else if (oSurveys.UserInvitationStatusId == 43)
                                {
                                    ViewBag.Message = "Thank you, you have now completed step one of this process. Please follow the instructions you were given by our client and complete the process by confirming your email address or participating in the follow up interview. Once the second step is completed, you will receive your reward.";
                                }
                                else
                                {
                                    ViewBag.Message = "Thank you for participating but your profile does not match our Client’s target on this survey.";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (usg.ToString().ToLower() == "6AC169C6-DF47-4CD1-8F4D-1311F5C5F163".ToLower() || usg.ToString().ToLower() == "181cf682-614e-46ec-9716-816af9dfe43d".ToLower() || usg.ToString().ToLower() == "167944ad-051f-48e2-b458-184a27c27ece".ToLower() || usg.ToString().ToLower() == "e999a83c-f5c0-4cde-bee1-6557b6fa001f".ToLower())
                {
                    ViewBag.Message = "Success! You’ve completed this survey.";
                }
                else if (usg.ToString().ToLower() == "F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4".ToLower())
                {
                    ViewBag.Message = "Thank you for participating but your profile does not match our Client’s target on this survey.";
                }
                else if (usg.ToString().ToLower() == "67B98BED-9C3F-42AE-BDD3-7E15F9C17F00".ToLower())
                {
                    ViewBag.Message = "Thank you for participating but your profile does not match our Client’s target on this survey.";
                }
                // TFS ID 9900 -- Update messaging on CST redirect
                //else if (usg.ToString().ToLower() == "D5F04CF6-50AB-4617-9B0F-95B23A07488C".ToLower())
                //{
                //    ViewBag.Message = "You have been terminated from this survey because our client identified you as a poor quality respondent. This means that you either took the survey too quickly or were not providing well considered responses. Your account will be deactivated after two (2) poor quality terminations.";
                //}
                else if (usg.ToString().ToLower() == "2BC664BA-94DD-41E8-B7E1-251A90105119".ToLower())
                {
                    ViewBag.Message = "Thank you, you have now completed step one of this process. Please follow the instructions you were given by our client and complete the process by confirming your email address or participating in the follow up interview. Once the second step is completed, you will receive your reward.";
                }
                else
                {
                    ViewBag.Message = "Thank you for participating but your profile does not match our Client’s target on this survey.";
                }
            }

            return View("/Views/pages/psr.cshtml");
        }
        #endregion

        #region Recaptcha Logic
        /// <summary>
        ///  Recaptcha Logics
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInviationGuid</param>
        /// <param name="prjId">ProjectId</param>
        /// <returns></returns>
        public ActionResult interstitial(Guid ug, Guid uig, string sr, int cid, string cc, string fc, string pid, string sentry_status, string conid)
        {
            InterstitialResponse resp = oManager.InterstialBeforeCaptcha(ug, uig, sr, cid, cc, fc, pid, sentry_status, conid);
            if (!string.IsNullOrEmpty(resp.RedirectURL))
            {
                return Redirect(resp.RedirectURL);
            }
            ViewBag.ug = resp.UserGuid;
            ViewBag.uig = resp.UserInvitationGuid;
            ViewBag.fc = resp.FirstClick;
            ViewBag.errMsg = "";
            ViewBag.AcceptanceMsg = "";
            ViewBag.cid = resp.ClientId;
            ViewBag.recaptchaToken = "";
            ViewBag.UserLanguage = resp.UserLanguage;
            RecaptchaClientModel recaptchaObj = new RecaptchaClientModel();
            string CaptchaCountryIDs = ConfigurationManager.AppSettings["CaptchaCountryID"].ToString();
            List<int> countryIds = CaptchaCountryIDs.Split(',').Select(int.Parse).ToList();
            if (countryIds.Contains(Convert.ToInt32(conid)))
            {
                recaptchaObj.CaptchaClient = 2;
            }
            else
            {
                recaptchaObj.CaptchaClient = new Random().Next(CaptchaMinClients, CaptchaMaxClients);
            }
            recaptchaObj.ClientSiteKey = recaptchaObj.GetCaptchaSiteKey();
            return View("/views/pages/Recaptcha.cshtml", recaptchaObj);
        }
        #endregion

        #region
        [HttpGet]
        public string GetCookie(Guid ug, int cid)
        {
            string sh1val = string.Empty;
            string mdval = string.Empty;
            string sha256val = string.Empty;
            string result = oManager.GetCookie(ug, cid);
            string emailAdd = result.Split(',')[1];
            if (emailAdd != "")
            {
                mdval = md5(emailAdd);
                sh1val = Sha1hash(emailAdd);
                sha256val = sha256hash(emailAdd);
            }
            return result + (',') + mdval + (',') + sh1val + (',') + sha256val;

        }
        #endregion

        #region Sha1 Hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public string Sha1hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        #endregion

        #region sha256 hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public String sha256hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        #endregion

        #region HMACSha256Hash
        public string HMACSha256hash(string str, string key)
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

        #region HMACSha1Hash
        public string HMACSha1hash(string str, string key)
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

        #region Save Cookie
        [HttpPost]
        public void saveCookie(Guid ug, string CookieIds, int cid)
        {
            oManager.saveCookie(ug, CookieIds, cid);
        }
        #endregion

        #region Validate Captcha
        /// <summary>
        /// Validate Captcha
        /// </summary>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult interstitial(Guid ug, Guid uig, string re, string re1, int cid, string cc, string fc, int captchaId, bool gdprtermsandconditions = false)
        {
            bool _isetrmschecked = gdprtermsandconditions;
            if (fc == "y" && _isetrmschecked == false)
            {
                ViewBag.ug = ug;
                ViewBag.uig = uig;
                ViewBag.errMsg = "";
                ViewBag.cid = cid;
                ViewBag.fc = fc;
                if (!string.IsNullOrEmpty(cc))
                {
                    ViewBag.UserLanguage = cc;
                }
                else
                {
                    ViewBag.UserLanguage = "en";
                }
                ViewBag.errMsg = "";
                ViewBag.AcceptanceMsg = "Acceptance of terms and conditions is required.";
                RecaptchaClientModel recaptchaObj = new RecaptchaClientModel();
                recaptchaObj.CaptchaClient = new Random().Next(CaptchaMinClients, CaptchaMaxClients);
                recaptchaObj.ClientSiteKey = recaptchaObj.GetCaptchaSiteKey();
                return View("/views/pages/Recaptcha.cshtml", recaptchaObj);
            }
            else
            {
                ViewBag.UserLanguage = cc;
                var recaptchaInfo = new RecaptchaClientModel();
                recaptchaInfo.CaptchaClient = captchaId;
                var provider = (CaptchaProvider)captchaId;
                recaptchaInfo.ClientToken = provider == CaptchaProvider.Cloudflare ? Request.Form["cf-turnstile-response"] :
                                            provider == CaptchaProvider.hCaptcha ? Request.Form["h-captcha-response"] :
                                            Request.Form["g-recaptcha-response"];
                recaptchaInfo.ug = ug.ToString();
                var responseInfo = false;
                if (!String.IsNullOrEmpty(recaptchaInfo.ClientToken))
                {
                    responseInfo = recaptchaInfo.GetResponse();
                }
                if (responseInfo)
                {
                    Logging.NLog.ClassLogger.Trace("ug:" + ug + " | uig:" + uig + "| Provider:" + provider + " | Response Success ");
                    string surveyUrl = oManager.InterstialAfterCaptcha(ug, uig, "y", cid);
                    return Redirect(surveyUrl);
                }
                else
                {
                    Logging.NLog.ClassLogger.Error("ug:" + ug + " | uig:" + uig + " | Response Fail |" + "Url:" + Request.Url);
                    ViewBag.ug = ug;
                    ViewBag.uig = uig;
                    ViewBag.errMsg = "";
                    ViewBag.cid = cid;
                    ViewBag.fc = "n";
                    if (!string.IsNullOrEmpty(cc))
                    {
                        ViewBag.UserLanguage = cc;
                    }
                    else
                    {
                        ViewBag.UserLanguage = "en";
                    }
                    ViewBag.AcceptanceMsg = "";
                    ViewBag.errMsg = "Invalid Captcha";
                    RecaptchaClientModel recaptchaObj = new RecaptchaClientModel();
                    recaptchaObj.CaptchaClient = captchaId;
                    recaptchaObj.ClientSiteKey = recaptchaObj.GetCaptchaSiteKey();
                    return View("/views/pages/Recaptcha.cshtml", recaptchaObj);
                }
            }
        }
        #endregion

        #region Helper Methods

        #region RedirectToSurveyUrl
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string RedirectToSurveyUrl(Guid ug, Guid uig, string sr, int clientid)
        {
            SurveyUrl survyUrl = new SurveyUrl();
            string surveyUrl = string.Empty;
            try
            {
                Recaptcha objRecaptcha = oManager.GetRecaptchaEntryInfo(uig, ug, clientid);
                //if survey url is not null simply redirect to survey_url else continue
                if (!string.IsNullOrEmpty(objRecaptcha.SurveyUrl))
                {
                    surveyUrl = objRecaptcha.SurveyUrl;
                }
                else
                {
                    if (objRecaptcha.HasPrescreenerQuestions)
                    {
                        //Get all Quotas taht are opened/closed in that Project from Activity Database.
                        List<Quotas> lstQuotas = oManager.GetQuotaCompletesaccesses(objRecaptcha.ProjectId, objRecaptcha.BalancingTypeId, ug, clientid);
                        var json = JsonConvert.SerializeObject(lstQuotas);

                        // to update the status of Target Connected with the Quotas
                        string RedirectSurURL = string.Empty;
                        RedirectSurURL = oManager.QuotaStatusUpdate(objRecaptcha.ProjectId, json, clientid, objRecaptcha.UserInvitationGuid);
                        if (!string.IsNullOrEmpty(RedirectSurURL))
                        {
                            return RedirectSurURL;
                        }
                        //Check if the User has Matched Any Quotas and in that if any Quota is clsoed we need to mark the member as PS OQs/Balance OQs.
                        //Recaptcha obj = oManager.GetSelectedQuotas(objRecaptcha.UserInvitationGuid, ug, json, objRecaptcha.IsInternalMember, objRecaptcha.ProjectId, objRecaptcha.UserId, objRecaptcha.UserInvitationId, clientid);
                        ////means if the Member is marked as any PS oq, or Balance OQ thenw e will get the URL.
                        //if (!string.IsNullOrEmpty(obj.SurveyUrl))
                        //{
                        //    surveyUrl = obj.SurveyUrl;
                        //}
                        //else
                        //{
                        //Here Step7 GUID is the GUID, we generate on Re-Captcha page Submit.+ User Invitation GUID is the Final GUID, used in get the Survey URL.
                        survyUrl = oManager.GetSurveyUrl(ug, objRecaptcha.Step7Guid, objRecaptcha.Source, clientid);
                        if (survyUrl.client_id == 7047 || survyUrl.client_id == 7159)
                        {
                            string PartnerGUID = string.Empty;
                            string TolunaAPIKey = string.Empty;
                            PartnerGUID = ConfigurationManager.AppSettings[objRecaptcha.CountryID.ToString()].ToString();
                            TolunaAPIKey = ConfigurationManager.AppSettings["TolunaAPIKey"].ToString();
                            //{IP_ES_URL}/IPExternalSamplingService/ExternalSample/{PanelGuid}/{MemberCode}/Invite/{QuotaID}
                            string TolunaSurveyURL = ConfigurationManager.AppSettings["TolunaSurveyURL"].ToString() + PartnerGUID + "/" + ug + "/Invite/" + objRecaptcha.DemandAPIQuotaID;
                            GetInvite objInvite = new GetInvite();
                            objInvite = TolunaSurveyURLGet(TolunaSurveyURL, TolunaAPIKey);
                            logger.Trace("GET Invite | " + objInvite.URL + "&uig=" + objRecaptcha.UserInvitationGuid + " | " + objInvite.Result + " | " + objInvite.ResultCode);
                            if (!string.IsNullOrEmpty(objInvite.URL))
                            {
                                survyUrl.ProjectUrl = objInvite.URL + "&uig=" + objRecaptcha.UserInvitationGuid;
                                //Task 9140: Toluna - CPI Changes at Member Registration
                                if (survyUrl.cpi > objInvite.PartnerAmount && ConfigurationManager.AppSettings["TolunaCPIRefresh"].ToString() == "1")
                                {
                                    if ((survyUrl.cpi - objInvite.PartnerAmount) >= 0.11)
                                    {
                                        logger.Trace("TolunaProjectRefresh | Project GUID" + survyUrl.projectGuid + "CPI" + survyUrl.cpi + "Partner Amount" + objInvite.PartnerAmount);
                                        oManager.ProjectRefresh(survyUrl.projectGuid, "erick@precisionsample.com", "t", objInvite.PartnerAmount);
                                    }
                                }
                            }
                            else
                            {
                                List<int> clientTerm = new List<int> { 3, 4, 6, 12, 13, 14, 17, 20, 21, 22, 23, 24, 25, 26, 30, 32, 33, 34, 35, 36, 37, 43 };
                                List<int> overQuota = new List<int> { 5, 8, 9, 10, 15, 16, 27, 28, 29, 39, 40, 41, 45, 46 };
                                if (clientTerm.Contains(objInvite.ResultCode))
                                {
                                    survyUrl.ProjectUrl = "https://e.reachcollective.com/e/psr?usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4&ug=" + ug + "&uig=" + objRecaptcha.UserInvitationGuid + "&marketplace_status=" + objInvite.ResultCode.ToString();
                                }
                                if (overQuota.Contains(objInvite.ResultCode))
                                {
                                    survyUrl.ProjectUrl = "https://e.reachcollective.com/e/psr?usg=67B98BED-9C3F-42AE-BDD3-7E15F9C17F00&ug=" + ug + "&uig=" + objRecaptcha.UserInvitationGuid + "&marketplace_status=" + objInvite.ResultCode.ToString();
                                }
                                if (objInvite.ResultCode == 31)
                                {
                                    survyUrl.ProjectUrl = "https://e.reachcollective.com/e/psr?usg=5CE933ED-9891-4CD7-8AC6-B529C58C6B55&ug=" + ug + "&uig=" + objRecaptcha.UserInvitationGuid + "&marketplace_status=" + objInvite.ResultCode.ToString();
                                }
                                if (objInvite.ResultCode == 38)
                                {
                                    survyUrl.ProjectUrl = "https://e.reachcollective.com/e/psr?usg=664B50CB-E1E7-40CC-B2EB-A94E1D54228F&ug=" + ug + "&uig=" + objRecaptcha.UserInvitationGuid + "&marketplace_status=" + objInvite.ResultCode.ToString();
                                }
                                if (objInvite.ResultCode == 0)
                                {
                                    survyUrl.ProjectUrl = "https://e.reachcollective.com/e/psr?usg=F6A6B754-4CF8-41BB-B9AA-5B97C412B1F4&ug=" + ug + "&uig=" + objRecaptcha.UserInvitationGuid + "&marketplace_status=" + objInvite.ResultCode.ToString();
                                }
                            }

                        }
                        //#region Task 7474: SHA-1 Hashing Guide - Lucid Testing
                        if (survyUrl.client_id == 6331)
                        {
                            string Fedkey = ConfigurationManager.AppSettings["FedHashKey"].ToString();
                            string param = oManager.GetProfileAsURLParams(ug.ToString(), clientid);
                            if (!string.IsNullOrEmpty(param))
                            {
                                survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&{param}&";
                            }
                            else
                            {
                                survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&";
                            }
                            string hashValue = HMACSha1hash(survyUrl.ProjectUrl, Fedkey);
                            survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}hash={hashValue}";
                            logger.Trace($"SurveyURL: {survyUrl.ProjectUrl} | User GUID: {ug}");
                        }
                        //Task 10457: Paradigm Secure Redirects
                        if (survyUrl.client_id == 7201)
                        {
                            string ParadigmSampleKey = ConfigurationManager.AppSettings["ParadigmHashKey"].ToString();
                            string hash = HMACSha256hash(survyUrl.ProjectUrl, ParadigmSampleKey);
                            survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&hash={hash}";
                            logger.Trace($"SurveyURL: {survyUrl.ProjectUrl} | User GUID: {ug}");
                            surveyUrl = survyUrl.ProjectUrl;
                        }
                        //#endregion 
                        //Task 8887: Marketcast Signed Link
                        if (survyUrl.client_id == 378)
                        {
                            string MarketcastKey = ConfigurationManager.AppSettings["MarketcastKey"].ToString();
                            survyUrl.ProjectUrl = survyUrl.ProjectUrl + "&_k=71";
                            string hashValue = ForstaHMACSHA1(survyUrl.ProjectUrl, MarketcastKey, survyUrl.client_id);
                            survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&_s={hashValue}";
                            logger.Trace($"Marketcast Signed Link SurveyURL: {survyUrl.ProjectUrl}");
                            surveyUrl = survyUrl.ProjectUrl;
                        }
                        else
                        {
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, null);
                        }
                        if (survyUrl.client_id == 392)
                        {
                            string AceUIG = string.Empty;
                            string AcePswd = ConfigurationManager.AppSettings["AcePswd"];
                            AceUIG = objRecaptcha.UserInvitationGuid.ToString().ToLower();
                            objRecaptcha.UserInvitationGuid = new Guid(AceUIG);
                            string encrypted = Encrypt(objRecaptcha.UserInvitationGuid, AcePswd);
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, encrypted);
                        }
                        else
                        {
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, null);
                        }
                        // Bera survey url code
                        if (survyUrl.client_id == 7191)
                        {
                            ReserveCatBrands brandData = new ReserveCatBrands();
                            if (string.IsNullOrEmpty(survyUrl.Payload))
                            {
                                ReserveCatBrandRequest respondant = MockRespondantRequest();
                                M2MToken objM2MToken = new M2MToken();
                                string apiUrl = ConfigurationManager.AppSettings["BersAPIURL"].ToString();
                                objM2MToken = oManager.GetM2MToken();
                                string token = objM2MToken.access_token;
                                JObject jObj = JObject.Parse(survyUrl.RespondentJSON);
                                brandData = oManager.RespondentReserve(survyUrl.projectId, jObj, token, apiUrl);
                            }
                            else
                            {
                                brandData = JsonConvert.DeserializeObject<ReserveCatBrands>(survyUrl.Payload);
                            }
                            StringBuilder queryString = new StringBuilder();
                            int index = 1;
                            foreach (var item in brandData.mainBrands)
                            {
                                if (index > 12) break;
                                var brand = item.brand;
                                queryString.Append($"dbrandid{index}={Uri.EscapeDataString(brand.brandId.ToString())}&");
                                queryString.Append($"dcategoryid{index}={Uri.EscapeDataString(brand.categoryId.ToString())}&");
                                queryString.Append($"dlogourl{index}={Uri.EscapeDataString(brand.logoUrl)}&");
                                queryString.Append($"dbrandlabel{index}={WebUtility.UrlEncode(item.brandLabel)}&");
                                index++;
                            }

                            int i = 1;
                            foreach (var item in brandData.mainBrands)
                            {
                                var attr = item.wordings;
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_1={WebUtility.UrlEncode(attr.peopleText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_2={WebUtility.UrlEncode(attr.priceText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_3={WebUtility.UrlEncode(attr.promotionText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_4={WebUtility.UrlEncode(attr.productText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_5={WebUtility.UrlEncode(attr.placementText)}&");
                                queryString.Append($"Brand{i}_QPart7A_2x={WebUtility.UrlEncode(attr.sowQuestionText)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1={WebUtility.UrlEncode(attr.usecon12Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1_1={WebUtility.UrlEncode(attr.usecon1Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1_2={WebUtility.UrlEncode(attr.usecon2Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2={WebUtility.UrlEncode(attr.usecon34Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2_1={WebUtility.UrlEncode(attr.usecon3Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2_2={WebUtility.UrlEncode(attr.usecon4Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3={WebUtility.UrlEncode(attr.usecon56Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3_1={WebUtility.UrlEncode(attr.usecon5Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3_2={WebUtility.UrlEncode(attr.usecon6Text)}&");
                                queryString.Append($"Brand{i}_BOTTOM={WebUtility.UrlEncode(attr.volumetricsBottomValue)}&");
                                queryString.Append($"Brand{i}_QPart7Ax1xSTART={WebUtility.UrlEncode(attr.volumetricsLeftUnits)}&");
                                queryString.Append($"Brand{i}_volumetricsMaxValue={WebUtility.UrlEncode(attr.volumetricsMaxValue)}&");
                                queryString.Append($"Brand{i}_volumetricsMinValue={WebUtility.UrlEncode(attr.volumetricsMinValue)}&");
                                queryString.Append($"Brand{i}_QPart7Ax={WebUtility.UrlEncode(attr.volumetricsQuestionText)}&");
                                queryString.Append($"Brand{i}_QPart7Ax1xEND={WebUtility.UrlEncode(attr.volumetricsRightUnits)}&");
                                queryString.Append($"Brand{i}_TOP={WebUtility.UrlEncode(attr.volumetricsTopValue)}&");
                                queryString.Append($"Brand{i}_UNIT_TEXT={WebUtility.UrlEncode(attr.volumetricsUnitText)}&");
                                i++;
                            }
                            if (queryString.Length > 0)
                                queryString.Length--;
                            logger.Trace($"Bera SurveyURL: UIG: {uig} | ProjectId: {survyUrl.projectId} | {survyUrl.ProjectUrl}&{queryString.ToString()}");
                            surveyUrl = $"{survyUrl.ProjectUrl}&{queryString.ToString()}";
                        }
                    }
                    else
                    {
                        survyUrl = oManager.GetSurveyUrl(ug, objRecaptcha.Step7Guid, objRecaptcha.Source, clientid);

                        //#region Task 7474: SHA-1 Hashing Guide - Lucid Testing
                        if (survyUrl.client_id == 6331)
                        {
                            string Fedkey = ConfigurationManager.AppSettings["FedHashKey"].ToString();
                            string param = oManager.GetProfileAsURLParams(ug.ToString(), clientid);
                            if (!string.IsNullOrEmpty(param))
                            {
                                survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&{param}&";
                            }
                            else
                            {
                                survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&";
                            }
                            string hashValue = HMACSha1hash(survyUrl.ProjectUrl, Fedkey);
                            survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}hash={hashValue}";
                            logger.Trace($"SurveyURL: {survyUrl.ProjectUrl} | User GUID: {ug}");
                        }
                        //#endregion
                        //Task 8887: Marketcast Signed Link
                        if (survyUrl.client_id == 378)
                        {
                            string MarketcastKey = ConfigurationManager.AppSettings["MarketcastKey"].ToString();
                            survyUrl.ProjectUrl = survyUrl.ProjectUrl + "&_k=71";
                            string hashValue = ForstaHMACSHA1(survyUrl.ProjectUrl, MarketcastKey, survyUrl.client_id);
                            survyUrl.ProjectUrl = $"{survyUrl.ProjectUrl}&_s={hashValue}";
                            logger.Trace($"Marketcast Signed Link SurveyURL: {survyUrl.ProjectUrl}");
                            surveyUrl = survyUrl.ProjectUrl;
                        }
                        else
                        {
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, null);
                        }
                        if (survyUrl.client_id == 392)
                        {
                            string AceUIG = string.Empty;
                            string AcePswd = ConfigurationManager.AppSettings["AcePswd"];
                            AceUIG = objRecaptcha.UserInvitationGuid.ToString().ToLower();
                            objRecaptcha.UserInvitationGuid = new Guid(AceUIG);
                            string encrypted = Encrypt(objRecaptcha.UserInvitationGuid, AcePswd);
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, encrypted);
                        }
                        else
                        {
                            surveyUrl = RedirectSurveyUrl(survyUrl.ProjectUrl, ug, objRecaptcha.UserInvitationGuid, clientid, null);
                        }
                        // Bera survey url code
                        if (survyUrl.client_id == 7191)
                        {
                            ReserveCatBrands brandData = new ReserveCatBrands();
                            if (string.IsNullOrEmpty(survyUrl.Payload))
                            {
                                ReserveCatBrandRequest respondant = MockRespondantRequest();
                                M2MToken objM2MToken = new M2MToken();
                                string apiUrl = ConfigurationManager.AppSettings["BersAPIURL"].ToString();
                                objM2MToken = oManager.GetM2MToken();
                                string token = objM2MToken.access_token;
                                JObject jObj = JObject.Parse(survyUrl.RespondentJSON);
                                brandData = oManager.RespondentReserve(survyUrl.projectId, jObj, token, apiUrl);
                            }
                            else
                            {
                                brandData = JsonConvert.DeserializeObject<ReserveCatBrands>(survyUrl.Payload);
                            }
                            StringBuilder queryString = new StringBuilder();
                            int index = 1;
                            foreach (var item in brandData.mainBrands)
                            {
                                if (index > 12) break;
                                var brand = item.brand;
                                queryString.Append($"dbrandid{index}={Uri.EscapeDataString(brand.brandId.ToString())}&");
                                queryString.Append($"dcategoryid{index}={Uri.EscapeDataString(brand.categoryId.ToString())}&");
                                queryString.Append($"dlogourl{index}={Uri.EscapeDataString(brand.logoUrl)}&");
                                queryString.Append($"dbrandlabel{index}={WebUtility.UrlEncode(item.brandLabel)}&");
                                index++;
                            }

                            int i = 1;
                            foreach (var item in brandData.mainBrands)
                            {
                                var attr = item.wordings;
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_1={WebUtility.UrlEncode(attr.peopleText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_2={WebUtility.UrlEncode(attr.priceText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_3={WebUtility.UrlEncode(attr.promotionText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_4={WebUtility.UrlEncode(attr.productText)}&");
                                queryString.Append($"Brand{i}_ATTR_P3_Wording_5={WebUtility.UrlEncode(attr.placementText)}&");
                                queryString.Append($"Brand{i}_QPart7A_2x={WebUtility.UrlEncode(attr.sowQuestionText)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1={WebUtility.UrlEncode(attr.usecon12Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1_1={WebUtility.UrlEncode(attr.usecon1Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_1_2={WebUtility.UrlEncode(attr.usecon2Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2={WebUtility.UrlEncode(attr.usecon34Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2_1={WebUtility.UrlEncode(attr.usecon3Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_2_2={WebUtility.UrlEncode(attr.usecon4Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3={WebUtility.UrlEncode(attr.usecon56Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3_1={WebUtility.UrlEncode(attr.usecon5Text)}&");
                                queryString.Append($"Brand{i}_ATTR_P4_Wording_3_2={WebUtility.UrlEncode(attr.usecon6Text)}&");
                                queryString.Append($"Brand{i}_BOTTOM={WebUtility.UrlEncode(attr.volumetricsBottomValue)}&");
                                queryString.Append($"Brand{i}_QPart7Ax1xSTART={WebUtility.UrlEncode(attr.volumetricsLeftUnits)}&");
                                queryString.Append($"Brand{i}_volumetricsMaxValue={WebUtility.UrlEncode(attr.volumetricsMaxValue)}&");
                                queryString.Append($"Brand{i}_volumetricsMinValue={WebUtility.UrlEncode(attr.volumetricsMinValue)}&");
                                queryString.Append($"Brand{i}_QPart7Ax={WebUtility.UrlEncode(attr.volumetricsQuestionText)}&");
                                queryString.Append($"Brand{i}_QPart7Ax1xEND={WebUtility.UrlEncode(attr.volumetricsRightUnits)}&");
                                queryString.Append($"Brand{i}_TOP={WebUtility.UrlEncode(attr.volumetricsTopValue)}&");
                                queryString.Append($"Brand{i}_UNIT_TEXT={WebUtility.UrlEncode(attr.volumetricsUnitText)}&");
                                i++;
                            }
                            if (queryString.Length > 0)
                                queryString.Length--;
                            logger.Trace($"Bera SurveyURL: UIG: {uig} | ProjectId: {survyUrl.projectId} | {survyUrl.ProjectUrl}&{queryString.ToString()}");
                            surveyUrl = $"{survyUrl.ProjectUrl}&{queryString.ToString()}";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return surveyUrl;
        }
        #endregion

        #region Encrypt
        public static string Encrypt(Guid plainText, string passphrase)
        {
            // generate salt 
            byte[] key, iv; byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt); DeriveKeyAndIV(passphrase, salt, out key, out iv);
            // encrypt bytes 
            byte[] encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
            // add salt as ﬁrst 8 bytes 
            byte[] encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + 8];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("Salted__"), 0, encryptedBytesWithSalt, 0, 8);
            Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, 8, salt.Length);
            Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + 8, encryptedBytes.Length);
            // base64 encode 
            return Convert.ToBase64String(encryptedBytesWithSalt);
        }
        private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
        {
            // generate key and iv 
            List<byte> concatenatedHashes = new List<byte>(48);
            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            byte[] currentHash = new byte[0]; MD5 md5 = MD5.Create();
            bool enoughBytesForKey = false;
            // See http://www.openssl.org/docs/crypto/ EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM 
            while (!enoughBytesForKey)
            {
                int preHashLength = currentHash.Length + password.Length + salt.Length;
                byte[] preHash = new byte[preHashLength];
                Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
                Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);
                currentHash = md5.ComputeHash(preHash);
                concatenatedHashes.AddRange(currentHash);
                if (concatenatedHashes.Count >= 48) enoughBytesForKey = true;
            }
            key = new byte[32];
            iv = new byte[16];
            concatenatedHashes.CopyTo(0, key, 0, 32);
            concatenatedHashes.CopyTo(32, iv, 0, 16);
            md5.Clear(); md5 = null;
        }
        static byte[] EncryptStringToBytesAes(Guid plainText, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (plainText == null || plainText == Guid.Empty)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            // Declare the stream used to encrypt to an in memory 
            // array of bytes. 
            MemoryStream msEncrypt;
            // Declare the RijndaelManaged object 
            // used to encrypt the data. 
            RijndaelManaged aesAlg = null;
            try
            {
                // Create a RijndaelManaged object 
                // with the speciﬁed key and IV. 
                aesAlg = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    KeySize = 256,
                    BlockSize = 128,
                    Key = key,
                    IV = iv
                };
                // Create an encryptor to perform the stream transform. 
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption. 
                msEncrypt = new MemoryStream(); using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream. 
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                        swEncrypt.Close();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object. 
                if (aesAlg != null) aesAlg.Clear();
            }
            // Return the encrypted bytes from the memory stream. 
            return msEncrypt.ToArray();
        }

        public static string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }
        #endregion

        #region Redirect Url 
        /// <summary>
        /// Redirect SurveyUrl
        /// </summary>
        /// <param name="surveyUrl">SurveyUrl</param>
        /// <param name="ug">UserGuid</param>
        /// <param name="uig">UserInvitationGuid</param>
        public string RedirectSurveyUrl(string surveyUrl, Guid ug, Guid uig, int clientid, string encrypted)
        {
            string redirectSurveyUrl = string.Empty;

            if (surveyUrl.Contains("%%payload%%"))
            {
                string url = string.Empty;
                url = GetPeanutlabsRediectUrl(surveyUrl, ug, uig, clientid);
                redirectSurveyUrl = url;
            }
            else
            {
                if ((surveyUrl.Contains("&pid=prs&") || surveyUrl.Contains("source=prs&")) & surveyUrl.Contains("%%hash%%"))
                {
                    string invokeurl = surveyUrl;
                    string[] s1 = invokeurl.Split('?');
                    string[] s3 = s1[0].Split('/');
                    string sid = s3[s3.Length - 1];
                    string[] s2 = s1[1].Split('&');
                    string _invokesid = string.Empty;
                    string _invokepid = string.Empty;
                    string _invokekey = ConfigurationManager.AppSettings["InvokeMD5hash"].ToString();
                    string _invokeuserid = string.Empty;
                    _invokesid = sid;
                    _invokepid = "prs";
                    string _md5 = string.Empty;
                    for (int i = 0; i < s2.Length; i++)
                    {
                        //if (s2[i].StartsWith("sid="))
                        //{
                        //    _invokesid = s2[i].Split('=')[1];
                        //}
                        if (s2[i].StartsWith("pid="))
                        {
                            _invokepid = s2[i].Split('=')[1];
                        }
                        if (s2[i].StartsWith("uid="))
                        {
                            _invokeuserid = s2[i].Split('=')[1];
                        }
                    }
                    //Hash the Value in Combination of all these Params with secret key
                    _md5 = md5(_invokesid + _invokepid + _invokeuserid + _invokekey);
                    invokeurl = invokeurl.Replace("%%hash%%", _md5);
                    redirectSurveyUrl = invokeurl;
                }
                else
                {
                    if (encrypted == null)
                    {
                        redirectSurveyUrl = surveyUrl;
                    }
                    else
                    {
                        string AceMetrixUIG = (HttpUtility.ParseQueryString(surveyUrl).Get("uig").ToString().ToLower());
                        uig = new Guid(AceMetrixUIG);
                        surveyUrl = RemoveQueryStringByKey(surveyUrl, "uig");
                        //surveyUrl = surveyUrl.Replace("uig=" + uig, "uig=" + HttpUtility.ParseQueryString(surveyUrl).Get("uig"));
                        redirectSurveyUrl = surveyUrl + "&uig=" + uig + "&vuig=" + encrypted;
                    }
                }


            }
            return redirectSurveyUrl;
        }
        #endregion

        #region Get Fed SurveyUrl
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSurveys"></param>
        public void RedirectToFedPages(Surveys oSurveys, Guid InvitationGuid)
        {
            //Redirect to terminate or OverQuota or rejected URLs .
            string _url = string.Empty;
            if (oSurveys.UserInvitationStatusId == 2 || oSurveys.UserInvitationStatusId == 7)
            {
                //Terminate Redirect
                _url = ConfigurationManager.AppSettings["FederatedTerminateURL"].ToString();
            }
            else if (oSurveys.UserInvitationStatusId == 4)
            {
                //Over Quota
                _url = ConfigurationManager.AppSettings["FederatedOverQuotaURL"].ToString();
            }
            else if (oSurveys.UserInvitationStatusId == 5)
            {
                //Rejected
                _url = ConfigurationManager.AppSettings["FederatedQOtherTermURL"].ToString();
            }
            else
            {
                //Redirect the member if fails Verity/Relevant/Other Security Reasons.
                _url = ConfigurationManager.AppSettings["FederatedQDuplicateURL"].ToString();
            }

            _url = _url.Replace("%%user_invitation_guid%%", InvitationGuid.ToString());
            _url = _url.Replace("%%EID%%", oSurveys.Fedresponseid.ToString());
            _url = _url.Replace("%%user_id%%", oSurveys.UserId.ToString());

            Response.Redirect(_url);
        }
        #endregion

        #region MD5 Encryption logic
        /// <summary>
        /// MD5 Encryption logic
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
            return str.ToString();
        }
        #endregion

        #region PeanutLabs Surveys
        /// <summary>
        /// PenutLabs Survey Url Build
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string GetPeanutlabsRediectUrl(string url, Guid ug, Guid uig, int clientid)
        {
            //Step1 : Get the User Info in PL Format.
            PLUserDeatails objPlUser = new PLUserDeatails();
            UserManager oUserManger = new UserManager();
            string applicationKey = ConfigurationManager.AppSettings["PLAppKey"].ToString(); //"abcdef0123456789abcdef0123456789";
            string payload = oUserManger.GetUserDetailsForPeanutLabs(ug, clientid);
            // string payload =@{"user_id":"14002-8967-9207cb59c4","cc":"US","sex":1,"dob":"1983-01-01","postal":"28201","profile_data":{"q121":["qx121-0"],"q157":["1"],"q186":["qx186-0","qx186-3","qx186-3"],"q101":["qx101-5"],"q102":["qx102-105"],"q122":["qx122-0"],"q129":["qx129-2"],"q158":["qx158-0"],"q180":["qx180-27"],"q182":["qx182-25"],"q184":["qx184-0"],"q159":["qx159-0"],"q160":["qx160-58"],"q161":["2015"],"q183":["qx183-1"],"q126":["qx126-10"],"q185":["qx185-17"]}}
            //@"{""user_id"":""14002-8967-9207cb59c4"",""cc"":""US"",""sex"":1,""dob"":""1990-04-10"",""postal"":""94104"",""profile_data"":{""q122"":[""qx122-0""],""q159"":[""qx159-1""],""q102"":[""qx102-105""],""q158"":[""qx158-3""],""q101"":[""qx101-2""],""q157"":[""3""],""ch-m"":[""4-2004""],""ch-f"":[""10-1998"",""11-1999""]}}";
            string _applicationId = ConfigurationManager.AppSettings["PLAppId"].ToString();
            HelperMethods oHelperMethod = new HelperMethods();
            string iv = oHelperMethod.getInitVector(); //Store the iv
            string encrypted_payload = oHelperMethod.EncryptIt(payload, applicationKey, iv);
            string decrypted_payload = oHelperMethod.DecryptIt(encrypted_payload, applicationKey, iv);
            string _rdUrl = url.Replace("%%application_id%%", _applicationId).Replace("%%user_invitation_guid%%", uig.ToString()).Replace("%%iv%%", iv).Replace("%%payload%%", encrypted_payload);
            return _rdUrl;

        }


        #endregion

        #region Post Request method
        /// <summary>
        /// Post Request
        /// </summary>
        /// <param name="RequestURL">Request Url</param>
        /// <returns></returns>
        public string PostRequest(string RequestURL)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
            LoginRequest.Method = "GET";

            HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        #endregion

        public string PutRequest(string postUrl, string LoginCredentials)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);

            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            LoginRequest.Method = "PUT";
            LoginRequest.ContentType = "application/json; charset=UTF-8";
            LoginRequest.ContentLength = data.Length;
            LoginRequest.Headers.Add("APIkey", ConfigurationManager.AppSettings["FEDS2SAPI"]);
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

        public string CINTGetRequest(string getUrl, string key)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + key);
            request.ContentType = "text/plain";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    return responseText;
                }
            }
        }

        public string CINTPostRequest(string postUrl, string LoginCredentials, string key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = LoginCredentials;
            byte[] data = encoding.GetBytes(postData);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            LoginRequest.Method = "POST";
            LoginRequest.ContentType = "application/json; charset=UTF-8";
            LoginRequest.ContentLength = data.Length;
            LoginRequest.Headers.Add("Authorization", "Bearer " + key);
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

        #region Toluna Survey URL Get
        public GetInvite TolunaSurveyURLGet(string postUrl, string key)
        {
            GetInvite objInvite = new GetInvite();
            string url = string.Empty;
            ASCIIEncoding encoding = new ASCIIEncoding();
            logMessage = "GetInvite | " + postUrl + "Key | " + key;
            logger.Trace(logMessage);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "GET";
            request.Headers.Add("API_AUTH_KEY", key);
            try
            {
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    logMessage = "TolunaSurveyURLGet | GetInvite: " + postUrl + " | Key: " + key + " | Exception: " + text;
                    logger.Info(logMessage);
                    return JsonConvert.DeserializeObject<GetInvite>(text);
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    // TODO: Handle response being null
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        logMessage = "TolunaSurveyURLGet | GetInvite: " + postUrl + " | Key: " + key + " | Exception: " + text;
                        logger.Error(logMessage);
                        return JsonConvert.DeserializeObject<GetInvite>(text);
                    }
                }
            }
        }
        #endregion


        public ActionResult top20(string leadid, string pg, string issam)
        {
            return View("/Views/pages/top20.cshtml");
        }
        #region Get Top 20 Profile Questions
        /// <summary>
        /// Get Top 20 Profile Questions
        /// </summary>
        /// <param name="leadguid"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult Top20Get(string leadguid)
        {
            List<ProfileQuestions> lstQuestion = new List<ProfileQuestions>();

            var Pagedata = oManager.GetquestionsforTop20(new Guid(leadguid));
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
        public string Top20Save(string leadguid, string xml)
        {
            string _message = string.Empty;
            string url = string.Empty;
            string BadWordsFlag = string.Empty;
            string BadPhraseFlag = string.Empty;
            string GarbageWordsFlag = string.Empty;
            string NonEngagedFlag = string.Empty;
            string PastedTextFlag = string.Empty;
            string RobotFlag = string.Empty;
            string ErrorMessage = string.Empty;
            string memberUrl = string.Empty;


            memberUrl = oManager.Top20SaveOptions(xml, new Guid(leadguid));
            return memberUrl;
            //writer.Write(1);
        }
        #endregion

        public ActionResult lbs(string leadid, string pg, string is_t)
        {
            return View("/Views/pages/lbmsg.cshtml");
        }
        public ActionResult msglb(string leadid, string pg, string is_t)
        {
            return View("/Views/pages/msglb.cshtml");
        }

        public string PureSpectrumSHA1(string key)
        {
            //string url = "https://e.reachcollective.com/e/psr?usg=D5F04CF6-50AB-4617-9B0F-95B23A07488C&supplier_id=82&survey_id=15317050&ps_rstatus=40&ps_psid=de86510b-ae8d-bf21-7ac0-3fa1cab111e7&uig=F5B75741-143D-44B4-8D7A-3A243EBEB6F0&ug=F5B75741-143D-44B4-8D7A-3A243EBEB6F0&ps_s_hash=a13d94c66af799448850890de48cfdc49c78c4b1";
            string url = Request.Url.AbsoluteUri;
            int position = url.IndexOf("&ps_s_hash=");
            string result = url.Substring(0, position);
            var sha1 = new System.Security.Cryptography.SHA1Managed();
            var plaintextBytes = Encoding.UTF8.GetBytes(result + key);
            var hashBytes = sha1.ComputeHash(plaintextBytes);
            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.AppendFormat("{0:x2}", hashByte);
            }
            var hashString = sb.ToString();
            return hashString;
        }

        public string ForstaHMACSHA1(string str, string key, int? client_id)
        {
            if (string.IsNullOrEmpty(str))
            {
                Uri URL = new Uri(Request.Url.AbsoluteUri);
                str = "/e/psr" + URL.Query;
            }
            int position = client_id == 378 ? str.IndexOf("/survey") : str.IndexOf("&_s=");
            string result = client_id == 378 ? str.Substring(position) : str.Substring(0, position);
            byte[] messageBytes = Encoding.UTF8.GetBytes(result);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            string hmacCode = string.Empty;
            using (HMACSHA1 hmac = new HMACSHA1(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);
                hmacCode = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return hmacCode;
        }

        public HttpWebResponse HttpPost(string RequestURL, byte[] payload, string Method)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestURL);
            request.Method = Method;
            request.ContentType = "application/json";
            request.Accept = "application/json;version=2.0";
            request.ContentLength = payload.Length;
            Stream LoginRequestStream = request.GetRequestStream();
            LoginRequestStream.Write(payload, 0, payload.Length);
            LoginRequestStream.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                logMessage = "Member Registration | " + RequestURL + "Method | " + Method + "Response | " + ex;
                logger.Error(logMessage);
                return response;
            }
        }

        public string TolunaMemReg(string RequestURL, string postData, string Method)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            logMessage = "Member Registration | " + RequestURL + "JSON | " + postData;
            logger.Trace(logMessage);
            byte[] data = encoding.GetBytes(postData);
            var LoginResponse = HttpPost(RequestURL, data, Method);
            string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
            StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
            String strMainPage = sr.ReadToEnd();
            sr.Close();
            return strMainPage;
        }

        public ReserveCatBrandRequest MockRespondantRequest()
        {
            return new ReserveCatBrandRequest()
            {
                respondentId = "testRespondentId_PS_User",
                respondentLocale = "en_US",
                bucketId = "4_3_112"
            };
        }

        public void RespondentComplete(RespondentComplete objRespondent, string _accessToken, string _survey_API_Url)
        {
            ReserveCatBrands brands = new ReserveCatBrands();
            string json = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                json = JsonConvert.SerializeObject(objRespondent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{_survey_API_Url}/respondent/complete", content).Result;
                logger.Trace($"Bera Respondent Complete:  {response.ToString()} | Respondent JSON: {json}");
            }
            catch (Exception ex)
            {
                logger.Trace($"Bera Respondent Complete:  {ex.ToString()} | Respondent JSON: {json}");
            }
        }
    }
}