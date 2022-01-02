using System;
using BaseLibS.Graph;
namespace BaseLibS.Param {
	[Serializable]
	public class LabelParamWf : LabelParam {
		[NonSerialized] protected LabelModel labelModel;
		public LabelParamWf(string name) : this(name, "") { }

		public LabelParamWf(string name, string value) : base(name, value) {
		}

		protected LabelParamWf(string name, string help, string url, bool visible, string value, string default1) :
			base(name, help, url, visible, value, default1) { }

		public override ParamType Type => ParamType.WinForms;

		public override void SetValueFromControl() {
			if (labelModel == null) {
				return;
			}
			Value = labelModel.Text;
		}

		public override void UpdateControlFromValue() {
			if (labelModel == null) {
				return;
			}
			labelModel.Text = Value;
		}

		public override object CreateControl() {
			return labelModel = new LabelModel { Text = Value };
		}

		public override object Clone() {
			return new LabelParamWf(Name, Help, Url, Visible, Value, Default);
		}
	}
}