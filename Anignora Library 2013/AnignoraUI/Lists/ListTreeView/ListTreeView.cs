using System;
using System.Windows.Forms;

namespace AnignoraUI.Lists.ListTreeView
{
    public class ListTreeView : ListView
    {
        private ListTreeViewItem Root{ get; set;}

        public ListTreeView()
        {
            GridLines = true;
            View = View.Details;
            Root = new ListTreeViewItem();
            ImageList smallImageList=new ImageList();
            smallImageList.Images.Add(ResourceImages.Open_Folder);
            smallImageList.Images.Add(ResourceImages.Closed_Folder);
            smallImageList.Images.Add(ResourceImages.FILE);
            SmallImageList = smallImageList;
        }

         public ListTreeViewItem AddListTreeViewItem(ListTreeViewItem parent, string text)
         {
             ListTreeViewItem item=new ListTreeViewItem();
             item.Text = text;
             return AddListTreeViewItem(parent, item);
         }

        public ListTreeViewItem AddListTreeViewItem(ListTreeViewItem parent, ListTreeViewItem item)
        {
            parent.Childrens.Add(item);
            item.Parent = parent;
            item.Container = this;
            SetListTreeViewItemIcon(item.Parent);
            if (item.Parent.IsExpended)
            {
                SetListTreeViewItemIcon(item.Parent);
            }
            return item;
        }

        public ListTreeViewItem AddListTreeViewRoot(string text)
        {
            ListTreeViewItem item = new ListTreeViewItem();
            item.Text = text;
            return AddListTreeViewRoot(item);
        }

        public ListTreeViewItem AddListTreeViewRoot(ListTreeViewItem rootItem)
        {
            Root.Childrens.Add(rootItem);
            rootItem.Parent = null;
            rootItem.Container = this;
            SetListTreeViewItemIcon(rootItem);
            Items.Add(rootItem);
            return rootItem;
        }

        internal static int GetIndentCount(ListTreeViewItem item)
        {
            int a = 0;
            ListTreeViewItem iterator = item;
            while (iterator.Parent != null)
            {
                iterator = iterator.Parent;
                a+=1;
            }
            return a;
        }

        internal static void SetListTreeViewItemIcon(ListTreeViewItem item)
        {
            item.ImageIndex = 2; if (item.Childrens.Count > 0)
            {
                item.ImageIndex = item.IsExpended ? 0 : 1;
            }
            int i = GetIndentCount(item);
            item.IndentCount = i;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (SelectedItems.Count <= 0) return;
            ListTreeViewItem item = (ListTreeViewItem) SelectedItems[0];
            item.IsExpended = !item.IsExpended;
        }
    }
}
