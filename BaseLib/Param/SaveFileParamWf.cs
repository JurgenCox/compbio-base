using System;
using System.Windows.Forms;
using BaseLibS.Graph;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class SaveFileParamWf : SaveFileParam{
		[NonSerialized] private readonly ButtonModel button;

		public SaveFileParamWf(string labelText, string buttonText, string fileName, string filter,
			Action<string> writeAction) : base(labelText, buttonText, fileName, filter, writeAction){
			button = new ButtonModel{Text = buttonText};
			button.Click += (o, args) => {
				SaveFileDialog dialog = new SaveFileDialog{FileName = fileName, Filter = filter};
				if (dialog.ShowDialog() == DialogResult.OK){
					writeAction(dialog.FileName);
				}
			};
		}

		protected SaveFileParamWf(string name, string help, string url, bool visible, string value, string default1,
			string fileName, string filter, Action<string> writeAction) : base(name, help, url, visible, value,
			default1, fileName, filter, writeAction){ }

		public override object CreateControl(){
			return button;
		}

		public override void SetValueFromControl(){
			if (button == null){
				return;
			}
			Value = button.Text;
		}

		public override void UpdateControlFromValue(){
			if (button == null){
				return;
			}
			button.Text = Value;
		}

		public override ParamType Type => ParamType.WinForms;

		public override object Clone(){
			return new SaveFileParamWf(Name, Help, Url, Visible, Value, Default, FileName, Filter, WriteAction);
		}
	}
}