#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

#endregion

namespace DataGridWithComboBoxExample
{
    public class BrendEnums : List<BrendEnum>
    {
        #region Constructors

        public BrendEnums()
        {
            foreach (BrendEnum v in Enum.GetValues(typeof (BrendEnum)))
            {
                Add(v);
            }
        }

        #endregion
    }

    public class YearsDictionaryResource : Dictionary<uint, string>
    {
        #region Constructors

        public YearsDictionaryResource()
        {
            foreach (KeyValuePair<uint, string> pair in MainWindow.YearsDictionary)
            {
                Add(pair.Key, pair.Value);
            }
        }

        #endregion
    }


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
            InitData();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<CarData> CarDataCollection { get; set; }

        public uint Year { get; private set; }

        public static Dictionary<uint, string> YearsDictionary { get; private set; }

        #endregion

        #region Private Methods

        private void InitData()
        {
            YearsDictionary = new Dictionary<uint, string>();
            for (uint a = 2000; a < 2020; a++)
            {
                YearsDictionary.Add(a, (a - 2000).ToString("00"));
            }


            CarDataCollection = new ObservableCollection<CarData>();
            CarDataCollection.Add(new CarData() {Brend = BrendEnum.Bmw, LicenseNumber = "4352352343", Year = 2000});
            CarDataCollection.Add(new CarData() {Brend = BrendEnum.Subaru, LicenseNumber = "2323523523", Year = 2014});
            CarDataCollection.Add(new CarData() {Brend = BrendEnum.Toyota, LicenseNumber = "9247627282", Year = 2010});
            CarDataCollection.Add(new CarData() {Brend = BrendEnum.Bmw, LicenseNumber = "5452352347", Year = 2004});
            CarDataCollection.Add(new CarData() {Brend = BrendEnum.Toyota, LicenseNumber = "4733468243", Year = 2001});
        }

        #endregion
    }
}