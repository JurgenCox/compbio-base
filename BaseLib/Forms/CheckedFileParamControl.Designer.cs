using System.Runtime.CompilerServices;

namespace BaseLib.Forms
{
    partial class CheckedFileParamControl
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
			this.filePathTextBox = new System.Windows.Forms.TextBox();
			this.selectButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.63063F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.36937F));
			this.tableLayoutPanel1.Controls.Add(this.filePathTextBox, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.selectButton, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1525, 100);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// filePathTextBox
			// 
			this.filePathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filePathTextBox.Location = new System.Drawing.Point(0, 0);
			this.filePathTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.filePathTextBox.Name = "filePathTextBox";
			this.filePathTextBox.Size = new System.Drawing.Size(1229, 38);
			this.filePathTextBox.TabIndex = 0;
			// 
			// selectButton
			// 
			this.selectButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.selectButton.Location = new System.Drawing.Point(1229, 0);
			this.selectButton.Margin = new System.Windows.Forms.Padding(0);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(296, 48);
			this.selectButton.TabIndex = 1;
			this.selectButton.Text = "Select";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.ChooseFile);
			// 
			// CheckedFileParamControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "CheckedFileParamControl";
			this.Size = new System.Drawing.Size(1544, 100);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button selectButton;
    }
}
