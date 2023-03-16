using System.Windows.Forms;
namespace BaseLib.Forms {
	partial class IsobaricLabelsComplexEditForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IsobaricLabelsComplexEditForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.helpLabel13 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel11 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel9 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel7 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel5 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel3 = new BaseLib.Forms.Table.HelpLabel();
			this.helpLabel1 = new BaseLib.Forms.Table.HelpLabel();
			this.tmtLikeCheckBox = new System.Windows.Forms.CheckBox();
			this.internalLabelComboBox = new System.Windows.Forms.ComboBox();
			this.terminalLabelComboBox = new System.Windows.Forms.ComboBox();
			this.correctionFactorControlM2 = new System.Windows.Forms.TextBox();
			this.correctionFactorControlM1 = new System.Windows.Forms.TextBox();
			this.correctionFactorControlP1 = new System.Windows.Forms.TextBox();
			this.correctionFactorControlP2 = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1307, 525);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 213F));
			this.tableLayoutPanel2.Controls.Add(this.cancelButton, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.okButton, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 465);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1307, 60);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// cancelButton
			// 
			this.cancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cancelButton.Location = new System.Drawing.Point(0, 0);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(213, 60);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(1094, 0);
			this.okButton.Margin = new System.Windows.Forms.Padding(0);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(200, 55);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 379F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.helpLabel13, 0, 6);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel11, 0, 5);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel9, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel7, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel5, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel3, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.helpLabel1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tmtLikeCheckBox, 1, 6);
			this.tableLayoutPanel3.Controls.Add(this.internalLabelComboBox, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.terminalLabelComboBox, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.correctionFactorControlM2, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.correctionFactorControlM1, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.correctionFactorControlP1, 1, 4);
			this.tableLayoutPanel3.Controls.Add(this.correctionFactorControlP2, 1, 5);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 7);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 8;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(1291, 451);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// helpLabel13
			// 
			this.helpLabel13.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel13.HelpText = "Text";
			this.helpLabel13.HelpTitle = "Title";
			this.helpLabel13.Location = new System.Drawing.Point(8, 367);
			this.helpLabel13.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel13.Name = "helpLabel13";
			this.helpLabel13.Size = new System.Drawing.Size(363, 46);
			this.helpLabel13.TabIndex = 12;
			this.helpLabel13.Text = "TMT-like";
			// 
			// helpLabel11
			// 
			this.helpLabel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel11.HelpText = "Text";
			this.helpLabel11.HelpTitle = "Title";
			this.helpLabel11.Location = new System.Drawing.Point(8, 307);
			this.helpLabel11.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel11.Name = "helpLabel11";
			this.helpLabel11.Size = new System.Drawing.Size(363, 46);
			this.helpLabel11.TabIndex = 10;
			this.helpLabel11.Text = "Correction factor +2 [%]";
			// 
			// helpLabel9
			// 
			this.helpLabel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel9.HelpText = "Text";
			this.helpLabel9.HelpTitle = "Title";
			this.helpLabel9.Location = new System.Drawing.Point(8, 247);
			this.helpLabel9.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel9.Name = "helpLabel9";
			this.helpLabel9.Size = new System.Drawing.Size(363, 46);
			this.helpLabel9.TabIndex = 8;
			this.helpLabel9.Text = "Correction factor +1 [%]";
			// 
			// helpLabel7
			// 
			this.helpLabel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel7.HelpText = "Text";
			this.helpLabel7.HelpTitle = "Title";
			this.helpLabel7.Location = new System.Drawing.Point(8, 187);
			this.helpLabel7.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel7.Name = "helpLabel7";
			this.helpLabel7.Size = new System.Drawing.Size(363, 46);
			this.helpLabel7.TabIndex = 6;
			this.helpLabel7.Text = "Correction factor -1 [%]";
			// 
			// helpLabel5
			// 
			this.helpLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel5.HelpText = "Text";
			this.helpLabel5.HelpTitle = "Title";
			this.helpLabel5.Location = new System.Drawing.Point(8, 127);
			this.helpLabel5.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel5.Name = "helpLabel5";
			this.helpLabel5.Size = new System.Drawing.Size(363, 46);
			this.helpLabel5.TabIndex = 4;
			this.helpLabel5.Text = "Correction factor -2 [%]";
			// 
			// helpLabel3
			// 
			this.helpLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel3.HelpText = "Text";
			this.helpLabel3.HelpTitle = "Title";
			this.helpLabel3.Location = new System.Drawing.Point(8, 67);
			this.helpLabel3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel3.Name = "helpLabel3";
			this.helpLabel3.Size = new System.Drawing.Size(363, 46);
			this.helpLabel3.TabIndex = 2;
			this.helpLabel3.Text = "Terminal label";
			// 
			// helpLabel1
			// 
			this.helpLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.helpLabel1.HelpText = "Text";
			this.helpLabel1.HelpTitle = "Title";
			this.helpLabel1.Location = new System.Drawing.Point(8, 7);
			this.helpLabel1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.helpLabel1.Name = "helpLabel1";
			this.helpLabel1.Size = new System.Drawing.Size(363, 46);
			this.helpLabel1.TabIndex = 0;
			this.helpLabel1.Text = "Internal label";
			// 
			// tmtLikeCheckBox
			// 
			this.tmtLikeCheckBox.AutoSize = true;
			this.tmtLikeCheckBox.Location = new System.Drawing.Point(387, 367);
			this.tmtLikeCheckBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.tmtLikeCheckBox.Name = "tmtLikeCheckBox";
			this.tmtLikeCheckBox.Size = new System.Drawing.Size(34, 33);
			this.tmtLikeCheckBox.TabIndex = 13;
			this.tmtLikeCheckBox.UseVisualStyleBackColor = true;
			// 
			// internalLabelComboBox
			// 
			this.internalLabelComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.internalLabelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.internalLabelComboBox.FormattingEnabled = true;
			this.internalLabelComboBox.Location = new System.Drawing.Point(387, 7);
			this.internalLabelComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.internalLabelComboBox.Name = "internalLabelComboBox";
			this.internalLabelComboBox.Size = new System.Drawing.Size(896, 39);
			this.internalLabelComboBox.TabIndex = 18;
			// 
			// terminalLabelComboBox
			// 
			this.terminalLabelComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terminalLabelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.terminalLabelComboBox.FormattingEnabled = true;
			this.terminalLabelComboBox.Location = new System.Drawing.Point(387, 67);
			this.terminalLabelComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.terminalLabelComboBox.Name = "terminalLabelComboBox";
			this.terminalLabelComboBox.Size = new System.Drawing.Size(896, 39);
			this.terminalLabelComboBox.TabIndex = 19;
			// 
			// correctionFactorControlM2
			// 
			this.correctionFactorControlM2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.correctionFactorControlM2.Location = new System.Drawing.Point(400, 137);
			this.correctionFactorControlM2.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.correctionFactorControlM2.Name = "correctionFactorControlM2";
			this.correctionFactorControlM2.Size = new System.Drawing.Size(870, 26);
			this.correctionFactorControlM2.TabIndex = 20;
			// 
			// correctionFactorControlM1
			// 
			this.correctionFactorControlM1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.correctionFactorControlM1.Location = new System.Drawing.Point(400, 197);
			this.correctionFactorControlM1.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.correctionFactorControlM1.Name = "correctionFactorControlM1";
			this.correctionFactorControlM1.Size = new System.Drawing.Size(870, 26);
			this.correctionFactorControlM1.TabIndex = 21;
			// 
			// correctionFactorControlP1
			// 
			this.correctionFactorControlP1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.correctionFactorControlP1.Location = new System.Drawing.Point(400, 257);
			this.correctionFactorControlP1.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.correctionFactorControlP1.Name = "correctionFactorControlP1";
			this.correctionFactorControlP1.Size = new System.Drawing.Size(870, 26);
			this.correctionFactorControlP1.TabIndex = 22;
			// 
			// correctionFactorControlP2
			// 
			this.correctionFactorControlP2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.correctionFactorControlP2.Location = new System.Drawing.Point(400, 317);
			this.correctionFactorControlP2.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.correctionFactorControlP2.Name = "correctionFactorControlP2";
			this.correctionFactorControlP2.Size = new System.Drawing.Size(870, 26);
			this.correctionFactorControlP2.TabIndex = 23;
			// 
			// IsobaricLabelsComplexEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1307, 525);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "IsobaricLabelsComplexEditForm";
			this.Text = "Define isobaric label";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private Table.HelpLabel helpLabel1;
		private Table.HelpLabel helpLabel13;
		private Table.HelpLabel helpLabel11;
		private Table.HelpLabel helpLabel9;
		private Table.HelpLabel helpLabel7;
		private Table.HelpLabel helpLabel5;
		private Table.HelpLabel helpLabel3;
		private System.Windows.Forms.CheckBox tmtLikeCheckBox;
		private System.Windows.Forms.ComboBox internalLabelComboBox;
		private System.Windows.Forms.ComboBox terminalLabelComboBox;
		private System.Windows.Forms.TextBox correctionFactorControlM2;
		private System.Windows.Forms.TextBox correctionFactorControlM1;
		private System.Windows.Forms.TextBox correctionFactorControlP1;
		private System.Windows.Forms.TextBox correctionFactorControlP2;
	}
}