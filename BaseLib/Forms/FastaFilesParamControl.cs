using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BaseLibS.Mol;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms {
	public partial class FastaFilesParamControl : UserControl {
		private DataTable2 table;
		private readonly bool hasVariationData;
		private readonly bool hasModifications;
		private TableLayoutPanel tableLayoutPanel2;
		private Button addButton;
		private Button removeButton;
		private Button identifierRuleButton;
		private Button descriptionRuleButton;
		private Button taxonomyRuleButton;
		private Button testButton;
		private Button taxonomyIdButton;
		private Button variationRuleButton;
		private Button modificationRuleButton;

		public FastaFilesParamControl(bool hasVariationData, bool hasModifications) {
			this.hasVariationData = hasVariationData;
			this.hasModifications = hasModifications;
			InitializeComponent();
			InitializeComponent2();
			tableView1.TableModel = CreateTable();
			addButton.Click += AddButton_OnClick;
			removeButton.Click += RemoveButton_OnClick;
			identifierRuleButton.Click += (sender, args) => {
				ParseRuleButtonClick("Identifier",
					new[] {@">.*\|(.*)\|", @">(gi\|[0-9]*)", @">IPI:([^\| .]*)", @">(.*)", @">([^ ]*)", @">([^\t]*)"},
					new[] {
						"Uniprot identifier", "NCBI accession", "IPI accession", "Everything after “>”", "Up to first space",
						"Up to first tab character"
					});
			};
			descriptionRuleButton.Click += (sender, args) => {
				ParseRuleButtonClick("Description", new string[0], new string[0]);
			};
			taxonomyRuleButton.Click += (sender, args) => { ParseRuleButtonClick("Taxonomy", new string[0], new string[0]); };
			taxonomyIdButton.Click += TaxonomyIdButtonOnClick;
			testButton.Click += TestButtonOnClick;
			if (hasVariationData) {
				variationRuleButton.Click += (sender, args) => { ParseRuleButtonClick("Variation", new string[0], new string[0]); };
			}
			if (hasModifications) {
				modificationRuleButton.Click += (sender, args) => {
					ParseRuleButtonClick("Modification", new string[0], new string[0]);
				};
			}
		}

		private void ParseRuleButtonClick(string ruleName, string[] rules, string[] descriptions) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0) {
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			int colInd = table.GetColumnIndex(ruleName + " rule");
			EditParseRuleForm f = new EditParseRuleForm(ruleName.ToLower(), GetMostFrequentValue(colInd), rules, descriptions);
			f.ShowDialog();
			if (f.DialogResult == DialogResult.OK) {
				foreach (int i in sel) {
					table.SetEntry(i, colInd, f.ParseRule);
				}
				tableView1.Invalidate(true);
			}
		}

		private void InitializeComponent2() {
			tableLayoutPanel2 = new TableLayoutPanel();
			addButton = new Button();
			removeButton = new Button();
			identifierRuleButton = new Button();
			descriptionRuleButton = new Button();
			taxonomyRuleButton = new Button();
			testButton = new Button();
			taxonomyIdButton = new Button();
			if (hasVariationData) {
				variationRuleButton = new Button();
			}
			if (hasModifications) {
				modificationRuleButton = new Button();
			}
			tableLayoutPanel2.SuspendLayout();
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
			// 
			// tableLayoutPanel2
			// 
			int nbuttons = 7;
			if (hasVariationData) {
				nbuttons++;
			}
			if (hasModifications) {
				nbuttons++;
			}
			tableLayoutPanel2.ColumnCount = 2 * nbuttons;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
			for (int i = 0; i < nbuttons - 1; i++) {
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
				tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
			}
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Controls.Add(addButton, 0, 0);
			tableLayoutPanel2.Controls.Add(removeButton, 2, 0);
			tableLayoutPanel2.Controls.Add(identifierRuleButton, 4, 0);
			tableLayoutPanel2.Controls.Add(descriptionRuleButton, 6, 0);
			tableLayoutPanel2.Controls.Add(taxonomyRuleButton, 8, 0);
			tableLayoutPanel2.Controls.Add(taxonomyIdButton, 10, 0);
			tableLayoutPanel2.Controls.Add(testButton, 12, 0);
			if (hasVariationData) {
				tableLayoutPanel2.Controls.Add(variationRuleButton, 14, 0);
			}
			if (hasModifications) {
				tableLayoutPanel2.Controls.Add(variationRuleButton, hasVariationData ? 16 : 14, 0);
			}
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel2.Margin = new Padding(0);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Size = new System.Drawing.Size(2135, 50);
			tableLayoutPanel2.TabIndex = 2;
			// 
			// addButton
			// 
			addButton.Dock = DockStyle.Fill;
			addButton.Location = new System.Drawing.Point(0, 0);
			addButton.Margin = new Padding(0);
			addButton.Name = "addButton";
			addButton.Size = new System.Drawing.Size(220, 50);
			addButton.TabIndex = 0;
			addButton.Text = "Add";
			addButton.UseVisualStyleBackColor = true;
			// 
			// removeButton
			// 
			removeButton.Dock = DockStyle.Fill;
			removeButton.Location = new System.Drawing.Point(230, 0);
			removeButton.Margin = new Padding(0);
			removeButton.Name = "removeButton";
			removeButton.Size = new System.Drawing.Size(220, 50);
			removeButton.TabIndex = 1;
			removeButton.Text = "Remove";
			removeButton.UseVisualStyleBackColor = true;
			// 
			// identifierRuleButton
			// 
			identifierRuleButton.Dock = DockStyle.Fill;
			identifierRuleButton.Location = new System.Drawing.Point(460, 0);
			identifierRuleButton.Margin = new Padding(0);
			identifierRuleButton.Name = "identifierRuleButton";
			identifierRuleButton.Size = new System.Drawing.Size(220, 50);
			identifierRuleButton.TabIndex = 2;
			identifierRuleButton.Text = "Identifier rule";
			identifierRuleButton.UseVisualStyleBackColor = true;
			// 
			// descriptionRuleButton
			// 
			descriptionRuleButton.Dock = DockStyle.Fill;
			descriptionRuleButton.Location = new System.Drawing.Point(690, 0);
			descriptionRuleButton.Margin = new Padding(0);
			descriptionRuleButton.Name = "descriptionRuleButton";
			descriptionRuleButton.Size = new System.Drawing.Size(220, 50);
			descriptionRuleButton.TabIndex = 3;
			descriptionRuleButton.Text = "Description rule";
			descriptionRuleButton.UseVisualStyleBackColor = true;
			// 
			// taxonomyRuleButton
			// 
			taxonomyRuleButton.Dock = DockStyle.Fill;
			taxonomyRuleButton.Location = new System.Drawing.Point(920, 0);
			taxonomyRuleButton.Margin = new Padding(0);
			taxonomyRuleButton.Name = "taxonomyRuleButton";
			taxonomyRuleButton.Size = new System.Drawing.Size(220, 50);
			taxonomyRuleButton.TabIndex = 4;
			taxonomyRuleButton.Text = "Taxonomy rule";
			taxonomyRuleButton.UseVisualStyleBackColor = true;
			// 
			// testButton
			// 
			testButton.Dock = DockStyle.Fill;
			testButton.Location = new System.Drawing.Point(1380, 0);
			testButton.Margin = new Padding(0);
			testButton.Name = "testButton";
			testButton.Size = new System.Drawing.Size(220, 50);
			testButton.TabIndex = 5;
			testButton.Text = "Test";
			testButton.UseVisualStyleBackColor = true;
			// 
			// taxonomyIdButton
			// 
			taxonomyIdButton.Dock = DockStyle.Fill;
			taxonomyIdButton.Location = new System.Drawing.Point(1150, 0);
			taxonomyIdButton.Margin = new Padding(0);
			taxonomyIdButton.Name = "taxonomyIdButton";
			taxonomyIdButton.Size = new System.Drawing.Size(220, 50);
			taxonomyIdButton.TabIndex = 6;
			taxonomyIdButton.Text = "Taxonomy ID";
			taxonomyIdButton.UseVisualStyleBackColor = true;
			if (hasVariationData) {
				// 
				// variationRuleButton
				// 
				variationRuleButton.Dock = DockStyle.Fill;
				variationRuleButton.Location = new System.Drawing.Point(1150, 0);
				variationRuleButton.Margin = new Padding(0);
				variationRuleButton.Name = "variationRuleButton";
				variationRuleButton.Size = new System.Drawing.Size(220, 50);
				variationRuleButton.TabIndex = 6;
				variationRuleButton.Text = "Variation rule";
				variationRuleButton.UseVisualStyleBackColor = true;
			}
			if (hasModifications) {
				// 
				// modificationRuleButton
				// 
				modificationRuleButton.Dock = DockStyle.Fill;
				modificationRuleButton.Location = new System.Drawing.Point(1150, 0);
				modificationRuleButton.Margin = new Padding(0);
				modificationRuleButton.Name = "modificationRuleButton";
				modificationRuleButton.Size = new System.Drawing.Size(220, 50);
				modificationRuleButton.TabIndex = 6;
				modificationRuleButton.Text = "Modification rule";
				modificationRuleButton.UseVisualStyleBackColor = true;
			}
			tableLayoutPanel2.ResumeLayout(false);
		}

		private void TestButtonOnClick(object sender, EventArgs eventArgs) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length != 1) {
				MessageBox.Show("Please select exactly one row.");
				return;
			}
			int ind = sel[0];
			string path = (string) table.GetEntry(ind, 0);
			string identifierRule = (string) table.GetEntry(ind, 1);
			string descriptionRule = (string) table.GetEntry(ind, 2);
			string taxonomyRule = (string) table.GetEntry(ind, 3);
			TestParseRulesForm f = new TestParseRulesForm(path, identifierRule, descriptionRule, taxonomyRule);
			f.ShowDialog();
		}

		private void TaxonomyIdButtonOnClick(object sender, EventArgs eventArgs) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0) {
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			EditTaxonomyForm f = new EditTaxonomyForm();
			f.ShowDialog();
			if (f.DialogResult == DialogResult.OK) {
				int colInd = table.GetColumnIndex("Taxonomy ID");
				int colInd2 = table.GetColumnIndex("Organism");
				string org = "";
				if (TaxonomyItems.taxId2Item.ContainsKey(f.Id)) {
					org = TaxonomyItems.taxId2Item[f.Id].GetScientificName();
				}
				foreach (int i in sel) {
					table.SetEntry(i, colInd, "" + f.Id);
					table.SetEntry(i, colInd2, org);
				}
				tableView1.Invalidate(true);
			}
		}

		private string GetMostFrequentValue(int colInd) {
			Dictionary<string, int> counts = new Dictionary<string, int>();
			for (int i = 0; i < table.RowCount; i++) {
				string s = (string) table.GetEntry(i, colInd);
				if (string.IsNullOrEmpty(s)) {
					continue;
				}
				if (!counts.ContainsKey(s)) {
					counts.Add(s, 0);
				}
				counts[s]++;
			}
			int max = -1;
			string maxVal = "";
			foreach (KeyValuePair<string, int> pair in counts) {
				if (pair.Value > max) {
					max = pair.Value;
					maxVal = pair.Key;
				}
			}
			return maxVal;
		}

		private DataTable2 CreateTable() {
			table = new DataTable2("fasta file table");
			table.AddColumn("Fasta file path", 285, ColumnType.Text, "Path to the fasta file used in the Andromeda searches.");
			table.AddColumn("Identifier rule", 85, ColumnType.Text);
			table.AddColumn("Description rule", 100, ColumnType.Text);
			table.AddColumn("Taxonomy rule", 95, ColumnType.Text);
			table.AddColumn("Taxonomy ID", 90, ColumnType.Text);
			table.AddColumn("Organism", 90, ColumnType.Text);
			if (hasVariationData) {
				table.AddColumn("Variation rule", 95, ColumnType.Text);
			}
			if (hasModifications) {
				table.AddColumn("Modification rule", 95, ColumnType.Text);
			}
			return table;
		}

		private void AddButton_OnClick(object sender, EventArgs e) {
			OpenFileDialog ofd = new OpenFileDialog {Multiselect = true, Filter = Filter};
			if (ofd.ShowDialog() == DialogResult.OK) {
				AddFastaFiles(ofd.FileNames);
			}
		}

		private void AddFastaFiles(string[] fileNames) {
			HashSet<string> currentFiles = GetCurrentFiles();
			int count = 0;
			foreach (string fileName in fileNames) {
				if (!currentFiles.Contains(fileName)) {
					AddFastaFile(fileName);
					count++;
				}
			}
			if (count > 0) {
				tableView1.Invalidate(true);
			}
		}

		private void AddFastaFile(string filePath) {
			GuessRules(filePath, out string identifierRule, out string descriptionRule, out string taxonomyRule,
				out string taxonomyId, out string variationRule, out string modificationRule);
			AddFastaFile(filePath, identifierRule, descriptionRule, taxonomyRule, taxonomyId, variationRule, modificationRule);
		}

		private void GuessRules(string filePath, out string identifierRule, out string descriptionRule,
			out string taxonomyRule, out string taxonomyId, out string variationRule, out string modificationRule) {
			string fileName = Path.GetFileName(filePath);
			descriptionRule = @">(.*)";
			taxonomyRule = "";
			variationRule = "";
			modificationRule = "";
			if (LooksLikeUniprot(fileName)) {
				identifierRule = @">.*\|(.*)\|";
				taxonomyId = GetUniprotTaxonomyId(fileName);
				return;
			}
			identifierRule = @">.*\|(.*)\|";
			taxonomyId = "";
		}

		private string GetUniprotTaxonomyId(string fileName) {
			if (!fileName.ToUpper().EndsWith(".FASTA")) {
				return "";
			}
			fileName = fileName.Substring(0, fileName.Length - 6);
			if (fileName.ToLower().EndsWith("_additional")) {
				fileName = fileName.Substring(0, fileName.Length - 11);
			}
			int ind = fileName.IndexOf("_", StringComparison.InvariantCulture);
			if (ind < 0) {
				return "";
			}
			string s = fileName.Substring(ind + 1);
			bool ok = int.TryParse(s, out _);
			return !ok ? "" : s;
		}

		private static bool LooksLikeUniprot(string fileName) {
			if (!fileName.ToUpper().EndsWith(".FASTA")) {
				return false;
			}
			fileName = fileName.Substring(0, fileName.Length - 6);
			if (fileName.ToLower().EndsWith("_additional")) {
				fileName = fileName.Substring(0, fileName.Length - 11);
			}
			return fileName.ToUpper().StartsWith("UP") && fileName.Contains("_");
		}

		private void AddFastaFile(string fileName, string identifierRule, string descriptionRule, string taxonomyRule,
			string taxonomyId, string variationRule, string modificationRule) {
			DataRow2 row = table.NewRow();
			row["Fasta file path"] = fileName;
			row["Identifier rule"] = identifierRule;
			row["Description rule"] = descriptionRule;
			row["Taxonomy rule"] = taxonomyRule;
			row["Taxonomy ID"] = taxonomyId;
			row["Organism"] = "";
			bool success = int.TryParse(taxonomyId, out int taxId);
			if (success && TaxonomyItems.taxId2Item.ContainsKey(taxId)) {
				string n = TaxonomyItems.taxId2Item[taxId].GetScientificName();
				row["Organism"] = n;
			}
			if (hasVariationData) {
				row["Variation rule"] = variationRule;
			}
			if (hasModifications) {
				row["Modification rule"] = modificationRule;
			}
			table.AddRow(row);
		}

		private HashSet<string> GetCurrentFiles() {
			int col = table.GetColumnIndex("Fasta file path");
			HashSet<string> result = new HashSet<string>();
			for (int i = 0; i < table.RowCount; i++) {
				string s = (string) table.GetEntry(i, col);
				result.Add(s);
			}
			return result;
		}

		private void RemoveButton_OnClick(object sender, EventArgs e) {
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0) {
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			table.RemoveRows(sel);
			tableView1.Invalidate(true);
		}

		public string Filter { get; set; } = FileUtils.fastaFilter;

		public string[][] Value {
			get {
				string[][] result = new string[table.RowCount][];
				for (int i = 0; i < result.Length; i++) {
					result[i] = new string[7];
					for (int j = 0; j < 5; j++) {
						result[i][j] = (string) table.GetEntry(i, j);
					}
					if (hasVariationData) {
						result[i][5] = (string) table.GetEntry(i, "Variation rule");
					} else {
						result[i][5] = "";
					}
					if (hasModifications) {
						result[i][6] = (string) table.GetEntry(i, "Modification rule");
					} else {
						result[i][6] = "";
					}
				}
				return result;
			}
			set {
				table.Clear();
				foreach (string[] t in value) {
					if (t.Length >= 7) {
						AddFastaFile(t[0], t[1], t[2], t[3], t[4], t[5], t[6]);
					} else {
						AddFastaFile(t[0], "", "", "", "", "", "");
					}
				}
			}
		}
	}
}