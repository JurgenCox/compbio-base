using System;
using System.Windows.Forms;
using BaseLibS.Util;

namespace BaseLib.Query {
	public partial class DoubleQueryForm : Form {
		public DoubleQueryForm(double value) {
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			textBox1.Text = Parser.ToString(value);
			textBox1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = textBox1;
		}

		public double Value => Parser.TryDouble(textBox1.Text, out double val) ? val : double.NaN;

		private void TextBox1OnKeyDown(object sender, KeyEventArgs keyEventArgs) {
			if (keyEventArgs.KeyCode == Keys.Return) {
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void CancelButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}