using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Members.PrecisionSample.Components.Entities
{
    public enum CaptchaProvider
    {
        Cloudflare = 1,
        hCaptcha = 2,
        GoogleRecaptcha = 3
    }
    public class RecaptchaClientModel
    {
       
        public int CaptchaClient { get; set; }
        public string ClientSiteKey { get; set; }
        public string ClientToken { get; set; } = string.Empty;
        public string ug { get; set; }

        public bool GetByPass()
        {
            var bypass = ConfigurationManager.AppSettings["ByPassCaptchaTimeout"];
            return (Convert.ToInt16(bypass) == 1);
        }

        public string GetCaptchaSiteKey()
        {
            switch ((CaptchaProvider)CaptchaClient)
            {
                case CaptchaProvider.Cloudflare:
                    return ConfigurationManager.AppSettings["CloudFlareSiteKey"];
                case CaptchaProvider.hCaptcha:
                    return ConfigurationManager.AppSettings["HCaptchaSiteKey"];
                default:
                    return ConfigurationManager.AppSettings["GoogleSiteKey"];
            }
        }
        public string GetCaptchaSecretKey()
        {
            switch ((CaptchaProvider)CaptchaClient)
            {
                case CaptchaProvider.Cloudflare:
                    return ConfigurationManager.AppSettings["CloudFlareSecretKey"];
                case CaptchaProvider.hCaptcha:
                    return ConfigurationManager.AppSettings["HCaptchaSecretKey"];
                default:
                    //return ConfigurationManager.AppSettings["GoogleSecretKey"];
                    return ConfigurationManager.AppSettings["Re-CapthaSecretKey"];
            }
        }

        public string GetCaptchaUrl()
        {
            switch ((CaptchaProvider)CaptchaClient)
            {
                case CaptchaProvider.Cloudflare:
                    return ConfigurationManager.AppSettings["CloudFlareUrl"];
                case CaptchaProvider.hCaptcha:
                    return ConfigurationManager.AppSettings["HCaptchaUrl"];
                default:
                    return ConfigurationManager.AppSettings["GoogleCaptchaUrl"];
            }
        }

        public bool CloudFlareCaptchaValidate(HttpClient client)
        {
            CloudFlareResponse objInfo = new CloudFlareResponse();
            try
            {
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                dicData["secret"] = this.GetCaptchaSecretKey();
                dicData["response"] = this.ClientToken;
                FormUrlEncodedContent mfd = new FormUrlEncodedContent(dicData);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.GetCaptchaUrl());
                request.Content = mfd;
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;               
                objInfo = JsonConvert.DeserializeObject<CloudFlareResponse>(responseString);
                if (!objInfo.success)
                {
                    var errorResponse = string.Join(",", objInfo.errorcodes).ToString();
                    if (errorResponse.Contains("timeout-or-duplicate") && GetByPass())
                    {
                        NLog.ClassLogger.Error("CloudFlare Timeout" + "| ug:" + this.ug);
                        return true;
                    }
                    ExceptionNotify.SendEmail("|Date :" + DateTime.Now + " |CloudFlare Error Code: " + errorResponse + "| UserGuid "+this.ug+"| Token:"+this.ClientToken +"|");
                    return false;
                }
            }
            catch (Exception ex)
            {
                NLog.ClassLogger.Error("CloudFlare API Error:" + ex.StackTrace.ToString() + "| ug:" + this.ug);
            }
            return objInfo.success;
        }

        public bool HCaptchaValidate(HttpClient client)
        {
            HCaptchaResponse objInfo = new HCaptchaResponse();
            try
            {
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                dicData["secret"] = this.GetCaptchaSecretKey();
                dicData["response"] = this.ClientToken;
                FormUrlEncodedContent formData = new FormUrlEncodedContent(dicData);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,this.GetCaptchaUrl());
                request.Content = formData;
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                objInfo = JsonConvert.DeserializeObject<HCaptchaResponse>(responseString);
                if (!objInfo.success)
                {
                    var errorResponse = string.Join(",", objInfo.errorcodes).ToString();
                    if (errorResponse.Contains("invalid-or-already-seen-response") && GetByPass())
                    {
                        NLog.ClassLogger.Error("HCaptcha Timeout" + "| ug:" + this.ug);
                        return true;
                    }
                    ExceptionNotify.SendEmail("|Date :" + DateTime.Now + " |HCaptcha Error Code: " + errorResponse + "| ug:" + this.ug + "|Response: " + responseString + "| Token:" + this.ClientToken);
                    return false;
                }
            }
            catch (Exception ex)
            {
                NLog.ClassLogger.Error("HCaptcha API Error:" + ex.StackTrace.ToString() + "| ug:" + this.ug);
            }
            return objInfo.success;
        }

        public bool GoogleCaptchaValidate(HttpClient client)
        {
            GoogleRecaptcha objInfo = new GoogleRecaptcha();
            try
            {
                var parameter = "?secret=" + GetCaptchaSecretKey() + "&response=" + this.ClientToken;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, GetCaptchaUrl()+ parameter);
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                objInfo = JsonConvert.DeserializeObject<GoogleRecaptcha>(responseString);
                if (!objInfo.success)
                {
                    var errorResponse = string.Join(",", objInfo.errorcodes).ToString();
                    if (errorResponse.Contains("timeout-or-duplicate") && GetByPass())
                    {
                        NLog.ClassLogger.Error("GoogleCaptcha Timeout" + "| ug:" + this.ug);
                        return true;
                    }
                    ExceptionNotify.SendEmail("|Date :" + DateTime.Now + " |Google Captcha Error Code: " + errorResponse + "| ug:" + this.ug + "|Response: " + responseString + "| Token:" + this.ClientToken);
                    return objInfo.success;
                }
            }
            catch (Exception ex)
            {
                NLog.ClassLogger.Error("Google Captcha API Error:" + ex.StackTrace.ToString() + "| ug:" + this.ug);
            }
            return objInfo.success;

        }

        public bool ValidateCaptcha(CaptchaProvider provider, HttpClient client)
        {
            string url = this.GetCaptchaUrl();
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "secret", GetCaptchaSecretKey() },
                { "response", ClientToken }
            };

            if (provider == CaptchaProvider.GoogleRecaptcha)
            {
                url += "?" + string.Join("&", parameters.Select(kv => $"{kv.Key}={kv.Value}")); // Build query string
            }

            var requestContent = provider != CaptchaProvider.GoogleRecaptcha
                                ? new FormUrlEncodedContent(parameters): null; // No content for Google reCAPTCHA

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = requestContent;
            try
            {
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic objInfo = JsonConvert.DeserializeObject(responseString);
                if (!((bool)objInfo.success))
                {
                    ExceptionNotify.SendEmail($"{provider} Error Code: {string.Join(",", objInfo.errorcodes)}| UserGuid {ug}| Token:{ClientToken}|");
                    return false;
                }
                if (provider == CaptchaProvider.GoogleRecaptcha)
                {
                    return ((double)objInfo.score) >= 0.5 && ((bool)objInfo.success);
                }
            }
            catch (Exception ex)
            {
                ExceptionNotify.SendEmail($"{provider} API Error: {ex.StackTrace}| ug:{ug}");
            }

            return true;
        }

        public bool GetResponse()
        {
            using (var client = new HttpClient())
            {
                switch ((CaptchaProvider)CaptchaClient)
                {
                    case CaptchaProvider.Cloudflare:
                        return CloudFlareCaptchaValidate(client);
                    case CaptchaProvider.hCaptcha:
                        return HCaptchaValidate(client);
                    default:
                        return GoogleCaptchaValidate(client);
                }
            }
        }
     
    }

    public class ExceptionNotify
    {
        public static void SendEmail(string ExceptionMsg)
        {
            var subject = "Captcha Exception" + " from EndLinks";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            string defaultToEmail = ConfigurationManager.AppSettings["ExceptionEmailAddress"].ToString();
            String HOST = ConfigurationManager.AppSettings["HOST"].ToString();
            const int PORT = 587;
            string SMTP_USERNAME = ConfigurationManager.AppSettings["SMTP_USERNAME"].ToString();
            string SMTP_PASSWORD = ConfigurationManager.AppSettings["SMTP_PASSWORD"].ToString();
            string[] toEmailAddress = defaultToEmail.Split(';');
            MailMessage message = new MailMessage();
            string DisplayName = ConfigurationManager.AppSettings["DisplayName"].ToString();
            message.IsBodyHtml = false;
            message.From = new MailAddress("helpdesk@precisionsample.com",DisplayName);
            for (var i = 0; i < toEmailAddress.Length; i++)
            {
                if (toEmailAddress[i] != "")
                {
                    message.To.Add(new MailAddress(toEmailAddress[i]));
                }
            }
            message.Subject = subject;
            message.Body = ExceptionMsg.Replace("\n\n", "<br />"); 
            SmtpClient client = new SmtpClient(HOST, PORT);
            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;
            try
            {
                
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    public class GoogleRecaptcha : GeneralCaptchaResponse
    {
        public string hostname { get; set; }
        public double score { get; set; }
        public string action { get; set; }
    }

    public class GeneralCaptchaResponse
    {
        public bool success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> errorcodes { get; set; }
        public DateTime challenge_ts { get; set; }

    }

    public class HCaptchaResponse : GeneralCaptchaResponse
    {
        public string hostname { get; set; }
        public bool credit { get; set; }
    }

    public class Metadata
    {
        public bool interactive { get; set; }
    }

    public class CloudFlareResponse : GeneralCaptchaResponse
    {

        public string hostname { get; set; }
        public string action { get; set; }
        public string cdata { get; set; }
        public Metadata metadata { get; set; }
    }


}
