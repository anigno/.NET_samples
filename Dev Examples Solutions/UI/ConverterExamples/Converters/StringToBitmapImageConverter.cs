#region

using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#endregion

namespace ConverterExamples.Converters
{
    public class StringToBitmapImageConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string) value;
            try
            {
                BitmapImage b = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + DirectoryPath + "/" + s + ".bmp"));
                return b;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Properties

        public string DirectoryPath { get; set; }

        #endregion
    }
}