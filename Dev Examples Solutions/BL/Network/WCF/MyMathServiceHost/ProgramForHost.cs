#region

using System;
using System.Configuration;
using System.ServiceModel;
using BlCommon;

#endregion

namespace MyMathServiceHost
{
    internal class ProgramForHost
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            ServiceHost serviceHost;
            try
            {
                serviceHost = new ServiceHost(typeof (MyMathServiceLib.MyMathService));
                serviceHost.Open();
                TestUtils.ConsoleLog("Service is Running at following address");
                TestUtils.ConsoleLog("http://localhost:9001/MyMathService");
                TestUtils.ConsoleLog("net.tcp://localhost:9002/MyMathService");
            }
            catch (Exception ex)
            {
                TestUtils.ConsoleLog(ex);
                return;
            }
            TestUtils.WaitForEnter();
            serviceHost.Close();
        }

        #endregion
    }
}