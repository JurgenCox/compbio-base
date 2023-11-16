using System;
using System.Collections.Generic;
using BaseLib.Forms;
using BaseLibS.Param;
using BaseLibS.Util;
namespace BaseLib.Param{
	/// <summary>
	/// Parameter value format: 8 item string array
	/// [0] file name: string
	/// [1] column names: string
	/// [2-6] main, num, cat, text, multi-numeric: integers separated by ';'
	/// [7] shorten column names: bool
	/// </summary>
	[Serializable]
	public class PerseusLoadMatrixParamWf : PerseusLoadMatrixParam{
		[NonSerialized] private PerseusLoadMatrixControl control;
		/// <summary>
		/// for xml serialization only
		/// </summary>
		private PerseusLoadMatrixParamWf() : this(""){
		}
		public PerseusLoadMatrixParamWf(string name) : base(name){
		}
		protected PerseusLoadMatrixParamWf(string name, string help, string url, bool visible, string[] value,
			string[] default1, string filter) : base(name, help, url, visible, value, default1, filter){
		}
		public override ParamType Type => ParamType.WinForms;
		public override string StringValue{
			get => StringUtils.Concat("\n", Value);
			set => Value = value.Split('\n');
		}
		public override bool IsDropTarget => true;
		public override void Drop(string x){
			UpdateFile(x);
		}
		public override void SetValueFromControl(){
			Value = control.Value;
			FilterParameterValues = control.GetSubParameterValues();
		}
		public override void UpdateControlFromValue(){
			control.Value = Value;
		}
		private void UpdateFile(string filename){
			control?.UpdateFile(filename);
		}
		public override object CreateControl(){
			string[] items = Value[1].Length > 0 ? Value[1].Split(';') : new string[0];
			control = new PerseusLoadMatrixControl(items){Filter = Filter, Value = Value};
			return control;
		}
		public override object Clone(){
			PerseusLoadMatrixParamWf result =
				new PerseusLoadMatrixParamWf(Name, Help, Url, Visible, Value, Default, Filter){
					FilterParameterValues = new List<Parameters[]>()
				};
			foreach (Parameters[] value in FilterParameterValues){
				if (value == null){
					result.FilterParameterValues.Add(null);
				} else{
					Parameters[] x = new Parameters[value.Length];
					for (int i = 0; i < x.Length; i++){
						x[i] = (Parameters) value[i].Clone();
					}
					result.FilterParameterValues.Add(x);
				}
			}
			return result;
		}
	}
}