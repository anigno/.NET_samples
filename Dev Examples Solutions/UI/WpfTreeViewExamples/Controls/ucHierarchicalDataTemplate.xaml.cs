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
    /// Interaction logic for ucHierarchicalDataTemplate.xaml
    /// </summary>
    public partial class ucHierarchicalDataTemplate : UserControl
    {
        #region Constructors

        public ucHierarchicalDataTemplate()
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
            tvMain.ItemsSource = WorldArea.GetAll();
        }

        #endregion
    }
}