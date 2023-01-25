#region

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DataGridExamples.Data;

#endregion

namespace DataGridExamples.Converters
{
    public class StatusEnumToBitmapImageByFileConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatusEnum statusEnum = (StatusEnum) value;
            BitmapImage b;
            switch (statusEnum)
            {
                case StatusEnum.Unknown:
                    b = null;
                    break;
                case StatusEnum.InGarage:
                    b = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + ImagesPath + "/" + 1 + ".bmp"));
                    return b;
                case StatusEnum.OnRoad:
                    b = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + ImagesPath + "/" + 3 + ".bmp"));
                    return b;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion

        #region Public Properties

        public string ImagesPath { get; set; }

        #endregion
    }
}