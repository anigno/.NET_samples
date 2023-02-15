using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoggingProvider
{
    internal class Runner
    {
        public static void Main()
        {
            // Must run once at app start
            new LoggingInitiator(logsPath: "d:/dev/logs", isUseConsole: true);
            //new LoggingInitiator(null,true); // console only example
            ILog log = LogManager.GetLogger("main_logger");

            Thread t = new Thread(() => { log.Info("hello4"); });
            t.Name = "some thread name";
            t.Start();
            Task.Factory.StartNew(() => { log.Info("hello1"); });
            Task.Factory.StartNew(() => { log.Debug("hello2"); });
            Task.Factory.StartNew(() => { log.Error("hello3"); });

            Console.ReadKey();
            return;

            string s = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            s = s + s + s + s;
            s = s + s + s + s;
            Random rnd = new Random();
            for (int i = 0; i < 11; i++)
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
