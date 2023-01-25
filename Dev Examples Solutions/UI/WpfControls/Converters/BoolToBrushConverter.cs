#region

using System;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace WpfControls.Converters
{
    [ValueConversion(typeof (bool), typeof (object))]
    public class BoolToBrushConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            if (p_value == null) return FalseValue;
            return (bool) p_value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            return p_value != null && p_value.Equals(TrueValue);
        }

        #endregion

        #region Public Properties

        public GradientBrush FalseValue { get; set; }
        public GradientBrush TrueValue { get; set; }

        #endregion
    }
}