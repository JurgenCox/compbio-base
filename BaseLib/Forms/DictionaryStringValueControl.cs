using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Query;
using BaseLibS.Table;

namespace BaseLib.Forms{
	public partial class DictionaryStringValueControl : UserControl{
		private readonly DataTable2 table;

		public DictionaryStringValueControl(){
			InitializeComponent();
			table = CreateTable();
			tableView1.TableModel = table;
			tableView1.Sortable = false;
			toolStripButton1.Click += (sender, args) => {
				int[] sel = tableView1.GetSelectedRows();
				if (sel.Length == 0){
					MessageBox.Show("Please select some rows.");
					return;
				}
				string d = (string) table.GetEntry(sel[0], 0);
				StringQueryForm f = new StringQueryForm(d);
				Point p = tableView1.PointToScreen(new Point(0, 0));
				f.Top = p.Y + 27;
				f.Left = p.X;
				DialogResult result = f.ShowDialog(this);
				if (result != DialogResult.OK){
					return;
				}
				string s = f.Value;
				if (string.IsNullOrEmpty(s)){
					return;
				}
				foreach (int i in sel){
					table.SetEntry(i, 1, s);
				}
				tableView1.Invalidate(true);
			};
		}

		public string KeyName{ get; set; } = "Keys";
		public string ValueName{ get; set; } = "Values";

		public Dictionary<string, string> Value{
			get{
				Dictionary<string, string> map = new Dictionary<string, string>();
				for (int i = 0; i < table.RowCount; i++){
					DataRow2 row = table.GetRow(i);
					map.Add((string) row[KeyName], (string) row[ValueName]);
				}
				return map;
			}
			set{
				table.Clear();
				foreach (KeyValuePair<string, string> pair in value){
					DataRow2 row = table.NewRow();
					row[KeyName] = pair.Key;
					row[ValueName] = pair.Value;
					table.AddRow(row);
				}
			}
		}

		private DataTable2 CreateTable(){
			DataTable2 table1 = new DataTable2("key value table");
			table1.AddColumn(KeyName, 250, ColumnType.Text, "");
			table1.AddColumn(ValueName, 250, ColumnType.Text);
			return table1;
		}
	}
}