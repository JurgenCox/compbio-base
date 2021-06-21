using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class RegexMatchParamWf : RegexMatchParam{
		[NonSerialized] private PatternMatchParamControl control;
		public RegexMatchParamWf(string name, Regex value, List<string> replacement) : base(name, value, replacement){}
		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed) {
				return;
			}
			Value = control.regex;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed) {
				return;
			}
			control.preview = Previews;
			control.Regex = Value.ToString(); // setting as string will refresh the view
		}

		public override float Height => 200;

		public override object CreateControl(){
			control = new PatternMatchParamControl(Value, Previews, pattern => new Regex(pattern));
			return control;
		}
	}
}