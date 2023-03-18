using System;
using System.Linq;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Util;
namespace BaseLib.Forms{
	public partial class IsobaricLabelsComplexEditForm : Form{
		public IsobaricLabelsComplexEditForm(IsobaricLabelInfoComplex info){
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
			CorrectionFactorM2X13C = info.correctionFactorM2X13C;
			CorrectionFactorM13C15N = info.correctionFactorM13C15N;
			CorrectionFactorM13C = info.correctionFactorM13C;
			CorrectionFactorM15N = info.correctionFactorM15N;
			CorrectionFactorP15N = info.correctionFactorP15N;
			CorrectionFactorP13C = info.correctionFactorP13C;
			CorrectionFactorP15N13C = info.correctionFactorP15N13C;
			CorrectionFactorP2X13C = info.correctionFactorP2X13C;
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
		internal IsobaricLabelInfoComplex Info =>
			new IsobaricLabelInfoComplex(InternalLabel, TerminalLabel, CorrectionFactorM2X13C, 
				CorrectionFactorM13C15N, CorrectionFactorM13C, CorrectionFactorM15N, CorrectionFactorP15N, 
				CorrectionFactorP13C, CorrectionFactorP15N13C, CorrectionFactorP2X13C, TmtLike);
		private string InternalLabel =>
			internalLabelComboBox.SelectedIndex <= 0 ? "" : internalLabelComboBox.SelectedItem.ToString();
		private string TerminalLabel =>
			terminalLabelComboBox.SelectedIndex <= 0 ? "" : terminalLabelComboBox.SelectedItem.ToString();
		private double CorrectionFactorM2X13C{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlM2X13C.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlM2X13C.Text = Parser.ToString(value);
		}
		private double CorrectionFactorM13C15N{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlM13C15N.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlM13C15N.Text = Parser.ToString(value);
		}
		private double CorrectionFactorM13C{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlM13C.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlM13C.Text = Parser.ToString(value);
		}
		private double CorrectionFactorM15N{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlM15N.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlM15N.Text = Parser.ToString(value);
		}
		private double CorrectionFactorP15N{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlP15N.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP15N.Text = Parser.ToString(value);
		}
		private double CorrectionFactorP13C{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlP13C.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP13C.Text = Parser.ToString(value);
		}
		private double CorrectionFactorP15N13C{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlP15N13C.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP15N13C.Text = Parser.ToString(value);
		}
		private double CorrectionFactorP2X13C{
			get{
				bool ok = Parser.TryDouble(correctionFactorControlP2X13C.Text, out double val);
				return ok ? val : 0;
			}
			set => correctionFactorControlP2X13C.Text = Parser.ToString(value);
		}
		public bool TmtLike => tmtLikeCheckBox.Checked;
	}
}