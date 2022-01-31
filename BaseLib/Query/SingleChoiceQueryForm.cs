using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Param;
namespace BaseLib.Query {
	public class SingleChoiceQueryForm : GenericQueryForm{
		private readonly SingleChoiceParamWf param;
		public SingleChoiceQueryForm(string name, int selected, string[] values) {
			Text = name;
			ClientSize = new Size(400, 54);
			param = new SingleChoiceParamWf(name, selected) {
				Values = values
			};
			InitializeComponent();
		}
		public int Value => param.Value2;
		private void InitializeComponent() {
			Control c = FormUtil.GetControl(param.CreateControl());
			c.Dock = DockStyle.Fill;
			tableLayoutPanel1.Controls.Add(c, 0, 0);
			ActiveControl = c;
		}

		public string SelectedText => param.Values[param.Value];
	}
}
