using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AnignoraUI.ProgressBarsAndGauges
{
    public class ProgressBarGradient : PictureBox
    {
		#region (------  Fields  ------)

        private Orientation _barOrientation;
        private Color _colorFirst;
        private Color _colorSecond;
        private int _maximum;
        private int _minimum;
        private string _text;
        private int _value;
        private LinearGradientBrush brushBar = new LinearGradientBrush(new Rectangle(0, 0, 10, 10), Color.White, Color.Black, 180);
        private SolidBrush brushText=new SolidBrush(Color.Black);
        private const string CATEGORY_STRING = " ProgressBarGradientHorizontal";

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ProgressBarGradient()
        {
            Minimum = 0;
            Maximum = 10;
            Value = 5;
            ColorFirst = Color.Black;
            ColorSecond = Color.White;
            BarOrientation = Orientation.Horizontal;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public Orientation BarOrientation
        {
            get { return _barOrientation; }
            set
            {
                _barOrientation = value;
                CreateNewBrushBar();
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public string BarText
        {
            get { return _text; }
            set
            {
                _text = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color ColorFirst
        {
            get { return _colorFirst; }
            set
            {
                _colorFirst = value;
                CreateNewBrushBar();
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color ColorSecond
        {
            get { return _colorSecond; }
            set
            {
                _colorSecond = value;
                CreateNewBrushBar();
                Refresh();
            }
        }

        public int LastMouseUpValue { get; set; }

        [Category(CATEGORY_STRING)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if(value==Minimum)return;
                _maximum = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value == Maximum) return;
                _minimum = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color TextColor
        {
            get { return ForeColor; }
            set
            {
                ForeColor = value;
                Refresh();
            }
        }

        [Category(CATEGORY_STRING)]
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            brushText = new SolidBrush(ForeColor);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if(BarOrientation==Orientation.Horizontal)
            {
                LastMouseUpValue = e.X * (Maximum - Minimum) / Width + Minimum;
            }
            else
            {
                LastMouseUpValue = (Height - e.Y) * (Maximum - Minimum) / Height + Minimum;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (BarOrientation == Orientation.Horizontal)
            {
                int vx = Width * (Value - Minimum) / (Maximum - Minimum);
                pe.Graphics.FillRectangle(brushBar, 0, 0, vx, Height);
            }
            else
            {
                int vy = Height - Height*(Value - Minimum)/(Maximum - Minimum);
                pe.Graphics.FillRectangle(brushBar, 0, vy,Width,Height-vy);
            }
            SizeF sf = pe.Graphics.MeasureString(BarText, Font);
            pe.Graphics.DrawString(BarText, Font, brushText, Width / 2f - sf.Width / 2f, Height / 2f - sf.Height / 2f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CreateNewBrushBar();
            Refresh();
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void CreateNewBrushBar()
        {
            if (BarOrientation == Orientation.Horizontal)
            {
                brushBar = new LinearGradientBrush(ClientRectangle, ColorFirst, ColorSecond, 0f);
            }
            else
            {
                brushBar = new LinearGradientBrush(ClientRectangle, ColorFirst, ColorSecond, 270f);
            }
        }

		#endregion (------  Private Methods  ------)
    }
}