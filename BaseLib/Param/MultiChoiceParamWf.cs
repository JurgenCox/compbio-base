using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	internal class MultiChoiceParamWf : MultiChoiceParam{
		[NonSerialized] private ListSelectorControl control;
		internal MultiChoiceParamWf(string name) : base(name){ }
		internal MultiChoiceParamWf(string name, int[] value) : base(name, value){ }

		protected MultiChoiceParamWf(string name, string help, string url, bool visible, int[] value, int[] default1,
			bool repeats, IList<string> values, List<string> defaultSelectionNames, List<string[]> defaultSelections) :
			base(name, help, url, visible, value, default1, repeats, values, defaultSelectionNames, defaultSelections){
			Repeats = repeats;
			Values = values;
			DefaultSelectionNames = defaultSelectionNames;
			DefaultSelections = defaultSelections;
		}

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.SelectedIndices;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.SelectedIndices = Value;
		}

		public override object CreateControl(){
			control = new ListSelectorControl{HasMoveButtons = true};
			foreach (string value in Values){
				control.Items.Add(value);
			}
			control.Repeats = Repeats;
			control.SelectedIndices = Value;
			control.SetDefaultSelectors(DefaultSelectionNames, DefaultSelections);
			return control;
		}

		public override object Clone(){
			return new MultiChoiceParamWf(Name, Help, Url, Visible, Value, Default, Repeats, Values,
				DefaultSelectionNames, DefaultSelections);
		}
	}
}