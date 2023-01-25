using System;
using System.Threading;
using AnignoraCommunication.Agents.Handlers;
using AnignoraCommunication.Agents.Icd;

namespace AnignoraLibrary_2010_Tester.CommunicationTests
{
    [Serializable]
    public class RequestTestMessage : IcdRequest
    {
        public RequestTestMessage(TimeSpan p_timeout) : base(p_timeout)
        {
        }

        public string RequestString { get; set; }
    }

    [Serializable]
    public class ResponseTestMessage : IcdResponse
    {
        public ResponseTestMessage(uint p_requestId) : base(p_requestId)
        {
        }

        public string ResponseString { get; set; }
    }

    public class RequestTestHandler : RequestHandler
    {
        public override IcdResponse RunRequestHandler(IcdRequest p_message)
        {
            RequestTestMessage requestTestMessage=(RequestTestMessage)p_message;
            TestsHelper.DebugWrite(requestTestMessage.RequestString + " " + p_message.Id);
            Thread.Sleep(2000);
            return new ResponseTestMessage(p_message.Id) { ResponseString = "Response to: "+p_message.Id};
        }
    }

    public class ResponseTestHandler : ResponseHandler
    {
        public override void RunResponseHandler(IcdResponse p_message)
        {
            ResponseTestMessage responseTestMessage = (ResponseTestMessage)p_message;
            TestsHelper.DebugWrite(responseTestMessage.ResponseString);
        }
    }

    [Serializable]
    public class TestCommandMessage : IcdCommand
    {
        public string CommandString { get; set; }
    }

    public class TestCommandHandler : CommandHandler
    {
        public override void RunCommandHandler(IcdCommand p_command)
        {
            TestCommandMessage message = (TestCommandMessage)p_command;
            TestsHelper.DebugWrite(message.CommandString + " " + p_command.Id);
        }
    }

}
