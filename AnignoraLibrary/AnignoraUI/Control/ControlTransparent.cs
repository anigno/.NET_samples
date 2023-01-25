using System.Drawing;
using System.Windows.Forms;

namespace AnignoraUI.Control
{
    public class ControlTransparent : System.Windows.Forms.Control
    {
        public ControlTransparent()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.BackColor = Color.Transparent;
        }



        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // do nothing
        }

        //protected override void OnMove(EventArgs e)
        //{
        //    RecreateHandle();
        //}



    }
}