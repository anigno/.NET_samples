#region

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

#endregion

//(?<DATA>)

namespace RegExTester
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

        private void ButtonMatch_Click(object sender, RoutedEventArgs e)
        {
            tryMatch();
        }

        private void TextBlockRegEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            tryMatch();
        }

        private void tryMatch()
        {
            ListBoxResults.Items.Clear();
            try
            {
                MatchCollection matches = Regex.Matches(TextBlockSource.Text, TextBlockRegEx.Text, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match match in matches)
                {
                    string s = string.Format("[{0}][{1}]", match, match.Result(@"${DATA}"));
                    ListBoxResults.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                ListBoxResults.Items.Add("Error: " + ex.ToString());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText("Regex.txt", TextBlockRegEx.Text);
            File.WriteAllText("source.txt", TextBlockSource.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBlockRegEx.Text = File.ReadAllText("Regex.txt");
                TextBlockSource.Text = File.ReadAllText("source.txt");
            }
            catch
            {
                TextBlockRegEx.Text = "(?<DATA>)";
            }
        }

        private void TextBlockSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            tryMatch();
        }

        #endregion
    }
}