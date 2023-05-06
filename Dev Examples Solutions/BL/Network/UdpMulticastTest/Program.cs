#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BlCommon;

#endregion

namespace UdpMulticastTest
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            IPAddress listenerIpAddress = IPAddress.Parse("192.168.117.2");
            IPAddress multicastAddr = IPAddress.Parse("224.0.1.0");             //Multicast Ip addresses could be [224.0.1.0] to [239.255.255.255]
            IPEndPoint listenerIpEndPoint = new IPEndPoint(IPAddress.Any, 7002);

            UdpClient multicastListenerA = new UdpClient();
            multicastListenerA.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            multicastListenerA.Client.Bind(listenerIpEndPoint);

            UdpClient multicastListenerB = new UdpClient();
            multicastListenerB.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            multicastListenerB.Client.Bind(listenerIpEndPoint);

            multicastListenerA.JoinMulticastGroup(multicastAddr, listenerIpAddress);
            multicastListenerB.JoinMulticastGroup(multicastAddr, listenerIpAddress);

            Task.Factory.StartNew(() =>
            {
                IPEndPoint remoteEP = null;
                byte[] receivedBytes = multicastListenerA.Receive(ref remoteEP);
                TestUtils.ConsoleLog(ConsoleColor.Green, "multicastListenerA Received {0} from {1}", receivedBytes.ToString(","), remoteEP);
                receivedBytes = multicastListenerA.Receive(ref remoteEP);
                TestUtils.ConsoleLog(ConsoleColor.Green, "multicastListenerA Received {0} from {1}", receivedBytes.ToString(","), remoteEP);
            });
            Task.Factory.StartNew(() =>
            {
                IPEndPoint remoteEP = null;
                byte[] receivedBytes = multicastListenerB.Receive(ref remoteEP);
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "multicastListenerB Received {0} from {1}", receivedBytes.ToString(","), remoteEP);
                receivedBytes = multicastListenerB.Receive(ref remoteEP);
                TestUtils.ConsoleLog(ConsoleColor.Yellow, "multicastListenerB Received {0} from {1}", receivedBytes.ToString(","), remoteEP);
            });

            UdpClient multicastSenderClient = new UdpClient();
            multicastSenderClient.Send(new byte[] { 1, 2, 3, 4 }, 4, new IPEndPoint(multicastAddr, 7002));
            multicastSenderClient.Send(new byte[] { 5, 6, 7, 8 }, 4, new IPEndPoint(multicastAddr, 7002));
        }

        #endregion

        #region Public Methods

        public void Test02()
        {
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new Program();
            TestUtils.WaitForEnter();
        }

        #endregion

        #region Fields

        private readonly IPEndPoint m_multicastRouterOrLoopBackIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.40"), 9000); //LoopBack microsoft driver speceified Ip address
        private readonly IPEndPoint m_multicastEndPoint = new IPEndPoint(IPAddress.Parse("239.190.250.250"), 9000); //Multicast Ip addresses could be [224.0.1.0] to [239.255.255.255]
        private readonly IPEndPoint m_unicastEndPoint = new IPEndPoint(IPAddress.Parse("101.1.156.86"), 1000);

        #endregion
    }
}