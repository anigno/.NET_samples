#region

using System;
using System.Windows.Controls;

#endregion

namespace MvvmPatternExample.Utils
{
    public static class CrossThreadedActivities
    {
        #region Public Methods

        public static void Do(Control p_control, Action p_action)
        {
            if (p_control.Dispatcher.CheckAccess())
            {
                p_action();
            }
            else
            {
                p_control.Dispatcher.Invoke(p_action);
            }
        }

        public static T Get<T>(Control p_control, Func<T> p_func)
        {
            if (p_control.Dispatcher.CheckAccess())
            {
                return p_func();
            }
            return (T) p_control.Dispatcher.Invoke(p_func);
        }

        #endregion
    }
}