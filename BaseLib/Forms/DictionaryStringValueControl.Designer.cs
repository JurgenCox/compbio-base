namespace BaseLib.Forms
{
	partial class DictionaryStringValueControl
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
			this.tableView1 = new BaseLib.Forms.Table.TableView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableView1
			// 
			this.tableView1.ColumnHeaderHeight = 26;
			this.tableView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableView1.Location = new System.Drawing.Point(0, 51);
			this.tableView1.Margin = new System.Windows.Forms.Padding(0);
			this.tableView1.MultiSelect = true;
			this.tableView1.Name = "tableView1";
			this.tableView1.RowHeaderWidth = 70;
			this.tableView1.Size = new System.Drawing.Size(1053, 960);
			this.tableView1.Sortable = true;
			this.tableView1.TabIndex = 1;
			this.tableView1.TableModel = null;
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.toolStrip1.Size = new System.Drawing.Size(1053, 51);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::BaseLib.Properties.Resources.check;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(58, 44);
			this.toolStripButton1.Text = "Set group name";
			// 
			// DictionaryStringValueControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableView1);
			this.Controls.Add(this.toolStrip1);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "DictionaryStringValueControl";
			this.Size = new System.Drawing.Size(1053, 1011);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Table.TableView tableView1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
	}
}
