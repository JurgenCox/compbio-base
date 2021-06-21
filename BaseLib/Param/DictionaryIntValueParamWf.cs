using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class DictionaryIntValueParamWf : DictionaryIntValueParam{
		[NonSerialized] private DictionaryIntValueControl control;
		internal DictionaryIntValueParamWf(string name, Dictionary<string, int> value, string[] keys) : base(name, value, keys){}
		public override ParamType Type => ParamType.WinForms;

		public override string[] Keys{
			get => keys;
			set{
				keys = value;
				if (control != null){
					control.Keys = keys;
				}
			}
		}

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.Value;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed) {
				return;
			}
			control.Value = Value;
		}

		public override object CreateControl(){
			return control = new DictionaryIntValueControl{Value = Value, Keys = Keys, Default = DefaultValue};
		}
	}
}