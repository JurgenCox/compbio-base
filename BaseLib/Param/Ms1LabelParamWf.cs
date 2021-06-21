using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class Ms1LabelParamWf : Ms1LabelParam{
		[NonSerialized] private Ms1LabelPanel control;
		internal Ms1LabelParamWf(string name, int[][] value) : base(name, value){ }

		protected Ms1LabelParamWf(string name, string help, string url, bool visible, int[][] value, int[][] default1,
			int multiplicity, string[] values) :
			base(name, help, url, visible, value, default1, multiplicity, values){ }

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
			return control = new Ms1LabelPanel(Multiplicity, Values){SelectedIndices = Value};
		}

		public override object Clone(){
			return new Ms1LabelParamWf(Name, Help, Url, Visible, Value, Default, Multiplicity, Values);
		}
	}
}