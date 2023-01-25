using System;
using System.Net;

namespace AnignoraCommunication.Http
{
    public class HttpAgentRequestReceivedEventArgs:EventArgs
    {
        public string RawUrl { get; private set; }
        public IPEndPoint LocalIpEndPoint { get; private set; }
        public IPEndPoint RemoteIpEndPoint { get; private set; }

        public HttpAgentRequestReceivedEventArgs(string p_rawUrl, IPEndPoint p_localIpEndPoint, IPEndPoint p_remoteIpEndPoint)
        {
            RawUrl = p_rawUrl;
            LocalIpEndPoint = p_localIpEndPoint;
            RemoteIpEndPoint = p_remoteIpEndPoint;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", RawUrl, LocalIpEndPoint, RemoteIpEndPoint);
        }
    }
}
