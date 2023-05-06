#region

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace WpfControls.CommunicationStatusControls
{
    /// <summary>
    /// Interaction logic for AgCommunicationStatus.xaml
    /// </summary>
    public partial class AgCommunicationStatus : UserControl
    {
        #region Constructors

        public AgCommunicationStatus()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        public bool ConnectionStatus
        {
            get { return (bool) GetValue(ConnectionStatusProperty); }
            set { SetValue(ConnectionStatusProperty, value); }
        }

        public bool CommunicationStatus
        {
            get { return (bool) GetValue(CommunicationStatusProperty); }
            set { SetValue(CommunicationStatusProperty, value); }
        }

        #endregion

        #region Private Methods

        private static void PropertyChangedCallback(DependencyObject p_dependencyObject, DependencyPropertyChangedEventArgs p_dependencyPropertyChangedEventArgs)
        {
            if ((bool) p_dependencyPropertyChangedEventArgs.NewValue)
            {
                Task.Factory.StartNew(() =>
                {
                    Task.Delay(500).Wait();
                    Action action = () => { ((AgCommunicationStatus) p_dependencyObject).CommunicationStatus = false; };
                    Application.Current.Dispatcher.BeginInvoke(action);
                });
            }
        }

        private static bool CommunicationStatusValidateValueCallback(object p_value)
        {
            return true;
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty ConnectionStatusProperty = DependencyProperty.Register("ConnectionStatus", typeof (bool), typeof (AgCommunicationStatus), new PropertyMetadata(default(bool)));

        //Must use FrameworkPropertyMetadataOptions with two way binding for this
        public static readonly DependencyProperty CommunicationStatusProperty = DependencyProperty.Register(
            "CommunicationStatus",
            typeof (bool),
            typeof (AgCommunicationStatus),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback), CommunicationStatusValidateValueCallback);

        #endregion
    }
}