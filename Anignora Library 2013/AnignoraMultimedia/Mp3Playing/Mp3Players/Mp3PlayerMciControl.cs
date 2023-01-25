using System;
using System.IO;
using System.Windows.Forms;

namespace AnignoraMultimedia.Mp3Playing.Mp3Players
{
    public partial class Mp3PlayerMciControl : UserControl
    {
		#region (------  Fields  ------)

        private bool m_isPlaying;
        private readonly Mp3PlayerMci m_player=new Mp3PlayerMci();

		#endregion (------  Fields  ------)

		#region (------  Events  ------)

        public event Action OnPlayingStoped;

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        public Mp3PlayerMciControl()
        {
            InitializeComponent();
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public string  SongFileName
        {
            get { return m_player.FileName; }
            set { LoadSong(value); }
        }

        public bool IsPlaying
        {
            get { return m_isPlaying; }
            private set { m_isPlaying = value; }
        }

        #endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

        public void LoadSong(string p_songFileName)
        {
            if (!File.Exists(p_songFileName)) return;
            m_player.Open(p_songFileName);
        }

        public void Pause()
        {
            m_player.Pause();
            IsPlaying = false;
        }

        public void Play()
        {
            m_player.Play();
            IsPlaying = true;
        }

        public void SetPlayingPosition(int miliseconds)
        {
            m_player.Seek((ulong) miliseconds);
        }

        /// <summary>
        /// Sets All Volume 0-1000
        /// </summary>
        /// <param name="volume">0 to 1000</param>
        public void SetVolume(int volume)
        {
            m_player.VolumeAll = volume;
        }

        public void Stop()
        {
            m_player.Stop();
            IsPlaying = false;
            RaiseOnPlayingStoped();
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void buttonFwd_Click(object sender, EventArgs e)
        {
            SetPlayingPosition((int) (m_player.CurrentPosition+10000));
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            Pause();
            IsPlaying = !IsPlaying;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void buttonRew_Click(object sender, EventArgs e)
        {
            SetPlayingPosition((int) (m_player.CurrentPosition-10000));
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private string GetTimeString(ulong sec)
        {
            return string.Format("{0:0}:{1:00}", sec/60/1000, sec/1000%60);
        }

        private void progressBarTime_MouseUp(object sender, MouseEventArgs e)
        {
            SetPlayingPosition(progressBarTime.LastMouseUpValue);
        }

        private void RaiseOnPlayingStoped()
        {
            if (OnPlayingStoped != null) OnPlayingStoped();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (IsPlaying)
            {
                progressBarTime.BarText = GetTimeString(m_player.CurrentPosition) + "/" + GetTimeString(m_player.AudioLength);
                progressBarTime.Maximum = (int) m_player.AudioLength;
                progressBarTime.Value = (int) m_player.CurrentPosition;
                if(m_player.CurrentPosition>=m_player.AudioLength)Stop();
                switch (buttonPlay.Text)
                {
                    case ">":
                        buttonPlay.Text = " >";
                        break;
                    case " >":
                        buttonPlay.Text = "  >";
                        break;
                    case "  >":
                        buttonPlay.Text = ">";
                        break;
                }
            }
        }

		#endregion (------  Private Methods  ------)
    }
}
