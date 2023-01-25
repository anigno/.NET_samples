using AnignoraCommunication.Agents.Icd;

namespace AnignoraCommunication.Agents.Handlers
{
    public abstract class RequestHandler
    {
        public abstract IcdResponse RunRequestHandler(IcdRequest p_command);
    }
}
