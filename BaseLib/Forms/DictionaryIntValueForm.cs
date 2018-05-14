using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLib.Param;
using BaseLibS.Param;

namespace BaseLib.Forms{
	public partial class DictionaryIntValueForm : Form{
		public DictionaryIntValueForm(){
			InitializeComponent();
			cancelButton.Click += CancelButton_OnClick;
			okButton.Click += OkButton_OnClick;
		}

		internal void SetData(Dictionary<string, int> v, string[] keys, int d){
			Parameter[] p = new Parameter[keys.Length];
			for (int i = 0; i < p.Length; i++){
				p[i] = new IntParamWf(keys[i], v.ContainsKey(keys[i]) ? v[keys[i]] : d);
			}
			parameterPanel1.Init(new Parameters(p));
		}

		internal Dictionary<string, int> GetData(string[] keys){
			Dictionary<string, int> result = new Dictionary<string, int>();
			foreach (string key in keys){
				int y = parameterPanel1.Parameters.GetParam<int>(key).Value;
				result.Add(key, y);
			}
			return result;
		}

		private void CancelButton_OnClick(object sender, EventArgs e){
			Close();
		}

		private void OkButton_OnClick(object sender, EventArgs e){
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}