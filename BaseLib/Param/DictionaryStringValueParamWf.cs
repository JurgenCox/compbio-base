using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class DictionaryStringValueParamWf : DictionaryStringValueParam{
		[NonSerialized] private DictionaryStringValueControl control;
		internal DictionaryStringValueParamWf(string name, Dictionary<string, string> value) : base(name, value){ }
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.Value;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Value = Value;
		}

		public override object CreateControl(){
			return control = new DictionaryStringValueControl{KeyName = KeyName, ValueName = ValueName, Value = Value};
		}
	}
}