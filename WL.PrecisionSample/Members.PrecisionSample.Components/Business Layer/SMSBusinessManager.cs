using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Members.PrecisionSample.Components.Business_Layer
{
    public  class SMSBusinessManager
    {
        #region Trumpia Related
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public string GetTrupiaId(string response)
        {
            string result = "";

            //response = "{\"statuscode\":\"1\",\"message\":\"Query Success\",\"contactid\":\"26535242\"}";
            string[] strArray = response.Split(',');
            foreach (string objString in strArray)
            {
                string[] splitByColon = objString.Split(':');
                if (splitByColon[0] == "\"contactid\"")
                {
                    result = splitByColon[1];
                    string[] arr = result.Split('"');
                    result = arr[1];
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public string GetResponseId(string response)
        {
            string result = "";

            //response = "{\"statuscode\":\"1\",\"message\":\"Query Success\",\"contactid\":\"26535242\"}";

            string[] splitByColon = response.Split(':');
            if (splitByColon[0] == "{\"requestID\"")
            {
                result = splitByColon[1];
                string[] arr = result.Split('"');
                result = arr[1];
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string GetTrumpiaIdByParseXml(string xml)
        {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(xml))
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "CONTACTID")
                                {
                                    string strCode = reader.ReadInnerXml();
                                    if (!string.IsNullOrEmpty(strCode))
                                    {
                                        id = strCode.Split('[')[2].Split(']')[0];
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public bool ParseRespone(string response, string requestId)
        {
            bool result = false;
            //response = " {\"statuscode\":\"1\",\"message\":\"Create Keyword Success\"}";
            TrumpiaServiceManager oTrumpiaService = new TrumpiaServiceManager();
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

                    string[] splitByColon = response.Split(':');
                    if (splitByColon[0] == "{\"requestID\"")
                    {
                        result1 = splitByColon[1];
                        string[] arr = result1.Split('"');
                        result1 = arr[1];
                        result = oTrumpiaService.CheckResponse(result1);
                    }

                }
            }
            return result;
        }
        #endregion
    }
}
