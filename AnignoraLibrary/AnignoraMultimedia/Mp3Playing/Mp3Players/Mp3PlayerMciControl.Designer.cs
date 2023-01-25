namespace AnignoraMultimedia.Mp3Playing.Mp3Players
{
    partial class Mp3PlayerMciControl
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
            this.components = new System.ComponentModel.Container();
            this.progressBarTime = new AnignoraUI.ProgressBarsAndGauges.ProgressBarGradient();
            this.buttonStop = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonPause = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonRew = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonPlay = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonFwd = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarTime)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarTime
            // 
            this.progressBarTime.BarOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.progressBarTime.BarText = "00:00";
            this.progressBarTime.ColorFirst = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarTime.ColorSecond = System.Drawing.Color.Black;
            this.progressBarTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.progressBarTime.LastMouseUpValue = 0;
            this.progressBarTime.Location = new System.Drawing.Point(3, 33);
            this.progressBarTime.Maximum = 10;
            this.progressBarTime.Minimum = 0;
            this.progressBarTime.Name = "progressBarTime";
            this.progressBarTime.Size = new System.Drawing.Size(174, 23);
            this.progressBarTime.TabIndex = 5;
            this.progressBarTime.TabStop = false;
            this.progressBarTime.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.progressBarTime.Value = 10;
            this.progressBarTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.progressBarTime_MouseUp);
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.Black;
            this.buttonStop.CornerSize = 3;
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonStop.GlowColor = System.Drawing.Color.Blue;
            this.buttonStop.GlowIntensity = ((uint)(100u));
            this.buttonStop.GradientAngle = 270;
            this.buttonStop.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonStop.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonStop.Location = new System.Drawing.Point(3, 0);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(30, 30);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "[]";
            this.buttonStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.BackColor = System.Drawing.Color.Black;
            this.buttonPause.CornerSize = 3;
            this.buttonPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonPause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonPause.GlowColor = System.Drawing.Color.Blue;
            this.buttonPause.GlowIntensity = ((uint)(100u));
            this.buttonPause.GradientAngle = 270;
            this.buttonPause.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonPause.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonPause.Location = new System.Drawing.Point(39, 0);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(30, 30);
            this.buttonPause.TabIndex = 3;
            this.buttonPause.Text = "| |";
            this.buttonPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonRew
            // 
            this.buttonRew.BackColor = System.Drawing.Color.Black;
            this.buttonRew.CornerSize = 3;
            this.buttonRew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonRew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonRew.GlowColor = System.Drawing.Color.Blue;
            this.buttonRew.GlowIntensity = ((uint)(100u));
            this.buttonRew.GradientAngle = 270;
            this.buttonRew.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonRew.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonRew.Location = new System.Drawing.Point(75, 0);
            this.buttonRew.Name = "buttonRew";
            this.buttonRew.Size = new System.Drawing.Size(30, 30);
            this.buttonRew.TabIndex = 2;
            this.buttonRew.Text = "<<";
            this.buttonRew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRew.Click += new System.EventHandler(this.buttonRew_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackColor = System.Drawing.Color.Black;
            this.buttonPlay.CornerSize = 3;
            this.buttonPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonPlay.GlowColor = System.Drawing.Color.Blue;
            this.buttonPlay.GlowIntensity = ((uint)(100u));
            this.buttonPlay.GradientAngle = 270;
            this.buttonPlay.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonPlay.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonPlay.Location = new System.Drawing.Point(111, 0);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(30, 30);
            this.buttonPlay.TabIndex = 1;
            this.buttonPlay.Text = ">";
            this.buttonPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonFwd
            // 
            this.buttonFwd.BackColor = System.Drawing.Color.Black;
            this.buttonFwd.CornerSize = 3;
            this.buttonFwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.buttonFwd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonFwd.GlowColor = System.Drawing.Color.Blue;
            this.buttonFwd.GlowIntensity = ((uint)(100u));
            this.buttonFwd.GradientAngle = 270;
            this.buttonFwd.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonFwd.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonFwd.Location = new System.Drawing.Point(147, 0);
            this.buttonFwd.Name = "buttonFwd";
            this.buttonFwd.Size = new System.Drawing.Size(30, 30);
            this.buttonFwd.TabIndex = 0;
            this.buttonFwd.Text = ">>";
            this.buttonFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonFwd.Click += new System.EventHandler(this.buttonFwd_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 500;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // ucMp3PlayerMCI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.progressBarTime);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonRew);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.buttonFwd);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Name = "Mp3PlayerMciControl";
            this.Size = new System.Drawing.Size(181, 57);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AnignoraUI.Buttons.ButtonGradientGlow buttonFwd;
        private AnignoraUI.Buttons.ButtonGradientGlow buttonPlay;
        private AnignoraUI.Buttons.ButtonGradientGlow buttonRew;
        private AnignoraUI.Buttons.ButtonGradientGlow buttonPause;
        private AnignoraUI.Buttons.ButtonGradientGlow buttonStop;
        private AnignoraUI.ProgressBarsAndGauges.ProgressBarGradient progressBarTime;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
