#region

using System.IO.Pipes;
using System.Threading.Tasks;

#endregion

namespace WorkingWithAppDomains.SimplePipesOld
{
    public class SimplePipeWriter_old
    {
        #region Constructors

        public SimplePipeWriter_old(string p_pipeName)
        {
            m_pipeName = p_pipeName;
        }

        #endregion

        #region Public Methods

        public bool WriteData(byte[] p_buffer, int p_writeTimeoutMs = 1000)
        {
            m_client = new NamedPipeClientStream(".", m_pipeName, PipeDirection.InOut, PipeOptions.None);
            bool bResultConnect = Task.Factory.StartNew(() => { m_client.Connect(); }).Wait(p_writeTimeoutMs);
            if (bResultConnect)
            {
                bool bResultWrite = Task.Factory.StartNew(() => { m_client.Write(p_buffer, 0, p_buffer.Length); }).Wait(p_writeTimeoutMs);
                if (bResultWrite)
                {
                    m_client.Close();
                    m_client.Dispose();
                    return true;
                }
            }
            m_client.Close();
            m_client.Dispose();
            return false;
        }

        #endregion

        #region Fields

        private readonly string m_pipeName;

        private NamedPipeClientStream m_client;

        #endregion
    }
}