using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLib.Query;
using BaseLibS.Drawing;
using BaseLibS.Graph;
using BaseLibS.Graph.Base;
using BaseLibS.Mol;
using BaseLibS.Table;
using BaseLibS.Util;

namespace BaseLib.Forms{
	public partial class FastaFilesParamControl : UserControl{
		private DataTable2 table;
		private readonly bool hasVariationData;
		private readonly bool hasModifications;
		private TableLayoutModel tableLayoutPanel2;
		private TableLayoutPanel tableLayoutPanel3;
		private ButtonModel addButton;
		private ButtonModel removeButton;
		private ButtonModel changeFolderButton;
		private ButtonModel identifierRuleButton;
		private ButtonModel descriptionRuleButton;
		private ButtonModel taxonomyRuleButton;
		private ButtonModel testButton;
		private ButtonModel taxonomyIdButton;
		private ButtonModel variationRuleButton;
		private ButtonModel modificationRuleButton;

		private readonly Func<TaxonomyItems> getTaxonomyItems;
		private TaxonomyItems TaxonomyItems => getTaxonomyItems();

		public FastaFilesParamControl(bool hasVariationData, bool hasModifications,
			Func<TaxonomyItems> getTaxonomyItems){
			this.getTaxonomyItems = getTaxonomyItems;
			this.hasVariationData = hasVariationData;
			this.hasModifications = hasModifications;
			InitializeComponent();
			InitializeComponent2();
			tableView1.TableModel = CreateTable();
			addButton.Click += AddButton_OnClick;
			removeButton.Click += RemoveButton_OnClick;
			changeFolderButton.Click += ChangeFolderButton_OnClick;
			identifierRuleButton.Click += (sender, args) => {
				ParseRuleButtonClick("Identifier",
					new[]{@">.*\|(.*)\|", @">(gi\|[0-9]*)", @">IPI:([^\| .]*)", @">(.*)", @">([^ ]*)", @">([^\t]*)"},
					new[]{
						"UniProt identifier", "NCBI accession", "IPI accession", "Everything after “>”",
						"Up to first space", "Up to first tab character"
					});
			};
			descriptionRuleButton.Click += (sender, args) => {
				ParseRuleButtonClick("Description", new string[0], new string[0]);
			};
			taxonomyRuleButton.Click += (sender, args) => {
				ParseRuleButtonClick("Taxonomy", new string[0], new string[0]);
			};
			taxonomyIdButton.Click += TaxonomyIdButtonOnClick;
			if (hasVariationData){
				variationRuleButton.Click += (sender, args) => {
					ParseRuleButtonClick("Variation", new[]{@">[^\s]+\s+(.+)"}, new[]{">ID r0:A1C;r1:D2E"});
				};
			}
			if (hasModifications){
				modificationRuleButton.Click += (sender, args) => {
					ParseRuleButtonClick("Modification", new string[0], new string[0]);
				};
			}
			testButton.Click += TestButtonOnClick;
			DragDrop += FormDragDrop;
			DragEnter += FormDragEnter;
		}

		private void FormDragDrop(object sender, DragEventArgs e){
			string[] sourceList = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
			List<string> selectedSourceList = new List<string>();
			string[] fastaExt = FileUtils.fastaFilter.Split('|')[1].Split(';');
			for (int i = 0; i < fastaExt.Length; i++){
				fastaExt[i] = fastaExt[i].TrimStart('*');
			}
			foreach (string source in sourceList){
				if (!File.Exists(source)) continue;
				string sourceExt = Path.GetExtension(source);
				bool flag = true;
				foreach (string ext in fastaExt){
					flag = flag && (sourceExt != ext);
				}
				if (!flag) selectedSourceList.Add(Path.GetFullPath(source));
			}
			if (selectedSourceList.Count == 0) return;
			AddFastaFiles(selectedSourceList.ToArray());
		}

		private static void FormDragEnter(object sender, DragEventArgs e){
			if (e.Data.GetDataPresent(DataFormats.FileDrop)){
				string[] fileList = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
				e.Effect = fileList.Length > 0 ? DragDropEffects.Copy : DragDropEffects.None;
			} else{
				e.Effect = DragDropEffects.None;
			}
		}

		private void ParseRuleButtonClick(string ruleName, string[] rules, string[] descriptions){
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0){
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			int colInd = table.GetColumnIndex(ruleName + " rule");
			EditParseRuleForm f =
				new EditParseRuleForm(ruleName.ToLower(), GetMostFrequentValue(colInd), rules, descriptions);
			f.ShowDialog();
			if (f.DialogResult == DialogResult.OK){
				foreach (int i in sel){
					table.SetEntry(i, colInd, f.ParseRule);
				}
				tableView1.Invalidate(true);
			}
		}

