using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Members.OpinionBar.Components.Entities
{
    public class RecaptchaClient
    {
        #region Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region CloudFlareCaptchaValidate
        public static bool CloudFlareCaptchaValidate(string token)
        {
            HttpClient client = new HttpClient();
            CloudFlareResponse objInfo = new CloudFlareResponse();
            try
            {
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                dicData["secret"] = "0x4AAAAAAAdP1k2RGUA7_eUc7NwuiL4LsjA";
                dicData["response"] = token;
                FormUrlEncodedContent mfd = new FormUrlEncodedContent(dicData);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://challenges.cloudflare.com/turnstile/v0/siteverify");
                request.Content = mfd;
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var responseString = response.Content.ReadAsStringAsync().Result;
                objInfo = JsonConvert.DeserializeObject<CloudFlareResponse>(responseString);
                if (!objInfo.success)
                {
                    var errorResponse = string.Join(",", objInfo.errorcodes).ToString();  
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("CloudFlare API Error in Forgot Password Method:" + ex.StackTrace.ToString());
            }
            return objInfo.success;
        }
        #endregion
    }

    public class CloudFlareResponse : GeneralCaptchaResponse
    {
        public string hostname { get; set; }
        public string action { get; set; }
        public string cdata { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public bool interactive { get; set; }
    }

    public class GeneralCaptchaResponse
    {
        public bool success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> errorcodes { get; set; }
        public DateTime challenge_ts { get; set; }

    }
}
