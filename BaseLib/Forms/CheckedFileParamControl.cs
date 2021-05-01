using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class CheckedFileParamControl : UserControl{
		private readonly Func<string, string> processFileName;
		private readonly Func<string, Tuple<string, bool>> checkFileName;

		private string fileName;

		public string FileName{
			get => fileName;
			set{
				fileName = value;
				filePathTextBox.Text = value;
				Tuple<string, bool> x = checkFileName(value);
				if (x != null){
					SetMessage(x.Item1, x.Item2);
				}
			}
		}

		private readonly OpenFileDialog dialog;
		private readonly ToolTip toolTip;

		public CheckedFileParamControl(string value, string filter, Func<string, string> processFileName,
			Func<string, Tuple<string, bool>> checkFileName){
			InitializeComponent();
			toolTip = new ToolTip();
			this.checkFileName = checkFileName ?? ((s) => null);
			FileName = value;
			dialog = new OpenFileDialog();
			if (!string.IsNullOrEmpty(filter)){
				dialog.Filter = filter;
			}
			this.processFileName = processFileName ?? (s => s);
		}

		private void SetMessage(string text, bool ok){
			selectButton.BackColor = ok ? Color.LimeGreen : Color.Red;
			toolTip.SetToolTip(selectButton, text);
		}

		private void ChooseFile(object sender, EventArgs e){
			if (dialog.ShowDialog() == DialogResult.OK){
				FileName = processFileName(dialog.FileName);
			}
		}
	}
}