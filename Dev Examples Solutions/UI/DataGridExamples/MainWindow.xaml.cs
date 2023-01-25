#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataGridExamples.Data;

#endregion

namespace DataGridExamples
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
            DataContext = this;
            CarsData = new ObservableCollection<CarData>();

            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "56392635",
                Manufacturer = ManufacturerEnum.Bmw,
                BodyColor = Colors.LightBlue,
                ClientId = 8364,
                Readiness = true,
                Status = StatusEnum.OnRoad
            });
            CarsData.Add(new CarData()
            {
                PlateNumber = "23673576",
                Manufacturer = ManufacturerEnum.Subaru,
                BodyColor = Colors.Indigo,
                ClientId = 2854,
                Readiness = false,
                Status = StatusEnum.InGarage
            });
        }

        #endregion

        #region Public Properties

        public ObservableCollection<CarData> CarsData { get; set; }

        #endregion

        #region Private Methods

        private void OnReadinessCheckedChanged(object p_sender, RoutedEventArgs p_e)
        {
        }

        private void OnTextInput(object p_sender, KeyEventArgs p_e)
        {
            throw new NotImplementedException();
        }

        private void OnTextInput(object p_sender, KeyboardFocusChangedEventArgs p_e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}