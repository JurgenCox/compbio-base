using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class DictionaryIntValueControl : UserControl{
		public DictionaryIntValueControl(){
			InitializeComponent();
			button1.Click += EditButton_OnClick;
		}

		public Dictionary<string, int> Value { get; set; }
		public string[] Keys { get; set; }
		public int Default { get; set; }
		public void Connect(int connectionId, object target){}

		private void EditButton_OnClick(object sender, EventArgs e){
			DictionaryIntValueForm p = new DictionaryIntValueForm();
			p.SetData(Value, Keys, Default);
			if (p.ShowDialog() == DialogResult.OK){
				Value = p.GetData(Keys);
				textBox1.Text = StringVal;
			}
		}

		private string StringVal{
			get{
				List<string> result = new List<string>();
				foreach (KeyValuePair<string, int> pair in Value){
					result.Add("[" + pair.Key + "," + pair.Value + "]");
				}
				return StringUtils.Concat(",", result);
			}
		}
	}
}