using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param {
	public class ParameterPanel : UserControl {
		public Parameters Parameters { get; private set; }
		public bool Collapsible { get; set; }
		public bool CollapsedDefault { get; set; }
		private ParameterGroupPanel[] parameterGroupPanels;

		public ParameterPanel() {
			Collapsible = true;
		}

		public float Init(Parameters parameters1) {
			return Init(parameters1, 250F, 1050);
		}

		public float Init(Parameters parameters1, float paramNameWidth, int totalWidth) {
			foreach (Control control in Controls) {
				control?.Dispose();
			}
			Controls.Clear();
			Parameters = parameters1;
			Parameters.Convert(WinFormsParameterFactory.Convert);
			int nrows = Parameters.GroupCount;
			parameterGroupPanels = new ParameterGroupPanel[nrows];
			TableLayoutPanel grid = new TableLayoutPanel {AutoScroll = true};
			grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			grid.Name = "tableLayoutPanel";
			int totalHeight = 0;
			for (int i = 0; i < nrows; i++) {
				int h = (int) (parameters1.GetGroup(i).Height + 26);
				grid.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
				totalHeight += h;
			}
			grid.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100));
			grid.Width = totalWidth;
			grid.Height = totalHeight;
			for (int i = 0; i < nrows; i++) {
				AddParameterGroup(parameters1.GetGroup(i), i, paramNameWidth, totalWidth, grid);
			}
			grid.Dock = DockStyle.Fill;
			Controls.Add(grid);
			Name = "ParameterPanel";
			Width = totalWidth;
			Height = totalHeight;
			return totalHeight;
		}

		private void AddParameterGroup(ParameterGroup p, int i, float paramNameWidth, int totalWidth, TableLayoutPanel grid) {
			ParameterGroupPanel pgp = new ParameterGroupPanel();
			parameterGroupPanels[i] = pgp;
			pgp.Init(p, paramNameWidth, totalWidth);
			pgp.Dock = DockStyle.Fill;
			if (p.Name == null) {
				grid.Controls.Add(pgp, 0, i);
			} else {
				GroupBox gb = new GroupBox {
					Text = p.Name,
					Margin = new Padding(3),
					Padding = new Padding(3),
					Dock = DockStyle.Fill,
				};
				gb.Controls.Add(pgp);
				grid.Controls.Add(gb, 0, i);
			}
		}

		public void SetParameters() {
			Parameters p1 = Parameters;
			for (int i = 0; i < p1.GroupCount; i++) {
				p1.GetGroup(i).SetParametersFromConrtol();
			}
		}

		public void Disable() {
			foreach (ParameterGroupPanel parameterGroupPanel in parameterGroupPanels) {
				parameterGroupPanel.Disable();
			}
		}

		public void Enable() {
			foreach (ParameterGroupPanel parameterGroupPanel in parameterGroupPanels) {
				parameterGroupPanel.Enable();
			}
		}
	}
}