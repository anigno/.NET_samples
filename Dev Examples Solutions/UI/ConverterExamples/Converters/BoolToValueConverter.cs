#region

using System;
using System.Windows.Data;

#endregion

namespace ConverterExamples.Converters
{
    [ValueConversion(typeof (bool), typeof (object))]
    public class BoolToValueConverter<T> : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            if (p_value == null)
                return FalseValue;
            else
                return (bool) p_value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            return p_value != null && p_value.Equals(TrueValue);
        }

        #endregion

        #region Public Properties

        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        #endregion
    }
}