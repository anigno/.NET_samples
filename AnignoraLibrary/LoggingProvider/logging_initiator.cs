using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using LoggingInitiator;

namespace LoggingProvider
{
    public class LoggingInitiator
    {
        public LoggingInitiator(string logsPath)
        {
            var patternLayout = new PatternLayout() { ConversionPattern = "$%d|%t|%p|%c|%m%n" };
            patternLayout.ActivateOptions();
            var fileName = string.Format($"{logsPath}/log/log.txt_{DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}");

            var rollingFileAppender = new RollingFileAppenderStartWithOne
            {
                AppendToFile = true,
                Layout = patternLayout,
                RollingStyle = RollingFileAppenderStartWithOne.RollingMode.Size,
                StaticLogFileName = false,
                CountDirection = 1,
                File = fileName,
                MaxSizeRollBackups = 1000,
                MaximumFileSize = "2MB"
            };
            rollingFileAppender.ActivateOptions();
            BasicConfigurator.Configure(rollingFileAppender);
        }
    }
}





