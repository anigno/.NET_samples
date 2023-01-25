using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AnignoraUI.Labels
{
    public class LabelDateTime : Label
    {
        private readonly Timer m_timer = new Timer();

        public LabelDateTime()
        {
            m_timer.Interval = 1000;
            m_timer.Tick += mTimerTick;
            m_timer.Start();
            base.Text = ". . .";
        }

        void mTimerTick(object sender, EventArgs e)
        {
            Text = DateTime.Now.ToString(FormatString);
        }

        [Category(" LabelDateTime")]
        public string FormatString { get; set; }
    }
}