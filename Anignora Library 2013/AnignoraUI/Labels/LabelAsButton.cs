using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AnignoraUI.Labels
{
    public class LabelAsButton:Label
    {
        private const string CATEGORY_STRING = " LabelAsButton";

        [Category(CATEGORY_STRING)]
        public Color MouseOverColor { get; set; }
        [Category(CATEGORY_STRING)]
        public Color MouseDownColor { get; set; }

        [Category(CATEGORY_STRING)]
        public string ToolTipText { get; set; }

        private Color m_savedColor;

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            m_savedColor = base.BackColor;
            BackColor = MouseOverColor;
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            BackColor = m_savedColor;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            BackColor = MouseDownColor;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            BackColor = MouseOverColor;
        }

    }
}
