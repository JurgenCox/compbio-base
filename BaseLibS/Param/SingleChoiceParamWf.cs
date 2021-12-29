using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibS.Graph;
namespace BaseLibS.Param{
	[Serializable]
	public class SingleChoiceParamWf : SingleChoiceParam{
		[NonSerialized] private ComboBoxModel controlModel;
		public SingleChoiceParamWf(string name) : base(name){
		}
		public SingleChoiceParamWf(string name, int value) : base(name, value){
		}
		protected SingleChoiceParamWf(string name, string help, string url, bool visible, int value, int default1,
			IList<string> values) : base(name, help, url, visible, value, default1, values){
		}
		public override ParamType Type => ParamType.WinForms;
		public override void SetValueFromControl(){
			if (controlModel == null){
				return;
			}
			int val = controlModel.SelectedIndex;
			Value = val;
		}
		public override void UpdateControlFromValue(){
			if (controlModel == null){
				return;
			}
			if (Value >= 0 && Value < Values.Count){
				controlModel.SelectedIndex = Value;
			}
		}
		public void UpdateControlFromValue2(){
			if (controlModel != null && Values != null){
				controlModel.Values = Values.ToArray();
				if (Value >= 0 && Value < Values.Count){
					controlModel.SelectedIndex = Value;
				}
			}
			if (controlModel != null && Value >= 0 && Value < Values.Count){
				controlModel.SelectedIndex = Value;
			}
		}
		public override object CreateControl(){
			ComboBoxModel cb = new ComboBoxModel();
			cb.SelectedIndexChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			if (Values != null){
				cb.Values = Values.ToArray();
				if (Value >= 0 && Value < Values.Count){
					cb.SelectedIndex = Value;
				}
			}
			controlModel = cb;
			return controlModel;
		}
		public override object Clone(){
			return new SingleChoiceParamWf(Name, Help, Url, Visible, Value, Default, Values);
		}
	}
}