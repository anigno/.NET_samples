using log4net;
using log4net.Appender;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoggingProvider
{
    internal class runner
    {

        public static void Main()
        {
            LoggingInitiator logIni = new LoggingInitiator("d:/dev/logs");
            ILog log = LogManager.GetLogger("main_logger");
            for (int i = 0; i < 10000; i++)
            {
                string s = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
                s = s + s + s + s;
                s = s + s + s + s;
                log.Info(s);
                Thread.Sleep(10);
            }
        }
    }
}
