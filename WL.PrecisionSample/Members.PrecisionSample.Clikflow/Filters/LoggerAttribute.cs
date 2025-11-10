using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Members.PrecisionSample.Clikflow.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger clickstarterrorlog = LogManager.GetCurrentClassLogger();
        private static Logger piicollectionerrorlog = LogManager.GetCurrentClassLogger();
        private static Logger verityerrorlog = LogManager.GetCurrentClassLogger();
        #region Global Action Filter For Logging
        /// <summary>
        ///  Global Action Filter For Logging
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            var descriptor = actionExecutedContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;

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
                //logger.Fatal(logMessage);
                if (controllerName.ToLower() == "cr")
                {
                    clickstarterrorlog.Error(logMessage);
                }
                else if (controllerName.ToLower() == "reg")
                {
                    piicollectionerrorlog.Error(logMessage);
                }
                else if (controllerName.ToLower() == "cv")
                {
                    piicollectionerrorlog.Error(logMessage);
                }
                else
                {
                    logger.Fatal(logMessage);
                }

            }
            //else
            //{
            //    logMessage = request.RawUrl.ToString() + "|" + request.Url.AbsoluteUri.ToString() + "|" + formBody + "|" + outputMessage;
            //    logger.Info(logMessage);
            //}

        }
        #endregion

    }
}