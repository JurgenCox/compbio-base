namespace BaseLib.Forms
{
	partial class SubSelectionControl
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.selectButton = new System.Windows.Forms.Button();
			this.deselectButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.topButton = new System.Windows.Forms.Button();
			this.upButton = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.bottomButton = new System.Windows.Forms.Button();
			this.parameterButton = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.titleLabel = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.listBox1, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 20);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(313, 268);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.selectButton, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.deselectButton, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(22, 268);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// selectButton
			// 
			this.selectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectButton.Location = new System.Drawing.Point(0, 0);
			this.selectButton.Margin = new System.Windows.Forms.Padding(0);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(22, 22);
			this.selectButton.TabIndex = 0;
			this.selectButton.Text = ">";
			this.toolTip1.SetToolTip(this.selectButton, "Select");
			this.selectButton.UseVisualStyleBackColor = true;
			// 
			// deselectButton
			// 
			this.deselectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.deselectButton.Location = new System.Drawing.Point(0, 22);
			this.deselectButton.Margin = new System.Windows.Forms.Padding(0);
			this.deselectButton.Name = "deselectButton";
			this.deselectButton.Size = new System.Drawing.Size(22, 22);
			this.deselectButton.TabIndex = 1;
			this.deselectButton.Text = "<";
			this.toolTip1.SetToolTip(this.deselectButton, "Deselect");
			this.deselectButton.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.topButton, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.upButton, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.downButton, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.bottomButton, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.parameterButton, 0, 4);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(291, 0);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 6;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(22, 268);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// topButton
			// 
			this.topButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.topButton.Location = new System.Drawing.Point(0, 0);
			this.topButton.Margin = new System.Windows.Forms.Padding(0);
			this.topButton.Name = "topButton";
			this.topButton.Size = new System.Drawing.Size(22, 22);
			this.topButton.TabIndex = 0;
			this.topButton.Text = "↑↑";
			this.toolTip1.SetToolTip(this.topButton, "Top");
			this.topButton.UseVisualStyleBackColor = true;
			// 
			// upButton
			// 
			this.upButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.upButton.Location = new System.Drawing.Point(0, 22);
			this.upButton.Margin = new System.Windows.Forms.Padding(0);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(22, 22);
			this.upButton.TabIndex = 1;
			this.upButton.Text = "↑";
			this.toolTip1.SetToolTip(this.upButton, "Up");
			this.upButton.UseVisualStyleBackColor = true;
			// 
			// downButton
			// 
			this.downButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.downButton.Location = new System.Drawing.Point(0, 44);
			this.downButton.Margin = new System.Windows.Forms.Padding(0);
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size(22, 22);
			this.downButton.TabIndex = 2;
			this.downButton.Text = "↓";
			this.toolTip1.SetToolTip(this.downButton, "Down");
			this.downButton.UseVisualStyleBackColor = true;
			// 
			// bottomButton
			// 
			this.bottomButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bottomButton.Location = new System.Drawing.Point(0, 66);
			this.bottomButton.Margin = new System.Windows.Forms.Padding(0);
			this.bottomButton.Name = "bottomButton";
			this.bottomButton.Size = new System.Drawing.Size(22, 22);
			this.bottomButton.TabIndex = 3;
			this.bottomButton.Text = "↓↓";
			this.toolTip1.SetToolTip(this.bottomButton, "Bottom");
			this.bottomButton.UseVisualStyleBackColor = true;
			// 
			// parameterButton
			// 
			this.parameterButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parameterButton.Location = new System.Drawing.Point(0, 88);
			this.parameterButton.Margin = new System.Windows.Forms.Padding(0);
			this.parameterButton.Name = "parameterButton";
			this.parameterButton.Size = new System.Drawing.Size(22, 22);
			this.parameterButton.TabIndex = 4;
			this.parameterButton.Text = "F";
			this.toolTip1.SetToolTip(this.parameterButton, "Filter");
			this.parameterButton.UseVisualStyleBackColor = true;
			this.parameterButton.Visible = false;
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(22, 0);
			this.listBox1.Margin = new System.Windows.Forms.Padding(0);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBox1.Size = new System.Drawing.Size(269, 268);
			this.listBox1.TabIndex = 2;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.titleLabel, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(313, 288);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.titleLabel.Location = new System.Drawing.Point(3, 0);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(307, 20);
			this.titleLabel.TabIndex = 1;
			this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SubSelectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel4);
			this.Name = "SubSelectionControl";
			this.Size = new System.Drawing.Size(313, 288);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.Button deselectButton;
		private System.Windows.Forms.Button topButton;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Button downButton;
		private System.Windows.Forms.Button bottomButton;
		internal System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Button parameterButton;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}
