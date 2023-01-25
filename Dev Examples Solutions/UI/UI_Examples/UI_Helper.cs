using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace UI_Examples
{
    public class UI_Helper
    {
        public static ImageSource BytesToImageSource(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            ImageSource imgSrc = biImg as ImageSource;
            return imgSrc;
        }

        public static byte[] GetRgbValues(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmpData.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            return rgbValues;
        }

        public static void DrawOnBitMap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawLine(new System.Drawing.Pen(new SolidBrush(Color.Yellow)), new Point(0, 0), new Point(599, 599));
                g.DrawLine(new System.Drawing.Pen(new SolidBrush(Color.Green)), new Point(607, 599), new Point(8, 0));
                g.DrawLine(new System.Drawing.Pen(new SolidBrush(Color.Blue)), new Point(0, 599), new Point(599, 0));
                g.DrawLine(new System.Drawing.Pen(new SolidBrush(Color.Red)), new Point(607, 0), new Point(8, 599));
                g.DrawString("Hello WORLD", new Font(new System.Drawing.FontFamily(GenericFontFamilies.SansSerif), 10), new SolidBrush(Color.White), new PointF(100, 100));
            }
        }
        public static void SetPixleOnRgbArray(byte[] image, int x, int y, byte r, byte g, byte b)
        {
            int index0 = x * 3 + y * 608 * 3;
            image[index0 + 0] = r;
            image[index0 + 1] = g;
            image[index0 + 2] = b;
        }

    }
}
