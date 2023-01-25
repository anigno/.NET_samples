using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace AnignoraUI.ProgressBarsAndGauges
{
    public class ProgressBarRoundedSimple : PictureBox
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " ProgressBarRoundedSimple";
        private readonly Pen m_defaultPen = new Pen(Color.Blue);
        private readonly Label m_label=new Label();
        private int m_maxValue;
        private int m_minValue;
        private int m_needleLength;
        private int m_needleWidth;
        private int m_value;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ProgressBarRoundedSimple()
        {
            NeedleWidth = 10;
            NeedleLength = 20;
            MinValue = 0;
            MaxValue = 100;
            Value = 50;
            Controls.Add(m_label);
            m_label.BackColor = Color.Transparent;
            m_label.ForeColor = base.ForeColor;
            m_label.AutoSize = false;
            m_label.TextAlign = ContentAlignment.MiddleCenter;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public int MaxValue
        {
            get { return m_maxValue; }
            set
            {
                m_maxValue = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int MinValue
        {
            get { return m_minValue; }
            set
            {
                m_minValue = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int NeedleLength
        {
            get { return m_needleLength; }
            set
            {
                m_needleLength = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int NeedleWidth
        {
            get { return m_needleWidth; }
            set
            {
                m_needleWidth = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                m_label.Text = SetText(m_value);
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            m_defaultPen.Color = ForeColor;
            m_label.ForeColor = ForeColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            base.OnPaint(e);
            Draw(e.Graphics);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            m_label.Left = 0;
            m_label.Top = Height/2 - m_label.Height/2;
            m_label.Width = Width;
        }

        /// <summary>
        /// Set custom text
        /// </summary>
        protected string SetText(int value)
        {
            return value.ToString();
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void Draw(Graphics g)
        {
            float angle = (float)Value / (MaxValue - MinValue) * 240 + 150;
            g.DrawArc(m_defaultPen, 0, 0, Width - 1, Height - 1, 150, 240);
            g.DrawArc(m_defaultPen, Width / 2 - NeedleWidth, Height / 2 - NeedleWidth, NeedleWidth * 2, NeedleWidth * 2, angle - 270, 180);
            PointF[] points = new PointF[3];
            points[0].X = (float)(Width / 2f + NeedleLength * Math.Cos(angle / 57.3));
            points[0].Y = (float)(Height / 2f + NeedleLength * Math.Sin(angle / 57.3));
            points[1].X = (float)(Width / 2f + NeedleWidth * Math.Cos((angle - 90) / 57.3));
            points[1].Y = (float)(Height / 2f + NeedleWidth * Math.Sin((angle - 90) / 57.3));
            points[2].X = (float)(Width / 2f + NeedleWidth * Math.Cos((angle + 90) / 57.3));
            points[2].Y = (float)(Height / 2f + NeedleWidth * Math.Sin((angle + 90) / 57.3));
            g.DrawLine(m_defaultPen, points[0], points[1]);
            g.DrawLine(m_defaultPen, points[0], points[2]);
        }

		#endregion (------  Private Methods  ------)
    }
}