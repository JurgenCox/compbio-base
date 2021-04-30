using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class SaveFileParamWf : SaveFileParam{
		[NonSerialized] private readonly Button button;

		public SaveFileParamWf(string labelText, string buttonText, string fileName, string filter,
			Action<string> writeAction) : base(labelText, buttonText, fileName, filter, writeAction){
			button = new Button{Text = buttonText};
			button.Click += (o, args) => {
				SaveFileDialog dialog = new SaveFileDialog{FileName = fileName, Filter = filter};
				if (dialog.ShowDialog() == DialogResult.OK){
					writeAction(dialog.FileName);
				}
			};
		}

		public override object CreateControl(){
			return button;
		}

		public override void SetValueFromControl(){
			if (button == null || button.IsDisposed){
				return;
			}
			Value = button.Text;
		}

		public override void UpdateControlFromValue(){
			if (button == null || button.IsDisposed){
				return;
			}
			button.Text = Value;
		}

		public override ParamType Type => ParamType.WinForms;
	}
}