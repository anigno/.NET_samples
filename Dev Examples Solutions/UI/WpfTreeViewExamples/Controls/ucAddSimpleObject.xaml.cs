#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTreeViewExamples.Library;

#endregion

namespace WpfTreeViewExamples.Controls
{
    /// <summary>
    /// Interaction logic for ucAddSimpleObject.xaml
    /// </summary>
    public partial class ucAddSimpleObject : UserControl
    {
        #region Constructors

        public ucAddSimpleObject()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillTree();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> list = new List<CheckBox>();
            list = GetSelectedCheckBoxes(tvMain.Items, tvMain.ItemContainerGenerator);


            string str = "";
            foreach (CheckBox chk in list)
            {
                if (str.Length > 0)
                    str += ", ";

                //str += chk.Content;
                str += chk.Tag.ToString();
            }

            MessageBox.Show(str);
        }

        private void FillTree()
        {
            foreach (WorldArea area in WorldArea.GetAll())
            {
                tvMain.Items.Add(area);
            }
        }

        private List<CheckBox> GetSelectedCheckBoxes(ItemCollection items, ItemContainerGenerator generator)
        {
            List<CheckBox> list = new List<CheckBox>();
            foreach (object area in items)
            {
                TreeViewItem item = (TreeViewItem) generator.ContainerFromItem(area);

                if (item != null)
                {
                    UIElement elemnt = Utility.GetChildControl(item, "chk");
                    if (elemnt != null)
                    {
                        CheckBox chk = (CheckBox) elemnt;
                        if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                        {
                            list.Add(chk);
                        }
                    }


                    List<CheckBox> l = GetSelectedCheckBoxes(item.Items, item.ItemContainerGenerator);
                    list = list.Concat(l).ToList();
                }
            }

            return list;
        }

        #endregion
    }
}