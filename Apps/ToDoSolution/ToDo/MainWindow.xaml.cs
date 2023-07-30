using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using LoggingProvider;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _ = new LoggingInitiator(logsPath: "d:/dev/logs", isUseConsole: true);
            ILog logger = LogManager.GetLogger("main_logger");
            logger.Info("app started");
            InitializeComponent();
        }
    }
}
