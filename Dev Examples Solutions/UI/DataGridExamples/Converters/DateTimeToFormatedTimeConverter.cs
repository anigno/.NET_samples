#region

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace DataGridExamples.Converters
{
    [ValueConversion(typeof (DateTime), typeof (DateTime))]
    public class DateTimeToFormatedTimeConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            DateTime val = (DateTime) p_value;
            return val.ToString(Format);
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Properties

        public string Format { get; set; }

        #endregion
    }
}