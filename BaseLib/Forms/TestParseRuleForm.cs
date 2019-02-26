using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BaseLibS.Parse.Misc;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class TestParseRulesForm : Form {
		private readonly string filePath;
		private readonly string identifierParseRule;
		private readonly string descriptionParseRule;
		private readonly string taxonomyParseRule;
		private readonly string variationParseRule;
		private readonly string modificationParseRule;
		private readonly DataTable2 tableModel;

		public TestParseRulesForm(string filePath, string identifierParseRule, string descriptionParseRule,
			string taxonomyParseRule, string variationParseRule, string modificationParseRule) {
			InitializeComponent();
			this.filePath = filePath;
			this.identifierParseRule = identifierParseRule;
			this.descriptionParseRule = descriptionParseRule;
			this.taxonomyParseRule = taxonomyParseRule;
			this.variationParseRule = variationParseRule;
			this.modificationParseRule = modificationParseRule;
			tableModel = CreateTable(!string.IsNullOrEmpty(variationParseRule), !string.IsNullOrEmpty(modificationParseRule));
			mainTable.TableModel = tableModel;
			testButton.Click += TestButton_OnClick;
			TestButton_OnClick(null, null);
		}

		private void TestButton_OnClick(object sender, EventArgs e) {
			int minEntry;
			if (!Parser.TryInt(minEntryTextBox.Text, out minEntry)) {
				minEntry = 1;
				minEntryTextBox.Text = "1";
			}
			int maxEntry;
			if (!Parser.TryInt(maxEntryTextBox.Text, out maxEntry)) {
				maxEntry = 100;
				maxEntryTextBox.Text = "100";
			}
			minEntry = Math.Max(0, minEntry);
			maxEntry = Math.Max(0, maxEntry);
			maxEntry = Math.Max(minEntry, maxEntry);
			minEntry--;
			if (string.IsNullOrEmpty(filePath)) {
				MessageBox.Show("Please specify a fasta File");
				return;
			}
			if (!File.Exists(filePath)) {
				MessageBox.Show("The File " + filePath + " does not exist.");
				return;
			}
			TestFile(minEntry, maxEntry);
		}

		private static DataTable2 CreateTable(bool hasVariation, bool hasModification) {
			DataTable2 t = new DataTable2("databases");
			t.AddColumn("Header", 250, ColumnType.Text, "");
			t.AddColumn("Sequence", 100, ColumnType.Text, "");
			t.AddColumn("Identifier", 100, ColumnType.Text, "");
			t.AddColumn("Description", 100, ColumnType.Text, "");
			t.AddColumn("Taxonomy ID", 100, ColumnType.Text, "");
			if (hasVariation) {
				t.AddColumn("Variation", 100, ColumnType.Text, "");
			}
			if (hasModification) {
				t.AddColumn("Modification", 100, ColumnType.Text, "");
			}
			return t;
		}

		private void TestFile(int minEntry, int maxEntry) {
			Regex nameRegex = !string.IsNullOrEmpty(identifierParseRule) ? new Regex(identifierParseRule) : null;
			Regex descriptionRegex = !string.IsNullOrEmpty(descriptionParseRule) ? new Regex(descriptionParseRule) : null;
			Regex taxonomyRegex = !string.IsNullOrEmpty(taxonomyParseRule) ? new Regex(taxonomyParseRule) : null;
			Regex variationRegex = !string.IsNullOrEmpty(variationParseRule) ? new Regex(variationParseRule) : null;
			Regex modificationRegex = !string.IsNullOrEmpty(modificationParseRule) ? new Regex(modificationParseRule) : null;
			string[] headers;
			string[] sequences;
			GetDataFromFile(filePath, minEntry, maxEntry, out headers, out sequences);
			tableModel.Clear();
			for (int i = 0; i < headers.Length; i++) {
				DataRow2 r = tableModel.NewRow();
				string header = headers[i];
				r["Sequence"] = sequences[i];
				r["Header"] = header;
				if (nameRegex != null) {
					r["Identifier"] = nameRegex.Match(header).Groups[1].ToString();
				}
				if (descriptionRegex != null) {
					r["Description"] = descriptionRegex.Match(header).Groups[1].ToString();
				}
				if (taxonomyRegex != null) {
					r["Taxonomy ID"] = taxonomyRegex.Match(header).Groups[1].ToString();
				}
				if (variationRegex != null) {
					r["Variation"] = variationRegex.Match(header).Groups[1].ToString();
				}
				if (modificationRegex != null) {
					r["Modification"] = modificationRegex.Match(header).Groups[1].ToString();
				}
				tableModel.AddRow(r);
			}
			mainTable.Invalidate(true);
		}

		private static void GetDataFromFile(string filePath, int minEntry, int maxEntry, out string[] headers,
			out string[] sequences) {
			List<string> headers1 = new List<string>();
			List<string> sequences1 = new List<string>();
			int count = 0;
			FastaParser fp = new FastaParser(filePath, (header, sequence) => {
				if (count < minEntry) {
					count++;
					return false;
				}
				if (count >= maxEntry) {
					return true;
				}
				headers1.Add(">" + header);
				sequences1.Add(sequence);
				count++;
				return false;
			});
			fp.Parse();
			headers = headers1.ToArray();
			sequences = sequences1.ToArray();
		}
	}
}