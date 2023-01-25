using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
using System.Xml;
using log4net;
using log4net.Config;

namespace NoiseManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitLogger();
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void InitLogger()
        {
            XmlElement element = (XmlElement) ConfigurationManager.GetSection("log4net");
            XmlConfigurator.Configure(element);
            Logger.Info("Logger initialized");
        }

        #endregion

        #region Fields

        public static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}