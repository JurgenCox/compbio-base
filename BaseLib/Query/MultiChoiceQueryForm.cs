using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLib.Param;
namespace BaseLib.Query{
	public class MultiChoiceQueryForm : GenericQueryForm{
		private readonly MultiChoiceParamWf param;
		public MultiChoiceQueryForm(string name, int[] selected, string[] values){
			Text = name;
			ClientSize = new Size(500, 260);
			param = new MultiChoiceParamWf(name, selected){
				Values = values
			};
			InitializeComponent();
		}
		public int[] Value => param.Value2;
		private void InitializeComponent() {
			Control c = FormUtil.GetControl(param.CreateControl());
			c.Dock = DockStyle.Fill;
			tableLayoutPanel1.Controls.Add(c, 0, 0);
			ActiveControl = c;
		}
	}
}