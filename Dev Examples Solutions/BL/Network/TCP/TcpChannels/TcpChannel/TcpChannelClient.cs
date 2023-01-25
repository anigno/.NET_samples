#region

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace TcpChannel
{
    public class TcpChannelClient
    {
        #region Constructors

        public TcpChannelClient(IPEndPoint remoteIpEndPoint, int keepAliveInterval = 1000)
        {
            m_remoteIpEndPoint = remoteIpEndPoint;
            m_keepAliveInterval = keepAliveInterval;
            m_keepAliveSendingTimer = new Timer(KeepAliveSendingCallback);
            BeginConnect();
        }

        #endregion

        #region Public Methods

        public int Send(byte[] bytes)
        {
            byte[] lengthBytes = BitConverter.GetBytes(bytes.Length);
            m_sendingItemsQueue.Add(new SendingItem(lengthBytes, bytes));
            return m_sendingItemsQueue.Count;
        }

        #endregion

        #region Events

        public event Action<bool> OnConnectivityChanged = delegate { };
        public event Action<byte[]> OnBytesSent = delegate { };

        #endregion

        #region Private Methods

        private void BeginConnect()
        {
            StopKeepAliveSendingTimer();
            Task.Factory.StartNew(async () =>
            {
                if (m_tcpClient != null) m_tcpClient.Close();
                m_tcpClient = new TcpClient();
                try
                {
                    m_tcpClient.Connect(m_remoteIpEndPoint);
                    OnConnectivityChanged(true);
                    InitCancellationTokenSource();
                    Task.Factory.StartNew(SendingTask, m_cancellationTokenSource.Token);
                    ResetKeepAliveSendingTimer();
                }
                catch (Exception exception)
                {
                    OnConnectivityChanged(false);
                    BeginConnect();
                }
                await Task.Delay(m_keepAliveInterval);
            });
        }

        private void KeepAliveSendingCallback(object state)
        {
            Send(new byte[0]);
        }

        private void ResetKeepAliveSendingTimer()
        {
            m_keepAliveSendingTimer.Change(m_keepAliveInterval, m_keepAliveInterval);
        }

        private void StopKeepAliveSendingTimer()
        {
            m_keepAliveSendingTimer.Change(-1, -1);
        }

        private void InitCancellationTokenSource()
        {
            if (m_cancellationTokenSource != null && !m_cancellationTokenSource.IsCancellationRequested) m_cancellationTokenSource.Cancel(false);
            m_cancellationTokenSource = new CancellationTokenSource();
        }


        private void SendingTask()
        {
            SendingItem item;
            while ((item = m_sendingItemsQueue.Take(m_cancellationTokenSource.Token)) != null)
            {
                ResetKeepAliveSendingTimer();
                try
                {
                    m_tcpClient.Client.Send(item.LengthBytes, 0, item.LengthBytes.Length, SocketFlags.None);
                    m_tcpClient.Client.Send(item.DataBytes, 0, item.DataBytes.Length, SocketFlags.None);
                    if (item.DataBytes.Length > 0) OnBytesSent(item.DataBytes);
                }
                catch (Exception exception)
                {
                    m_cancellationTokenSource.Cancel(false);
                    m_sendingItemsQueue.Add(item);
                    OnConnectivityChanged(false);
                    BeginConnect();
                    break;
                }
            }
        }

        #endregion

        #region Fields

        private readonly Timer m_keepAliveSendingTimer;

        private readonly BlockingCollection<SendingItem> m_sendingItemsQueue = new BlockingCollection<SendingItem>(new ConcurrentQueue<SendingItem>());

        private readonly IPEndPoint m_remoteIpEndPoint;
        private readonly int m_keepAliveInterval;
        private TcpClient m_tcpClient;
        private CancellationTokenSource m_cancellationTokenSource;

        #endregion
    }
}