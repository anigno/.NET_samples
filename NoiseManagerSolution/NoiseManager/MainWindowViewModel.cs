using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using AnignoraUI.Common;
using log4net;

namespace NoiseManager
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            m_timerPlaying = new Timer(TimerPlayingCallback);
            m_mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
            WindowHeader = "Noise Manager ";
        }

        #endregion

        #region Public Methods

        public static void RunOverUiThread(Action p_action)
        {
            bool isCheckAccess = Application.Current.Dispatcher.CheckAccess();
            if (!isCheckAccess)
            {
                Application.Current.Dispatcher.Invoke(p_action, DispatcherPriority.Normal);
            }
        }

        #endregion

        #region Public Properties

        public bool IsChecked00
        {
            get { return m_isChecked00; }
            set
            {
                m_isChecked00 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked00"));
            }
        }

        public bool IsChecked02
        {
            get { return m_isChecked02; }
            set
            {
                m_isChecked02 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked02"));
            }
        }

        public bool IsChecked04
        {
            get { return m_isChecked04; }
            set
            {
                m_isChecked04 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked04"));
            }
        }

        public bool IsChecked06
        {
            get { return m_isChecked06; }
            set
            {
                m_isChecked06 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked06"));
            }
        }

        public bool IsChecked08
        {
            get { return m_isChecked08; }
            set
            {
                m_isChecked08 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked08"));
            }
        }

        public bool IsChecked10
        {
            get { return m_isChecked10; }
            set
            {
                m_isChecked10 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked10"));
            }
        }

        public bool IsChecked12
        {
            get { return m_isChecked12; }
            set
            {
                m_isChecked12 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked12"));
            }
        }

        public bool IsChecked14
        {
            get { return m_isChecked14; }
            set
            {
                m_isChecked14 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked14"));
            }
        }

        public bool IsChecked16
        {
            get { return m_isChecked16; }
            set
            {
                m_isChecked16 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked16"));
            }
        }

        public bool IsChecked18
        {
            get { return m_isChecked18; }
            set
            {
                m_isChecked18 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked18"));
            }
        }

        public bool IsChecked20
        {
            get { return m_isChecked20; }
            set
            {
                m_isChecked20 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked20"));
            }
        }

        public bool IsChecked22
        {
            get { return m_isChecked22; }
            set
            {
                m_isChecked22 = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsChecked22"));
            }
        }

        public bool IsOn
        {
            get { return m_isOn; }
            set
            {
                m_isOn = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsOn"));
                if (m_isOn)
                {
                    Status = StatusEnum.Selecting;
                    m_timerPlaying.Change(0, 1000);
                }
                else
                {
                    m_timerPlaying.Change(-1, -1);
                    Status = StatusEnum.Stopped;
                    m_mediaPlayer.Stop();
                }
            }
        }

        public StatusEnum Status
        {
            get { return m_status; }
            private set
            {
                if (Status == value) return;
                Logger.DebugFormat("Status will change from: {0} to: {1}", Status, value);
                m_status = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        public string TextPlaying
        {
            get { return m_textPlaying; }
            set
            {
                m_textPlaying = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TextPlaying"));
            }
        }

        public string Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Progress"));
            }
        }

        public ComboBoxItem DelaySelected
        {
            get { return m_delaySelected; }
            set
            {
                if (value == null || value.Content == null) return;
                m_delaySelected = value;
                m_delayPlaying = int.Parse(value.Content.ToString());
                PropertyChanged(this, new PropertyChangedEventArgs("DelaySelected"));
            }
        }

        public string WindowHeader
        {
            get { return m_windowHeader; }
            set
            {
                m_windowHeader = value;
                PropertyChanged(this, new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        #region Private Methods

        private void MediaPlayerOnMediaEnded(object p_sender, EventArgs p_eventArgs)
        {
            Status = StatusEnum.EndPlaying;
        }

        private void TimerPlayingCallback(object p_state)
        {
            WindowHeader = $"Noise Manager {DateTime.Now.ToString("HH:mm:ss")}";
            try
            {
                switch (Status)
                {
                    case StatusEnum.Stopped:
                        break;
                    case StatusEnum.Playing:
                        if (!CheckTime())
                        {
                            Status = StatusEnum.Selecting;
                            CrossThreadActivities.Do(() =>
                            {
                                m_mediaPlayer.Stop();
                            });
                        }
                        RunOverUiThread(() => { Progress = $"{(int)m_mediaPlayer.Position.TotalSeconds + 1} / {(int)m_mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds}"; });
                        break;
                    case StatusEnum.EndPlaying:
                        m_counter = 0;
                        Status = StatusEnum.Waiting;
                        break;
                    case StatusEnum.Waiting:
                        if (m_counter++ >= m_delayPlaying * 60) Status = StatusEnum.Selecting;
                        Progress = $"{m_counter} / {m_delayPlaying * 60}";
                        break;
                    case StatusEnum.Selecting:
                        if (!CheckTime()) return;
                        Status = StatusEnum.Playing;
                        TextPlaying = RandomSelectNoiseFile();
                        RunOverUiThread(() =>
                        {
                            m_mediaPlayer.Open(new Uri(TextPlaying));
                            m_mediaPlayer.Play();
                            m_counter = 0;
                        });
                        break;
                }
            }
            catch (Exception exception)
            {
                Logger.ErrorFormat("Last state={0} [{1}] [{2}]", Status, exception.Message, exception);
            }
        }

        private bool CheckTime()
        {
            if (m_isChecked00 && DateTime.Now.Hour >= 00 && DateTime.Now.Hour < 02) return true;
            if (m_isChecked02 && DateTime.Now.Hour >= 02 && DateTime.Now.Hour < 04) return true;
            if (m_isChecked04 && DateTime.Now.Hour >= 04 && DateTime.Now.Hour < 06) return true;
            if (m_isChecked06 && DateTime.Now.Hour >= 06 && DateTime.Now.Hour < 08) return true;
            if (m_isChecked08 && DateTime.Now.Hour >= 08 && DateTime.Now.Hour < 10) return true;
            if (m_isChecked10 && DateTime.Now.Hour >= 10 && DateTime.Now.Hour < 12) return true;
            if (m_isChecked12 && DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 14) return true;
            if (m_isChecked14 && DateTime.Now.Hour >= 14 && DateTime.Now.Hour < 16) return true;
            if (m_isChecked16 && DateTime.Now.Hour >= 16 && DateTime.Now.Hour < 18) return true;
            if (m_isChecked18 && DateTime.Now.Hour >= 18 && DateTime.Now.Hour < 20) return true;
            if (m_isChecked20 && DateTime.Now.Hour >= 20 && DateTime.Now.Hour < 22) return true;
            if (m_isChecked22 && DateTime.Now.Hour >= 22 && DateTime.Now.Hour < 24) return true;
            return false;
        }

        private string RandomSelectNoiseFile()
        {
            string[] files = Directory.GetFiles("NoiseFiles");
            int a = m_random.Next(0, files.Length);
            string absolutePath = Path.GetFullPath(files[a]);
            return absolutePath;
        }

        #endregion

        #region Fields

        public static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int m_delayPlaying;
        private int m_counter;
        private readonly Random m_random = new Random((int)DateTime.Now.Ticks);
        private readonly MediaPlayer m_mediaPlayer = new MediaPlayer();
        private readonly Timer m_timerPlaying;
        private bool m_isChecked00;
        private bool m_isChecked02;
        private bool m_isChecked04;
        private bool m_isChecked06;
        private bool m_isChecked08;
        private bool m_isChecked10;
        private bool m_isChecked12;
        private bool m_isChecked14;
        private bool m_isChecked16;
        private bool m_isChecked18;
        private bool m_isChecked20;
        private bool m_isChecked22;
        private bool m_isOn;
        private string m_textPlaying;
        private string m_progress;
        private ComboBoxItem m_delaySelected;
        private StatusEnum m_status;
        private string m_windowHeader;

        #endregion

        public enum StatusEnum
        {
            Stopped = 0,
            Selecting,
            Playing,
            EndPlaying,
            Waiting
        }
    }
}