using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	//TODO: should be internal
	[Serializable]
	public class SingleChoiceParamWf : SingleChoiceParam{
		[NonSerialized] private ComboBox control;
		public SingleChoiceParamWf(string name) : base(name){ }
		public SingleChoiceParamWf(string name, int value) : base(name, value){ }

		protected SingleChoiceParamWf(string name, string help, string url, bool visible, int value, int default1,
			IList<string> values) : base(name, help, url, visible, value, default1, values){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			int val = control.SelectedIndex;
			Value = val;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			if (control != null && Value >= 0 && Value < Values.Count){
				control.SelectedIndex = Value;
			}
		}

		public void UpdateControlFromValue2(){
			if (control != null && Values != null){
				control.Items.Clear();
				foreach (string value in Values){
					control.Items.Add(value);
				}
				if (Value >= 0 && Value < Values.Count){
					control.SelectedIndex = Value;
				}
			}
			if (control != null && Value >= 0 && Value < Values.Count){
				control.SelectedIndex = Value;
			}
		}

		public override object CreateControl(){
			ComboBox cb = new ComboBox{DropDownStyle = ComboBoxStyle.DropDownList};
			cb.SelectedIndexChanged += (sender, e) => {
				SetValueFromControl();
				ValueHasChanged();
			};
			if (Values != null){
				foreach (string value in Values){
					cb.Items.Add(value);
				}
				if (Value >= 0 && Value < Values.Count){
					cb.SelectedIndex = Value;
				}
			}
			control = cb;
			return control;
		}

		public override object Clone(){
			return new SingleChoiceParamWf(Name, Help, Url, Visible, Value, Default, Values);
		}
	}
}