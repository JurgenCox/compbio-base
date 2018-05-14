using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class BoolParamWf : BoolParam{
		[NonSerialized] private CheckBox control;
		internal BoolParamWf(string name) : base(name){}
		internal BoolParamWf(string name, bool value) : base(name, value){}
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.Checked;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed) {
				return;
			}
			control.Checked = Value;
		}

		public override object CreateControl(){
			control = new CheckBox{Checked = Value};
            control.CheckedChanged += (sender, e) => {SetValueFromControl();};
		    return control;
		}

    }
}