#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#endregion

namespace ConverterExamples.Converters
{
    [ValueConversion(typeof (int), typeof (string))]
    public class EnumIntToStringConverterWithParams : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            String[] itemDescriptions = p_parameter as String[];
            int val = (int) p_value;
            if (itemDescriptions != null) return itemDescriptions[val];
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            //No convert back
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}