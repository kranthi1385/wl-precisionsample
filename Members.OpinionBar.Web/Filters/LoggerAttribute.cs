using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace Members.OpinionBar.Web.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Global Action Filter For Logging
        /// <summary>
        ///  Global Action Filter For Logging
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            string logMessage = string.Empty;
            string formBody = string.Empty;
            string outputMessage = string.Empty;
            base.OnActionExecuted(actionExecutedContext);
            var request = actionExecutedContext.HttpContext.Request;
            var inputStream = request.InputStream;
            //var body = request.InputStream;
            //var encoding = request.ContentEncoding;
            //var reader = new StreamReader(body, encoding);
            //var json = reader.ReadToEnd();
            ////outStream.Position = 0;
            using (var reader = new StreamReader(inputStream))
            {
                var body = reader.ReadToEnd();
                formBody = body;
            }

            JsonResult jsonRes = actionExecutedContext.Result as JsonResult;
            if (jsonRes == null)
            {

            }
            else
            {
                outputMessage = new JavaScriptSerializer().Serialize(jsonRes.Data);
            }

            if (actionExecutedContext.Exception != null)
            {
                //actionExecutedContext.HttpContext.Trace.Write("Exception thrown");
                logMessage = request.RawUrl.ToString() + "|" + request.Url.AbsoluteUri.ToString() + "|" + formBody + "|" + actionExecutedContext.Exception.ToString();
                logger.Fatal(logMessage);
            }
            else
            {
                if (ConfigurationManager.AppSettings["InfoLog"].ToString() == "1")
                {
                    logMessage = request.RawUrl.ToString() + "|" + request.Url.AbsoluteUri.ToString() + "|" + formBody + "|" + outputMessage;
                    logger.Info(logMessage);
                }
            }

        }
        #endregion

        #region URL Filter
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url.AbsoluteUri;
            if (url.Contains("xmlrpc") || url.Contains("wlwmanifest"))
            {
                filterContext.Result = new RedirectResult("/Error403.html");
            }

            base.OnActionExecuting(filterContext);
        }
        #endregion

    }
}