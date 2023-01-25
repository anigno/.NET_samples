using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AnignoraCommunication.Agents.Icd;
using AnignoraCommunication.Communicators;

namespace AnignoraCommunication.Agents
{
    public class RequestsList
    {
		#region (------  Fields  ------)

        private readonly Dictionary<uint, IcdRequest> m_requests = new Dictionary<uint, IcdRequest>();
        private readonly List<uint> m_timedoutRequests = new List<uint>();
        private readonly TimeSpan m_timeoutCheckInterval;
        private readonly Timer m_timer;

		#endregion (------  Fields  ------)

		#region (------  Events  ------)

        public event CommunicatorRequestHandler OnRequestTimeout = delegate { };

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        public RequestsList(TimeSpan p_timeoutCheckInterval)
        {
            m_timeoutCheckInterval = p_timeoutCheckInterval;
            m_timer = new Timer(timerCallback, null, p_timeoutCheckInterval,Common.MAX_REQUEST_HANDLE_TIME);
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public uint[] RequestsIds
        {
            get {
                lock (m_requests)
                {
                    return m_requests.Keys.ToArray();
                }
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

        public void AddRequest( IcdRequest p_request)
        {
            lock (m_requests)
            {
                m_requests.Add(p_request.Id, p_request);
            }
        }

        public bool Contains(uint p_id)
        {
            return m_requests.ContainsKey(p_id);
        }

        public void RemoveRequest(uint p_requestId)
        {
            lock (m_requests)
            {
                m_requests.Remove(p_requestId);
            }
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

         private void timerCallback(object p_object)
        {
            lock (m_requests)
            {
                m_timedoutRequests.Clear();
                foreach (IcdRequest request in m_requests.Values)
                {
                    TimeSpan timeDelta = DateTime.Now - request.SendTime;
                    if (timeDelta > request.Timeout)
                    {
                        m_timedoutRequests.Add(request.Id);
                    }
                }
                foreach (uint id in m_timedoutRequests)
                {
                    OnRequestTimeout(m_requests[id]);
                    RemoveRequest(id);
                }
            }
            m_timer.Change(m_timeoutCheckInterval, Common.MAX_REQUEST_HANDLE_TIME);
        }

		#endregion (------  Private Methods  ------)
    }
}
