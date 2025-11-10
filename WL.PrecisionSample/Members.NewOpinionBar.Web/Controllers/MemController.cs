using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Members.OpinionBar.Components.Business_Layer;
using Members.OpinionBar.Components.Entities;
using Members.NewOpinionBar.Web.Filters;
using Members.NewOpinionBar.Web.Utlis;
using System.IO;
using System.Net;
using System.Text;


namespace Members.NewOpinionBar.Web.Controllers
{
    public class MemController : Controller
    {
        // GET: Mem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pf()
        {
            return View("/Views/Home/ProQues.cshtml");
        }

        #region Get Profile Questions
        /// <summary>
        /// Get Profile Questions
        /// </summary>
        /// <param name="ug">UserGuid</param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [HttpGet]
        public JsonResult ProfileGet(string pid, Guid ug, int cid, int scid)
        {
            List<ProfileQuestions> lstQuestion = new List<ProfileQuestions>();
            ProfileQuestionBusinessService objQuestionBl = new ProfileQuestionBusinessService();
            var Pagedata = objQuestionBl.GetProfileQuestions(new Guid(pid), ug, cid, scid);
            return Json(Pagedata, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        [ValidateJsonAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult ProfileSave(string xml, Guid ug, int cid, string pfId)
        {
            List<ProfileQuestions> objQuestionSave = new List<ProfileQuestions>();
            ProfileQuestionBusinessService objQuestionBl = new ProfileQuestionBusinessService();
            string profileResponsesXML = string.Empty;
            objQuestionBl.ProfileSave(xml, ug, cid);

            UserManager objManager = new UserManager();
            Client objClient = new Client();
            if (cid != 0)
            {
                objClient = objManager.GetClientDetailsByRid(null, null, cid);

                //fire profile responses pixel to partners.
                if (cid == 11)
                {
                    profileResponsesXML = objQuestionBl.GetProfileResponse(ug, cid, pfId);
                    if (!string.IsNullOrEmpty(profileResponsesXML))
                    {
                        PostRequest(objClient.ProfileCompletePixelUrl, "pr=" + Server.UrlEncode(profileResponsesXML));
                    }
                }
                else if (cid == 28)
                {
                    DateTime dt = DateTime.Now;
                    string s = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    UserEntity objUserEntity = objQuestionBl.GetProfilePixelDetails(ug, cid, pfId);
                    objUserEntity.ProfileClickDate = s;
                    string url = Code.PixelReplacements(objClient.ProfileCompletePixelUrl, objUserEntity, "A5E5D6E0D10811E18A960DAD6188709B");
                    GettRequest(url);
                }
            }
            string data = objClient.HomePageURL + ';' + objClient.IsPopUp;
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        #region Post Request
        /// <summary>
        /// Post Profile Responses to partners
        /// </summary>
        /// <param name="RequestURL">Request Url</param>
        /// <param name="Text">Text</param>
        /// <returns></returns>
        public string PostRequest(string RequestURL, string Text)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = Text;
                byte[] data = encoding.GetBytes(postData);

                HttpWebRequest LoginRequest = (HttpWebRequest)WebRequest.Create(RequestURL);
                LoginRequest.Method = "POST";
                //NetworkCredential nc = new NetworkCredential("sumank", "123456");
                LoginRequest.ContentType = "application/x-www-form-urlencoded";
                LoginRequest.ContentLength = data.Length;
                Stream LoginRequestStream = LoginRequest.GetRequestStream();
                LoginRequestStream.Write(data, 0, data.Length);
                LoginRequestStream.Close();

                HttpWebResponse LoginResponse = (HttpWebResponse)LoginRequest.GetResponse();
                string strNewResponseUrl = LoginResponse.ResponseUri.ToString();
                StreamReader sr = new StreamReader(LoginResponse.GetResponseStream());
                String strData = sr.ReadToEnd();
                sr.Close();
                return strData;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Http Get Request
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RequestURL"></param>
        /// <returns></returns>
        public string GettRequest(string RequestURL)
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
    }
}