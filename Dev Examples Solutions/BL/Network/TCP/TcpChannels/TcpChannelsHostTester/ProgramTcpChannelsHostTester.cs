#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlCommon;
using TcpChannel;

#endregion

namespace TcpChannelsHostTester
{
    internal class ProgramTcpChannelsHostTester
    {
        #region Constructors

        public ProgramTcpChannelsHostTester()
        {
            IPEndPoint localIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9017);
            Task.Factory.StartNew(() =>
            {
                TcpChannelServer server = new TcpChannelServer(localIpEndPoint);
                server.OnBytesReceived += bytes => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "Received: {0}", bytes.Length); };
                server.OnConnectivityChanged += b => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "Connectivity: {0}", b); };
            });
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new ProgramTcpChannelsHostTester();
            Console.ReadLine();
        }

        #endregion
    }
}