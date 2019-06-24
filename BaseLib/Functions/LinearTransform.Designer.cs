namespace BaseLib.Functions
{
    partial class LinearTransform { 
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.titleTextBox = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.minusMinValue = new System.Windows.Forms.Button();
            this.plusMinValue = new System.Windows.Forms.Button();
            this.plusMaxValue = new System.Windows.Forms.Button();
            this.minusMaxValue = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // minValueTextBox
            // 
            this.minValueTextBox.Location = new System.Drawing.Point(57, 217);
            this.minValueTextBox.Name = "minValueTextBox";
            this.minValueTextBox.Size = new System.Drawing.Size(168, 20);
            this.minValueTextBox.TabIndex = 0;
            // 
            // maxValueTextBox
            // 
            this.maxValueTextBox.Location = new System.Drawing.Point(357, 217);
            this.maxValueTextBox.Name = "maxValueTextBox";
            this.maxValueTextBox.Size = new System.Drawing.Size(168, 20);
            this.maxValueTextBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(430, 267);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(95, 27);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(324, 267);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 27);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // titleTextBox
            // 
            this.titleTextBox.AutoSize = true;
            this.titleTextBox.Location = new System.Drawing.Point(64, 21);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(62, 13);
            this.titleTextBox.TabIndex = 8;
            this.titleTextBox.Text = "titleTextBox";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Min:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Max:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Scale:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(15, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(510, 141);
            this.panel1.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(174, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Histogram available soon";
            // 
            // minusMinValue
            // 
            this.minusMinValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusMinValue.Location = new System.Drawing.Point(15, 188);
            this.minusMinValue.Name = "minusMinValue";
            this.minusMinValue.Size = new System.Drawing.Size(23, 23);
            this.minusMinValue.TabIndex = 17;
            this.minusMinValue.Text = "-";
            this.minusMinValue.UseVisualStyleBackColor = true;
            // 
            // plusMinValue
            // 
            this.plusMinValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusMinValue.Location = new System.Drawing.Point(44, 188);
            this.plusMinValue.Name = "plusMinValue";
            this.plusMinValue.Size = new System.Drawing.Size(23, 23);
            this.plusMinValue.TabIndex = 18;
            this.plusMinValue.Text = "+";
            this.plusMinValue.UseVisualStyleBackColor = true;
            // 
            // plusMaxValue
            // 
            this.plusMaxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusMaxValue.Location = new System.Drawing.Point(502, 188);
            this.plusMaxValue.Name = "plusMaxValue";
            this.plusMaxValue.Size = new System.Drawing.Size(23, 23);
            this.plusMaxValue.TabIndex = 19;
            this.plusMaxValue.Text = "+";
            this.plusMaxValue.UseVisualStyleBackColor = true;
            // 
            // minusMaxValue
            // 
            this.minusMaxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusMaxValue.Location = new System.Drawing.Point(473, 188);
            this.minusMaxValue.Name = "minusMaxValue";
            this.minusMaxValue.Size = new System.Drawing.Size(23, 23);
            this.minusMaxValue.TabIndex = 20;
            this.minusMaxValue.Text = "-";
            this.minusMaxValue.UseVisualStyleBackColor = true;
            // 
            // LinearTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 316);
            this.Controls.Add(this.minusMaxValue);
            this.Controls.Add(this.plusMaxValue);
            this.Controls.Add(this.plusMinValue);
            this.Controls.Add(this.minusMinValue);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.maxValueTextBox);
            this.Controls.Add(this.minValueTextBox);
            this.Name = "LinearTransform";
            this.Text = "Linear Transform";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox minValueTextBox;
        private System.Windows.Forms.TextBox maxValueTextBox;
        private System.Windows.Forms.Label titleTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button okButton;
        public System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button minusMinValue;
        private System.Windows.Forms.Button plusMinValue;
        private System.Windows.Forms.Button plusMaxValue;
        private System.Windows.Forms.Button minusMaxValue;
    }
}