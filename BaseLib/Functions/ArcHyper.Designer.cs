namespace BaseLib.Functions
{
    partial class ArcHyper
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
            this.minValueTextBox = new System.Windows.Forms.TextBox();
            this.maxValueTextBox = new System.Windows.Forms.TextBox();
            this.NegValues = new BaseLib.Forms.SliderControl();
            this.trackbasis = new BaseLib.Forms.SliderControl();
            this.NegText = new System.Windows.Forms.TextBox();
            this.basisText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titleTextBox = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // minValueTextBox
            // 
            this.minValueTextBox.Location = new System.Drawing.Point(68, 62);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(222, 20);
            this.minValueTextBox.TabIndex = 0;
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(419, 62);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(222, 20);
            this.maxValueTextBox.TabIndex = 1;
            // 
            // NegValues
            // 
            this.NegValues.Location = new System.Drawing.Point(33, 102);
            this.NegValues.Maximum = 0D;
            this.NegValues.Minimum = 0D;
            this.NegValues.Name = "NegValues";
            this.NegValues.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.NegValues.Size = new System.Drawing.Size(257, 58);
            this.NegValues.TabIndex = 2;
            this.NegValues.TickFrequency = 0;
            this.NegValues.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.NegValues.Value = 0D;
            // 
            // trackbasis
            // 
            this.trackbasis.Location = new System.Drawing.Point(386, 102);
            this.trackbasis.Maximum = 0D;
            this.trackbasis.Minimum = 0D;
            this.trackbasis.Name = "trackbasis";
            this.trackbasis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.trackbasis.Size = new System.Drawing.Size(255, 58);
            this.trackbasis.TabIndex = 3;
            this.trackbasis.TickFrequency = 0;
            this.trackbasis.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.trackbasis.Value = 0D;
            // 
            // NegText
            // 
            this.NegText.Location = new System.Drawing.Point(214, 177);
            this.NegText.Name = "NegText";
            this.NegText.Size = new System.Drawing.Size(76, 20);
            this.NegText.TabIndex = 4;
            // 
            // basisText
            // 
            this.basisText.Location = new System.Drawing.Point(565, 177);
            this.basisText.Name = "basisText";
            this.basisText.Size = new System.Drawing.Size(76, 20);
            this.basisText.TabIndex = 5;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(546, 234);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(95, 27);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(445, 234);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 27);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // titleTextBox
            // 
            this.titleTextBox.AutoSize = true;
            this.titleTextBox.Location = new System.Drawing.Point(30, 20);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(62, 13);
            this.titleTextBox.TabIndex = 8;
            this.titleTextBox.Text = "titleTextBox";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Min:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(383, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Max:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Extra Neg. Values";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(383, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Width Basis";
            // 
            // ArcHyper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 282);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.basisText);
            this.Controls.Add(this.NegText);
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.NegValues);
            this.Controls.Add(this.maxValueTextBox);
            this.Controls.Add(this.minValueTextBox);
            this.Name = "ArcHyper";
            this.Text = "Transform";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox minValueTextBox;
        private System.Windows.Forms.TextBox maxValueTextBox;
        private Forms.SliderControl NegValues;
        private Forms.SliderControl trackbasis;
        private System.Windows.Forms.TextBox NegText;
        private System.Windows.Forms.TextBox basisText;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label titleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}