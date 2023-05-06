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

#endregion

namespace WpfTreeViewExamples.Controls
{
    /// <summary>
    /// Interaction logic for ucSimpleTreeCodeBehind.xaml
    /// </summary>
    public partial class ucSimpleTreeCodeBehind : UserControl
    {
        #region Constructors

        public ucSimpleTreeCodeBehind()
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
            treeItem = new TreeViewItem();
            treeItem.Header = "North America";
            treeItem.IsExpanded = true;

            treeItem.Items.Add(new TreeViewItem() {Header = "USA"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Canada"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Mexico"});

            tvMain.Items.Add(treeItem);


            // South America 
            treeItem = new TreeViewItem();
            treeItem.Header = "South America";
            treeItem.IsExpanded = true;

            treeItem.Items.Add(new TreeViewItem() {Header = "Argentina"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Brazil"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Uruguay"});

            tvMain.Items.Add(treeItem);


            // Europe
            treeItem = new TreeViewItem();
            treeItem.Header = "Europe";
            treeItem.IsExpanded = true;

            treeItem.Items.Add(new TreeViewItem() {Header = "UK"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Denmark"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Farance"});

            tvMain.Items.Add(treeItem);

            // Africa
            treeItem = new TreeViewItem();
            treeItem.Header = "Africa";
            treeItem.IsExpanded = true;

            treeItem.Items.Add(new TreeViewItem() {Header = "Somalia"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Uganda"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Egypt"});

            tvMain.Items.Add(treeItem);


            // Asia
            treeItem = new TreeViewItem();
            treeItem.Header = "Asia";
            treeItem.IsExpanded = true;

            treeItem.Items.Add(new TreeViewItem() {Header = "Pakistan"});
            treeItem.Items.Add(new TreeViewItem() {Header = "China"});
            treeItem.Items.Add(new TreeViewItem() {Header = "Japan"});


            tvMain.Items.Add(treeItem);


            // Australia
            treeItem = new TreeViewItem();
            treeItem.Header = "Australia";
            treeItem.IsExpanded = true;

            tvMain.Items.Add(treeItem);


            // Antarctica
            treeItem = new TreeViewItem();
            treeItem.Header = "Antarctica";
            treeItem.IsExpanded = true;

            tvMain.Items.Add(treeItem);
        }

        #endregion
    }
}