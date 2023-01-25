using System;
using System.Windows.Forms;
using AnignoFileSync.Data;
using AnignoraDataTypes.DataManager;
using LoggingProvider;

namespace AnignoFileSync2010
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.LogInfo("Application started " + Application.ProductVersion);
            DataManager<ApplicationData>.DataFilePath = Environment.CurrentDirectory;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
