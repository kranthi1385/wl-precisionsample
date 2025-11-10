using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;

namespace Members.OpinionBar.Web.Filters
{
    public static class Nlog
    {
        private static AmazonCloudWatchLogsClient _client;
        private static LogInfo _info;
        private static BasicAWSCredentials _credentials;
        private static bool infoLogGroupCreated = false;
        private static string logStreamInfo;

        // Task running status tracking
        private static bool _isTaskRunning = false;
        private static DateTime _serviceStartDate;
        private static DateTime _serviceEndDate;

        // Constructor - Async initialization
        public static void Initialize(LogInfo info)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _credentials = new BasicAWSCredentials("AKIARPVSAWTLB3F4ZUFM", "t6qcoJxhVJRMLYDeAB0m8v0AO1nazPSTiMBCELIF");
            _client = new AmazonCloudWatchLogsClient(_credentials, RegionEndpoint.USEast1);
            _info = info;
            if (!infoLogGroupCreated)
            {
                bool response = checkLogGroupExistance(info.InfoLogGroup);
                infoLogGroupCreated = response;
            }
            CreateInfoLogStream(info);
        }

        public static bool CreateLogGroup(string logGroupName)
        {
            CreateLogGroupResponse response = _client.CreateLogGroupAsync(new CreateLogGroupRequest(logGroupName)).Result;
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public static bool checkLogGroupExistance(string infoLogGroupName)
        {
            bool isInfoCreated = CheckLogGroupExists(infoLogGroupName);
            if (!isInfoCreated)
            {
                isInfoCreated = CreateLogGroup(infoLogGroupName);
            }
            return isInfoCreated;
        }

        public static bool CheckLogGroupExists(string logGroupName)
        {
            string nextToken = null;
            do
            {
                var request = new DescribeLogGroupsRequest
                {
                    LogGroupNamePrefix = logGroupName,
                    NextToken = nextToken
                };
                var response = _client.DescribeLogGroupsAsync(request).Result;
                if (response.LogGroups.Exists(lg => lg.LogGroupName == logGroupName))
                {
                    return true;
                }
                nextToken = response.NextToken;
            } while (nextToken != null);
            return false;
        }

        //// Create Info Log Stream
        public static void CreateInfoLogStream(LogInfo info)
        {
            logStreamInfo = $"{DateTime.Now}".Replace(" ", "T").Replace(":", ".") + $" - {info.InfoLogStream}";
            CreateLogStreamRequest infoRequest = new CreateLogStreamRequest(info.InfoLogGroup, logStreamInfo);
            var result = _client.CreateLogStreamAsync(infoRequest).Result;
        }

        // Static Logger Creation
        public static void CreateLogger(string InfoLogStreamName)
        {
            LogInfo logInfo = new LogInfo() { InfoLogGroup = "OpinionBarPayswellLog", InfoLogStream = InfoLogStreamName };
            Nlog.Initialize(logInfo);
        }

        // Info Logging
        public static void Info(object events)
        {
            PutLogEventsRequest input = new PutLogEventsRequest()
            {
                LogEvents = new List<InputLogEvent>
             {
                 new InputLogEvent { Message = events.ToString(), Timestamp = DateTime.Now}
             },
                LogGroupName = _info.InfoLogGroup,
                LogStreamName = logStreamInfo
            };
            var response = _client.PutLogEventsAsync(input).Result;
        }
    }

    public class LogInfo
    {
        public string InfoLogGroup { get; set; }
        public string InfoLogStream { get; set; }
    }

}