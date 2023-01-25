using System;
using System.Windows.Controls;

namespace WpfServices
{
    public static class CrossThreadedActivities
    {
        public static void Do(Control p_control,Action p_action)
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
    }
}
