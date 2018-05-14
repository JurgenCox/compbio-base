using System;
using System.Windows.Forms;

namespace BaseLib.Query{
	public partial class FolderQueryForm : Form{
		private bool hasRecursiveBox;
		public FolderQueryForm() : this(""){}

		public FolderQueryForm(string value){
			InitializeComponent();
			StartPosition = FormStartPosition.Manual;
			okButton.Click += OkButtonOnClick;
			cancelButton.Click += CancelButtonOnClick;
			browseButton.Click += BrowseButtonOnClick;
			textBox1.Text = value;
			textBox1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = textBox1;
		}

		private void BrowseButtonOnClick(object sender, EventArgs eventArgs){
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK){
				textBox1.Text = fbd.SelectedPath;
			}
		}

		public bool HasRecursiveBox{
			get => hasRecursiveBox;
			set{
				hasRecursiveBox = value;
				recursiveCheckBox.Width = value ? 90 : 0;
				recursiveCheckBox.Margin = new Padding(value ? 2 : 0);
			}
		}

		public bool Recursive => recursiveCheckBox.Checked;
		public string Value => textBox1.Text;

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