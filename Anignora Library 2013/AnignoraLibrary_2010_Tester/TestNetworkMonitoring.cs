using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AnignoraCommunication.Monitoring;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class TestNetworkMonitoring
    {
        private readonly ManualResetEvent m_event=new ManualResetEvent(false);
        private int m_counter = 0;

        [Test]
        public void TestMonitoring()
        {
            NetworkTrafficMonitor trafficMonitor=new NetworkTrafficMonitor();
            string[] adaptersNames = trafficMonitor.NetworkAdaptersNames;
            foreach (string name in adaptersNames)
            {
                Console.WriteLine(name);
            }
            trafficMonitor.NetworkAdapterName = trafficMonitor.NetworkAdaptersNames[2];
            trafficMonitor.MonitorInterval = 1000;
            trafficMonitor.NetworkMonitored += monitor_NetworkMonitored;
            trafficMonitor.Start();
            m_event.Reset();
            m_event.WaitOne();

        }

        void monitor_NetworkMonitored(object sender, NetworkMonitorEventArgs e)
        {
            Console.WriteLine("Adapter: {0} Received: {1}b Sent: {2}b", e.NetworkAdapterName, e.BytesReceived, e.BytesSent);
            m_counter++;
            if (m_counter > 10) m_event.Set();
        }
    }
}
