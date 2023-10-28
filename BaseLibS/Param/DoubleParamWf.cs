using System;
using BaseLibS.Graph;
using BaseLibS.Util;
namespace BaseLibS.Param{
	[Serializable]
	public class DoubleParamWf : DoubleParam{
		[NonSerialized] private TextFieldModel textField;
		public DoubleParamWf() : base("", double.NaN) {
		}
		public DoubleParamWf(string name, double value) : base(name, value) {
		}
		public DoubleParamWf(string name, string help, string url, bool visible, double value, double default1) :
			base(name, help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (textField == null){
				return;
			}
			bool success = Parser.TryDouble(textField.Text, out double val);
			val = success ? val : double.NaN;
			Value = val;
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
			return new DoubleParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}