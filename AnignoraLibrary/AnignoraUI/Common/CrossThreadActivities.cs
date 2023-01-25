using System;
using System.Windows;

namespace AnignoraUI.Common
{
    public static class CrossThreadActivities
    {
        /// <summary>
        /// Winform run over UI thread
        /// </summary>
        public static void Do<TControl>(TControl p_control, Action<TControl> p_action) where TControl : System.Windows.Forms.Control
        {
            if (p_control.InvokeRequired)
            {
                if (p_control.IsHandleCreated)
                    if (!p_control.IsDisposed)
                        try
                        {
                            p_control.Invoke(p_action, p_control);
                        }
                        catch {/*Nothing*/ }
                return;
            }
            p_action(p_control);
        }

        /// <summary>
        /// Winform Get from UI thread
        /// </summary>
        public static TReturn Get<TControl, TReturn>(TControl p_control, Func<TControl, TReturn> p_function) where TControl : System.Windows.Forms.Control
        {
            if (p_control.InvokeRequired)
            {
                return (TReturn)p_control.Invoke(p_function, p_control);
            }
            return p_function(p_control);
        }

        public static void Do(Action p_action)
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(p_action);
            }
            else
            {
                p_action();
            }

        }
    }
}
