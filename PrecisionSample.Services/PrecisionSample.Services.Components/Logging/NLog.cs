using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
namespace PrecisionSample.Services.Components.Logging
{
    public static class NLog
    {
        public static readonly Logger ClassLogger = LogManager.GetCurrentClassLogger();
    }
}