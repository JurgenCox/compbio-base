using System;
using System.Windows.Forms;

namespace BaseLib.Query{
	public partial class IntMinMaxQueryForm : Form{
		public IntMinMaxQueryForm(int value1, int value2, int min, int max){
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			numericUpDown1.Value = value1;
			numericUpDown1.Minimum = min;
			numericUpDown1.Maximum = max;
			numericUpDown2.Value = value2;
			numericUpDown2.Minimum = min;
			numericUpDown2.Maximum = max;
			numericUpDown1.KeyDown += TextBox1OnKeyDown;
			numericUpDown2.KeyDown += TextBox1OnKeyDown;
			numericUpDown1.ValueChanged += NumericUpDown1OnValueChanged;
			numericUpDown2.ValueChanged += NumericUpDown2OnValueChanged;
			ActiveControl = numericUpDown1;
		}

		private void NumericUpDown2OnValueChanged(object sender, EventArgs eventArgs){
			if (numericUpDown2.Value < numericUpDown1.Value){
				numericUpDown1.Value = numericUpDown2.Value;
			}
		}

		private void NumericUpDown1OnValueChanged(object sender, EventArgs eventArgs){
			if (numericUpDown2.Value < numericUpDown1.Value){
				numericUpDown2.Value = numericUpDown1.Value;
			}
		}

		public int Value1 => (int) numericUpDown1.Value;
		public int Value2 => (int) numericUpDown2.Value;

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