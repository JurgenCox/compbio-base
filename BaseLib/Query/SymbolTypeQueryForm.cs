using System;
using System.Windows.Forms;
using BaseLibS.Symbol;

namespace BaseLib.Query{
	public partial class SymbolTypeQueryForm : Form{
		private readonly bool hasNoSymbol;
		public SymbolTypeQueryForm() : this(false){}

		public SymbolTypeQueryForm(bool hasNoSymbol){
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			this.hasNoSymbol = hasNoSymbol;
			if (hasNoSymbol){
				comboBox1.Items.Add("<None>");
			}
			foreach (string name in SymbolType.allNames){
				comboBox1.Items.Add(name);
			}
			comboBox1.SelectedIndex = 0;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			comboBox1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = comboBox1;
		}

		public int SymbolTypeIndex{
			get{
				int selInd = comboBox1.SelectedIndex;
				return hasNoSymbol ? selInd - 1 : selInd;
			}
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