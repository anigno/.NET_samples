using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AnignoraUI.Labels
{
    public class LabelGradientEllipse : LabelGradientBase
    {
        private float m_gradientEllipseScale = 1;
        private const string CATEGORY_STRING = " LabelGradientPath";

        public LabelGradientEllipse()
        {
            base.ForeColor = Color.LightGray;
            GradientColorFirst = Color.Gray;
            GradientColorSecond = Color.Black;
        }

        [Category(CATEGORY_STRING)]
        public float GradientEllipseScale
        {
            get { return m_gradientEllipseScale; }
            set
            {
                if (value < 0.2) return;
                m_gradientEllipseScale = value;
                Refresh();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(-Width*GradientEllipseScale, -Height*GradientEllipseScale, Width + 2*GradientEllipseScale*Width, Height + 2*GradientEllipseScale*Height);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(rect);
            PathGradientBrush brush = new PathGradientBrush(path);
            brush.CenterColor = GradientColorFirst;
            brush.SurroundColors = new[] { GradientColorSecond };
            brush.CenterPoint = new PointF(Width / 2f, Height / 2f);
            e.Graphics.FillRectangle(brush, rect);
            base.OnPaint(e);
        }
    }
}
