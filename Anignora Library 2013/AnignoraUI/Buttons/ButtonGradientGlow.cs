using System;
using System.Windows.Forms;
using AnignoraUI.Labels;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AnignoraUI.Buttons
{
    [ToolboxBitmap(typeof(Button))]
    public class ButtonGradientGlow : LabelGradientLinear
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " ButtonGradientGlow";
        private Color m_glowColor = Color.Blue;
        private uint m_glowIntensity = 100;
        private bool m_mouseDown = false;
        private bool m_mouseOver = false;
        private static readonly int CLICK_STEP = 2;

		#endregion (------  Fields  ------)

		#region (------  Properties  ------)

       [Category(CATEGORY_STRING)]
        public Color GlowColor
        {
            get { return m_glowColor; }
            set
            {
                m_glowColor = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public uint GlowIntensity
        {
            get { return m_glowIntensity; }
            set { m_glowIntensity = value; }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            GradientAngle = 270;
            AutoSize = false;
            GradientColorSecond = Color.Gray;
            BackColor = Color.Black;
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_mouseDown = true;
            Top += CLICK_STEP;
            Left += CLICK_STEP;
            Refresh();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            m_mouseOver = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            m_mouseOver = false;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_mouseDown = false;
            Top -= CLICK_STEP;
            Left -= CLICK_STEP;
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            uint tempGlow = GlowIntensity;
            if(m_mouseDown)
            {
                tempGlow = (2*tempGlow)%256;
            }
            if (m_mouseOver)
            {
                GraphicsPath gPath = new GraphicsPath();
                gPath.AddEllipse(Width*0/4, Height*2/4, Width, Height/2);
                PathGradientBrush pgBrush = new PathGradientBrush(gPath);
                pgBrush.CenterPoint = new PointF(Width/2f, Height*3/4f);
                pgBrush.CenterColor = Color.FromArgb((int)tempGlow, GlowColor);
                pgBrush.SurroundColors = new[] {Color.FromArgb(0, Color.White)};
                e.Graphics.FillPath(pgBrush, gPath);
            }
        }

		#endregion (------  Protected Methods  ------)
    }
}
