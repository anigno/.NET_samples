using System;
using AnignoraCommunication.Communicators;

namespace AnignoraCommunication.Agents.Icd
{
    [Serializable]
    public class IcdRequest : MsmqCommunicatorMessage
    {
        public DateTime SendTime { get; set; }
        public TimeSpan Timeout { get; set; }

        protected IcdRequest(TimeSpan p_timeout)
        {
            Timeout = p_timeout;
        }
    }
}
