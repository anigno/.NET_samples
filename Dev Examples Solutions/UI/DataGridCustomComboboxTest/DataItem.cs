#region

using System.ComponentModel;

#endregion

namespace WpfDataGridCustomTest
{
    public enum NumberEnum
    {
        EnumOne,
        EnumTwo,
        EnumThree,
        EnumFour
    }

    public class DataItem : INotifyPropertyChanged
    {
        #region Public Properties

        public uint Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        public NumberEnum Number
        {
            get { return m_number; }
            set
            {
                m_number = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Number"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private uint m_id;
        private NumberEnum m_number;

        #endregion
    }
}