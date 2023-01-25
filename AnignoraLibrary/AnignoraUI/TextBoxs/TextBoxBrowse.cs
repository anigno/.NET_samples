using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AnignoraUI.TextBoxs
{
    public partial class TextBoxBrowse : UserControl
    {
		#region (------  Enums  ------)

        public enum BrowseForEnum
        {
            File,
            Folder
        }

		#endregion (------  Enums  ------)

		#region (------  Fields  ------)

        public const string CATEGORY_STRING = " TextBoxBrowse";
        private string m_text;
        private BrowseForEnum m_browseFor;
        private Color m_colorExist;
        private Color m_colorNotExist;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public TextBoxBrowse()
        {
            InitializeComponent();
            ColorExist = Color.LightGreen;
            ColorNotExist = Color.Pink;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public new string Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                textBoxData.Text = Text;
                setExistColors();
            }
        }


        [Category(CATEGORY_STRING)]
        public BrowseForEnum BrowseFor
        {
            get { return m_browseFor; }
            set
            {
                m_browseFor = value;
                setExistColors();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color ColorExist
        {
            get { return m_colorExist; }
            set
            {
                m_colorExist = value;
                setExistColors();
            }
        }

        [Category(CATEGORY_STRING)]
        public Color ColorNotExist
        {
            get { return m_colorNotExist; }
            set
            {
                m_colorNotExist = value;
                setExistColors();
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnBackColorChanged(EventArgs p_e)
        {
            base.OnBackColorChanged(p_e);
            textBoxData.BackColor = BackColor;
        }

        protected override void OnForeColorChanged(EventArgs p_e)
        {
            base.OnForeColorChanged(p_e);
            textBoxData.ForeColor = ForeColor;
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void buttonBrowseClick(object p_sender, EventArgs p_e)
        {
            if (BrowseFor == BrowseForEnum.File)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBoxData.Text = openFileDialog.FileName;
                    }
                }
            }
            if (BrowseFor == BrowseForEnum.Folder)
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBoxData.Text = folderBrowserDialog.SelectedPath;
                    }
                }
            }
        }

        private void onTextBoxBrowseLoad(object p_sender, EventArgs p_e)
        {
            textBoxData.Text = Text;
        }

        private void onTextBoxDataTextChanged(object p_sender, EventArgs p_e)
        {
            Text = textBoxData.Text;
        }

        private void setExistColors()
        {
            textBoxData.BackColor = ColorNotExist;
            if (BrowseFor == BrowseForEnum.File)
            {
                if (File.Exists(Text)) textBoxData.BackColor = ColorExist;
            }
            if (BrowseFor == BrowseForEnum.Folder)
            {
                if (Directory.Exists(Text)) textBoxData.BackColor = ColorExist;
            }
        }

		#endregion (------  Private Methods  ------)
    }
}