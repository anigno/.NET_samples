using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AnignoraCommonAndHelpers.Helpers
{
    public static class EventHalper
    {
        #region Public Methods

        public static void ClearEventRegistration(EventHandler p_eventHandler)
        {
            Delegate[] delegates = p_eventHandler.GetInvocationList();
            for (int a = 0; a < delegates.Length; a++)
            {
                p_eventHandler -= (EventHandler) delegates[a];
            }
        }


        public static void RegisterUnhandledExceptionHandlers(Action<UnhandledExceptionEventArgs> p_domainExceptionAction, Action<ThreadExceptionEventArgs> p_threadExceptionAction, Action<UnobservedTaskExceptionEventArgs> p_taskUnobservedExceptionHandlerAction)
        {
            AppDomain.CurrentDomain.UnhandledException += delegate(object p_sender, UnhandledExceptionEventArgs p_args) { p_domainExceptionAction(p_args); };
            Application.ThreadException += delegate(object p_sender, ThreadExceptionEventArgs p_args) { p_threadExceptionAction(p_args); };
            TaskScheduler.UnobservedTaskException += (p_sender, p_args) => p_taskUnobservedExceptionHandlerAction(p_args);
        }

        #endregion
    }
}