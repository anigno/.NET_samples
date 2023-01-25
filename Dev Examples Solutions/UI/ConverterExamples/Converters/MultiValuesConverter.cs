#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace ConverterExamples.Converters
{
    public class MultiValuesConverter : IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool) values[0];
            string s = (string) values[1];
            return string.Format("Text:{0} Checked:{1}", s, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}