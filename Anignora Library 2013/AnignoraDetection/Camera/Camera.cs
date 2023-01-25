// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
using AnignoraCommonAndHelpers.Helpers;
using AnignoraDetection.MotionDetectors;
using AnignoraDetection.VideoSource;
using System;
using System.Drawing;
using System.Threading;

namespace AnignoraDetection.Camera
{
    /// <summary>
    /// Camera class
    /// </summary>
    public class Camera
    {
        #region (------  Fields  ------)

        // alarm level
        private double m_alarmLevel = 0.005;
        private Bitmap m_lastFrame = null;
        private IMotionDetector m_motionDetecotor = null;
        private readonly IVideoSource m_videoSource = null;
        // image width and height
        private int m_width = -1, m_height = -1;

        #endregion (------  Fields  ------)

        #region (------  Events  ------)

        public event EventHandler Alarm;

        public event EventHandler NewFrame;

        #endregion (------  Events  ------)

        #region (------  Constructors  ------)

        public Camera(IVideoSource p_source, IMotionDetector p_detector)
        {
            m_videoSource = p_source;
            m_motionDetecotor = p_detector;
        }

        // Constructor
        public Camera(IVideoSource p_source)
            : this(p_source, null)
        { }

        #endregion (------  Constructors  ------)

        #region (------  Properties  ------)

        /// <summary>
        /// Alarm level motion detected percentage (0.0 - 1.0)
        /// </summary>
        public double AlarmLevel
        {
            get { return m_alarmLevel; }
            set { m_alarmLevel = value; }
        }

        // BytesReceived property
        public int BytesReceived
        {
            get { return (m_videoSource == null) ? 0 : m_videoSource.BytesReceived; }
        }

        // FramesReceived property
        public int FramesReceived
        {
            get { return (m_videoSource == null) ? 0 : m_videoSource.FramesReceived; }
        }

        // Height property
        public int Height
        {
            get { return m_height; }
        }

        // LastFrame property
        public Bitmap LastFrame
        {
            get { return m_lastFrame; }
        }

        // MotionDetector property
        public IMotionDetector MotionDetector
        {
            get { return m_motionDetecotor; }
            set { m_motionDetecotor = value; }
        }

        // Running property
        public bool Running
        {
            get { return (m_videoSource == null) ? false : m_videoSource.Running; }
        }

        // Width property
        public int Width
        {
            get { return m_width; }
        }

        #endregion (------  Properties  ------)

        #region (------  Public Methods  ------)

        public void ClearAllEventsRegistrations()
        {
            EventHalper.ClearEventRegistration(Alarm);
            EventHalper.ClearEventRegistration(NewFrame);
        }

        // Lock it
        public void Lock()
        {
            Monitor.Enter(this);
        }

        // Siganl video source to stop
        public void SignalToStop()
        {
            if (m_videoSource != null)
            {
                m_videoSource.SignalToStop();
            }
        }

        // Start video source
        public void Start()
        {
            if (m_videoSource != null)
            {
                m_videoSource.NewFrame += videoNewFrame;
                m_isClosing = false;
                m_videoSource.Start();
            }
        }
        private bool m_isClosing = false;


        // Abort Camera
        public void Stop()
        {
            m_videoSource.NewFrame -= videoNewFrame;
            // lock
            Monitor.Enter(this);
            m_isClosing = true;
            if (m_videoSource != null)
            {
                m_videoSource.Stop();
            }
            // unlock
            Monitor.Exit(this);
        }

        // Unlock it
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        // Wait video source for stop
        public void WaitForStop()
        {
            // lock
            Monitor.Enter(this);

            if (m_videoSource != null)
            {
                m_videoSource.WaitForStop();
            }
            // unlock
            Monitor.Exit(this);
        }

        #endregion (------  Public Methods  ------)

        #region (------  Private Methods  ------)

        // On new frame
        private void videoNewFrame(object p_sender, CameraEventArgs p_e)
        {
            if (m_isClosing) return;
            try
            {
                // lock
                Monitor.Enter(this);
                // dispose old frame
                if (m_lastFrame != null)
                {
                    m_lastFrame.Dispose();
                }

                m_lastFrame = (Bitmap)p_e.Bitmap.Clone();

                // apply motion detector
                if (m_motionDetecotor != null)
                {
                    m_motionDetecotor.ProcessFrame(ref m_lastFrame);

                    // check motion level
                    if (
                        (m_motionDetecotor.MotionLevel >= m_alarmLevel) &&
                        (Alarm != null)
                        )
                    {
                        Alarm(this, new EventArgs());
                    }
                }

                // image dimension
                m_width = m_lastFrame.Width;
                m_height = m_lastFrame.Height;
            }
            finally
            {
                // unlock
                Monitor.Exit(this);
            }

            // notify client
            if (NewFrame != null)
                NewFrame(this, new EventArgs());
        }

        #endregion (------  Private Methods  ------)
    }
}