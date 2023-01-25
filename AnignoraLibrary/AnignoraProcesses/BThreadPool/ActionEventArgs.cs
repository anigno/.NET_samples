using System;

namespace AnignoraProcesses.BThreadPool
{
    public class ActionEventArgs : EventArgs
    {
        public ActionltemBase Actionltem { get; private set; }

        public ActionEventArgs(ActionltemBase p_actionItemBase)
        {
            Actionltem = p_actionItemBase;
        }
    }
}