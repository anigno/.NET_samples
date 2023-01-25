using System;

namespace AnignoraProcesses.BThreadPool
{
    public class ActionExceptionEventArgs : ActionEventArgs
    {
        public Exception ActionException { get; private set; }

        public ActionExceptionEventArgs(ActionltemBase p_actionItemBase, Exception p_exception)
            : base(p_actionItemBase)
        {
            ActionException = p_exception;
        }
    }
}