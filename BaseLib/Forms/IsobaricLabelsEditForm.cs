using System;
using System.Linq;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class IsobaricLabelsEditForm : Form {
		public IsobaricLabelsEditForm(IsobaricLabelInfo info) {
			InitializeComponent();
			cancelButton.Click += CancelButtonOnClick;
			okButton.Click += OkButtonOnClick;
			string[] internallabels = Tables.InternalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(internallabels);
			internalLabelComboBox.Items.Add("<None>");
			internalLabelComboBox.Items.AddRange(internallabels);
			int i1 = Array.BinarySearch(internallabels, info.internalLabel);
			if (i1 >= 0) {
				internalLabelComboBox.SelectedIndex = i1 + 1;
			} else {
				internalLabelComboBox.SelectedIndex = 0;
			}
			string[] terminalLabels = Tables.TerminalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(terminalLabels);
			terminalLabelComboBox.Items.Add("<None>");
			terminalLabelComboBox.Items.AddRange(terminalLabels);
			int i2 = Array.BinarySearch(terminalLabels, info.terminalLabel);
			if (i2 >= 0) {
				terminalLabelComboBox.SelectedIndex = i2 + 1;
			} else {
				terminalLabelComboBox.SelectedIndex = 0;
			}
			correctionFactorM2TextBox.Text = "" + info.correctionFactorM2;
			correctionFactorM1TextBox.Text = "" + info.correctionFactorM1;
			correctionFactorP1TextBox.Text = "" + info.correctionFactorP1;
			correctionFactorP2TextBox.Text = "" + info.correctionFactorP2;
			tmtLikeCheckBox.Checked = info.tmtLike;
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButtonOnClick(object sender, EventArgs eventArgs) {
			DialogResult = DialogResult.Cancel;
			Close();
		}

		internal IsobaricLabelInfo Info => new IsobaricLabelInfo(InternalLabel, TerminalLabel, CorrectionFactorM2,
			CorrectionFactorM1, CorrectionFactorP1, CorrectionFactorP2, TmtLike);

		private string InternalLabel => internalLabelComboBox.SelectedIndex <= 0
			? ""
			: internalLabelComboBox.SelectedItem.ToString();

		private string TerminalLabel => terminalLabelComboBox.SelectedIndex <= 0
			? ""
			: terminalLabelComboBox.SelectedItem.ToString();

		private double CorrectionFactorM2 => Parser.TryDouble(correctionFactorM2TextBox.Text, out double x) ? x : 0;
		private double CorrectionFactorM1 => Parser.TryDouble(correctionFactorM1TextBox.Text, out double x) ? x : 0;
		private double CorrectionFactorP1 => Parser.TryDouble(correctionFactorP1TextBox.Text, out double x) ? x : 0;
		private double CorrectionFactorP2 => Parser.TryDouble(correctionFactorP2TextBox.Text, out double x) ? x : 0;
		public bool TmtLike => tmtLikeCheckBox.Checked;
	}
}