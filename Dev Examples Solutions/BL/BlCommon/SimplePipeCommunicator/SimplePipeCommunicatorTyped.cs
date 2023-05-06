#region

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace BlCommon.SimplePipeCommunicator
{
    public class SimplePipeCommunicatorTyped<TData> : SimplePipeCommunicator
    {
        #region Constructors

        /// <summary>
        /// Writer Ctor
        /// </summary>
        public SimplePipeCommunicatorTyped(string p_pipeName, string p_serverName = ".")
            : base(p_pipeName, p_serverName)
        {
        }

        /// <summary>
        /// Reader Ctor
        /// </summary>
        public SimplePipeCommunicatorTyped(string p_pipeName, int p_maxBufferSize)
            : base(p_pipeName, p_maxBufferSize)
        {
            OnBytesReceived += OnBytesReceivedHandler;
        }

        #endregion

        #region Public Methods

        public override void BeginClose()
        {
            OnBytesReceived -= OnBytesReceivedHandler;
            base.BeginClose();
        }

        public void BeginWriteData(TData p_data)
        {
            MemoryStream memoryStream = new MemoryStream();
            m_binaryFormatter.Serialize(memoryStream, p_data);
            BeginWriteBytes(memoryStream.GetBuffer());
        }

        #endregion

        #region Events

        public event Action<TData> OnDataReceived = delegate { };

        #endregion

        #region Private Methods

        private void OnBytesReceivedHandler(byte[] p_bytes)
        {
            TData t = (TData) m_binaryFormatter.Deserialize(new MemoryStream(p_bytes));
            OnDataReceived(t);
        }

        #endregion

        #region Fields

        private readonly BinaryFormatter m_binaryFormatter = new BinaryFormatter();

        #endregion
    }
}