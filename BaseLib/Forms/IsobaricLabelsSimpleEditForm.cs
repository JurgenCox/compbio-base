﻿using System;
using System.Linq;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class IsobaricLabelsSimpleEditForm : Form{
		public IsobaricLabelsSimpleEditForm(IsobaricLabelInfoSimple info){
			InitializeComponent();
			cancelButton.Click += CancelButtonOnClick;
			okButton.Click += OkButtonOnClick;
			string[] internallabels = Tables.InternalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(internallabels);
			internalLabelComboBox.Items.Add("<None>");
			internalLabelComboBox.Items.AddRange(internallabels);
			int i1 = Array.BinarySearch(internallabels, info.internalLabel);
			if (i1 >= 0){
				internalLabelComboBox.SelectedIndex = i1 + 1;
			} else{
				internalLabelComboBox.SelectedIndex = 0;
			}
			string[] terminalLabels = Tables.TerminalIsobaricLabelModifications.Keys.ToArray();
			Array.Sort(terminalLabels);
			terminalLabelComboBox.Items.Add("<None>");
			terminalLabelComboBox.Items.AddRange(terminalLabels);
			int i2 = Array.BinarySearch(terminalLabels, info.terminalLabel);
			if (i2 >= 0){
				terminalLabelComboBox.SelectedIndex = i2 + 1;
			} else{
				terminalLabelComboBox.SelectedIndex = 0;
			}
			CorrectionFactorM2 = info.correctionFactorM2;
			CorrectionFactorM1 = info.correctionFactorM1;
			CorrectionFactorP1 = info.correctionFactorP1;
			CorrectionFactorP2 = info.correctionFactorP2;
			tmtLikeCheckBox.Checked = info.tmtLike;
		}

		private void OkButtonOnClick(object sender, EventArgs eventArgs){
			DialogResult = DialogResult.OK;
			Close();
		}

		private void CancelButtonOnClick(object sender, EventArgs eventArgs){
			DialogResult = DialogResult.Cancel;
			Close();
		}

		internal IsobaricLabelInfoSimple Info =>
			new IsobaricLabelInfoSimple(InternalLabel, TerminalLabel, CorrectionFactorM2, CorrectionFactorM1,
				CorrectionFactorP1, CorrectionFactorP2, TmtLike);

		private string InternalLabel =>
			internalLabelComboBox.SelectedIndex <= 0 ? "" : internalLabelComboBox.SelectedItem.ToString();

		private string TerminalLabel =>
			terminalLabelComboBox.SelectedIndex <= 0 ? "" : terminalLabelComboBox.SelectedItem.ToString();

		private double CorrectionFactorM2 {
			get{
				bool ok = Parser.TryDouble(correctionFactorControlM2.Text, out double val);
				return ok ? val : 0;
			} 
			set => correctionFactorControlM2.Text = Parser.ToString(value);
		}

		private double CorrectionFactorM1 {
			get {
				bool ok = Parser.TryDouble(correctionFactorControlM1.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlM1.Text = Parser.ToString(value);
		}

		private double CorrectionFactorP1 {
			get {
				bool ok = Parser.TryDouble(correctionFactorControlP1.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP1.Text = Parser.ToString(value);
		}

		private double CorrectionFactorP2 {
			get {
				bool ok = Parser.TryDouble(correctionFactorControlP2.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP2.Text = Parser.ToString(value);
		}

		public bool TmtLike => tmtLikeCheckBox.Checked;
	}
}