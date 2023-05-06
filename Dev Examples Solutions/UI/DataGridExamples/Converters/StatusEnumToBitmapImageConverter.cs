#region

using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataGridExamples.Data;

#endregion

namespace DataGridExamples.Converters
{
    public class StatusEnumToBitmapImageConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatusEnum statusEnum = (StatusEnum) value;
            switch (statusEnum)
            {
                case StatusEnum.Unknown:
                    return null;
                case StatusEnum.InGarage:
                    var v1 = (BitmapImage) Application.Current.TryFindResource("_1");
                    return v1;
                case StatusEnum.OnRoad:
                    //Bitmap v = ImagesResource._1;
                    //BitmapImage bitmapImage=new BitmapImage();
                    //ResourceManager rm=new ResourceManager(typeof(ImagesResource));
                    //var o = rm.GetObject("_3");
                    //var v2= (BitmapImage)Application.Current.TryFindResource("_3");
                    ImageSource imageSource = doGetImageSourceFromResource(System.Reflection.Assembly.GetExecutingAssembly().FullName, "String1");
                    return imageSource;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        internal static ImageSource doGetImageSourceFromResource(string psAssemblyName, string psResourceName)
        {
            //Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
            //return BitmapFrame.Create(oUri);
            return null;
        }
    }
}