using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AnignoraUI.Common;

namespace AnignoraUI.TabControls
{
    public class TabControlColors:TabControl
    {
        private const string CATEGORY_STRING = " TabControlColors";
        public TabControlColors()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        [Category(CATEGORY_STRING)]
        public Color TabControlBackColor { get; set; }
        [Category(CATEGORY_STRING)]
        public Color TabControlBorderColor { get; set; }
        [Category(CATEGORY_STRING)]
        public Color TabControlSelectedBackColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g=e.Graphics;

            Rectangle tabArea = new Rectangle(ClientRectangle.X - 0, ClientRectangle.Y - 0, ClientRectangle.Width -1, ClientRectangle.Height - 1);
            Rectangle tabPageArea = new Rectangle(DisplayRectangle.X - 1, DisplayRectangle.Y - 1, DisplayRectangle.Width + 1, DisplayRectangle.Height + 1);
            g.FillRectangle(new SolidBrush(TabControlBackColor), tabArea);
            g.DrawRectangle(new Pen(TabControlBorderColor), tabArea);
            g.DrawRectangle(new Pen(TabControlBorderColor), tabPageArea);

            for (int a = 0; a < TabPages.Count; a++)
            {
                Rectangle tabHeaderArea = GetTabRect(a);
                tabHeaderArea.Offset(1,1);
                Color tabPageBackColor = TabPages[a].BackColor;
                Color tabPageForeColor = TabPages[a].ForeColor;
                if (SelectedTab == TabPages[a])
                {
                    tabPageBackColor = TabControlSelectedBackColor;
                }
                g.FillRectangle(new SolidBrush(tabPageBackColor), tabHeaderArea);
                g.DrawRectangle(new Pen(TabControlBorderColor), tabHeaderArea);
                if (TabPages[a] == SelectedTab)
                {
                    tabHeaderArea.Inflate(-1, 0);
                    tabHeaderArea.Offset(1, 2);
                    g.FillRectangle(new SolidBrush(tabPageBackColor), tabHeaderArea);
                }
                UiHelper.DrawStringCenterPoint(TabPages[a].Text, g, Font, new SolidBrush(tabPageForeColor), tabHeaderArea);
            }

        }



    }
}
