using System.Collections.Generic;
using System.Net;
using System.Timers;

namespace AnignoraCommunication.UdpCommunication.UdpAckCommunication
{
    public class UdpAckCommunicator
    {
        #region Fields
        private bool _showDebug;
        UdpCommunicator _comm;
        Dictionary<int, UdpAckQueuedMessage> _sendQueue = new Dictionary<int, UdpAckQueuedMessage>(50);
        Timer _sendTimer;
        #endregion

        #region Properties
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
        public UdpAckCommunicator(int communicatorId,IPEndPoint localIPE, bool showDebug)
        {
            _showDebug = showDebug;
            _comm = new UdpCommunicator(localIPE);
            _comm.OnBytesReceived += comm_OnBytesReceived;
            _sendTimer = new Timer(1000);
            _sendTimer.Elapsed += _sendTimer_Elapsed;
        }
        #endregion

        #region Events handlers
        void _sendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _sendTimer.Stop();
            List<UdpAckQueuedMessage> ackMessages = new List<UdpAckQueuedMessage>();
            lock (_sendQueue)
            {
                //Add all queued messages to UdpCommunicator's send queue
                foreach (UdpAckQueuedMessage message in _sendQueue.Values)
                {
                    _comm.Send(message.GetByteArray(), message.RemoteIPE);
                    if (message.IsAck) ackMessages.Add(message);
                }
                //Remove all Ack messages so they will not be sent again
                foreach (UdpAckQueuedMessage ackMessage in ackMessages)
                {
                    _sendQueue.Remove(ackMessage.MessageId);
                }
            }
            _sendTimer.Start();
            _comm.StartSendTimerOnce();
        }
        #endregion

        #region public
        public void Send(byte[] buffer, IPEndPoint remoteIPE)
        {
            UdpAckQueuedMessage message = new UdpAckQueuedMessage(buffer, remoteIPE);
            lock (_sendQueue)
            {
                _sendQueue.Add(message.MessageId, message);
            }
        }

        public void Start()
        {
            _comm.StartReceiving();
            _sendTimer.Start();
        }

        void comm_OnBytesReceived(IPEndPoint remoteIPE, byte[] receivedBuffer)
        {
            UdpAckQueuedMessage message = new UdpAckQueuedMessage(receivedBuffer);
            if (message.IsAck)
            {
                lock (_sendQueue)
                {
                    _sendQueue.Remove(message.AckForMessageId);
                }
            }
            else
            {
                UdpAckQueuedMessage ackMessage = new UdpAckQueuedMessage(remoteIPE, message.MessageId);
                Send(ackMessage);
            }
        }
        #endregion

        #region private
        private void Send(UdpAckQueuedMessage message)
        {
            lock (_sendQueue)
            {
                _sendQueue.Add(message.MessageId, message);
            }
        }
        #endregion


    }
}
