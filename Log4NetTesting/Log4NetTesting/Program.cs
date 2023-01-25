using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using log4net.Config;

namespace Log4NetTesting
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            InitLogger();
            int a = 0;
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);
                    var message = $"Logging {a++}";
                    Logger.Debug(message);
                    Console.WriteLine(message);
                }
            });
        }

        #endregion

        #region Private Methods

        private void InitLogger()
        {
            XmlElement element = (XmlElement)ConfigurationManager.GetSection("log4net");
            XmlConfigurator.Configure(element);
            Logger.Info("Logger initialized");
        }

        private static void Main(string[] p_args)
        {
            Program p = new Program();
            Console.ReadLine();
            GC.KeepAlive(p);
        }

        #endregion

        #region Fields

        public static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}