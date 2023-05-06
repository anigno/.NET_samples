#region

using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

namespace WpfDataGridCustomTest
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            DataItemsCollection = new ObservableCollection<DataItem>();
            DataItemsCollection.Add(new DataItem() {Id = 100, Number = NumberEnum.EnumFour});
            DataItemsCollection.Add(new DataItem() {Id = 101, Number = NumberEnum.EnumOne});
            DataItemsCollection.Add(new DataItem() {Id = 102, Number = NumberEnum.EnumTwo});
        }

        #endregion

        #region Public Properties

        public DataItem DataGridSelectedItem
        {
            get { return m_dataGridSelectedItem; }
            set
            {
                m_dataGridSelectedItem = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DataGridSelectedItem"));
                NumberDescriptor = value.Id%2;
            }
        }

        public ObservableCollection<DataItem> DataItemsCollection { get; set; }

        public static ObservableCollection<NumberEnum> MyEnumTypeValues
        {
            get
            {
                ObservableCollection<NumberEnum> numberEnums = new ObservableCollection<NumberEnum>();
                if (NumberDescriptor == 0)
                {
                    numberEnums.Add(NumberEnum.EnumTwo);
                    numberEnums.Add(NumberEnum.EnumFour);
                }
                if (NumberDescriptor == 1)
                {
                    numberEnums.Add(NumberEnum.EnumOne);
                    numberEnums.Add(NumberEnum.EnumThree);
                }
                return numberEnums;
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private DataItem m_dataGridSelectedItem;

        #endregion

        private static uint NumberDescriptor { get; set; }
    }
}