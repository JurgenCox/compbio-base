using System.Windows.Forms;
using BaseLibS.Util;
namespace BaseLib.Query{
	public class DoubleQueryForm : GenericQueryForm{
		private TextBox textBox1;
		public DoubleQueryForm(double value){
			InitializeComponent();
			textBox1.Text = Parser.ToString(value);
			textBox1.KeyDown += TextBox1OnKeyDown;
			ActiveControl = textBox1;
		}
		private void InitializeComponent(){
			textBox1 = new TextBox();
			tableLayoutPanel1.Controls.Add(textBox1, 0, 0);
			textBox1.Dock = DockStyle.Fill;
			textBox1.Location = new System.Drawing.Point(0, 0);
			textBox1.Margin = new Padding(0);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(285, 20);
			textBox1.TabIndex = 1;
		}
		public double Value => Parser.TryDouble(textBox1.Text, out double val) ? val : double.NaN;
		private void TextBox1OnKeyDown(object sender, KeyEventArgs keyEventArgs){
			if (keyEventArgs.KeyCode == Keys.Return){
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}