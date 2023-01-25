using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using AnignoraUI.Common;

namespace AnignoraUI.ProgressBarsAndGauges
{
    public class AnignoProgressBarFlat : System.Windows.Forms.Control
    {
		#region (------  Const Fields  ------)

        private const string CATEGORY_STRING = " AnignoProgressBarFlat";

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        private Color _colorMainFirst = Color.Green;
        private Color _colorMainSecond = Color.Red;
        private Color _colorMin = Color.Green;
        private Color _colorMax = Color.Red;
        private Color _BackColorFirst = Color.Gray;
        private Color _backColorSecond = Color.LightGray;
        private Color _progressBarTextColor=Color.White;
        private float _colorMainGradientAngle = 30;
        private float _value = 50;
        private float _minimum;
        private float _maximum = 100;
        private float _minimumRange = 20;
        private float _MaximumRange = 80;
        private readonly object _syncRoot = new object();
        private Orientation _progressBarOrientation;

		#endregion (------  Fields  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        [Description("")]
        public bool DrawBorder { get; set; }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color ProgressBarTextColor
        {
            get { return _progressBarTextColor; }
            set
            {
                _progressBarTextColor = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color ColorMainFirst
        {
            get
            {
                lock (_syncRoot)
                {
                    return _colorMainFirst;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _colorMainFirst = value;
                    Refresh();
                }
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color ColorMainSecond
        {
            get
            {
                lock (_syncRoot)
                {
                    return _colorMainSecond;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _colorMainSecond = value;
                    Refresh();
                }
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color ColorMinimumRange
        {
            get
            {
                lock (_syncRoot)
                {
                    return _colorMin;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _colorMin = value;
                    Refresh();
                }
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color ColorMaximumRange
        {
            get
            {
                lock (_syncRoot)
                {
                    return _colorMax;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _colorMax = value;
                    Refresh();
                }
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color BackColorFirst
        {
            get { return _BackColorFirst; }
            set
            {
                _BackColorFirst = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Color BackColorSecond
        {
            get { return _backColorSecond; }
            set
            {
                _backColorSecond = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float GradientAngle
        {
            get
            {
                lock (_syncRoot)
                {
                    return ColorMainGradientAngle;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    ColorMainGradientAngle = value;
                }
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float Value
        {
            get
            {
                lock (_syncRoot)
                {
                    return _value;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _value = value;
                }
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float Minimum
        {
            get
            {
                lock (_syncRoot)
                {
                    return _minimum;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _minimum = value;
                }
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float Maximum
        {
            get
            {
                lock (_syncRoot)
                {
                    return _maximum;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _maximum = value;
                }
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float MinimumRange
        {
            get { return _minimumRange; }
            set
            {
                _minimumRange = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float MaximumRange
        {
            get { return _MaximumRange; }
            set
            {
                _MaximumRange = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public float ColorMainGradientAngle
        {
            get { return _colorMainGradientAngle; }
            set
            {
                _colorMainGradientAngle = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("")]
        public Orientation ProgressBarOrientation
        {
            get { return _progressBarOrientation; }
            set
            {
                _progressBarOrientation = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        [Description("Text to be drawn on the control")]
        public string ProgressBarText
        {
            get { return Text; }
            set
            {
                Text = value;
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Overridden Methods  ------)

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Bitmap backBuffer = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(backBuffer);
            drawPartRect(g, Minimum, Maximum, Maximum, BackColorFirst, BackColorSecond);
            float minDrawValue = Value > MinimumRange ? MinimumRange : Value;
            drawPartRect(g, Minimum, MinimumRange, minDrawValue, _colorMin, _colorMin);
            float mainDrawValue = Value < MaximumRange ? Value : MaximumRange;
            if (Value > MinimumRange)
            {
                drawPartRect(g, MinimumRange, MaximumRange, mainDrawValue, _colorMainFirst, _colorMainSecond);
            }
            if (Value >= MaximumRange)
            {
                drawPartRect(g, MaximumRange, Maximum, Value, _colorMax, _colorMax);
            }
            if (DrawBorder)
            {
                Pen pen = new Pen(ForeColor);
                g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
            Brush textBrush=new SolidBrush(ProgressBarTextColor);
            UiHelper.DrawStringCenterPoint(ProgressBarText,g,Font,textBrush,Width/2f,Height/2f);
            g.Dispose();
            e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);

        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Private Methods  ------)

        private void drawPartRect(Graphics g, float startValue, float endValue, float value, Color colorFirst, Color colorSecond)
        {
            if (Maximum == Minimum) return;
            if (ProgressBarOrientation == Orientation.Horizontal)
            {
                float drawValueUnit = Width / (Maximum - Minimum);
                float drawWidthBrush = drawValueUnit * (endValue - startValue);
                float drawWidth = drawValueUnit * (value - startValue);
                if (drawWidthBrush == 0) return;
                RectangleF rectBrush = new RectangleF(drawValueUnit * startValue - 1, 0, drawWidthBrush + 1, Height);
                RectangleF rectDraw = new RectangleF(drawValueUnit * startValue, 0, drawWidth, Height);
                Brush brush = new LinearGradientBrush(rectBrush, colorFirst, colorSecond, ColorMainGradientAngle);
                g.FillRectangle(brush, rectDraw);
            }
            else
            {
                float drawValueUnit = Height / (Maximum - Minimum);
                float drawHeightBrush = drawValueUnit * (endValue - startValue);
                float drawHeight =drawValueUnit * (value - startValue);
                if (drawHeightBrush == 0) return;
                RectangleF rectBrush = new RectangleF(0,drawValueUnit * startValue - 1,Width, drawHeightBrush + 1);
                Brush brush = new LinearGradientBrush(rectBrush, colorFirst, colorSecond, ColorMainGradientAngle);
                RectangleF rectDraw = new RectangleF(0, Height - drawHeight - drawValueUnit * startValue, Width, drawHeight);
                g.FillRectangle(brush, rectDraw);
            }

        }

		#endregion (------  Private Methods  ------)
    }
}