#region

using System.Windows;
using System.Windows.Controls;
using WPF.Themes;

#endregion

namespace ThemesExample
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
            string[] themes = ThemeManager.GetThemes();
            foreach (string theme in themes)
            {
                ListBoxThemes.Items.Add(theme);
            }
        }

        #endregion

        #region Private Methods

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            string selectedTheme = ListBoxThemes.SelectedItem.ToString();
            this.ApplyTheme(selectedTheme);
        }

        private void ListBoxThemes_OnSelectionChanged(object p_sender, SelectionChangedEventArgs p_e)
        {
        }

        #endregion
    }
}