using System.Drawing;
using System.Drawing.Drawing2D;

namespace AnignoraUI.Labels
{
    public class LabelGradientPath : LabelGradientBase
    {
		#region (------  Constructors  ------)
        public LabelGradientPath()
        {
            base.ForeColor = Color.LightGray;
            GradientColorFirst = Color.Gray;
            GradientColorSecond = Color.Black;
        }

		#endregion (------  Constructors  ------)

		#region (------  Protected Methods  ------)

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF rect = new RectangleF(-1, -1, Width + 1, Height + 1);
            GraphicsPath path=new GraphicsPath();
            path.AddRectangle(rect);
            PathGradientBrush brush=new PathGradientBrush(path);
            brush.CenterColor = GradientColorFirst;
            brush.SurroundColors=new []{GradientColorSecond};
            brush.CenterPoint = new PointF(Width/2f, Height/2f);
            e.Graphics.FillRectangle(brush, rect);
            base.OnPaint(e);
        }

		#endregion (------  Protected Methods  ------)
    }
}
