using AnignoraDetection.Camera;

namespace AnignoraDetection
{
    partial class CameraDetector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDownDetectionLevel = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxSaveVideo = new System.Windows.Forms.CheckBox();
            this.checkBoxSendByFtp = new System.Windows.Forms.CheckBox();
            this.checkBoxSetByEmail = new System.Windows.Forms.CheckBox();
            this.checkBoxActiveAlarm = new System.Windows.Forms.CheckBox();
            this.labelOneShotTriggeredMain = new AnignoraUI.Labels.LabelOneShotTriggered();
            this.progressBarGradientMain = new AnignoraUI.Gauges.ProgressBarGradient();
            this.cameraWindowMain = new AnignoraDetection.Camera.CameraWindow();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDetectionLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarGradientMain)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownDetectionLevel
            // 
            this.numericUpDownDetectionLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownDetectionLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.numericUpDownDetectionLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownDetectionLevel.DecimalPlaces = 2;
            this.numericUpDownDetectionLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.numericUpDownDetectionLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownDetectionLevel.Location = new System.Drawing.Point(96, 295);
            this.numericUpDownDetectionLevel.Name = "numericUpDownDetectionLevel";
            this.numericUpDownDetectionLevel.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownDetectionLevel.TabIndex = 1;
            this.numericUpDownDetectionLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownDetectionLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownDetectionLevel.ValueChanged += new System.EventHandler(this.onNumericUpDownDetectionLevelValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(11, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Detection Level";
            // 
            // checkBoxSaveVideo
            // 
            this.checkBoxSaveVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSaveVideo.AutoSize = true;
            this.checkBoxSaveVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxSaveVideo.Location = new System.Drawing.Point(179, 274);
            this.checkBoxSaveVideo.Name = "checkBoxSaveVideo";
            this.checkBoxSaveVideo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxSaveVideo.Size = new System.Drawing.Size(78, 17);
            this.checkBoxSaveVideo.TabIndex = 3;
            this.checkBoxSaveVideo.Text = "Save Video";
            this.checkBoxSaveVideo.UseVisualStyleBackColor = true;
            this.checkBoxSaveVideo.CheckedChanged += new System.EventHandler(this.onCheckBoxSaveVideoCheckedChanged);
            // 
            // checkBoxSendByFtp
            // 
            this.checkBoxSendByFtp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSendByFtp.AutoSize = true;
            this.checkBoxSendByFtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxSendByFtp.Location = new System.Drawing.Point(267, 274);
            this.checkBoxSendByFtp.Name = "checkBoxSendByFtp";
            this.checkBoxSendByFtp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxSendByFtp.Size = new System.Drawing.Size(86, 17);
            this.checkBoxSendByFtp.TabIndex = 4;
            this.checkBoxSendByFtp.Text = "Send By FTP";
            this.checkBoxSendByFtp.UseVisualStyleBackColor = true;
            // 
            // checkBoxSetByEmail
            // 
            this.checkBoxSetByEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSetByEmail.AutoSize = true;
            this.checkBoxSetByEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxSetByEmail.Location = new System.Drawing.Point(262, 295);
            this.checkBoxSetByEmail.Name = "checkBoxSetByEmail";
            this.checkBoxSetByEmail.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxSetByEmail.Size = new System.Drawing.Size(91, 17);
            this.checkBoxSetByEmail.TabIndex = 5;
            this.checkBoxSetByEmail.Text = "Send By Email";
            this.checkBoxSetByEmail.UseVisualStyleBackColor = true;
            // 
            // checkBoxActiveAlarm
            // 
            this.checkBoxActiveAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxActiveAlarm.AutoSize = true;
            this.checkBoxActiveAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxActiveAlarm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxActiveAlarm.Location = new System.Drawing.Point(11, 274);
            this.checkBoxActiveAlarm.Name = "checkBoxActiveAlarm";
            this.checkBoxActiveAlarm.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxActiveAlarm.Size = new System.Drawing.Size(82, 17);
            this.checkBoxActiveAlarm.TabIndex = 6;
            this.checkBoxActiveAlarm.Text = "Active Alarm";
            this.checkBoxActiveAlarm.UseVisualStyleBackColor = true;
            this.checkBoxActiveAlarm.CheckedChanged += new System.EventHandler(this.onCheckBoxActiveAlarmCheckedChanged);
            // 
            // labelOneShotTriggeredMain
            // 
            this.labelOneShotTriggeredMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOneShotTriggeredMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelOneShotTriggeredMain.Location = new System.Drawing.Point(337, 3);
            this.labelOneShotTriggeredMain.Name = "labelOneShotTriggeredMain";
            this.labelOneShotTriggeredMain.Size = new System.Drawing.Size(30, 23);
            this.labelOneShotTriggeredMain.TabIndex = 8;
            this.labelOneShotTriggeredMain.Text = "!";
            this.labelOneShotTriggeredMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelOneShotTriggeredMain.TriggeredColor = System.Drawing.Color.Red;
            this.labelOneShotTriggeredMain.TriggeredIntervalMs = 1000;
            // 
            // progressBarGradientMain
            // 
            this.progressBarGradientMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarGradientMain.BarOrientation = System.Windows.Forms.Orientation.Vertical;
            this.progressBarGradientMain.BarText = "";
            this.progressBarGradientMain.ColorFirst = System.Drawing.Color.Maroon;
            this.progressBarGradientMain.ColorSecond = System.Drawing.Color.Red;
            this.progressBarGradientMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.progressBarGradientMain.LastMouseUpValue = 0;
            this.progressBarGradientMain.Location = new System.Drawing.Point(340, 29);
            this.progressBarGradientMain.Maximum = 100;
            this.progressBarGradientMain.Minimum = 0;
            this.progressBarGradientMain.Name = "progressBarGradientMain";
            this.progressBarGradientMain.Size = new System.Drawing.Size(27, 239);
            this.progressBarGradientMain.TabIndex = 7;
            this.progressBarGradientMain.TabStop = false;
            this.progressBarGradientMain.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.progressBarGradientMain.Value = 90;
            // 
            // cameraWindowMain
            // 
            this.cameraWindowMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraWindowMain.BackColor = System.Drawing.Color.Black;
            this.cameraWindowMain.Camera = null;
            this.cameraWindowMain.Location = new System.Drawing.Point(3, 3);
            this.cameraWindowMain.Name = "cameraWindowMain";
            this.cameraWindowMain.Size = new System.Drawing.Size(331, 265);
            this.cameraWindowMain.TabIndex = 0;
            this.cameraWindowMain.Text = "cameraWindow";
            // 
            // CameraDetector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.labelOneShotTriggeredMain);
            this.Controls.Add(this.progressBarGradientMain);
            this.Controls.Add(this.checkBoxActiveAlarm);
            this.Controls.Add(this.checkBoxSetByEmail);
            this.Controls.Add(this.checkBoxSendByFtp);
            this.Controls.Add(this.checkBoxSaveVideo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownDetectionLevel);
            this.Controls.Add(this.cameraWindowMain);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Name = "CameraDetector";
            this.Size = new System.Drawing.Size(370, 320);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDetectionLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarGradientMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CameraWindow cameraWindowMain;
        private System.Windows.Forms.NumericUpDown numericUpDownDetectionLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxSaveVideo;
        private System.Windows.Forms.CheckBox checkBoxSendByFtp;
        private System.Windows.Forms.CheckBox checkBoxSetByEmail;
        private System.Windows.Forms.CheckBox checkBoxActiveAlarm;
        private AnignoraUI.Gauges.ProgressBarGradient progressBarGradientMain;
        private AnignoraUI.Labels.LabelOneShotTriggered labelOneShotTriggeredMain;
    }
}
