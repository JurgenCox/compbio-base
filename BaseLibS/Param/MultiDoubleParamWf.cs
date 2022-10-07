using System;
using BaseLibS.Graph;
using BaseLibS.Util;
namespace BaseLibS.Param{
	[Serializable]
	public class MultiDoubleParamWf : MultiDoubleParam{
		[NonSerialized] private TextFieldModel textField;
		public MultiDoubleParamWf(string name) : base(name) { }
		public MultiDoubleParamWf(string name, double[] value) : base(name, value) { }

		public MultiDoubleParamWf(string name, string help, string url, bool visible, double[] value,
			double[] default1) : base(name, help, url, visible, value, default1) { }
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (textField == null){
				return;
			}
			Value = ParseDoubles(textField.Text);
		}
		public override void UpdateControlFromValue(){
			if (textField == null){
				return;
			}
			textField.Text = StringUtils.Concat(";", Value);
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
			return new MultiDoubleParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}