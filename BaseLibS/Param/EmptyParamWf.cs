using System;
using BaseLibS.Graph;
namespace BaseLibS.Param{
	[Serializable]
	public class EmptyParamWf : EmptyParam{
		[NonSerialized] private LabelModel labelModel;
		public EmptyParamWf(string name) : base(name){
		}
		public EmptyParamWf(string name, string help, string url, bool visible, bool value, bool default1) : base(
			name, help, url, visible, value, default1){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (labelModel == null){
				return;
			}
			Value = false;
		}
		public override void UpdateControlFromValue(){
		}
		public override object CreateControl(){
			labelModel = new LabelModel();
			return labelModel;
		}
		public override object Clone(){
			return new EmptyParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}