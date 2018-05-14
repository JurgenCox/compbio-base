namespace BaseLib.Forms
{
    partial class PatternMatchParamControl
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
            this.PreviewDataGridView = new System.Windows.Forms.DataGridView();
            this.RegexTextBox = new System.Windows.Forms.TextBox();
            this.RegexLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviewDataGridView
            // 
            this.PreviewDataGridView.AllowUserToAddRows = false;
            this.PreviewDataGridView.AllowUserToDeleteRows = false;
            this.PreviewDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.PreviewDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.PreviewDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PreviewDataGridView.Location = new System.Drawing.Point(23, 74);
            this.PreviewDataGridView.Name = "PreviewDataGridView";
            this.PreviewDataGridView.ReadOnly = true;
            this.PreviewDataGridView.Size = new System.Drawing.Size(240, 150);
            this.PreviewDataGridView.TabIndex = 0;
            this.PreviewDataGridView.SelectionChanged += new System.EventHandler(this.PreviewDataGridView_SelectionChanged);
            // 
            // RegexTextBox
            // 
            this.RegexTextBox.Location = new System.Drawing.Point(75, 28);
            this.RegexTextBox.Name = "RegexTextBox";
            this.RegexTextBox.Size = new System.Drawing.Size(188, 20);
            this.RegexTextBox.TabIndex = 1;
            this.RegexTextBox.TextChanged += new System.EventHandler(this.RegexTextBox_TextChanged);
            // 
            // RegexLabel
            // 
            this.RegexLabel.AutoSize = true;
            this.RegexLabel.Location = new System.Drawing.Point(31, 31);
            this.RegexLabel.Name = "RegexLabel";
            this.RegexLabel.Size = new System.Drawing.Size(38, 13);
            this.RegexLabel.TabIndex = 2;
            this.RegexLabel.Text = "Regex";
            // 
            // RegexMatchParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RegexLabel);
            this.Controls.Add(this.RegexTextBox);
            this.Controls.Add(this.PreviewDataGridView);
            this.Name = "PatternMatchParamControl";
            this.Size = new System.Drawing.Size(291, 376);
            this.Load += new System.EventHandler(this.RegexMatchParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PreviewDataGridView;
        private System.Windows.Forms.TextBox RegexTextBox;
        private System.Windows.Forms.Label RegexLabel;
    }
}
