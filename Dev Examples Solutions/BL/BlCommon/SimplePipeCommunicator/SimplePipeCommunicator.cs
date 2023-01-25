#region

using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace BlCommon.SimplePipeCommunicator
{
    public class SimplePipeCommunicator
    {
        #region Constructors

        /// <summary>
        /// Writer Ctor
        /// </summary>
        public SimplePipeCommunicator(string p_pipeName, string p_serverName = ".")
        {
            m_pipeName = p_pipeName;
            m_serverName = p_serverName;
            m_buffer = new byte[0];
        }

        /// <summary>
        /// Reader Ctor
        /// </summary>
        public SimplePipeCommunicator(string p_pipeName, int p_maxBufferSize)
        {
            m_pipeName = p_pipeName;
            m_serverName = null;
            m_buffer = new byte[p_maxBufferSize];
        }

        #endregion

        #region Public Methods

        public void BeginConnect(int p_timeOut = 1000)
        {
            InitCancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                m_clientStream = new NamedPipeClientStream(m_serverName, m_pipeName, PipeDirection.Out, PipeOptions.None);
                try
                {
                    m_clientStream.Connect(p_timeOut);
                    OnConnected();
                }
                catch (TimeoutException)
                {
                    OnConnectionTimeout();
                }
            }, m_cancellationTokenSource.Token);
        }

        public void BeginWaitForConnection()
        {
            InitCancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                m_serverStream = new NamedPipeServerStream(m_pipeName, PipeDirection.In, 1);
                m_serverStream.WaitForConnection();
                OnConnected();
                BeginReading();
            }, m_cancellationTokenSource.Token);
        }

        public virtual void BeginClose()
        {
            Task.Factory.StartNew(() =>
            {
                m_cancellationTokenSource.Cancel(false);
                if (m_clientStream != null) m_clientStream.Close();
                if (m_serverStream != null) m_serverStream.Close();
                OnClosed();
            }, m_cancellationTokenSource.Token);
        }

        public void BeginWriteBytes(byte[] p_bytes)
        {
            m_clientStream.WriteAsync(p_bytes, 0, p_bytes.Length);
        }

        #endregion

        #region Events

        public event Action OnConnected = delegate { };
        public event Action OnConnectionTimeout = delegate { };
        public event Action OnClosed = delegate { };
        public event Action<byte[]> OnBytesReceived = delegate { };

        #endregion

        #region Private Methods

        private void InitCancellationTokenSource()
        {
            if (m_cancellationTokenSource != null && !m_cancellationTokenSource.IsCancellationRequested) m_cancellationTokenSource.Cancel();
            m_cancellationTokenSource = new CancellationTokenSource();
        }

        private void BeginReading()
        {
            Task.Factory.StartNew(() =>
            {
                while (!m_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    int nBytes = m_serverStream.Read(m_buffer, 0, m_buffer.Length);
                    if (nBytes == 0) continue;
                    byte[] tempBuffer = new byte[nBytes];
                    Buffer.BlockCopy(m_buffer, 0, tempBuffer, 0, nBytes);
                    OnBytesReceived(tempBuffer);
                }
            }, m_cancellationTokenSource.Token);
        }

        #endregion

        #region Fields

        private readonly string m_pipeName;
        private readonly string m_serverName;
        private NamedPipeClientStream m_clientStream;
        private NamedPipeServerStream m_serverStream;
        private CancellationTokenSource m_cancellationTokenSource;
        private readonly byte[] m_buffer;

        #endregion
    }
}