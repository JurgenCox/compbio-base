﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaseLibS.Param;
using BaseLibS.Util;

namespace BaseLib.Param{
	[Serializable]
	internal class MultiStringParamWf : MultiStringParam{
		[NonSerialized] private TextBox control;
		internal MultiStringParamWf(string name) : base(name){ }
		internal MultiStringParamWf(string name, string[] value) : base(name, value){ }

		protected MultiStringParamWf(string name, string help, string url, bool visible, string[] value,
			string[] default1) : base(name, help, url, visible, value, default1){ }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl(){
			if (control == null || control.IsDisposed){
				return;
			}
			string text = control.Text;
			string[] b = text.Split('\n');
			List<string> result = new List<string>();
			foreach (string x in b){
				string y = x.Trim();
				if (y.Length > 0){
					result.Add(y);
				}
			}
			Value = result.ToArray();
		}

		public override void UpdateControlFromValue(){
			if (control == null || control.IsDisposed){
				return;
			}
			control.Text = StringUtils.Concat("\r\n", Value);
		}

		public override object CreateControl(){
			return control = new TextBox{
				Text = StringUtils.Concat("\r\n", Value), AcceptsReturn = true, Multiline = true
			};
		}

		public override object Clone(){
			return new MultiStringParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}