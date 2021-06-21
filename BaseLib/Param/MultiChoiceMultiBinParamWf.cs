using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class MultiChoiceMultiBinParamWf : MultiChoiceMultiBinParam{
		[NonSerialized] private MultiListSelectorControl control;
		internal MultiChoiceMultiBinParamWf(string name) : base(name){ }
		internal MultiChoiceMultiBinParamWf(string name, int[][] value) : base(name, value){ }

		protected MultiChoiceMultiBinParamWf(string name, string help, string url, bool visible, int[][] value,
			int[][] default1, IList<string> values, IList<string> bins) : base(name, help, url, visible, value,
			default1, values, bins){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.SelectedIndices;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
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

		public override object Clone(){
			return new MultiChoiceMultiBinParamWf(Name, Help, Url, Visible, Value, Default, Values, Bins);
		}
	}
}