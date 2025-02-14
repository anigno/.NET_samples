// Motion Detector
//
// Copyright � Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace AnignoraDetection.Camera
{
	/// <summary>
	/// Summary description for CameraWindow.
	/// </summary>
	public class CameraWindow : System.Windows.Forms.Control
	{
		#region (------  Fields  ------)

		private bool	autosize = false;
		private Camera	m_camera = null;
		private bool	firstFrame = true;
		private int		flash = 0;
		private bool	needSizeUpdate = false;
		private Color	rectColor = Color.Black;
		private System.Timers.Timer timer;

		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

		// Constructor
		public CameraWindow( )
		{
			InitializeComponent( );

			SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
				ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true );
		}

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

		// AutoSize property
		[DefaultValue(false)]
		public bool AutoSize
		{
			get { return autosize; }
			set
			{
				autosize = value;
				UpdatePosition( );
			}
		}

		/// <summary>
		/// Get or Set the camera, and registed camera events. Set to null to unregister to camera events
		/// </summary>
		[Browsable(false)]
		public Camera Camera
		{
			get { return m_camera; }
			set
			{
				// lock
				Monitor.Enter( this );

				// detach event
				if ( m_camera != null )
				{
					m_camera.NewFrame	-= new EventHandler( camera_NewFrame );
					m_camera.Alarm	-= new EventHandler( camera_Alarm );
					timer.Stop( );
				}

				m_camera			= value;
				needSizeUpdate	= true;
				firstFrame		= true;
				flash			= 0;

				// atach event
				if ( m_camera != null )
				{
					m_camera.NewFrame += new EventHandler( camera_NewFrame );
					m_camera.Alarm	+= new EventHandler( camera_Alarm );
					timer.Start( );
				}

				// unlock
				Monitor.Exit( this );
			}
		}

		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

		// Update position and size of the control
		public void UpdatePosition( )
		{
			// lock
			Monitor.Enter( this );

			if ( ( autosize ) && ( this.Parent != null ) )
			{
				Rectangle	rc = this.Parent.ClientRectangle;
				int			width = 320;
				int			height = 240;

				if ( m_camera != null )
				{
					m_camera.Lock( );

					// get frame dimension
					if ( m_camera.LastFrame != null )
					{
						width = m_camera.LastFrame.Width;
						height = m_camera.LastFrame.Height;
					}
					m_camera.Unlock( );
				}

				//
				this.SuspendLayout( );
				this.Location = new Point( ( rc.Width - width - 2 ) / 2, ( rc.Height - height - 2 ) / 2 );
				this.Size = new Size( width + 2, height + 2 );
				this.ResumeLayout( );

			}
			// unlock
			Monitor.Exit( this );
		}

		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)

		// Paint control
		protected override void OnPaint( PaintEventArgs pe )
		{
			if ( ( needSizeUpdate ) || ( firstFrame ) )
			{
				UpdatePosition( );
				needSizeUpdate = false;
			}

			// lock
			Monitor.Enter( this );

			Graphics	g = pe.Graphics;
			Rectangle	rc = this.ClientRectangle;
			Pen			pen = new Pen( rectColor, 1 );

			// draw rectangle
			g.DrawRectangle( pen, rc.X, rc.Y, rc.Width - 1, rc.Height - 1 );

			if ( m_camera != null )
			{
				try
				{
					m_camera.Lock( );

					// draw frame
					if ( m_camera.LastFrame != null )
					{
						g.DrawImage( m_camera.LastFrame, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2 );
						firstFrame = false;
					}
					else
					{
						// Create font and brush
						Font drawFont = new Font( "Arial", 12 );
						SolidBrush drawBrush = new SolidBrush( Color.White );

						g.DrawString( "Connecting ...", drawFont, drawBrush, new PointF( 5, 5 ) );

						drawBrush.Dispose( );
						drawFont.Dispose( );
					}
				}
				catch ( Exception )
				{
				}
				finally
				{
					m_camera.Unlock( );
				}
			}

			pen.Dispose( );

			// unlock
			Monitor.Exit( this );

			base.OnPaint( pe );
		}

        public void StopInnerTimer()
        {
            timer.Stop();
        }

	    #endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

		// On alarm
		private void camera_Alarm( object sender, System.EventArgs e )
		{
			// flash for 2 seconds
			flash = (int) ( 2 * ( 1000 / timer.Interval ) );
		}

		// On new frame ready
		private void camera_NewFrame( object sender, System.EventArgs e )
		{
			Invalidate();
		}

		// On timer
		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if ( flash > 0 )
			{
				// calculate color
				if ( --flash == 0 )
				{
					rectColor = Color.Black;
				}
				else
				{
					rectColor = ( rectColor == Color.Red ) ? Color.Black : Color.Red;
				}

				// draw rectangle
				if ( !needSizeUpdate )
				{
					Graphics	g = this.CreateGraphics( );
					Rectangle	rc = this.ClientRectangle;
					Pen			pen = new Pen( rectColor, 1 );

					// draw rectangle
					g.DrawRectangle( pen, rc.X, rc.Y, rc.Width - 1, rc.Height - 1 );

					g.Dispose( );
					pen.Dispose( );
				}
			}
		}

		#endregion (------  Private Methods  ------)



		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
			this.timer = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
			// 
			// timer
			// 
			this.timer.Interval = 250;
			this.timer.SynchronizingObject = this;
			this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
			((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();

		}
		#endregion
	}
}
