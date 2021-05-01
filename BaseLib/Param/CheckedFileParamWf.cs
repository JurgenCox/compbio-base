using System;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class CheckedFileParamWf : CheckedFileParam{
		[NonSerialized] private CheckedFileParamControl control;

		public CheckedFileParamWf(string name, string value, Func<string, Tuple<string, bool>> checkFileName) : base(
			name, value, checkFileName){ }

		public CheckedFileParamWf(string name, Func<string, Tuple<string, bool>> checkFileName) : base(name,
			checkFileName){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			CheckedFileParamControl vm = control;
			Value = vm.FileName;
		}

		public override void UpdateControlFromValue(){
			if (control == null){
				return;
			}
			CheckedFileParamControl vm = control;
			vm.FileName = Value;
		}

		public override object CreateControl(){
			control = new CheckedFileParamControl(Value, Filter, ProcessFileName, checkFileName);
			return control;
		}
	}
}