// Motion Detector
//
// Copyright � Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
using System;

namespace AnignoraDetection.VideoSource
{
    // NewFrame delegate
	public delegate void CameraEventHandler(object sender, CameraEventArgs e);

	/// <summary>
	/// Camera event arguments
	/// </summary>
	public class CameraEventArgs : EventArgs
	{
		private System.Drawing.Bitmap bmp;

		// Constructor
		public CameraEventArgs(System.Drawing.Bitmap bmp)
		{
			this.bmp = bmp;
		}

		// Bitmap property
		public System.Drawing.Bitmap Bitmap
		{
			get { return bmp; }
		}
	}
}