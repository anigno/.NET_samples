#region

using System;
using System.Windows.Data;

#endregion

namespace ConverterExamples.Converters
{
    public class BoolToStringWithDataResourceParameterConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            DataResource dataResource = (DataResource) p_parameter;
            string falseValue = "NOT " + dataResource.BindingTarget;
            string trueValue = "YES " + dataResource.BindingTarget;
            if (p_value == null)
                return falseValue;
            return (bool) p_value ? trueValue : falseValue;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, System.Globalization.CultureInfo p_culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}