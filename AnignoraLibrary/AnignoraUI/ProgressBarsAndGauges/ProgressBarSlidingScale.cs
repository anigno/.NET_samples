using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace AnignoraUI.ProgressBarsAndGauges
{
    /// <summary>
    /// ProgressBarSlidingScale UserControl.
    /// </summary>
    public class ProgressBarSlidingScale : UserControl
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " ProgressBarSlidingScale";
        private double curValue;
        private int largeTicksCount = 5;
        private Color needleColor = Color.Blue;
        private double scaleRange = 100.0;
        private Color shadowColor = Color.Black;
        private bool shadowEnabled = true;
        private int smallTicksCount = 4;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        /// <summary>
        /// SclidingScale default constructor.
        /// </summary>
        public ProgressBarSlidingScale()
        {
            InitializeComponent();
			
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true );
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        /// <summary>
        /// The number of large ticks.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The number of large ticks."),
            DefaultValue(typeof(int), "5")
        ]
        public int LargeTicksCount
        {
            get { return largeTicksCount; }
            set
            {
                if (value!=largeTicksCount && value>0)
                {
                    largeTicksCount = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The color of the scala needle.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The color of the scala needle."),
            DefaultValue(typeof(Color), "Blue")
        ]
        public Color NeedleColor
        {
            get { return needleColor; }
            set
            {
                if (value!=needleColor)
                {
                    needleColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The visible range of the scale.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The visible range of the scale."),
            DefaultValue(typeof(double), "100.0")
        ]
        public double ScaleRange
        {
            get { return scaleRange; }
            set
            {
                if (value>0.001)
                {
                    scaleRange = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The shadow color of the component.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The shadow color of the component."),
            DefaultValue(typeof(Color), "Black")
        ]
        public Color ShadowColor
        {
            get { return shadowColor; }
            set
            {
                if (value!=shadowColor)
                {
                    shadowColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The shadow color of the component.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("Is the shadow enabled?"),
            DefaultValue(typeof(bool), "true")
        ]
        public bool ShadowEnabled
        {
            get { return shadowEnabled; }
            set
            {
                if (value!=shadowEnabled)
                {
                    shadowEnabled = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The number of small ticks.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The number of small ticks."),
            DefaultValue(typeof(int), "4")
        ]
        public int SmallTickCount
        {
            get { return smallTicksCount; }
            set	{
                if (value!=smallTicksCount && value>-1 && value<10)
                {
                    smallTicksCount = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// The current position of the scale.
        /// </summary>
        [
            Browsable(true),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category(CATEGORY_STRING),
            Description("The current position of the scale."),
            DefaultValue(typeof(double), "0.0")
        ]
        public double Value
        {
            get { return curValue; }
            set
            {
                double oldValue = curValue;
				
                curValue = value;
				
                // Refresh only if curValue is significant changed.
                if (Math.Abs(oldValue - curValue) > .0001)
                    Refresh();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        /// <summary>
        /// OnPaint override.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
			
            // Draw simple text, don't waste time with luxus render:
            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			
			
            // Calculate help variables
            int W = ClientRectangle.Width;
            int H = ClientRectangle.Height;

            int Wm = W / 2;
            int Hm = H / 2;
			
            // Calculate distances between ticks
            double largeTicksDistance = scaleRange / largeTicksCount;
            double smallTicksDistance = largeTicksDistance / (smallTicksCount+1);
			
            // Calculate number of pixel between small ticks
            float smallTicksPixels = (float)(W/scaleRange*smallTicksDistance);
			
			
            // Calculate first large tick value and position
            double tickValue = Math.Floor((curValue-scaleRange/2) / largeTicksDistance) * largeTicksDistance;
            float  tickPosition = (float)Math.Floor(Wm - W/scaleRange*(curValue-tickValue));
			
            // Create drawing resources
            Pen pen = new Pen(ForeColor);
            Brush brush = new SolidBrush(ForeColor);
			
            // For all large ticks
            for (int L=0; L<=largeTicksCount; L++)
            {
                // Draw large tick
                e.Graphics.DrawLine(pen, tickPosition-0,0, tickPosition-0,15);
                e.Graphics.DrawLine(pen, tickPosition-1,0, tickPosition-1,15);

                // Draw large tick numerical value
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(Math.Round(tickValue,2).ToString(),
                                      Font, brush,
                                      new PointF(tickPosition, Hm),sf);
				
                // For all small ticks
                for (int S=1; S<=smallTicksCount; S++)
                {
                    // Update tick value and position
                    tickValue += smallTicksDistance;
                    tickPosition += smallTicksPixels;
					
                    // Draw small tick
                    e.Graphics.DrawLine(pen, tickPosition,0,tickPosition,10);
                }
				
                // Update tick value and position
                tickValue += smallTicksDistance;
                tickPosition += smallTicksPixels;
            }
			
            // Dispose drawing resources
            brush.Dispose();
            pen.Dispose();
			
			
            if (ShadowEnabled)
            {
                // Draw left side shadow
                LinearGradientBrush LGBrush = new LinearGradientBrush(new Rectangle(0, 0, Wm, H),
                                                                      Color.FromArgb(255, ShadowColor),
                                                                      Color.FromArgb(0, BackColor),
                                                                      000, true);
                e.Graphics.FillRectangle(LGBrush, new Rectangle(0,0, Wm, H));
				
                // Draw right side shadow
                LGBrush = new LinearGradientBrush(new Rectangle(Wm+1, 0, Wm, H),
                                                  Color.FromArgb(255, ShadowColor),
                                                  Color.FromArgb(0, BackColor),
                                                  180, true);
                e.Graphics.FillRectangle(LGBrush, new Rectangle(Wm+1, 0, Wm, H));

                LGBrush.Dispose();
            }

            // Draw scala needle
            e.Graphics.DrawLine(new Pen(NeedleColor), Wm-0,0, Wm-0,H);
            e.Graphics.DrawLine(new Pen(NeedleColor), Wm-1,0, Wm-1,H);
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // SlidingScale
            // 
            BackColor = Color.White;
            ForeColor = Color.Gray;
            BorderStyle = BorderStyle.Fixed3D;
            Name = "SlidingScale";
            Size = new Size(254, 40);
            ResumeLayout(false);
        }

		#endregion (------  Private Methods  ------)
    }
}