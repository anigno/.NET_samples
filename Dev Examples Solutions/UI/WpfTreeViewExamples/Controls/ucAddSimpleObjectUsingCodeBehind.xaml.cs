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
using System.Windows.Controls.Primitives;

#endregion

namespace WpfTreeViewExamples.Controls
{
    /// <summary>
    /// Interaction logic for ucAddSimpleObjectUsingCodeBehind.xaml
    /// </summary>
    public partial class ucAddSimpleObjectUsingCodeBehind : UserControl
    {
        #region Constructors

        public ucAddSimpleObjectUsingCodeBehind()
        {
            InitializeComponent();

            _list = WorldArea.GetAll();
        }

        #endregion

        #region Private Methods

        private void FillTree()
        {
            tvMain.ItemTemplate = GetHeaderTemplate();
            tvMain.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);

            foreach (WorldArea area in _list)
            {
                tvMain.Items.Add(area);
                ////tvMain.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
            }
        }

        private DataTemplate GetHeaderTemplate()
        {
            //create the data template
            DataTemplate dataTemplate = new DataTemplate();

            //create stack pane;
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof (StackPanel));
            stackPanel.Name = "parentStackpanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // Create check box
            FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof (CheckBox));
            checkBox.Name = "chk";
            checkBox.SetValue(CheckBox.NameProperty, "chk");
            checkBox.SetValue(CheckBox.TagProperty, new Binding());
            checkBox.SetValue(CheckBox.MarginProperty, new Thickness(2));
            checkBox.SetValue(CheckBox.TagProperty, new Binding() {Path = new PropertyPath("Name")});
            stackPanel.AppendChild(checkBox);

            // Create Image 
            FrameworkElementFactory image = new FrameworkElementFactory(typeof (Image));
            image.SetValue(Image.MarginProperty, new Thickness(2));
            image.SetBinding(Image.SourceProperty, new Binding() {Path = new PropertyPath("ImageUrl")});
            stackPanel.AppendChild(image);

            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof (TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding() {Path = new PropertyPath("Name")});
            label.SetValue(TextBlock.ToolTipProperty, new Binding());

            stackPanel.AppendChild(label);


            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
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

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (tvMain.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                foreach (WorldArea area in _list)
                {
                    TreeViewItem item = (TreeViewItem) tvMain.ItemContainerGenerator.ContainerFromItem(area);
                    if (item == null) continue;
                    item.IsExpanded = true;
                    if (item.Items.Count == 0)
                    {
                        foreach (Country country in area.Countries)
                        {
                            item.Items.Add(country);
                        }
                    }
                }
            }
        }

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

        #endregion

        #region Fields

        private System.Collections.ObjectModel.ObservableCollection<WorldArea> _list = null;

        #endregion
    }
}