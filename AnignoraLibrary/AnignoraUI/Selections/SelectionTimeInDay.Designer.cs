namespace AnignoraUI.Selections
{
    partial class SelectionTimeInDay
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
            this.checkBoxDay = new System.Windows.Forms.CheckBox();
            this.checkedListBoxIntervals = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkBoxDay
            // 
            this.checkBoxDay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDay.Location = new System.Drawing.Point(3, 3);
            this.checkBoxDay.Name = "checkBoxDay";
            this.checkBoxDay.Size = new System.Drawing.Size(116, 24);
            this.checkBoxDay.TabIndex = 0;
            this.checkBoxDay.Text = "Day";
            this.checkBoxDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDay.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxIntervals
            // 
            this.checkedListBoxIntervals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxIntervals.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBoxIntervals.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxIntervals.FormattingEnabled = true;
            this.checkedListBoxIntervals.IntegralHeight = false;
            this.checkedListBoxIntervals.Location = new System.Drawing.Point(3, 33);
            this.checkedListBoxIntervals.Name = "checkedListBoxIntervals";
            this.checkedListBoxIntervals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkedListBoxIntervals.Size = new System.Drawing.Size(116, 268);
            this.checkedListBoxIntervals.TabIndex = 1;
            // 
            // SelectionTimeInDay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBoxIntervals);
            this.Controls.Add(this.checkBoxDay);
            this.Name = "SelectionTimeInDay";
            this.Size = new System.Drawing.Size(122, 301);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxDay;
        private System.Windows.Forms.CheckedListBox checkedListBoxIntervals;
    }
}
