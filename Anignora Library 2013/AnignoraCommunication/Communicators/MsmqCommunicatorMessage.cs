using System;
using AnignoraCommonAndHelpers;

namespace AnignoraCommunication.Communicators
{
    [Serializable]
    public class MsmqCommunicatorMessage : UniqueIdBase
    {
        public string SenderMachineName { get; set; }
        public int SenderIndex { get; set; }

    }
}