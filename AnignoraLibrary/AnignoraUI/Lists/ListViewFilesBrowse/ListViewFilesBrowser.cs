using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AnignoraUI.Lists.ListViewFilesBrowse
{
    public class ListViewFilesBrowser :ListView
    {
		#region (------  Fields  ------)

        private readonly string[] m_extensionsSplitters = new[] { " ", ";", "," };
        private readonly ImageList m_imageList=new ImageList();

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ListViewFilesBrowser()
        {
            AllowedExtensions = "*";
            View = View.Details;
            Columns.Add("","",100);
            HeaderStyle = ColumnHeaderStyle.None;
            KeyPress += listViewFilesBrowserKeyPress;
            m_imageList.Images.Add(ResourceMain.hard_drive_1);
            m_imageList.Images.Add(ResourceMain.open_folder);
            m_imageList.Images.Add(ResourceMain.document_blank);
            SmallImageList = m_imageList;
            m_imageList.ImageSize=new Size(50,50);
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(" ListViewFilesBrowser")]
        public string AllowedExtensions { get; set; }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            addDrives();
        }

        protected override void OnResize(EventArgs p_e)
        {
            base.OnResize(p_e);
            Columns[0].Width = Width-4;
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void addDrives()
        {
            Items.Clear();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                ListViewItem item = new ListViewItem(drive.Name);
                item.Tag = drive.Name;
                item.ImageIndex = 0;
                Items.Add(item);
            }
        }

        private void extractFolder(string p_path)
        {
            if (!Directory.Exists(p_path)) return;
            Items.Clear();
            ListViewItem item=new ListViewItem("..");
            item.Tag = Path.GetDirectoryName(p_path);
            Items.Add(item);
            foreach (string folder in Directory.GetDirectories(p_path))
            {
                item = new ListViewItem(Path.GetFileName(folder));
                item.Tag = folder;
                item.ImageIndex = 1;
                Items.Add(item);
            } 
            foreach (string file in Directory.GetFiles(p_path))
            {
                if (!isExtensionAllowed(file)) continue;
                item = new ListViewItem(Path.GetFileName(file));
                item.Tag = file;
                item.ImageIndex = 2;
                Items.Add(item);
            }
        }

        private bool isExtensionAllowed(string p_file)
        {
            if (AllowedExtensions.IndexOf("*") >= 0) return true;
            string[] allowedExtensionsArray = AllowedExtensions.Split(m_extensionsSplitters, StringSplitOptions.RemoveEmptyEntries);
            string extension = Path.GetExtension(p_file);
            foreach (string e in allowedExtensionsArray)
            {
                if ("."+e.ToLower() == extension.ToLower()) return true;
            }
            return false;
        }

        void listViewFilesBrowserKeyPress(object p_sender, KeyPressEventArgs p_e)
        {
            if (SelectedItems.Count == 0) return;
            if (p_e.KeyChar == 13)
            {
                if (SelectedItems[0].Tag == null)
                {
                    addDrives();
                    return;
                }
                extractFolder(SelectedItems[0].Tag.ToString());
            }
           
        }

		#endregion (------  Private Methods  ------)
    }
}
