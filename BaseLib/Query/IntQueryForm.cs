using System;
using System.Windows.Forms;

namespace BaseLib.Query{
	public partial class IntQueryForm : Form{
		public IntQueryForm(int value, int min, int max){
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			numericUpDown1.Minimum = min;
			numericUpDown1.Maximum = max;
			numericUpDown1.Value = value;
			numericUpDown1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = numericUpDown1;
		}

		public int Value => (int)numericUpDown1.Value;

		private void TextBox1OnKeyDown(object sender, KeyEventArgs keyEventArgs){
			if (keyEventArgs.KeyCode == Keys.Return){
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void CancelButtonOnClick(object sender, EventArgs eventArgs){
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs){
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}