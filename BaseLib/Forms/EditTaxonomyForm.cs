using System.Windows.Forms;

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
		}
	}
}
