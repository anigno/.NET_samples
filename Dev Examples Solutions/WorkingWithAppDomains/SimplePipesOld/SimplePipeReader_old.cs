#region

using System;
using System.Data;
using System.IO.Pipes;
using System.Threading.Tasks;

#endregion

namespace WorkingWithAppDomains.SimplePipesOld
{
    public class SimplePipeReader_old
    {
        #region Constructors

        public SimplePipeReader_old(string p_pipeName, int p_maxBufferLength)
        {
            m_pipeName = p_pipeName;
            m_maxBufferLength = p_maxBufferLength;
        }

        #endregion

        #region Public Methods

        public void StopWaitingForData()
        {
            m_server.Close();
            m_server.Dispose();
        }

        public byte[] WaitForData()
        {
            m_server = new NamedPipeServerStream(m_pipeName, PipeDirection.InOut, 1);
            byte[] buffer = new byte[m_maxBufferLength + 1];
            m_server.WaitForConnection();
            int nBytes = m_server.Read(buffer, 0, buffer.Length);
            m_server.Close();
            m_server.Dispose();
            if (nBytes == m_maxBufferLength + 1) throw new DataException("Received data is larger then buffer length, Data will be trunct and wrong");
            byte[] bufferRet = new byte[nBytes];
            Buffer.BlockCopy(buffer, 0, bufferRet, 0, nBytes);
            return bufferRet;
        }

        public void BeginWaitForData()
        {
            Task.Factory.StartNew(() =>
            {
                byte[] buffer = WaitForData();
                OnDataReceived(buffer);
            });
        }

        #endregion

        #region Events

        public event Action<byte[]> OnDataReceived = delegate { };

        #endregion

        #region Fields

        private readonly string m_pipeName;
        private readonly int m_maxBufferLength;
        private NamedPipeServerStream m_server;

        #endregion
    }
}