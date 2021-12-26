using System;
using BaseLibS.Graph;
namespace BaseLibS.Param{
	[Serializable]
	public class StringParamWf : StringParam{
		[NonSerialized] private TextFieldModel textField;
		public StringParamWf(string name) : base(name){
		}
		public StringParamWf(string name, string value) : base(name, value){
		}
		public StringParamWf(string name, string help, string url, bool visible, string value, string default1) :
			base(name, help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (textField == null){
				return;
			}
			Value = textField.Text;
		}
		public override void UpdateControlFromValue(){
			if (textField == null){
				return;
			}
			textField.Text = Value;
		}
		public override object CreateControl(){
			return textField = new TextFieldModel(Value){LineHeight = 12};
		}
		public override object Clone(){
			return new StringParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}