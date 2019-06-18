namespace BaseLib.Functions
{
    partial class HypeTranform
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.titleTextBox = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackbasis = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.NegText = new System.Windows.Forms.TextBox();
            this.basisText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NegValues = new BaseLib.Forms.SliderControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackbasis)).BeginInit();
            this.SuspendLayout();
            // 
            // minValueTextBox
            // 
            this.minValueTextBox.Location = new System.Drawing.Point(45, 41);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(185, 20);
            this.minValueTextBox.TabIndex = 0;
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(320, 41);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(185, 20);
            this.maxValueTextBox.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(347, 213);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(428, 213);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(76, 25);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "Apply";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // titleTextBox
            // 
            this.titleTextBox.AutoSize = true;
            this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.Location = new System.Drawing.Point(12, 16);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(41, 15);
            this.titleTextBox.TabIndex = 4;
            this.titleTextBox.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Min:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Max:";
            // 
            // trackbasis
            // 
            this.trackbasis.Location = new System.Drawing.Point(273, 80);
            this.trackbasis.Name = "trackbasis";
            this.trackbasis.Size = new System.Drawing.Size(232, 45);
            this.trackbasis.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Extra Neg. Decades";
            // 
            // NegText
            // 
            this.NegText.Location = new System.Drawing.Point(177, 147);
            this.NegText.Name = "NegText";
            this.NegText.Size = new System.Drawing.Size(53, 20);
            this.NegText.TabIndex = 10;
            // 
            // basisText
            // 
            this.basisText.Location = new System.Drawing.Point(452, 147);
            this.basisText.Name = "basisText";
            this.basisText.Size = new System.Drawing.Size(53, 20);
            this.basisText.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(270, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Width Basis";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(280, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "label6";
            // 
            // NegValues
            // 
            this.NegValues.Location = new System.Drawing.Point(15, 88);
            this.NegValues.Maximum = 0D;
            this.NegValues.Minimum = 0D;
            this.NegValues.Name = "NegValues";
            this.NegValues.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.NegValues.Size = new System.Drawing.Size(215, 29);
            this.NegValues.TabIndex = 16;
            this.NegValues.TickFrequency = 0;
            this.NegValues.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.NegValues.Value = 0D;
            // 
            // HypeTranform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 258);
            this.Controls.Add(this.NegValues);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.basisText);
            this.Controls.Add(this.NegText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.maxValueTextBox);
            this.Controls.Add(this.minValueTextBox);
            this.Name = "HypeTranform";
            this.Text = "Tranform";
            ((System.ComponentModel.ISupportInitialize)(this.trackbasis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        public System.Windows.Forms.TextBox minValueTextBox;
        public System.Windows.Forms.TextBox maxValueTextBox;
        private System.Windows.Forms.Label titleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackbasis;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NegText;
        private System.Windows.Forms.TextBox basisText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private Forms.SliderControl NegValues;
    }
}