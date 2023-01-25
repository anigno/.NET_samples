#region

using System;
using System.Collections.Generic;
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
using System.Reflection;

#endregion

namespace WPF.Themes.Demo
{
    public class Person
    {
        #region Public Properties

        public string Name { get; set; }
        public string Surname { get; set; }

        #endregion
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region Constructors

        public Window1()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            themes.ItemsSource = ThemeManager.GetThemes();

            comboBox.ItemsSource = SouthPark;
            listBox.ItemsSource = SouthPark;
        }

        private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string theme = e.AddedItems[0].ToString();

                // Window Level
                // this.ApplyTheme(theme);

                // Application Level
                // Application.Current.ApplyTheme(theme);
            }
        }

        #endregion

        #region Fields

        private List<Person> SouthPark = new List<Person>()
        {
            new Person() {Name = "Eric", Surname = "Cartman"},
            new Person() {Name = "Stan", Surname = "Marsh"},
            new Person() {Name = "Kyle", Surname = "Broflovski"},
            new Person() {Name = "Kenny", Surname = "McCormick"},
            new Person() {Name = "Bebe", Surname = "Stevens"},
            new Person() {Name = "Clyde", Surname = "Donovan"},
            new Person() {Name = "Craig", Surname = "Tucker"},
            new Person() {Name = "Jimmy", Surname = "Vulmer"},
            new Person() {Name = "Pip", Surname = "Pirrup"},
            new Person() {Name = "Token", Surname = "Black"},
            new Person() {Name = "Tweek", Surname = "Tweak"},
            new Person() {Name = "Wendy", Surname = "Testaburger"},
            new Person() {Name = "Annie", Surname = "Polk"},
            new Person() {Name = "Randy", Surname = "Marsh"},
            new Person() {Name = "Sharon", Surname = "Marsh"},
            new Person() {Name = "Shelley", Surname = "Marsh"},
            new Person() {Name = "Marvin", Surname = "Marsh"},
            new Person() {Name = "Jimbo", Surname = "Kern"},
            new Person() {Name = "Gerald", Surname = "Broflovski"},
            new Person() {Name = "Sheila", Surname = "Broflovski"},
            new Person() {Name = "Ike", Surname = "Broflovski"},
            new Person() {Name = "Kyle", Surname = "Schwartz"},
            new Person() {Name = "Liane", Surname = "Cartman"},
            new Person() {Name = "Stuart", Surname = "McCormick"},
            new Person() {Name = "Carol", Surname = "McCormick"},
            new Person() {Name = "Kevin", Surname = "McCormick"},
            new Person() {Name = "Stephen", Surname = "Stotch"},
            new Person() {Name = "Linda", Surname = "Stotch"},
            new Person() {Name = "Richard", Surname = "Tweak"}
        };

        #endregion
    }
}