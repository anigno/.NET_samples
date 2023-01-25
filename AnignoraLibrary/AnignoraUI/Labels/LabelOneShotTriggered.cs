using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace AnignoraUI.Labels
{
    public class LabelOneShotTriggered:Label
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " ";
        private Color m_normalColor;
        private readonly System.Threading.Timer m_timer;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public LabelOneShotTriggered()
        {
            m_timer = new System.Threading.Timer(timerCallBack);
            TriggeredColor = Color.Red;
            TriggeredIntervalMs = 1000;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING),Browsable(false)]
        public bool Triggered { get; private set; }

        [Category(CATEGORY_STRING)]
        public Color TriggeredColor { get; set; }

        [Category(CATEGORY_STRING)]
        public int TriggeredIntervalMs { get; set; }

		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

        public void Trigger()
        {
            if(!Triggered) m_normalColor = BackColor;
            Triggered = true;
            BackColor = TriggeredColor;
            m_timer.Change( TriggeredIntervalMs,int.MaxValue);
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void timerCallBack(object p_object)
        {
            m_timer.Change(int.MaxValue, int.MaxValue);
            Triggered = false;
            BackColor = m_normalColor;
        }

		#endregion (------  Private Methods  ------)
    }
}
