using System;
using System.Windows.Forms;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLib.Param{
	[Serializable]
	internal class IntParamWf : IntParam{
		[NonSerialized] private TextBox control;
		internal IntParamWf(string name, int value) : base(name, value){ }

		protected IntParamWf(string name, string help, string url, bool visible, int value, int default1) : base(name,
			help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			bool s = Parser.TryInt(control.Text, out int val);
			if (s){
				Value = val;
			}
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Text = Parser.ToString(Value);
		}

		public override object CreateControl(){
			control = new TextBox{Text = Parser.ToString(Value)};
			control.TextChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			return control;
		}

		public override object Clone(){
			return new IntParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}