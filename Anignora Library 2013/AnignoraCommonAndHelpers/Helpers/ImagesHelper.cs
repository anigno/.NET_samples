using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class ImagesHelper
    {
        #region Public Methods

        /// <summary>
        /// Returns a new Image which size doesn't extends MaxSize
        /// </summary>
        /// <param name="image"></param>
        /// <param name="MaxSize"></param>
        /// <returns></returns>
        public static Image GetResizedImage(Image image, Size MaxSize)
        {
            int newWidth = image.Width;
            int newHeight = image.Height;
            if (newWidth > MaxSize.Width)
            {
                float factor = newWidth/(float) (MaxSize.Width);
                newWidth = (int) (newWidth/factor);
                newHeight = (int) (newHeight/factor);
            }
            if (newHeight > MaxSize.Height)
            {
                float factor = newHeight/(float) (MaxSize.Height);
                newWidth = (int) (newWidth/factor);
                newHeight = (int) (newHeight/factor);
            }
            return image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
        }


        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            // Encoder parameter for image quality 
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        #endregion

        #region Private Methods

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        #endregion
    }
}