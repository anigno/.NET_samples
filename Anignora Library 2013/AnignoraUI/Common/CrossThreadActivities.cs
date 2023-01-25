using System;

namespace AnignoraUI.Common
{
    public static class CrossThreadActivities
    {
        public static void Do<TControl>(TControl p_control, Action<TControl> p_action) where TControl : System.Windows.Forms.Control
        {
            if (p_control.InvokeRequired)
            {
                if (p_control.IsHandleCreated)
                    if(!p_control.IsDisposed)
                    p_control.Invoke(p_action, p_control);
                return;
            }
            p_action(p_control);
        }

        public static TReturn Get<TControl, TReturn>(TControl p_control, Func<TControl, TReturn> p_function) where TControl : System.Windows.Forms.Control
        {
            if (p_control.InvokeRequired)
            {
                return (TReturn) p_control.Invoke(p_function,p_control);
            }
            return p_function(p_control);
        }

    }
}
