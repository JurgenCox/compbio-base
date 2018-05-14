using BaseLibS.Graph.Axis;

namespace BaseLib.Forms{
	partial class ColorsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.colorScale = new ColorScale();
			this.SuspendLayout();
			// 
			// colorScale1
			// 
			this.colorScale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			                                                                 | System.Windows.Forms.AnchorStyles.Left)
			                                                                | System.Windows.Forms.AnchorStyles.Right)));
			this.colorScale.BackColor = System.Drawing.Color.White;
			this.colorScale.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorScale.Location = new System.Drawing.Point(5, 0);
			this.colorScale.Margin = new System.Windows.Forms.Padding(0);
			this.colorScale.Max = 10.000000000000002;
			this.colorScale.Min = 1;
			this.colorScale.Name = "ColorScale";
			this.colorScale.IsLogarithmic = true;
			this.colorScale.Positioning = AxisPositioning.Top;
			this.colorScale.Reverse = false;
			this.colorScale.Size = new System.Drawing.Size(753, 70);
			this.colorScale.TabIndex = 0;
			// 
			// ColorsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(762, 74);
			this.Controls.Add(this.colorScale);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(100000, 108);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(8, 108);
			this.Name = "ColorsForm";
			this.Text = "Colors";
			this.ResumeLayout(false);

		}

		#endregion

		private ColorScale colorScale;
	}
}