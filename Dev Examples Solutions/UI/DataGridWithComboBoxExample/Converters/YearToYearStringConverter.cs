#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace DataGridWithComboBoxExample.Converters
{
    public class YearToYearStringConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string shortYearString = MainWindow.YearsDictionary[(uint) value];
            return shortYearString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    public class YearToKeyValueConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            uint year = (uint) value;
            string sYear = MainWindow.YearsDictionary[year];
            return new KeyValuePair<uint, string>(year, sYear);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            KeyValuePair<uint, string> pair = (KeyValuePair<uint, string>) value;
            return pair.Key;
        }

        #endregion
    }
}