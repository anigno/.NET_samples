#region

using System;
using System.Windows;

#endregion

namespace BlCommon
{
    public static class UnhandledExceptionManager
    {
        #region Public Methods

        public static void Register(Action<Exception> p_appDomainExceptionHanler, Action<Exception> p_dispatcherExceptionHandler)
        {
            AppDomain.CurrentDomain.UnhandledException += (p_o, p_args) => p_appDomainExceptionHanler((Exception) p_args.ExceptionObject);
            Application.Current.DispatcherUnhandledException += (p_o, p_args) => p_dispatcherExceptionHandler(p_args.Exception);
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (p_sender, p_args) => { };
        }

		        public static void RegisterUnhandledExceptions(Action<Exception> appDomainExceptionHandler, Action<Exception> dispatcherExceptionHandler, Action<Exception> unobservedTaskException)
        {
            AppDomain.CurrentDomain.UnhandledException += (p_o, args) => appDomainExceptionHandler((Exception)args.ExceptionObject);
            Application.Current.DispatcherUnhandledException += (p_o, p_args) => dispatcherExceptionHandler(p_args.Exception);
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (sender, args) => unobservedTaskException(args.Exception);
        }

        #endregion
    }
}