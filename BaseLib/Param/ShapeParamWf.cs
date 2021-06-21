using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param {
	[Serializable]
	internal class ShapeParamWf : ShapeParam {
		[NonSerialized] private TextBox control;
		internal ShapeParamWf(string name) : base(name) { }
		internal ShapeParamWf(string name, string value) : base(name, value) { }
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl() {
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.Text;
		}

		public override void UpdateControlFromValue() {
			if (control == null || control.IsDisposed) {
				return;
			}
			control.Text = Value;
		}

		public override object CreateControl() {
			return control = new TextBox {Text = Value};
		}
	}
}