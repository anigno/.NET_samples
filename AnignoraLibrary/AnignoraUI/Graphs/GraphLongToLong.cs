using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace AnignoraUI.Graphs
{
    public class GraphLongToLong : Panel
    {
		#region (------  Const Fields  ------)

        private const string CATEGORY_STRING = " GraphLongToLong";

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        private readonly Pen m_graphPen = new Pen(Color.Black);
        private readonly Pen m_gridsPen = new Pen(Color.LightGray);
        readonly SolidBrush m_gridsTextBrush = new SolidBrush(Color.Black);
        private readonly Dictionary<long, long> m_points = new Dictionary<long, long>();
        private readonly Pen zeroPen=new Pen(Color.White,2);

		#endregion (------  Fields  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public Color GraphColor
        {
            get { return m_graphPen.Color; }
            set { m_graphPen.Color = value; }
        }

        [Category(CATEGORY_STRING)]
        public Color GridsColor
        {
            get { return m_gridsPen.Color; }
            set
            {
                m_gridsPen.Color = value;
            }
        }

        [Category(CATEGORY_STRING)]
        public uint GridsHorizontal { get; set; }

        [Category(CATEGORY_STRING)]
        public Font GridsTextFont { get; set; }

        [Category(CATEGORY_STRING)]
        public uint GridsVertical { get; set; }

        [Category(CATEGORY_STRING)]
        public Color GridTextColor
        {
            get
            {
                return m_gridsTextBrush.Color;
            }
            set
            {
                m_gridsTextBrush.Color = value;
            }
        }

        [Category(CATEGORY_STRING)]
        public bool ShowGrids { get; set; }

        [Category(CATEGORY_STRING)]
        public Color ZeroLineColor
        {
            get { return zeroPen.Color; }
            set { zeroPen.Color = value; }
        }

		#endregion (------  Properties  ------)

		#region (------  Constructors  ------)

        public GraphLongToLong()
        {
            ShowGrids = true;
            GridsHorizontal = 5;
            GridsVertical = 5;
            GridsTextFont = base.Font;
        }

		#endregion (------  Constructors  ------)

		#region (------  Static Methods  ------)

        private static long DrawToReal(long min, long max, long dVal, long drawMaxSize)
        {
            if (drawMaxSize < 1) drawMaxSize = 1;
            return dVal * (max - min) / drawMaxSize + min;
        }

        private static long RealToDraw(long min,long max,long rVal,long drawMaxSize)
        {
            if(max==min)
            {
                max = max+Math.Abs(max);
            }
            if (max == min) return 0;
            return (rVal - min)*drawMaxSize/(max - min);
        }

		#endregion (------  Static Methods  ------)

		#region (------  Overridden Methods  ------)

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (m_points.Count < 2) return;
            DrawAll(e.Graphics);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            RedrawGraph();
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Public Methods  ------)

        public void AddPointLongLong(long lX, long lY)
        {
            m_points.Add(lX, lY);
        }

        public void ClearAllPoints()
        {
            m_points.Clear();
        }

        public void RedrawGraph()
        {
            Refresh();
        }

		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)

        protected virtual string GetLabelStringX(long l)
        {
            return l.ToString();
        }

        protected virtual string GetLabelStringY(long l)
        {
            return l.ToString();
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void DrawAll(Graphics g)
        {
            long minY = m_points.Values.Min();
            long maxY = m_points.Values.Max();
            long minX = m_points.Keys.Min();
            long maxX = m_points.Keys.Max();
            minY -= Math.Abs(minY/100);
            maxY += Math.Abs(minY/100);

            //Draw zero line
            long zeroY = RealToDraw(minY, maxY, 0, Height);
            g.DrawLine(zeroPen, 0, Height - zeroY, Width, Height - zeroY);

            //Horizontal grids
            for (int a = 1; a < GridsHorizontal; a++)
            {
                long l = a * Height / GridsHorizontal;
                long v = DrawToReal(minY, maxY, l, Height);
                g.DrawLine(m_gridsPen, 0, Height - l, Width, Height - l);
                g.DrawString(GetLabelStringY(v), GridsTextFont, m_gridsTextBrush, 0, Height - l - GridsTextFont.Height/2);
            }
            //Draw min/max y values
            g.DrawString(GetLabelStringY(maxY), GridsTextFont, m_gridsTextBrush, 0, 0);
            g.DrawString(GetLabelStringY(minY), GridsTextFont, m_gridsTextBrush, 0, Height-GridsTextFont.Height);

            //Vertical grids
            for (int a = 1; a < GridsVertical; a++)
            {
                long l = a * Width / GridsVertical;
                long v = DrawToReal(minX, maxX, l, Width);
                g.DrawLine(m_gridsPen, l, 0, l, Height);
                float fw = g.MeasureString(GetLabelStringX(v), GridsTextFont).Width;
                g.DrawString(GetLabelStringX(v), GridsTextFont, m_gridsTextBrush, l - fw / 2, Height - GridsTextFont.Height);
            }

            //Draw graph
            int b = 0;
            KeyValuePair<long, long> prevPair = new KeyValuePair<long, long>();
            foreach (KeyValuePair<long, long> pair in m_points)
            {
                if (b++ > 0)
                {
                    long p1x = RealToDraw(minX, maxX, prevPair.Key, Width);
                    long p2x = RealToDraw(minX, maxX, pair.Key, Width);
                    long p1y = RealToDraw(minY, maxY, prevPair.Value, Height);
                    long p2y = RealToDraw(minY, maxY, pair.Value, Height);
                    g.DrawLine(m_graphPen, p1x, Height - p1y, p2x, Height - p2y);
                }
                prevPair = pair;
            }

        }

		#endregion (------  Private Methods  ------)
    }
}