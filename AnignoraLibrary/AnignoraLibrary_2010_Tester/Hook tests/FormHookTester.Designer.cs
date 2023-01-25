namespace AnignoraCommonAndHelpers.HooksAndImmulate
{
    partial class FormHookTester
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxMouseLocation = new System.Windows.Forms.TextBox();
            this.buttonMoveMouseAndClick = new System.Windows.Forms.Button();
            this.buttonScrollDown = new System.Windows.Forms.Button();
            this.buttonScrollUp = new System.Windows.Forms.Button();
            this.listBoxScrollTest = new System.Windows.Forms.ListBox();
            this.buttonMouseClickTest = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.textBoxKeyTest2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTestKeyEmulate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxMouseLocation
            // 
            this.textBoxMouseLocation.Location = new System.Drawing.Point(176, 84);
            this.textBoxMouseLocation.Name = "textBoxMouseLocation";
            this.textBoxMouseLocation.Size = new System.Drawing.Size(138, 20);
            this.textBoxMouseLocation.TabIndex = 9;
            // 
            // buttonMoveMouseAndClick
            // 
            this.buttonMoveMouseAndClick.Location = new System.Drawing.Point(176, 26);
            this.buttonMoveMouseAndClick.Name = "buttonMoveMouseAndClick";
            this.buttonMoveMouseAndClick.Size = new System.Drawing.Size(69, 52);
            this.buttonMoveMouseAndClick.TabIndex = 8;
            this.buttonMoveMouseAndClick.Text = "Move mouse and click";
            this.buttonMoveMouseAndClick.UseVisualStyleBackColor = true;
            this.buttonMoveMouseAndClick.Click += new System.EventHandler(this.buttonMoveMouseAndClick_Click);
            // 
            // buttonScrollDown
            // 
            this.buttonScrollDown.Location = new System.Drawing.Point(72, 55);
            this.buttonScrollDown.Name = "buttonScrollDown";
            this.buttonScrollDown.Size = new System.Drawing.Size(75, 23);
            this.buttonScrollDown.TabIndex = 7;
            this.buttonScrollDown.Text = "scroll down";
            this.buttonScrollDown.UseVisualStyleBackColor = true;
            this.buttonScrollDown.Click += new System.EventHandler(this.buttonScrollDown_Click);
            // 
            // buttonScrollUp
            // 
            this.buttonScrollUp.Location = new System.Drawing.Point(72, 26);
            this.buttonScrollUp.Name = "buttonScrollUp";
            this.buttonScrollUp.Size = new System.Drawing.Size(75, 23);
            this.buttonScrollUp.TabIndex = 6;
            this.buttonScrollUp.Text = "scroll up";
            this.buttonScrollUp.UseVisualStyleBackColor = true;
            this.buttonScrollUp.Click += new System.EventHandler(this.buttonScrollUp_Click);
            // 
            // listBoxScrollTest
            // 
            this.listBoxScrollTest.FormattingEnabled = true;
            this.listBoxScrollTest.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0"});
            this.listBoxScrollTest.Location = new System.Drawing.Point(12, 26);
            this.listBoxScrollTest.Name = "listBoxScrollTest";
            this.listBoxScrollTest.Size = new System.Drawing.Size(54, 121);
            this.listBoxScrollTest.TabIndex = 5;
            // 
            // buttonMouseClickTest
            // 
            this.buttonMouseClickTest.Location = new System.Drawing.Point(251, 26);
            this.buttonMouseClickTest.Name = "buttonMouseClickTest";
            this.buttonMouseClickTest.Size = new System.Drawing.Size(107, 52);
            this.buttonMouseClickTest.TabIndex = 10;
            this.buttonMouseClickTest.Text = "Mouse Click Test";
            this.buttonMouseClickTest.UseVisualStyleBackColor = true;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 250;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // textBoxKeyTest2
            // 
            this.textBoxKeyTest2.Location = new System.Drawing.Point(9, 213);
            this.textBoxKeyTest2.Name = "textBoxKeyTest2";
            this.textBoxKeyTest2.Size = new System.Drawing.Size(138, 20);
            this.textBoxKeyTest2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Key hook";
            // 
            // textBoxTestKeyEmulate
            // 
            this.textBoxTestKeyEmulate.Location = new System.Drawing.Point(153, 213);
            this.textBoxTestKeyEmulate.Name = "textBoxTestKeyEmulate";
            this.textBoxTestKeyEmulate.Size = new System.Drawing.Size(138, 20);
            this.textBoxTestKeyEmulate.TabIndex = 15;
            this.textBoxTestKeyEmulate.Click += new System.EventHandler(this.textBoxTestKeyEmulate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Key emulate (mouse click to activate)";
            // 
            // FormHookTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 438);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTestKeyEmulate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxKeyTest2);
            this.Controls.Add(this.buttonMouseClickTest);
            this.Controls.Add(this.textBoxMouseLocation);
            this.Controls.Add(this.buttonMoveMouseAndClick);
            this.Controls.Add(this.buttonScrollDown);
            this.Controls.Add(this.buttonScrollUp);
            this.Controls.Add(this.listBoxScrollTest);
            this.Name = "FormHookTester";
            this.Text = "FormHookTester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMouseLocation;
        private System.Windows.Forms.Button buttonMoveMouseAndClick;
        private System.Windows.Forms.Button buttonScrollDown;
        private System.Windows.Forms.Button buttonScrollUp;
        private System.Windows.Forms.ListBox listBoxScrollTest;
        private System.Windows.Forms.Button buttonMouseClickTest;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.TextBox textBoxKeyTest2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTestKeyEmulate;
        private System.Windows.Forms.Label label2;
    }
}