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
    /// Interaction logic for ucDataBindCodeBehind.xaml
    /// </summary>
    public partial class ucDataBindCodeBehind : UserControl
    {
        #region Constructors

        public ucDataBindCodeBehind()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindTree();
        }

        private void BindTree()
        {
            tvMain.ItemTemplate = GetTemplate();
            tvMain.ItemsSource = WorldArea.GetAll();
        }

        private HierarchicalDataTemplate GetTemplate()
        {
            //create the data template
            HierarchicalDataTemplate dataTemplate = new HierarchicalDataTemplate();

            //create stack pane;
            FrameworkElementFactory grid = new FrameworkElementFactory(typeof (Grid));
            grid.Name = "parentStackpanel";
            grid.SetValue(Grid.WidthProperty, Convert.ToDouble(100));
            grid.SetValue(Grid.HeightProperty, Convert.ToDouble(24));
            grid.SetValue(Grid.MarginProperty, new Thickness(2));
            grid.SetValue(Grid.BackgroundProperty, new SolidColorBrush(Colors.LightSkyBlue));

            ////// Create check box
            ////FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof(CheckBox));
            ////checkBox.Name = "chk";
            ////checkBox.SetValue(CheckBox.NameProperty, "chk");
            ////checkBox.SetValue(CheckBox.TagProperty, new Binding());
            ////checkBox.SetValue(CheckBox.MarginProperty, new Thickness(2));
            ////checkBox.SetValue(CheckBox.TagProperty, new Binding() { Path = new PropertyPath("Name") });
            ////grid.AppendChild(checkBox);


            // Create Image 
            FrameworkElementFactory image = new FrameworkElementFactory(typeof (Image));
            image.SetValue(Image.MarginProperty, new Thickness(2));
            image.SetValue(Image.WidthProperty, Convert.ToDouble(32));
            image.SetValue(Image.HeightProperty, Convert.ToDouble(24));
            image.SetValue(Image.VerticalAlignmentProperty, VerticalAlignment.Center);
            image.SetValue(Image.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            image.SetBinding(Image.SourceProperty, new Binding() {Path = new PropertyPath("ImageUrl")});


            grid.AppendChild(image);


            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof (TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding() {Path = new PropertyPath("Name")});
            label.SetValue(TextBlock.MarginProperty, new Thickness(2));
            label.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            label.SetValue(TextBlock.ToolTipProperty, new Binding());

            grid.AppendChild(label);


            dataTemplate.ItemsSource = new Binding("Countries");

            //set the visual tree of the data template
            dataTemplate.VisualTree = grid;

            return dataTemplate;
        }

        #endregion
    }
}