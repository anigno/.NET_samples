#region

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace WpfControlsTest
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
            SelectedObject = new ListBox();

            Action action = () =>
            {
                OuterConnectionStatus = !OuterConnectionStatus;
                OuterCommunicationStatus = !OuterCommunicationStatus;
                MyListBox.Items.Add(new ListBoxItem() {Content = DateTime.Now.ToLongTimeString()});
                MyProgressBarFade.Trigger = true;
            };
            //m_timer = new Timer(p_state => Application.Current.Dispatcher.BeginInvoke(action), null, 500, 1000);

            MyProgressBarFade.Trigger = true;
        }

        #endregion

        #region Public Properties

        public bool OuterConnectionStatus
        {
            get { return m_outerConnectionStatus; }
            set
            {
                m_outerConnectionStatus = value;
                PropertyChanged(this, new PropertyChangedEventArgs("OuterConnectionStatus"));
            }
        }

        public bool OuterCommunicationStatus
        {
            get { return m_outerCommunicationStatus; }
            set
            {
                m_outerCommunicationStatus = value;
                PropertyChanged(this, new PropertyChangedEventArgs("OuterCommunicationStatus"));
            }
        }

        public object SelectedObject
        {
            get { return m_selectedObject; }
            set
            {
                m_selectedObject = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedObject"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Fields

        private Timer m_timer;
        private bool m_outerConnectionStatus;
        private bool m_outerCommunicationStatus;
        private object m_selectedObject = new object();

        #endregion
    }
}