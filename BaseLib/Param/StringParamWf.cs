using System;
using System.Windows.Forms;
using BaseLib.Forms.Scroll;
using BaseLibS.Graph;
using BaseLibS.Param;
namespace BaseLib.Param{
	[Serializable]
	internal class StringParamWf : StringParam{
		[NonSerialized] private TextBox control;
		[NonSerialized] private TextFieldModel textField;
		[NonSerialized] private SimpleScrollableControl textFieldControl;
		internal StringParamWf(string name) : base(name){
		}
		internal StringParamWf(string name, string value) : base(name, value){
		}
		protected StringParamWf(string name, string help, string url, bool visible, string value, string default1) :
			base(name, help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (GraphUtil.newParameterPanel){
				if (textFieldControl == null || textFieldControl.IsDisposed){
					return;
				}
				Value = textField.Text;
			} else{
				if (control == null || control.IsDisposed){
					return;
				}
				Value = control.Text;
			}
		}
		public override void UpdateControlFromValue(){
			if (GraphUtil.newParameterPanel){
				if (textFieldControl == null || textFieldControl.IsDisposed){
					return;
				}
				textField.Text = Value;
			} else{
				if (control == null || control.IsDisposed){
					return;
				}
				control.Text = Value;
			}
		}
		public override object CreateControl(){
			if (GraphUtil.newParameterPanel){
				textField = new TextFieldModel(Value) { LineHeight = 12 };
				return textFieldControl = new SimpleScrollableControl{
					Client = textField
				};
			} else{
				return control = new TextBox{Text = Value};
			}
		}
		public override object Clone(){
			return new StringParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}