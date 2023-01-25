using System;
using AnignoraCommunication.Communicators;

namespace AnignoraCommunication.Agents.Icd
{
    [Serializable]
    public class IcdCommand : IcdRequest
    {
        protected IcdCommand()
            : base(Common.MAX_REQUEST_HANDLE_TIME)
        {
        }
    }
}