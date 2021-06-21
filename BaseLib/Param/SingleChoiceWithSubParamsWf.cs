using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class SingleChoiceWithSubParamsWf : SingleChoiceWithSubParams{
		[NonSerialized] private TableLayoutPanel control;
		public SingleChoiceWithSubParamsWf(string name) : base(name){ }
		public SingleChoiceWithSubParamsWf(string name, int value) : base(name, value){ }

		protected SingleChoiceWithSubParamsWf(string name, string help, string url, bool visible, int value,
			int default1, float paramNameWidth, float totalWidth, IList<string> values, List<Parameters> subParams) :
			base(name, help, url, visible, value, default1, paramNameWidth, totalWidth, values, subParams){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			ComboBox cb = (ComboBox) control.Controls[0];
			if (cb != null){
				Value = cb.SelectedIndex;
			}
			foreach (Parameters p in SubParams){
				p.SetValuesFromControl();
			}
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			ComboBox cb = (ComboBox) control.Controls[0];
			if (Value >= 0 && Value < Values.Count){
				cb.SelectedIndex = Value;
			}
			foreach (Parameters p in SubParams){
				p.UpdateControlsFromValue();
			}
		}

		public override object CreateControl(){
			ParameterPanel[] panels = new ParameterPanel[SubParams.Count];
			float panelHeight = 0;
			for (int i = 0; i < panels.Length; i++){
				panels[i] = new ParameterPanel();
				float h = panels[i].Init(SubParams[i], ParamNameWidth, (int) TotalWidth);
				panelHeight = Math.Max(panelHeight, h);
			}
			panelHeight += 7;
			ComboBox cb = new ComboBox{DropDownStyle = ComboBoxStyle.DropDownList};
			cb.SelectedIndexChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			if (Values != null){
				foreach (string value in Values){
					cb.Items.Add(value);
				}
				if (Value >= 0 && Value < Values.Count){
					cb.SelectedIndex = Value;
				}
			}
			TableLayoutPanel grid = new TableLayoutPanel();
			grid.RowStyles.Add(new RowStyle(SizeType.Absolute, paramHeight));
			grid.RowStyles.Add(new RowStyle(SizeType.Absolute, panelHeight));
			cb.Dock = DockStyle.Fill;
			grid.Controls.Add(cb, 0, 0);
			Panel placeholder = new Panel{Margin = new Padding(0), Dock = DockStyle.Fill};
			grid.Controls.Add(placeholder, 0, 1);
			foreach (ParameterPanel t in panels){
				t.Dock = DockStyle.Top;
			}
			if (Value >= 0 && panels.Length > 0){
				placeholder.Controls.Add(panels[Value]);
			}
			cb.SelectedIndexChanged += (sender, e) => {
				placeholder.Controls.Clear();
				if (cb.SelectedIndex >= 0){
					placeholder.Controls.Add(panels[cb.SelectedIndex]);
				}
			};
			grid.Width = (int) TotalWidth;
			grid.Dock = DockStyle.Top;
			control = grid;
			return control;
		}

		public override object Clone(){
			List<Parameters> subParams = new List<Parameters>();
			foreach (Parameters p in SubParams){
				subParams.Add((Parameters) p.Clone());
			}
			return new SingleChoiceWithSubParamsWf(Name, Help, Url, Visible, Value, Default, ParamNameWidth, TotalWidth,
				Values, subParams);
		}
	}
}