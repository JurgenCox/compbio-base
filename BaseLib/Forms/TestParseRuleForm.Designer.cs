namespace BaseLib.Forms
{
	partial class TestParseRulesForm
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.minEntryTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.maxEntryTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.testButton = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.mainTable = new BaseLib.Forms.Table.TableView();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.minEntryTextBox,
            this.toolStripLabel2,
            this.maxEntryTextBox,
            this.testButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.toolStrip1.Size = new System.Drawing.Size(1653, 47);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(152, 44);
			this.toolStripLabel1.Text = "Min. entry";
			// 
			// minEntryTextBox
			// 
			this.minEntryTextBox.Name = "minEntryTextBox";
			this.minEntryTextBox.Size = new System.Drawing.Size(260, 47);
			this.minEntryTextBox.Text = "1";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(157, 44);
			this.toolStripLabel2.Text = "Max. entry";
			// 
			// maxEntryTextBox
			// 
			this.maxEntryTextBox.Name = "maxEntryTextBox";
			this.maxEntryTextBox.Size = new System.Drawing.Size(260, 47);
			this.maxEntryTextBox.Text = "100";
			// 
			// testButton
			// 
			this.testButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.testButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.testButton.Name = "testButton";
			this.testButton.Size = new System.Drawing.Size(23, 44);
			this.testButton.Text = "toolStripButton1";
			this.testButton.ToolTipText = "Refresh";
			// 
			// toolStrip2
			// 
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(40, 40);
			this.toolStrip2.Location = new System.Drawing.Point(0, 47);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.toolStrip2.Size = new System.Drawing.Size(1653, 48);
			this.toolStrip2.TabIndex = 1;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// mainTable
			// 
			this.mainTable.ColumnHeaderHeight = 26;
			this.mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTable.HasSelectionAgent = false;
			this.mainTable.Location = new System.Drawing.Point(0, 95);
			this.mainTable.Margin = new System.Windows.Forms.Padding(0);
			this.mainTable.MultiSelect = true;
			this.mainTable.Name = "mainTable";
			this.mainTable.RowHeaderWidth = 70;
			this.mainTable.Size = new System.Drawing.Size(1653, 988);
			this.mainTable.Sortable = true;
			this.mainTable.TabIndex = 4;
			this.mainTable.TableModel = null;
			this.mainTable.VisibleX = 0;
			this.mainTable.VisibleY = 0;
			// 
			// TestParseRulesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1653, 1083);
			this.Controls.Add(this.mainTable);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "TestParseRulesForm";
			this.Text = "Testing parse rules";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private BaseLib.Forms.Table.TableView mainTable;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox minEntryTextBox;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripTextBox maxEntryTextBox;
		private System.Windows.Forms.ToolStripButton testButton;
	}
}