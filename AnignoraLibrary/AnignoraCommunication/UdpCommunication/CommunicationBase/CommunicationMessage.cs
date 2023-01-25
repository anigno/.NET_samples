using System.Net;

namespace AnignoraCommunication.UdpCommunication.CommunicationBase
{
    public class CommunicationMessage
    {
        #region Fields
        private static int _uniqueId;
        private object _remoteIdSync = new object();
        private string _messageId;
        private readonly IPEndPoint _remoteIPE;
        private readonly byte[] _buffer;
        #endregion

        #region Properties
        public string MessageId
        {
            get { return _messageId; }
        }

        public IPEndPoint RemoteIPE
        {
            get { return _remoteIPE; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }
        #endregion

        /// <summary>
        /// CTOR, create new communication message to be sent, local IPE is used for creating unique message id
        /// </summary>
        public CommunicationMessage(IPEndPoint localIPE, IPEndPoint remoteIPE, byte[] buffer)
        {
            lock (_remoteIdSync)
            {
                _messageId = string.Format("[{0}][{1}]", localIPE.ToString(), _uniqueId++.ToString());
                if (_uniqueId > 999999) _uniqueId = 0;
            }
            _remoteIPE = remoteIPE;
            _buffer = buffer;
        }
    }
}