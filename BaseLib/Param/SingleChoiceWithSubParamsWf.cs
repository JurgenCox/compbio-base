using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param {
	[Serializable]
	public class SingleChoiceWithSubParamsWf : SingleChoiceWithSubParams {
		[NonSerialized] private TableLayoutPanel control;
		public SingleChoiceWithSubParamsWf(string name) : base(name) { }
		public SingleChoiceWithSubParamsWf(string name, int value) : base(name, value) { }
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl() {
			if (control == null || control.IsDisposed) {
				return;
			}
			ComboBox cb = (ComboBox) control.Controls[0];
			if (cb != null) {
				Value = cb.SelectedIndex;
			}
			foreach (Parameters p in SubParams) {
				p.SetValuesFromControl();
			}
		}

		public override void UpdateControlFromValue() {
			if (control == null || control.IsDisposed) {
				return;
			}
			ComboBox cb = (ComboBox) control.Controls[0];
			if (Value >= 0 && Value < Values.Count) {
				cb.SelectedIndex = Value;
			}
			foreach (Parameters p in SubParams) {
				p.UpdateControlsFromValue();
			}
		}

		public override object CreateControl() {
			ParameterPanel[] panels = new ParameterPanel[SubParams.Count];
			float panelHeight = 0;
			for (int i = 0; i < panels.Length; i++) {
				panels[i] = new ParameterPanel();
				float h = panels[i].Init(SubParams[i], ParamNameWidth, (int) TotalWidth);
				panelHeight = Math.Max(panelHeight, h);
			}
			panelHeight += 7;
			ComboBox cb = new ComboBox {DropDownStyle = ComboBoxStyle.DropDownList};
			cb.SelectedIndexChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			if (Values != null) {
				foreach (string value in Values) {
					cb.Items.Add(value);
				}
				if (Value >= 0 && Value < Values.Count) {
					cb.SelectedIndex = Value;
				}
			}
			TableLayoutPanel grid = new TableLayoutPanel();
			//float sfx = FormUtils.GetDpiScale(grid.CreateGraphics());
			float sfx = 1;
			grid.RowStyles.Add(new RowStyle(SizeType.Absolute, paramHeight));
			grid.RowStyles.Add(new RowStyle(SizeType.Absolute, sfx * panelHeight));
			cb.Dock = DockStyle.Fill;
			grid.Controls.Add(cb, 0, 0);
			Panel placeholder = new Panel {Margin = new Padding(0), Dock = DockStyle.Fill};
			grid.Controls.Add(placeholder, 0, 1);
			foreach (ParameterPanel t in panels) {
				t.Dock = DockStyle.Top;
			}
			if (Value >= 0 && panels.Length > 0) {
				placeholder.Controls.Add(panels[Value]);
			}
			cb.SelectedIndexChanged += (sender, e) => {
				placeholder.Controls.Clear();
				if (cb.SelectedIndex >= 0) {
					placeholder.Controls.Add(panels[cb.SelectedIndex]);
				}
			};
			grid.Width = (int) TotalWidth;
			grid.Dock = DockStyle.Top;
			control = grid;
			return control;
		}
	}
}