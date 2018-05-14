namespace BaseLib.Forms
{
	partial class ListSelectorControl
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
			this.allListBox = new System.Windows.Forms.ListBox();
			this.selectedListBox = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.selectButton = new System.Windows.Forms.Button();
			this.deselectButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.topButton = new System.Windows.Forms.Button();
			this.upButton = new System.Windows.Forms.Button();
			this.downButton = new System.Windows.Forms.Button();
			this.bottomButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.defaultButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.Controls.Add(this.allListBox, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.selectedListBox, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 3, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(650, 444);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// allListBox
			// 
			this.allListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.allListBox.FormattingEnabled = true;
			this.allListBox.Location = new System.Drawing.Point(0, 0);
			this.allListBox.Margin = new System.Windows.Forms.Padding(0);
			this.allListBox.Name = "allListBox";
			this.allListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.allListBox.Size = new System.Drawing.Size(303, 444);
			this.allListBox.TabIndex = 0;
			// 
			// selectedListBox
			// 
			this.selectedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedListBox.FormattingEnabled = true;
			this.selectedListBox.Location = new System.Drawing.Point(325, 0);
			this.selectedListBox.Margin = new System.Windows.Forms.Padding(0);
			this.selectedListBox.Name = "selectedListBox";
			this.selectedListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.selectedListBox.Size = new System.Drawing.Size(303, 444);
			this.selectedListBox.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.selectButton, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.deselectButton, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(303, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(22, 444);
			this.tableLayoutPanel2.TabIndex = 2;
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
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(628, 0);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(22, 444);
			this.tableLayoutPanel3.TabIndex = 3;
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
			this.bottomButton.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.defaultButtonPanel, 0, 1);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(650, 444);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// defaultButtonPanel
			// 
			this.defaultButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.defaultButtonPanel.Location = new System.Drawing.Point(0, 444);
			this.defaultButtonPanel.Margin = new System.Windows.Forms.Padding(0);
			this.defaultButtonPanel.Name = "defaultButtonPanel";
			this.defaultButtonPanel.Size = new System.Drawing.Size(650, 1);
			this.defaultButtonPanel.TabIndex = 1;
			// 
			// ListSelectorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel4);
			this.Name = "ListSelectorControl";
			this.Size = new System.Drawing.Size(650, 444);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ListBox allListBox;
		private System.Windows.Forms.ListBox selectedListBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.Button deselectButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button topButton;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Button downButton;
		private System.Windows.Forms.Button bottomButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.FlowLayoutPanel defaultButtonPanel;
	}
}
