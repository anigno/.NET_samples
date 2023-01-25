using System;
using AnignoraCommunication.Agents.Icd;

namespace AnignoraCommunication.Communicators
{
    public delegate void CommunicatorHandler(MsmqCommunicatorMessage p_msmqCommunicatorMessage);
    public delegate void CommunicatorRequestHandler(IcdRequest p_msmqCommunicatorMessage);

    public static class Common
    {
        public static readonly TimeSpan MAX_REQUEST_HANDLE_TIME = TimeSpan.FromDays(10);
    }
}
