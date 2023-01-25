using System;

namespace AnignoraCommunication.Monitoring
{
    public class NetworkMonitorEventArgs:EventArgs
    {
        public string NetworkAdapterName { get; private set; }
        public float BytesReceived { get; private set; }
        public float BytesSent { get; private set; }


        public NetworkMonitorEventArgs(string p_networkAdapterName, float p_bytesReceived, float p_bytesSent)
        {
            NetworkAdapterName = p_networkAdapterName;
            BytesReceived = p_bytesReceived;
            BytesSent = p_bytesSent;
        }
    }
}
