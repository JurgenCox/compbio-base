using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class PatternMatchParamControl : UserControl{
		internal Regex regex;
		internal List<string> preview;
	    internal Func<string, Regex> Converter;

		public PatternMatchParamControl(Regex regex, List<string> preview, Func<string, Regex> converter){
			InitializeComponent();
			this.regex = regex;
		    Converter = converter;
			RegexTextBox.Text = Regex;
			this.preview = preview;
		}

		public string Regex{
            get => regex.ToString();
            set
            {
				if (value == Regex){
					return;
				}
				try{
					regex = Converter(value);
					UpdatePreviewTable();
				} catch (ArgumentException){
					Debug.WriteLine("Unable to parse regex");
				}
			}
		}

		private void UpdatePreviewTable(){
			DataTable table = new DataTable("Preview");
			string[] groupNames = regex.GetGroupNames().Skip(1).ToArray();
			const string inputColumn = "Input";
			table.Columns.Add(inputColumn);
			foreach (string groupName in groupNames){
				table.Columns.Add(groupName);
			}
			foreach (string s in preview){
				Match match = regex.Match(s);
				DataRow row = table.NewRow();
				row[inputColumn] = s;
				foreach (string groupName in groupNames){
					row[groupName] = match.Groups[groupName];
				}
				table.Rows.Add(row);
			}
			PreviewDataGridView.DataSource = table;
			Debug.WriteLine("update table");
		}

		private void RegexMatchParamControl_Load(object sender, EventArgs e){
			UpdatePreviewTable();
		}

		private void RegexTextBox_TextChanged(object sender, EventArgs e){
			Regex = RegexTextBox.Text;
		}

		private void PreviewDataGridView_SelectionChanged(object sender, EventArgs e){
			PreviewDataGridView.ClearSelection();
		}
	}
}