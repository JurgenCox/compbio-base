using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class MultiFileParamWf : MultiFileParam{
		[NonSerialized] private MultiFileParameterControl control;
		internal MultiFileParamWf(string name) : base(name){ }
		internal MultiFileParamWf(string name, string[] value) : base(name, value){ }

		protected MultiFileParamWf(string name, string help, string url, bool visible, string[] value,
			string[] default1, string filter) : base(name, help, url, visible, value, default1, filter){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.Filenames;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Filenames = Value;
		}

		public override object CreateControl(){
			return control = new MultiFileParameterControl{Filter = Filter, Filenames = Value};
		}

		public override object Clone(){
			return new MultiFileParamWf(Name, Help, Url, Visible, Value, Default, Filter);
		}
	}
}