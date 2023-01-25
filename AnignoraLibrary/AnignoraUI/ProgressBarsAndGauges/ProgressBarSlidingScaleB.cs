using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using AnignoraUI.Common;

namespace AnignoraUI.ProgressBarsAndGauges
{
    public class ProgressBarSlidingScaleB : Label
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " ProgressBarSlidingScaleB";
        private LinearGradientBrush m_brushBackground;
        private readonly SolidBrush m_brushScale = new SolidBrush(Color.Black);
        private Color m_colorLess;
        private Color m_colorMore;
        private float m_maximum;
        private float m_mediumValue;
        private float m_minimum;
        private readonly Pen m_penMarker = new Pen(Color.Blue, 1f);
        private readonly Pen m_penScale = new Pen(Color.Black, 1f);
        private readonly Pen m_penScaleSmall = new Pen(Color.Black, 0.5f);
        private float m_spacing;
        private float m_step;
        private uint m_subSteps;
        private bool m_useMediumColors;
        private float m_value;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ProgressBarSlidingScaleB()
        {
            Minimum = -5;
            Maximum = 5;
            Value = 0;
            Step = 0.5f;
            SubSteps = 4;
            Spacing = 10f;
            MediumMColorLess = Color.Pink;
            MediumColorMore = Color.LightGreen;
            MediumUse = true;
            MediumValue = 0;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public Color MarkerColor
        {
            get { return m_penMarker.Color; }
            set { m_penMarker.Color = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float Maximum
        {
            get { return m_maximum; }
            set { m_maximum = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public Color MediumColorMore
        {
            get { return m_colorMore; }
            set { m_colorMore = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public Color MediumMColorLess
        {
            get { return m_colorLess; }
            set { m_colorLess = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public bool MediumUse
        {
            get { return m_useMediumColors; }
            set { m_useMediumColors = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float MediumValue
        {
            get { return m_mediumValue; }
            set { m_mediumValue = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float Minimum
        {
            get { return m_minimum; }
            set { m_minimum = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float Spacing
        {
            get { return m_spacing; }
            set { m_spacing = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float Step
        {
            get { return m_step; }
            set { m_step = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public uint SubSteps
        {
            get { return m_subSteps; }
            set { m_subSteps = value; Refresh(); }
        }

        [Category(CATEGORY_STRING)]
        public float Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            createNewBrushBackground();
            Refresh();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            AutoSize = false;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            m_penScale.Color = ForeColor;
            m_penScaleSmall.Color = ForeColor;
            m_brushScale.Color = ForeColor;
            createNewBrushBackground();
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            //base.OnPaint(pe);
            if (m_value < Minimum || m_value > Maximum) return;

            if (MediumUse)
            {
                float factor = (m_value - Minimum) / (Maximum - Minimum);
                Color c = UiHelper.GetBlendedColor(MediumMColorLess, MediumColorMore, factor);
                BackColor = c;
            }

            pe.Graphics.FillRectangle(m_brushBackground, 0, 0, Width, Height);
            //Draw SubSteps
            for (float f = Minimum + Value; f < Maximum + Value + Step / 10; f += Step / (SubSteps + 1))
            {
                float a = valueToAngle(f - Minimum);
                if (a % 360 < 0 || a % 360 > 180) continue;
                float x = angleToDrawX(a);
                pe.Graphics.DrawLine(m_penScaleSmall, x, 0, x, Height / 4f);
            }
            //Draw main steps with numbers
            for (float f = Minimum + Value; f < Maximum + Value + Step / 10; f += Step)
            {
                float a = valueToAngle(f - Minimum);
                if (a % 360 < 0 || a % 360 > 180) continue;

                float x = angleToDrawX(a);
                float v = Maximum - f + Value + Minimum;
                pe.Graphics.DrawLine(m_penScale, x, 0, x, Height / 2f);
                string s = v.ToString("0.0");
                SizeF sizeFont = pe.Graphics.MeasureString(s, Font);
                pe.Graphics.DrawString(s, Font, m_brushScale, x - sizeFont.Width / 2, Height * 3 / 4f - sizeFont.Height / 2);
            }
            pe.Graphics.DrawLine(m_penMarker, Width / 2 - 1, 0, Width / 2 - 1, Height);
            pe.Graphics.DrawLine(m_penMarker, Width / 2 + 1, 0, Width / 2 + 1, Height);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            createNewBrushBackground();
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private float angleToDrawX(float angle)
        {
            return (float)(Width / 2.0 + Spacing * Width / 2.0 * Math.Cos(angle / 57.3));
        }

        private void createNewBrushBackground()
        {
            m_brushBackground = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), ForeColor, BackColor, LinearGradientMode.Horizontal);
            Blend newBlend = new Blend(3)
            {
                Factors = new[] { 0f, 1f, 0f },
                Positions = new[] { 0f, 0.5f, 1f }
            };
            m_brushBackground.Blend = newBlend;
        }

        private float valueToAngle(float value)
        {
            return (value - Minimum) / (Maximum - Minimum) * 300 + 150;
        }

		#endregion (------  Private Methods  ------)
    }
}