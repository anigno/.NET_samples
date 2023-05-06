#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Windows.Threading;

#endregion

namespace WpfUiAndThreads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            MyItems = new ObservableCollection<MyItem>();
            Task.Factory.StartNew(() =>
            {
                for (int a = 0; a < 999; a++)
                {
                    MyItem myItem = new MyItem() {NumberFirst = DateTime.Now.Ticks, NumberSecond = DateTime.Now.Millisecond, NumberThird = 17};
                    RunOverDispatcher(() => MyItems.Add(myItem));
                    PropertyChanged(this, new PropertyChangedEventArgs("MyItems"));
                    Task.Delay(1).Wait();
                }
            });
        }

        #endregion

        #region Public Properties

        public ObservableCollection<MyItem> MyItems { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        #endregion

        #region Private Methods

        private void RunOverDispatcher(Action p_action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                //Already run from dispatcher
                p_action();
                return;
            }
            Application.Current.Dispatcher.BeginInvoke(p_action, DispatcherPriority.Normal);
        }

        #endregion
    }

    public class MyItem
    {
        #region Public Properties

        public double NumberFirst { get; set; }
        public double NumberSecond { get; set; }
        public double NumberThird { get; set; }

        #endregion
    }
}