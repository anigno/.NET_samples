using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AnignoraUI.TreeViews
{
    /// <summary>
    /// Adds a Getter and setter methods for a path, seperated by the seperator string,
    ///  each part if not exists in keys (name) creates a new TreeNode.
    ///  (E.G. "c:\dir1\dir2" will create or add c: root with dir1-->dir2 children/
    /// Adds scrolling data and event
    /// Adds CheckedTreeNodes property and some check logic methods
    /// </summary>
    public class TreeViewExt : TreeView
    {
		#region (------  Const Fields  ------)

        private const string CATEGORY_STRING = " TreeViewExt";
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;

		#endregion (------  Const Fields  ------)

		#region (------  Fields  ------)

        private readonly List<TreeNode> m_checkedTreeNodesList = new List<TreeNode>();

		#endregion (------  Fields  ------)

		#region (------  Events  ------)

        [Category(CATEGORY_STRING)]
        public event ScrollEventHandler Scroll;

		#endregion (------  Events  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public int HScrollPos
        {
            get { return GetScrollPos((int)Handle, SB_VERT); }
        }

        [Category(CATEGORY_STRING)]
        public int VScrollPos
        {
            get { return GetScrollPos((int)Handle, SB_HORZ); }
        }

        public List<TreeNode> CheckedTreeNodes
        {
            get
            {
                m_checkedTreeNodesList.Clear();
                foreach(TreeNode node in Nodes)
                {
                    UpdateCheckedTreeNodesRecurse(node);
                }
                return m_checkedTreeNodesList;
            }
        }

		#endregion (------  Properties  ------)

		#region (------  Static Methods  ------)

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetScrollPos(int hWnd, int nBar);

		#endregion (------  Static Methods  ------)

		#region (------  Overridden Methods  ------)

        protected override void WndProc(ref Message m)
        {
            if (Scroll != null)
            {
                switch (m.Msg)
                {
                    case WM_VSCROLL:
                    case WM_HSCROLL:
                        {
                            ScrollEventType t = (ScrollEventType)Enum.Parse(typeof(ScrollEventType), (m.WParam.ToInt32() & 65535).ToString());
                            Scroll(m.HWnd, new ScrollEventArgs(t, ((int)(m.WParam.ToInt64() >> 16)) & 255));
                        }
                        break;

                }
            }
            base.WndProc(ref m);


        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Public Methods  ------)

        /// <summary>
        /// Adds a path, seperated by the seperator string, each part if not exists in keys (name) creates a new TreeNode
        /// (E.G. "c:\dir1\dir2" will create or add c: root with dir1-->dir2 children
        /// </summary>
        /// <param name="path">The path to add using seperator</param>
        /// <param name="seperator">path seperator</param>
        /// <param name="text">text to add</param>
        /// <param name="autoExpend">automaticaly expend after adding</param>
        /// <param name="showTextInPath">if true, adds the text to each node, else show only key (on newly created nodes)</param>
        /// <returns>The last node that was added or changed</returns>
        public TreeNode AddPath(string path, string seperator,string text,bool autoExpend,bool showTextInPath)
        {
            string[] splitter=new string[]{seperator};
            string[] parts = path.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            TreeNodeCollection nodes=Nodes;
            TreeNode lastNode=null;
            foreach (string key in parts)
            {
                if (nodes.ContainsKey(key))
                {
                    if (showTextInPath)
                    {
                        nodes[key].Text = key + " " + text;
                    }
                    else
                    {
                        nodes[key].Text = key;
                    }
                    lastNode = nodes[key];
                }
                else
                {
                    lastNode=nodes.Add(key, key + " " + text);
                    if (autoExpend && nodes[key].Parent != null) nodes[key].Parent.Expand();
                }
                nodes = nodes[key].Nodes;
            }
            return lastNode;
        }

        /// <summary>
        /// Gets the full keys path of a node using seperator
        /// </summary>
        public string GetFullKeysPath(TreeNode node,string seperator)
        {
            string sRet = string.Empty;
            while (node != null)
            {
                sRet=node.Name+seperator+sRet;
                node = node.Parent;
            }
            return sRet;
        }

        /// <summary>
        /// Returns true if any child is checked
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsAnyChildChecked(TreeNode node)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked)
                {
                    return true;
                }
                if (IsAnyChildChecked(childNode)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if any parent is checked
        /// </summary>
        public bool IsAnyParentChecked(TreeNode node)
        {
            while (node.Parent != null)
            {
                if (node.Parent.Checked) return true;
                node = node.Parent;
            }
            return false;
        }

        /// <summary>
        /// Set all childs nodes to a check value
        /// </summary>
        public void SetCheckAllChildNodes(TreeNode node,bool check)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = check;
                SetCheckAllChildNodes(childNode,check);
            }
        }

        /// <summary>
        /// Set all parents nodes to a check value
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        public void SetCheckAllParents(TreeNode node, bool check)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
                node.Checked=check;
            }
        }

        /// <summary>
        /// Reset all checked nodes
        /// </summary>
        public void ClearAllChecked()
        {
            foreach (TreeNode node in Nodes)
            {
                node.Checked = false;
                SetCheckAllChildNodes(node,false);
            }
        }

        #endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void sssss()
        {
        }

        private void UpdateCheckedTreeNodesRecurse(TreeNode node)
        {
            if(node.Checked)m_checkedTreeNodesList.Add(node);
            foreach (TreeNode childNode in node.Nodes)
            {
                UpdateCheckedTreeNodesRecurse(childNode);
            }
        }

		#endregion (------  Private Methods  ------)
    }
}
