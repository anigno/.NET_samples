using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using AnignoraCommunication.Email;
using AnignoraDetection.dshow;
using AnignoraDetection.dshow.Core;
using AnignoraDetection.MotionDetectors;
using AnignoraDetection.VideoSource;
using AnignoraUI.Common;

namespace AnignoraDetection
{
    public partial class CameraDetector : UserControl
    {
		#region (------  Fields  ------)

        private const string CATEGORY_STRING = " CameraDetector";
        private readonly IMotionDetector m_motionDetector = new MotionDetector3Optimized();

		#endregion (------  Fields  ------)

		#region (------  Delegates  ------)

        public delegate void CameraDetectorDelegate(CameraDetector p_cameraDetector,Bitmap p_lastFrame);

		#endregion (------  Delegates  ------)

		#region (------  Events  ------)

        [Category(CATEGORY_STRING)]
        public event CameraDetectorDelegate CameraDetectorAlarm = delegate { };

        [Category(CATEGORY_STRING)]
        public event CameraDetectorDelegate CameraDetectorNewFrame = delegate { };

		#endregion (------  Events  ------)

		#region (------  Constructors  ------)

        public CameraDetector()
        {
            InitializeComponent();
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        [Category(CATEGORY_STRING)]
        public string EmailAddress { get; set; }
        [Category(CATEGORY_STRING)]
        public string EmailUsername { get; set; }
        [Category(CATEGORY_STRING)]
        public string EmailPassword { get; set; }
        [Category(CATEGORY_STRING)]
        public string FtpAddress { get; set; }
        [Category(CATEGORY_STRING)]
        public string FtpUsername { get; set; }
        [Category(CATEGORY_STRING)]
        public string FtpPassword { get; set; }

        /// <summary>
        /// Gets or sets if alarm event will be raised
        /// </summary>
        [Category(CATEGORY_STRING)]
        public bool ActiveAlarm
        {
            get { return checkBoxActiveAlarm.Checked; }
            set { checkBoxActiveAlarm.Checked = value; }
        }

        /// <summary>
        /// Get or set the triggering level of detection to fire an alert
        /// </summary>
        [Category(CATEGORY_STRING)]
        public decimal DetectionLevel
        {
            get { return numericUpDownDetectionLevel.Value; }
            set
            {
                numericUpDownDetectionLevel.Value = value;
                if (cameraWindowMain.Camera != null && cameraWindowMain.Camera.MotionDetector != null)
                {
                    cameraWindowMain.Camera.AlarmLevel = (double) value;
                }
            }
        }

        /// <summary>
        /// Gets the currently detection value
        /// </summary>
        [Category(CATEGORY_STRING)]
        public Double DetectionValue
        {
            get { return cameraWindowMain.Camera.MotionDetector.MotionLevel; }
        }

        [Category(CATEGORY_STRING)]
        public bool SaveVideo
        {
            get { return checkBoxSaveVideo.Checked; }
            set { checkBoxSaveVideo.Checked = value; }
        }

        [Category(CATEGORY_STRING)]
        public bool SendByEmail
        {
            get { return checkBoxSetByEmail.Checked; }
            set { checkBoxSetByEmail.Checked = value; }
        }

        [Category(CATEGORY_STRING)]
        public bool SendByFtp
        {
            get { return checkBoxSendByFtp.Checked; }
            set { checkBoxSendByFtp.Checked = value; }
        }

		#endregion (------  Properties  ------)

		#region (------  Events Handlers  ------)

        void onCameraAlarm(object p_sender, EventArgs p_e)
        {
            if (!ActiveAlarm) return;
            labelOneShotTriggeredMain.Trigger();
            Bitmap lastFrame = cameraWindowMain.Camera.LastFrame;
            CameraDetectorAlarm(this, lastFrame);
            lastFrame.Save("tmp.jpg",ImageFormat.Jpeg);
            if (SendByEmail) runSendByMail(lastFrame);
            if (SendByFtp) runSendByFtp(lastFrame);
            if (SaveVideo) runSaveVideo(lastFrame);
        }

        private void runSendByMail(Bitmap p_lastFrame)
        {
            EmailSendingService emailSendingService=new EmailSendingService();
            SmtpClientByGmail smtpClient=new SmtpClientByGmail(EmailAddress,EmailPassword,2);
            emailSendingService.SendEmailSync(smtpClient, "", "", EmailAddress, DateTime.Now.ToString(), "", "tmp.jpg");
            
        }
        private void runSendByFtp(Bitmap p_lastFrame)
        {
        }
        private void runSaveVideo(Bitmap p_lastFrame)
        {
        }


        void onCameraNewFrame(object p_sender, EventArgs p_e)
        {
            int value = (int)(Math.Sqrt(DetectionValue) * 100);
            CrossThreadActivities.Do(this, delegate
                                               {
                                                   progressBarGradientMain.Value = value;
                                                   progressBarGradientMain.BarText = (DetectionValue).ToString("0.00");
                                               });
            CameraDetectorNewFrame(this, cameraWindowMain.Camera.LastFrame);
        }

        private void onCheckBoxActiveAlarmCheckedChanged(object p_sender, EventArgs p_e)
        {
            ActiveAlarm = checkBoxActiveAlarm.Checked;
        }

        private void onCheckBoxSaveVideoCheckedChanged(object p_sender, EventArgs p_e)
        {
            SaveVideo = checkBoxSaveVideo.Checked;
        }

        private void onNumericUpDownDetectionLevelValueChanged(object p_sender, EventArgs p_e)
        {
            DetectionLevel = numericUpDownDetectionLevel.Value;
        }

		#endregion (------  Events Handlers  ------)

		#region (------  Public Methods  ------)

        public string[] GetConnectedWebCams()
        {
            FilterCollection filters = new FilterCollection(FilterCategory.VideoInputDevice);
            List<string> filterStrings=new List<string>();
            foreach (Filter filter in filters)
            {
                filterStrings.Add(filter.MonikerString);
            }
            return filterStrings.ToArray();
        }

        public void RemoveVideoSource()
        {
            if (cameraWindowMain.Camera == null) return;
            cameraWindowMain.StopInnerTimer();
            cameraWindowMain.Camera.Stop();
            cameraWindowMain.Camera.NewFrame -= onCameraNewFrame;
            cameraWindowMain.Camera.Alarm -= onCameraAlarm;
            cameraWindowMain.Camera.MotionDetector.Reset();
            cameraWindowMain.Camera.MotionDetector = null;
            cameraWindowMain.Camera = null;
        }

        public void SetVideoSource(IVideoSource p_videoSource)
        {
            cameraWindowMain.Camera=new Camera.Camera(p_videoSource,m_motionDetector);
            cameraWindowMain.Camera.AlarmLevel = (double)DetectionLevel;
            cameraWindowMain.Camera.NewFrame += onCameraNewFrame;
            cameraWindowMain.Camera.Alarm += onCameraAlarm;
            cameraWindowMain.Camera.MotionDetector.MotionLevelCalculation = true;
        }

        public void SetVideoSource(int p_webCamVideoSourceIndex)
        {
            string[] videoSources = GetConnectedWebCams();
            CaptureDevice device=new CaptureDevice();
            device.VideoSource = videoSources[p_webCamVideoSourceIndex];
            SetVideoSource(device);
        }

        public void Start()
        {
            cameraWindowMain.Camera.Start();
        }

        public void Stop()
        {
            RemoveVideoSource();
            Thread.Sleep(1000);
        }

		#endregion (------  Public Methods  ------)
    }
}
