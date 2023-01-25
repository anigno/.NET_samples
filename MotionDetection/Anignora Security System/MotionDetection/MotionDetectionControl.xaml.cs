using System;
using System.ComponentModel;
using System.Windows.Media;
using AForge.Video.DirectShow;
using WpfServices;

namespace Anignora_Security_System.MotionDetection
{
    /// <summary>
    /// Interaction logic for MotionDetectionControl.xaml
    /// </summary>
    public partial class MotionDetectionControl : INotifyPropertyChanged
    {
		#region (------  Fields  ------)
        readonly AMotionDetector m_motionDetector = new AMotionDetector();
        private Color m_sliderDetectionTriggerBackgroundColor;
        private string m_sourceHeader;
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        public event EventHandler<FrameProcessedEventArgs> MotionDetected = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public MotionDetectionControl()
        {
            DataContext = this;
            InitializeComponent();
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public int DetectionTrigger
        {
            get { return (int) (m_motionDetector.DetectionValue*100); }
            set
            {
                m_motionDetector.DetectionValue = value/100f;
                PropertyChanged(this, new PropertyChangedEventArgs("DetectionTrigger"));
            }
        }

        public Color SliderDetectionTriggerBackgroundColor
        {
            get { return m_sliderDetectionTriggerBackgroundColor; }
            set
            {
                m_sliderDetectionTriggerBackgroundColor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SliderDetectionTriggerBackgroundColor"));
            }
        }

        public string SourceHeader
        {
            get { return m_sourceHeader; }
            set
            {
                m_sourceHeader = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SourceHeader"));
            }
        }
		#endregion (------  Properties  ------)

		#region (------  Events Handlers  ------)
        void onMotionDetectorFrameProcessed(object p_sender, FrameProcessedEventArgs p_e)
        {
            SliderDetectionTriggerBackgroundColor = Colors.Transparent;

            CrossThreadedActivities.Do(this, () =>
            {
                ImageMain.Source = p_e.FrameBitmap.ToBitmapSource();
                ProgressBarValue.Value = p_e.LastDetectedValue * 100;
                LabelValue.Content = (int)(p_e.LastDetectedValue * 100) + " %";
            });
        }

        void onMotionDetectorMotionDetected(object p_sender, FrameProcessedEventArgs p_e)
        {
            SliderDetectionTriggerBackgroundColor = Colors.Red;
            MotionDetected(this, p_e);
        }
		#endregion (------  Events Handlers  ------)

		#region (------  Public Methods  ------)
        public static FilterInfoCollection GetFiltersInfo()
        {
            return AMotionDetector.GetFiltersInfo();
        }

        public void Start(FilterInfo p_filter)
        {
            VideoCaptureDevice videoCaptureDevice=m_motionDetector.SetFilterInfo(p_filter);
            m_motionDetector.FrameProcessed += onMotionDetectorFrameProcessed;
            m_motionDetector.MotionDetected += onMotionDetectorMotionDetected;
            m_motionDetector.Start();
            SourceHeader = p_filter.Name;
        }

        public void Stop()
        {
            m_motionDetector.Stop();
            m_motionDetector.FrameProcessed -= onMotionDetectorFrameProcessed;
            m_motionDetector.MotionDetected -= onMotionDetectorMotionDetected;
        }
		#endregion (------  Public Methods  ------)

        private void ImageMain_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
