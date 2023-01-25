#region

using System;
using System.Collections.Generic;
using AnignoraCommunication.Agents.Handlers;
using AnignoraCommunication.Agents.Icd;
using AnignoraCommunication.Communicators;

#endregion

namespace AnignoraCommunication.Agents
{
    public class MsmqAgent
    {
		#region (------  Fields  ------)

        private readonly MsmqCommunicator m_communicator;
        private readonly Dictionary<Type, Type> m_registeredHandlers = new Dictionary<Type, Type>();
        private readonly RequestsList m_requestsList;

        public uint[] ActiveRequestsIds { get { return m_requestsList.RequestsIds; } }

            #endregion (------  Fields  ------)

		#region (------  Events  ------)

        public event CommunicatorRequestHandler OnRequestTimeout;

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        public MsmqAgent(int p_localQueueIndex, int p_destQueueIndex, string p_destinationMachine)
        {
            m_communicator = new MsmqCommunicator(p_localQueueIndex, p_destQueueIndex, p_destinationMachine);
            m_communicator.OnMessageReceived += m_communicator_OnMessageReceived;
            m_requestsList = new RequestsList(TimeSpan.FromSeconds(1));
            m_requestsList.OnRequestTimeout += m_requestsList_OnRequestTimeout;
        }

		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)

        public void SendRequest(IcdRequest p_request)
        {
            p_request.SendTime = DateTime.Now;
            m_requestsList.AddRequest(p_request);
            m_communicator.SendMessage(p_request);
        }

        public void SendCommand(IcdCommand p_command)
        {
            SendRequest(p_command);
        }

        #endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void m_communicator_OnMessageReceived(MsmqCommunicatorMessage p_msmqCommunicatorMessage)
        {
            Type messageType = p_msmqCommunicatorMessage.GetType();
            Type handlerType = m_registeredHandlers[messageType];
            IcdCommand command = p_msmqCommunicatorMessage as IcdCommand;
            IcdRequest requestOrCommand = p_msmqCommunicatorMessage as IcdRequest;
            IcdResponse response = p_msmqCommunicatorMessage as IcdResponse;
            if (command != null)
            {
                m_requestsList.RemoveRequest(command.Id);
            }
            if (requestOrCommand != null)
            {
                RequestHandler requestHandler = (RequestHandler) Activator.CreateInstance(handlerType);
                IcdResponse msmqResponse = requestHandler.RunRequestHandler(requestOrCommand);
                if (msmqResponse != null) m_communicator.SendMessage(msmqResponse);
            }
            if (response != null)
            {
                if (!m_requestsList.Contains(response.RequestId)) return; //request was timeout
                m_requestsList.RemoveRequest(response.RequestId);
                ResponseHandler responseHandler = (ResponseHandler) Activator.CreateInstance(handlerType);
                responseHandler.RunResponseHandler(response);
            }
        }

        private void m_requestsList_OnRequestTimeout(IcdRequest p_msmqCommunicatorMessage)
        {
            OnRequestTimeout(p_msmqCommunicatorMessage);
        }

		#endregion (------  Private Methods  ------)

        public void RegisterRequestResponseHandler<TRequestMessage, TRequestHandler, TResponseMessage, TResponseHandler>()
            where TRequestMessage : IcdRequest
            where TRequestHandler : RequestHandler
            where TResponseMessage : IcdResponse
            where TResponseHandler : ResponseHandler
        {
            m_registeredHandlers.Add(typeof(TRequestMessage), typeof(TRequestHandler));
            m_registeredHandlers.Add(typeof(TResponseMessage), typeof(TResponseHandler));
        }

        public void RegisterCommandHandler<TCommandMessage, TCommandHandler>()
            where TCommandMessage : IcdCommand
            where TCommandHandler : CommandHandler
        {
            RegisterRequestResponseHandler<TCommandMessage, TCommandHandler, ResponseNullMessage, ResponseNullHandler>();
        }

    }
}