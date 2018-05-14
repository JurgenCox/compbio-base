using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BaseLib.Query{
	public partial class SingleChoiceQueryForm : Form{
		public SingleChoiceQueryForm(IEnumerable<string> choice){
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			foreach (string s in choice){
				comboBox1.Items.Add(s);
			}
			comboBox1.SelectedIndex = 0;
			comboBox1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = comboBox1;
		}

		public string SelectedText => comboBox1.Text;

		public string Label{
			set => label1.Text = value;
		}

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