using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThemesExamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataGridItemsSource=new ObservableCollection<SomeDataItem>();
            DataGridItemsSource.Add(new SomeDataItem(){Name = "AAA",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "bbb",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "ccc",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "AAA",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "bbb",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "ccc",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "AAA",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "bbb",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "ccc",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "AAA",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "bbb",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "ccc",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "AAA",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "bbb",Number = 123});
            DataGridItemsSource.Add(new SomeDataItem(){Name = "ccc",Number = 123});
        }

        public ObservableCollection<SomeDataItem> DataGridItemsSource { get; set; }
        public event PropertyChangedEventHandler PropertyChanged= delegate { };
    }

    public class SomeDataItem
    {
        public string  Name { get; set; }
        public int Number { get; set; }
    }
}
