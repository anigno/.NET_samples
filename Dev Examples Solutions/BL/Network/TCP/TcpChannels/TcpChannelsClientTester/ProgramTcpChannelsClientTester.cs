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

namespace TcpChannelsClientTester
{
    internal class ProgramTcpChannelsClientTester
    {
        #region Constructors

        public ProgramTcpChannelsClientTester()
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9017);

            Task.Factory.StartNew(() =>
            {
                TcpChannelClient client = new TcpChannelClient(remoteIpEndPoint);
                client.OnBytesSent += bytes => { TestUtils.ConsoleLog(ConsoleColor.Green, "Sent: {0}", bytes.Length); };
                client.OnConnectivityChanged += b => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "Connectivity: {0}", b); };
                Task.Factory.StartNew(async () =>
                {
                    for (int a = 1; a < 1000; a++)
                    {
                        byte[] bytesRange = Enumerable.Range(1, 1000 + a).Select(p => { return (byte) p; }).ToArray();
                        client.Send(bytesRange);
                        await Task.Delay(5000);
                    }
                });
            });
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new ProgramTcpChannelsClientTester();
            Console.ReadLine();
        }

        #endregion
    }
}