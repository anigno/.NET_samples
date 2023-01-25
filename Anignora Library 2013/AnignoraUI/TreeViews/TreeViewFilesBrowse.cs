using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace AnignoraUI.TreeViews
{
    public class TreeViewFilesBrowse:TreeView
    {
        public enum ElementTypeEnum
        {
            Drive=0,
            Folder=1,
            File=2
        }
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " TreeViewFilesBrowser";
        private readonly string[] m_extensionsSplitter = new string[] { " ", ";", "," };

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public TreeViewFilesBrowse()
        {
            AllowedExtenssions = "*";
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public string AllowedExtenssions { get; set; }

        [Category(CATEGORY_STRING)]
        public bool ShowFiles { get; set; }

		#endregion (------  Properties  ------)

		#region (------  Protected Methods  ------)

        protected override void OnAfterCollapse(TreeViewEventArgs e)
        {
            base.OnAfterCollapse(e);
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Nodes.Clear();
            }
        }

        protected override void OnAfterExpand(TreeViewEventArgs e)
        {
            base.OnAfterExpand(e);
            foreach (TreeNode node in e.Node.Nodes)
            {
                readSubDir(node);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            createDrives();
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        private void createDrives()
        {
            Nodes.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo di in drives)
            {
                TreeNode node=new TreeNode(di.Name);
                node.Tag = ElementTypeEnum.Drive;
                Nodes.Add(node);
                if (di.IsReady)  readSubDir(node);
            }
        }

        private bool isAllowedExtension(string p_file)
        {
            if (AllowedExtenssions.Contains("*")) return true;
            string[] extensions = AllowedExtenssions.Split(m_extensionsSplitter, StringSplitOptions.RemoveEmptyEntries);
            string fileExtension = Path.GetExtension(p_file).Substring(1).ToLower();
            foreach (string extemsion in extensions)
            {
                if (extemsion.ToLower() == fileExtension) return true;
            }
            return false;
        }

        private void readSubDir(TreeNode p_node)
        {
            try
            {
                if ((ElementTypeEnum)p_node.Tag == ElementTypeEnum.File) return;
                string path = p_node.FullPath;
                string[] folders = Directory.GetDirectories(path);
                foreach (string folder in folders)
                {
                    TreeNode node = new TreeNode(Path.GetFileName(folder));
                    node.Tag = ElementTypeEnum.Folder;
                    p_node.Nodes.Add(node);
                }
                if (!ShowFiles) return;
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if(!isAllowedExtension(file))continue;
                    TreeNode node = new TreeNode(Path.GetFileName(file));
                    node.Tag = ElementTypeEnum.File;
                    p_node.Nodes.Add(node);

                }
            }
            catch
            {
                //nothing
            }
        }

		#endregion (------  Private Methods  ------)
    }
    
    
}
