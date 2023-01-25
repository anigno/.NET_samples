using System.Messaging;
using System.Net;
using AnignoraCommonAndHelpers;

namespace AnignoraCommunication.Communicators
{
    public class MsmqCommunicator : UniqueIdBase
    {
		#region (------  Fields  ------)

        private readonly string m_localMachineName;
        private readonly int m_localQueueIndex;
        private readonly MessageQueue m_receiveQueue;
        private readonly MessageQueue m_sendQueue;

		#endregion (------  Fields  ------)

		#region (------  Events  ------)

        public event CommunicatorHandler OnMessageReceived;

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        public MsmqCommunicator(int p_localQueueIndex, int p_destQueueIndex, string p_destinationMachine)
        {
            m_localMachineName = Dns.GetHostName();
            m_localQueueIndex = p_localQueueIndex;
            string sendQueueName = getRemoteQueueName(p_destinationMachine, p_destQueueIndex);
            m_sendQueue = new MessageQueue(sendQueueName);
            m_receiveQueue = createLocalPrivateMsmQueue("Communicator" + p_localQueueIndex);
            m_receiveQueue.Formatter = new BinaryMessageFormatter();
            m_sendQueue.Formatter = new BinaryMessageFormatter();
            m_receiveQueue.ReceiveCompleted += receiveQueueReceiveCompleted;
            m_receiveQueue.BeginReceive();
        }

		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)

        public void SendMessage(MsmqCommunicatorMessage p_msmqCommunicatorMessage)
        {
            p_msmqCommunicatorMessage.SenderMachineName = m_localMachineName;
            p_msmqCommunicatorMessage.SenderIndex = m_localQueueIndex;
            m_sendQueue.Send(p_msmqCommunicatorMessage);
        }

		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)

        protected void RaiseOnMessageReceived(MsmqCommunicatorMessage p_msmqCommunicatorMessage)
        {
            //OnMessageReceived.BeginInvoke(p_msmqCommunicatorMessage, null, null);
            OnMessageReceived(p_msmqCommunicatorMessage);
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private MessageQueue createLocalPrivateMsmQueue(string p_queueName)
        {
            string queueNameToCreate = @".\private$\" + p_queueName;
            if (MessageQueue.Exists(queueNameToCreate)) MessageQueue.Delete(queueNameToCreate);
            MessageQueue retQueue = MessageQueue.Create(queueNameToCreate);
            return retQueue;
        }

        private string getRemoteQueueName(string p_destinationMachine, int p_destQueueIndex)
        {
            return @"FormatName:Direct=OS:" + p_destinationMachine + @"\private$\Communicator" + p_destQueueIndex;
        }

        void receiveQueueReceiveCompleted(object p_sender, ReceiveCompletedEventArgs p_receiveCompletedEventArgs)
        {
            Message msg = m_receiveQueue.EndReceive(p_receiveCompletedEventArgs.AsyncResult);
            MsmqCommunicatorMessage msmqCommunicatorMessage = (MsmqCommunicatorMessage)msg.Body;
            //Will not dequeue next message until finish with current handler to maintain handing order
            RaiseOnMessageReceived(msmqCommunicatorMessage);
            m_receiveQueue.BeginReceive();
        }

		#endregion (------  Private Methods  ------)
    }
}