using System;
using System.Drawing;
using System.Threading;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;

namespace Anignora_Security_System.MotionDetection
{
    public class AMotionDetector
    {
		#region (------  Constants  ------)
        public const int START_DELAY = 5000;
		#endregion (------  Constants  ------)

		#region (------  Fields  ------)
        private string m_cameraName;
        private Timer m_delayTimer;
        private readonly MotionDetector m_detector;
        private bool m_startDelayPassed = false;
        private VideoCaptureDevice m_videoSource;
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        public event EventHandler<FrameProcessedEventArgs> FrameProcessed = delegate { };

        public event EventHandler<FrameProcessedEventArgs> MotionDetected = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public AMotionDetector()
        {
            m_delayTimer = new Timer(p_state => m_startDelayPassed = true, null, START_DELAY, int.MaxValue);
            TwoFramesDifferenceDetector twoFramesDifferenceDetector = new TwoFramesDifferenceDetector(true);
            MotionAreaHighlighting motionAreaHighlighting = new MotionAreaHighlighting(Color.Red);
            m_detector = new MotionDetector(twoFramesDifferenceDetector, motionAreaHighlighting);
            DetectionValue = 1.0f;
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public float DetectionValue { get; set; }
		#endregion (------  Properties  ------)

		#region (------  Events Handlers  ------)
        private void onNewFrame(object p_sender, AForge.Video.NewFrameEventArgs p_eventArgs)
        {
            Bitmap bitmap = p_eventArgs.Frame;
            float detectedValue = m_detector.ProcessFrame(bitmap);
            detectedValue = (float) (Math.Pow(detectedValue,0.25) );
            FrameProcessed(this, new FrameProcessedEventArgs(bitmap, detectedValue,m_cameraName));
            if (m_startDelayPassed && detectedValue > DetectionValue)
            {
                MotionDetected(this, new FrameProcessedEventArgs(bitmap, detectedValue,m_cameraName));
            }
        }
		#endregion (------  Events Handlers  ------)

		#region (------  Public static Methods  ------)
        public static FilterInfoCollection GetFiltersInfo()
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return videoDevices;
        }
		#endregion (------  Public static Methods  ------)

		#region (------  Public Methods  ------)
        public VideoCaptureDevice SetFilterInfo(FilterInfo p_filterInfo)
        {
            m_videoSource = new VideoCaptureDevice(p_filterInfo.MonikerString);
            m_cameraName = p_filterInfo.Name;
            return m_videoSource;
        }

        public void Start()
        {
            m_videoSource.Start();
            m_videoSource.NewFrame += onNewFrame;
        }

        public void Stop()
        {
            if (m_videoSource == null) return;
            m_videoSource.SignalToStop();
            m_videoSource.NewFrame -= onNewFrame;
        }
		#endregion (------  Public Methods  ------)
    }
}