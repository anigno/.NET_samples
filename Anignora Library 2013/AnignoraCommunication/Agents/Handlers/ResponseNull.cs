using AnignoraCommunication.Agents.Icd;

namespace AnignoraCommunication.Agents.Handlers
{
    /// <summary>A null response message and handler for using request as command that does not response</summary>
    public class ResponseNullMessage : IcdResponse
    {
        protected ResponseNullMessage(uint p_requestId) : base(p_requestId)
        {
        }
    }

    public class ResponseNullHandler : ResponseHandler
    {
        public override void RunResponseHandler(IcdResponse p_response)
        {
            //Nothing
        }
    }
}
