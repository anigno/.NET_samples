#region

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

#endregion

namespace WpfDataGridCustomTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        #endregion
    }
}