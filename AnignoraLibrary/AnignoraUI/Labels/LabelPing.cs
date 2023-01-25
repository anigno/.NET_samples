using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace AnignoraUI.Labels
{
    public class LabelPing : Label
    {
		#region (------  Constants  ------)
        public const string CATEGORY_STRING = " LabelPing";
		#endregion (------  Constants  ------)

		#region (------  Fields  ------)
        private Ping m_pinger = new Ping();
        private System.Threading.Timer m_timer ;
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        public event PingCompletedEventHandler PingCompleted = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public LabelPing()
        {
            PingHost = "128.0.0.1";
            PingIntervalMs = 10000;
            m_timer = new System.Threading.Timer(onTimerCallback);
            m_timer.Change(100, PingIntervalMs);
            ColorDisconnected = Color.Red;
            ColorConnected = Color.LightGreen;
            base.BackColor = ColorDisconnected;
            m_pinger.PingCompleted += onPingerPingCompleted;
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        [Category(CATEGORY_STRING)]
        public bool Active { get; set; }

        [Category(CATEGORY_STRING)]
        public Color ColorConnected { get; set; }

        [Category(CATEGORY_STRING)]
        public Color ColorDisconnected { get; set; }

        [Category(CATEGORY_STRING)]
        public string PingHost { get; set; }

        [Category(CATEGORY_STRING)]
        public int PingIntervalMs { get; set; }
		#endregion (------  Properties  ------)

		#region (------  Events Handlers  ------)
        void onPingerPingCompleted(object sender, PingCompletedEventArgs e)
        {
            int nextPingIntervalMs;
            //e.reply is null when the host name is wronge
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                BackColor = ColorConnected;
                nextPingIntervalMs = PingIntervalMs;
            }
            else
            {
                BackColor = ColorDisconnected;
                nextPingIntervalMs = PingIntervalMs/4;
            }
            PingCompleted(this, e);
            m_timer.Change(nextPingIntervalMs, nextPingIntervalMs);
        }

        private void onTimerCallback(object p_state)
        {
            try
            {
                m_timer.Change(int.MaxValue, int.MaxValue);
                m_pinger.SendAsync(PingHost, PingIntervalMs);
            }
            catch (Exception ex)
            {
            }
        }
		#endregion (------  Events Handlers  ------)
    }
}
