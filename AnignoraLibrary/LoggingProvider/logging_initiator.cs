using System;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace LoggingProvider
{
    public class LoggingInitiator
    {
        /// <param name="logsPath">folder for the rolling log files, if null no rolling files log</param>
        /// <param name="isUseConsole">should write log to console</param>
        public LoggingInitiator(string logsPath, bool isUseConsole = false)
        {
            var patternLayout = new PatternLayout() { ConversionPattern = "$%d|%t|%p|%c|%m%n" };
            patternLayout.ActivateOptions();
            if (logsPath != null)
            {
                //rollingFileAppender
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
            if (isUseConsole)
            {
                //consoleAppender
                var consoleAppender = new ConsoleAppender { Layout = patternLayout };
                consoleAppender.ActivateOptions();
                BasicConfigurator.Configure(consoleAppender);
            }
        }
    }
}





