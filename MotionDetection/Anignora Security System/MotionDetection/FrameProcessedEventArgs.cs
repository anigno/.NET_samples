using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Anignora_Security_System.MotionDetection
{
    public class FrameProcessedEventArgs : EventArgs
    {
        #region (------  Constructors  ------)
        public FrameProcessedEventArgs(Bitmap p_frameBitmap, float p_lastDetectionValue,string p_cameraName)
        {
            FrameBitmap = p_frameBitmap;
            LastDetectedValue = p_lastDetectionValue;
        }
        #endregion (------  Constructors  ------)

        #region (------  Properties  ------)
        public Bitmap FrameBitmap { get; private set; }

        public BitmapSource FrameBitmapSource { get; set; }

        public float LastDetectedValue { get; private set; }

        public string CameraName { get; private set; }
        #endregion (------  Properties  ------)
    }
}