using System;
using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class FolderParameterControl : UserControl{
		public FolderParameterControl(){
			InitializeComponent();
			button1.Click += ButtonClick;
		}

		private void ButtonClick(object sender, EventArgs e){
			FolderBrowserDialog ofd = new FolderBrowserDialog();
			if (ofd.ShowDialog() == DialogResult.OK){
				textBox1.Text = ofd.SelectedPath;
			}
		}

		public string Text1{
			get => textBox1.Text;
			set => textBox1.Text = value;
		}
	}
}