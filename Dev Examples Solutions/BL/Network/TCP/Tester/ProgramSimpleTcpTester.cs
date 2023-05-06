#region

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlCommon;
using SimpleTcpCommunication;
using SimpleTcpCommunication.TcpCommunicator;

#endregion

namespace Tester
{
    internal class ProgramSimpleTcpTester
    {
        #region Constructors

        public ProgramSimpleTcpTester()
        {
            byte[] bytesRange = Enumerable.Range(1, 2000000).Select(p => { return (byte)p; }).ToArray();

            Task.Factory.StartNew(() =>
            {
                m_server.OnProgress += p_f => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "Progress: " + p_f); };
                m_server.OnConnectivityChanged += p_b => { TestUtils.ConsoleLog(ConsoleColor.Yellow, "Connectivity changed: " + p_b); };
                m_server.OnBytesReceived += p_bytes =>
                {
                    TestUtils.ConsoleLog(ConsoleColor.Green, "Received: " + p_bytes.Length);
                    for (int i = 0; i < p_bytes.Length; i++)
                    {
                        if (p_bytes[i] != bytesRange[i]) throw new Exception("Data missmatch");
                    }
                };
                m_server.Listen(localIpEndPoint);
            });

            Task.Factory.StartNew(() =>
            {
                m_client.OnConnectivityChanged += p_b => { TestUtils.ConsoleLog(ConsoleColor.Green, "Connectivity changed: " + p_b); };
                m_client.Connect(localIpEndPoint);
                m_client.SendBytes(bytesRange);
                m_client.SendBytes(bytesRange);
                m_client.SendBytes(bytesRange);
                //m_server.Close();
                //m_client.Close();
            });
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new ProgramSimpleTcpTester();
            TestUtils.WaitForEnter();
        }

        #endregion

        #region Fields

        private IPEndPoint localIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1000);
        private SimpleTcpCommunicator m_server = new SimpleTcpCommunicator();
        private SimpleTcpCommunicator m_client = new SimpleTcpCommunicator();

        #endregion
    }
}