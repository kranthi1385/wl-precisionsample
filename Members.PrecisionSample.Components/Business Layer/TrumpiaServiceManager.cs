using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using System.Xml;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public class TrumpiaServiceManager
    {
        public string MakeTinyUrl(string surveyurl)
        {
            try
            {
                if (surveyurl.Length <= 30)
                {
                    return surveyurl;
                }
                if (!surveyurl.ToLower().StartsWith("http") && !surveyurl.ToLower().StartsWith("ftp"))
                {
                    surveyurl = "http://" + surveyurl;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + surveyurl);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return surveyurl;
            }
        }

        public string CreateList(string listName)
        {
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://trumpia.com/api/createlist.php";
            param = "APIKEY=" + ConfigurationManager.AppSettings["TrumpiaKey"] + "&ListName=" + listName;
            return PostRequest(requestURL, param);
        }

        public string AddContact(string fName, string lName, string mobileNumber, string listName, string apiKey)
        {
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://api.precisemobile.co/http/v2/addcontact";
            param = "apikey=" + apiKey + "&first_name=" + fName + "&last_name=" + lName + "&mobile_number=" + mobileNumber + "&list_name=" + listName + "&send_verification=FALSE";
            string res = PostRequest(requestURL, param);
            string resultFinal = string.Empty;
            if (ParseRespone(res, apiKey))
                resultFinal = GetContactId(apiKey, fName, lName, mobileNumber, listName);
            return resultFinal;
        }
        public string GetContactId(string apiKey, string fName, string lName, string mobileNumber, string listName)
        {
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://api.precisemobile.co/http/v2/getcontactid";
            param = "apikey=" + apiKey + "&list_name=" + listName + "&tool_type=2" + "&tool_data=" + mobileNumber;
            string result = PostRequest(requestURL, param);
            if (ParseRespone(result, apiKey))
            {
                return result;
            }
            else
            {
                return AddContact(fName, lName, mobileNumber, listName, apiKey);
            }
        }

        public string SendToList(string description, string textBody, string orgName, string listName)
        {
            description = string.IsNullOrEmpty(description) ? "Test" : description;
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://trumpia.com/api/sendtolist.php";
            param = "APIKEY=" + ConfigurationManager.AppSettings["TrumpiaKey"] + "&EmailMode=False&IMMode=False&SMSMode=TRUE&SBMode=False" + "&Description=" + description + "&SMSMessage=" + textBody + "&ChangeOrganizationName" + orgName + "&ListNames=" + listName + "&SendLater=False";
            return PostRequest(requestURL, param);
        }
        public string SendToContact(string description, string textBody, string listName, string contactId, string apiKey, string clientDisplayName)
        {
            description = string.IsNullOrEmpty(description) ? "Test" : description;
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://api.precisemobile.co/http/v2/sendtocontact";
            if (!string.IsNullOrEmpty(clientDisplayName))
                param = "apikey=" + apiKey + "&email_mode=FALSE&im_mode=FALSE&sms_mode=TRUE&sb_mode=FALSE&description=test&sms_message=" + textBody + "&contact_ids=" + contactId + "&change_org_name=" + clientDisplayName;
            else
                param = "apikey=" + apiKey + "&email_mode=FALSE&im_mode=FALSE&sms_mode=TRUE&sb_mode=FALSE&description=test&sms_message=" + textBody + "&contact_ids=" + contactId;
            return PostRequest(requestURL, param);
        }
        public bool ParseXml(string xml)
        {
            string message = string.Empty;
            if (!string.IsNullOrEmpty(xml))
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "STATUSCODE")
                                {
                                    string strCode = reader.ReadInnerXml();
                                    if (!string.IsNullOrEmpty(strCode))
                                    {
                                        if (strCode != "1")
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            return true;
                                        }

                                    }
                                }

                                break;
                        }
                    }
                }
            }
            return false;
        }
        public bool ParseRespone(string response, string requestId)
        {
            bool result = false;
            //response = " {\"statuscode\":\"1\",\"message\":\"Create Keyword Success\"}";
            if (!string.IsNullOrEmpty(response))
            {
                if (response.Contains("statuscode"))
                {
                    if (response.Contains("1") || response.Contains("Progress"))
                    {
                        result = true;

                    }
                    else if (response.Contains("0"))
                    {
                        result = false;
                    }
                }
                else if (response.Contains("Progress"))
                {
                    result = true;
                }
                else if (response.Contains("requestID"))
                {
                    string result1 = "";

                    //response = "{\"statuscode\":\"1\",\"message\":\"Query Success\",\"contactid\":\"26535242\"}";

                    string[] splitByColon = response.Split(':');
                    if (splitByColon[0] == "{\"requestID\"")
                    {
                        result1 = splitByColon[1];
                        string[] arr = result1.Split('"');
                        result1 = arr[1];
                        result = CheckResponse(result1);
                    }

                }
                //else if (response.Contains("In Progress"))
                //{
                //    CheckResponse(requestId);
                //}
            }
            return result;
        }
        public bool CheckResponse(string requestId)
        {
            string requestURL = string.Empty;
            string param = string.Empty;
            requestURL = "http://api.precisemobile.co/http/v2/checkresponse";
            param = "request_id=" + requestId;
            string result = PostRequest(requestURL, param);
            return ParseRespone(result, requestId);
        }
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
    }
}
