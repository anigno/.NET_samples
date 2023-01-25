using System.Collections.Generic;
using System.Windows.Forms;

namespace AnignoraUI.Lists.ListTreeView
{
    public class ListTreeViewItem :ListViewItem
    {
		#region (------  Fields  ------)

        private List<ListTreeViewItem> m_childrens = new List<ListTreeViewItem>();
        private bool m_isExpended = false;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ListTreeViewItem(string[] subItemsText)
            : base(subItemsText)
        {
            Container = null;
        }

        public ListTreeViewItem()
        {
            Container = null;
            Parent = null;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public List<ListTreeViewItem> Childrens
        {
            get { return m_childrens; }
            set { m_childrens = value; }
        }

        public ListTreeView Container { get; set; }

        public bool IsExpended
        {
            get { return m_isExpended; }
            set
            {
                if (m_isExpended == value) return;
                m_isExpended = value;
                if (m_isExpended)
                {
                    foreach (ListTreeViewItem item in Childrens)
                    {
                        Container.Items.Insert(Index + 1, item);
                        ListTreeView.SetListTreeViewItemIcon(item);
                    }
                }
                else
                {
                    foreach (ListTreeViewItem item in Childrens)
                    {
                        if (item.IsExpended)
                        {
                            item.IsExpended = false;
                        }
                        Container.Items.RemoveAt(item.Index);
                    }
                }
                ListTreeView.SetListTreeViewItemIcon(this);

            }
        }

        public ListTreeViewItem Parent { get; set; }

		#endregion (------  Properties  ------)
    }
}
