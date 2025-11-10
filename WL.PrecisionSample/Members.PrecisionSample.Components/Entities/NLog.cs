using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
namespace Members.PrecisionSample.Components.Entities
{
    public class NLog
    {
        public static readonly Logger ClassLogger = LogManager.GetCurrentClassLogger();
    }
}
