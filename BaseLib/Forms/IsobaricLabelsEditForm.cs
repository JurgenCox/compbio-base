using System;
using System.Linq;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class IsobaricLabelsEditForm : Form {
		public IsobaricLabelsEditForm(string internalLabel, string terminalLabel, double correctionFactorM2,
			double correctionFactorM1, double correctionFactorP1, double correctionFactorP2, bool tmtLike) {
			InitializeComponent();
			cancelButton.Click += CancelButtonOnClick;
			okButton.Click += OkButtonOnClick;
			string[] internallabels = Tables.InternalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(internallabels);
			internalLabelComboBox.Items.Add("<None>");
			internalLabelComboBox.Items.AddRange(internallabels);
			int i1 = Array.BinarySearch(internallabels, internalLabel);
			if (i1 >= 0) {
				internalLabelComboBox.SelectedIndex = i1 + 1;
			} else {
				internalLabelComboBox.SelectedIndex = 0;
			}
			string[] terminalLabels = Tables.TerminalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(terminalLabels);
			terminalLabelComboBox.Items.Add("<None>");
			terminalLabelComboBox.Items.AddRange(terminalLabels);
			int i2 = Array.BinarySearch(terminalLabels, terminalLabel);
			if (i2 >= 0) {
				terminalLabelComboBox.SelectedIndex = i2 + 1;
			} else {
				terminalLabelComboBox.SelectedIndex = 0;
			}
			correctionFactorM2TextBox.Text = "" + correctionFactorM2;
			correctionFactorM1TextBox.Text = "" + correctionFactorM1;
			correctionFactorP1TextBox.Text = "" + correctionFactorP1;
			correctionFactorP2TextBox.Text = "" + correctionFactorP2;
			tmtLikeCheckBox.Checked = tmtLike;
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		internal string InternalLabel {
			get {
				if (internalLabelComboBox.SelectedIndex <= 0) {
					return "";
				}
				return internalLabelComboBox.SelectedItem.ToString();
			}
		}

		internal string TerminalLabel {
			get {
				if (terminalLabelComboBox.SelectedIndex <= 0) {
					return "";
				}
				return terminalLabelComboBox.SelectedItem.ToString();
			}
		}

		public double CorrectionFactorM2 {
			get {
				if (Parser.TryDouble(correctionFactorM2TextBox.Text, out double x)) {
					return x;
				}
				return 0;
			}
		}

		public double CorrectionFactorM1 {
			get {
				if (Parser.TryDouble(correctionFactorM1TextBox.Text, out double x)) {
					return x;
				}
				return 0;
			}
		}

		public double CorrectionFactorP1 {
			get {
				if (Parser.TryDouble(correctionFactorP1TextBox.Text, out double x)) {
					return x;
				}
				return 0;
			}
		}

		public double CorrectionFactorP2 {
			get {
				if (Parser.TryDouble(correctionFactorP2TextBox.Text, out double x)) {
					return x;
				}
				return 0;
			}
		}

		public bool TmtLike => tmtLikeCheckBox.Checked;
	}
}