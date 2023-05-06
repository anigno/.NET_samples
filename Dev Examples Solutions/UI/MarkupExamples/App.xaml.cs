#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#endregion

namespace MarkupExamples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constructors

        public App()
        {
            ViewModelBinder.AddViewModel("TheViewModel", new TheViewModel());

            StringsMarkupExtension.MyStrings.Add("Added in App");

            Window mainWindow = new MainWindow();
            mainWindow.Activated += mainWindow_Activated;
            mainWindow.Closing += mainWindow_Closing;
            mainWindow.Loaded += mainWindow_Loaded;
            mainWindow.Show();
        }

        #endregion

        #region Private Methods

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void mainWindow_Activated(object sender, EventArgs e)
        {
        }

        #endregion
    }
}