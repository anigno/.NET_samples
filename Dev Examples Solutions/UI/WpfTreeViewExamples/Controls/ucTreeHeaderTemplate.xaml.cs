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
    /// Interaction logic for ucTreeHeaderTemplate.xaml
    /// </summary>
    public partial class ucTreeHeaderTemplate : UserControl
    {
        #region Constructors

        public ucTreeHeaderTemplate()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            List<CheckBox> list = new List<CheckBox>();
            list = GetSelectedCheckBoxes(tvMain.Items);

            ////UIElement elemnt = GetChildControl(item, "chk");
            ////if (elemnt != null)
            ////{
            ////    CheckBox chk = (CheckBox)elemnt;
            ////    if (chk.IsChecked.HasValue && chk.IsChecked.Value)
            ////    {
            ////        string m = chk.Content.ToString();
            ////        MessageBox.Show(m);
            ////    }
            ////}


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

        private List<CheckBox> GetSelectedCheckBoxes(ItemCollection items)
        {
            List<CheckBox> list = new List<CheckBox>();
            foreach (TreeViewItem item in items)
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

                List<CheckBox> l = GetSelectedCheckBoxes(item.Items);
                list = list.Concat(l).ToList();
            }

            return list;
        }

        #endregion
    }
}