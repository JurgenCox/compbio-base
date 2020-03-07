using System.Windows.Forms;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class CorrectionFactorControl : UserControl{
		private bool twoValues;
		private readonly TextBox singleTextBox;
		private readonly CorrectionFactorControl2 twoTextBoxes;

		public CorrectionFactorControl(){
			InitializeComponent();
			singleTextBox = new TextBox{
				Dock = DockStyle.Fill,
				Location = new System.Drawing.Point(30, 0),
				Margin = new Padding(0),
				Name = "singleTextBox",
				Size = new System.Drawing.Size(285, 20),
				TabIndex = 1
			};
			twoTextBoxes = new CorrectionFactorControl2{
				Dock = DockStyle.Fill,
				Location = new System.Drawing.Point(30, 0),
				Margin = new Padding(0),
				Name = "twoTextBoxes",
				Size = new System.Drawing.Size(285, 31),
				TabIndex = 1
			};
			tableLayoutPanel1.Controls.Add(singleTextBox, 1, 0);
			expandButton.Click += (sender, args) => {
				expandButton.Text = twoValues ? "+" : "-";
				if (twoValues){
					tableLayoutPanel1.Controls.Remove(twoTextBoxes);
					tableLayoutPanel1.Controls.Add(singleTextBox, 1, 0);
				} else{
					tableLayoutPanel1.Controls.Remove(singleTextBox);
					tableLayoutPanel1.Controls.Add(twoTextBoxes, 1, 0);
				}
				twoValues = !twoValues;
			};
		}

		public string Label1{
			get => twoTextBoxes.Label1;
			set => twoTextBoxes.Label1 = value;
		}

		public string Label2{
			get => twoTextBoxes.Label2;
			set => twoTextBoxes.Label2 = value;
		}

		public double Value{
			get => Parser.TryDouble(singleTextBox.Text, out double x) ? x : 0;
			set => singleTextBox.Text = "" + value;
		}

		public (double, double) Values{
			get => (twoTextBoxes.Value1, twoTextBoxes.Value2);
			set{
				twoTextBoxes.Value1 = value.Item1;
				twoTextBoxes.Value2 = value.Item2;
			}
		}
	}
}