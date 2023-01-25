using AnignoraCommunication.Agents.Icd;

namespace AnignoraCommunication.Agents.Handlers
{
    public abstract class CommandHandler :RequestHandler
    {
        public override IcdResponse RunRequestHandler(IcdRequest p_request)
        {
            RunCommandHandler((IcdCommand)p_request);
            return null;
        }
        public abstract void RunCommandHandler(IcdCommand p_command);
    }
}