#region

using System.ComponentModel;

#endregion

namespace DataGridWithComboBoxExample
{
    public enum BrendEnum
    {
        Subaru,
        Toyota,
        Bmw
    }

    public class CarData : INotifyPropertyChanged
    {
        #region Public Properties

        public BrendEnum Brend
        {
            get { return m_brend; }
            set
            {
                m_brend = value;
                PropertyChanged(this, new PropertyChangedEventArgs("BrendEnum"));
            }
        }

        public string LicenseNumber
        {
            get { return m_licenseNumber; }
            set
            {
                m_licenseNumber = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LicenseNumber"));
            }
        }

        public uint Year
        {
            get { return m_year; }
            set
            {
                m_year = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Year"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private BrendEnum m_brend;
        private string m_licenseNumber;
        private uint m_year;

        #endregion
    }
}