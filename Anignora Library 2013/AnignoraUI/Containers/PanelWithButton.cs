using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace AnignoraUI.Containers
{
    public class PanelWithButton : Panel
    {
		#region (------  Const Fields  ------)

        public const string CATEGORY_STRING = " PanelWithButton";

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        private readonly Button _button=new Button();

		#endregion (------  Fields  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public int HeaderPos { get; set; }

        [Category(CATEGORY_STRING)]
        public string HeaderText
        {
            get { return _button.Text; }
            set { _button.Text = value; }
        }

        [Category(CATEGORY_STRING)]
        public int HeaderWidth
        {
            get { return _button.Width; }
            set { _button.Width = value; }
        }

		#endregion (------  Properties  ------)

		#region (------  Event Handlers  ------)

        void _button_Click(object sender, EventArgs e)
        {
            if (Parent == null) return;
            foreach(System.Windows.Forms.Control c in Parent.Controls)
            {
                if (c is PanelWithButton) c.Visible = false;
            }
            Visible = true;
        }

		#endregion (------  Event Handlers  ------)

		#region (------  Overridden Methods  ------)

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _button.BackColor = BackColor;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if(Parent!=null)
            {
                Parent.Controls.Add(_button);
                _button.FlatStyle = FlatStyle.Flat;
                _button.Click += new EventHandler(_button_Click);
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _button.ForeColor = ForeColor;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            _button.Left = Left+HeaderPos;
            _button.Top = Top - _button.Height;
        }

		#endregion (------  Overridden Methods  ------)
    }
}
