#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataGridStyles
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
            DataContext = this;
            Persons = new ObservableCollection<PersonData>();

            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
            Persons.Add(new PersonData() {Id = 83839682, Name = "aaa bbb", Age = 45});
            Persons.Add(new PersonData() {Id = 23489234, Name = "ccc ddd", Age = 34});
        }

        #endregion

        #region Public Properties

        public ObservableCollection<PersonData> Persons { get; set; }

        #endregion
    }
}