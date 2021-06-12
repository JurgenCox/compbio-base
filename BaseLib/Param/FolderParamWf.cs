using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class FolderParamWf : FolderParam{
		[NonSerialized] private FolderParameterControl control;
		internal FolderParamWf(string name) : base(name){ }
		internal FolderParamWf(string name, string value) : base(name, value){ }

		protected FolderParamWf(string name, string help, string url, bool visible, string value, string default1) :
			base(name, help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.Text1;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Text1 = Value;
		}

		public override object CreateControl(){
			return control = new FolderParameterControl{Text1 = Value};
		}

		public override object Clone(){
			return new FolderParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}