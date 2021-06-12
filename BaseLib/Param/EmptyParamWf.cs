using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class EmptyParamWf : EmptyParam{
		[NonSerialized] private Control control;
		internal EmptyParamWf(string name) : base(name){ }

		protected EmptyParamWf(string name, string help, string url, bool visible, bool value, bool default1) : base(
			name, help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = false;
		}

		public override void UpdateControlFromValue(){ }

		public override object CreateControl(){
			control = new Control();
			return control;
		}

		public override object Clone(){
			return new EmptyParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}