#region

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace SimpleTcpCommunication.TcpCommunicator
{
    public class SimpleTcpCommunicator : ISimpleTcpCommunicator
    {
        #region Public Methods

        public void Connect(IPEndPoint p_remoteIpEndPoint)
        {
            InitCancellationTokenSource();
            m_tcpClient = new TcpClient();
            m_tcpClient.Connect(p_remoteIpEndPoint);
            OnConnectivityChanged(true);
        }

        public void Listen(IPEndPoint p_localIpEndPoint)
        {
            InitCancellationTokenSource();
            m_tcpListener = new TcpListener(p_localIpEndPoint);
            m_tcpClient = new TcpClient();
            m_tcpListener.Start();
            m_tcpClient.Client = m_tcpListener.AcceptSocket();
            OnConnectivityChanged(true);
            Task.Factory.StartNew(DataReceivingTask, m_cancellationTokenSource.Token);
        }


        public void SendBytes(byte[] p_bytes)
        {
            int length = p_bytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);
            m_tcpClient.GetStream().Write(lengthBytes, 0, lengthBytes.Length);
            m_tcpClient.GetStream().Write(p_bytes, 0, p_bytes.Length);
        }

        public void Close()
        {
            m_cancellationTokenSource.Cancel(false);
            if (m_tcpClient != null) m_tcpClient.Close();
            if (m_tcpListener != null) m_tcpListener.Stop();
        }

        #endregion

        #region Events

        public event Action<byte[]> OnBytesReceived = delegate { };
        public event Action<bool> OnConnectivityChanged = delegate { };
        public event Action<float> OnProgress = delegate { };

        #endregion

        #region Private Methods

        private void DataReceivingTask()
        {
            int nextDataLength = 0;
            while (!m_cancellationTokenSource.IsCancellationRequested)
            {
                switch (m_readPhase)
                {
                    case ReadPhaseEnum.ReadData:
                        MemoryStream memoryStreamData = new MemoryStream(nextDataLength);
                        byte[] buffer = new byte[nextDataLength];
                        while (memoryStreamData.Length < nextDataLength)
                        {
                            int readCount = m_tcpClient.Client.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                            memoryStreamData.Write(buffer, 0, readCount);
                            buffer = new byte[nextDataLength - memoryStreamData.Length];
                            OnProgress(100f * memoryStreamData.Length / nextDataLength);
                        }
                        OnBytesReceived(memoryStreamData.GetBuffer());
                        m_readPhase=ReadPhaseEnum.ReadLength;
                        break;
                    case ReadPhaseEnum.ReadLength:
                        MemoryStream memoryStreamLength = new MemoryStream();
                        buffer = new byte[SIZE_BYTES_NUMBER];
                        while (memoryStreamLength.Length < SIZE_BYTES_NUMBER)
                        {
                            int readCount = m_tcpClient.Client.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                            memoryStreamLength.Write(buffer, 0, readCount);
                            buffer = new byte[SIZE_BYTES_NUMBER - memoryStreamLength.Length];
                        }
                        nextDataLength = BitConverter.ToInt32(memoryStreamLength.GetBuffer(), 0);
                        m_readPhase = ReadPhaseEnum.ReadData;
                        break;
                }
            }
        }

        private void InitCancellationTokenSource()
        {
            if (m_cancellationTokenSource != null && !m_cancellationTokenSource.IsCancellationRequested) m_cancellationTokenSource.Cancel(false);
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Fields

        private TcpListener m_tcpListener;
        private TcpClient m_tcpClient;
        private CancellationTokenSource m_cancellationTokenSource;
        private ReadPhaseEnum m_readPhase = ReadPhaseEnum.ReadLength;

        #endregion

        #region Constants

        public const int SIZE_BYTES_NUMBER = 4;

        #endregion

        private enum ReadPhaseEnum
        {
            ReadLength,
            ReadData
        }
    }
}