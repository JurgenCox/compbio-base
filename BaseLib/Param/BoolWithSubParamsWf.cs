using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class BoolWithSubParamsWf : BoolWithSubParams{
		[NonSerialized] private TableLayoutPanel control;
		internal BoolWithSubParamsWf(string name) : base(name){}
		internal BoolWithSubParamsWf(string name, bool value) : base(name, value){}
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			CheckBox cb = (CheckBox) control.Controls[0];
			Value = cb.Checked;
			SubParamsFalse.SetValuesFromControl();
			SubParamsTrue.SetValuesFromControl();
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed) {
				return;
			}
			CheckBox cb = (CheckBox) control?.Controls[0];
			if (cb == null){
				return;
			}
			cb.Checked = Value;
			SubParamsFalse?.UpdateControlsFromValue();
			SubParamsTrue?.UpdateControlsFromValue();
		}

		public override object CreateControl(){
			ParameterPanel panelFalse = new ParameterPanel();
			ParameterPanel panelTrue = new ParameterPanel();
			panelFalse.Init(SubParamsFalse, ParamNameWidth, (int) TotalWidth);
			panelTrue.Init(SubParamsTrue, ParamNameWidth, (int) TotalWidth);
			CheckBox cb = new CheckBox{Checked = Value};
			cb.CheckedChanged += (sender, e) => ValueHasChanged();
			TableLayoutPanel tlp = new TableLayoutPanel();
			tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, paramHeight));
			tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize, 100));
			tlp.Controls.Add(cb, 0, 0);
			panelFalse.Visible = !Value;
			panelTrue.Visible = Value;
			tlp.Controls.Add(panelFalse, 0, 1);
			tlp.Controls.Add(panelTrue, 0, 1);
			cb.CheckedChanged += (sender, e) =>{
				panelFalse.Visible = !cb.Checked;
				panelTrue.Visible = cb.Checked;
			    Value = cb.Checked;
			};
			tlp.PerformLayout();
			control = tlp;
			return control;
		}
	}
}