using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AnignoraUI.Common
{
    public class UiHelper
    {
		#region (------  Fields  ------)

        public static readonly Color ColorActivityLightOrange = Color.FromArgb(255, 224, 192);
        public static readonly Color ColorIsValidLightGreen = Color.FromArgb(192, 255, 192);
        public static readonly Color ColorNotValidLightRed = Color.FromArgb(255, 192, 192);

		#endregion (------  Fields  ------)

		#region (------  Public Methods  ------)

        public static void DrawShiningBall(Graphics g, float x, float y, float size, Color color, int centerPercentage = 40)
        {
            GraphicsPath pth = new GraphicsPath();
            pth.AddEllipse(x, y, size, size);
            PathGradientBrush pgb = new PathGradientBrush(pth);
            pgb.CenterColor = Color.White;
            pgb.CenterPoint = new PointF(x + size * centerPercentage / 100, x + size * centerPercentage / 100);
            pgb.SurroundColors = new[] { color };
            g.FillPath(pgb, pth);
        }

        public static void DrawStringCenterPoint(string s, Graphics g, Font font, Brush brush, float cx, float cy)
        {
            SizeF textSize = g.MeasureString(s, font);
            g.DrawString(s, font, brush, cx - textSize.Width / 2, cy - textSize.Height / 2);
        }

        public static void DrawStringCenterPoint(string s, Graphics g, Font font, Brush brush, Rectangle p_rect)
        {
            DrawStringCenterPoint(s,g,font,brush,p_rect.X+p_rect.Width/2,p_rect.Y+p_rect.Height/2);
        }

        public static void FillEllipsePathGradient(Graphics g, Rectangle rect, Color[] colors, float[] positions)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(rect);
            FillPathGradient(g, graphicsPath, colors, positions);
        }

        public static void FillLinearGradientRectangle(Graphics g, PointF p1, PointF p2, Color firstColor, Color secondColor, float gradientAngle, float cornerRadious)
        {
            FillLinearGradientRoundedRectangle(g, p1.X, p1.Y, p2.X, p2.Y, firstColor, secondColor, gradientAngle, cornerRadious);
        }

        public static void FillLinearGradientRectangle(Graphics g, float x1, float y1, float x2, float y2, Color firstColor, Color secondColor, float gradientAngle)
        {

            RectangleF rect = new RectangleF(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
            LinearGradientBrush brush = new LinearGradientBrush(rect, firstColor, secondColor, gradientAngle);
            g.FillRectangle(brush, rect);
        }

        public static void FillLinearGradientRoundedRectangle(Graphics g, float x1, float y1, float x2, float y2, Color firstColor, Color secondColor, float gradientAngle, float cornerRadious)
        {

            RectangleF rect = new RectangleF(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
            LinearGradientBrush brush = new LinearGradientBrush(rect, firstColor, secondColor, gradientAngle);
            GraphicsPath path = GetRoundedRectanglePath(x1, y1, x2 - x1, y2 - y1, cornerRadious);
            g.FillPath(brush, path);
        }

        public static void FillPathGradient(Graphics g, GraphicsPath graphicsPath, Color[] colors, float[] positions)
        {
            if (colors.Length != positions.Length) throw new Exception("DrawRectanglePathGradient, colors and positions must have same number of parameters");
            PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
            pathGradientBrush.InterpolationColors = new ColorBlend { Colors = colors, Positions = positions };
            g.FillPath(pathGradientBrush, graphicsPath);
        }

        public static void FillPieByCenterPoint(Graphics g, float cx, float cy, float w, float h, float startAngle, float endAngle, Color color)
        {
            g.FillPie(new SolidBrush(color), cx - w / 2, cy - h / 2, w, h, startAngle, endAngle);
        }

        public static void FillRectanglePathGradient(Graphics g, Rectangle rect, Color[] colors, float[] positions)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(rect);
            FillPathGradient(g, graphicsPath, colors, positions);
        }

        public static void FillRoundedGradient(Graphics g, float x1, float y1, float x2, float y2, Color FirstColor, Color SecondColor)
        {
            GraphicsPath gp = new GraphicsPath();
            //gp.AddEllipse(-5, Height / 2 - 20, Width + 11, Height + 11);
            gp.AddEllipse(x1, y1, x2 - x1, y2 - y1);
            PathGradientBrush pgb = new PathGradientBrush(gp);
            pgb.CenterColor = FirstColor;
            pgb.SurroundColors = new[] { SecondColor };
            g.FillPath(pgb, gp);
        }

        public static Color GetBlendedColor(Color c1,Color c2,float factor)
        {
            return Color.FromArgb((int) GetBlendFactor(c1.R, c2.R, factor), (int) GetBlendFactor(c1.G, c2.G, factor), (int) GetBlendFactor(c1.B, c2.B, factor));
        }

        public static float GetBlendFactor(float f1,float f2,float factor)
        {
            return (f2*factor) + f1*(1 - factor);
        }

        public static Color GetModifiedColor(Color color, int factor, int alpha)
        {
            int newR = color.R + factor;
            if (newR < 0 || newR > 255) newR = color.R;

            int newG = color.G + factor;
            if (newG < 0 || newG > 255) newG = color.R;

            int newB = color.B + factor;
            if (newB < 0 || newB > 255) newB = color.R;
            return Color.FromArgb(alpha, newR, newG, newB);
        }

        public static GraphicsPath GetRoundedCornersGraphicsPath(int width, int height, int cornerSize)
        {
            GraphicsPath gPath = new GraphicsPath();
            gPath.AddArc(0, 0, cornerSize, cornerSize, 180, 90);
            gPath.AddArc(width-0-cornerSize, 0, cornerSize, cornerSize, 270, 90);
            gPath.AddArc(width - 0 - cornerSize, height-0-cornerSize, cornerSize, cornerSize, 0, 90);
            gPath.AddArc(0, height - 0 - cornerSize, cornerSize, cornerSize, 90, 90);
            return gPath;
        }

        public static GraphicsPath GetRoundedRectanglePath(float x, float y, float w, float h, float r)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, r, r, 180, 90);
            gp.AddArc(x + w - r, y, r, r, 270, 90);
            gp.AddArc(x + w - r, y + h - r, r, r, 0, 90);
            gp.AddArc(x, y + h - r, r, r, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        public static GraphicsPath GetRoundedUpperHalfRectanglePath(int x, int y, int w, int h, int r)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, r, r, 180, 90);
            gp.AddArc(x + w - r, y, r, r, 270, 90);
            gp.AddLine(x + w, y + h / 2, x, y + h / 2);
            gp.CloseFigure();
            return gp;
        }

		#endregion (------  Public Methods  ------)

    }
}
