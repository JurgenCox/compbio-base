namespace BaseLib.Forms.Functions
{
    partial class BiexTransform
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
            this.maxvalue = new System.Windows.Forms.Label();
            this.minvalue = new System.Windows.Forms.Label();
            this.typefunction = new System.Windows.Forms.Label();
            this.TopScale = new System.Windows.Forms.TextBox();
            this.MinScale = new System.Windows.Forms.TextBox();
            this.AxisName = new System.Windows.Forms.TextBox();
            this.TrackBasis = new System.Windows.Forms.TrackBar();
            this.TrackNeg = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Wvalue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Nvalue = new System.Windows.Forms.Label();
            this.Applybutton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBasis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackNeg)).BeginInit();
            this.SuspendLayout();
            // 
            // maxvalue
            // 
            this.maxvalue.AutoSize = true;
            this.maxvalue.Location = new System.Drawing.Point(273, 80);
            this.maxvalue.Name = "maxvalue";
            this.maxvalue.Size = new System.Drawing.Size(30, 13);
            this.maxvalue.TabIndex = 0;
            this.maxvalue.Text = "Max.";
            // 
            // minvalue
            // 
            this.minvalue.AutoSize = true;
            this.minvalue.Location = new System.Drawing.Point(12, 80);
            this.minvalue.Name = "minvalue";
            this.minvalue.Size = new System.Drawing.Size(27, 13);
            this.minvalue.TabIndex = 1;
            this.minvalue.Text = "Min.";
            // 
            // typefunction
            // 
            this.typefunction.AutoSize = true;
            this.typefunction.Location = new System.Drawing.Point(12, 9);
            this.typefunction.Name = "typefunction";
            this.typefunction.Size = new System.Drawing.Size(117, 13);
            this.typefunction.TabIndex = 2;
            this.typefunction.Text = "Bi-exponential Function";
            this.typefunction.Click += new System.EventHandler(this.label3_Click);
            // 
            // TopScale
            // 
            this.TopScale.Location = new System.Drawing.Point(309, 73);
            this.TopScale.Name = "TopScale";
            this.TopScale.Size = new System.Drawing.Size(112, 20);
            this.TopScale.TabIndex = 3;
            this.TopScale.TextChanged += new System.EventHandler(this.TopScale_TextChanged);
            // 
            // MinScale
            // 
            this.MinScale.Location = new System.Drawing.Point(45, 73);
            this.MinScale.Name = "MinScale";
            this.MinScale.Size = new System.Drawing.Size(114, 20);
            this.MinScale.TabIndex = 4;
            this.MinScale.TextChanged += new System.EventHandler(this.MinScale_TextChanged);
            // 
            // AxisName
            // 
            this.AxisName.Location = new System.Drawing.Point(12, 34);
            this.AxisName.Name = "AxisName";
            this.AxisName.Size = new System.Drawing.Size(409, 20);
            this.AxisName.TabIndex = 5;
            this.AxisName.TextChanged += new System.EventHandler(this.AxisName_TextChanged);
            // 
            // TrackBasis
            // 
            this.TrackBasis.Location = new System.Drawing.Point(227, 116);
            this.TrackBasis.Name = "TrackBasis";
            this.TrackBasis.Size = new System.Drawing.Size(194, 45);
            this.TrackBasis.TabIndex = 6;
            // 
            // TrackNeg
            // 
            this.TrackNeg.Location = new System.Drawing.Point(12, 116);
            this.TrackNeg.Name = "TrackNeg";
            this.TrackNeg.Size = new System.Drawing.Size(179, 45);
            this.TrackNeg.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Width Basis";
            // 
            // Wvalue
            // 
            this.Wvalue.AutoSize = true;
            this.Wvalue.Location = new System.Drawing.Point(386, 181);
            this.Wvalue.Name = "Wvalue";
            this.Wvalue.Size = new System.Drawing.Size(35, 13);
            this.Wvalue.TabIndex = 9;
            this.Wvalue.Text = "label2";
            this.Wvalue.Click += new System.EventHandler(this.Wvalue_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Extra Neg. Decades";
            // 
            // Nvalue
            // 
            this.Nvalue.AutoSize = true;
            this.Nvalue.Location = new System.Drawing.Point(156, 181);
            this.Nvalue.Name = "Nvalue";
            this.Nvalue.Size = new System.Drawing.Size(35, 13);
            this.Nvalue.TabIndex = 11;
            this.Nvalue.Text = "label4";
            this.Nvalue.Click += new System.EventHandler(this.Nvalue_Click);
            // 
            // Applybutton
            // 
            this.Applybutton.Location = new System.Drawing.Point(265, 215);
            this.Applybutton.Name = "Applybutton";
            this.Applybutton.Size = new System.Drawing.Size(75, 23);
            this.Applybutton.TabIndex = 12;
            this.Applybutton.Text = "Apply";
            this.Applybutton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(346, 215);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 13;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // BiexTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 250);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.Applybutton);
            this.Controls.Add(this.Nvalue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Wvalue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TrackNeg);
            this.Controls.Add(this.TrackBasis);
            this.Controls.Add(this.AxisName);
            this.Controls.Add(this.MinScale);
            this.Controls.Add(this.TopScale);
            this.Controls.Add(this.typefunction);
            this.Controls.Add(this.minvalue);
            this.Controls.Add(this.maxvalue);
            this.Name = "BiexTransform";
            this.Text = "BiexTransform";
            ((System.ComponentModel.ISupportInitialize)(this.TrackBasis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackNeg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label maxvalue;
        private System.Windows.Forms.Label minvalue;
        private System.Windows.Forms.Label typefunction;
        private System.Windows.Forms.TextBox TopScale;
        private System.Windows.Forms.TextBox MinScale;
        private System.Windows.Forms.TrackBar TrackBasis;
        private System.Windows.Forms.TrackBar TrackNeg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Wvalue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Nvalue;
        private System.Windows.Forms.Button Applybutton;
        private System.Windows.Forms.Button CancelButton;
        public System.Windows.Forms.TextBox AxisName;
    }
}