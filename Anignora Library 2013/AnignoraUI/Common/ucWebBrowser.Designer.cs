namespace AnignoraUI.Common
{
    partial class ucWebBrowser
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
            this.webBrowserMain = new System.Windows.Forms.WebBrowser();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonHome = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonFwd = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonBack = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.buttonGo = new AnignoraUI.Buttons.ButtonGradientGlow();
            this.SuspendLayout();
            // 
            // webBrowserMain
            // 
            this.webBrowserMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowserMain.Location = new System.Drawing.Point(3, 26);
            this.webBrowserMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserMain.Name = "webBrowserMain";
            this.webBrowserMain.Size = new System.Drawing.Size(588, 325);
            this.webBrowserMain.TabIndex = 0;
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBoxUrl.Location = new System.Drawing.Point(3, 3);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(448, 20);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxUrl_KeyUp);
            // 
            // buttonHome
            // 
            this.buttonHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHome.BackColor = System.Drawing.Color.Black;
            this.buttonHome.CornerSize = 3;
            this.buttonHome.ForeColor = System.Drawing.Color.White;
            this.buttonHome.GlowColor = System.Drawing.Color.Blue;
            this.buttonHome.GlowIntensity = ((uint)(100u));
            this.buttonHome.GradientAngle = 270;
            this.buttonHome.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonHome.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonHome.Location = new System.Drawing.Point(527, 3);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(29, 20);
            this.buttonHome.TabIndex = 4;
            this.buttonHome.Text = "H";
            this.buttonHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // buttonFwd
            // 
            this.buttonFwd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFwd.BackColor = System.Drawing.Color.Black;
            this.buttonFwd.CornerSize = 3;
            this.buttonFwd.ForeColor = System.Drawing.Color.White;
            this.buttonFwd.GlowColor = System.Drawing.Color.Blue;
            this.buttonFwd.GlowIntensity = ((uint)(100u));
            this.buttonFwd.GradientAngle = 270;
            this.buttonFwd.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonFwd.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonFwd.Location = new System.Drawing.Point(562, 3);
            this.buttonFwd.Name = "buttonFwd";
            this.buttonFwd.Size = new System.Drawing.Size(29, 20);
            this.buttonFwd.TabIndex = 3;
            this.buttonFwd.Text = ">>";
            this.buttonFwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonFwd.Click += new System.EventHandler(this.buttonFwd_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBack.BackColor = System.Drawing.Color.Black;
            this.buttonBack.CornerSize = 3;
            this.buttonBack.ForeColor = System.Drawing.Color.White;
            this.buttonBack.GlowColor = System.Drawing.Color.Blue;
            this.buttonBack.GlowIntensity = ((uint)(100u));
            this.buttonBack.GradientAngle = 270;
            this.buttonBack.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonBack.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonBack.Location = new System.Drawing.Point(492, 3);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(29, 20);
            this.buttonBack.TabIndex = 2;
            this.buttonBack.Text = "<<";
            this.buttonBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.BackColor = System.Drawing.Color.Black;
            this.buttonGo.CornerSize = 3;
            this.buttonGo.ForeColor = System.Drawing.Color.White;
            this.buttonGo.GlowColor = System.Drawing.Color.Blue;
            this.buttonGo.GlowIntensity = ((uint)(100u));
            this.buttonGo.GradientAngle = 270;
            this.buttonGo.GradientColorFirst = System.Drawing.Color.Black;
            this.buttonGo.GradientColorSecond = System.Drawing.Color.Gray;
            this.buttonGo.Location = new System.Drawing.Point(457, 2);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(29, 20);
            this.buttonGo.TabIndex = 5;
            this.buttonGo.Text = ">";
            this.buttonGo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // ucWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.buttonHome);
            this.Controls.Add(this.buttonFwd);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.webBrowserMain);
            this.Name = "ucWebBrowser";
            this.Size = new System.Drawing.Size(594, 354);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserMain;
        private System.Windows.Forms.TextBox textBoxUrl;
        private Buttons.ButtonGradientGlow buttonBack;
        private Buttons.ButtonGradientGlow buttonFwd;
        private Buttons.ButtonGradientGlow buttonHome;
        private Buttons.ButtonGradientGlow buttonGo;
    }
}
