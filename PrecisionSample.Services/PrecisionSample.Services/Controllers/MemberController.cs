using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PrecisionSample.Services.Components.Entites;
using PrecisionSample.Services.Components.BussinessLayer;
using System.Xml.Linq;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Script.Serialization;
namespace PrecisionSample.Services.Controllers
{
    [RoutePrefix("api/Member")]
    public class MemberController : ApiController
    {
        public MemberBussinesslayer memberobj = new MemberBussinesslayer();
        Output objOutput = new Output();

        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Create(Member member)
        {
            string _methodName = "Create";
            if (ModelState.IsValid)
            {
                string msg = memberobj.Create(member);
                //objOutput = Status(objOutput.ErroMessage);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = ModelStateErrors(_methodName);
                string msg = objOutput.ErrorMessage;
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
        }

        [HttpPost]
        [Route("CreateWL")]
        public HttpResponseMessage CreateWL(User oUser)
        {
            string _methodName = "CreateWL";
            if (ModelState.IsValid)
            {
                string msg = memberobj.CreateWL(oUser);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = ModelStateErrors(_methodName);
                return Request.CreateResponse(HttpStatusCode.OK, objOutput);
            }
        }

        [HttpPost]
        [Route("CreateWidget")]
        public HttpResponseMessage CreateWidget(MemberEntity oUser)
        {
            string _methodName = "CreateWL";
            string msg = memberobj.CreateWidget(oUser);
            return Request.CreateResponse(HttpStatusCode.OK, msg);

        }


        [HttpPost]
        [Route("Update")]
        public HttpResponseMessage Update(UMember member)
        {
            string _methodName = "Update";
            if (ModelState.IsValid && member.UserGuid != Guid.Empty)
            {
                string msg = memberobj.Update(member);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = ModelStateErrors(_methodName);
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }

        [HttpPost]
        [Route("UpdateWL")]
        public HttpResponseMessage UpdateWL(User oUser)
        {
            string _methodName = "UpdateWL";
            if (oUser.UserGuid != Guid.Empty)
            {
                string msg = memberobj.UpdateWL(oUser);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = ModelStateErrors(_methodName);
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput);
            }
        }

        [HttpPost]
        [Route("Unsubscribe")]
        public HttpResponseMessage Unsubscribe(int Rid, string UserName)
        {
            string _methodName = "Unsubscribe";
            if (Rid != 0 && !string.IsNullOrEmpty(UserName))
            {
                string msg = memberobj.Unsubscribe(Rid, UserName);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                if (Rid == 0 && string.IsNullOrEmpty(UserName))
                    objOutput = Status("Rid is Required ; UserGuid Or EmailAddress is Required");
                else if (string.IsNullOrEmpty(UserName))
                    objOutput = Status("UserGuid Or EmailAddress is Required");
                else if (Rid == 0)
                    objOutput = Status("Rid is Required");
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }
        [HttpPost]
        [Route("IsProjectOpen")]
        public HttpResponseMessage IsProjectopen(int projectId)
        {
            if (projectId > 0)
            {
                string msg = memberobj.IsProjectOpen(projectId);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = Status("Valid ProjectId is Required");
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }
        [HttpPost]
        [Route("ProjectsClosedToday")]
        public HttpResponseMessage GetProjectsClosedToday()
        {

            string msg = memberobj.GetProjectsClosedToday();
            return Request.CreateResponse(HttpStatusCode.OK, msg);

        }
        [HttpPost]
        [Route("Resubscribe")]
        public HttpResponseMessage Resubscribe(Guid UserGuid, int ClientId)
        {
            string _methodName = "Resubscribe";
            if (UserGuid != Guid.Empty)
            {
                string msg = memberobj.Reubscribe(UserGuid, ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                objOutput = ModelStateErrors(_methodName);
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }
        [HttpPost]
        [Route("GetProfile")]
        public HttpResponseMessage GetPartnerUserProfilesList(string UserGuid, int ClientId)
        {
            string message = string.Empty;
            List<Profile> lstProfiles = new List<Profile>();
            if (string.IsNullOrEmpty(UserGuid))
            {

                message = "UserGuid is Required";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                if (new Guid(UserGuid) != Guid.Empty)
                {
                    lstProfiles = memberobj.GetPartnerUserProfilesList(UserGuid, ClientId);
                    if (lstProfiles.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, lstProfiles);
                    }
                    else
                    {
                        message = "No Profile List was found.";
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                else
                {
                    message = "UserGuid is Required";
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
        }
        [HttpPost]
        [Route("GetSurveys")]
        public HttpResponseMessage GetSurveys(string UserGuid, int ClientId)
        {
            string message = string.Empty;
            List<Surveys> lstSurveys = new List<Surveys>();
            if (!string.IsNullOrEmpty(UserGuid))
            {
                if (new Guid(UserGuid) != Guid.Empty)
                {
                    string[] _gsorgIds = ConfigurationManager.AppSettings["getsurveysOrgIds"].ToString().Split(';');
                    int _gscount = 0;
                    var jsonString = string.Empty;
                    //For New get surveys Calls.
                    HttpClient client = new HttpClient();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    Logging.NLog.ClassLogger.Info("GSNewAPi4|v2.asxm-Request:" + UserGuid + "| " + HttpContext.Current.Request.Url.AbsoluteUri + "&cid:" + ClientId);
                    string Url = ConfigurationManager.AppSettings["gsapiurl"].ToString();
                    client.BaseAddress = new Uri(Url);
                    //HTTP GET
                    var responseTask = client.GetStringAsync("SurveysGet?userGuid=" + new Guid(UserGuid) + "&clientId=" + Convert.ToInt32(ClientId));
                    responseTask.Wait();
                    jsonString = responseTask.Result;
                    if (jsonString.ToLower().Contains("no surveys"))
                    {

                    }
                    else
                    {
                        lstSurveys = new JavaScriptSerializer().Deserialize<List<Surveys>>(jsonString);
                    }
                    _gscount = _gscount + 1;                   
                    if (lstSurveys.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, lstSurveys);
                    }
                    else
                    {
                        message = "No Surveys were found for your profile.";
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                else
                {
                    message = "UserGuid is Required";
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            else
            {
                message = "UserGuid is Required";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        [HttpPost]
        [Route("GetSurveysAPI")]
        public HttpResponseMessage GetSurveysAPI(string UserGuid, int ClientId)
        {
            string message = string.Empty;
            List<ApiSurveys> lstSurveys = new List<ApiSurveys>();
            if (!string.IsNullOrEmpty(UserGuid))
            {
                if (new Guid(UserGuid) != Guid.Empty)
                {
                    lstSurveys = memberobj.GetSurveysforAPIPartnersOnly(UserGuid, ClientId);
                    if (lstSurveys.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, lstSurveys);
                    }
                    else
                    {
                        message = "No Surveys were found for your profile.";
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                else
                {
                    message = "UserGuid is Required";
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            else
            {
                message = "UserGuid is Required";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }

        [HttpPost]
        [Route("GetSurveysHistory")]
        public HttpResponseMessage GetSurveysHistory(string UserGuid, int ClientId)
        {
            string message = string.Empty;
            List<SurveyHistory> lstSurveyHistory = new List<SurveyHistory>();
            if (!string.IsNullOrEmpty(UserGuid) && new Guid(UserGuid) != Guid.Empty)
            {
                lstSurveyHistory = memberobj.GetSurveyHistory(UserGuid, ClientId);
                if (lstSurveyHistory.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, lstSurveyHistory);
                }
                else
                {
                    message = "No Survey History was found";
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
            else
            {
                message = "UserGuid is Required";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
        }
        [HttpPost]
        [Route("GetRewardsHistory")]
        public HttpResponseMessage GetRewardsHistory(int UserId, int ClientId, Guid UserGuid)
        {
            string message = string.Empty;
            Rewards objRewards = new Rewards();

            objRewards = memberobj.GetRewardsHistory(UserId, ClientId, UserGuid);
            if (objRewards.LstRewardHistory.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, objRewards);
            }
            else
            {
                message = "No Reward History was found";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

        }
        [HttpPost]
        [Route("GetRedeemHistory")]
        public HttpResponseMessage GetRedeemHistory(int UserId, int ClientId, Guid UserGuid)
        {
            string message = string.Empty;
            Rewards objRewards = new Rewards();

            objRewards = memberobj.GetRedeemHistory(UserId, ClientId, UserGuid);
            if (objRewards.LstRedeemptionHistory.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, objRewards);
            }
            else
            {
                message = "No Redeem History was found";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }

        }
        [HttpPost]
        [Route("UpdateProfile")]
        public string UpdateProfile(string xml)
        {
            //xml = "<member mid=\"C440CB7A - 0007 - 4DCC - AD19 - ED361A12EEB5\" rid=\"69\"><profiles><profile qid=\"137\" oid=\"3,5,7\"></profile><profile qid=\"138\" oid=\"1\"></profile></profiles></member> ";
            if (!string.IsNullOrEmpty(xml))
            {

                var xDoc = XDocument.Parse(xml);
                XNode status1 = xDoc.FirstNode; //to read the User GUID
                string ug = (((System.Xml.Linq.XElement)status1).FirstAttribute).Value.ToString();
                int rid = Convert.ToInt32((((System.Xml.Linq.XElement)status1).LastAttribute).Value);
                return memberobj.UpdateProfile(xml, ug, rid);
            }
            else
            {
                return "Response xml is Required";
            }
        }

        [HttpPost]
        [Route("UserLogin")]
        public HttpResponseMessage UserLogin(string EmailAddress, string Password, string DomainUrl)
        {
            string message = string.Empty;
            User oUser = new User();
            oUser.EmailAddress = EmailAddress;
            oUser.Password = Password;
            oUser.DomainUrl = DomainUrl;
            oUser = memberobj.UserLogin(oUser);
            return Request.CreateResponse(HttpStatusCode.OK, oUser);
        }


        [HttpPost]
        [Route("UpdateProfile2")]
        public HttpResponseMessage UpdateProfile2(string json, Guid UserGuid, int RID)
        {
            if (!string.IsNullOrEmpty(json) && UserGuid != Guid.Empty && RID > 0)
            {
                string msg = memberobj.UpdateProfile2(json, UserGuid, RID);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                if (RID == 0 && UserGuid == Guid.Empty && string.IsNullOrEmpty(json))
                {
                    objOutput = Status("ResponseJson, User Guid & RID are Required");
                }
                else if (RID == 0 && UserGuid != Guid.Empty && !string.IsNullOrEmpty(json))
                {
                    objOutput = Status("RID is Required");
                }
                else if (RID > 0 && UserGuid == Guid.Empty && !string.IsNullOrEmpty(json))
                {
                    objOutput = Status("UserGuid is Required");
                }
                else if (RID > 0 && UserGuid != Guid.Empty && string.IsNullOrEmpty(json))
                {
                    objOutput = Status("ResponseJson is Required");
                }
                else
                {
                    objOutput = Status("ResponseJson, User Guid & RID is Required");
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }

        #region Model State Errors
        public Output ModelStateErrors(string _methodName)
        {
            string _errorMessage = string.Empty;
            var _errors = new List<string>();

            #region Errors Handling                  
            string messages = string.Join("; ", ModelState.Values
                                     .SelectMany(x => x.Errors)
                                     .Select(x => x.ErrorMessage));
            _errorMessage = messages;
            #endregion

            if (_methodName.ToLower() == "create" || _methodName.ToLower() == "update")
            {
                objOutput = Status(_errorMessage);
            }
            else if (_methodName.ToLower() == "resubscribe" || _methodName.ToLower() == "getredeemHistory" || _methodName.ToLower() == "getrewardshistory" || _methodName.ToLower() == "getsurveyshistory" || _methodName.ToLower() == "getsurveys" || _methodName.ToLower() == "getprofile")
            {
                objOutput = Status("UserGuid is Required");
            }

            return objOutput;
        }
        #endregion

        #region Output Status
        public Output Status(string _message)
        {
            Output obj = new Output();
            if (_message.ToLower().Contains("success"))
            {
                obj.StatusCode = 1;
                obj.ErrorMessage = _message;
                return obj;
            }
            else
            {
                obj.StatusCode = 2;
                obj.ErrorMessage = _message;
                return obj;
            }

        }
        #endregion

        #region Delete User
        [HttpPost]
        [Route("Delete")]
        public HttpResponseMessage Delete(int Rid, string ExtMemberId, Guid UserGuid)
        {
            if (Rid != 0 && UserGuid != Guid.Empty)
            {
                string msg = memberobj.Delete(Rid, ExtMemberId, UserGuid);
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                if (Rid == 0 && UserGuid != Guid.Empty)
                    objOutput = Status("Rid is Required & UserGuid Or ExtMemberId is Required");
                else if (UserGuid != Guid.Empty)
                    objOutput = Status("UserGuid Or ExtMemberId is Required");
                else if (Rid == 0)
                    objOutput = Status("Rid is Required");
                return Request.CreateResponse(HttpStatusCode.BadRequest, objOutput.ErrorMessage);
            }
        }
        #endregion

        [HttpPost]
        [Route("GetQuestions")]
        public HttpResponseMessage GetQuestions(int questionId)
        {
            string message = string.Empty;
            List<Questions> lstquestion = new List<Questions>();
            if (questionId == 0)
            {

                message = "QuestionId is Required";
                return Request.CreateResponse(HttpStatusCode.OK, message);
            }
            else
            {
                if (questionId != 0)
                {
                    lstquestion = memberobj.GetQuestions(questionId);
                    if (lstquestion.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, lstquestion);
                    }
                    else
                    {
                        message = "No Question List was found.";
                        return Request.CreateResponse(HttpStatusCode.OK, message);
                    }
                }
                else
                {
                    message = "QuestionId is Required";
                    return Request.CreateResponse(HttpStatusCode.OK, message);
                }
            }
        }
    }
}
