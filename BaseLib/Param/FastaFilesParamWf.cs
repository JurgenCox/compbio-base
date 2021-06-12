using System;
using BaseLib.Forms;
using BaseLibS.Mol;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class FastaFilesParamWf : FastaFilesParam{
		[NonSerialized] private FastaFilesParamControl control;

		internal FastaFilesParamWf(string name) : base(name){ }
		internal FastaFilesParamWf(string name, string[][] value) : base(name, value){ }

		protected FastaFilesParamWf(string name, string help, string url, bool visible, string[][] value,
			string[][] default1, bool hasVariationData, bool hasModifications) : base(name, help, url, visible, value,
			default1, hasVariationData, hasModifications){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			Value = control.Value;
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Value = Value;
		}

		public override object CreateControl(){
			return control =
				new FastaFilesParamControl(HasVariationData, HasModifications, TaxonomyItems.GetTaxonomyItems);
		}

		public override object Clone(){
			return new FastaFilesParamWf(Name, Help, Url, Visible, Value, Default, HasVariationData, HasModifications);
		}
	}
}