namespace BaseLib.Functions
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
            this.label1 = new System.Windows.Forms.Label();
            this.minvalue = new System.Windows.Forms.TextBox();
            this.maxvalue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackbasis = new System.Windows.Forms.TrackBar();
            this.NegValues = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.basisvalue = new System.Windows.Forms.Label();
            this.Basisiname = new System.Windows.Forms.Label();
            this.n_value = new System.Windows.Forms.Label();
            this.negTitle = new System.Windows.Forms.Label();
            this.cancel_button = new System.Windows.Forms.Button();
            this.apply_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackbasis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NegValues)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Biexponential Function";
            // 
            // minvalue
            // 
            this.minvalue.Location = new System.Drawing.Point(45, 36);
            this.minvalue.Name = "minvalue";
            this.minvalue.Size = new System.Drawing.Size(143, 20);
            this.minvalue.TabIndex = 2;
            this.minvalue.TabStop = false;
            // 
            // maxvalue
            // 
            this.maxvalue.Location = new System.Drawing.Point(322, 36);
            this.maxvalue.Name = "maxvalue";
            this.maxvalue.Size = new System.Drawing.Size(143, 20);
            this.maxvalue.TabIndex = 3;
            this.maxvalue.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Max.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Min.";
            // 
            // trackbasis
            // 
            this.trackbasis.Location = new System.Drawing.Point(274, 73);
            this.trackbasis.Name = "trackbasis";
            this.trackbasis.Size = new System.Drawing.Size(191, 45);
            this.trackbasis.TabIndex = 6;
            this.trackbasis.Scroll += new System.EventHandler(this.trackbasis_Scroll);
            // 
            // NegValues
            // 
            this.NegValues.Location = new System.Drawing.Point(12, 73);
            this.NegValues.Name = "NegValues";
            this.NegValues.Size = new System.Drawing.Size(193, 45);
            this.NegValues.TabIndex = 7;
            this.NegValues.Scroll += new System.EventHandler(this.NegValues_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(281, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "label5";
            // 
            // basisvalue
            // 
            this.basisvalue.AutoSize = true;
            this.basisvalue.Location = new System.Drawing.Point(441, 138);
            this.basisvalue.Name = "basisvalue";
            this.basisvalue.Size = new System.Drawing.Size(13, 13);
            this.basisvalue.TabIndex = 10;
            this.basisvalue.Text = "0";
            this.basisvalue.Click += new System.EventHandler(this.basisvalue_Click);
            // 
            // Basisiname
            // 
            this.Basisiname.AutoSize = true;
            this.Basisiname.Location = new System.Drawing.Point(281, 138);
            this.Basisiname.Name = "Basisiname";
            this.Basisiname.Size = new System.Drawing.Size(63, 13);
            this.Basisiname.TabIndex = 11;
            this.Basisiname.Text = "Width Basis";
            // 
            // n_value
            // 
            this.n_value.AutoSize = true;
            this.n_value.Location = new System.Drawing.Point(175, 138);
            this.n_value.Name = "n_value";
            this.n_value.Size = new System.Drawing.Size(13, 13);
            this.n_value.TabIndex = 12;
            this.n_value.Text = "0";
            this.n_value.Click += new System.EventHandler(this.n_value_Click);
            // 
            // negTitle
            // 
            this.negTitle.AutoSize = true;
            this.negTitle.Location = new System.Drawing.Point(16, 138);
            this.negTitle.Name = "negTitle";
            this.negTitle.Size = new System.Drawing.Size(87, 13);
            this.negTitle.TabIndex = 13;
            this.negTitle.Text = "Extra Neg. Value";
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(300, 174);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 14;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // apply_button
            // 
            this.apply_button.Location = new System.Drawing.Point(390, 174);
            this.apply_button.Name = "apply_button";
            this.apply_button.Size = new System.Drawing.Size(75, 23);
            this.apply_button.TabIndex = 15;
            this.apply_button.Text = "Apply";
            this.apply_button.UseVisualStyleBackColor = true;
            // 
            // BiexTransform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 207);
            this.Controls.Add(this.apply_button);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.negTitle);
            this.Controls.Add(this.n_value);
            this.Controls.Add(this.Basisiname);
            this.Controls.Add(this.basisvalue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NegValues);
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.maxvalue);
            this.Controls.Add(this.minvalue);
            this.Controls.Add(this.label1);
            this.Name = "BiexTransform";
            this.Text = "BiexTransform";
            ((System.ComponentModel.ISupportInitialize)(this.trackbasis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NegValues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label Basisiname;
        private System.Windows.Forms.Label negTitle;
        public System.Windows.Forms.TextBox minvalue;
        public System.Windows.Forms.TextBox maxvalue;
        public System.Windows.Forms.TrackBar trackbasis;
        public System.Windows.Forms.TrackBar NegValues;
        public System.Windows.Forms.Label basisvalue;
        public System.Windows.Forms.Label n_value;
        public System.Windows.Forms.Button cancel_button;
        public System.Windows.Forms.Button apply_button;
    }
}