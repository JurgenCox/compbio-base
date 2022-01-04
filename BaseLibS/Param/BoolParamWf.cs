using System;
using BaseLibS.Graph;
namespace BaseLibS.Param{
	[Serializable]
	public class BoolParamWf : BoolParam{
		[NonSerialized] private CheckBoxModel control;
		public BoolParamWf(string name) : base(name){
		}
		public BoolParamWf(string name, bool value) : base(name, value){
		}
		public BoolParamWf(string name, string help, string url, bool visible, bool value, bool default1) : base(
			name, help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (control == null){
				return;
			}
			Value = control.Checked;
		}
		public override void UpdateControlFromValue(){
			if (control == null){
				return;
			}
			control.Checked = Value;
		}
		public override object CreateControl(){
			control = new CheckBoxModel{Checked = Value};
			control.CheckedChanged += (sender, e) => { SetValueFromControl(); };
			return control;
		}
		public override object Clone(){
			return new BoolParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}