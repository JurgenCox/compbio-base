using System;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class EditTaxonomyForm : Form {
		public EditTaxonomyForm() {
			InitializeComponent();
			cancelButton.Click += (sender, args) => {
				DialogResult = DialogResult.Cancel;
				Close();
			};
			okButton.Click += (sender, args) => {
				DialogResult = DialogResult.OK;
				Close();
			};
			gtButton.Click += GtButton_OnClick;
			ltButton.Click += LtButton_OnClick;
		}
		public int Id {
			get {
				if (!Parser.TryInt(taxIdTextBox.Text, out int id)) {
					return -1;
				}
				return id;
			}
			set {
				taxIdTextBox.Text = "" + value;
				if (TaxonomyItems.taxId2Item.ContainsKey(value)) {
					string n = TaxonomyItems.taxId2Item[value].GetScientificName();
					taxNameTextBox.Text = n;
				}
			}
		}
		private void GtButton_OnClick(object sender, EventArgs e) {
			string stringId = taxIdTextBox.Text;
			if (!Parser.TryInt(stringId, out int id)) {
				MessageBox.Show(stringId + " is not a valid taxonomy ID.");
				return;
			}
			if (TaxonomyItems.taxId2Item.ContainsKey(id)) {
				string n = TaxonomyItems.taxId2Item[id].GetScientificName();
				taxNameTextBox.Text = n;
			} else {
				if (string.IsNullOrEmpty(stringId)) {
					MessageBox.Show("Please specify a taxonomy ID.");
				} else {
					MessageBox.Show(stringId + " is not a valid taxonomy ID.");
				}
			}
		}
		private void LtButton_OnClick(object sender, EventArgs e) {
			string name = taxNameTextBox.Text.ToLower();
			if (name.Length == 0) {
				MessageBox.Show("Please specify a name");
				return;
			}
			name = name.ToLower();
			string first = "" + name[0];
			name = first.ToUpper() + name.Substring(1);
			taxNameTextBox.Text = name;
			if (TaxonomyItems.name2Item.ContainsKey(name.ToLower())) {
				taxIdTextBox.Text = "" + TaxonomyItems.name2Item[name.ToLower()].TaxId;
			} else {
				MessageBox.Show(name + " is not a valid taxonomy name.");
			}
		}

	}
}
