using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class FileParamWf : FileParam{
		[NonSerialized] private FileParameterControl control;
		public FileParamWf(string name) : base(name){ }
		public FileParamWf(string name, string value) : base(name, value){ }

		protected FileParamWf(string name, string help, string url, bool visible, string value, string default1,
			string filter, Func<string, string> processFileName, bool save) : base(name, help, url, visible, value,
			default1, filter, processFileName, save){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.FileName;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.FileName = Value;
		}

		public override object CreateControl(){
			control = new FileParameterControl(Value, Filter, ProcessFileName, Save);
			return control;
		}

		public override object Clone(){
			return new FileParamWf(Name, Help, Url, Visible, Value, Default, Filter, ProcessFileName, Save);
		}
	}
}