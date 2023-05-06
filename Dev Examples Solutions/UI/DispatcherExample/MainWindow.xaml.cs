#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace DispatcherExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            m_mySubject.Subscribe(p_s => { MyLabel = p_s; });
            m_mySubject.ObserveOnDispatcher().Subscribe(p_s => { MyCollection.Add("ObserveOnDispatcher " + p_s); });
            m_mySubject.ObserveOn(System.Windows.Threading.Dispatcher.CurrentDispatcher).Subscribe(
                p_s => MyCollection.Add("System.Windows.Threading.Dispatcher.CurrentDispatcher " + p_s));
            m_mySubject.ObserveOn(System.Windows.Application.Current.Dispatcher).Subscribe(
                p_s => MyCollection.Add("System.Windows.Application.Current.Dispatcher " + p_s));

            m_timer1 = new Timer(p_state =>
            {
                RunOverDispatcher(() =>
                {
                    MyCollection.Add(MyLabel);
                    MyLabel = DateTime.Now.ToLongTimeString();
                });
            });
            m_timer2 = new Timer(p_state => { m_mySubject.OnNext("Subject OnNext"); });

            m_timer1.Change(100, 1000);
            m_timer2.Change(600, 1000);
        }

        #endregion

        #region Public Properties

        public ObservableCollection<string> MyCollection
        {
            get { return m_collection; }
        }

        public string MyLabel
        {
            get { return m_label; }
            set
            {
                m_label = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MyLabel"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

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

        #region Fields

        private Timer m_timer1;
        private Timer m_timer2;

        private readonly Subject<string> m_mySubject = new Subject<string>();

        private readonly ObservableCollection<string> m_collection = new ObservableCollection<string>();

        private string m_label = "My Label";

        #endregion
    }
}