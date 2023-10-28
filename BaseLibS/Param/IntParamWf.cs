using System;
using BaseLibS.Graph;
using BaseLibS.Util;
namespace BaseLibS.Param{
	[Serializable]
	public class IntParamWf : IntParam{
		[NonSerialized] private TextFieldModel textField;
		public IntParamWf() : this("", 0) {
		}
		public IntParamWf(string name, int value) : base(name, value) {
		}
		public IntParamWf(string name, string help, string url, bool visible, int value, int default1) : base(name,
			help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (textField == null){
				return;
			}
			bool s = Parser.TryInt(textField.Text, out int val);
			if (s){
				Value = val;
			}
		}
		public override void UpdateControlFromValue(){
			if (textField == null){
				return;
			}
			textField.Text = Parser.ToString(Value);
		}
		public override object CreateControl(){
			textField = new TextFieldModel(Parser.ToString(Value)){LineHeight = 12};
			textField.TextChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			return textField;
		}
		public override object Clone(){
			return new IntParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}