using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AnignoraCommonAndHelpers.Helpers
{
    public static class ImageHelper
    {
        #region Public Methods

        public static void SaveJpeg(string p_path, Bitmap p_img, long p_quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, p_quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            p_img.Save(p_path, jpegCodec, encoderParams);
        }

        // Jpeg image codec
        //ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

        public static ImageCodecInfo GetEncoderInfo(string p_mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == p_mimeType)
                    return codecs[i];
            return null;
        }

        public static Image CropImage(Image p_img, Rectangle p_cropArea)
        {
            Bitmap bmpImage = new Bitmap(p_img);
            Bitmap bmpCrop = bmpImage.Clone(p_cropArea,
                bmpImage.PixelFormat);
            return (Image) (bmpCrop);
        }

        public static Bitmap RotateImage(Bitmap p_bitmap, float p_angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(p_bitmap.Width, p_bitmap.Height);
            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(returnBitmap);
            //move rotation point to center of image
            g.TranslateTransform((float) p_bitmap.Width/2, (float) p_bitmap.Height/2);
            //rotate
            g.RotateTransform(p_angle);
            //move image back
            g.TranslateTransform(-(float) p_bitmap.Width/2, -(float) p_bitmap.Height/2);
            //draw passed in image onto graphics object
            g.DrawImage(p_bitmap, new Point(0, 0));
            return returnBitmap;
        }

        public static Bitmap MakeGrayscale2(Bitmap original)
        {
            unsafe
            {
                //create an empty bitmap the same size as original
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                //lock the original bitmap in memory
                BitmapData originalData = original.LockBits(
                    new Rectangle(0, 0, original.Width, original.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                //lock the new bitmap in memory
                BitmapData newData = newBitmap.LockBits(
                    new Rectangle(0, 0, original.Width, original.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                //set the number of bytes per pixel
                int pixelSize = 3;

                for (int y = 0; y < original.Height; y++)
                {
                    //get the data from the original image
                    byte* oRow = (byte*) originalData.Scan0 + (y*originalData.Stride);

                    //get the data from the new image
                    byte* nRow = (byte*) newData.Scan0 + (y*newData.Stride);

                    for (int x = 0; x < original.Width; x++)
                    {
                        //create the grayscale version
                        byte grayScale =
                            (byte) ((oRow[x*pixelSize]*.11) + //B
                                    (oRow[x*pixelSize + 1]*.59) + //G
                                    (oRow[x*pixelSize + 2]*.3)); //R

                        //set the new image's pixel to the grayscale version
                        nRow[x*pixelSize] = grayScale; //B
                        nRow[x*pixelSize + 1] = grayScale; //G
                        nRow[x*pixelSize + 2] = grayScale; //R
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                original.UnlockBits(originalData);

                return newBitmap;
            }
        }

        #endregion

        #region Private Methods

        private static Image ResizeImage(Image p_imgToResize, Size p_size)
        {
            int sourceWidth = p_imgToResize.Width;
            int sourceHeight = p_imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float) p_size.Width/(float) sourceWidth);
            nPercentH = ((float) p_size.Height/(float) sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int) (sourceWidth*nPercent);
            int destHeight = (int) (sourceHeight*nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image) b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(p_imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image) b;
        }

        #endregion
    }
}