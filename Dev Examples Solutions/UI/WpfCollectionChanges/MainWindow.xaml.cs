#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

#endregion

namespace WpfCollectionChanges
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            VehiclesList = new ObservableCollection<Vehicle>();
            VehiclesList.Add(new Vehicle() {Number = 892174278, Name = "Opel"});
            VehiclesList.Add(new Vehicle() {Number = 73464273, Name = "Subaru"});
            VehiclesList.Add(new Vehicle() {Number = 67823236, Name = "BMW"});
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Vehicle> VehiclesList { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Private Methods

        private void DataGrid_OnSelectedCellsChanged(object p_sender, SelectedCellsChangedEventArgs p_e)
        {
            DataGrid grid = p_sender as DataGrid;
            Vehicle vehicle = grid.SelectedItem as Vehicle;
            vehicle.Number += 1;
        }

        #endregion
    }

    public class Vehicle : INotifyPropertyChanged
    {
        #region Public Properties

        public int Number
        {
            get { return m_number; }
            set
            {
                m_number = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Number"));
            }
        }

        public string Name { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private int m_number;

        #endregion
    }
}