		private void InitializeComponent2(){
			AllowDrop = true;
			tableLayoutPanel2 = new TableLayoutModel();
			tableLayoutPanel3 = new TableLayoutPanel();
			addButton = new ButtonModel();
			removeButton = new ButtonModel();
			changeFolderButton = new ButtonModel();
			identifierRuleButton = new ButtonModel();
			descriptionRuleButton = new ButtonModel();
			taxonomyRuleButton = new ButtonModel();
			testButton = new ButtonModel();
			taxonomyIdButton = new ButtonModel();
			if (hasVariationData){
				variationRuleButton = new ButtonModel();
			}
			if (hasModifications){
				modificationRuleButton = new ButtonModel();
			}
			tableLayoutPanel3.SuspendLayout();
			tableLayoutPanel1.Controls.Add(BasicControl.CreateControl(tableLayoutPanel2), 0, 0);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
			// 
			// tableLayoutPanel2
			// 
			int nbuttons = 8;
			if (hasVariationData){
				nbuttons++;
			}
			if (hasModifications){
				nbuttons++;
			}
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, 100F));
			tableLayoutPanel3.ColumnCount = 2 * nbuttons;
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
			for (int i = 0; i < nbuttons - 1; i++){
				tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, 4F));
				tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Absolute, 84F));
				tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 4F));
				tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));
			}
			tableLayoutPanel2.ColumnStyles.Add(new BasicColumnStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			tableLayoutPanel2.Add(addButton , 0, 0);
			tableLayoutPanel2.Add(removeButton, 2, 0);
			tableLayoutPanel2.Add(changeFolderButton, 4, 0);
			tableLayoutPanel2.Add(identifierRuleButton, 6, 0);
			tableLayoutPanel2.Add(descriptionRuleButton, 8, 0);
			tableLayoutPanel2.Add(taxonomyRuleButton, 10, 0);
			tableLayoutPanel2.Add(taxonomyIdButton, 12, 0);
			int z = 0;
			if (hasVariationData){
				tableLayoutPanel3.Controls.Add(BasicControl.CreateControl(variationRuleButton), z, 0);
				z += 2;
			}
			if (hasModifications){
				tableLayoutPanel3.Controls.Add(BasicControl.CreateControl(modificationRuleButton), z, 0);
				z += 2;
			}
			tableLayoutPanel3.Controls.Add(BasicControl.CreateControl(testButton), z, 0);
			tableLayoutPanel2.Margin = new Padding2(0);
			tableLayoutPanel2.RowStyles.Add(new BasicRowStyle(BasicSizeType.Percent, 100F));
			tableLayoutPanel3.Dock = DockStyle.Fill;
			tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			tableLayoutPanel3.Margin = new Padding(0);
			tableLayoutPanel3.Name = "tableLayoutPanel3";
			tableLayoutPanel3.RowCount = 1;
			tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel3.Size = new System.Drawing.Size(2135, 50);
			tableLayoutPanel3.TabIndex = 3;
			addButton.Margin = new Padding2(0);
			addButton.Text = @"Add";
			removeButton.Margin = new Padding2(0);
			removeButton.Text = @"Remove";
			changeFolderButton.Margin = new Padding2(0);
			changeFolderButton.Text = @"Change folder";
			identifierRuleButton.Margin = new Padding2(0);
			identifierRuleButton.Text = @"Identifier rule";
			descriptionRuleButton.Margin = new Padding2(0);
			descriptionRuleButton.Text = @"Description rule";
			taxonomyRuleButton.Margin = new Padding2(0);
			taxonomyRuleButton.Text = @"Taxonomy rule";
			taxonomyIdButton.Margin = new Padding2(0);
			taxonomyIdButton.Text = @"Taxonomy ID";
			int j = 7;
			if (hasVariationData){
				variationRuleButton.Margin = new Padding2(0);
				variationRuleButton.Text = @"Variation rule";
				j++;
			}
			if (hasModifications){
				modificationRuleButton.Margin = new Padding2(0);
				modificationRuleButton.Text = @"Modification rule";
				j++;
			}
			testButton.Margin = new Padding2(0);
			testButton.Text = @"Test";
			tableLayoutPanel3.ResumeLayout(false);
		}

		private void TestButtonOnClick(object sender, EventArgs eventArgs){
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length != 1){
				MessageBox.Show(@"Please select exactly one row.");
				return;
			}
			int ind = sel[0];
			string path = (string) table.GetEntry(ind, "Fasta file path");
			string identifierRule = (string) table.GetEntry(ind, "Identifier rule");
			string descriptionRule = (string) table.GetEntry(ind, "Description rule");
			string taxonomyRule = (string) table.GetEntry(ind, "Taxonomy rule");
			TestParseRulesForm f = new TestParseRulesForm(path, identifierRule, descriptionRule, taxonomyRule,
				hasVariationData ? (string) table.GetEntry(ind, "Variation rule") : null,
				hasModifications ? (string) table.GetEntry(ind, "Modification rule") : null);
			f.ShowDialog();
		}

		private void TaxonomyIdButtonOnClick(object sender, EventArgs eventArgs){
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0){
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			EditTaxonomyForm f = new EditTaxonomyForm(getTaxonomyItems);
			f.ShowDialog();
			if (f.DialogResult == DialogResult.OK){
				int colInd = table.GetColumnIndex("Taxonomy ID");
				int colInd2 = table.GetColumnIndex("Organism");
				string org = "";
				if (TaxonomyItems.taxId2Item.ContainsKey(f.Id)){
					org = TaxonomyItems.taxId2Item[f.Id].GetScientificName();
				}
				foreach (int i in sel){
					table.SetEntry(i, colInd, "" + f.Id);
					table.SetEntry(i, colInd2, org);
				}
				tableView1.Invalidate(true);
			}
		}

		private string GetMostFrequentValue(int colInd){
			Dictionary<string, int> counts = new Dictionary<string, int>();
			for (int i = 0; i < table.RowCount; i++){
				string s = (string) table.GetEntry(i, colInd);
				if (string.IsNullOrEmpty(s)){
					continue;
				}
				if (!counts.ContainsKey(s)){
					counts.Add(s, 0);
				}
				counts[s]++;
			}
			int max = -1;
			string maxVal = "";
			foreach (KeyValuePair<string, int> pair in counts){
				if (pair.Value > max){
					max = pair.Value;
					maxVal = pair.Key;
				}
			}
			return maxVal;
		}

		private DataTable2 CreateTable(){
			table = new DataTable2("fasta file table");
			table.AddColumn("Fasta file path", 250, ColumnType.Text,
				"Path to the fasta file used in the Andromeda searches.");
			table.AddColumn("Exists", 50, ColumnType.Text);
			table.AddColumn("Identifier rule", 100, ColumnType.Text);
			table.AddColumn("Description rule", 100, ColumnType.Text);
			table.AddColumn("Taxonomy rule", 100, ColumnType.Text);
			table.AddColumn("Taxonomy ID", 100, ColumnType.Text);
			table.AddColumn("Organism", 100, ColumnType.Text);
			if (hasVariationData){
				table.AddColumn("Variation rule", 100, ColumnType.Text);
			}
			if (hasModifications){
				table.AddColumn("Modification rule", 100, ColumnType.Text);
			}
			return table;
		}

		private void ChangeFolderButton_OnClick(object sender, EventArgs e){
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0){
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			FolderQueryForm fqw =
				new FolderQueryForm(Path.GetDirectoryName((string) table.GetRow(sel[0])["Fasta file path"]));
			if (!Directory.Exists(fqw.Value)) return;
			if (fqw.ShowDialog() == DialogResult.OK){
				foreach (int i in sel){
					DataRow2 row = table.GetRow(i);
					string name = (string) row["Fasta file path"];
					string newFile = Path.Combine(Path.GetFullPath(fqw.Value), Path.GetFileName(name));
					row["Fasta file path"] = newFile;
					row["Exists"] = File.Exists(newFile).ToString();
				}
			}
			tableView1.Invalidate(true);
		}

		private void AddButton_OnClick(object sender, EventArgs e){
			OpenFileDialog ofd = new OpenFileDialog{Multiselect = true, Filter = Filter};
			if (ofd.ShowDialog() == DialogResult.OK){
				AddFastaFiles(ofd.FileNames);
			}
		}

		private void AddFastaFiles(string[] fileNames){
			HashSet<string> currentFiles = GetCurrentFiles();
			int count = 0;
			foreach (string fileName in fileNames){
				if (!currentFiles.Contains(fileName)){
					AddFastaFile(fileName);
					count++;
				}
			}
			if (count > 0){
				tableView1.Invalidate(true);
			}
		}

		private void AddFastaFile(string filePath){
			GuessRules(filePath, out string identifierRule, out string descriptionRule, out string taxonomyRule,
				out string taxonomyId, out string variationRule, out string modificationRule);
			AddFastaFile(filePath, identifierRule, descriptionRule, taxonomyRule, taxonomyId, variationRule,
				modificationRule);
		}

		private void GuessRules(string filePath, out string identifierRule, out string descriptionRule,
			out string taxonomyRule, out string taxonomyId, out string variationRule, out string modificationRule){
			string fileName = Path.GetFileName(filePath);
			descriptionRule = @">(.*)";
			taxonomyRule = "";
			variationRule = "";
			modificationRule = "";
			if (LooksLikeUniprot(fileName)){
				identifierRule = @">.*\|(.*)\|";
				taxonomyId = GetUniprotTaxonomyId(fileName);
				return;
			}
			identifierRule = @">([^\s]*)";
			taxonomyId = "";
		}

		private string GetUniprotTaxonomyId(string fileName){
			if (!fileName.ToUpper().EndsWith(".FASTA")){
				return "";
			}
			fileName = fileName.Substring(0, fileName.Length - 6);
			if (fileName.ToLower().EndsWith("_additional")){
				fileName = fileName.Substring(0, fileName.Length - 11);
			}
			int ind = fileName.IndexOf("_", StringComparison.InvariantCulture);
			if (ind < 0){
				return "";
			}
			string s = fileName.Substring(ind + 1);
			bool ok = int.TryParse(s, out _);
			return !ok ? "" : s;
		}

		private static bool LooksLikeUniprot(string fileName){
			if (!fileName.ToUpper().EndsWith(".FASTA")){
				return false;
			}
			fileName = fileName.Substring(0, fileName.Length - 6);
			if (fileName.ToLower().Contains("uniprot")){
				return true;
			}
			if (fileName.ToLower().EndsWith("_additional")){
				fileName = fileName.Substring(0, fileName.Length - 11);
			}
			return fileName.ToUpper().StartsWith("UP") && fileName.Contains("_");
		}

		private void AddFastaFile(string fileName, string identifierRule, string descriptionRule, string taxonomyRule,
			string taxonomyId, string variationRule, string modificationRule){
			DataRow2 row = table.NewRow();
			row["Fasta file path"] = fileName;
			row["Exists"] = File.Exists(fileName).ToString();
			row["Identifier rule"] = identifierRule;
			row["Description rule"] = descriptionRule;
			row["Taxonomy rule"] = taxonomyRule;
			row["Taxonomy ID"] = taxonomyId;
			row["Organism"] = "";
			bool success = int.TryParse(taxonomyId, out int taxId);
			if (success && TaxonomyItems.taxId2Item.ContainsKey(taxId)){
				string n = TaxonomyItems.taxId2Item[taxId].GetScientificName();
				row["Organism"] = n;
			}
			if (hasVariationData){
				row["Variation rule"] = variationRule;
			}
			if (hasModifications){
				row["Modification rule"] = modificationRule;
			}
			table.AddRow(row);
		}

		private HashSet<string> GetCurrentFiles(){
			int col = table.GetColumnIndex("Fasta file path");
			HashSet<string> result = new HashSet<string>();
			for (int i = 0; i < table.RowCount; i++){
				string s = (string) table.GetEntry(i, col);
				result.Add(s);
			}
			return result;
		}

		private void RemoveButton_OnClick(object sender, EventArgs e){
			int[] sel = tableView1.GetSelectedRows();
			if (sel.Length == 0){
				MessageBox.Show(Loc.PleaseSelectSomeRows);
				return;
			}
			table.RemoveRows(sel);
			tableView1.Invalidate(true);
		}

		public string Filter{ get; set; } = FileUtils.fastaFilter;

		public string[][] Value{
			get{
				string[][] result = new string[table.RowCount][];
				for (int i = 0; i < result.Length; i++){
					result[i] = new[]{
						(string) table.GetEntry(i, "Fasta file path"), (string) table.GetEntry(i, "Identifier rule"),
						(string) table.GetEntry(i, "Description rule"), (string) table.GetEntry(i, "Taxonomy rule"),
						(string) table.GetEntry(i, "Taxonomy ID"),
						hasVariationData ? (string) table.GetEntry(i, "Variation rule") : "",
						hasModifications ? (string) table.GetEntry(i, "Modification rule") : ""
					};
				}
				return result;
			}
			set{
				table.Clear();
				foreach (string[] t in value){
					if (t.Length >= 7){
						AddFastaFile(t[0], t[1], t[2], t[3], t[4], t[5], t[6]);
					} else{
						AddFastaFile(t[0], "", "", "", "", "", "");
					}
				}
			}
		}
	}
}