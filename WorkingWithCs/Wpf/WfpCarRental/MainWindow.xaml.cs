using System;
using System.Collections.Generic;
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
using WfpCarRental.BL;

namespace WfpCarRental
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext=null;
            CarRentalManager carRentalManager= new CarRentalManager();
            carRentalManager.StartNewCarRentalTimer(1234, 5);
            carRentalManager.StartNewCarRentalTimer(3456, 6);
            carRentalManager.StartNewCarRentalTimer(2345, 7);
        }
    }
}
