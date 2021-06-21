using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class DictionaryStringValueParamWf : DictionaryStringValueParam{
		[NonSerialized] private DictionaryStringValueControl control;
		internal DictionaryStringValueParamWf(string name, Dictionary<string, string> value) : base(name, value){ }

		protected DictionaryStringValueParamWf(string name, string help, string url, bool visible,
			Dictionary<string, string> value, Dictionary<string, string> default1, string keyName, string valueName) :
			base(name, help, url, visible, value, default1, keyName, valueName){ }

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

		public override object Clone(){
			return new DictionaryStringValueParamWf(Name, Help, Url, Visible, Value, Default, KeyName, ValueName);
		}
	}
}