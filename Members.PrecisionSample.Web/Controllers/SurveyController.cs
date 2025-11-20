using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.PrecisionSample.Components.Entities;
using Members.PrecisionSample.Components.Business_Layer;
using Members.PrecisionSample.Web.Filters;
using NLog;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Newtonsoft.Json;
using Amazon;
using Amazon.Runtime;

namespace Members.PrecisionSample.Web.Controllers
{
    public class SurveyController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        // GET: en
        public ActionResult entry()
        {
            return View("/Views/pages/entry.cshtml");
        }
        SurveyEntry oManager = new SurveyEntry();

        #region Save Entry
        [ValidateJsonAntiForgeryToken]
        public SEntry saveEntry(string uig, int pid, Decimal? cost, int? FedProjectId)
        {
            return oManager.saveEntry(uig, pid, Convert.ToDecimal(cost), Convert.ToInt32(FedProjectId));
        }
        #endregion

        public void fedentry(string mid, Guid eid, int? project, decimal? ecost, string hash)
        {
            string surveyURL = string.Empty;
            int hashValue = 0;
            string hashKey = ConfigurationManager.AppSettings["FedHashKey"].ToString();
            //Writing Log file Trace.
            logger.Trace("fed_entry|" + Request.Url.AbsoluteUri.ToString());
            if (eid != Guid.Empty && eid != null && project != null && project > 0)
            {
                if (!string.IsNullOrEmpty(hash))
                {
                    surveyURL = $"{Request.Url.AbsoluteUri.ToString()}&";
                    string value = HMACSha1hash(surveyURL, hashKey);
                    if (value != hash)
                    {
                        hashValue = 1;
                    }
                }
                string url = oManager.GetFedEntryUrl(mid, eid, project, Convert.ToDecimal(ecost));
                url = $"{url}&hash={hashValue}";
                //Writing Log file Info.
                logger.Info("fed_entry|" + Request.Url.AbsoluteUri.ToString() + "|routed:" + url);
                Response.Redirect(url);
            }
        }

        public JsonResult RedirectAPI(Guid ug, int project, Guid key, string subid)
        {

            List<Question> lstQst = new List<Question>();
            lstQst = oManager.GetQuestion(ug, project);
            //string IpAddress = Request.UserHostAddress;
            //return oManager.GetRedirectUrl(ug, project, key, subid, IpAddress);
            if (lstQst[0].QuestionId == -1)
            {
                //string IpAddress = Request.UserHostAddress;
                //string url = oManager.GetRedirectUrl(ug, project, key, subid, IpAddress);
                //lstQst[0].ProjectURL = url;
                lstQst = GetSelectedTarget(lstQst, ug, project, key, subid);
            }
            return Json(lstQst, JsonRequestBehavior.AllowGet);
        }

        #region Save PII Data
        public JsonResult SavePiiData(Guid ug, int project, Guid key, string subid, int qid, string otext, int optId, int cid, string zip)
        {
            List<Question> lstQst = new List<Question>();
            lstQst = oManager.SaveResponse(qid, otext, optId, ug, cid, zip,project);
            if (lstQst[0].QuestionId == -1)
            {
                lstQst = GetSelectedTarget(lstQst, ug, project, key, subid);
            }
            return Json(lstQst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Setected target
        public List<Question> GetSelectedTarget(List<Question> lstQst, Guid ug, int project, Guid key, string subid)
        {
            string projectURL = string.Empty;
            string AWSAccesskey = ConfigurationManager.AppSettings["AWSAccesskey"].ToString();
            string AWSSecretkey = ConfigurationManager.AppSettings["AWSSecretkey"].ToString();
            BasicAWSCredentials _credentials = new BasicAWSCredentials(AWSAccesskey, AWSSecretkey);
            MatchedTargets objMatchedTargets = new MatchedTargets();
            objMatchedTargets.UserGUID = ug.ToString();
            objMatchedTargets.OrgGUID = key.ToString();
            objMatchedTargets.ProjectID = project;
            var payload = JsonConvert.SerializeObject(objMatchedTargets);
            string functionArn = ConfigurationManager.AppSettings["AWSARN"].ToString(); // Replace with your function's ARN
            var lambdaClient = new AmazonLambdaClient(_credentials, RegionEndpoint.USEast1); // Uses default credentials
            var request = new InvokeRequest
            {
                FunctionName = functionArn, // Use ARN instead of function name
                InvocationType = InvocationType.RequestResponse, // Synchronous execution
                Payload = payload // JSON input payload
            };
            var response = lambdaClient.Invoke(request);
            // Read response payload
            string responsePayload = Encoding.UTF8.GetString(response.Payload.ToArray());
            projectURL = responsePayload.Trim('"');
            projectURL = $"{projectURL}&sub_id={subid}";
            lstQst[0].ProjectURL = projectURL;
            return lstQst;
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

    }
}