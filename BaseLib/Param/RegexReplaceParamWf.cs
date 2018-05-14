using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class RegexReplaceParamWf : RegexReplaceParam{
		[NonSerialized] private RegexReplaceParamControl control;

		internal RegexReplaceParamWf(string name, Regex pattern, string replacement, List<string> items)
			: base(name, pattern, replacement, items){}

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control?.GetValue();
		}

		//TODO
		public override void UpdateControlFromValue() {
			base.UpdateControlFromValue();
		}

		public override float Height => 200;

		public override object CreateControl(){
            control = new RegexReplaceParamControl(Value.Item1, Value.Item2, Previews);
			return control;
		}
	}
}