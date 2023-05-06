#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

#endregion

namespace WPF.Themes.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Private Methods

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is InvalidOperationException)
                e.Handled = true;
        }

        #endregion
    }
}