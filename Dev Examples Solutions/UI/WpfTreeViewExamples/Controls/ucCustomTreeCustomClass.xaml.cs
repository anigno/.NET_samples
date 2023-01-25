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
    /// Interaction logic for ucCustomTreeCustomClass.xaml
    /// </summary>
    public partial class ucCustomTreeCustomClass : UserControl
    {
        #region Constructors

        public ucCustomTreeCustomClass()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTree();
        }

        private void LoadTree()
        {
            TreeViewItem treeItem = null;

            // North America 
            treeItem = GetTreeView("North America");

            treeItem.Items.Add(GetTreeView("USA", "usa.png"));
            treeItem.Items.Add(GetTreeView("Canada", "canada.png"));
            treeItem.Items.Add(GetTreeView("Mexico", "mexico.png"));

            tvMain.Items.Add(treeItem);

            // South America 
            treeItem = GetTreeView("South America");

            treeItem.Items.Add(GetTreeView("Argentina", "argentina.png"));
            treeItem.Items.Add(GetTreeView("Brazil", "brazil.png"));
            treeItem.Items.Add(GetTreeView("Uruguay", "uruguay.png"));

            tvMain.Items.Add(treeItem);

            // Europe
            treeItem = GetTreeView("Europe");

            treeItem.Items.Add(GetTreeView("UK", "uk.png"));
            treeItem.Items.Add(GetTreeView("Denmark", "denmark.png"));
            treeItem.Items.Add(GetTreeView("France", "france.png"));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeView("Asia");

            treeItem.Items.Add(GetTreeView("Pakistan", "pakistan.png"));
            treeItem.Items.Add(GetTreeView("Japan", "japan.png"));
            treeItem.Items.Add(GetTreeView("China", "china.png"));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeView("Africa");

            treeItem.Items.Add(GetTreeView("Somalia", "Somalia.png"));
            treeItem.Items.Add(GetTreeView("Uganda", "Uganda.png"));
            treeItem.Items.Add(GetTreeView("Egypt", "Egypt.png"));

            tvMain.Items.Add(treeItem);


            // Australia
            treeItem = GetTreeView("Australia");

            treeItem.Items.Add(GetTreeView("Australia", "australia.gif"));
            tvMain.Items.Add(treeItem);


            // Antarctica
            treeItem = GetTreeView("Antarctica");

            tvMain.Items.Add(treeItem);
        }

        private ImageTreeViewItem GetTreeView(string text)
        {
            return GetTreeView(text, string.Empty);
        }

        private ImageTreeViewItem GetTreeView(string text, string imageUrl)
        {
            ImageTreeViewItem item = null;


            item = new ImageTreeViewItem();
            item.Text = text;

            if (imageUrl.Length > 0)
                item.ImageUrl = new Uri("pack://application:,,/Images/" + imageUrl);


            return item;
        }

        #endregion
    }
}