#region

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DataGridExamples.Data;

#endregion

namespace DataGridExamples.Converters
{
    [ValueConversion(typeof (StatusEnum), typeof (SolidColorBrush))]
    public class StatusEnumToSolidColorBrushConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            StatusEnum val = (StatusEnum) p_value;
            switch (val)
            {
                case StatusEnum.Unknown:
                    return new SolidColorBrush(Colors.Orange);
                case StatusEnum.InGarage:
                    return new SolidColorBrush(Colors.Red);
                case StatusEnum.OnRoad:
                    return new SolidColorBrush(Colors.Green);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}