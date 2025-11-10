using NLog;
using NLog.AWS.Logger;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Members.PrecisionSample.Clikflow.Logging
{
    public static class Nlog
    {
        static Nlog()
        {
            Configure();
        }

        public static void Configure()
        {
            var config = new LoggingConfiguration();
            string accessKey = "AKIARPVSAWTLGAJKRA4P";
            string secretKey = "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";
            var awsTarget_Info = new AWSTarget()
            {
                Name = "API 4",
                LogGroup = "/api4-info",
                Region = "us-east-1",
                LogStreamNamePrefix = "API_4 Info",
                Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey),

            };

            var awsTarget_Error = new AWSTarget()
            {
                Name = "API 4",
                LogGroup = "/api4-error",
                Region = "us-east-1",
                LogStreamNamePrefix = "API_4 Error",
                Credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey),
            };

            config.AddTarget("aws_info", awsTarget_Info);
            config.AddTarget("aws_error", awsTarget_Error);

            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, awsTarget_Info));
            config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Error, awsTarget_Error));
            LogManager.Configuration = config;
        }

        public static readonly Logger ClassLogger_AWS = LogManager.GetCurrentClassLogger();
    }
}