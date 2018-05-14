namespace BaseLib.Forms
{
    partial class RegexReplaceParamControl
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
            this.ReplaceTextBox = new System.Windows.Forms.MaskedTextBox();
            this.PatternTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CurrentListBox = new System.Windows.Forms.ListBox();
            this.PreviewListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ReplaceTextBox
            // 
            this.ReplaceTextBox.Location = new System.Drawing.Point(147, 28);
            this.ReplaceTextBox.Name = "ReplaceTextBox";
            this.ReplaceTextBox.Size = new System.Drawing.Size(100, 20);
            this.ReplaceTextBox.TabIndex = 0;
            // 
            // PatternTextBox
            // 
            this.PatternTextBox.Location = new System.Drawing.Point(19, 28);
            this.PatternTextBox.Name = "PatternTextBox";
            this.PatternTextBox.Size = new System.Drawing.Size(100, 20);
            this.PatternTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Preview";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pattern";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Replace";
            // 
            // CurrentListBox
            // 
            this.CurrentListBox.FormattingEnabled = true;
            this.CurrentListBox.Location = new System.Drawing.Point(19, 84);
            this.CurrentListBox.Name = "CurrentListBox";
            this.CurrentListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.CurrentListBox.Size = new System.Drawing.Size(100, 160);
            this.CurrentListBox.TabIndex = 8;
            // 
            // PreviewListBox
            // 
            this.PreviewListBox.FormattingEnabled = true;
            this.PreviewListBox.Location = new System.Drawing.Point(147, 84);
            this.PreviewListBox.Name = "PreviewListBox";
            this.PreviewListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.PreviewListBox.Size = new System.Drawing.Size(100, 160);
            this.PreviewListBox.TabIndex = 9;
            // 
            // RegexReplaceParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PreviewListBox);
            this.Controls.Add(this.CurrentListBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PatternTextBox);
            this.Controls.Add(this.ReplaceTextBox);
            this.Name = "RegexReplaceParamControl";
            this.Size = new System.Drawing.Size(274, 278);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox ReplaceTextBox;
        private System.Windows.Forms.MaskedTextBox PatternTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox CurrentListBox;
        private System.Windows.Forms.ListBox PreviewListBox;
    }
}
