using System.Windows.Forms;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class CorrectionFactorControl2 : UserControl{
		public CorrectionFactorControl2(){
			InitializeComponent();
		}

		public string Label1{
			get => label1.Text;
			set => label1.Text = value;
		}

		public string Label2{
			get => label2.Text;
			set => label2.Text = value;
		}

		public double Value1{
			get => Parser.TryDouble(textBox1.Text, out double x) ? x : 0;
			set => textBox1.Text = "" + value;
		}

		public double Value2{
			get => Parser.TryDouble(textBox2.Text, out double x) ? x : 0;
			set => textBox2.Text = "" + value;
		}
	}
}