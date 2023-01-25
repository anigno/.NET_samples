#region

using System;
using System.Net;

#endregion

namespace SimpleTcpCommunication.TcpCommunicator
{
    public interface ISimpleTcpCommunicator
    {
        #region Public Methods

        void Connect(IPEndPoint p_remoteIpEndPoint);
        void Listen(IPEndPoint p_localIpEndPoint);
        void SendBytes(byte[] p_bytes);
        void Close();

        #endregion

        #region Events

        event Action<byte[]> OnBytesReceived;
        event Action<bool> OnConnectivityChanged;
        event Action<float> OnProgress;

        #endregion
    }
}