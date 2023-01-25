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
    /// Interaction logic for ucCustomTreeCodeBehind.xaml
    /// </summary>
    public partial class ucCustomTreeCodeBehind : UserControl
    {
        #region Constructors

        public ucCustomTreeCodeBehind()
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
            treeItem = GetTreeView("North America", Colors.Green);

            treeItem.Items.Add(GetTreeView("USA", "usa.png"));
            treeItem.Items.Add(GetTreeView("Canada", "canada.png"));
            treeItem.Items.Add(GetTreeView("Mexico", "mexico.png"));

            tvMain.Items.Add(treeItem);

            // South America 
            treeItem = GetTreeView("South America", Colors.LightGreen);

            treeItem.Items.Add(GetTreeView("Argentina", "argentina.png"));
            treeItem.Items.Add(GetTreeView("Brazil", "brazil.png"));
            treeItem.Items.Add(GetTreeView("Uruguay", "uruguay.png"));

            tvMain.Items.Add(treeItem);

            // Europe
            treeItem = GetTreeView("Europe", Colors.Brown);

            treeItem.Items.Add(GetTreeView("UK", "uk.png"));
            treeItem.Items.Add(GetTreeView("Denmark", "denmark.png"));
            treeItem.Items.Add(GetTreeView("France", "france.png"));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeView("Asia", Colors.Red);

            treeItem.Items.Add(GetTreeView("Pakistan", "pakistan.png"));
            treeItem.Items.Add(GetTreeView("Japan", "japan.png"));
            treeItem.Items.Add(GetTreeView("China", "china.png"));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeView("Africa", Colors.Yellow);

            treeItem.Items.Add(GetTreeView("Somalia", "Somalia.png"));
            treeItem.Items.Add(GetTreeView("Uganda", "Uganda.png"));
            treeItem.Items.Add(GetTreeView("Egypt", "Egypt.png"));

            tvMain.Items.Add(treeItem);


            // Australia
            treeItem = GetTreeView("Australia", Colors.Purple);

            treeItem.Items.Add(GetTreeView("Australia", "australia.gif"));
            tvMain.Items.Add(treeItem);


            // Antarctica
            treeItem = GetTreeView("Antarctica", Colors.Blue);

            tvMain.Items.Add(treeItem);
        }

        private TreeViewItem GetTreeView(string text, string imagePath)
        {
            TreeViewItem item = new TreeViewItem();

            item.IsExpanded = true;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            // create Image
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("pack://application:,,/Images/" + imagePath));

            // Label
            Label lbl = new Label();
            lbl.Content = text;


            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            return item;
        }

        private TreeViewItem GetTreeView(string text, Color boxColor)
        {
            TreeViewItem item = new TreeViewItem();
            item.IsExpanded = true;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;


            // create Image

            Border border = new Border();
            border.Width = 8;
            border.Height = 12;
            border.Background = new SolidColorBrush(boxColor);


            // Label
            Label lbl = new Label();
            lbl.Content = text;


            stack.Children.Add(border);
            stack.Children.Add(lbl);

            //item.HeaderTemplate.ad  


            item.Header = stack;
            return item;
        }

        #endregion
    }
}