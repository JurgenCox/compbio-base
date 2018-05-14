using System;
using System.Windows.Forms;

namespace BaseLib.Forms.Table{
	public partial class PasteSelectionForm : Form{
		private readonly int ncols;
		public bool Ok { get; set; }
		private readonly ComboBox[] cbs;

		public PasteSelectionForm(int ncols, string[] columnNames){
			InitializeComponent();
			cancelButton.Click += CancelButton_OnClick;
			okButton.Click += OkButton_OnClick;
			this.ncols = ncols;
			TableLayoutPanel g = new TableLayoutPanel();
			for (int i = 0; i < ncols; i++){
				g.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
			}
			g.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			g.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
			g.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
			if (ncols == 1){
				Label tb = new Label{Text = "Column"};
				g.Controls.Add(tb, 0, 0);
			} else{
				for (int i = 0; i < ncols; i++){
					Label tb = new Label{Text = "Column " + (i + 1)};
					g.Controls.Add(tb, 0, i);
				}
			}
			cbs = new ComboBox[ncols];
			for (int i = 0; i < ncols; i++){
				ComboBox cb = new ComboBox();
				cbs[i] = cb;
				foreach (string t in columnNames){
					cb.Items.Add(t);
				}
				g.Controls.Add(cb, 1, i);
				cb.SelectedIndex = Math.Min(columnNames.Length - 1, i);
			}
			panel1.Controls.Add(g);
		}

		private void CancelButton_OnClick(object sender, EventArgs e){
			Ok = false;
			Close();
		}

		private void OkButton_OnClick(object sender, EventArgs e){
			Ok = true;
			Close();
		}

		public int[] GetSelectedIndices(){
			int[] result = new int[ncols];
			for (int i = 0; i < result.Length; i++){
				result[i] = cbs[i].SelectedIndex;
			}
			return result;
		}
	}
}