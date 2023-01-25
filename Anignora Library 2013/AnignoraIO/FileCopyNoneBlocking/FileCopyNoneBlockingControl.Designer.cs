namespace AnignoraIO.FileCopyNoneBlocking
{
    partial class FileCopyNoneBlockingControl
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
            this.labelSource = new System.Windows.Forms.Label();
            this.labelDestination = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAbort = new System.Windows.Forms.Button();
            this.anignoProgressBarFlatCopyProgress = new AnignoraUI.ProgressBarsAndGauges.AnignoProgressBarFlat();
            this.SuspendLayout();
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(72, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(40, 13);
            this.labelSource.TabIndex = 1;
            this.labelSource.Text = "No File";
            // 
            // labelDestination
            // 
            this.labelDestination.AutoSize = true;
            this.labelDestination.Location = new System.Drawing.Point(72, 13);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(40, 13);
            this.labelDestination.TabIndex = 2;
            this.labelDestination.Text = "No File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Destination:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Source:";
            // 
            // buttonAbort
            // 
            this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbort.Location = new System.Drawing.Point(413, 29);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(46, 23);
            this.buttonAbort.TabIndex = 5;
            this.buttonAbort.Text = "Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
            // 
            // anignoProgressBarFlatCopyProgress
            // 
            this.anignoProgressBarFlatCopyProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.anignoProgressBarFlatCopyProgress.BackColorFirst = System.Drawing.SystemColors.ControlLightLight;
            this.anignoProgressBarFlatCopyProgress.BackColorSecond = System.Drawing.SystemColors.Control;
            this.anignoProgressBarFlatCopyProgress.ColorMainFirst = System.Drawing.SystemColors.Control;
            this.anignoProgressBarFlatCopyProgress.ColorMainGradientAngle = 0F;
            this.anignoProgressBarFlatCopyProgress.ColorMainSecond = System.Drawing.SystemColors.ControlDark;
            this.anignoProgressBarFlatCopyProgress.ColorMaximumRange = System.Drawing.Color.Red;
            this.anignoProgressBarFlatCopyProgress.ColorMinimumRange = System.Drawing.Color.Green;
            this.anignoProgressBarFlatCopyProgress.DrawBorder = false;
            this.anignoProgressBarFlatCopyProgress.GradientAngle = 0F;
            this.anignoProgressBarFlatCopyProgress.Location = new System.Drawing.Point(3, 29);
            this.anignoProgressBarFlatCopyProgress.Maximum = 100F;
            this.anignoProgressBarFlatCopyProgress.MaximumRange = 100F;
            this.anignoProgressBarFlatCopyProgress.Minimum = 0F;
            this.anignoProgressBarFlatCopyProgress.MinimumRange = 0F;
            this.anignoProgressBarFlatCopyProgress.Name = "anignoProgressBarFlatCopyProgress";
            this.anignoProgressBarFlatCopyProgress.ProgressBarOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.anignoProgressBarFlatCopyProgress.ProgressBarText = "0%";
            this.anignoProgressBarFlatCopyProgress.ProgressBarTextColor = System.Drawing.Color.White;
            this.anignoProgressBarFlatCopyProgress.Size = new System.Drawing.Size(404, 23);
            this.anignoProgressBarFlatCopyProgress.TabIndex = 0;
            this.anignoProgressBarFlatCopyProgress.Text = "0%";
            this.anignoProgressBarFlatCopyProgress.Value = 0F;
            // 
            // FileCopyNoneBlockingControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDestination);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.anignoProgressBarFlatCopyProgress);
            this.Name = "FileCopyNoneBlockingControl";
            this.Size = new System.Drawing.Size(462, 59);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AnignoraUI.ProgressBarsAndGauges.AnignoProgressBarFlat anignoProgressBarFlatCopyProgress;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAbort;
    }
}
