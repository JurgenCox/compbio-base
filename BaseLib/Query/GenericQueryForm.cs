using System;
using System.Windows.Forms;
namespace BaseLib.Query{
	public abstract partial class GenericQueryForm : Form{
		protected GenericQueryForm(){
			InitializeComponent2();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
		}



		private void CancelButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.OK;
			Close();
		}
		protected void InitializeComponent2() {
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel2 = new TableLayoutPanel();
			cancelButton = new Button();
			okButton = new Button();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 2;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
			tableLayoutPanel1.Size = new System.Drawing.Size(285, 54);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 3;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
			tableLayoutPanel2.Controls.Add(cancelButton, 0, 0);
			tableLayoutPanel2.Controls.Add(okButton, 2, 0);
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new System.Drawing.Size(279, 28);
			tableLayoutPanel2.TabIndex = 0;
			// 
			// cancelButton
			// 
			cancelButton.DialogResult = DialogResult.Cancel;
			cancelButton.Dock = DockStyle.Fill;
			cancelButton.Location = new System.Drawing.Point(0, 0);
			cancelButton.Margin = new Padding(0);
			cancelButton.Name = "cancelButton";
			cancelButton.Size = new System.Drawing.Size(93, 28);
			cancelButton.TabIndex = 0;
			cancelButton.Text = "Cancel";
			cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			okButton.Dock = DockStyle.Fill;
			okButton.Location = new System.Drawing.Point(186, 0);
			okButton.Margin = new Padding(0);
			okButton.Name = "okButton";
			okButton.Size = new System.Drawing.Size(93, 28);
			okButton.TabIndex = 1;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			// 
			// DoubleQueryForm
			// 
			AcceptButton = okButton;
			AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = cancelButton;
			ClientSize = new System.Drawing.Size(289, 58);
			Controls.Add(tableLayoutPanel1);
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			MaximizeBox = false;
			//MaximumSize = new System.Drawing.Size(305, 97);
			MinimizeBox = false;
			//MinimumSize = new System.Drawing.Size(305, 97);
			Name = "DoubleQueryForm";
			ShowIcon = false;
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			tableLayoutPanel2.ResumeLayout(false);
			ResumeLayout(false);

		}
		internal TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private Button cancelButton;
		private Button okButton;

	}
}