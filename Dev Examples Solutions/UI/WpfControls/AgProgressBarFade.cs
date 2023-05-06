#region

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace WpfControls
{
    public class AgProgressBarFade : ProgressBar
    {
        #region Constructors

        public AgProgressBarFade()
        {
            m_fadeTimer = new Timer(p_state =>
            {
                Action action = () =>
                {
                    if (Value > Minimum)
                    {
                        Value *= 0.95;
                    }
                };
                Application.Current.Dispatcher.BeginInvoke(action);
            }, null, 100, 100);
        }

        #endregion

        #region Public Properties

        public bool Trigger
        {
            get { return (bool) GetValue(TriggerProperty); }
            set { SetValue(TriggerProperty, value); }
        }

        #endregion

        #region Private Methods

        private static void OnPropertyChangedCallback(DependencyObject p_o, DependencyPropertyChangedEventArgs p_args)
        {
            AgProgressBarFade item = (AgProgressBarFade) p_o;
            if (item.Trigger == false) return;
            item.Value = item.Maximum;
            item.Trigger = false;
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty TriggerProperty = DependencyProperty.Register(
            "Trigger", typeof (bool), typeof (AgProgressBarFade), new FrameworkPropertyMetadata(default(bool),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChangedCallback), p_value => true);

        private readonly Timer m_fadeTimer;

        #endregion

        ~AgProgressBarFade()
        {
            m_fadeTimer.Dispose();
        }
    }
}