using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class MultiChoiceMultiBinParamWf : MultiChoiceMultiBinParam{
		[NonSerialized] private MultiListSelectorControl control;
		internal MultiChoiceMultiBinParamWf(string name) : base(name){}
		internal MultiChoiceMultiBinParamWf(string name, int[][] value) : base(name, value){}
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.SelectedIndices;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed) {
				return;
			}
			control.SelectedIndices = Value;
		}

		public override object CreateControl(){
			control = new MultiListSelectorControl();
			control.Init(Values, Bins);
			control.SelectedIndices = Value;
			return control;
		}
	}
}