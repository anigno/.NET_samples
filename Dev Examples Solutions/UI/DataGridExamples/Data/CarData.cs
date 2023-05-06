#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

#endregion

namespace DataGridExamples.Data
{
    public enum ManufacturerEnum
    {
        Subaru,
        Toyota,
        Honda,
        Bmw
    }

    public enum StatusEnum
    {
        Unknown,
        InGarage,
        OnRoad
    }

    public class CarData : INotifyPropertyChanged
    {
        #region Constructors

        public CarData()
        {
            UpdateTime = DateTime.Now;
        }

        #endregion

        #region Public Methods

        public static CarData Add(ObservableCollection<CarData> p_carsData, string p_plateNumber)
        {
            CarData carData = new CarData() {PlateNumber = p_plateNumber};
            p_carsData.Add(carData);
            return carData;
        }

        #endregion

        #region Public Properties

        public string PlateNumber { get; set; }
        public Color BodyColor { get; set; }
        public ManufacturerEnum Manufacturer { get; set; }

        public bool Readiness
        {
            get { return m_readiness; }
            set
            {
                m_readiness = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Readiness"));
            }
        }

        public DateTime UpdateTime { get; set; }
        public int ClientId { get; set; }
        public StatusEnum Status { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private bool m_readiness;

        #endregion
    }
}