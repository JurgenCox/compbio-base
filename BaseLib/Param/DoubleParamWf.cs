using System;
using System.Windows.Forms;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLib.Param{
	[Serializable]
	internal class DoubleParamWf : DoubleParam{
		[NonSerialized] private TextBox control;
		internal DoubleParamWf(string name, double value) : base(name, value){ }

		protected DoubleParamWf(string name, string help, string url, bool visible, double value, double default1) :
			base(name, help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			bool success = Parser.TryDouble(control.Text, out double val);
			val = success ? val : double.NaN;
			Value = val;
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
			return new DoubleParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}