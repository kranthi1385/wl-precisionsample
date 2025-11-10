//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Helpers;
//using System.Web.Mvc;

//namespace Members.PrecisionSample.Web
//{

//    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//    //public class CustomValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
//    //{
//    //    private void ValidateRequestHeader(HttpRequestBase request)
//    //    {
//    //        string cookieToken = String.Empty;
//    //        string formToken = String.Empty;
//    //        string tokenValue = request.Headers["RequestVerificationToken"];
//    //        if (!String.IsNullOrEmpty(tokenValue))
//    //        {
//    //            string[] tokens = tokenValue.Split(':');
//    //            if (tokens.Length == 2)
//    //            {
//    //                cookieToken = tokens[0].Trim();
//    //                formToken = tokens[1].Trim();
//    //            }
//    //        }
//    //        AntiForgery.Validate(cookieToken, formToken);
//    //    }

//    //    public void OnAuthorization(AuthorizationContext filterContext)
//    //    {

//    //        try
//    //        {
//    //            if (filterContext.HttpContext.Request.IsAjaxRequest())
//    //            {
//    //                ValidateRequestHeader(filterContext.HttpContext.Request);
//    //            }
//    //            else
//    //            {
//    //                AntiForgery.Validate();
//    //            }
//    //        }
//    //        catch (HttpAntiForgeryException e)
//    //        {
//    //            throw new HttpAntiForgeryException("Anti forgery token cookie not found");
//    //        }
//    //    }
//    //}
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//    public class ValidateJsonAntiForgeryToken : AuthorizeAttribute
//    {
//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            var request = filterContext.HttpContext.Request;

//            if (request.HttpMethod == WebRequestMethods.Http.Post)
//            {
//                if (request.IsAjaxRequest())
//                    AntiForgery.Validate(CookieValue(request), request.Headers["__RequestVerificationToken"]);
//                else
//                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
//            }
//        }

//        private string CookieValue(HttpRequestBase request)
//        {
//            var cookie = request.Cookies[AntiForgeryConfig.CookieName];
//            return cookie != null ? cookie.Value : null;
//        }
//    }
//}