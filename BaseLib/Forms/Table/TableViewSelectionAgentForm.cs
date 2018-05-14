using System;
using System.Windows.Forms;
using BaseLibS.Table;

namespace BaseLib.Forms.Table{
	public partial class TableViewSelectionAgentForm : Form{
		public TableViewSelectionAgentForm(ITableModel tableModel){
			InitializeComponent();
			cancelButton.Click += CancelButton_OnClick;
			okButton.Click += OkButton_OnClick;
			if (tableModel == null){
				return;
			}
			foreach (ITableSelectionAgent agent in TableView.selectionAgents){
				sourceBox.Items.Add(agent.Title);
			}
			for (int i = 0; i < tableModel.ColumnCount; i++){
				columnBox.Items.Add(tableModel.GetColumnName(i));
			}
		}

		private void CancelButton_OnClick(object sender, EventArgs e){
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OkButton_OnClick(object sender, EventArgs e){
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}