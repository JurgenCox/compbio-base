using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class CorrectionFactorControl : UserControl{
		private bool twoValues;

		public CorrectionFactorControl(){
			InitializeComponent();
			TextBox singleTextBox = new TextBox{
				Dock = DockStyle.Fill,
				Location = new System.Drawing.Point(30, 0),
				Margin = new Padding(0),
				Name = "singleTextBox",
				Size = new System.Drawing.Size(285, 20),
				TabIndex = 1
			};
			CorrectionFactorControl2 twoTextBoxes = new CorrectionFactorControl2{
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
	}
}