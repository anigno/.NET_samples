using System;
using AnignoraCommunication.Communicators;

namespace AnignoraCommunication.Agents.Icd
{
    [Serializable]
    public class IcdResponse : MsmqCommunicatorMessage
    {
        public uint RequestId { get; private set; }

        protected IcdResponse(uint p_requestId)
        {
            RequestId = p_requestId;
        }
    }
}
