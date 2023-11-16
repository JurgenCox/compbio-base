namespace BaseLib.Forms
{
	partial class PerseusLoadMatrixControl
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.selectButton = new System.Windows.Forms.Button();
			this.multiListSelectorControl1 = new BaseLib.Forms.MultiListSelectorControl();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.multiListSelectorControl1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1584, 1555);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
			this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.selectButton, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1584, 64);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Margin = new System.Windows.Forms.Padding(0);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(1371, 38);
			this.textBox1.TabIndex = 0;
			// 
			// selectButton
			// 
			this.selectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectButton.Location = new System.Drawing.Point(1371, 0);
			this.selectButton.Margin = new System.Windows.Forms.Padding(0);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(213, 64);
			this.selectButton.TabIndex = 1;
			this.selectButton.Text = "Select";
			this.selectButton.UseVisualStyleBackColor = true;
			// 
			// multiListSelectorControl1
			// 
			this.multiListSelectorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.multiListSelectorControl1.Location = new System.Drawing.Point(0, 64);
			this.multiListSelectorControl1.Margin = new System.Windows.Forms.Padding(0);
			this.multiListSelectorControl1.Name = "multiListSelectorControl1";
			this.multiListSelectorControl1.Size = new System.Drawing.Size(1584, 1439);
			this.multiListSelectorControl1.TabIndex = 1;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkBox1.Location = new System.Drawing.Point(0, 1503);
			this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(1584, 52);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "Shorten main column names";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// PerseusLoadMatrixControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "PerseusLoadMatrixControl";
			this.Size = new System.Drawing.Size(1584, 1555);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button selectButton;
		private BaseLib.Forms.MultiListSelectorControl multiListSelectorControl1;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}
