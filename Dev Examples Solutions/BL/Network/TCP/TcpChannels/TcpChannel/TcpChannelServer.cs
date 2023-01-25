#region

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace TcpChannel
{
    public class TcpChannelServer
    {
        #region Constructors

        public TcpChannelServer(IPEndPoint localIpEndPoint, int noMessageDisconnectInterval = 5000)
        {
            m_localIpEndPoint = localIpEndPoint;
            m_noMessageDisconnectInterval = noMessageDisconnectInterval;
            m_noMessageDisconnectionTimer = new Timer(DisconnectionTimerCallback);
            BeginListen();
        }

        #endregion

        #region Events

        public event Action<bool> OnConnectivityChanged = delegate { };
        public event Action<byte[]> OnBytesReceived = delegate { };
        public event Action<float> OnProgress = delegate { };

        #endregion

        #region Private Methods

        private void BeginListen()
        {
            InitCancellationTokenSource();
            StopMessageReceiveTimer();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (m_listener != null) m_listener.Stop();
                    m_listener = new TcpListener(m_localIpEndPoint);
                    m_listener.Start();
                    if (m_tcpClient != null) m_tcpClient.Close();
                    m_tcpClient = m_listener.AcceptTcpClient();
                    OnConnectivityChanged(true);
                    Task.Factory.StartNew(DataReceivingTask, m_cancellationTokenSource.Token);
                    ResetMessageReceiveTimer();
                }
                catch (Exception exception)
                {
                    BeginListen();
                }
            }, m_cancellationTokenSource.Token);
        }

        private void DisconnectionTimerCallback(object state)
        {
            StopMessageReceiveTimer();
            m_cancellationTokenSource.Cancel(false);
            OnConnectivityChanged(false);
            BeginListen();
        }

        private void InitCancellationTokenSource()
        {
            if (m_cancellationTokenSource != null && !m_cancellationTokenSource.IsCancellationRequested) m_cancellationTokenSource.Cancel(false);
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        private void ResetMessageReceiveTimer()
        {
            m_noMessageDisconnectionTimer.Change(m_noMessageDisconnectInterval, m_noMessageDisconnectInterval);
        }

        private void StopMessageReceiveTimer()
        {
            m_noMessageDisconnectionTimer.Change(-1, -1);
        }

        private void DataReceivingTask()
        {
            try
            {
                int nextDataLength = 0;
                ReadPhaseEnum m_readPhase = ReadPhaseEnum.ReadLength;

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
                                OnProgress(100f*memoryStreamData.Length/nextDataLength);
                            }

                            byte[] memoryStreamBUffer = memoryStreamData.GetBuffer();
                            if (memoryStreamBUffer.Length > 0) OnBytesReceived(memoryStreamBUffer);
                            m_readPhase = ReadPhaseEnum.ReadLength;
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
                    ResetMessageReceiveTimer();
                }
            }
            catch (Exception exception)
            {
                OnConnectivityChanged(false);
                BeginListen();
            }
        }

        #endregion

        #region Fields

        private Timer m_noMessageDisconnectionTimer;

        private readonly IPEndPoint m_localIpEndPoint;
        private readonly int m_noMessageDisconnectInterval;

        private TcpClient m_tcpClient;
        private TcpListener m_listener;
        private CancellationTokenSource m_cancellationTokenSource;

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