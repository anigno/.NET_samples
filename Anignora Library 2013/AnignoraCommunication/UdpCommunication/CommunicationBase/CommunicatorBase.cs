using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Timers;

namespace AnignoraCommunication.UdpCommunication.CommunicationBase
{
    /// <summary>
    /// Implement base for communication using a send queue
    /// </summary>
    public abstract class CommunicatorBase
    {
        #region Delegates
        public delegate void OnBytesReceivedDelegate(IPEndPoint remoteIPE, byte[] receivedBuffer);
        public delegate void OnBytesReceivedErrorDelegate(string errorCode, string errorMessage);
        #endregion

        #region Events decleration
        public event OnBytesReceivedDelegate OnBytesReceived;
        public event OnBytesReceivedErrorDelegate OnBytesReceivedError;
        #endregion

        #region Fields
        private IPEndPoint _localIPE;
        private readonly Dictionary<IPEndPoint, List<CommunicationMessage>> _sendQueue = new Dictionary<IPEndPoint, List<CommunicationMessage>>();
        private Timer _sendTimer = new Timer(1000);
        #endregion

        #region Properties
        public IPEndPoint LocalIPE
        {
            get { return _localIPE; }
            set { _localIPE = value; }
        }

        public Dictionary<IPEndPoint, List<CommunicationMessage>> SendQueue
        {
            get { return _sendQueue; }
        }

        public double SendInterval
        {
            get
            {
                return _sendTimer.Interval;
            }
            set
            {
                _sendTimer.Interval = value;
            }
        }
        #endregion

        #region CTOR

        protected CommunicatorBase(IPEndPoint localIPE)
        {
            _localIPE = localIPE;
            _sendTimer.Elapsed += new ElapsedEventHandler(_sendTimer_Elapsed);
        }
        #endregion

        #region Public
        /// <summary>
        /// Enqueue message in send queue
        /// </summary>
        protected void CommunicationBaseSendMessage(CommunicationMessage message)
        {
            lock (_sendQueue)
            {
                if (!_sendQueue.ContainsKey(message.RemoteIPE))
                {
                    List<CommunicationMessage> messageList = new List<CommunicationMessage>();
                    _sendQueue.Add(message.RemoteIPE, messageList);
                }
                _sendQueue[message.RemoteIPE].Add(message);
            }
        }

        protected void RemoveMessageFromQueue(CommunicationMessage message)
        {
            lock (_sendQueue)
            {
                if (!_sendQueue.ContainsKey(message.RemoteIPE)) return;
                if (!_sendQueue[message.RemoteIPE].Contains(message)) return;
                _sendQueue[message.RemoteIPE].Remove(message);
                if (_sendQueue[message.RemoteIPE].Count == 0) _sendQueue.Remove(message.RemoteIPE);
            }
        }

        protected void RemoveMessageFromQueue(string messageId)
        {
            lock (_sendQueue)
            {
                CommunicationMessage messageToBeRemoved = null;
                foreach (KeyValuePair<IPEndPoint, List<CommunicationMessage>> pair in _sendQueue)
                {
                    if (messageToBeRemoved != null)
                    {
                        break;
                    }
                    foreach (CommunicationMessage message in pair.Value)
                    {
                        if (message.MessageId == messageId)
                        {
                            messageToBeRemoved = message;
                            break;
                        }
                    }
                }
                if (messageToBeRemoved != null) RemoveMessageFromQueue(messageToBeRemoved);
            }
        }

        public void StartSendTimerOnce()
        {
            SendQueuedMessages();
        }
        public void StartSendingTimer()
        {
            _sendTimer.Start();
        }
        
        public void StopSendingTimer()
        {
            _sendTimer.Stop();
        }

        public void RaiseOnBytesReceived(IPEndPoint remoteIPE, byte[] receivedBuffer)
        {
            if (OnBytesReceived != null) OnBytesReceived(remoteIPE, receivedBuffer);
        }
        public void RaiseOnBytesReceivedError(string errorCode, string errorMessage)
        {
            if (OnBytesReceivedError != null) OnBytesReceivedError(errorCode, errorMessage);
        }
        #endregion

        #region Events handlers
        void _sendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendQueuedMessages();
        }
        #endregion

        #region Override
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            lock (_sendQueue)
            {
                sb.Append(string.Format("--- CommunicatorBase ---\n"));
                sb.Append(string.Format("LocalIPE:{0}\n", LocalIPE.ToString()));
                foreach (KeyValuePair<IPEndPoint, List<CommunicationMessage>> pair in _sendQueue)
                {
                    sb.Append(string.Format(" RemoteIPE:{0}\n", pair.Key.ToString()));
                    foreach (CommunicationMessage message in pair.Value)
                    {
                        sb.Append(string.Format("  MessageId:{0}\n", message.MessageId));
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Abstracts

        /// <summary>
        /// Use this method to send a byte array to remote IP endpoint
        /// </summary>
        /// <param name="sendBuffer"></param>
        /// <param name="remoteIPE"></param>
        public abstract void Send(byte[] sendBuffer, IPEndPoint remoteIPE);

        /// <summary>
        /// Send bytes array to remote IPEndPoint.
        /// This methods is being called at send timer elapsed event, for each CommunicationMessage added.
        /// The message will not be removed from the send queue.
        /// </summary>
        public abstract void SendCommunicationMessageCallBack(CommunicationMessage message);

        /// <summary>
        /// Start receiving messages.
        /// This method is called in Activate()
        /// </summary>
        public abstract void StartReceiving();

        /// <summary>
        /// Stop receiving messages.
        /// This method is called in Deactivate()
        /// </summary>
        public abstract void StopReceiving();

        /// <summary>
        /// Activities after the message has been sent
        /// </summary>
        /// <param name="message"></param>
        public abstract void CommunicationMessageSentCallBack(CommunicationMessage message);

        #endregion

        #region Private
        private void SendQueuedMessages()
        {
            lock (_sendQueue)
            {
                List<CommunicationMessage> messagesListForSentCallBack = new List<CommunicationMessage>();
                //Build message list to be sent for all IPEndPoints
                foreach (KeyValuePair<IPEndPoint, List<CommunicationMessage>> pair in _sendQueue)
                {
                    foreach (CommunicationMessage message in pair.Value)
                    {
                        SendCommunicationMessageCallBack(message);
                        messagesListForSentCallBack.Add(message);
                    }
                }
                //Send messaged using abstract method
                foreach (CommunicationMessage message in messagesListForSentCallBack)
                {
                    CommunicationMessageSentCallBack(message);
                }
                messagesListForSentCallBack.Clear();
            }
        }
        #endregion
    }
}
