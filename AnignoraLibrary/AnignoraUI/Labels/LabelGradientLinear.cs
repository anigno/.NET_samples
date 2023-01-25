using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Drawing;

namespace AnignoraUI.Labels
{
    public class LabelGradientLinear : LabelGradientBase
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " LabelGradientLinear";
        private int m_gradientAngle = 30;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public LabelGradientLinear()
        {
            GradientColorFirst = Color.Black;
            GradientColorSecond = Color.Silver;
            base.ForeColor = Color.White;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public int GradientAngle
        {
            get { return m_gradientAngle; }
            set
            {
                m_gradientAngle = value;
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(-1, -1, Width+1, Height+1);
            LinearGradientBrush brush = new LinearGradientBrush(rect, GradientColorFirst,GradientColorSecond, GradientAngle);
            e.Graphics.FillRectangle(brush,rect);
            base.OnPaint(e);
        }

		#endregion (------  Protected Methods  ------)
    }
}
