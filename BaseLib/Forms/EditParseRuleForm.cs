using System.Windows.Forms;
using BaseLibS.Table;

namespace BaseLib.Forms {
	public sealed partial class EditParseRuleForm : Form {
		private DataTable2 table;
		public EditParseRuleForm(string name, string value, string[] parseRules,string[] descriptions) {
			InitializeComponent();
			tableView1.TableModel = CreateTable(parseRules, descriptions);
			textBox1.Text = value;
			Text = "Edit " + name + " rule";
			tableView1.SelectionChanged += (sender, args) => {
				textBox1.Text = (string)table.GetEntry(tableView1.GetSelectedRow(), 0);
			};
			cancelButton.Click += (sender, args) => {
				DialogResult = DialogResult.Cancel;
				Close();
			};
			okButton.Click += (sender, args) => {
				DialogResult = DialogResult.OK;
				Close();
			};
		}

		public string ParseRule => textBox1.Text;

		private ITableModel CreateTable(string[] parseRules, string[] descriptions) {
			table = new DataTable2("parse rules");
			table.AddColumn("Parse rule", 90, ColumnType.Text);
			table.AddColumn("Description", 170, ColumnType.Text);
			for (int i = 0; i < parseRules.Length; i++) {
				DataRow2 row = table.NewRow();
				row[0] = parseRules[i];
				row[1] = descriptions[i];
				table.AddRow(row);
			}
			return table;
		}
	}
}
