using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfTreeViewExample2
{
    public class Folder
    {
        #region Public Properties

        public string FullPath { get; set; }
        public string FolderLabel { get; set; }
        public List<Folder> Folders { get; set; }

        #endregion
    }
}