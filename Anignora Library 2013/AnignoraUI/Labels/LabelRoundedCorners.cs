using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AnignoraUI.Common;

namespace AnignoraUI.Labels
{
    public class LabelRoundedCorners : Label
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " LabelRoundedCorners";
        private int m_cornerSize = 3;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public LabelRoundedCorners()
        {
            base.BackColor = Color.White;
            base.ForeColor = Color.Black;
            base.TextAlign = ContentAlignment.MiddleCenter;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public int CornerSize
        {
            get { return m_cornerSize; }
            set
            {
                if (value < 1) return;
                m_cornerSize = value;
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath gPath = UiHelper.GetRoundedCornersGraphicsPath(Width, Height, CornerSize);
            Region = new Region(gPath);
            base.OnPaint(e);
        }

		#endregion (------  Protected Methods  ------)
    }
}
