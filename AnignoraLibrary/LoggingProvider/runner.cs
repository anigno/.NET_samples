using log4net;
using System;
using System.Threading;

namespace LoggingProvider
{
    internal class runner
    {

        public static void Main()
        {
            new LoggingInitiator("d:/dev/logs"); // Must run once at app start
            ILog log = LogManager.GetLogger("main_logger");

            string s = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            s = s + s + s + s;
            s = s + s + s + s;
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int r = rnd.Next(0, 5);
                switch (r)
                {
                    case 0:
                        log.Debug(s); break;
                    case 1:
                        log.Info(s); break;
                    case 2:
                        log.Warn(s); break;
                    case 3:
                        log.Error(s); break;
                    case 4:
                        log.Fatal(s); break;
                }
                Thread.Sleep(10);
            }
        }
    }
}
