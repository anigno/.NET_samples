using System.Windows.Forms;

namespace AnignoraUI.Lists
{
    public class ListViewHelper
    {
        public static int GetColumnMouseClicked(ListView listView, MouseEventArgs e)
        {
            int x = 0;
            foreach (ColumnHeader c in listView.Columns)
            {
                if (e.X >= x && e.X <= x + c.Width) return c.Index;
                x += c.Width;
            }
            return -1;
        }

        public static ListViewItem.ListViewSubItem GetSubItemBounds(ListView listView,int rowIndex, int columnIndex)
        {
            ListViewItem item= listView.Items[rowIndex];
            return item.SubItems[columnIndex];
        }
    }
}
