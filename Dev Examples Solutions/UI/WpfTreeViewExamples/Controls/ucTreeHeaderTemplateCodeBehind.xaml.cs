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
    /// Interaction logic for ucTreeHeaderTemplateCodeBehind.xaml
    /// </summary>
    public partial class ucTreeHeaderTemplateCodeBehind : UserControl
    {
        #region Constructors

        public ucTreeHeaderTemplateCodeBehind()
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
            list = GetSelectedCheckBoxes(tvMain.Items);


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
            DataTemplate template = GetHeaderTemplate();
            ;

            foreach (WorldArea area in WorldArea.GetAll())
            {
                TreeViewItem item = new TreeViewItem();
                item.HeaderTemplate = template;
                item.Header = area.Name;


                foreach (Country country in area.Countries)
                {
                    TreeViewItem child = new TreeViewItem();
                    child.HeaderTemplate = template;
                    child.Header = country.Name;

                    item.Items.Add(child);
                }

                item.IsExpanded = true;
                tvMain.Items.Add(item);
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
            stackPanel.AppendChild(checkBox);

            // Create Image 
            FrameworkElementFactory image = new FrameworkElementFactory(typeof (Image));
            image.SetValue(Image.MarginProperty, new Thickness(2));
            image.SetBinding(Image.SourceProperty, new Binding() {Converter = new CustomImagePathConverter()});
            stackPanel.AppendChild(image);

            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof (TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding());
            label.SetValue(TextBlock.ToolTipProperty, new Binding());

            stackPanel.AppendChild(label);


            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
        }

        private List<CheckBox> GetSelectedCheckBoxes(ItemCollection items)
        {
            List<CheckBox> list = new List<CheckBox>();
            foreach (TreeViewItem item in items)
            {
                UIElement elemnt = GetChildControl(item, "chk");
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

        private UIElement GetChildControl(DependencyObject parentObject, string childName)
        {
            UIElement element = null;

            if (parentObject != null)
            {
                int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
                for (int i = 0; i < totalChild; i++)
                {
                    DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);

                    if (childObject is FrameworkElement && ((FrameworkElement) childObject).Name == childName)
                    {
                        element = childObject as UIElement;
                        break;
                    }

                    // get its child
                    element = GetChildControl(childObject, childName);
                    if (element != null) break;
                }
            }

            return element;
        }

        #endregion
    }
}