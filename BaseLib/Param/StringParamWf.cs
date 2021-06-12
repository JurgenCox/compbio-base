using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class StringParamWf : StringParam{
		[NonSerialized] private TextBox control;
		internal StringParamWf(string name) : base(name){ }
		internal StringParamWf(string name, string value) : base(name, value){ }

		protected StringParamWf(string name, string help, string url, bool visible, string value, string default1) :
			base(name, help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.Text;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Text = Value;
		}

		public override object CreateControl(){
			return control = new TextBox{Text = Value};
		}

		public override object Clone(){
			return new StringParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}