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
using WpfTreeViewExamples.Controls;

#endregion

namespace WpfTreeViewExamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void ShowControl(Control control, string title)
        {
            control.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            control.Width = double.NaN;
            control.Height = double.NaN;

            gvContainer.Children.Clear();
            gvContainer.Children.Add(control);


            lblTitle.Content = title;
        }

        private void btnSimple_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucSimpleTree(), "Simple Tree");
        }

        private void btnSimpleCodeBehind_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucSimpleTreeCodeBehind(), "Simple Tree using code behind");
        }

        private void btnCustomByXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucCustomTree(), "Custom Tree");
        }

        private void btnCustomByCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucCustomTreeCodeBehind(), "Custom Tree using code behind");
        }

        private void btnCustomByInheritance_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucCustomTreeCustomClass(), "Custom Tree by custom class");
        }

        private void btnHeaderTemplateByXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucTreeHeaderTemplate(), "Create Header Template");
        }

        private void btnHeaderTemplateByCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucTreeHeaderTemplateCodeBehind(), "Create Header Template from code behind");
        }

        private void btnBindingAddSimpleObject_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucAddSimpleObject(), "Add Simple object");
        }

        private void btnBindingAddSimpleObjectUsingCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucAddSimpleObjectUsingCodeBehind(), "Add Simple object from code behind");
        }

        private void btnDataBinding_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucDataBinding(), "Data Binding");
        }

        private void btnDataBindingUsingCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucDataBindCodeBehind(), "Data Binding using code behind");
        }

        private void btnDataTemplate_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucDataTemplate(), "Data Template");
        }

        private void btnHierarchicalDataTemplate_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ucHierarchicalDataTemplate(), "Hierarchical Data Template");
        }

        #endregion
    }
}