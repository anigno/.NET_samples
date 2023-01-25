using System;
using System.Diagnostics;

namespace AnignoraCommunication.Monitoring
{
    /// <summary>
    /// Monitor network trafic for selected network adapter
    /// </summary>
    public class NetworkTrafficMonitor
    {
		#region (------  Constants  ------)
        private const string NETWORK_INTERFACE = "Network Interface";
		#endregion (------  Constants  ------)

		#region (------  Fields  ------)
         private int m_interval = 500;
         private readonly PerformanceCounterCategory m_networkInterfaceCategory = new PerformanceCounterCategory(NETWORK_INTERFACE);
        private PerformanceCounter m_performanceCounterReceived;
        private PerformanceCounter m_performanceCounterSent;
        private readonly System.Threading.Timer m_timer;
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        /// <summary>
        /// Raised according to interval, giving network data monitored
        /// </summary>
        public event EventHandler<NetworkMonitorEventArgs> NetworkMonitored = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public NetworkTrafficMonitor()
        {
            setNetworkAdapter(0);
            m_timer = new System.Threading.Timer(onTimerCallback);
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        /// <summary>
        /// Gets or sets the event raise interval with new monitored data
        /// </summary>
        public int MonitorInterval
        {
            get { return m_interval; }
            set
            {
                Stop();
                m_interval = value;
                Start();
            }
        }

        /// <summary>
        /// Gets or Sets the network adapter being monitored
        /// </summary>
        public string NetworkAdapterName
        {
            get { return m_performanceCounterSent.InstanceName; }
            set
            {
                Stop();
                string[] networkAdaptersNames = NetworkAdaptersNames;
                for (int a = 0; a < networkAdaptersNames.Length; a++)
                {
                    if (networkAdaptersNames[a] == value)
                    {
                        setNetworkAdapter(a);
                        Start();
                        return;
                    }
                }
                throw new ArgumentException("Couldn't find adapter: " + value);
            }
        }

        /// <summary>
        /// Get all network adapters names on local machine
        /// </summary>
        public string[] NetworkAdaptersNames
        {
            get
            {
                string[] instanceNames = m_networkInterfaceCategory.GetInstanceNames();
                return instanceNames;
            }
        }
		#endregion (------  Properties  ------)

		#region (------  Events Handlers  ------)
        private void onTimerCallback(object p_object)
        {
            float bytesReceived = m_performanceCounterReceived.NextValue();
            float bytesSent = m_performanceCounterSent.NextValue();
            NetworkMonitored(this,new NetworkMonitorEventArgs(NetworkAdapterName,bytesReceived,bytesSent));
        }
		#endregion (------  Events Handlers  ------)

		#region (------  Public Methods  ------)
        public void Start()
        {
            m_timer.Change(0, m_interval);
        }

        public void Stop()
        {
            m_timer.Change(int.MaxValue, int.MaxValue);
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
        private void setNetworkAdapter(int p_index)
        {
            string adaptersName = NetworkAdaptersNames[p_index];
            if(m_performanceCounterReceived!=null)m_performanceCounterReceived.Dispose();
            if(m_performanceCounterSent!=null)m_performanceCounterSent.Dispose();
            m_performanceCounterSent = new PerformanceCounter(NETWORK_INTERFACE, "Bytes Sent/sec", adaptersName);
            m_performanceCounterReceived = new PerformanceCounter(NETWORK_INTERFACE, "Bytes Received/sec", adaptersName);
        }
		#endregion (------  Private Methods  ------)
    }
}