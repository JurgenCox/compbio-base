using System;
using System.Windows.Forms;
using BaseLibS.Param;

namespace BaseLib.Param{
	[Serializable]
	public class SaveFolderParamWf : SaveFolderParam{
		[NonSerialized] private readonly Button button;

		public SaveFolderParamWf(string labelText, string buttonText, Action<string> writeAction) : base(labelText,
			buttonText, writeAction){
			button = new Button{Text = buttonText};
			button.Click += (o, args) => {
				FolderBrowserDialog dialog = new FolderBrowserDialog{ShowNewFolderButton = true};
				if (dialog.ShowDialog() == DialogResult.OK){
					writeAction(dialog.SelectedPath);
				}
			};
		}

		protected SaveFolderParamWf(string name, string help, string url, bool visible, string value, string default1,
			Action<string> writeAction) : base(name, help, url, visible, value, default1, writeAction){ }

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

		public override object Clone(){
			return new SaveFolderParamWf(Name, Help, Url, Visible, Value, Default, WriteAction);
		}
	}
}