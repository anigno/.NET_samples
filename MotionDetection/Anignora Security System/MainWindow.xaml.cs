using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using Anignora_Security_System.MotionDetection;
using WpfServices;

namespace Anignora_Security_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region (------  Fields  ------)

        #endregion (------  Fields  ------)

        #region (------  Constructors  ------)

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            FilterInfoCollection filtersInfo = AMotionDetector.GetFiltersInfo();
            foreach (FilterInfo filterInfo in filtersInfo)
            {
                MotionDetectionControl c = new MotionDetectionControl();
                c.Width = c.Height = 250;
                wrapPanelCams.Children.Add(c);
                c.Start(filterInfo);
                c.MotionDetected += onMotionDetected;
            }
        }

        private void saveImage(Bitmap p_bitmap,string p_cameraName)
        {
            const string CAPTURED_IMAGES_DIR = "CAPTURED_IMAGES";
            if (!Directory.Exists(CAPTURED_IMAGES_DIR)) Directory.CreateDirectory(CAPTURED_IMAGES_DIR);
            string filename = string.Format(@"{0}\image_{1}_{2}.jpg", CAPTURED_IMAGES_DIR, p_cameraName, DateTime.Now.ToString("yyy_MM_dd_hh_mm_ss.fff"));
            p_bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void onMotionDetected(object sender, FrameProcessedEventArgs e)
        {
            CrossThreadedActivities.Do(this, () =>
                {
                    if (checkBoxSaveImage.IsChecked != null && checkBoxSaveImage.IsChecked.Value)
                    {
                        saveImage(e.FrameBitmap, e.CameraName);
                    }
                });
        }

        #endregion (------  Constructors  ------)

        #region (------  Events Handlers  ------)


        private void onWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (MotionDetectionControl detectionControl in wrapPanelCams.Children)
            {
                detectionControl.Stop();
            }
        }

        #endregion (------  Events Handlers  ------)
    }
}