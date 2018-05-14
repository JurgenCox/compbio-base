namespace BaseLib.Forms {
	partial class FastaFilesParamControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableView1 = new BaseLib.Forms.Table.TableView();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableView1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(2135, 777);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableView1
			// 
			this.tableView1.ColumnHeaderHeight = 26;
			this.tableView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableView1.HasSelectionAgent = false;
			this.tableView1.Location = new System.Drawing.Point(0, 50);
			this.tableView1.Margin = new System.Windows.Forms.Padding(0);
			this.tableView1.MultiSelect = true;
			this.tableView1.Name = "tableView1";
			this.tableView1.RowHeaderWidth = 70;
			this.tableView1.Size = new System.Drawing.Size(2135, 727);
			this.tableView1.Sortable = true;
			this.tableView1.TabIndex = 0;
			this.tableView1.TableModel = null;
			this.tableView1.VisibleX = 0;
			this.tableView1.VisibleY = 0;
			// 
			// FastaFilesParamControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "FastaFilesParamControl";
			this.Size = new System.Drawing.Size(2135, 777);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Table.TableView tableView1;
	}
}
