using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param {
	[Serializable]
	public class FastaFilesParamWf : FastaFilesParam {
		[NonSerialized] private FastaFilesParamControl control;
		internal FastaFilesParamWf(string name) : base(name) { }
		internal FastaFilesParamWf(string name, string[][] value) : base(name, value) { }
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl() {
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.Value;
		}

		public override void UpdateControlFromValue() {
			if (control == null || control.IsDisposed) {
				return;
			}
			control.Value = Value;
		}

		public override object CreateControl() {
			return control = new FastaFilesParamControl(HasVariationData, HasModifications);
		}
	}
}