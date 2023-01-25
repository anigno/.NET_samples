using AnignoraCommunication.Agents.Icd;

namespace AnignoraCommunication.Agents.Handlers
{
    public abstract class ResponseHandler
    {
        public abstract void RunResponseHandler(IcdResponse p_response);
    }
}
